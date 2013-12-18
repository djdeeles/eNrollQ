using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class M_dil : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];
            var oEntities = new Entities();
            System_language oLanguage = oEntities.System_language.Where("it.languageId=" + id).First();
            EnrollContext.Current.WorkingLanguage.LanguageId = Convert.ToInt32(oLanguage.languageId);
            Response.Redirect("~/m/Default.aspx");
        }
    }
}