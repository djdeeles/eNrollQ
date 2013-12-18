using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;
using eNroll.App_Data;

namespace eNroll
{
    public partial class Feed : Page
    {
        private readonly Entities _oEnroll = new Entities();

        private string _rssImage = string.Empty;
        private string _sayfa = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            _sayfa = "http://" + Request.Url.Host + "/";
            var dilId = EnrollContext.Current.WorkingLanguage.LanguageId;
            var siteGeneralInfo = _oEnroll.SiteGeneralInfo.FirstOrDefault(p => p.languageId == dilId);
            if (siteGeneralInfo != null)
            {
                string baslik = siteGeneralInfo.title;
                Response.Clear();
                Response.ContentType = "text/xml";
                var objX = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
                objX.WriteStartDocument();
                objX.WriteStartElement("rss");
                objX.WriteAttributeString("version", "2.0");
                objX.WriteStartElement("channel");
                objX.WriteElementString("title", baslik);
                objX.WriteElementString("link", _sayfa + "rss.aspx");
                objX.WriteElementString("description", baslik + " - RSS");

                if (Request.QueryString.Count != 0 && Request.QueryString["bolum"] != null &&
                    Request.QueryString["bolum"] != string.Empty)
                {
                    int bolum = Convert.ToInt32(Request.QueryString["bolum"]);
                    switch (bolum)
                    {
                        case 1:
                            HaberlerRssVer(objX);
                            break;
                        case 2:
                            DuyurularRssVer(objX);
                            break;
                        case 3:
                            EtkinliklerRssVer(objX);
                            break;
                        case 4:
                            MenuIcerikRssVer(objX);
                            break;
                    }
                }
                else
                {
                    HaberlerRssVer(objX);
                    DuyurularRssVer(objX);
                    EtkinliklerRssVer(objX);
                    MenuIcerikRssVer(objX);
                }
                objX.WriteEndDocument();
                objX.Flush();
                objX.Close();
            }
            Response.End();
            Response.Write(Request.RawUrl);
        }

        private void HaberlerRssVer(XmlTextWriter objX)
        {
            _rssImage = string.Empty;
            var newsListe = _oEnroll.News.Where(p => p.state == true &&
                                                     p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId).
                OrderByDescending(p => p.enterDate).ToList();
            foreach (var n in newsListe)
            {
                objX.WriteStartElement("item");
                objX.WriteElementString("title", n.header);

                if (!string.IsNullOrEmpty(n.thumbnailPath))
                {
                    _rssImage = "<img align='left' border='0' src='" + _sayfa + n.thumbnailPath.Replace("~/", "") +
                                "' alt='" + UrlMapping.AltCevir(n.header) + "' " +
                                "style='margin:0 5px 5px 0' />";
                }
                objX.WriteElementString("description", _rssImage + n.brief);
                objX.WriteElementString("link", _sayfa + "haberdetay-" + n.newsId + "-" + UrlMapping.cevir(n.header));
                objX.WriteEndElement();
                _rssImage = string.Empty;
            }
        }

        private void DuyurularRssVer(XmlTextWriter objX)
        {
            _rssImage = string.Empty;
            var noticesListe = _oEnroll.Notices.Where(p => p.state == true &&
                                                           p.languageId ==
                                                           EnrollContext.Current.WorkingLanguage.LanguageId).
                OrderByDescending(p => p.startDate).ToList();
            foreach (var n in noticesListe)
            {
                objX.WriteStartElement("item");
                objX.WriteElementString("title", n.header);

                if (!string.IsNullOrEmpty(n.thumbnailPath))
                {
                    _rssImage = "<img align='left' border='0' src='" + _sayfa + n.thumbnailPath.Replace("~/", "") +
                                "' alt='" + UrlMapping.AltCevir(n.header) + "' " +
                                "style='margin:0 5px 5px 0' />";
                }
                objX.WriteElementString("description", _rssImage + n.description);
                objX.WriteElementString("link", _sayfa + "duyurudetay-" + n.noticeId + "-" + UrlMapping.cevir(n.header));
                objX.WriteEndElement();
                _rssImage = string.Empty;
            }
        }

        private void EtkinliklerRssVer(XmlTextWriter objX)
        {
            _rssImage = string.Empty;
            var eventsListe = _oEnroll.Events.Where(p => p.State == true &&
                                                         p.languageId ==
                                                         EnrollContext.Current.WorkingLanguage.LanguageId).
                OrderByDescending(p => p.StartDate).ToList();
            foreach (var n in eventsListe)
            {
                objX.WriteStartElement("item");
                objX.WriteElementString("title", n.Name);
                objX.WriteElementString("description", n.Description);
                objX.WriteElementString("link", _sayfa + "etkinlik-" + n.id + "-" + UrlMapping.cevir(n.Name));
                objX.WriteEndElement();
            }
        }

        private void MenuIcerikRssVer(XmlTextWriter objX)
        {
            _rssImage = string.Empty;
            var menuListe = _oEnroll.System_menu.Where(p => p.state==true && p.menuId > 1 && (p.type == "2" || p.type == "0") &&
                                                            p.languageId ==
                                                            EnrollContext.Current.WorkingLanguage.LanguageId).
                OrderByDescending(p => p.menuId).ToList();
            foreach (var s in menuListe)
            {
                objX.WriteStartElement("item");
                objX.WriteElementString("title", s.name);
                objX.WriteElementString("description", s.brief);
                objX.WriteElementString("link", _sayfa + "sayfa-" + s.menuId + "-" + UrlMapping.cevir(s.name));
                objX.WriteEndElement();
            }
        }
    }
}