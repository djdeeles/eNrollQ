using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Enroll.Managers;

public class UrlMapping : IHttpModule
{
    #region IHttpModule Members

    public void Dispose()
    {
    }

    public void Init(HttpApplication context)
    {
        context.BeginRequest += context_BeginRequest;
    }

    #endregion

    private void context_BeginRequest(object sender, EventArgs e)
    {
        try
        {
            var newUrl = new StringBuilder();
            string path = HttpContext.Current.Request.RawUrl;
            var fileName = Path.GetFileName(path);


            if (fileName != null && !path.ToLower().Contains(string.Format("/filemanager")) &&
                !path.ToLower().Contains(string.Format("/admin")))
            {
                var parcalar = fileName.Split('-');

                #region Mobile

                if (path.Contains("/m/sayfa-r-"))
                {
                    if (parcalar.Count() > 3)
                        newUrl.AppendFormat("~/m/RandomField.aspx?id={0}&title={1}", parcalar[2], parcalar[3]);
                    else if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/m/RandomField.aspx?randomfieldspage={0}", parcalar[2]);
                    else
                        newUrl.AppendFormat("~/m/RandomField.aspx?randomfieldspage=1");

                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                if (path.Contains("/m/sayfa-"))
                {
                    newUrl.AppendFormat("~/m/MenuPlace.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/dinamik-"))
                {
                    newUrl.AppendFormat("~/m/DynamicContent.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/listeler-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/m/Lists.aspx?listId={0}&listpage={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/Lists.aspx?listId={0}&listpage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/listedetay-"))
                {
                    newUrl.AppendFormat("~/m/ListsDetails.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/haberler"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/News.aspx?newslistpage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/m/News.aspx?newslistpage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/haberdetay-"))
                {
                    newUrl.AppendFormat("~/m/NewsDetails.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/duyurular"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/Notices.aspx?noticelistpage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/m/Notices.aspx?noticelistpage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/duyurudetay-"))
                {
                    newUrl.AppendFormat("~/m/NoticeDetails.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/videogaleri"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/VideoGalleryMain.aspx?videogallerypage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/m/VideoGalleryMain.aspx?videogallerypage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/albumvideolari-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/m/AlbumVideos.aspx?videoalbumid={0}&videopage={1}", parcalar[1],
                                            parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/AlbumVideos.aspx?videoalbumid={0}&videopage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/galeri"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/PhotoGalleryMain.aspx?photogaleripage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/m/PhotoGalleryMain.aspx?photogaleripage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/albumler-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/m/Albums.aspx?categoryid={0}&albumpage={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/Albums.aspx?categoryid={0}&albumpage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/albumdetay-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/m/AlbumPhotos.aspx?albumid={0}&photoalbumpage={1}", parcalar[1],
                                            parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/AlbumPhotos.aspx?albumid={0}&photoalbumpage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/etkinlik-"))
                {
                    newUrl.AppendFormat("~/m/Events.aspx?eventid={0}&title={1}", parcalar[1], parcalar[2]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/etkinlikler"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/Events.aspx?page={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/m/Events.aspx?page=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/urunkategorileri"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/ProductCategoryMain.aspx?page={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/m/ProductCategoryMain.aspx?page=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/urunler-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/m/Products.aspx?cat={0}&page={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/Products.aspx?cat={0}&page=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/urundetay-"))
                {
                    newUrl.AppendFormat("~/m/ProductDetails.aspx?id={0}&name={1}", parcalar[1], parcalar[2]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/ara-"))
                {
                    string searchText = string.Empty;
                    for (int i = 1; i < parcalar.Length; i++)
                    {
                        searchText += parcalar[i] + " ";
                    }
                    searchText = searchText.TrimEnd(' ');
                    newUrl.AppendFormat("~/m/SearchResults.aspx?key={0}", searchText);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/siteharitasi"))
                {
                    newUrl.Append("~/m/SiteMap.aspx");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/sss"))
                {
                    newUrl.Append("~/m/Sss.aspx");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/dil-"))
                {
                    newUrl.AppendFormat("~/m/Dil.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/m/rss"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/m/Rss.aspx?rssId={0}&page={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/m/Rss.aspx?rssId={0}&page=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                    #endregion

                    #region Desktop

                else if (path.Contains("/sayfa-r-"))
                {
                    if (parcalar.Count() > 3)
                        newUrl.AppendFormat("~/RandomField.aspx?id={0}&title={1}", parcalar[2], parcalar[3]);
                    else if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/RandomField.aspx?randomfieldspage={0}", parcalar[2]);
                    else
                        newUrl.AppendFormat("~/RandomField.aspx?randomfieldspage=1");

                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/sayfa-"))
                {
                    newUrl.AppendFormat("~/MenuPlace.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/dinamik-"))
                {
                    newUrl.AppendFormat("~/DynamicContent.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }

                else if (path.Contains("/listeler-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/Lists.aspx?listId={0}&listpage={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/Lists.aspx?listId={0}&listpage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/listedetay-"))
                {
                    newUrl.AppendFormat("~/ListsDetails.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/haberler"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/News.aspx?newslistpage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/News.aspx?newslistpage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/haberdetay-"))
                {
                    newUrl.AppendFormat("~/NewsDetails.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/duyurular"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/Notices.aspx?noticelistpage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/Notices.aspx?noticelistpage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/duyurudetay-"))
                {
                    newUrl.AppendFormat("~/NoticeDetails.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/videogaleri"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/VideoGalleryMain.aspx?videogallerypage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/VideoGalleryMain.aspx?videogallerypage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/albumvideolari-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/AlbumVideos.aspx?videoalbumid={0}&videopage={1}", parcalar[1],
                                            parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/AlbumVideos.aspx?videoalbumid={0}&videopage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/galeri"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/PhotoGalleryMain.aspx?photogaleripage={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/PhotoGalleryMain.aspx?photogaleripage=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/albumler-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/Albums.aspx?categoryid={0}&albumpage={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/Albums.aspx?categoryid={0}&albumpage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/albumdetay-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/AlbumPhotos.aspx?albumid={0}&photoalbumpage={1}", parcalar[1],
                                            parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/AlbumPhotos.aspx?albumid={0}&photoalbumpage=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/urunkategorileri"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/ProductCategoryMain.aspx?page={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/ProductCategoryMain.aspx?page=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/urunler-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/Products.aspx?cat={0}&page={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/Products.aspx?cat={0}&page=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/urundetay-"))
                {
                    newUrl.AppendFormat("~/ProductDetails.aspx?id={0}&name={1}", parcalar[1], parcalar[2]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/ara-"))
                {
                    string searchText = string.Empty;
                    for (int i = 1; i < parcalar.Length; i++)
                    {
                        searchText += parcalar[i] + " ";
                    }
                    searchText = searchText.TrimEnd(' ');
                    newUrl.AppendFormat("~/SearchResults.aspx?key={0}", searchText);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/etkinlik-"))
                {
                    newUrl.AppendFormat("~/Events.aspx?eventid={0}&title={1}", parcalar[1], parcalar[2]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/etkinlikler"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/Events.aspx?page={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/Events.aspx?page=1");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/siteharitasi"))
                {
                    newUrl.Append("~/SiteMap.aspx");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/feed"))
                {
                    if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/Feed.aspx?bolum={0}", parcalar[1]);
                    else
                        newUrl.AppendFormat("~/Feed.aspx");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/rss-"))
                {
                    if (parcalar.Count() > 2)
                        newUrl.AppendFormat("~/Rss.aspx?rssId={0}&page={1}", parcalar[1], parcalar[2]);
                    else if (parcalar.Count() > 1)
                        newUrl.AppendFormat("~/Rss.aspx?rssId={0}&page=1", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/sss"))
                {
                    newUrl.Append("~/Sss.aspx");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/dil-"))
                {
                    newUrl.AppendFormat("~/Dil.aspx?id={0}", parcalar[1]);
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/yeniuye"))
                {
                    newUrl.AppendFormat("~/NewMember.aspx");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/giris"))
                {
                    newUrl.AppendFormat("~/MemberLogin.aspx");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/profil"))
                {
                    newUrl.AppendFormat("~/MemberProfile.aspx?show=dPersonalDetail");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }
                else if (path.Contains("/finans"))
                {
                    newUrl.AppendFormat("~/MemberProfile.aspx?show=dFinanceDetail");
                    HttpContext.Current.RewritePath(newUrl.ToString(), true);
                }

                #endregion
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    public static string LinkOlustur(string gContentID, string gName, string gHtmlAd)
    {
        gName = cevir(gName);
        return String.Format("~/{0}-{1}-{2}", gHtmlAd, gContentID, gName);
    }

    public static string MobilLinkOlustur(string gContentID, string gName, string gHtmlAd)
    {
        gName = cevir(gName);
        return String.Format("{0}-{1}-{2}", gHtmlAd, gContentID, gName);
    }

    public static string AltCevir(string text)
    {
        text = text.Replace('\'', ' ');
        text = text.Replace("”", " ");
        text = text.Replace("“", " ");
        text = text.Replace("<", " ");
        text = text.Replace(">", " ");
        return text;
    }

    public static string cevir(string name)
    {
        name = name.ToLower();
        name = name.Replace("-", "");
        name = name.Replace(" ", "-");
        name = name.Replace("ç", "c");
        name = name.Replace("ğ", "g");
        name = name.Replace("ı", "i");
        name = name.Replace("ö", "o");
        name = name.Replace("ş", "s");
        name = name.Replace("ü", "u");
        name = name.Replace("\"", "");
        name = name.Replace("/", "");
        name = name.Replace("(", "");
        name = name.Replace(")", "");
        name = name.Replace("{", "");
        name = name.Replace("}", "");
        name = name.Replace("%", "");
        name = name.Replace("&", "");
        name = name.Replace("+", "");
        name = name.Replace(",", "");
        name = name.Replace("?", "");
        name = name.Replace(".", "_");
        name = name.Replace("ı", "i");
        name = name.Replace("#", "sharp");
        name = name.Replace("'", "-");
        name = name.Replace("\"", "");
        name = name.Replace("”", "");
        name = name.Replace("“", "");
        name = name.Replace(":", "");
        name = name.Replace("<", "");
        name = name.Replace(">", "");
        return name;
    }
}