using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Enroll.Managers;
using eNroll.App_Data;

namespace eNroll.Admin
{
    public partial class MultipleFileSelector : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var entities = new Entities();
            var adminLanguage =
                entities.System_language.FirstOrDefault(
                    p => p.languageId == EnrollAdminContext.Current.AdminLanguage.LanguageId);
            if (adminLanguage != null)
            {
                var culture = adminLanguage.languageCulture;

                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
        }

        protected string GetSiteTitle(int DilId)
        {
            string Title = string.Empty;
            try
            {
                var ent = new Entities();
                var SiteTile = (from p in ent.SiteGeneralInfo
                                where p.languageId == DilId
                                select new
                                           {
                                               p.title
                                           }).FirstOrDefault();
                if (!string.IsNullOrEmpty(SiteTile.title))
                {
                    Title = SiteTile.title;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return Title;
        }
    }
}