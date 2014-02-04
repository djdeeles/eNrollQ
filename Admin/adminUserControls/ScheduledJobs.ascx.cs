using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class ScheduledJobs : UserControl
    {
        private readonly Entities _entities = new Entities();

        protected override void OnInit(EventArgs e)
        {
            CheckCulture();
            Session["currentPath"] = AdminResource.lbScheduledJobs;

            #region Resources

            btCancelEditTask.Text = AdminResource.lbCancel;

            gVScheduledJobs.Columns[0].HeaderText = AdminResource.lbActions;
            gVScheduledJobs.Columns[1].HeaderText = AdminResource.lbJobName;
            gVScheduledJobs.Columns[2].HeaderText = AdminResource.lbSubject;
            gVScheduledJobs.Columns[3].HeaderText = AdminResource.lbType;
            gVScheduledJobs.Columns[4].HeaderText = AdminResource.lbDate;
            gVScheduledJobs.Columns[5].HeaderText = AdminResource.lbState;

            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 31))
            {
                mvAuthoriztn.ActiveViewIndex = 0;
                if (!IsPostBack) mvScheduledJobs.SetActiveView(vGridViewScheduledJobs);
            }
            else
            {
                mvAuthoriztn.ActiveViewIndex = 1;
            }
        }

        protected void gVSchduledJobs_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnShowMail = e.Row.FindControl("imgBtnShowMail") as ImageButton;
            if (btnShowMail != null) btnShowMail.AlternateText = AdminResource.lbShowMail;

            var btnDelete = e.Row.FindControl("imgBtnTaskDelete") as ImageButton;
            if (btnDelete != null)
                btnDelete.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "') ";
        }

        protected string HaveTaskReport(string data)
        {
            var result = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var taskId = Convert.ToInt32(data);
                    var taskReport = _entities.EmailReport.FirstOrDefault(p => p.taskId == taskId);
                    result = taskReport == null ? "style='display:none;'" : string.Empty;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        #region set page culture

        public static void CheckCulture()
        {
            var ent = new EnrollAdminContext();
            var entities = new Entities();
            int lang = ent.AdminLanguage.LanguageId;
            System_language system = entities.System_language.FirstOrDefault(p => p.languageId == lang);
            string cultureName = system.languageCulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
        }

        #endregion

        #region Bind Task For Edit

        public void ShowTaskForEdit(int taskId)
        {
            try
            {
                var task = _entities.Task.FirstOrDefault(p => p.taskId == taskId);
                if (task != null)
                {
                    var hfTaskId = (HiddenField) cEditTask.FindControl("hfTaskId");
                    hfTaskId.Value = taskId.ToString();

                    var btSendReport = (Button) cEditTask.FindControl("btSendReport");
                    btSendReport.Visible = false;

                    var btUpdateTask = (Button) cEditTask.FindControl("btUpdateTask");
                    btUpdateTask.Visible = true;

                    var ddlMailTemplates = (DropDownList) cEditTask.FindControl("ddlMailTemplates");
                    EnrollMembershipHelper.BindDDlMailTemplates(ddlMailTemplates);

                    var tbMailSubject = (TextBox) cEditTask.FindControl("tbMailSubject");
                    tbMailSubject.Text = task.Subject;

                    var tbJobName = (TextBox) cEditTask.FindControl("tbJobName");
                    tbJobName.Text = task.Name;

                    var rbSendReport = (RadioButtonList) cEditTask.FindControl("rbSendReport");
                    EnrollMembershipHelper.BindRbSendReport(rbSendReport);
                    rbSendReport.SelectedIndex = task.mailReadInfo != null && task.mailReadInfo.Value ? 0 : 1;

                    #region date-time

                    var dpMailSendDate = (RadDatePicker) cEditTask.FindControl("dpMailSendDate");
                    if (task.StartDate != null)
                        dpMailSendDate.SelectedDate = Convert.ToDateTime(task.StartDate.Value);

                    var tpMailSendTime = (RadTimePicker) cEditTask.FindControl("tpMailSendTime");
                    if (task.StartDate != null)
                        tpMailSendTime.SelectedDate = Convert.ToDateTime(task.StartDate.Value);

                    #endregion

                    var cRtbMailContent = cEditTask.FindControl("RtbMailContent");
                    var rtb = ((RadEditor) cRtbMailContent.FindControl("RadEditor1"));
                    rtb.Content = task.Content;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #endregion

        #region EditClick, DeleteClick, CancelClick

        protected void ImgBtnMemberEditClick(object sender, ImageClickEventArgs e)
        {
            var btnEdit = sender as ImageButton;
            if (btnEdit != null)
            {
                try
                {
                    var btnEditData = btnEdit.CommandArgument;
                    if (btnEditData != null)
                    {
                        var taskId = Convert.ToInt32(btnEditData);
                        if (taskId > 0)
                        {
                            ShowTaskForEdit(taskId);
                            btCancelEditTask.Visible = true;
                            mvScheduledJobs.SetActiveView(vEditTask);
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
        }

        protected void ImgBtnTaskDeleteClick(object sender, ImageClickEventArgs e)
        {
            var btnDelete = sender as ImageButton;
            if (btnDelete != null)
            {
                try
                {
                    var btnDeleteData = btnDelete.CommandArgument;
                    if (btnDeleteData != null)
                    {
                        var taskId = Convert.ToInt32(btnDeleteData);
                        var task = _entities.Task.FirstOrDefault(p => p.taskId == taskId);
                        if (task != null)
                        {
                            _entities.DeleteObject(task);
                            _entities.SaveChanges();
                            gVScheduledJobs.DataBind();
                            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
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
        }

        protected void BtCancelEditTaskClick(object sender, EventArgs e)
        {
            cEditTask.ClearFormInputs();
            btCancelEditTask.Visible = false;
            mvScheduledJobs.SetActiveView(vGridViewScheduledJobs);
        }

        #endregion
    }
}