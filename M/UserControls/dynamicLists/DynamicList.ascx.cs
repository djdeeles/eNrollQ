using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

public partial class M_UserControls_DynamicList : UserControl
{
    private readonly Entities _entities = new Entities();
    private readonly Localizations _localizations = new Localizations();
    protected string ListTitle = null;

    private int _listId = -1;

    public int ListId
    {
        get { return _listId; }
        set { _listId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack)
            {
                #region listId yi session dan alıyoruz

                // sayfa postback olmuşsa yeni yüklenirken ListId değerini Session nesnesinden alırız
                if (Session["listItemshfListId"] != null &&
                    !string.IsNullOrWhiteSpace(Session["listItemshfListId"].ToString()))
                    ListId = Convert.ToInt32(Session["listItemshfListId"]);

                #endregion
            }
            else
            {
                #region query string ten varsa listId değerini alıyoruz

                if (!String.IsNullOrEmpty(Request.QueryString["listId"]))
                {
                    try
                    {
                        ListId = Convert.ToInt32(Request.QueryString["listId"]);
                        Session["listItemshfListId"] = ListId;
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.ManageException(exception);
                    }
                }

                #endregion

                #region query string ten sayfa indexini alıp session da tutuyoruz

                if (!String.IsNullOrEmpty(Request.QueryString["listpage"]))
                {
                    try
                    {
                        Session["listItemPageIndex"] = Request.QueryString["listpage"];
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.ManageException(exception);
                    }
                }

                #endregion
            }

            #region order by default değer

            //  sıralama fonksiyonu için default değeri belirledik, 
            //  order by desc için sıralama yapılacak
            if (Session["OrderByAscDesc"] == null ||
                string.IsNullOrWhiteSpace(Session["OrderByAscDesc"].ToString()))
            {
                Session["OrderByAscDesc"] = string.Format(" desc");
            }

            #endregion

            EntityDataSource1.Where = string.Format("it.State=true and it.ListId={0}", ListId);
            OrderBy();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }


        Session["listItemshfListId"] = ListId;

        if (Session["listItemPageIndex"] == null || string.IsNullOrWhiteSpace(Session["listItemPageIndex"].ToString()))
        {
            Session["listItemPageIndex"] = 1;
        }


        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = _entities.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title + " - " + _entities.Lists.First(p => p.Id == ListId).Name;
        MetaGenerate.SetMetaTags(site, Page);
    }

    protected void HyperLink1_DataBinding(object sender, EventArgs e)
    {
        var myHyper = (HyperLink) sender;
        int listDataId = Convert.ToInt32(myHyper.NavigateUrl);
        ListData listData = _entities.ListData.First(p => p.Id == listDataId);
        myHyper.NavigateUrl = "../../listedetay-" + listDataId + "-" + UrlMapping.cevir(listData.Title);
    }

    protected void BtOrderbyTitleClick(object sender, EventArgs e)
    {
        Session["OrderByColumn"] = string.Format("it.Title");
        OrderBy();
    }

    protected void BtOrderbyUpdatedTimeClick(object sender, EventArgs e)
    {
        Session["OrderByColumn"] = string.Format("it.UpdatedTime");
        OrderBy();
    }

    protected void BtOrderbyAscDescClick(object sender, EventArgs e)
    {
        if (Session["OrderByAscDesc"] != null && !string.IsNullOrWhiteSpace(Session["OrderByAscDesc"].ToString()))
        {
            if (Session["OrderByAscDesc"].ToString() == " asc")
            {
                Session["OrderByAscDesc"] = " desc";
            }
            else
            {
                Session["OrderByAscDesc"] = " asc";
            }
        }
        OrderBy();
    }

    public void OrderBy()
    {
        if (Session["OrderByColumn"] == null ||
            string.IsNullOrWhiteSpace(Session["OrderByColumn"].ToString()))
        {
            Session["OrderByColumn"] = string.Format("it.UpdatedTime");
        }

        SetOrderByButtonText();
        SetActiveButtonCssClass();

        EntityDataSource1.OrderBy = string.Format(Session["OrderByColumn"] + " " + Session["OrderByAscDesc"]);
    }

    public void SetActiveButtonCssClass()
    {
        if (Session["OrderByColumn"].ToString() == "it.Title")
        {
            btOrderbyTitle.CssClass = "buttonactive";
            btOrderbyUpdatedTime.CssClass = "button";
        }
        else if (Session["OrderByColumn"].ToString() == "it.UpdatedTime")
        {
            btOrderbyTitle.CssClass = "button";
            btOrderbyUpdatedTime.CssClass = "buttonactive";
        }
    }

    public void SetOrderByButtonText()
    {
        if (Session["OrderByAscDesc"].ToString() == " asc")
        {
            btOrderbyAscDesc.Text = Resource.lbAsc;
        }
        else
            btOrderbyAscDesc.Text = Resource.lbDesc;

        btOrderbyTitle.Text = Resource.lbByTitle;
        btOrderbyUpdatedTime.Text = Resource.lbByDate;
    }

    public bool IsOrderByAscSelected()
    {
        if (Session["OrderByAscDesc"].ToString() == string.Format(" asc"))
            return true;
        else
            return false;
    }

    #region data pager

    protected void DataPager1_Init(object sender, EventArgs e)
    {
        _localizations.ChangeDataPager((DataPager) sender);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataPager1.PreRender += DataPager1_PreRender;
    }

    private void DataPager1_PreRender(object sender, EventArgs e)
    {
        foreach (Control control in DataPager1.Controls)
        {
            foreach (Control c in control.Controls)
            {
                if (c is HyperLink)
                {
                    var currentLink = (HyperLink) c;
                    if ((!string.IsNullOrEmpty(Request.Url.AbsolutePath)) && (!string.IsNullOrEmpty(Request.Url.Query)))
                    {
                        if (Request.Url.AbsolutePath != "/m/Lists.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace( Request.Url.PathAndQuery + "&", "../../m/listeler-");
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("listpage=", "");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery, "../../listeler-" + ListId + "-");
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("listpage=", "");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/m/Lists.aspx?", "/m/listeler-");
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("listpage=", "-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("listId=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                    }
                }
            }
        }
    }

    #endregion
}