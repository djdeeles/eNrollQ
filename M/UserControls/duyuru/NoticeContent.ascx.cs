using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

public partial class M_UserControls_duyuru_NewsContent : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var oEnroll = new Entities();
            string id = Request.QueryString["id"];
            if (id != null || id != "")
            {
                Notices duyuru = oEnroll.Notices.Where("it.noticeId=" + id + " and it.state=True").First();
                if (duyuru.state != true)
                    Response.Redirect("~/404.aspx");
                lblBaslik.Text = duyuru.header;
                lblYazi.Text = duyuru.details;
                if (duyuru.startDate != null)
                {
                    lbDate.Text = duyuru.startDate.Value.ToShortDateString() + " " +
                                  duyuru.startDate.Value.ToShortTimeString();
                }
                string img = duyuru.imagePath.Replace("~", "");
                if (!String.IsNullOrEmpty(img))
                {
                    Image1.ImageUrl = img;
                }
                else
                {
                    Image1.Visible = false;
                }

                int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
                SiteGeneralInfo site = oEnroll.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
                if (site != null) Page.Title = site.title + " - " + duyuru.header;
                MetaGenerate.SetMetaTags(site, Page);
            }
        }
    }
}