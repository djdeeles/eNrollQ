using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_StaticField : UserControl
{
    private readonly Entities _entities = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 16))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        gvStaticFields.Columns[0].HeaderText = AdminResource.lbActions;
        gvStaticFields.Columns[1].HeaderText = AdminResource.lbFieldName;

        EntityDataSource1.Where = " it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();

        imBtSave.Text = AdminResource.lbSave;
        imBtCancel.Text = AdminResource.lbCancel;
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbStaticFieldManagement;
    }

    private void GetDataProccess(Customer_Special data)
    {
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        radEditor.Content = data.text;
        txtHeader.Text = data.header;
    }

    private void SetDataProccess(Customer_Special data)
    {
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        data.text = radEditor.Content;
        data.header = txtHeader.Text;
        data.UpdatedTime = DateTime.Now;
        data.header = txtHeader.Text;
    }


    protected void imBtSave_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            var oEntities = new Entities();

            if (hdnId != null)
            {
                var specialId = Convert.ToInt32(hdnId.Value);

                Customer_Special customerSpecial = oEntities.Customer_Special.First(p => p.specialId == specialId);
                SetDataProccess(customerSpecial);
                oEntities.SaveChanges();

                Logger.Add(38, 0, customerSpecial.specialId, 3);

                gvStaticFields.DataBind();

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);

                pnlSpecialFieldEdit.Visible = false;
                pnlNewList.Visible = true;
                ClearFormInputs();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void imgBtnCancel(object sender, EventArgs e)
    {
        pnlSpecialFieldEdit.Visible = false;
        pnlNewList.Visible = true;
        ClearFormInputs();
    }

    protected void imgBtnEditClick(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        if (btn != null)
        {
            var specialId = Convert.ToInt32(btn.CommandArgument);
            var specialField = _entities.Customer_Special.First(p => p.specialId == specialId);
            hdnId.Value = btn.CommandArgument;
            GetDataProccess(specialField);
            pnlNewList.Visible = false;
            pnlSpecialFieldEdit.Visible = true;
        }
    }

    public void ClearFormInputs()
    {
        hdnId.Value = null;
    }
}