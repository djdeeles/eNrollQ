using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

public partial class M_UserControls_MainHeader2 : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var oEntites = new Entities();
        SiteGeneralInfo oInfo =
            oEntites.SiteGeneralInfo.Where("it.System_language.languageId=" +
                                           EnrollContext.Current.WorkingLanguage.LanguageId).First();
        if (oInfo.headerType == 1)
        {
            var Lt = new Literal();
            Lt.Text = "<img src=" + oInfo.headerPath.Replace("~/", "../") + " style='max-width:100%;'></img>";
            Panel1.Controls.Add(Lt);
        }
        if (oInfo.headerType == 2)
        {
            var olit = new Literal();
            string flashPath = oInfo.headerPath;
            flashPath = flashPath.Replace("~/", "../");
            olit.Text = "<object type='application/x-shockwave-flash' data='" + flashPath + "' width='" +
                        oInfo.headerWidth + "' height='" + oInfo.headerHeight + "'" + ">" +
                        "<param name='movie'" + " value='" + flashPath + "'/>" +
                        "<param name='wmode' value='transparent' />" +
                        "<param name='quality' value='high' />" +
                        "</object>";
            Panel1.Controls.Add(olit);
        }
        if (oInfo.headerType == 3)
        {
            var oEntities = new Entities();
            SiteSlideShow oSlide =
                oEntites.SiteSlideShow.Where("it.System_language.languageId=" +
                                             EnrollContext.Current.WorkingLanguage.LanguageId).First();
            var oLite = new Literal();
            var oLite2 = new Literal();
            string code = "";
            string s1 = oSlide.slideImage1.Replace("~/", "../");
            string s2 = oSlide.slideImage2.Replace("~/", "../");
            string s3 = oSlide.slideImage3.Replace("~/", "../");
            string s4 = oSlide.slideImage4.Replace("~/", "../");
            string s5 = oSlide.slideImage5.Replace("~/", "../");
            ;
            if (s1 != "" && s2 != "" && s3 != "" && s4 != "" && s5 != "")
            {
                code = "<div id='wrap'>" +
                       "<img style='max-width:100%;' src='" + s1 + "' alt='" + oSlide.slideDescription1 + "'/><a href='" +
                       oSlide.siteURl1 + "'></a>" +
                       "</div>";
            }
            else if (s1 != "" && s2 != "" && s3 != "" && s4 != "" && s5 == "")
            {
                code = "<div id='wrap'>" +
                       "<img style='max-width:100%;' src='" + s1 + "' alt='" + oSlide.slideDescription1 + "'/><a href='" +
                       oSlide.siteURl1 + "'></a>" +
                       "</div>";
            }
            else if (s1 != "" && s2 != "" && s3 != "" && s4 == "" && s5 == "")
            {
                code = "<div id='wrap'>" +
                       "<img style='max-width:100%;' src='" + s1 + "' alt='" + oSlide.slideDescription1 + "'/><a href='" +
                       oSlide.siteURl1 + "'></a>" +
                       "</div>";
            }
            else if (s1 != "" && s2 != "" && s3 == "" && s4 == "" && s5 == "")
            {
                code = "<div id='wrap'>" +
                       "<img style='max-width:100%;' src='" + s1 + "' alt='" + oSlide.slideDescription1 + "'/><a href='" +
                       oSlide.siteURl1 + "'></a>" +
                       "</div>";
            }
            else if (s1 != "" && s2 == "" && s3 == "" && s4 == "" && s5 == "")
            {
                code = "<div id='wrap'>" +
                       "<img style='max-width:100%;' src='" + s1 + "' alt='" + oSlide.slideDescription1 + "'/><a href='" +
                       oSlide.siteURl1 + "'></a>" +
                       "</div>";
            }

            oLite.Text = code;
            Panel1.Controls.Add(oLite);
            Panel1.Controls.Add(oLite2);
        }
    }
}