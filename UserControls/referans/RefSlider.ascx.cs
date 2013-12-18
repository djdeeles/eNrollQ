using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class UserControls_RefSlider : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string refLogoCode = "";
        var ent = new Entities();
        IQueryable<RefLogos> refLogoList = ent.RefLogos.Where(p => p.state == true &&
                                                                   p.languageId ==
                                                                   EnrollContext.Current.WorkingLanguage.LanguageId);
        refLogoCode = "<div id='refsliderdiv'><ul id='refslider' class='logosliderskin' >";
        foreach (RefLogos i in refLogoList)
        {
            refLogoCode += "<li><a href='" + i.url + "' target='_blank'><img src='" + i.image.Replace("~", "") +
                           "' style='border:0;' alt='" + i.name + "'/></a></li>";
        }
        refLogoCode += "</ul></div>";
        Label1.Text = refLogoCode;
    }
}