using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class UserControls_haber_News : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();
    private string Sayfa = string.Empty;

    public string s = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HiddenField1.Value = EnrollContext.Current.WorkingLanguage.LanguageId.ToString();
        }

        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title;
        MetaGenerate.SetMetaTags(site, Page);
    }

    protected void DataPager1_OnLoad(object sender, EventArgs e)
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
                        if (Request.Url.AbsolutePath != "/m/News.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../m/haberler-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../haberler-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/m/News.aspx?", "/m/haberler-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("newslistpage=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                    }
                }
            }
        }
    }

    protected void HyperLink1_DataBinding(object sender, EventArgs e)
    {
        var myHyper = (HyperLink) sender;
        int newsId = Convert.ToInt32(myHyper.NavigateUrl);
        News haber = ent.News.Where(p => p.newsId == newsId).First();
        myHyper.NavigateUrl = "../../haberdetay-" + newsId + "-" + UrlMapping.cevir(haber.header);
    }

    protected void HyperLink2_DataBinding(object sender, EventArgs e)
    {
        var myHyper = (HyperLink) sender;
        int newsId = Convert.ToInt32(myHyper.NavigateUrl);
        News haber = ent.News.Where(p => p.newsId == newsId).First();
        myHyper.NavigateUrl = "../../haberdetay-" + newsId + "-" + UrlMapping.cevir(haber.header);

        myHyper.Text = Resource.details;
    }
}