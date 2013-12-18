using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

public partial class UserControls_MainPageSpecial2 : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        fillSpecialPlace(Label1);
    }

    private void fillSpecialPlace(Label Label)
    {
        var oEntities = new Entities();
        var oSpecialList =
            oEntities.Customer_Special.Where(
                p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId).Take(3).ToList();
        Customer_Special oSpecial = oSpecialList[1];
        if (oSpecial != null) Label.Text = oSpecial.text;
    }
}