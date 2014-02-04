using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;

namespace eNroll.Admin.adminUserControls
{
    public partial class MailTemplates : UserControl
    {
        private readonly Entities _entities = new Entities();

        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbMailTemplateManagement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 28))
            {
                mvAuthoriztn.ActiveViewIndex = 0;
                mvMailTemplates.ActiveViewIndex = 0;
            }
            else
            {
                mvAuthoriztn.ActiveViewIndex = 1;
            }

            gVMailTemplates.Columns[0].HeaderText = AdminResource.lbActions;
            gVMailTemplates.Columns[1].HeaderText = AdminResource.lbTitle;
            gVMailTemplates.Columns[2].HeaderText = AdminResource.lbDate;


            btSaveTemplate.Text = AdminResource.lbSave;
            btCancelSaveTemplate.Text = AdminResource.lbCancel;
            btNewTemplate.Text = AdminResource.lbNewMailTemplate;

            mvMailTemplates.SetActiveView(vGridTemplate);
        }

        protected void ImgBtnMemberEditClick(object sender, ImageClickEventArgs e)
        {
            var imgEditBtn = sender as ImageButton;
            try
            {
                if (imgEditBtn != null)
                {
                    var data = imgEditBtn.CommandArgument;
                    var templateId = Convert.ToInt32(data);
                    if (templateId > 0)
                    {
                        var template = _entities.MailTemplates.FirstOrDefault(p => p.Id == templateId);
                        if (template != null)
                        {
                            BindForm(template);
                            mvMailTemplates.SetActiveView(vEditTemplate);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected void ImgBtnMemberDeleteClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                var btnDelete = (ImageButton) sender;
                var data = btnDelete.CommandArgument;
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var templateId = Convert.ToInt32(data);
                    if (templateId > 0)
                    {
                        var template = _entities.MailTemplates.FirstOrDefault(p => p.Id == templateId);
                        if (template != null)
                        {
                            _entities.DeleteObject(template);
                            _entities.SaveChanges();
                            gVMailTemplates.DataBind();
                            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }

        public void BindForm(App_Data.MailTemplates mailTemplate)
        {
            hfMailTemplate.Value = mailTemplate.Id.ToString();
            tbTemplateTitle.Text = mailTemplate.Title;
            var radEditor = (RadEditor) RtbTemplateContent.FindControl("RadEditor1");
            radEditor.Content = mailTemplate.MailContent;
        }

        public void ClearForm()
        {
            hfMailTemplate.Value = string.Empty;
            var radEditor = (RadEditor) RtbTemplateContent.FindControl("RadEditor1");
            radEditor.Content = string.Empty;
            tbTemplateTitle.Text = string.Empty;
        }

        protected void BtSaveTemplateClick(object sender, EventArgs e)
        {
            var template = new App_Data.MailTemplates();
            int templateId = 0;
            try
            {
                if (!string.IsNullOrEmpty(hfMailTemplate.Value))
                {
                    templateId = Convert.ToInt32(hfMailTemplate.Value);
                    if (templateId > 0)
                        template = _entities.MailTemplates.FirstOrDefault(p => p.Id == templateId);
                }
                var radEditor = (RadEditor) RtbTemplateContent.FindControl("RadEditor1");
                if (template != null)
                {
                    template.MailContent = radEditor.Content;
                    template.Title = tbTemplateTitle.Text;

                    if (templateId == 0)
                    {
                        template.CreatedTime = DateTime.Now;
                        _entities.AddToMailTemplates(template);
                    }
                    template.UpdatedTime = DateTime.Now;
                    _entities.SaveChanges();

                    MessageBox.Show(MessageType.Success,
                                    templateId > 0 ? AdminResource.msgUpdated : AdminResource.msgSaved);

                    gVMailTemplates.DataBind();
                    mvMailTemplates.SetActiveView(vGridTemplate);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }

        protected void BtCancelSaveTemplateClick(object sender, EventArgs e)
        {
            mvMailTemplates.SetActiveView(vGridTemplate);
            ClearForm();
        }


        protected void ImageButtonbtNewTemplate_Click(object sender, EventArgs e)
        {
            ClearForm();
            mvMailTemplates.SetActiveView(vEditTemplate);
        }

        protected void gVMailTemplates_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnDelete = (ImageButton) e.Row.FindControl("imgBtnMemberDelete");
                btnDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
    }
}