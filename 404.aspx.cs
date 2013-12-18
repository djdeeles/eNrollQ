using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class _404 : Page
{
    protected override void OnInit(EventArgs e)
    {
        var entities = new Entities();
        var siteGeneralInfo =
            entities.SiteGeneralInfo.First(p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId);
        Page.Title = siteGeneralInfo.title + " - 404";
        base.OnInit(e);
    }
}