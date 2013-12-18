using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using eNroll.App_Data;

public partial class Admin_adminUserControls_DataLanguageChooser : UserControl
{

    Entities ent = new Entities();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["IsMultiLanguage"]))
                Visible = false;
            else
            {
                var languages = ent.System_language.ToList();
                foreach (var dil in languages.OrderBy(p => p.languageId))
                {
                    var lb = new ImageButton();
                    var imgUrl = "~/Admin/images/dil/" + dil.languageCulture + ".png";
                    lb.ImageUrl = imgUrl;
                    lb.CommandArgument = dil.languageId.ToString();
                    lb.Click += LbClick;
                    if (EnrollAdminContext.Current.DataLanguage.LanguageId == dil.languageId)
                    {
                        lb.CssClass = "languageactive";
                    }
                    else
                    {
                        lb.CssClass = "language";
                    }
                    Panel1.Controls.Add(lb);
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void LbClick(object sender, ImageClickEventArgs e)
    {
        var url = string.Empty;
        try
        {
            var lb = (ImageButton) sender;
            var langId = Convert.ToInt32(lb.CommandArgument); 
            var dataLang = ent.System_language.FirstOrDefault(p => p.languageId == langId);
            if (dataLang != null)
                EnrollAdminContext.Current.DataLanguage.LanguageId = Convert.ToInt32(dataLang.languageId);
            url = Request.RawUrl;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        if (url != string.Empty)
        {
            Response.Redirect(url, false);
        }
    }
}