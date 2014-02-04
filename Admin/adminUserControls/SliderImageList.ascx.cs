using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_SliderImageList : UserControl
{
    private readonly Entities oEntities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbSliderImageManagement;
        EntityDataSource1.Where = " it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 7))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }
        btnNewSliderImageAdd.Text = AdminResource.lbNewSliderImage;
        gVRef.Columns[0].HeaderText = AdminResource.lbActions;
        gVRef.Columns[1].HeaderText = AdminResource.lbName;
        gVRef.Columns[2].HeaderText = AdminResource.lbLink;
        gVRef.Columns[3].HeaderText = AdminResource.lbState;


        chkState.Text = AdminResource.lbActive;
        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;
        imgBtnImageSelect.Text = AdminResource.lbImageSelect;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteButton = (ImageButton) e.Row.FindControl("imgBtnDelete");
                deleteButton.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "')";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void DeletingProccess(string id)
    {
        try
        {
            var oEntities = new Entities();
            RefLogos refLogos = oEntities.RefLogos.Where("it.id=" + id).FirstOrDefault();

            if (refLogos != null)
            {
                Logger.Add(7, 0, refLogos.id, 2);
                oEntities.DeleteObject(refLogos);
            }
            oEntities.SaveChanges();

            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);

            gVRef.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        var myButton = (ImageButton) sender;
        DeletingProccess(myButton.CommandArgument);
    }

    protected void btnSaveUpdateSliderImage(object sender, EventArgs e)
    {
        try
        {
            RefLogos oRefLogos;
            if (String.IsNullOrEmpty(hdnId.Value)) //insert
            {
                oRefLogos = new RefLogos();
                SetDataProccess(oRefLogos);
                oRefLogos.CreatedTime = DateTime.Now;
                oEntities.AddToRefLogos(oRefLogos);
                oEntities.SaveChanges();

                Logger.Add(7, 0, oRefLogos.id, 1);

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            else //update
            {
                oRefLogos = oEntities.RefLogos.Where("it.id=" + hdnId.Value).FirstOrDefault();
                SetDataProccess(oRefLogos);
                oEntities.SaveChanges();

                if (oRefLogos != null) Logger.Add(7, 0, oRefLogos.id, 3);

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }

            gVRef.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        pnlList.Visible = true;
        pnlEdit.Visible = false;

        ClearFormInputs();
    }

    protected void btnCancelSliderImage(object sender, EventArgs e)
    {
        pnlList.Visible = true;
        pnlEdit.Visible = false;

        ClearFormInputs();
    }

    private void ClearFormInputs()
    {
        txtName.Text = string.Empty;
        txtUrl.Text = string.Empty;
        txtImage.Text = string.Empty;
        chkState.Checked = false;

        hdnId.Value = null;
    }

    protected void btnNewSliderImage(object sender, EventArgs e)
    {
        pnlList.Visible = false;
        pnlEdit.Visible = true;
    }

    protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var btnEdit = sender as ImageButton;
            if (btnEdit != null)
            {
                hdnId.Value = btnEdit.CommandArgument;
                int sliderId = Convert.ToInt32(btnEdit.CommandArgument);
                if (!String.IsNullOrEmpty(hdnId.Value))
                {
                    var oEntities = new Entities();
                    RefLogos oRefLogos = oEntities.RefLogos.First(p => p.id == sliderId);
                    GetDataProccess(oRefLogos);
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        pnlList.Visible = false;
        pnlEdit.Visible = true;
    }

    private void GetDataProccess(RefLogos reflogos)
    {
        txtName.Text = reflogos.name;
        txtUrl.Text = reflogos.url;
        txtImage.Text = reflogos.image;
        chkState.Checked = Convert.ToBoolean(reflogos.state);
    }

    private void SetDataProccess(RefLogos reflogos)
    {
        reflogos.image = txtImage.Text;
        reflogos.name = txtName.Text;
        reflogos.url = txtUrl.Text;
        reflogos.state = chkState.Checked;
        reflogos.UpdatedTime = DateTime.Now;
        reflogos.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
    }
}