using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using Resources;
using eNroll.App_Data;

public partial class UserControls_NoticeScroller : UserControl
{
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        int dilId = EnrollContext.Current.WorkingLanguage.LanguageId;
        DateTime tarih = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000");
        IOrderedQueryable<Notices> duyurular =
            ent.Notices.Where(
                p =>
                p.state == true && p.languageId == dilId && p.startDate <= tarih &&
                (p.stopDate >= tarih || p.stopDate == null)).OrderByDescending(p => p.startDate);

        var txt = new StringBuilder();
        var govde = new StringBuilder();
        foreach (Notices item in duyurular)
        {
            txt.AppendLine("<li><div class='duyuruscroller'>");
	    if (item.imagePath != "")
            {
                txt.AppendFormat("<div class='duyuruimg'><img src='{0}' alt='{1}' /></div>",
                                 item.thumbnailPath.Replace("~", ""), UrlMapping.AltCevir(item.header));
            }
            txt.AppendFormat("<div class='duyurubaslik'><a href='/duyurudetay-{1}-{2}'>{0}</a></div>",
                             item.header, item.noticeId, UrlMapping.cevir(item.header));
            txt.AppendFormat("<div class='duyuruozet'>{0}</div></li>", item.description);
        }

        SiteGeneralInfo siteGeneralInfo = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == dilId);
        if (siteGeneralInfo != null)
        {
            string duyuruBaslik = Resource.lbnoticeHeader;

            govde.AppendFormat("<div class=\"duyuruTepe\">{0}</div>" +
                               "<div class=\"duyuruGovde\">" +
                               "<div id=\"duyuruscrollerdiv\">" +
                               "<ul id=\"duyuru\" class=\"duyuruskin\">{1}</ul>" +
                               "</div>" +
                               "</div>" +
                               "<div class=\"duyuruAlt\"><div class='duyurudevam'>" +
                               "<a href='/duyurular-1' >{2}</a></div></div>", duyuruBaslik, txt, Resource.lbAllNotices);
        }
        Literal1.Text = govde.ToString();
    }
}