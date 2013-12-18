using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class UserControls_duyuru_NoticeList : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HiddenField1.Value = EnrollContext.Current.WorkingLanguage.LanguageId.ToString();
            HiddenField2.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000").ToString();

            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) Page.Title = site.title;
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
                        if (Request.Url.AbsolutePath != "/Notices.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../duyurular-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../duyurular-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/Notices.aspx?", "/duyurular-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("noticelistpage=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", ""); 
                    }
                }
            }
        }
    }

    protected void HyperLink1_DataBinding(object sender, EventArgs e)
    {
        var myHyper = (HyperLink) sender;
        int noticeId = Convert.ToInt32(myHyper.NavigateUrl);
        Notices duyuru = ent.Notices.First(p => p.noticeId == noticeId);
        myHyper.NavigateUrl = "~/duyurudetay-" + noticeId + "-" + UrlMapping.cevir(duyuru.header);
    }

    protected void HyperLink2_DataBinding(object sender, EventArgs e)
    {
        var myHyper = (HyperLink) sender;
        int noticeId = Convert.ToInt32(myHyper.NavigateUrl);
        Notices duyuru = ent.Notices.First(p => p.noticeId == noticeId);
        myHyper.NavigateUrl = "~/duyurudetay-" + noticeId + "-" + UrlMapping.cevir(duyuru.header);
        myHyper.Text = Resource.details;
    }
}