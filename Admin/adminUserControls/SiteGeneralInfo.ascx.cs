using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_SiteGeneralInfo : UserControl
{
    private readonly Entities _entities = new Entities();
    public string imgUrl;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 15))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }
        if (!String.IsNullOrEmpty(Request.QueryString["edit"]))
        {
            var v = (View) MultiView1.FindControl(Request.QueryString["edit"]);
            MultiView1.SetActiveView(v);
        }

        try
        {
            LoadData();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void Save()
    {
        #region save

        SiteGeneralInfo info = null;
        bool infoNew = false;
        SiteSlideShow slide;
        bool slideNew = false;
        var enroll = new Entities();

        #region site ana dilini kaydetme

        var systemLanguages = enroll.System_language.ToList();
        var selectedLangId = Convert.ToInt32(cbChooseLang.SelectedValue);
        var systemActiveLanguage = enroll.System_language.FirstOrDefault(p => p.languageId == selectedLangId);
        try
        {
            if (systemActiveLanguage != null)
            {
                foreach (var language in systemLanguages)
                {
                    language.state = false; // önce tüm diller pasif yapılıyor 
                }
                systemActiveLanguage.state = true; // daha sonra seçili dil aktif yapılarak değişiklikler kaydediliyor
                enroll.SaveChanges();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        #endregion

        #region analytics kodunu kaydetme

        try
        {
            string analyticsFilePath = Request.PhysicalApplicationPath + "/UserControls/base/Analytics.ascx";
            File.WriteAllText(analyticsFilePath, analyticsCode.Text, Encoding.UTF8);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        #endregion

        try
        {
            #region site bakımı işlemi

            var siteInfoList = _entities.SiteGeneralInfo.ToList();
            foreach (var siteGeneralInfo in siteInfoList)
            {
                siteGeneralInfo.State = (!cbSiteMaintenanceMode.Checked);
                _entities.SaveChanges();
            }

            #endregion

            info =
                enroll.SiteGeneralInfo.FirstOrDefault(
                    p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId);
            slide =
                enroll.SiteSlideShow.FirstOrDefault(
                    p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId);
            if (info == null)
            {
                infoNew = true;
                info = new SiteGeneralInfo();
                info.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
            }
            if (slide == null)
            {
                slideNew = true;
                slide = new SiteSlideShow();
                slide.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
            }


            info.title = txtBaslik.Text;
            info.description = txtAciklama.Text;
            info.keywords = txtKeywords.Text;
            var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
            info.bottomText = radEditor.Content;
            info.headerType = Convert.ToInt32(ddlTepe1DosyaTipi.SelectedValue);
            info.headerPath = txtTepeDosya2.Text;

            slide.slideImage1 = txtSsImg1.Text;
            slide.slideImage2 = txtSsImg2.Text;
            slide.slideImage3 = txtSsImg3.Text;
            slide.slideImage4 = txtSsImg4.Text;
            slide.slideImage5 = txtSsImg5.Text;

            slide.siteURl1 = tbUrl1.Text;
            slide.siteURl2 = tbUrl2.Text;
            slide.siteURl3 = tbUrl3.Text;
            slide.siteURl4 = tbUrl4.Text;
            slide.siteURl5 = tbUrl5.Text;

            slide.slideDescription1 = txtSsAciklama1.Text;
            slide.slideDescription2 = txtSsAciklama2.Text;
            slide.slideDescription3 = txtSsAciklama3.Text;
            slide.slideDescription4 = txtSsAciklama4.Text;
            slide.slideDescription5 = txtSsAciklama5.Text;
            slide.slideHeight = txtSlideShowYukseklik.Text;
            slide.slideWidth = txtSlideShowGenislik.Text;

            info.headerHeight = txtHeadYukseklik2.Text;
            info.headerWidth = txtHeadGenislik2.Text;

            slide.slideTime = txtSsTimer.Text;
            slide.slideEffect = ddlSlideShowEfekt.SelectedValue;

            info.header2Type = Convert.ToInt32(ddlTepeDosyaTipi1.SelectedValue);
            info.header2Path = txtTepeDosya1.Text;
            info.header2Heigth = txtTepeYukseklik1.Text;
            info.header2Width = txtTepeGenislik1.Text;

            var infokontrol =
                enroll.SiteGeneralInfo.Where(p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId).
                    ToList();

            if (infokontrol.Count == 0)
                infoNew = true;

            if (infoNew)
                enroll.AddToSiteGeneralInfo(info);

            if (slideNew)
                enroll.AddToSiteSlideShow(slide);

            info.UpdatedTime = DateTime.Now;
            enroll.SaveChanges();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        #endregion
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbSiteGeneralSettings;

        Menu1.Items[0].Text = AdminResource.lbGeneralSettings;
        Menu1.Items[1].Text = AdminResource.lbHeadImageSettings;

        ddlTepeDosyaTipi1.Items.Clear();
        ddlTepeDosyaTipi1.Items.Add(new ListItem(AdminResource.lbImage, "1"));
        ddlTepeDosyaTipi1.Items.Add(new ListItem(AdminResource.lbFlash, "2"));

        ddlTepe1DosyaTipi.Items.Clear();
        ddlTepe1DosyaTipi.Items.Add(new ListItem(AdminResource.lbImage, "1"));
        ddlTepe1DosyaTipi.Items.Add(new ListItem(AdminResource.lbFlash, "2"));
        ddlTepe1DosyaTipi.Items.Add(new ListItem(AdminResource.lbSlide, "3"));

        btnImageSelect.Text = AdminResource.lbImageSelect;
        imgBtnImageSelect0.Text = AdminResource.lbImageSelect;
        imgBtnImageSelect2.Text = AdminResource.lbImageSelect;
        imgBtnImageSelect3.Text = AdminResource.lbImageSelect;
        imgBtnImageSelect4.Text = AdminResource.lbImageSelect;
        imgBtnImageSelect5.Text = AdminResource.lbImageSelect;
        imgBtnImageSelect5.Text = AdminResource.lbImageSelect;
        imgBtnImageSelect6.Text = AdminResource.lbImageSelect;

        btnSave.Text = AdminResource.lbSave;

        cbSiteMaintenanceMode.Text = AdminResource.lbActive;
    }

    public bool SiteIsPublished()
    {
        var siteGeneralInfo = _entities.SiteGeneralInfo.Where(p => p.State == true).ToList();
        if (siteGeneralInfo.Count > 0) return true;

        return false;
    }

    protected void LoadData()
    {
        if (!IsPostBack)
        {
            try
            {
                var enroll = new Entities();

                SiteGeneralInfo info =
                    enroll.SiteGeneralInfo.FirstOrDefault(
                        p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId);

                SiteSlideShow slide =
                    enroll.SiteSlideShow.FirstOrDefault(
                        p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId);

                if (info != null && info.title != null) txtBaslik.Text = info.title;
                if (info != null && info.description != null) txtAciklama.Text = info.description;

                if (info != null && info.keywords != null) txtKeywords.Text = info.keywords;
                var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
                if (info != null && info.bottomText != null) radEditor.Content = info.bottomText;
                int sayac = 0;
                foreach (ListItem item in ddlTepe1DosyaTipi.Items)
                {
                    if (info != null && item.Value == info.headerType.ToString())
                        break;
                    sayac++;
                }
                ddlTepe1DosyaTipi.SelectedIndex = sayac;

                if (ddlTepe1DosyaTipi.SelectedIndex == 2)
                {
                    slideShow.Visible = true;
                    pnlImageFlashHeight.Visible = false;
                }
                else
                {
                    pnlImageFlashHeight.Visible = true;
                    slideShow.Visible = false;
                }

                if (info != null)
                {
                    cbSiteMaintenanceMode.Checked = (!Convert.ToBoolean(info.State));
                }


                if (info.headerPath != null) txtTepeDosya2.Text = info.headerPath;

                if (slide != null && slide.slideImage1 != null) txtSsImg1.Text = slide.slideImage1;
                if (slide != null && slide.slideImage2 != null) txtSsImg2.Text = slide.slideImage2;
                if (slide != null && slide.slideImage3 != null) txtSsImg3.Text = slide.slideImage3;
                if (slide != null && slide.slideImage4 != null) txtSsImg4.Text = slide.slideImage4;
                if (slide != null && slide.slideImage5 != null) txtSsImg5.Text = slide.slideImage5;
                if (slide != null && slide.slideDescription1 != null) txtSsAciklama1.Text = slide.slideDescription1;
                if (slide != null && slide.slideDescription2 != null) txtSsAciklama2.Text = slide.slideDescription2;
                if (slide != null && slide.slideDescription3 != null) txtSsAciklama3.Text = slide.slideDescription3;
                if (slide != null && slide.slideDescription4 != null) txtSsAciklama4.Text = slide.slideDescription4;
                if (slide != null && slide.slideDescription5 != null) txtSsAciklama5.Text = slide.slideDescription5;

                if (slide != null && slide.siteURl1 != null) tbUrl1.Text = slide.siteURl1;
                if (slide != null && slide.siteURl2 != null) tbUrl2.Text = slide.siteURl2;
                if (slide != null && slide.siteURl3 != null) tbUrl3.Text = slide.siteURl3;
                if (slide != null && slide.siteURl4 != null) tbUrl4.Text = slide.siteURl4;
                if (slide != null && slide.siteURl5 != null) tbUrl5.Text = slide.siteURl5;

                if (slide.slideHeight != null) txtSlideShowYukseklik.Text = slide.slideHeight;
                if (slide.slideWidth != null) txtSlideShowGenislik.Text = slide.slideWidth;

                if (info != null && info.headerHeight != null) txtHeadYukseklik2.Text = info.headerHeight;
                if (info != null && info.headerWidth != null) txtHeadGenislik2.Text = info.headerWidth;
                txtSsTimer.Text = slide.slideTime;
                if (slide != null && slide.slideEffect != null) ddlSlideShowEfekt.SelectedValue = slide.slideEffect;

                int index = 0;
                foreach (ListItem item in ddlTepeDosyaTipi1.Items)
                {
                    if (item.Value == info.header2Type.ToString())
                        break;
                    index++;
                }
                ddlTepeDosyaTipi1.SelectedIndex = index;

                if (info != null && info.header2Path != null) txtTepeDosya1.Text = info.header2Path;
                if (info != null && info.header2Heigth != null) txtTepeYukseklik1.Text = info.header2Heigth;
                if (info != null && info.header2Width != null) txtTepeGenislik1.Text = info.header2Width;

                var languages = enroll.System_language.ToList();
                cbChooseLang.DataSource = languages;
                cbChooseLang.DataBind();
                var systemLanguage = enroll.System_language.FirstOrDefault(p => p.state == true);
                if (systemLanguage != null)
                {
                    string defaultLanguage =
                        systemLanguage.languageId.ToString();
                    int itemIndex = 0;
                    foreach (ListItem item in cbChooseLang.Items)
                    {
                        if (item.Value == defaultLanguage)
                        {
                            cbChooseLang.SelectedIndex = itemIndex;
                            break;
                        }
                        itemIndex++;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }


            var analyticsFilePath = Request.PhysicalApplicationPath + "/UserControls/base/Analytics.ascx";
            try
            {
                var lines = File.ReadAllLines(analyticsFilePath, Encoding.UTF8);
                var code = string.Empty;
                foreach (var line in lines)
                {
                    code += line;
                }
                analyticsCode.Text = code;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            Save();

            Logger.Add(15, 0, 0, 3);

            MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        Response.Redirect("Content.aspx?content=SiteGeneralInfo&edit=" + Menu1.SelectedValue);
    }

    protected void ddlDosyaTipi_Chaged(object sender, EventArgs e)
    {
        if (ddlTepe1DosyaTipi.SelectedIndex == 2)
        {
            slideShow.Visible = true;
            pnlImageFlashHeight.Visible = false;
        }
        else
        {
            pnlImageFlashHeight.Visible = true;
            slideShow.Visible = false;
        }
    }
}