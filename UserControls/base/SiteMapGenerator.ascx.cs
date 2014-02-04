using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using eNroll.App_Data;

public partial class UserControls_base_SiteMapGenerator : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SiteMapVer();
    }

    private void SiteMapVer()
    {
        var ent = new Entities();
        string Host = "http://" + Request.Url.Host;
        var SB = new StringBuilder();
        SB.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        SB.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        #region SMMenuler

        var MList = ent.System_menu.Where(p => p.state == true && p.type == "2").ToList();
        foreach (System_menu menu in MList)
        {
            SB.AppendLine("<url>");
            SB.AppendLine("<loc>");
            SB.AppendLine(Host + "/sayfa-" + menu.menuId + "-" + UrlMapping.cevir(menu.name));
            SB.AppendLine("</loc>");
            SB.AppendLine("<changefreq>");
            SB.AppendLine("always");
            SB.AppendLine("</changefreq>");
            SB.AppendLine("<priority>");
            SB.AppendLine("1");
            SB.AppendLine("</priority>");
            SB.AppendLine("</url>");
        }

        #endregion

        #region SMHaberler

        var news = ent.News.Where(p => p.state == true).ToList();
        foreach (News n in news)
        {
            SB.AppendLine("<url>");
            SB.AppendLine("<loc>");
            SB.AppendLine(Host + "/haberdetay-" + n.newsId + "-" + UrlMapping.cevir(n.header));
            SB.AppendLine("</loc>");
            SB.AppendLine("<lastmod>");
            if (n.enterDate != null) SB.AppendLine(n.enterDate.Value.ToString("yyyy-MM-ddTHH':'mm':'sszzz"));
            SB.AppendLine("</lastmod>");
            SB.AppendLine("<changefreq>");
            SB.AppendLine("always");
            SB.AppendLine("</changefreq>");
            SB.AppendLine("<priority>");
            SB.AppendLine("0.5");
            SB.AppendLine("</priority>");
            SB.AppendLine("</url>");
        }

        #endregion

        #region SMDuyurular

        var noticeses = ent.Notices.Where(p => p.state == true).ToList();
        foreach (Notices notice in noticeses)
        {
            SB.AppendLine("<url>");
            SB.AppendLine("<loc>");
            SB.AppendLine(Host + "/duyurudetay-" + notice.noticeId + "-" + UrlMapping.cevir(notice.header));
            SB.AppendLine("</loc>");
            SB.AppendLine("<lastmod>");
            if (notice.startDate != null) SB.AppendLine(notice.startDate.Value.ToString("yyyy-MM-ddTHH':'mm':'sszzz"));
            SB.AppendLine("</lastmod>");
            SB.AppendLine("<changefreq>");
            SB.AppendLine("always");
            SB.AppendLine("</changefreq>");
            SB.AppendLine("<priority>");
            SB.AppendLine("0.5");
            SB.AppendLine("</priority>");
            SB.AppendLine("</url>");
        }

        #endregion

        #region SMEtkinlikler

        var eventses = ent.Events.Where(p => p.State == true).ToList();
        foreach (Events e in eventses)
        {
            SB.AppendLine("<url>");
            SB.AppendLine("<loc>");
            SB.AppendLine(Host + "/etkinlik-" + e.id + "-" + UrlMapping.cevir(e.Name));
            SB.AppendLine("</loc>");
            SB.AppendLine("<lastmod>");
            if (e.StartDate != null) SB.AppendLine(e.StartDate.Value.ToString("yyyy-MM-ddTHH':'mm':'sszzz"));
            SB.AppendLine("</lastmod>");
            SB.AppendLine("<changefreq>");
            SB.AppendLine("always");
            SB.AppendLine("</changefreq>");
            SB.AppendLine("<priority>");
            SB.AppendLine("0.5");
            SB.AppendLine("</priority>");
            SB.AppendLine("</url>");
        }

        #endregion

        #region SMFotoğrafGalerisi

        var albums = ent.Def_photoAlbum.Where(p => p.state == true).ToList();
        foreach (Def_photoAlbum album in albums)
        {
            SB.AppendLine("<url>");
            SB.AppendLine("<loc>");
            SB.AppendLine(Host + "/albumdetay-" + album.photoAlbumId + "-1");
            SB.AppendLine("</loc>");
            SB.AppendLine("<changefreq>");
            SB.AppendLine("always");
            SB.AppendLine("</changefreq>");
            SB.AppendLine("<priority>");
            SB.AppendLine("0.2");
            SB.AppendLine("</priority>");
            SB.AppendLine("</url>");
        }

        #endregion

        #region SMProducts

        var productses = ent.Products.Where(p => p.State == true).ToList();
        foreach (Products product in productses)
        {
            SB.AppendLine("<url>");
            SB.AppendLine("<loc>");
            SB.AppendLine(Host + "/urundetay-" + product.ProductId + "-" + UrlMapping.cevir(product.Name));
            SB.AppendLine("</loc>");
            SB.AppendLine("<changefreq>");
            SB.AppendLine("always");
            SB.AppendLine("</changefreq>");
            SB.AppendLine("<priority>");
            SB.AppendLine("0.7");
            SB.AppendLine("</priority>");
            SB.AppendLine("</url>");
        }

        #endregion

        #region SMListDatas

        var listDatas = ent.ListData.Where(p => p.State == true).ToList();
        foreach (ListData listData in listDatas)
        {
            SB.AppendLine("<url>");
            SB.AppendLine("<loc>");
            SB.AppendLine(Host + "/listedetay-" + listData.Id + "-" + UrlMapping.cevir(listData.Title));
            SB.AppendLine("</loc>");
            SB.AppendLine("<changefreq>");
            SB.AppendLine("always");
            SB.AppendLine("</changefreq>");
            SB.AppendLine("<priority>");
            SB.AppendLine("0.5");
            SB.AppendLine("</priority>");
            SB.AppendLine("</url>");
        }

        #endregion

        SB.AppendLine("</urlset>");
        Response.ContentType = "text/xml";
        Response.Write(SB.ToString());
        Response.End();
    }
}