using System;
using System.Linq;
using System.Web.UI;
using Enroll.WebParts;
using Resources;
using eNroll.App_Data;

public partial class UserControls_haber_MansetScroller : UserControl
{
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        var mansetHaberler =
            ent.News.Where(p => p.manset == true && p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId)
                .OrderByDescending(p => p.enterDate)
                .Take(20)
                .ToList();

        int counter = 0;
        pnlPhoto.Controls.Add(new LiteralControl("<div class='mansethaber'>"));
        pnlTabs.Controls.Add(new LiteralControl("<div class='slidetabs'>"));
        foreach (News mansetHaber in mansetHaberler)
        {
            counter++;

            string imgUrl = string.Empty;
            if (!String.IsNullOrEmpty(mansetHaber.thumbnailPath)) imgUrl = mansetHaber.thumbnailPath.Replace("~", "");
            else imgUrl = "/App_Themes/mainTheme/images/noimage.png";

            pnlPhoto.Controls.Add(new LiteralControl(
                                      string.Format(
                                          "<div><span class='mansetgorsel'><img src='{0}' alt='{1}' /></span>",
                                          imgUrl, UrlMapping.AltCevir(mansetHaber.header)) +
                                      string.Format(
                                          "<span class='mansetheader'>{0}</span><span class='mansetdate'>{1}</span>",
                                          mansetHaber.header, mansetHaber.enterDate.Value.ToShortDateString()) +
                                      string.Format("<p class='mansetsummery'>{0}",
                                                    HtmlRemoval.StripTagsRegexCompiled(mansetHaber.brief)) +
                                      string.Format(
                                          "<a class='mansetdevam' href='{0}://{1}/haberdetay-{2}-{3}'>{4}</a>",
                                          Request.Url.Scheme, Request.Url.Host, mansetHaber.newsId,
                                          UrlMapping.cevir(mansetHaber.header), Resource.details)));

            pnlPhoto.Controls.Add(new LiteralControl("</p></div>"));
            pnlTabs.Controls.Add(new LiteralControl("<a href='#'></a>"));
        }
        pnlPhoto.Controls.Add(new LiteralControl("</div>"));
        pnlTabs.Controls.Add(new LiteralControl("</div>"));
    }
}