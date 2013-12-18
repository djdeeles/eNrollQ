using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

namespace eNroll
{
    public partial class m_RandomField : System.Web.UI.Page
    {
        private readonly Localizations _localizations = new Localizations();
        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            hfLanguageId.Value = EnrollContext.Current.WorkingLanguage.LanguageId.ToString();
            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                if (HttpContext.Current.Request.QueryString["id"] != null && HttpContext.Current.Request.QueryString["title"] != null)
                {
                    try
                    {
                        var id = Convert.ToInt32(HttpContext.Current.Request.QueryString["id"]);
                        var randomField = _entities.Customer_Random.FirstOrDefault(p => p.Id == id);
                        if (randomField != null)
                        {
                            ltTitle.Text = randomField.Title;
                            ltContent.Text = randomField.Text;

                            var site = _entities.SiteGeneralInfo.FirstOrDefault(p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId);
                            if (site != null) Page.Title = string.Format("{0} - {1}", site.title, randomField.Title);
                            MetaGenerate.SetMetaTags(site, Page); 
                            mvRandomFields.SetActiveView(vDetail);
                        }
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.ManageException(exception);
                    }
                }
                else if (HttpContext.Current.Request.QueryString["randomfieldspage"] != null)
                {
                    var site = _entities.SiteGeneralInfo.FirstOrDefault(p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId);
                    if (site != null) Page.Title = site.title;
                    MetaGenerate.SetMetaTags(site, Page);

                    edsCustomerRandom.Where = "it.State=true and it.LanguageId=@languageId";
                    mvRandomFields.SetActiveView(vAll);
                } 
            } 
        } 

        #region DataPager localizations and rewrites

        protected void DataPager1Init(object sender, EventArgs e)
        {
            _localizations.ChangeDataPager((DataPager)sender);
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            DataPager1.PreRender += DataPager1PreRender;
        }
        private void DataPager1PreRender(object sender, EventArgs e)
        {
            foreach (Control control in DataPager1.Controls)
            {
                foreach (Control c in control.Controls)
                {
                    if (c is HyperLink)
                    {
                        var currentLink = (HyperLink)c;
                        if ((!string.IsNullOrEmpty(Request.Url.AbsolutePath)) && (!string.IsNullOrEmpty(Request.Url.Query)))
                        {
                            if (Request.Url.AbsolutePath != "RandomField.aspx")
                            {
                                if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                                {
                                    currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                        Request.Url.PathAndQuery + "&", "sayfa-r-");
                                }
                                else
                                {
                                    currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                              "sayfa-r-");
                                }
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("RandomField.aspx?", "sayfa-r-");
                            }
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("randomfieldspage=", "");
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                        }
                    }
                }
            }
        }
        protected void HyperLink2DataBinding(object sender, EventArgs e)
        {
            var myHyper = (HyperLink)sender;
            var id = Convert.ToInt32(myHyper.NavigateUrl);
            var customerRandom = _entities.Customer_Random.FirstOrDefault(p => p.Id == id);
            if (customerRandom != null)
            {
                myHyper.NavigateUrl = "sayfa-r-" + id + "-" + UrlMapping.cevir(customerRandom.Title);
            }
        }
        
        #endregion
    }
}