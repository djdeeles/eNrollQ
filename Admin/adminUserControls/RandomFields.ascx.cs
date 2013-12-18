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

public partial class Admin_adminUserControls_RandomFields : UserControl
{
    Entities _entities = new Entities();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 37))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        gvRandomFields.Columns[0].HeaderText = AdminResource.lbActions;
        gvRandomFields.Columns[1].HeaderText = AdminResource.lbFieldName;
        gvRandomFields.Columns[2].HeaderText = AdminResource.lbState;

        EntityDataSource1.Where = string.Format(" it.languageId={0} ",EnrollAdminContext.Current.DataLanguage.LanguageId.ToString());

        btNewRandomField.Text = AdminResource.lbNewRandomField;
        imBtSave.Text = AdminResource.lbSave;
        imBtCancel.Text = AdminResource.lbCancel;
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbRandomFields;
    }

    protected void ImBtSaveClick(object sender, EventArgs eventArgs)
    {
        try
        { 
            if (!string.IsNullOrWhiteSpace(hdnId.Value))
            {
                var customerRandomId = Convert.ToInt32(hdnId.Value);

                var customerRandom = _entities.Customer_Random.FirstOrDefault(p => p.Id == customerRandomId);
                if (customerRandom!=null)
                {
                    SetDataProccess(customerRandom);
                    _entities.SaveChanges();

                    Logger.Add(37, 1, customerRandom.Id, 3);

                    gvRandomFields.DataBind();
                    MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);

                    pnlRandomFieldEdit.Visible = false;
                    pnlNewList.Visible = true;
                    ClearFormInputs();   
                }
                else
                {
                    MessageBox.Show(MessageType.Success, AdminResource.msgAnErrorOccurred);
                }
            }
            else
            {
                var customerRandom = new Customer_Random();
                customerRandom.CreatedTime = DateTime.Now;
                customerRandom.LanguageId = EnrollAdminContext.Current.AdminLanguage.LanguageId;
                SetDataProccess(customerRandom);
                _entities.AddToCustomer_Random(customerRandom);
                _entities.SaveChanges();

                Logger.Add(37, 1, customerRandom.Id, 1);
                gvRandomFields.DataBind();
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                
                pnlRandomFieldEdit.Visible = false;
                pnlNewList.Visible = true;
                ClearFormInputs();   
            }


        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ImgBtnCancel(object sender, EventArgs e)
    {
        pnlRandomFieldEdit.Visible = false;
        pnlNewList.Visible = true;
        ClearFormInputs();
    }

    protected void ImgBtnDeleteClick(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        if (btn != null)
        {
            hdnId.Value = btn.CommandArgument;
            var id = Convert.ToInt32(hdnId.Value);
            var randomField = _entities.Customer_Random.FirstOrDefault(p => p.Id == id);
            if (randomField != null)
            {
                _entities.DeleteObject(randomField);
                _entities.SaveChanges();
                gvRandomFields.DataBind();
                Logger.Add(37, 1, id, 2);
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            }
            else
            {
                MessageBox.Show(MessageType.Warning, AdminResource.msgAnErrorOccurred);
            }
        }
    }

    protected void ImgBtnEditClick(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        if (btn != null)
        {
            hdnId.Value = btn.CommandArgument;
            var id = Convert.ToInt32(hdnId.Value);
            var randomField = _entities.Customer_Random.FirstOrDefault(p => p.Id == id);
            if(randomField!=null)
            {
                hdnId.Value = btn.CommandArgument;
                GetDataProccess(randomField); 
                pnlNewList.Visible = false;
                pnlRandomFieldEdit.Visible = true;    
            }
            else
            {
                MessageBox.Show(MessageType.Warning, AdminResource.msgAnErrorOccurred);
            } 
        }
    }
     
    private void GetDataProccess(Customer_Random data)
    {
        var radEditorContent = ((RadEditor)Rtb1.FindControl("RadEditor1"));
        var radEditorSummary = ((RadEditor)Rtb2.FindControl("RadEditor1"));
        radEditorContent.Content = data.Text;
        radEditorSummary.Content = data.Summary;
        txtHeader.Text = data.Title;
        cbState.Checked = data.State;
    }

    private void SetDataProccess(Customer_Random data)
    {
        var radEditorContent = ((RadEditor)Rtb1.FindControl("RadEditor1"));
        var radEditorSummary = ((RadEditor)Rtb2.FindControl("RadEditor1"));
        data.Text = radEditorContent.Content;
        data.Summary = radEditorSummary.Content;
        data.Title = txtHeader.Text;
        data.UpdatedTime = DateTime.Now;
        data.State = cbState.Checked;
    }

    public void ClearFormInputs()
    {
        hdnId.Value = null;
        txtHeader.Text = string.Empty;
        var radEditorContent = ((RadEditor)Rtb1.FindControl("RadEditor1"));
        var radEditorSummary = ((RadEditor)Rtb2.FindControl("RadEditor1"));
        radEditorContent.Content = string.Empty;
        radEditorSummary.Content = string.Empty;
        cbState.Checked = false;
    }

    protected void BtnNewRandomFieldOnClick(object sender, EventArgs e)
    {
        ClearFormInputs();
        hdnId.Value = null;
        pnlNewList.Visible = false;
        pnlRandomFieldEdit.Visible = true;
    }

    protected void gvRandomFields_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        var imgBtnDelete = (ImageButton)e.Row.FindControl("imgBtnDelete");
        if(imgBtnDelete!=null)
        {
            imgBtnDelete.OnClientClick = "return confirm('"+AdminResource.lbDeletingQuestion+"');";
        }
    }

}