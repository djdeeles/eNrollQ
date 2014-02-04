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

public partial class Admin_adminUserControls_TemplatePageList : UserControl
{
    private readonly Entities _entities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbTemplateManagement;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 18))
        {
            mvAuth.ActiveViewIndex = 0;
            mvTemplatePageList.SetActiveView(vGrid);
            gVTemplates.DataBind();
            if (IsPostBack)
            {
                txtName.Enabled = false;
                if (!string.IsNullOrWhiteSpace(hdnId.Value))
                {
                    var editId = Convert.ToInt32(hdnId.Value);
                    var page = _entities.TemplatePages.FirstOrDefault(p => p.id == editId);
                    if (page != null)
                    {
                        BindDataProccess(page);
                    }
                }
            }
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        var panel = (Panel) Rtb1.FindControl("ChooseTemplate");
        panel.Visible = true;

        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;

        btnNew.Text = AdminResource.lbNewTemplate;
        gVTemplates.Columns[0].HeaderText = AdminResource.lbActions;
        gVTemplates.Columns[1].HeaderText = AdminResource.lbTemplateName;
    }

    protected void ImgBtnEditClick(object sender, ImageClickEventArgs e)
    {
        var btnEdit = sender as ImageButton;
        if (btnEdit != null)
        {
            try
            {
                hdnId.Value = btnEdit.CommandArgument;
                var templateId = Convert.ToInt32(hdnId.Value);
                var tempPage = _entities.TemplatePages.FirstOrDefault(p => p.id == templateId);
                if (tempPage != null)
                {
                    BindDataProccess(tempPage);
                    mvTemplatePageList.SetActiveView(vAddEdit);
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbNotFound);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void GridView1RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //var myButton = (ImageButton)e.Row.FindControl("btnEdit");
                //myButton.OnClientClick = "window.open('EditContent.aspx?content=TemplatePageEdit&Id=" +
                //                         myButton.CommandArgument +
                //                         "','','width=800, height=680,scrollbars=yes,menubar=no,titlebar=no,resizable=yes');return false;";
                var deleteButton = (ImageButton) e.Row.FindControl("imgBtnDelete");

                if (deleteButton.CommandArgument != null)
                {
                    var tempId = Convert.ToInt32(deleteButton.CommandArgument);
                    var control = _entities.System_menu.Count(p => p.thema == tempId);
                    if (control > 0)
                    {
                        deleteButton.OnClientClick = string.Format("javascript:{0}('{1}');return false;",
                                                                   MessageType.Notice, AdminResource.msgTemplateUsing);
                    }
                    else
                    {
                        deleteButton.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ImgBtnDeleteClick(object sender, ImageClickEventArgs e)
    {
        var myButton = (ImageButton) sender;
        var data = myButton.CommandArgument;
        if (!string.IsNullOrWhiteSpace(data))
        {
            try
            {
                var id = Convert.ToInt32(data);
                if (id > 0)
                {
                    var templatePage = _entities.TemplatePages.FirstOrDefault(p => p.id == id);
                    if (templatePage != null)
                    {
                        _entities.DeleteObject(templatePage);
                        _entities.SaveChanges();
                        Logger.Add(18, 0, templatePage.id, 2);
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                        gVTemplates.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, AdminResource.lbNotFound);
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }
        else
        {
            MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
        }
    }

    private void BindDataProccess(TemplatePages pages)
    {
        txtName.Text = pages.name;
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        radEditor.Content = pages.Details;
    }

    private void SaveDataProccess(TemplatePages pages)
    {
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        pages.Details = radEditor.Content;
        pages.UpdatedTime = DateTime.Now;
    }

    protected void BtnNewClick(object sender, EventArgs e)
    {
        ClearForm();
        hdnId.Value = string.Empty;
        mvTemplatePageList.SetActiveView(vAddEdit);
        txtName.Enabled = true;
    }

    protected void BtnCancelClick(object sender, EventArgs e)
    {
        ClearForm();
        mvTemplatePageList.SetActiveView(vGrid);
    }

    public void ClearForm()
    {
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        radEditor.Content = string.Empty;
        txtName.Text = string.Empty;
    }

    protected void BtnSaveClick(object sender, EventArgs e)
    {
        try
        {
            var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));

            if (String.IsNullOrEmpty(hdnId.Value)) //insert
            {
                var oPages = new TemplatePages();
                oPages.name = txtName.Text;
                oPages.CreatedTime = DateTime.Now;
                oPages.UpdatedTime = DateTime.Now;
                oPages.Details = radEditor.Content;

                _entities.AddToTemplatePages(oPages);
                _entities.SaveChanges();
                Logger.Add(18, 0, oPages.id, 1);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                gVTemplates.DataBind();
            }
            else //update
            {
                var id = Convert.ToInt32(hdnId.Value);
                if (id > 0)
                {
                    TemplatePages oPages = _entities.TemplatePages.FirstOrDefault(p => p.id == id);
                    if (oPages != null)
                    {
                        SaveDataProccess(oPages);
                        _entities.SaveChanges();
                        Logger.Add(18, 0, oPages.id, 3);
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        gVTemplates.DataBind();
                    }
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbNotFound);
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}