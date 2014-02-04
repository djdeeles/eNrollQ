using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Enroll.BaseObjects;
using Enroll.Managers;
using eNroll.App_Data;

namespace eNroll
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private readonly Entities _entities = new Entities();

        protected override void OnInit(EventArgs e)
        {
            if ((!Request.RawUrl.Contains("admin")) &&
                ((_entities.SiteGeneralInfo.FirstOrDefault(p => p.State == false) != null) &&
                 !HttpContext.Current.User.Identity.IsAuthenticated))
            {
                Response.Redirect("Maintenance.aspx");
            }

            if (Session["mobile"] == null || (string) Session["mobile"] == "true")
            {
                CheckMobile();
            }

            CheckContentLanguage();

            //phAlt.Controls.Clear();
            //phGovde.Controls.Clear();
            //phTepe.Controls.Clear();
            //getLayout(0, phTepe);
            //getLayout(1, phGovde);
            //getLayout(2, phAlt);

            // url_mapping yaparken, postback lerde url in değişmesini önler 
            form1.Action = Request.RawUrl;
            form1.Attributes.Add("name", "Form");

            base.OnInit(e);
        }

        public void CheckMobile()
        {
            if (Request.UserAgent != null && (ConfigurationManager.AppSettings["IsMobile"] != null) &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["IsMobile"]))
            {
                var strUserAgent = Request.UserAgent.ToLower();
                if (Request.Browser.IsMobileDevice ||
                    strUserAgent.Contains("iphone") ||
                    strUserAgent.Contains("blackberry") ||
                    strUserAgent.Contains("mobile") ||
                    strUserAgent.Contains("windows ce") ||
                    strUserAgent.Contains("opera mini") ||
                    strUserAgent.Contains("android") ||
                    strUserAgent.Contains("iemobile") ||
                    strUserAgent.Contains("palm"))
                {
                    Session["mobile"] = "true";
                    if (!HttpContext.Current.Request.RawUrl.Contains("/m/") &&
                        !HttpContext.Current.Request.RawUrl.Contains("/giris") &&
                        !HttpContext.Current.Request.RawUrl.Contains("/MemberLogin.aspx"))
                    {
                        Response.Redirect("/m/" + HttpContext.Current.Request.RawUrl);
                    }
                }
            }
        }

        #region GetModules

        //private void getLayout(int location, PlaceHolder ph)
        //{
        //    var ent = new Entities();
        //    var paneller =
        //        ent.EnrollHtmlPanels.Where(p => p.location == location && p.parentId == null)
        //            .OrderBy(p => p.divOrder)
        //            .ToList();
        //    foreach (EnrollHtmlPanels panel in paneller)
        //    {
        //        var pnlDynamic = new HtmlGenericControl("div");
        //        pnlDynamic.Attributes.Add("id", panel.divId);
        //        ph.Controls.Add(pnlDynamic);
        //        getModules(panel, pnlDynamic);
        //        getChilds(pnlDynamic, panel);
        //    }
        //}

        //private void getChilds(HtmlGenericControl pnlParent, EnrollHtmlPanels panel)
        //{
        //    var ent = new Entities();
        //    var childPanels = ent.EnrollHtmlPanels.Where(p => p.parentId == panel.id).ToList();
        //    foreach (EnrollHtmlPanels childPanel in childPanels)
        //    {
        //        var pnlChild = new HtmlGenericControl("div");
        //        pnlChild.Attributes.Add("id", childPanel.divId);
        //        pnlParent.Controls.Add(pnlChild);
        //        getModules(childPanel, pnlChild);
        //    }
        //}

        //private void getModules(EnrollHtmlPanels pnl, HtmlGenericControl pnlDynamic)
        //{
        //    var ent = new Entities();
        //    EnrollModules module = ent.EnrollModules.First(p => p.moduleId == pnl.moduleId);
        //    if (module.path != null)
        //    {
        //        Control ct = LoadControl(module.path);
        //        pnlDynamic.Controls.Add(ct);
        //    }
        //    else
        //    {
        //        var cph = new ContentPlaceHolder();
        //        cph.ID = "ContentPlaceHolder1";
        //        pnlDynamic.Controls.Add(cph);
        //        ((ITemplate) base.ContentTemplates["ContentPlaceHolder1"]).InstantiateIn(cph);
        //    }
        //}

        #endregion

        #region language control

        public void CheckContentLanguage()
        {
            string param1 = string.Empty;
            string param2 = string.Empty;

            try
            {
                var enContext = new EnrollContext();
                var ent = new Entities();
                int lang = enContext.WorkingLanguage.LanguageId;
                System_language system = ent.System_language.FirstOrDefault(p => p.languageId == lang);
                string cultureName = system.languageCulture;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);

                var ReqUrl = Request.RawUrl.Split('-');
                param1 = ReqUrl[0].Trim('/');
                param2 = ReqUrl[1].Trim('/');
                switch (param1)
                {
                    case "sayfa":
                        MenuControlBase.GetPageLanguage(param2);
                        break;
                    case "haberdetay":
                        MenuControlBase.GetNewsLanguage(param2);
                        break;
                    case "duyurudetay":
                        MenuControlBase.GetNoticeLanguage(param2);
                        break;
                    case "albumler":
                        MenuControlBase.GetGalleryLanguage(param2);
                        break;
                    case "albumdetay":
                        MenuControlBase.GetAlbumPhotosLanguage(param2);
                        break;
                    case "albumvideolari":
                        MenuControlBase.GetAlbumVideosLanguage(param2);
                        break;
                    case "etkinlik":
                        MenuControlBase.GetEventLanguage(param2);
                        break;
                    case "dinamik":
                        MenuControlBase.GetDynamicLanguage(param2);
                        break;
                    case "urunler":
                        MenuControlBase.GetProductCategoryLanguage(param2);
                        break;
                    case "urundetay":
                        MenuControlBase.GetProductLanguage(param2);
                        break;
                    case "listedetay":
                        MenuControlBase.GetDynamicListDataLanguage(param2);
                        break;
                    case "rss":
                        MenuControlBase.GetRssLanguage(param2);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region sayfa açılış mesajı kontrolü

        /***
         *1)'introLightBoxHistory' cookie'si varsa lightbox gösterilmez.
         * 
         *2)'introLightBoxHistory' cookie'si yoksa cookie oluşturulur.
         *  cookie expiretime ına lightbox'ın CookieExpireTime değeri verilir ve lightbox gösterilir.
         *  
         *  cookie expire olduktan sonra tekrar 2. adım tekrar eder
         * */

        public string CheckIntroLightBox()
        {
            try
            {
                //cookie varsa lightbox gösterilmez
                if (Request.Cookies["introLightBoxHistory"] != null) return string.Empty;

                //cookie yoksa cookie oluşturulup lightbox gösterilir
                var activeLightBox = _entities.IntroLightBox.FirstOrDefault(p => p.State);
                if (activeLightBox != null)
                {
                    #region create cookie

                    var c = HttpContext.Current.Request.Cookies["introLightBoxHistory"];
                    c = new HttpCookie("introLightBoxHistory");
                    c.Expires = DateTime.Now.AddHours(activeLightBox.CookieExpireTime);
                    HttpContext.Current.Response.Cookies.Add(c);

                    #endregion

                    if (activeLightBox.Type == 0)
                        return string.Format("$.prettyPhoto.open('{0}', '{1}', '');",
                                             activeLightBox.Link.Replace("~", ""), activeLightBox.Title);
                    else
                        return
                            string.Format(
                                "$.prettyPhoto.open('{0}?iframe=true&amp;width={1}px&amp;height={2}px', '{3}', '');",
                                activeLightBox.Link, activeLightBox.Width, activeLightBox.Height, activeLightBox.Title);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return string.Empty;
        }

        #endregion
    }
}