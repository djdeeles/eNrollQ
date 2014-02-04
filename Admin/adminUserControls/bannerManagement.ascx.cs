using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class admin_bannerManagement : UserControl
{
    private readonly Entities ent = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbBannerFieldManagement;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 9))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }
        try
        {
            if (!IsPostBack)
            {
                MultiView1.SetActiveView(View1);
            }
            else
            {
                dpEndDate2.SelectedDate = null;
                dpEndDate.SelectedDate = null;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        btnAddNew.Text = AdminResource.lbPublishNewBanner;
        cbAddState.Text = AdminResource.lbActive;
        cbEditState.Text = AdminResource.lbActive;

        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;

        gvBanners.Columns[0].HeaderText = AdminResource.lbActions;
        gvBanners.Columns[1].HeaderText = AdminResource.lbBannerField;
        gvBanners.Columns[2].HeaderText = AdminResource.lbBanner;
        gvBanners.Columns[3].HeaderText = AdminResource.lbViewLimit;
        gvBanners.Columns[4].HeaderText = AdminResource.lbViewCount;
        gvBanners.Columns[5].HeaderText = AdminResource.lbStartDate;
        gvBanners.Columns[6].HeaderText = AdminResource.lbEndDate;
        gvBanners.Columns[7].HeaderText = AdminResource.lbState;

        btnEditSave.Text = AdminResource.lbSave;
        btnEditCancel.Text = AdminResource.lbCancel;

        ReklamlariVer();
    }

    private void ReklamAlanlariVer(DropDownList DropDownListAlan)
    {
        DropDownListAlan.DataSource = string.Empty;
        DropDownListAlan.DataBind();
        var ReklamAlanlari = from p in ent.BannerLocation
                             select p;
        if (ReklamAlanlari.Count() != 0)
        {
            foreach (BannerLocation item in ReklamAlanlari)
            {
                var i = new ListItem();
                i.Text = item.location;
                i.Value = item.bannerLocationId.ToString();
                DropDownListAlan.Items.Add(i);
            }
        }
        DropDownListAlan.Items.Insert(0, new ListItem(AdminResource.lbChoose, "Seçiniz"));
    }

    private void ReklamlariVer(DropDownList DropDownListAlan)
    {
        try
        {
            DropDownListAlan.DataSource = string.Empty;
            DropDownListAlan.DataBind();
            var Reklamlar = from p in ent.Banners
                            select p;
            if (Reklamlar.Count() != 0)
            {
                foreach (Banners item in Reklamlar)
                {
                    var i = new ListItem();
                    i.Text = item.bannerName;
                    i.Value = item.bannersId.ToString();
                    DropDownListAlan.Items.Add(i);
                }
            }
            DropDownListAlan.Items.Insert(0, new ListItem(AdminResource.lbChoose, "Seçiniz"));
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void ReklamlariVer()
    {
        try
        {
            EntityDataSourceBannerManagement.Where = " it.languageId=" +
                                                     EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
            EntityDataSourceBannerManagement.OrderBy = "it.bannerStartDate";
            gvBanners.DataSourceID = "EntityDataSourceBannerManagement";
            gvBanners.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs eventArgs)
    {
        ReklamAlanlariVer(DropDownListReklamAlanlari1);
        ReklamlariVer(DropDownListReklamlar);
        MultiView1.SetActiveView(View2);
    }

    protected void btnSave_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            var bmanagement = new BannerManagement();
            bmanagement.bannersId = Convert.ToInt32(DropDownListReklamlar.SelectedValue);
            bmanagement.bannerLocationId = Convert.ToInt32(DropDownListReklamAlanlari1.SelectedValue);
            bmanagement.state = cbAddState.Checked;
            if (txtLimit.Text != "")
            {
                bmanagement.bannerLimit = Convert.ToInt32(txtLimit.Text);
            }

            if (dpStartDate.SelectedDate != null)
            {
                bmanagement.bannerStartDate = Convert.ToDateTime(dpStartDate.SelectedDate.Value.ToShortDateString());
            }
            if (dpEndDate.SelectedDate != null)
            {
                if (dpEndDate.SelectedDate < dpStartDate.SelectedDate)
                {
                    MessageBox.Show(MessageType.Success, AdminResource.msgEndDateCanNotBeBeforeStartDate);
                    return;
                }
                else
                    bmanagement.bannerEndDate = Convert.ToDateTime(dpEndDate.SelectedDate.Value.ToShortDateString());
            }

            bmanagement.bannerCounter = 0;
            bmanagement.languageId = Convert.ToInt32(EnrollAdminContext.Current.DataLanguage.LanguageId);
            bmanagement.CreatedTime = DateTime.Now;
            bmanagement.UpdatedTime = DateTime.Now;
            ent.AddToBannerManagement(bmanagement);
            ent.SaveChanges();
            MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            MultiView1.SetActiveView(View1);
            Logger.Add(9, 2, bmanagement.bannerManagementId, 1);

            ReklamlariVer();
            Temizle();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs eventArgs)
    {
        MultiView1.SetActiveView(View1);
        Temizle();
    }

    protected void btnEditSave_Click(object sender, EventArgs eventArgs)
    {
        BannerManagement bnrmng;
        int bannersId = 0;
        if (!string.IsNullOrEmpty(HiddenFieldBannerId.Value))
        {
            try
            {
                bannersId = Convert.ToInt32(HiddenFieldBannerId.Value);
                bnrmng = ent.BannerManagement.Where(p => p.bannersId == bannersId).First();
                if (!string.IsNullOrEmpty(TextBoxLimit.Text))
                {
                    bnrmng.bannerLimit = Convert.ToInt32(TextBoxLimit.Text);
                }
                else
                {
                    bnrmng.bannerLimit = null;
                }
                if (dpStartDate2.SelectedDate != null)
                {
                    bnrmng.bannerStartDate = Convert.ToDateTime(dpStartDate2.SelectedDate.Value.ToShortDateString());
                }
                if (dpEndDate2.SelectedDate != null)
                {
                    bnrmng.bannerEndDate = Convert.ToDateTime(dpEndDate2.SelectedDate.Value.ToShortDateString());
                }
                else
                {
                    bnrmng.bannerEndDate = null;
                }

                bnrmng.state = cbEditState.Checked;

                bnrmng.UpdatedTime = DateTime.Now;
                ent.SaveChanges();

                Logger.Add(9, 2, bnrmng.bannerManagementId, 3);
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                ReklamlariVer();
                TextBoxLimit.Text = string.Empty;
                MultiView1.SetActiveView(View1);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void btnEditCancel_Click(object sender, EventArgs eventArgs)
    {
        MultiView1.SetActiveView(View1);
    }

    protected void gvBanners_DataBound(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gvBanners.Rows)
            {
                var lblId = (Label) row.FindControl("lblId");
                Banners myBanner = ent.Banners.Where("it.bannersId=" + lblId.Text).First();
                lblId.Text = myBanner.bannerName;
            }
            if (gvBanners.Rows.Count < 1)
            {
                lblUyariBos.Text = "";
            }
            else
            {
                lblUyariBos.Text = "";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void gvBanners_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        BannerManagement bnrmng;
        int bannerManagementId = 0;
        if (e.CommandName == "Guncelle")
        {
            bannerManagementId = Convert.ToInt32(e.CommandArgument);
            bnrmng = ent.BannerManagement.Where(p => p.bannerManagementId == bannerManagementId).First();
            ReklamAlaniGuncelle(bnrmng);
            MultiView1.SetActiveView(View3);
        }
        else if (e.CommandName == "Sil")
        {
            bannerManagementId = Convert.ToInt32(e.CommandArgument);
            bnrmng = ent.BannerManagement.Where(p => p.bannerManagementId == bannerManagementId).First();
            ent.DeleteObject(bnrmng);
            ent.SaveChanges();

            Logger.Add(9, 2, bannerManagementId, 2);

            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            ReklamlariVer();
        }
    }

    private void ReklamAlaniGuncelle(BannerManagement bannerManagement)
    {
        try
        {
            Temizle();
            LabelReklamAdi.Text = bannerManagement.Banners.bannerName;
            if (bannerManagement.bannerLimit != null && !string.IsNullOrEmpty(bannerManagement.bannerLimit.ToString()))
            {
                TextBoxLimit.Text = bannerManagement.bannerLimit.Value.ToString();
            }

            if (bannerManagement.bannerEndDate != null)
                dpEndDate2.SelectedDate
                    = Convert.ToDateTime(bannerManagement.bannerEndDate.Value.ToShortDateString());
            if (bannerManagement.bannerStartDate != null)
                dpStartDate2.SelectedDate =
                    Convert.ToDateTime(bannerManagement.bannerStartDate.Value.ToShortDateString());
            if (bannerManagement.state != null) cbEditState.Checked = bannerManagement.state.Value;
            HiddenFieldBannerId.Value = bannerManagement.Banners.bannersId.ToString();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void Temizle()
    {
        TextBoxLimit.Text = string.Empty;
        LabelReklamAdi.Text = string.Empty;
        TextBoxLimit.Text = string.Empty;
        HiddenFieldBannerId.Value = string.Empty;
        dpStartDate.SelectedDate = null;
        dpStartDate2.SelectedDate = null;
        dpEndDate.SelectedDate = null;
        dpEndDate2.SelectedDate = null;

        DropDownListReklamAlanlari1.SelectedIndex = 0;
        DropDownListReklamlar.SelectedIndex = 0;

        txtLimit.Text = string.Empty;
        cbAddState.Checked = false;
    }

    protected void gvBanners_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var myMastDelete = (ImageButton) e.Row.FindControl("LinkButtonSil");
                myMastDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}