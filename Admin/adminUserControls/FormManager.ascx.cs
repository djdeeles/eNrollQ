using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class FormManager : UserControl
    {
        private readonly Entities _entities = new Entities();
        public int FormId = 0;

        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbFormManagement;
            if (!IsPostBack)
            {
                try
                {
                    HiddenField1.Value = EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
                    AddDdlItems();
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
                //btNewForm.Text = "Yeni Form Oluştur";

                pBtnAddNewForm.Visible = true;
                pAddNewForm.Visible = false;
                pForms.Visible = true;
                pFormContents.Visible = false;

                btNewForm.Text = AdminResource.lbNewForm;
                cbFormState.Text = AdminResource.lbActive;
                btSaveNewForm.Text = AdminResource.lbSave;
                btCancelNewForm.Text = AdminResource.lbCancel;

                btSaveFormContent.Text = AdminResource.lbSave;
                btCancelFormContent.Text = AdminResource.lbClose;

                gvForms.Columns[0].HeaderText = AdminResource.lbActions;
                gvForms.Columns[1].HeaderText = AdminResource.lbName;
                gvForms.Columns[2].HeaderText = AdminResource.lbEmail;
                gvForms.Columns[3].HeaderText = AdminResource.lbState;

                gvFormContents.Columns[0].HeaderText = AdminResource.lbActions;
                gvFormContents.Columns[1].HeaderText = AdminResource.lbIndex;
                gvFormContents.Columns[2].HeaderText = AdminResource.lbRequiredField;
                gvFormContents.Columns[3].HeaderText = AdminResource.lbOptions;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 24))
            {
                MultiView2.ActiveViewIndex = 0;
            }
            else
            {
                MultiView2.ActiveViewIndex = 1;
            }
        }

        public string GetFormContentElementsDeleteButton(int contentId, int type)
        {
            List<FormContentOptions> options;
            var builder = new StringBuilder();
            FormId = _entities.FormContents.First(p => p.Id == contentId).FormId;
            switch (type)
            {
                case 4:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    if (options.Count > 0)
                        builder.AppendFormat(
                            "<a onclick='removeSelectOption(\"{2}\",\"{3}\",\"form_{0}_{1}\", \"{5}\")' style='cursor:pointer;'>" +
                            "<img src='{4}' width='16px' /></a>",
                            type, contentId, Crypto.Encrypt(FormId.ToString()), Crypto.Encrypt(contentId.ToString()),
                            "images/icon/cop.png", AdminResource.lbDeletingQuestion);
                    break;
                case 5:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    if (options.Count > 0)
                        builder.AppendFormat(
                            "<a onclick='removeRadioOption(\"{2}\",\"{3}\",\"form_{0}_{1}\", \"{5}\")' style='cursor:pointer;'>" +
                            "<img src='{4}' width='16px'/></a>", type, contentId, Crypto.Encrypt(FormId.ToString()),
                            Crypto.Encrypt(contentId.ToString()), "images/icon/cop.png",
                            AdminResource.lbDeletingQuestion);
                    break;
                case 6:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    if (options.Count > 0)
                        builder.AppendFormat(
                            "<a onclick='removeCheckOption(\"{2}\",\"{3}\",\"form_{0}_{1}\", \"{5}\")' style='cursor:pointer;'>" +
                            "<img src='{4}' width='16px' /></a>", type, contentId, Crypto.Encrypt(FormId.ToString()),
                            Crypto.Encrypt(contentId.ToString()), "images/icon/cop.png",
                            AdminResource.lbDeletingQuestion);
                    break;
                default:
                    break;
            }

            return builder.ToString();
        }

        public string GetFormContentElement(int contentId, int type)
        {
            List<FormContentOptions> options;
            var builder = new StringBuilder();
            FormId = _entities.FormContents.First(p => p.Id == contentId).FormId;
            switch (type)
            {
                case 1:
                    builder.AppendFormat("<input type='text' name='form_{0}_{1}' ></input>", type, contentId);
                    break;
                case 2:
                    builder.AppendFormat("<textarea name='form_{0}_{1}'></textarea>", type, contentId);
                    break;
                case 3:
                    builder.AppendFormat("<input type='file' name='form_{0}_{1}'>", type, contentId);
                    break;
                case 4:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    builder.AppendFormat("<select id='form_{0}_{1}' name='form_{0}_{1}' >",
                                         type, contentId);
                    foreach (var option in options)
                    {
                        builder.AppendFormat("<option value='{0}'>{0}</option>", option.Value);
                    }
                    builder.AppendLine("</select>");
                    break;
                case 5:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    foreach (var option in options)
                    {
                        builder.AppendFormat("<div><input type='radio' name='form_{1}_{2}' value='{0}'>{0}<br></div>",
                                             option.Value, type, contentId);
                    }
                    break;
                case 6:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    foreach (var option in options)
                    {
                        builder.AppendFormat(
                            "<div><input type='checkbox' name='form_{1}_{2}' value='{0}'>{0}<br></div>", option.Value,
                            type, contentId);
                    }
                    break;
                case 7:
                    builder.AppendFormat("<input type='text' name='form_{0}_{1}' ></input>", type, contentId);
                    break;
                default:
                    break;
            }

            return builder.ToString();
        }

        public void AddDdlItems()
        {
            ClearDdlItems(ddlFieldType);
            ddlFieldType.Items.Add(new ListItem(AdminResource.lbTextBox, "1"));
            ddlFieldType.Items.Add(new ListItem(AdminResource.lbEmail, "7"));
            ddlFieldType.Items.Add(new ListItem(AdminResource.lbMultiLineTextBox, "2"));
            ddlFieldType.Items.Add(new ListItem(AdminResource.lbFileUpload, "3"));
            ddlFieldType.Items.Add(new ListItem(AdminResource.lbDropDownList, "4"));
            ddlFieldType.Items.Add(new ListItem(AdminResource.lbRadioButton, "5"));
            ddlFieldType.Items.Add(new ListItem(AdminResource.lbCheckBox, "6"));
        }

        public void ClearDdlItems(DropDownList ddlList)
        {
            int count = ddlList.Items.Count;
            for (int i = 0; i < count; i++)
            {
                ddlList.Items.RemoveAt(0);
            }
        }

        public void ClearFormInputs()
        {
            lbSelectedFormName.Text = string.Empty;
            tbFieldName.Text = string.Empty;
            cbRequiredField.Checked = false;
            TextBoxSiraNo.Text = string.Empty;
        }

        #region FormContentActions

        protected void gvFormContents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Update")
                {
                    int id = int.Parse(e.CommandArgument.ToString());

                    FormContents formContents = _entities.FormContents.First(p => p.Id == id);

                    var v = (GridView) sender;
                    var tbName = (TextBox) gvFormContents.Rows[v.EditIndex].FindControl("tbFormContentName");
                    formContents.FieldName = tbName.Text;


                    var tbOrderId = (TextBox) gvFormContents.Rows[v.EditIndex].FindControl("tbOrderId");
                    int orderNo = Convert.ToInt32(tbOrderId.Text);

                    //sıra numarası güncellendiyse
                    var faqUpdate =
                        _entities.FormContents.Where(p => p.FormId == formContents.FormId && p.OrderId != orderNo).
                            ToList();
                    if (faqUpdate.Count > 0)
                    {
                        //seçtiği sıra numarası db de varsa
                        var update =
                            _entities.FormContents.Where(p => p.FormId == formContents.FormId && p.OrderId == orderNo).
                                ToList();
                        if (update.Count > 0)
                        {
                            var contentList =
                                _entities.FormContents.Where(
                                    p => p.FormId == formContents.FormId && p.OrderId >= orderNo).ToList();
                            foreach (FormContents item in contentList)
                            {
                                item.OrderId++;
                            }
                        }
                    }

                    formContents.UpdatedTime = DateTime.Now;

                    _entities.SaveChanges();
                    formContents.OrderId = orderNo;
                    _entities.SaveChanges();

                    Logger.Add(24, 2, formContents.Id, 3);

                    MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                }

                else if (e.CommandName == "AddNewOption")
                {
                    string commandArgs = e.CommandArgument.ToString();
                    var data = commandArgs.Split('|');
                    int rowIndex = Convert.ToInt32(data[0]);
                    int contentId = Convert.ToInt32(data[1]);
                    var imgBtnAddNewOption =
                        (ImageButton) gvFormContents.Rows[rowIndex].FindControl("imgBtnAddNewOption");
                    var txtNewOption = (TextBox) gvFormContents.Rows[rowIndex].FindControl("txtNewOption");
                    var btnSaveOption = (Button) gvFormContents.Rows[rowIndex].FindControl("btnSaveOption");
                    var btnCancelOption = (Button) gvFormContents.Rows[rowIndex].FindControl("btnCancelOption");
                    imgBtnAddNewOption.Visible = true;
                    txtNewOption.Visible = true;
                    btnSaveOption.Visible = true;
                    btnCancelOption.Visible = true;
                }

                else if (e.CommandName == "CancelAddNewOption")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    var imgBtnAddNewOption =
                        (ImageButton) gvFormContents.Rows[rowIndex].FindControl("imgBtnAddNewOption");
                    var txtNewOption = (TextBox) gvFormContents.Rows[rowIndex].FindControl("txtNewOption");
                    var btnSaveOption = (Button) gvFormContents.Rows[rowIndex].FindControl("btnSaveOption");
                    var btnCancelOption = (Button) gvFormContents.Rows[rowIndex].FindControl("btnCancelOption");

                    imgBtnAddNewOption.Visible = true;
                    txtNewOption.Visible = false;
                    btnSaveOption.Visible = false;
                    btnCancelOption.Visible = false;
                }
                else if (e.CommandName == "SaveAddNewOption")
                {
                    string commandArgs = e.CommandArgument.ToString();
                    var data = commandArgs.Split('|');
                    int rowIndex = Convert.ToInt32(data[0]);
                    int contentId = Convert.ToInt32(data[1]);

                    var imgBtnAddNewOption =
                        (ImageButton) gvFormContents.Rows[rowIndex].FindControl("imgBtnAddNewOption");
                    var txtNewOption = (TextBox) gvFormContents.Rows[rowIndex].FindControl("txtNewOption");
                    var btnSaveOption = (Button) gvFormContents.Rows[rowIndex].FindControl("btnSaveOption");
                    var btnCancelOption = (Button) gvFormContents.Rows[rowIndex].FindControl("btnCancelOption");

                    var options = new FormContentOptions();
                    options.Value = txtNewOption.Text;
                    options.FormContentId = contentId;

                    options.CreatedTime = DateTime.Now;

                    _entities.AddToFormContentOptions(options);
                    _entities.SaveChanges();

                    Logger.Add(24, 3, options.Id, 1);

                    imgBtnAddNewOption.Visible = true;
                    txtNewOption.Visible = false;
                    btnSaveOption.Visible = false;
                    btnCancelOption.Visible = false;
                    gvFormContents.DataBind();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void gvFormContents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e != null && e.Row.RowType == DataControlRowType.DataRow)
                {
                    var deleteBtn = (ImageButton) e.Row.FindControl("gvFormContentsSil");
                    var addOptionBtn = (ImageButton) e.Row.FindControl("imgBtnAddNewOption");
                    if (deleteBtn != null)
                    {
                        deleteBtn.OnClientClick =
                            " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
                        deleteBtn.ToolTip = AdminResource.lbDelete;
                    }
                    addOptionBtn.ToolTip = AdminResource.lbAddProperty;

                    var btnSaveOption = (Button) e.Row.FindControl("btnSaveOption");
                    var btnCancelOption = (Button) e.Row.FindControl("btnCancelOption");
                    btnSaveOption.Text = AdminResource.lbAdd;
                    btnCancelOption.Text = AdminResource.lbCancel;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void gvFormContentsSil_Click(object sender, ImageClickEventArgs e)
        {
            var lbFormContentDelete = sender as ImageButton;
            if (lbFormContentDelete != null)
            {
                var formContentId = Convert.ToInt32(lbFormContentDelete.CommandArgument);
                using (var ent = new Entities())
                {
                    try
                    {
                        var formContent = ent.FormContents.First(p => p.Id == formContentId);
                        var formContentOptions =
                            ent.FormContentOptions.Where(p => p.FormContentId == formContent.Id).ToList();
                        foreach (var options in formContentOptions)
                        {
                            ent.DeleteObject(options);
                        }
                        ent.SaveChanges();
                        ent.DeleteObject(formContent);
                        ent.SaveChanges();

                        Logger.Add(24, 2, formContent.Id, 2);

                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.ManageException(exception);
                    }
                }
            }
            gvFormContents.DataBind();
        }

        protected void btSaveFormContent_Click(object sender, EventArgs e)
        {
            var contents = new FormContents();
            contents.FieldName = tbFieldName.Text;
            contents.FieldType = Convert.ToInt32(ddlFieldType.SelectedValue);
            FormId = Convert.ToInt32(hfFormId.Value);
            contents.FormId = FormId;

            int orderNo = Convert.ToInt32(TextBoxSiraNo.Text);
            contents.OrderId = orderNo;
            var faqUpdate = _entities.FormContents.Where(p => p.FormId == FormId && p.OrderId == orderNo).ToList();
            if (faqUpdate.Count > 0)
            {
                var contentList = _entities.FormContents.Where(p => p.FormId == FormId && p.OrderId >= orderNo).ToList();
                foreach (FormContents content in contentList)
                {
                    content.OrderId++;
                }
            }

            contents.CreatedTime = DateTime.Now;
            contents.UpdatedTime = DateTime.Now;

            contents.Required = cbRequiredField.Checked;
            _entities.AddToFormContents(contents);
            _entities.SaveChanges();

            Logger.Add(24, 2, contents.Id, 1);

            gvFormContents.DataBind();

            ClearFormInputs();
        }

        protected void btCancelFormContent_Click(object sender, EventArgs e)
        {
            pBtnAddNewForm.Visible = true;
            pAddNewForm.Visible = false;
            pForms.Visible = true;
            pFormContents.Visible = false;

            ClearFormInputs();
        }

        #endregion

        #region FormActions

        protected void btNewForm_Click(object sender, EventArgs e)
        {
            tbFormName.Text = string.Empty;
            cbFormState.Checked = false;

            pBtnAddNewForm.Visible = false;
            pAddNewForm.Visible = true;
        }

        protected void btSaveNewForm_Click(object sender, EventArgs e)
        {
            var newForm = new Forms();
            newForm.Name = tbFormName.Text;
            newForm.LanguageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
            newForm.State = cbFormState.Checked;
            newForm.EmailAddress = tbEmail.Text;
            _entities.AddToForms(newForm);
            _entities.SaveChanges();

            Logger.Add(24, 1, newForm.Id, 1);

            FormId = newForm.Id;
            MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            gvForms.DataBind();

            pBtnAddNewForm.Visible = true;
            pAddNewForm.Visible = false;
            pForms.Visible = true;
            pFormContents.Visible = false;
        }

        protected void gvForms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Update")
            {
                int id = int.Parse(e.CommandArgument.ToString());

                Forms form = _entities.Forms.First(p => p.Id == id);

                var v = (GridView) sender;
                var tbName = (TextBox) gvForms.Rows[v.EditIndex].FindControl("tbFormName");
                var tbEmailAddress = (TextBox) gvForms.Rows[v.EditIndex].FindControl("tbEmailAddress");

                form.Name = tbName.Text;
                form.EmailAddress = tbEmailAddress.Text;

                form.UpdatedTime = DateTime.Now;
                Logger.Add(24, 1, form.Id, 3);

                _entities.SaveChanges();
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
        }

        protected void gvForms_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e != null && e.Row.RowType == DataControlRowType.DataRow)
                {
                    var deleteBtn = (ImageButton) e.Row.FindControl("lbFormSil");
                    if (deleteBtn != null)
                    {
                        deleteBtn.OnClientClick =
                            " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
                        deleteBtn.ToolTip = AdminResource.lbDelete;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void lbFormSil_Click(object sender, ImageClickEventArgs e)
        {
            var lbFormSil = sender as ImageButton;
            if (lbFormSil != null)
            {
                var formId = Convert.ToInt32(lbFormSil.CommandArgument);
                using (var ent = new Entities())
                {
                    try
                    {
                        var form = ent.Forms.First(p => p.Id == formId);
                        var formContents = ent.FormContents.Where(p => p.FormId == formId).ToList();
                        foreach (FormContents contents in formContents)
                        {
                            var options = ent.FormContentOptions.Where(p => p.FormContentId == contents.Id).ToList();
                            foreach (var option in options)
                            {
                                ent.DeleteObject(option);
                            }
                            ent.SaveChanges();
                            ent.DeleteObject(contents);
                        }
                        ent.SaveChanges();
                        ent.DeleteObject(form);
                        ent.SaveChanges();

                        Logger.Add(24, 1, form.Id, 2);

                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                        //Logger.Add(5, 1, cat.photoAlbumCategoryId, 2);
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.ManageException(exception);
                    }
                }
            }
            gvForms.DataBind();
        }

        protected void lbFormSec_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["formId"] != null) ViewState.Remove("formId");
                var lbFormSec = (LinkButton) sender;
                int formId = Convert.ToInt32(lbFormSec.CommandArgument);
                hfFormId.Value = lbFormSec.CommandArgument;
                ViewState.Add("formId", formId);
                hfFormId.Value = formId.ToString();
                gvFormContents.DataBind();
                pBtnAddNewForm.Visible = false;
                pAddNewForm.Visible = false;
                pForms.Visible = false;
                pFormContents.Visible = true;

                lbSelectedFormName.Text = _entities.Forms.First(p => p.Id == formId).Name;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void btCancelNewForm_Click(object sender, EventArgs e)
        {
            gvForms.DataBind();
            pBtnAddNewForm.Visible = true;
            pAddNewForm.Visible = false;
            pForms.Visible = true;
            pFormContents.Visible = false;
        }

        #endregion
    }
}