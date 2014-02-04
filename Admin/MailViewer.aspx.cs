using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin
{
    public partial class MailViewer : Page
    {
        private readonly Entities _entities = new Entities();

        public SqlConnection _oConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());

        #region sql query

        private const string SqlQueryTemplate =
            "SELECT * FROM EmailReport as er " +
            "Inner Join UserEmails as ue on ue.Email=er.emailAdress " +
            "Inner Join Users as u on u.Id=ue.UserId  " +
            "Inner Join Task as t on t.taskId=er.taskId " +
            "where er.taskId = {0} {1} {2}";

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Task task = null;
            ltMailContent.Text = string.Empty;
            CheckCulture();

            #region resources

            gvJobReportList.Columns[0].HeaderText = AdminResource.lbName;
            gvJobReportList.Columns[1].HeaderText = AdminResource.lbSurname;
            gvJobReportList.Columns[2].HeaderText = AdminResource.lbEmail;
            gvJobReportList.Columns[3].HeaderText = AdminResource.lbState;
            gvJobReportList.Columns[4].HeaderText = AdminResource.lbDate;
            ibWriteResultExcel.Text = AdminResource.lbExcelExport;

            #endregion

            pTasksReportList.Visible = false;
            pTasksReportChart.Visible = false;
            pTaskEdit.Visible = false;
            ltMailContent.Visible = false;

            if (!IsPostBack)
            {
                BindDdlFilterState();
            }
            try
            {
                hfSqlQuery.Value = Crypto.Encrypt(SqlQueryTemplate);

                #region get datas from QueryString for: "show mail content" or "show job detail"

                if (HttpContext.Current.Request.QueryString.Count > 0 &&
                    HttpContext.Current.Request.QueryString["proccess"] != null &&
                    HttpContext.Current.Request.QueryString["task"] != null)
                {
                    var proccessType = Convert.ToInt32(HttpContext.Current.Request.QueryString["proccess"]);
                    var taskId = Convert.ToInt32(HttpContext.Current.Request.QueryString["task"]);
                    hfTaskId.Value = Crypto.Encrypt(taskId.ToString());
                    if (proccessType > 0)
                    {
                        #region show mail

                        if (proccessType == 1)
                        {
                            if (taskId > 0)
                            {
                                ltMailContent.Visible = true;
                                task = _entities.Task.FirstOrDefault(p => p.taskId == taskId);
                                if (task != null)
                                {
                                    ltMailContent.Text = task.Content;
                                }
                            }
                        }
                            #endregion

                            #region show task details

                        else if (proccessType == 2)
                        {
                            pTasksReportList.Visible = true;
                            pTasksReportChart.Visible = true;
                            if (taskId > 0)
                            {
                                ShowTaskReportList(taskId);
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    form1.Visible = false;
                }

                #endregion
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected string GetTaskName()
        {
            if (!string.IsNullOrWhiteSpace(hfTaskId.Value))
            {
                var taskId = Convert.ToInt32(Crypto.Decrypt(hfTaskId.Value));
                var task = _entities.Task.FirstOrDefault(p => p.taskId == taskId);
                return task != null ? task.Name : string.Empty;
            }
            return string.Empty;
        }

        #region Bind Ddl DropDwnList filter "State"

        public void BindDdlFilterState()
        {
            ddlFilterState.Items.Clear();
            ddlFilterState.Items.Insert(0, new ListItem(AdminResource.lbAll, ""));
            ddlFilterState.Items.Insert(1, new ListItem(AdminResource.lbSent + " & " + AdminResource.lbRead,
                                                        Crypto.Encrypt(" AND er.State=2")));
            ddlFilterState.Items.Insert(2, new ListItem(AdminResource.lbNotSent,
                                                        Crypto.Encrypt(" AND er.State=0")));
            ddlFilterState.Items.Insert(3, new ListItem(AdminResource.lbSent + " & " + AdminResource.lbNotKnown,
                                                        Crypto.Encrypt(" AND er.State=1")));
        }

        #endregion

        #region set page culture

        public void CheckCulture()
        {
            var ent = new EnrollAdminContext();
            var entities = new Entities();
            var lang = ent.AdminLanguage.LanguageId;
            var system = entities.System_language.FirstOrDefault(p => p.languageId == lang);
            if (system != null)
            {
                var cultureName = system.languageCulture;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            }
        }

        #endregion

        #region "state:Read,NotRead,NotSend" DropDwnList filter Selected Index Changed

        protected void ddlFilterState_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWhereParams.Value = ddlFilterState.SelectedItem.Value;
            var taskId = Convert.ToInt32(Crypto.Decrypt(hfTaskId.Value));
            if (taskId > 0)
            {
                ShowTaskReportList(taskId);
            }
        }

        #endregion

        #region exec sql query for results

        public void ShowTaskReportList(int taskId)
        {
            try
            {
                var resultDataTable = new DataTable();

                var sqlQuery = string.Format(Crypto.Decrypt(hfSqlQuery.Value),
                                             Convert.ToInt32(Crypto.Decrypt(hfTaskId.Value)),
                                             hfWhereParams.Value == string.Empty
                                                 ? string.Empty
                                                 : Crypto.Decrypt(hfWhereParams.Value),
                                             hfSearchParams.Value == string.Empty
                                                 ? string.Empty
                                                 : Crypto.Decrypt(hfSearchParams.Value));

                var cmdSearchResault = new SqlCommand {Connection = _oConnection, CommandText = sqlQuery};
                if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                var oAdaptor = new SqlDataAdapter(cmdSearchResault);

                oAdaptor.Fill(resultDataTable);
                gvJobReportList.DataSource = resultDataTable;
                gvJobReportList.DataBind();
                gvJobReportList.Visible = true;

                ibWriteResultExcel.Enabled = resultDataTable.Rows.Count > 0;

                #region Read Count

                var countDataRead = new DataTable();
                var cmdReadCount = new SqlCommand
                                       {
                                           Connection = _oConnection,
                                           CommandText =
                                               string.Format(Crypto.Decrypt(hfSqlQuery.Value), taskId, " AND er.state=2",
                                                             string.Empty)
                                       };
                oAdaptor = new SqlDataAdapter(cmdReadCount);
                oAdaptor.Fill(countDataRead);
                hfReadCount.Value = countDataRead.Rows.Count.ToString();

                #endregion

                #region Not Read Count

                var countDataNotRead = new DataTable();
                var cmdNotReadCount = new SqlCommand
                                          {
                                              Connection = _oConnection,
                                              CommandText =
                                                  string.Format(Crypto.Decrypt(hfSqlQuery.Value), taskId,
                                                                " AND er.state=1", string.Empty)
                                          };
                oAdaptor = new SqlDataAdapter(cmdNotReadCount);
                oAdaptor.Fill(countDataNotRead);
                hfNotReadCount.Value = countDataNotRead.Rows.Count.ToString();

                #endregion

                #region Not Send Count

                var countDataNotSend = new DataTable();
                var cmdNotSendCount = new SqlCommand
                                          {
                                              Connection = _oConnection,
                                              CommandText =
                                                  string.Format(Crypto.Decrypt(hfSqlQuery.Value), taskId,
                                                                " AND er.state=0", string.Empty)
                                          };
                oAdaptor = new SqlDataAdapter(cmdNotSendCount);
                oAdaptor.Fill(countDataNotSend);
                hfNotSendCount.Value = countDataNotSend.Rows.Count.ToString();

                #endregion

                lbEmailCount.Text = (resultDataTable.Rows != null ? resultDataTable.Rows.Count.ToString() : "0");
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            finally
            {
                if (_oConnection.State == ConnectionState.Open) _oConnection.Close();
            }
        }

        #endregion

        #region filter by E-Mail

        protected void ibFilterEmail_OnClick(object sender, ImageClickEventArgs e)
        {
            var email = tbFilterEmail.Text.Trim(' ');
            if (email.Contains("'") || email.Contains('"')) return;
            hfSearchParams.Value = Crypto.Encrypt(string.Format(" AND er.emailAdress Like '%{0}%'", email));

            if (!string.IsNullOrWhiteSpace(hfTaskId.Value))
            {
                var taskId = Convert.ToInt32(Crypto.Decrypt(hfTaskId.Value));
                if (taskId > 0)
                {
                    ShowTaskReportList(taskId);
                }
            }
        }

        #endregion

        #region export table excel

        protected void IbWriteExcellClick(object sender, EventArgs e)
        {
            var view = new GridView();
            try
            {
                var resultDataTable = new DataTable();
                if (hfTaskId.Value != null)
                {
                    var taskId = Crypto.Decrypt(hfTaskId.Value);
                    var sqlQuery = string.Format(Crypto.Decrypt(hfSqlQuery.Value), taskId,
                                                 hfWhereParams.Value == string.Empty
                                                     ? string.Empty
                                                     : Crypto.Decrypt(hfWhereParams.Value),
                                                 hfSearchParams.Value == string.Empty
                                                     ? string.Empty
                                                     : Crypto.Decrypt(hfSearchParams.Value));

                    var cmdSearchResault = new SqlCommand {Connection = _oConnection, CommandText = sqlQuery};
                    if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                    var oAdaptor = new SqlDataAdapter(cmdSearchResault);

                    oAdaptor.Fill(resultDataTable);
                    gvJobReportList.DataSource = resultDataTable;
                    gvJobReportList.DataBind();
                    gvJobReportList.Visible = true;

                    if (resultDataTable.Rows.Count > 0)
                    {
                        // yeni tablo oluşturulur
                        var xlsJobsDataTable = EnrollMembershipHelper.CreateScheduledJobsDataTable();
                        foreach (DataRow item in resultDataTable.Rows)
                        {
                            var newRow = xlsJobsDataTable.NewRow();

                            newRow[AdminResource.lbName] = item["Name"].ToString();
                            newRow[AdminResource.lbSurname] = item["Surname"].ToString();
                            newRow[AdminResource.lbEmail] = item["email"].ToString();
                            newRow[AdminResource.lbState] = string.Format("{0}, {1}",
                                                                          Convert.ToInt32(item["state"]) != 0
                                                                              ? "Gönderildi"
                                                                              : "Gönderilmedi",
                                                                          Convert.ToInt32(item["state"]) == 2
                                                                              ? "Okundu"
                                                                              : "Okunma bilinmiyor");
                            newRow[AdminResource.lbDate] = item["readDate"].ToString();
                            xlsJobsDataTable.Rows.Add(newRow);
                        }

                        #region oluşturulan dataTable gridView e bind edilir

                        view.DataSource = xlsJobsDataTable;
                        view.DataBind();

                        #endregion
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            finally
            {
                if (_oConnection.State == ConnectionState.Open) _oConnection.Close();
            }

            #region gridView download edilir

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();

            Response.ContentEncoding = Encoding.GetEncoding("windows-1254");
            Response.Charset = "windows-1254"; //ISO-8859-13 ISO-8859-9  windows-1254

            Response.Buffer = true;
            EnableViewState = false;
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToShortDateString() + ".xls");
            const string header =
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<title></title>\n<meta http-equiv=\"Content-Type\" content=\"text/html;" +
                " charset=windows-1254\" />\n<style>\n</style>\n</head>\n<body>\n";

            var textWriter = new StringWriter();
            var htmlTextWriter = new HtmlTextWriter(textWriter);
            view.RenderControl(htmlTextWriter);
            Response.Write(header + textWriter);
            Response.End();

            #endregion
        }

        #endregion
    }
}