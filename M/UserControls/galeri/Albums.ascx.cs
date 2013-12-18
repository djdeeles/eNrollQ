using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

public partial class M_Albums : UserControl
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
        Def_photoAlbumCategory category = null;
        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        if (!String.IsNullOrEmpty(Request.QueryString["categoryid"]))
        {  
            if (IsPostBack)
            {
                #region AlbumId yi session dan alıyoruz
                // sayfa postback olmuşsa yeni yüklenirken AlbumId değerini Session nesnesinden alırız
                if (Session["listItemshfCategoryId"] != null && !string.IsNullOrWhiteSpace(Session["listItemshfCategoryId"].ToString()))
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
                    CategoryId = Convert.ToInt32(Request.QueryString["categoryid"]);
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
            category = ent.Def_photoAlbumCategory.FirstOrDefault(p => p.photoAlbumCategoryId == CategoryId);
            if (category != null)
            {
                lblCategoryName.Text = category.categoryName;
                lblCategoryNote.Text = category.categoryNotes;
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

            EntityDataSource1.Where = string.Format("it.languageId={0} and it.photoAlbumCategoryId={1} and it.State=True", lang, CategoryId);
            ListView1.DataBind();

            Session["listItemshfCategoryId"] = CategoryId;

            if (Session["listItemPageIndex"] == null || string.IsNullOrWhiteSpace(Session["listItemPageIndex"].ToString()))
            {
                Session["listItemPageIndex"] = 1;
            }
        }
        if (!String.IsNullOrEmpty(Request.QueryString["categoryid"]))
        {
            var site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (category != null && site != null)
            {
                Page.Title = site.title + " - " + category.categoryName;
                MetaGenerate.SetMetaTags(site, Page);
            }
        }
    }

    protected void DataPager1_Init(object sender, EventArgs e)
    {
        _localizations.ChangeDataPager((DataPager) sender);
    }

    protected void imgAlbum_DataBinding(object sender, EventArgs e)
    {
        try
        {
            var Resim = (Image) sender;
            int photoAlbumId = Convert.ToInt32(Resim.ImageUrl);
            PhotoAlbum PhotoAlbum = ent.PhotoAlbum.Where(p => p.photoAlbumId == photoAlbumId).FirstOrDefault();
            Resim.ImageUrl = "/" + PhotoAlbum.thumbnailPath.Replace("~/", "");
        }
        catch
        {
            //
        }
    }

    protected void HyperLink1_DataBinding(object sender, EventArgs e)
    {
        var myHyper = (HyperLink) sender;
        try
        {
            int photoAlbumId = Convert.ToInt32(myHyper.NavigateUrl);
            PhotoAlbum PhotoAlbum = ent.PhotoAlbum.Where(p => p.photoAlbumId == photoAlbumId).First();
            myHyper.NavigateUrl = "/m/albumdetay-" + PhotoAlbum.photoAlbumId.ToString() + "-1";
            myHyper.ToolTip = PhotoAlbum.photoName + PhotoAlbum.photoNote;
        }
        catch
        {
            //
        }
        myHyper.Text = Resource.lbAllPhotos;
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
                        if (Request.Url.AbsolutePath != "/m/Albums.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../m/albumler-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../albumler-"+CategoryId);
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/m/Albums.aspx?",
                                                                                      "/m/albumler-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&albumpage=", "-");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("categoryid=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                    }
                }
            }
        }
    }
}