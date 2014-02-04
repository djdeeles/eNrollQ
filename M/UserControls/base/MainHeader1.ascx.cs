using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

public partial class M_UserControls_MainHeader1 : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var oEntites = new Entities();
        SiteGeneralInfo oInfo =
            oEntites.SiteGeneralInfo.Where("it.System_language.languageId=" +
                                           EnrollContext.Current.WorkingLanguage.LanguageId).First();
        if (oInfo.header2Path != "")
        {
            if (oInfo.header2Type == 1)
            {
                var Lt = new Literal();
                Lt.Text = string.Format("<a href='{0}'><img src='{1}' style='max-width:100%;'></img></a>",
                                        "../../m/Default.aspx", oInfo.header2Path.Replace("~/", "../"));
                Panel1.Controls.Add(Lt);
            }
            if (oInfo.header2Type == 2)
            {
                var olit = new Literal();
                string flashPath = oInfo.header2Path;
                flashPath = flashPath.Replace("~/", "../");
                olit.Text = "<object type='application/x-shockwave-flash' data='" + flashPath +
                            "'  width='100%' height='" + oInfo.header2Heigth + "'" + ">" +
                            "<param name='movie'" + " value='" + flashPath + "'/>" +
                            "<param name='wmode' value='transparent' />" +
                            "<param name='quality' value='high' />" +
                            "</object>";
                Panel1.Controls.Add(olit);
            }
        }
    }
}