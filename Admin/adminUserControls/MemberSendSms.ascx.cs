using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
    public partial class MemberSendSms : System.Web.UI.UserControl
    {
        Entities _entities = new Entities();
        public string MembersSqlQuery = string.Empty;
        public SqlConnection Conn;
        private List<Users> _users;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 34))
            {
                mvAuth.SetActiveView(vAuth);
                btSendReport.Text = AdminResource.lbSend;
            }
            else
            {
                mvAuth.SetActiveView(vNotAuth);
            } 
        }

        public void GetMembers()
        {
            _users = EnrollMembershipHelper.GetMembers(Conn, MembersSqlQuery);
        }

        protected void BtnSendSmsClick(object sender, EventArgs e)
        {

            var ileriTarihli = false;
            var savedSuccessfuly = false;
            var tarih = string.Empty;
            try
            {
                var myTask = new Task();
                myTask.Content = tbSmsContent.Text;
                myTask.mailReadInfo = null;
                myTask.Name = tbJobName.Text.Trim(' ');
                if (!string.IsNullOrWhiteSpace(hfSqlQuery.Value))
                    myTask.SourceSelect = Crypto.Decrypt(hfSqlQuery.Value);
                if (dpSmsSendDate.SelectedDate != null)
                {
                    tarih = dpSmsSendDate.SelectedDate.Value.ToShortDateString();
                    if (tpSmsSendTime.SelectedTime != null)
                    {
                        var saat = tpSmsSendTime.SelectedTime.Value.ToString();
                        tarih += " " + saat;
                    }
                    myTask.StartDate = Convert.ToDateTime(tarih);
                    ileriTarihli = true;
                }
                myTask.State = 0;
                myTask.Subject = string.Empty;
                myTask.Type = 0;
                _entities.AddToTask(myTask);
                _entities.SaveChanges();
                savedSuccessfuly = true;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

            if (savedSuccessfuly)
            {
                lblUyari.Text = ileriTarihli ? 
                    string.Format(AdminResource.lbSmsTaskSavedInfo, tarih) : 
                    string.Format(AdminResource.lbSmsTaskCreated);
            }
            else
            {
                lblUyari.Text = string.Format(AdminResource.lbErrorOccurred);
            }


        }
    }
}