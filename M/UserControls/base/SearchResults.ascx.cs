using System;
using System.Linq;
using System.Web.UI;
using Enroll.WebParts;
using eNroll.App_Data;

public partial class M_UserControls_SearchResults : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var ent = new Entities();
        string result = EnrollSearch.writeResults(EnrollSearch.qStringDuzelt(Request.QueryString["key"]));
        PlaceHolder1.Controls.Add(new LiteralControl(result));

        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title + " - " + Request.QueryString["key"];
        MetaGenerate.SetMetaTags(site, Page);
    }
}