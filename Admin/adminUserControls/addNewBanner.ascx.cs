using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class admin_addNewBanner : UserControl
{
    private readonly Entities _ent = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbBanner + " " + AdminResource.lbAdd + " & " + AdminResource.lbEdit;
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

        if (!IsPostBack)
        {
            mvBanner.SetActiveView(vList);
            txtCliktag.Enabled = false;
            ddlDosyaTipi.DataSource = string.Empty;
            ddlDosyaTipi.DataBind();
            var item = new ListItem(AdminResource.lbImage, "1");
            ddlDosyaTipi.Items.Add(item);
            item = new ListItem(AdminResource.lbFlash, "2");
            ddlDosyaTipi.Items.Add(item);
        }
        gvBanners.Columns[0].HeaderText = AdminResource.lbActions;
        gvBanners.Columns[1].HeaderText = AdminResource.lbName;
        gvBanners.Columns[2].HeaderText = AdminResource.lbFileType;

        btnAddNewBanner.Text = AdminResource.lbNewBanner;
        btnCancel.Text = AdminResource.lbCancel;
        btnSave.Text = AdminResource.lbSave;

        btnEditSave.Text = AdminResource.lbSave;
        btnEditCancel.Text = AdminResource.lbCancel;

        btnSelectImage.Text = AdminResource.lbImageSelect;
    }

    protected void btnAddNewBanner_Click(object sender, EventArgs eventArgs)
    {
        mvBanner.SetActiveView(vAdd);
    }

    protected void ddlDosyaTipi_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlDosyaTipi.SelectedValue == "1")
            {
                txtUrl.Enabled = true;

                txtCliktag.Text = string.Empty;
                txtCliktag.Enabled = false;
            }
            if (ddlDosyaTipi.SelectedValue == "2")
            {
                txtUrl.Text = string.Empty;
                txtUrl.Enabled = false;

                txtCliktag.Enabled = true;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GvBannersRowDataBound(object sender, GridViewRowEventArgs e)
    {
        foreach (GridViewRow row in gvBanners.Rows)
        {
            try
            {
                var lbCategory = row.FindControl("lblCategory") as Label;
                if (lbCategory != null)
                {
                    int catId = Convert.ToInt32(lbCategory.Text);
                    lbCategory.Text = _ent.VideoCategories.First(p => p.id == catId).name;
                }
                var btnDelete = row.FindControl("lbCatSil") as ImageButton;
                btnDelete.OnClientClick =
                    "return confirm('" + AdminResource.lbDeletingQuestion + "') ";
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void GvBannersDataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvBanners.Rows)
        {
            try
            {
                var labelDosyaTipi = row.FindControl("LabelDosyaTipi") as Label;
                if (labelDosyaTipi != null)
                {
                    var dosyaTipi = Convert.ToInt32(labelDosyaTipi.Text);
                    switch (dosyaTipi)
                    {
                        case 1:
                            labelDosyaTipi.Text = AdminResource.lbImage;
                            break;
                        case 2:
                            labelDosyaTipi.Text = AdminResource.lbFlash;
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void GvBannersRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Guncelle")
            {
                int bannerId = Convert.ToInt32(e.CommandArgument);
                GetBannerByBannerId(bannerId);

                var banner = _ent.Banners.FirstOrDefault(p => p.bannersId == bannerId);
                if (banner != null)
                {
                    switch (banner.bannerFileTypeId)
                    {
                        case 1:
                            txteClickTag.Enabled = false;
                            txteUrl.Enabled = true;
                            break;
                        case 2:
                            txteClickTag.Enabled = true;
                            txteUrl.Enabled = false;
                            break;
                    }
                }

                mvBanner.SetActiveView(vEdit);
            }
            if (e.CommandName == "Sil")
            {
                int bannerId = Convert.ToInt32(e.CommandArgument);
                var banner = _ent.Banners.FirstOrDefault(p => p.bannersId == bannerId);
                _ent.DeleteObject(banner);
                _ent.SaveChanges();

                Logger.Add(9, 1, bannerId, 2);

                gvBanners.DataBind();
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void GetBannerByBannerId(int bannerId)
    {
        Temizle();
        try
        {
            var bman = _ent.BannerManagement.FirstOrDefault(p => p.bannersId == bannerId);

            if (bman != null)
            {
                LabelDurum.Text = bman.state == true ? AdminResource.lbPublised : AdminResource.lbUnpublised;
                if (bman.bannerCounter != null) LabelGosterimSayisi.Text = bman.bannerCounter.Value.ToString();
            }
            else
            {
                LabelDurum.Text = AdminResource.lbUnpublised;
            }
            var banner = _ent.Banners.FirstOrDefault(p => p.bannersId == bannerId);
            if (banner != null)
            {
                txteAd.Text = banner.bannerName;
                txteUrl.Text = banner.bannerUrl;
                txteHeight.Text = banner.bannerHeight.ToString();
                txteWidth.Text = banner.bannerWidth.ToString();
                HiddenFieldBannerId.Value = banner.bannersId.ToString();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void Temizle()
    {
        LabelDurum.Text = string.Empty;
        LabelGosterimSayisi.Text = string.Empty;
        txteAd.Text = string.Empty;
        txteUrl.Text = string.Empty;
        txteHeight.Text = string.Empty;
        txteWidth.Text = string.Empty;
        HiddenFieldBannerId.Value = string.Empty;
    }

    #region add

    protected void BtnSaveClick(object sender, EventArgs eventArgs)
    {
        try
        {
            var banner = new Banners();
            banner.bannerName = txtAd.Text;
            banner.bannerUrl = txtUrl.Text;
            banner.bannerFileTypeId = Convert.ToInt32(ddlDosyaTipi.SelectedValue);
            banner.bannerHeight = Convert.ToInt32(txtHeight.Text);
            banner.bannerWidth = Convert.ToInt32(txtWidth.Text);
            if (ddlDosyaTipi.SelectedValue == "2")
                banner.bannerSource = txtPath.Text + txtCliktag.Text;
            else
                banner.bannerSource = txtPath.Text;
            banner.CreatedTime = DateTime.Now;
            banner.UpdatedTime = DateTime.Now;
            _ent.AddToBanners(banner);
            _ent.SaveChanges();

            Logger.Add(9, 1, banner.bannersId, 1);

            gvBanners.DataBind();
            foreach (Control ct in vAdd.Controls)
            {
                if (ct is TextBox)
                    (ct as TextBox).Text = string.Empty;
            }
            MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            mvBanner.SetActiveView(vList);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnCancelClick(object sender, EventArgs eventArgs)
    {
        mvBanner.SetActiveView(vList);
    }

    #endregion

    #region edit

    protected void BtEditSaveClick(object sender, EventArgs eventArgs)
    {
        try
        {
            var bannerId = Convert.ToInt32(HiddenFieldBannerId.Value);
            var banner = _ent.Banners.First(p => p.bannersId == bannerId);
            banner.bannerName = txteAd.Text;
            banner.bannerUrl = txteUrl.Text;
            banner.bannerHeight = Convert.ToInt32(txteHeight.Text);
            banner.bannerWidth = Convert.ToInt32(txteWidth.Text);
            banner.UpdatedTime = DateTime.Now;
            _ent.SaveChanges();

            Logger.Add(9, 1, banner.bannersId, 3);

            gvBanners.DataBind();
            mvBanner.SetActiveView(vList);
            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnEditCancelClick(object sender, EventArgs eventArgs)
    {
        mvBanner.SetActiveView(vList);
    }

    #endregion
}