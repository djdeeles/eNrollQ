using System;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_NoticeList : UserControl
{
    private readonly Entities oEntities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbNoticeManagement;
        EntityDataSource1.Where = " it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
        GridView1.Columns[0].HeaderText = AdminResource.lbActions;
        GridView1.Columns[1].HeaderText = AdminResource.lbTitle;
        GridView1.Columns[2].HeaderText = AdminResource.lbStartDate;
        GridView1.Columns[3].HeaderText = AdminResource.lbEndDate;
        GridView1.Columns[4].HeaderText = AdminResource.lbState;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 4))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }

        btnNew.Text = AdminResource.lbNewNotice;

        chkState.Text = AdminResource.lbActive;
        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;
        btnImageSelect.Text = AdminResource.lbImageSelect;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteButton = (ImageButton) e.Row.FindControl("imgBtnDelete");
                deleteButton.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "');";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        var myButton = (ImageButton) sender;
        try
        {
            var oEntities = new Entities();
            var oNotices = oEntities.Notices.Where("it.noticeId=" + myButton.CommandArgument).FirstOrDefault();
            if (oNotices != null)
            {
                if (!string.IsNullOrWhiteSpace(oNotices.thumbnailPath))
                    ImageHelper.DeleteImage(Server.MapPath(oNotices.thumbnailPath.Replace("~", "..")));
                Logger.Add(4, 0, oNotices.noticeId, 2);
                oEntities.DeleteObject(oNotices);
                oEntities.SaveChanges();

                GridView1.DataBind();
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ImageButton2Ara_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var aranan = txtAra.Text;

            EntityDataSource1.Where = "it.System_language.languageId=" +
                                      EnrollAdminContext.Current.DataLanguage.LanguageId.ToString() +
                                      " and (it.header like '%" + aranan + "%' OR it.description like '%" + aranan +
                                      "%')";
            EntityDataSource1.DataBind();
            GridView1.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnSaveUpdateNotice(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(hdnId.Value)) //insert
            {
                var oNotices = new Notices();
                SetDataProccess(oNotices, false);
                oNotices.System_language =
                    oEntities.System_language.Where("it.languageId=" +
                                                    EnrollAdminContext.Current.DataLanguage.LanguageId)
                        .FirstOrDefault();
                oEntities.SaveChanges();
                Logger.Add(4, 0, oNotices.noticeId, 1);

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            else //update
            {
                Notices oNotices = oEntities.Notices.Where("it.noticeId=" + hdnId.Value).FirstOrDefault();
                SetDataProccess(oNotices, true);
                oEntities.SaveChanges();
                if (oNotices != null) Logger.Add(4, 0, oNotices.noticeId, 3);

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }

            GridView1.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        pnlNoticeEdit.Visible = false;
        pnlNoticeList.Visible = true;
        ClearFormInputs();
    }


    private void GetDataProccess(Notices notices)
    {
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        TextBox1.Text = notices.imagePath;
        txtHeader.Text = notices.header;
        radEditor.Content = notices.details;
        if (!String.IsNullOrEmpty(notices.startDate.ToString()))
        {
            dpStartDate.SelectedDate = notices.startDate;
        }
        if (!String.IsNullOrEmpty(notices.stopDate.ToString()))
        {
            dpEndDate.SelectedDate = notices.stopDate;
        }
        txtOzet.Text = notices.description;
        chkState.Checked = Convert.ToBoolean(notices.state);
    }

    private void SetDataProccess(Notices notices, bool deleteOldImage)
    {
        try
        {
            var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
            notices.header = txtHeader.Text;
            notices.details = radEditor.Content;
            notices.state = chkState.Checked;
            notices.description = txtOzet.Text;

            if (!deleteOldImage)
                notices.CreatedTime = DateTime.Now;
            notices.UpdatedTime = DateTime.Now;

            if (dpStartDate.SelectedDate != null)
                notices.startDate = Convert.ToDateTime(dpStartDate.SelectedDate.Value.ToShortDateString());
            if (dpEndDate.SelectedDate != null)
                notices.stopDate = Convert.ToDateTime(dpEndDate.SelectedDate.Value.ToShortDateString());
            else
            {
                notices.stopDate = null;
            }
            if (!String.IsNullOrEmpty(TextBox1.Text))
            {
                if (deleteOldImage && notices.thumbnailPath != string.Empty)
                {
                    ImageHelper.DeleteImage(Server.MapPath(notices.thumbnailPath.Replace("~", "..")));
                }
                notices.imagePath = TextBox1.Text;
                var fi = new System.IO.FileInfo(Server.MapPath(TextBox1.Text.Replace("~", "..")));
                var orj = new Bitmap(Server.MapPath(TextBox1.Text.Replace("~", "..")));
                var i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                var thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + fi.Extension;
                var dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                ImageHelper.SaveJpeg(dest, (Bitmap) i, 75);
                notices.thumbnailPath = "~/FileManager/thumbnails/" + thumbName;
            }
            else
            {
                notices.imagePath = string.Empty;
                notices.thumbnailPath = string.Empty;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }


    protected void btnCancelEditNotice(object sender, EventArgs e)
    {
        pnlNoticeEdit.Visible = false;
        pnlNoticeList.Visible = true;

        ClearFormInputs();
    }

    protected void btnNewNotice(object sender, EventArgs e)
    {
        pnlNoticeEdit.Visible = true;
        pnlNoticeList.Visible = false;
    }

    public void ClearFormInputs()
    {
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        TextBox1.Text = string.Empty;
        txtHeader.Text = string.Empty;
        radEditor.Content = string.Empty;
        dpStartDate.SelectedDate = null;

        dpEndDate.SelectedDate = null;
        txtOzet.Text = string.Empty;
        chkState.Checked = false;

        hdnId.Value = null;
    }

    protected void imgBtnEdit(object sender, ImageClickEventArgs e)
    {
        try
        {
            var btn = sender as ImageButton;
            if (btn != null)
            {
                hdnId.Value = btn.CommandArgument;
                int noticeId = Convert.ToInt32(btn.CommandArgument);
                if (!String.IsNullOrEmpty(hdnId.Value))
                {
                    Notices oNotices = oEntities.Notices.First(p => p.noticeId == noticeId);
                    GetDataProccess(oNotices);

                    pnlNoticeList.Visible = false;
                    pnlNoticeEdit.Visible = true;
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}