using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class MemberSendEmail : UserControl
    {
        private readonly Entities _entities = new Entities();
        private List<Users> _users;
        public string MembersSqlQuery { get; set; }
        public SqlConnection Conn { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 35))
            {
                mvAuth.SetActiveView(vAuth);
                btSendReport.Text = AdminResource.lbSend;
                btUpdateTask.Text = AdminResource.lbUpdate;
            }
            else
            {
                mvAuth.SetActiveView(vNotAuth);
            }
        }

        public void GetMembers()
        {
            ClearFormInputs();
            _users = EnrollMembershipHelper.GetMembers(Conn, MembersSqlQuery);
        }

        public void ClearFormInputs()
        {
            var rtb = ((RadEditor) RtbMailContent.FindControl("RadEditor1"));
            rtb.Content = string.Empty;
            tbJobName.Text = string.Empty;
            tbMailSubject.Text = string.Empty;
            dpMailSendDate.SelectedDate = null;
            tpMailSendTime.SelectedTime = null;
            EnrollMembershipHelper.BindDDlMailTemplates(ddlMailTemplates);
            EnrollMembershipHelper.BindRbSendReport(rbSendReport);
        }

        protected void BtnSendMailClick(object sender, EventArgs e)
        {
            var ileriTarihli = false;
            var savedSuccessfuly = false;
            var tarih = string.Empty;
            try
            {
                var rtb = ((RadEditor) RtbMailContent.FindControl("RadEditor1"));

                var myTask = new Task();
                myTask.Content = rtb.Content;
                myTask.mailReadInfo = rbSendReport.SelectedIndex == 0;
                myTask.Name = tbJobName.Text.Trim(' ');
                if (!string.IsNullOrWhiteSpace(hfSqlQuery.Value))
                    myTask.SourceSelect = Crypto.Decrypt(hfSqlQuery.Value);
                if (dpMailSendDate.SelectedDate != null)
                {
                    tarih = dpMailSendDate.SelectedDate.Value.ToShortDateString();
                    if (tpMailSendTime.SelectedTime != null)
                    {
                        var saat = tpMailSendTime.SelectedTime.Value.ToString();
                        tarih += " " + saat;
                    }
                    myTask.StartDate = Convert.ToDateTime(tarih);
                    ileriTarihli = true;
                }
                myTask.State = 0;
                myTask.Subject = tbMailSubject.Text.Trim(' ');
                myTask.Type = 0;
                _entities.AddToTask(myTask);
                _entities.SaveChanges();
                savedSuccessfuly = true;
                if (cbSaveAsTemplate.Checked)
                {
                    var template = new App_Data.MailTemplates();
                    template.Title = tbMailSubject.Text;
                    template.MailContent = rtb.Content;
                    template.CreatedTime = DateTime.Now;
                    template.UpdatedTime = DateTime.Now;
                    _entities.AddToMailTemplates(template);
                    _entities.SaveChanges();
                    MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                }
                ClearFormInputs();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

            if (savedSuccessfuly)
            {
                lblUyari.Text = ileriTarihli
                                    ? string.Format(AdminResource.lbEmailTaskSavedInfo, tarih)
                                    : string.Format(AdminResource.lbEmailTaskCreated);
            }
            else
            {
                lblUyari.Text = string.Format(AdminResource.lbErrorOccurred);
            }
        }

        protected void ddlMailTemplates_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var rtb = ((RadEditor) RtbMailContent.FindControl("RadEditor1"));
            var selectedTemplate = ddlMailTemplates.SelectedItem.Value;
            try
            {
                if (ddlMailTemplates.SelectedIndex > 0)
                {
                    var templateId = Convert.ToInt32(selectedTemplate);
                    if (templateId > 0)
                    {
                        var template = _entities.MailTemplates.FirstOrDefault(p => p.Id == templateId);
                        if (template != null)
                        {
                            rtb.Content = template.MailContent;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void iBRefreshTemplatesClick(object sender, ImageClickEventArgs e)
        {
            EnrollMembershipHelper.BindDDlMailTemplates(ddlMailTemplates);
        }

        protected void btUpdateTaskClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfTaskId.Value))
                {
                    var taskId = Convert.ToInt32(hfTaskId.Value);
                    if (taskId > 0)
                    {
                        var task = _entities.Task.FirstOrDefault(p => p.taskId == taskId);
                        if (task != null)
                        {
                            task.Name = tbJobName.Text;
                            task.Subject = tbMailSubject.Text;
                            if (dpMailSendDate.SelectedDate != null && tpMailSendTime.SelectedTime != null)
                                task.StartDate =
                                    Convert.ToDateTime(dpMailSendDate.SelectedDate.Value.ToShortDateString() + " " +
                                                       tpMailSendTime.SelectedTime.Value.ToString());

                            var rtb = ((RadEditor) RtbMailContent.FindControl("RadEditor1"));
                            task.Content = rtb.Content;

                            task.mailReadInfo = (rbSendReport.SelectedIndex == 0);
                            task.State = 0;
                            task.Type = 0;

                            if (cbSaveAsTemplate.Checked)
                            {
                                var template = new App_Data.MailTemplates();
                                template.Title = tbMailSubject.Text;
                                template.MailContent = rtb.Content;
                                template.CreatedTime = DateTime.Now;
                                template.UpdatedTime = DateTime.Now;
                                _entities.AddToMailTemplates(template);
                                _entities.SaveChanges();
                                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                            }
                            ClearFormInputs();

                            _entities.SaveChanges();

                            var gVScheduledJobs = Parent.FindControl("gVScheduledJobs");
                            gVScheduledJobs.DataBind();

                            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }
}