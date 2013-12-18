using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using Enroll.BaseObjects;
using eNroll.App_Data;

public partial class m_MasterPage : MasterPage
{
    private Entities _entities = new Entities();
    protected override void OnInit(EventArgs e)
    {

        if ((!Request.RawUrl.Contains("admin")) && ((_entities.SiteGeneralInfo.FirstOrDefault(p => p.State == false) != null) && !HttpContext.Current.User.Identity.IsAuthenticated))
        {
            Response.Redirect("../Maintenance.aspx");
        }

        if (!CheckMobileSiteIsActive()) Response.Redirect(Request.RawUrl.Replace("/m", ""));
        CheckContentLanguage();
        // url_mapping yaparken, postback lerde url in değişmesini önler 
        form1.Action = Request.RawUrl;
        base.OnInit(e);
    }

    private bool CheckMobileSiteIsActive()
    {
        if (ConfigurationManager.AppSettings["IsMobile"] != null)
            return Convert.ToBoolean(ConfigurationManager.AppSettings["IsMobile"]);
        else
            return false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ViewFullSite_Click(object sender, EventArgs e)
    {
        Session["mobile"] = "false";
        Response.Redirect(Request.RawUrl.Replace("/m", ""));
    }

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

            string[] ReqUrl = Request.RawUrl.Split('-');
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
}