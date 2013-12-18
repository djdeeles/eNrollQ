using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class M_UserControls_NewsContent : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var ent = new Entities();
        string header = string.Empty;
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                News haber = ent.News.First(p => p.newsId == id);
                if (haber.state != true)
                    Response.Redirect("~/404.aspx");
                header = haber.header;
                if (haber.enterDate != null)
                {
                    lbDate.Text = haber.enterDate.Value.ToShortDateString() + " " +
                                  haber.enterDate.Value.ToShortTimeString();
                }
                lblBaslik.Text = header;
                lblYazi.Text = haber.details;
                if (!String.IsNullOrEmpty(haber.imagePath))
                {
                    Image1.ImageUrl = haber.imagePath;
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