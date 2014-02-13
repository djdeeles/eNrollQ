using System;
using System.Data.Objects;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Enroll.Managers;
using eNroll.App_Data;

public partial class UserControls_DynamicDataList : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var oEntities = new Entities();

            var oGeneralInfo =
                oEntities.SiteGeneralInfo.FirstOrDefault(
                    p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId);

            //eğer grupid null ise
            if (string.IsNullOrEmpty(Request.QueryString["groupId"]))
            {
                Customer_Dynamic oDynamic = oEntities.Customer_Dynamic.Where("it.dynamicId=@paramId",
                                                                             new ObjectParameter("paramId",
                                                                                                 Convert.ToDecimal(
                                                                                                     Request.QueryString
                                                                                                         ["id"]))).
                    FirstOrDefault();

                if (oDynamic != null)
                {
                    oDynamic.Customer_Dynamic_GroupReference.Load();
                    hdnGroupId.Value = oDynamic.Customer_Dynamic_Group.groupId.ToString();

                    lblDetails.Text = oDynamic.details;
                    SetHeaders(oDynamic, oGeneralInfo);
                }
            }
            else
            {
                Customer_Dynamic_Group oGroup = oEntities.Customer_Dynamic_Group.Where("it.groupId=@paramGroupId",
                                                                                       new ObjectParameter(
                                                                                           "paramGroupId",
                                                                                           Convert.ToDecimal(
                                                                                               Request.QueryString[
                                                                                                   "groupId"]))).
                    FirstOrDefault();

                if (string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    lblDetails.Text = oGroup.details;
                }
                else
                {
                    Customer_Dynamic oDynamic =
                        oEntities.Customer_Dynamic.Where("it.dynamicId=@paramId",
                                                         new ObjectParameter("paramId",
                                                                             Convert.ToDecimal(Request.QueryString["id"])))
                            .FirstOrDefault();
                    lblDetails.Text = oDynamic.details;
                    SetHeaders(oDynamic, oGeneralInfo);
                }

                hdnGroupId.Value = Request.QueryString["groupId"];
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        var hl = (HyperLink) e.Item.FindControl("hlMenu");
        hl.NavigateUrl = UrlMapping.LinkOlustur(hl.ToolTip, hl.Text, "dinamik");
    }

    private void SetHeaders(Customer_Dynamic dMenu, SiteGeneralInfo GeneralInfo)
    {
        String strKeyWords = GeneralInfo.keywords;
        String strDescription = GeneralInfo.description;
        String strTitle = GeneralInfo.title;

        strTitle += " - " + dMenu.name;

        var MetaKeywords = new HtmlMeta();
        //Keywords için Meta tag nesnemizi oluşturuyoruz ve nesnemize name ve content niteliklerini ekliyoruz
        MetaKeywords.Attributes.Add("name", "Keywords");
        MetaKeywords.Attributes.Add("content", strKeyWords);
        //Şimdi oluşturduğumuz meta yı header kısmına ekliyoruz
        Page.Header.Controls.Add(MetaKeywords);
        //Description için Meta tag nesnemizi oluşturuyoruz ve nesnemize name ve content niteliklerini ekliyoruz
        var MetaDescription = new HtmlMeta();
        MetaDescription.Attributes.Add("name", "Description");
        MetaDescription.Attributes.Add("content", strDescription);
        Page.Header.Controls.Add(MetaDescription);
        //Sayfanın başlığınıda tablomuzdan çektiğimiz titleye eşitliyoruz
        Page.Title = strTitle;
    }
}