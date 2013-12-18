using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

public partial class UserControls_EventList : UserControl
{
    private readonly Localizations _localizations = new Localizations();
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        ListView1.DataSource =
            ent.Events.Where(p => p.State == true &&
                                  p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId).OrderByDescending(
                                      p => p.StartDate).ToList();
        ListView1.DataBind();

        if (String.IsNullOrEmpty(Request.QueryString["eventid"]))
        {
            ActivityDetail.Visible = false;
            activityList.Visible = true;
            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) Page.Title = site.title;
            MetaGenerate.SetMetaTags(site, Page);
        }
        else
        {
            ActivityDetail.Visible = true;
            activityList.Visible = false;
            var aktivite = ent.Events.Where("it.Id=" + Request.QueryString["eventid"]).FirstOrDefault();
            if (aktivite != null)
            {
                if (aktivite.State != true)
                    Response.Redirect("~/404.aspx");
                lName.Text = aktivite.Name;
                lDetails.Text = aktivite.Details;
                lStartDate.Text = Convert.ToDateTime(aktivite.StartDate).ToShortDateString();
                lEndDate.Text = Convert.ToDateTime(aktivite.EndDate).ToShortDateString();
            }

            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) if (aktivite != null) Page.Title = site.title + " - " + aktivite.Name;
            MetaGenerate.SetMetaTags(site, Page);
        }
    }


    protected void DataPager1_Init(object sender, EventArgs e)
    {
        _localizations.ChangeDataPager((DataPager) sender);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataPager1.PreRender += DataPager1_PreRender;
    }

    private void DataPager1_PreRender(object sender, EventArgs e)
    {
        foreach (Control control in DataPager1.Controls)
        {
            foreach (Control c in control.Controls)
            {
                if (c is HyperLink)
                {
                    var currentLink = (HyperLink) c;
                    if ((!string.IsNullOrEmpty(Request.Url.AbsolutePath)) && (!string.IsNullOrEmpty(Request.Url.Query)))
                    {
                        if (Request.Url.AbsolutePath != "/Events.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../etkinlikler-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../etkinlikler-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/Events.aspx?", "/etkinlikler-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("page=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                    }
                }
            }
        }
    }
}