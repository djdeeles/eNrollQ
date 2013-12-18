using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class test : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var ent = new Entities();
        System_language oLang =
            ent.System_language.Where("it.languageId=" + EnrollContext.Current.WorkingLanguage.LanguageId).First();
        System_menu systemMenu =
            ent.System_menu.FirstOrDefault(p => p.menuId == oLang.startupMenuId);
        if (oLang.startupMenuId > 0)
        {
            string redirect = string.Empty;
            if (systemMenu != null)
            {
                redirect = UrlMapping.LinkOlustur(oLang.startupMenuId.ToString(), systemMenu.name, "sayfa");
            }
            else
            {
                redirect = UrlMapping.LinkOlustur(oLang.startupMenuId.ToString(), "", "sayfa");
            }
            Response.Redirect(redirect);
        }
    }
}