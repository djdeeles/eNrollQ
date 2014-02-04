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

public partial class Admin_adminUserControls_DynamicField : UserControl
{
    private readonly Entities _entities = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 16))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }

        imgBtnNew.Text = AdminResource.lbNewField;
        imgBtnNew.CssClass = "NewBtn";
        GridView1.Columns[0].HeaderText = AdminResource.lbActions;
        GridView1.Columns[1].HeaderText = AdminResource.lbName;
        GridView1.Columns[2].HeaderText = AdminResource.lbState;
        GridView1.Columns[3].HeaderText = AdminResource.lbOptions;

        EntityDataSource1.Where = " it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();


        btnCustomerDynamicSaveUpdate.Text = AdminResource.lbSave;
        btnCustomerDynamicEditCancel.Text = AdminResource.lbCancel;

        btnCustomerDynamicGroupSaveUpdate.Text = AdminResource.lbSave;
        btnCustomerDynamicGroupEditCancel.Text = AdminResource.lbCancel;

        chkState.Text = AdminResource.lbActive;
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbDynamicFieldManagement;
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                var myButton = (Button) e.Row.FindControl("imgBtnInnerNew");
                myButton.Text = AdminResource.lbNewPage;
                myButton.CssClass = "NewBtn";

                var deleteButton = (ImageButton) e.Row.FindControl("imgBtnDelete");
                deleteButton.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "'); ";

                var gr = ((GridView) e.Row.FindControl("grdSecenek"));

                gr.Columns[0].HeaderText = AdminResource.lbOptions;
                gr.Columns[1].HeaderText = AdminResource.lbName;
                gr.EmptyDataText = AdminResource.lbNoRecord;

                EntityDataSource3.Where = "it.groupId=" + myButton.CommandArgument;
                gr.DataSource = EntityDataSource3;
                gr.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void DeletingProccess(string id)
    {
        try
        {
            int groupId = Convert.ToInt32(id);
            var oGroup = _entities.Customer_Dynamic_Group.FirstOrDefault(p => p.groupId == groupId);

            //önce alt datalar silinir.
            foreach (var dynamic in _entities.Customer_Dynamic.Where(p => p.groupId == groupId))
            {
                _entities.DeleteObject(dynamic);
            }
            Logger.Add(16, 2, groupId, 2);
            _entities.DeleteObject(oGroup);
            _entities.SaveChanges();
            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            GridView1.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }


    protected void grdSecenek_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var innerDeleteButton = (ImageButton) e.Row.FindControl("imgBtnInerDelete");
                innerDeleteButton.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void ClearFormInputs()
    {
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        var radEditor2 = ((RadEditor) Rtb_CustomerDynamic_details.FindControl("RadEditor1"));

        txtName.Text = string.Empty;
        chkState.Checked = false;
        radEditor.Content = string.Empty;

        radEditor2.Content = string.Empty;
        txtCustomerDynamic_name.Text = string.Empty;

        hfDynamicGroupId.Value = null;
        hfDynamicId.Value = null;
    }

    #region customer dynamic group {save, edit, update, delete, cancel}

    //new 
    protected void btnNewDynamicGroupList(object sender, EventArgs e)
    {
        pnlNewList.Visible = false;
        pnlNewDynamicGroup.Visible = true;
    }

    //save, update
    protected void btnCustomerDynamicGroupSaveUpdateClick(object sender, EventArgs e)
    {
        try
        {
            Customer_Dynamic_Group group = null;
            int groupId = 0;

            var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));

            if (!string.IsNullOrWhiteSpace(hfDynamicGroupId.Value))
            {
                groupId = Convert.ToInt32(hfDynamicGroupId.Value);

                group = _entities.Customer_Dynamic_Group.FirstOrDefault(p => p.groupId == groupId);
                group.UpdatedTime = DateTime.Now;
                group.name = txtName.Text;
                group.details = radEditor.Content;
                group.state = chkState.Checked;
                group.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                _entities.SaveChanges();
                Logger.Add(16, 2, group.groupId, 3);

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            else
            {
                group = new Customer_Dynamic_Group();
                group.CreatedTime = DateTime.Now;
                _entities.AddToCustomer_Dynamic_Group(group);
                group.UpdatedTime = DateTime.Now;
                group.name = txtName.Text;
                group.details = radEditor.Content;
                group.state = chkState.Checked;
                group.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                _entities.SaveChanges();
                Logger.Add(16, 2, group.groupId, 1);

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }

            GridView1.DataBind();

            pnlNewDynamicGroup.Visible = false;
            pnlNewList.Visible = true;

            ClearFormInputs();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    //cancel
    protected void btnCustomerDynamicGroupEditCancel_Click(object sender, EventArgs e)
    {
        ClearFormInputs();
        pnlNewDynamicGroup.Visible = false;
        pnlNewList.Visible = true;
    }

    //edit
    protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        if (btn != null)
        {
            var groupId = Convert.ToInt32(btn.CommandArgument);
            hfDynamicGroupId.Value = btn.CommandArgument;

            Customer_Dynamic_Group dynamicGroup = _entities.Customer_Dynamic_Group.First(p => p.groupId == groupId);

            txtName.Text = dynamicGroup.name;
            var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
            radEditor.Content = dynamicGroup.details;
            if (dynamicGroup.state != null)
                chkState.Checked = Convert.ToBoolean(dynamicGroup.state.Value);
            else
                chkState.Checked = false;

            pnlNewList.Visible = false;
            pnlNewDynamicGroup.Visible = true;
        }
    }

    //delete
    protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        var myButton = (ImageButton) sender;
        DeletingProccess(myButton.CommandArgument);
    }

    #endregion

    #region customer dynamic group page {save, edit, update, delete, cancel}

    //new
    protected void btnNewOptions_Click(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            hfDynamicGroupId.Value = btn.CommandArgument;
        }
        pnlNewDynamicGroup.Visible = false;
        pnlNewList.Visible = false;
        pnlCustomerDynamic.Visible = true;
    }

    //save, update
    protected void btnCustomerDynamicSaveUpdateClick(object sender, EventArgs e)
    {
        try
        {
            Customer_Dynamic customerDynamic = null;
            int dynamicId = 0;

            var radEditor = ((RadEditor) Rtb_CustomerDynamic_details.FindControl("RadEditor1"));

            if (!string.IsNullOrWhiteSpace(hfDynamicId.Value))
            {
                dynamicId = Convert.ToInt32(hfDynamicId.Value);

                customerDynamic = _entities.Customer_Dynamic.First(p => p.dynamicId == dynamicId);
                customerDynamic.UpdatedTime = DateTime.Now;
                customerDynamic.name = txtCustomerDynamic_name.Text;
                customerDynamic.details = radEditor.Content;
                _entities.SaveChanges();
                Logger.Add(16, 2, dynamicId, 3);

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            else
            {
                customerDynamic = new Customer_Dynamic();
                customerDynamic.CreatedTime = DateTime.Now;
                customerDynamic.UpdatedTime = DateTime.Now;
                customerDynamic.name = txtCustomerDynamic_name.Text;
                customerDynamic.details = radEditor.Content;
                if (!string.IsNullOrWhiteSpace(hfDynamicGroupId.Value))
                {
                    customerDynamic.groupId = Convert.ToInt32(hfDynamicGroupId.Value);
                }
                _entities.AddToCustomer_Dynamic(customerDynamic);
                _entities.SaveChanges();
                Logger.Add(16, 2, customerDynamic.dynamicId, 1);

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }

            GridView1.DataBind();

            pnlNewDynamicGroup.Visible = false;
            pnlCustomerDynamic.Visible = false;
            pnlNewList.Visible = true;

            ClearFormInputs();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    //cancel
    protected void btnCustomerDynamicEditCancel_Click(object sender, EventArgs e)
    {
        pnlCustomerDynamic.Visible = false;
        pnlNewDynamicGroup.Visible = false;
        pnlNewList.Visible = true;

        ClearFormInputs();
    }

    //edit
    protected void imgBtnEditOptions_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        if (btn != null)
        {
            var dynamicId = Convert.ToInt32(btn.CommandArgument);
            hfDynamicId.Value = btn.CommandArgument;

            Customer_Dynamic dynamic = _entities.Customer_Dynamic.First(p => p.dynamicId == dynamicId);

            txtCustomerDynamic_name.Text = dynamic.name;
            var radEditor = ((RadEditor) Rtb_CustomerDynamic_details.FindControl("RadEditor1"));
            radEditor.Content = dynamic.details;

            pnlNewList.Visible = false;
            pnlNewDynamicGroup.Visible = false;
            pnlCustomerDynamic.Visible = true;
        }
    }

    //delete
    protected void imgBtnInerDeleteOptions_Click(object sender, ImageClickEventArgs e)
    {
        var myImage = (ImageButton) sender;
        try
        {
            var dynamicId = Convert.ToInt32(myImage.CommandArgument);
            var oDynamic = _entities.Customer_Dynamic.FirstOrDefault(p => p.dynamicId == dynamicId);
            Logger.Add(16, 3, dynamicId, 2);
            _entities.DeleteObject(oDynamic);
            _entities.SaveChanges();

            GridView1.DataBind();
            pnlNewDynamicGroup.Visible = false;
            pnlCustomerDynamic.Visible = false;
            pnlNewList.Visible = true;

            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    #endregion
}