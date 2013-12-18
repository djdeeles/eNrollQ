using System;
using System.Linq;
using System.Web.UI;
using Enroll.Managers;
using eNroll.App_Data;

public partial class m_UserControls_RandomField : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        var oEntities = new Entities();
        var random = new Random();
        try
        {
            var oGeneralInfo = oEntities.SiteGeneralInfo.FirstOrDefault(p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId);
            var customerRandom = oEntities.Customer_Random.Where(p => p.LanguageId == oGeneralInfo.languageId && p.State).ToList();
            var itemCount = customerRandom.Count;
            var randomSelectedItemIndex = random.Next(0, itemCount);
            var selectedItem = customerRandom[randomSelectedItemIndex];
            var detailLink = string.Format("<a class='button' href='sayfa-r-{0}-{1}'>{2}</a>",
                selectedItem.Id, UrlMapping.cevir(selectedItem.Title), Resources.Resource.details);
            ltRandom.Text = string.Format("{0}<br/>{1}", selectedItem.Summary, detailLink);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}