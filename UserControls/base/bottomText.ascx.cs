using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class UserControls_bottomText : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var oEntites = new Entities();
        SiteGeneralInfo oInfo =
            oEntites.SiteGeneralInfo.Where("it.System_language.languageId=" +
                                           EnrollContext.Current.WorkingLanguage.LanguageId).First();
        Label1.Text = oInfo.bottomText;
    }
}