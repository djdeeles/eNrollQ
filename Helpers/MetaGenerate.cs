using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using eNroll.App_Data;

/// <summary>
///   Summary description for MetaGenerate
/// </summary>
public class MetaGenerate
{
    public static void SetMetaTags(SiteGeneralInfo GeneralInfo, Page page)
    {
        String strKeyWords = GeneralInfo.keywords;
        String strDescription = GeneralInfo.description;

        var MetaKeywords = new HtmlMeta();

        //Keywords için Meta tag nesnemizi oluşturuyoruz ve nesnemize name ve content niteliklerini ekliyoruz

        MetaKeywords.Attributes.Add("name", "Keywords");
        MetaKeywords.Attributes.Add("content", strKeyWords);
        //Şimdi oluşturduğumuz meta yı header kısmına ekliyoruz
        page.Header.Controls.Add(MetaKeywords);


        //Description için Meta tag nesnemizi oluşturuyoruz ve nesnemize name ve content niteliklerini ekliyoruz
        var MetaDescription = new HtmlMeta();
        MetaDescription.Attributes.Add("name", "Description");
        MetaDescription.Attributes.Add("content", strDescription);
        page.Header.Controls.Add(MetaDescription);
    }

    public static string SetMetaTags(int langId, Page page)
    {
        var entities = new Entities();
        SiteGeneralInfo generalInfo = entities.SiteGeneralInfo.First(p => p.languageId == langId);
        String strKeyWords = generalInfo.keywords;
        String strDescription = generalInfo.description;

        var MetaKeywords = new HtmlMeta();

        //Keywords için Meta tag nesnemizi oluşturuyoruz ve nesnemize name ve content niteliklerini ekliyoruz

        MetaKeywords.Attributes.Add("name", "Keywords");
        MetaKeywords.Attributes.Add("content", strKeyWords);
        //Şimdi oluşturduğumuz meta yı header kısmına ekliyoruz
        page.Header.Controls.Add(MetaKeywords);


        //Description için Meta tag nesnemizi oluşturuyoruz ve nesnemize name ve content niteliklerini ekliyoruz
        var MetaDescription = new HtmlMeta();
        MetaDescription.Attributes.Add("name", "Description");
        MetaDescription.Attributes.Add("content", strDescription);
        page.Header.Controls.Add(MetaDescription);

        return generalInfo.title;
    }
}