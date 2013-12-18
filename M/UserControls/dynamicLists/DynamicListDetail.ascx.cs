using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class M_UserControls_DynamicListDetail : UserControl
{
    public int ListDataId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        var ent = new Entities();
        string header = string.Empty;
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ListDataId = Convert.ToInt32(Request.QueryString["id"]);
                ListData lData = ent.ListData.First(p => p.Id == ListDataId);
                if (lData.State != true)
                    Response.Redirect("~/404.aspx");
                header = lData.Title;
                lblBaslik.Text = header;
                lblYazi.Text = lData.Detail;
                if (!String.IsNullOrEmpty(lData.Image))
                {
                    Image1.ImageUrl = lData.Image;
                }
                else
                {
                    Image1.Visible = false;
                }
            }

            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) Page.Title = site.title + " - " + header;
            MetaGenerate.SetMetaTags(site, Page);
        }
    }
}