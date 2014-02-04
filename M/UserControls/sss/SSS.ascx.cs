using System;
using System.Linq;
using System.Web.UI;
using Resources;
using eNroll.App_Data;

public partial class M_UserControls_SSS : UserControl
{
    private string MainBitis = "</dl>";
    private string ddvedl = "<dd><ul>";
    private string ddvedlkapat = "</ul></dd>";
    private string faqMain = "<dl id='faq'>";
    private string faqcatMain = "<dl id='faqcat'>";
    private string yazi = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        var oEntities = new Entities();

        if (!IsPostBack)
        {
            loadCategories();
        }
        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = oEntities.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title;
        MetaGenerate.SetMetaTags(site, Page);
    }

    private void loadCategories()
    {
        var oEntities = new Entities();
        var oCatList =
            oEntities.FaqCategories.Where(
                p =>
                p.active == true && p.System_language.languageId == EnrollContext.Current.WorkingLanguage.LanguageId)
                .OrderBy(p => p.orderId)
                .ToList();
        if (oCatList.Count > 0)
        {
            for (int i = 0; i < oCatList.Count; i++)
            {
                yazi = yazi + "<dt><b>" + oCatList[i].faqCategory + "</b></dt>";
                var oFaqList =
                    oEntities.Faq.Where("it.faqCategoryId=" + oCatList[i].faqCategoryId + " and it.active=True")
                        .ToList();
                if (oFaqList.Count > 0)
                {
                    yazi = yazi + ddvedl;
                    for (int z = 0; z < oFaqList.Count; z++)
                    {
                        yazi = yazi + "<li>" + faqMain + "<dt><b>" + oFaqList[z].faqQuestion + "</b></dt><dd>" +
                               oFaqList[z].faqAnswer + "</dd>" + MainBitis + "</li>";
                    }
                    yazi = yazi + ddvedlkapat;
                }
            }

            Label1.Text = faqcatMain + yazi + MainBitis;
        }
        else
        {
            Label1.Text = Resource.lbSSSNoCategory;
        }
    }
}