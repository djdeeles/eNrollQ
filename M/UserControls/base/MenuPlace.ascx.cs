using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using Enroll.BaseObjects;
using Enroll.Managers;
using eNroll.App_Data;

public partial class M_UserControls_MenuPlace : MenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            try
            {
                String currentMenuContent = string.Empty;
                var oEntities = new Entities();
                try
                {
                    decimal menuId = Convert.ToDecimal(Request.QueryString["id"]);
                    OMenuObject =
                        oEntities.System_menu.FirstOrDefault(p => p.menuId == menuId);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }

                try
                {
                    SiteGeneralInfo oGeneralInfo =
                        oEntities.SiteGeneralInfo.Where("it.System_language.languageId=" +
                                                        EnrollContext.Current.WorkingLanguage.LanguageId)
                            .FirstOrDefault();
                    SetHeaders(OMenuObject, oGeneralInfo);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }

                currentMenuContent = "MenuContent";
                if (OMenuObject != null && (OMenuObject.state != null && OMenuObject.state.Value != true))
                {
                    Response.Redirect("~/404.aspx");
                }

                var oTemplate = oEntities.TemplatePages.FirstOrDefault(p => p.id == OMenuObject.thema);
                if (oTemplate != null)
                {
                    ContentList = oTemplate.Details.Split('[');
                    CurrentMenuContent = currentMenuContent;
                    LoadMobilControl(PlaceHolder1);
                }
                else
                {
                    var tempControl = LoadControl("~/m/UserControls/base/" + currentMenuContent + ".ascx");
                    PlaceHolder1.Controls.Add(tempControl);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    private void SetHeaders(System_menu menu, SiteGeneralInfo GeneralInfo)
    {
        String strKeyWords = GeneralInfo.keywords;
        String strDescription = GeneralInfo.description;
        String strTitle = GeneralInfo.title;

        if (!string.IsNullOrEmpty(menu.brief))
            strDescription = menu.brief;

        if (!string.IsNullOrEmpty(menu.keywords))
            strKeyWords = menu.keywords;

        strTitle += " - " + OMenuObject.name;

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