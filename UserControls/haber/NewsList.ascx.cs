using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class UserControls_haber_NewsList : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();
    private string Sayfa = string.Empty;

    private bool _manset;
    public string s = string.Empty;

    public bool Manset
    {
        get { return _manset; }
        set { _manset = value; }
    }

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

        if (_manset)
        {
            EntityDataSource1.Where = "it.manset=false and it.state=true and it.languageId=@languageId";
        }
        else
        {
            EntityDataSource1.Where = "it.state=true and it.languageId=@languageId";
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
                        if (Request.Url.AbsolutePath != "/News.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../haberler-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../haberler-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/News.aspx?", "/haberler-");
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