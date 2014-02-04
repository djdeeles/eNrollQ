using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using eNroll.App_Data;

public partial class uye_userControls_photoAlbum : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();

    private int _albumId = -1;

    public int AlbumId
    {
        get { return _albumId; }
        set { _albumId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PhotoAlbum album = null;
        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        if (!String.IsNullOrEmpty(Request.QueryString["albumid"]))
        {
            if (IsPostBack)
            {
                #region AlbumId yi session dan alıyoruz

                // sayfa postback olmuşsa yeni yüklenirken AlbumId değerini Session nesnesinden alırız
                if (Session["listItemshfAlbumId"] != null &&
                    !string.IsNullOrWhiteSpace(Session["listItemshfAlbumId"].ToString()))
                {
                    AlbumId = Convert.ToInt32(Session["listItemshfAlbumId"]);
                    HiddenField1.Value = Session["listItemshfAlbumId"].ToString();
                }

                #endregion
            }
            else
            {
                #region query string ten varsa albumid değerini alıyoruz

                try
                {
                    AlbumId = Convert.ToInt32(Request.QueryString["albumid"]);
                    Session["listItemshfAlbumId"] = AlbumId;
                    HiddenField1.Value = AlbumId.ToString();
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
            album = ent.PhotoAlbum.FirstOrDefault(p => p.photoAlbumId == AlbumId);
            if (album != null)
            {
                lblAlbumName.Text = album.Def_photoAlbum.albumName;
                lblAlbumNote.Text = album.Def_photoAlbum.albumNote;
            }

            #region query string ten sayfa indexini alıp session da tutuyoruz

            if (!String.IsNullOrEmpty(Request.QueryString["albumpage"]))
            {
                try
                {
                    Session["listItemPageIndex"] = Request.QueryString["albumpage"];
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }

            #endregion

            EntityDataSource1.Where = string.Format("it.photoAlbumId={0}", AlbumId);
            ListView1.DataBind();

            Session["listItemshfAlbumId"] = AlbumId;

            if (Session["listItemPageIndex"] == null ||
                string.IsNullOrWhiteSpace(Session["listItemPageIndex"].ToString()))
            {
                Session["listItemPageIndex"] = 1;
            }
        }
        if (!String.IsNullOrEmpty(Request.QueryString["albumid"]))
        {
            var site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (album != null && site != null)
            {
                Page.Title = site.title + " - " + album.Def_photoAlbum.albumName;
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
                        if (Request.Url.AbsolutePath != "/AlbumPhotos.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../albumdetay-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../albumdetay-" + AlbumId);
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/AlbumPhotos.aspx?",
                                                                                      "/albumdetay-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&photoalbumpage=", "-");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("albumid=", "");
                    }
                }
            }
        }
    }

    protected void HyperLink1_DataBinding(object sender, EventArgs e)
    {
        var myHyper = (HyperLink) sender;
        int photoId = Convert.ToInt32(myHyper.NavigateUrl);
        PhotoAlbum PhotoAlbum = ent.PhotoAlbum.Where(p => p.photoId == photoId).First();
        myHyper.NavigateUrl = "/" + PhotoAlbum.photoPath.Replace("~/", "");
        myHyper.ToolTip = PhotoAlbum.photoName + PhotoAlbum.photoNote;
    }

    protected void Image1_DataBinding(object sender, EventArgs e)
    {
        var Resim = (Image) sender;
        int photoId = Convert.ToInt32(Resim.ImageUrl);
        PhotoAlbum PhotoAlbum = ent.PhotoAlbum.Where(p => p.photoId == photoId).First();
        Resim.ImageUrl = "/" + PhotoAlbum.thumbnailPath.Replace("~/", "");
    }
}