using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using eNroll.App_Data;

public partial class M_AlbumVideos : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();

    private int _categoryId = -1;

    public int CategoryId
    {
        get { return _categoryId; }
        set { _categoryId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        VideoCategories category = null;
        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        if (!String.IsNullOrEmpty(Request.QueryString["videoalbumid"]))
        {
            if (IsPostBack)
            {
                #region AlbumId yi session dan alıyoruz

                // sayfa postback olmuşsa yeni yüklenirken AlbumId değerini Session nesnesinden alırız
                if (Session["listItemshfCategoryId"] != null &&
                    !string.IsNullOrWhiteSpace(Session["listItemshfCategoryId"].ToString()))
                {
                    CategoryId = Convert.ToInt32(Session["listItemshfCategoryId"]);
                    HiddenField1.Value = Session["listItemshfCategoryId"].ToString();
                }

                #endregion
            }
            else
            {
                #region query string ten varsa albumid değerini alıyoruz

                try
                {
                    CategoryId = Convert.ToInt32(Request.QueryString["videoalbumid"]);
                    Session["listItemshfCategoryId"] = CategoryId;
                    HiddenField1.Value = CategoryId.ToString();
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }

                #endregion
            }
        }

        if (!IsPostBack)
        {
            category = ent.VideoCategories.FirstOrDefault(p => p.id == CategoryId);
            if (category != null)
            {
                lblAlbumName.Text = category.name;
            }

            #region query string ten sayfa indexini alıp session da tutuyoruz

            if (!String.IsNullOrEmpty(Request.QueryString["photogaleripage"]))
            {
                try
                {
                    Session["listItemPageIndex"] = Request.QueryString["photogaleripage"];
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }

            #endregion

            EntityDataSource1.Where = string.Format("it.categoryId={0} and it.languageId={1} and it.State=True",
                                                    CategoryId, lang);
            ListView1.DataBind();

            Session["listItemshfCategoryId"] = CategoryId;

            if (Session["listItemPageIndex"] == null ||
                string.IsNullOrWhiteSpace(Session["listItemPageIndex"].ToString()))
            {
                Session["listItemPageIndex"] = 1;
            }
        }
        if (!String.IsNullOrEmpty(Request.QueryString["videoalbumid"]))
        {
            var site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (category != null && site != null)
            {
                Page.Title = site.title + " - " + category.name;
                MetaGenerate.SetMetaTags(site, Page);
            }
        }
    }

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
                        if (Request.Url.AbsolutePath != "/m/AlbumVideos.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../m/albumvideolari-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../albumvideolari-" +
                                                                                          CategoryId);
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/m/AlbumVideos.aspx?",
                                                                                      "/m/albumvideolari-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&videopage=", "-");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("videoalbumid=", "");
                    }
                }
            }
        }
    }
}