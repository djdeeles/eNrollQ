using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class M_userControls_VideoGalleryMain : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            HiddenField1.Value = lang.ToString();
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) Page.Title = site.title + Resource.titleVideoGalleryMain;
            MetaGenerate.SetMetaTags(site, Page);
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
                        if (Request.Url.AbsolutePath != "/m/VideoGalleryMain.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../m/videogaleri-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../videogaleri-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/m/VideoGalleryMain.aspx?",
                                                                                      "/m/videogaleri-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("videogallerypage=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                    }
                }
            }
        }
    }
}