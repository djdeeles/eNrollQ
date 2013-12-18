using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class M_UserControls_PhotoGalleryMain : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HiddenField1.Value = EnrollContext.Current.WorkingLanguage.LanguageId.ToString();
        }
        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title + " - " + Resource.titlePhotoGalleryMain;
        MetaGenerate.SetMetaTags(site, Page);
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
                        if (Request.Url.AbsolutePath != "/m/PhotoGalleryMain.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../m/galeri-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../galeri-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/m/PhotoGalleryMain.aspx?",
                                                                                      "/m/galeri-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("photogaleripage=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                    }
                }
            }
        }
    }

    protected void imgAlbum_DataBinding(object sender, EventArgs e)
    {
        try
        {
            var Resim = (Image) sender;
            int categoryId = Convert.ToInt32(Resim.ImageUrl);
            Def_photoAlbum defPhotoAlbum =
                ent.Def_photoAlbum.Where(p => p.photoAlbumCategoryId == categoryId).FirstOrDefault();
            PhotoAlbum PhotoAlbum =
                ent.PhotoAlbum.Where(p => p.photoAlbumId == defPhotoAlbum.photoAlbumId).FirstOrDefault();
            Resim.ImageUrl = PhotoAlbum.thumbnailPath;
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
            int categoryId = Convert.ToInt32(myHyper.NavigateUrl);
            Def_photoAlbum defPhotoAlbum =
                ent.Def_photoAlbum.Where(p => p.photoAlbumCategoryId == categoryId).FirstOrDefault();
            PhotoAlbum PhotoAlbum =
                ent.PhotoAlbum.Where(p => p.photoAlbumId == defPhotoAlbum.photoAlbumId).FirstOrDefault();
            myHyper.NavigateUrl = "/m/albumler-" + categoryId + "-1";
            myHyper.ToolTip = PhotoAlbum.photoName + PhotoAlbum.photoNote;
        }
        catch
        {
            //
        }
        myHyper.Text = Resource.lbAllGalleries;
    }
}