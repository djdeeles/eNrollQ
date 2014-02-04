using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
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
    public partial class MemberSearch : UserControl
    {
        private static MemberInfo Member = new MemberInfo();
        private readonly Entities _entities = new Entities();
        private DataTable _memberSearchDataTable = new DataTable();

        #region create search sql variables

        private string _innerJoin = string.Empty;

        public SqlConnection _oConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());

        private int _parametersCount;
        private StringBuilder _searchCommandUserGeneralWhere = new StringBuilder();
        private StringBuilder _searchCommandUserMembershipWhere = new StringBuilder();
        private StringBuilder _searchCommandUsersWhere = new StringBuilder();
        private string _searchResultCommandText = string.Empty;
        private string _searchResultCountCommandText = string.Empty;
        private string _searchResultExcellCommandText = string.Empty;
        private string _sqlSearchCountQuery = "SELECT Count(distinct(u.Id)) FROM Users as u {0} {1} {2} {3} ";

        private string _sqlSearchExcellQuery =
            "SELECT distinct(u.Id) as uId,u.Email as uEmail,Name as uName, u.Surname as uSurname,u.State as uState,u.Admin as isAdmin," +
            "f.MemberRelType as fMembershipType,f.MemberNo as fMembershipNo,fin.Id as finId,fin.Dept as finDept," +
            "[SpecialNo],[MemberNo],[MemberRelType],[MemberState],[MembershipDate],[Term],[TermLeader],[Vip],[TC],[Gender],[BloodType]," +
            "[FatherName],[MotherName],[MaritalStatus],[MaidenName],[MarriageDate],[Birthdate],[Birthplace],[PhotoUrl]," +
            "[Hobby],[Decease],[DeceaseDate],[Web],[GsmNo],[GsmError],[AllowSms],[LastSchool]," +
            "[LastSchoolGraduateDate],[HomePhone],[HomeAddress],[HomeTown],[HomeCity],[HomeCountry],[HomeZipCode]," +
            "[MemberFoundation],[HidePersonalInfo],[WorkPhone],[WorkAddress],[WorkTown],[WorkCity],[WorkCountry],[WorkZipCode]," +
            "[WorkFax],[WorkCorporation],[WorkTitle],[JobNo],[JobSectorNo],[HideJobInfo],[AdminNote] " +
            " FROM Users as u {0} {1} {2} {3}";

        private string _sqlSearchQuery =
            "SELECT distinct(u.Id) as uId, u.Email as uEmail, u.Name as uName, u.Surname as uSurname, u.State as uState, u.Admin as isAdmin, " +
            "f.MemberRelType as fMembershipType, f.MemberNo as fMembershipNo, " +
            "fin.Id as finId, fin.Dept as finDept " +
            "FROM Users as u {0} {1} {2} {3}";

        #endregion

        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbMemberSearch;

            #region resources

            btSearch.Text = AdminResource.lbSearch;
            btShowResults.Text = AdminResource.lbShowResults;
            btClearSearchCriterias.Text = AdminResource.lbClearSearchCriterias;
            btDownloadUsersExcelList.Text = AdminResource.lbDownloadAsExcel;
            btSendEmail.Text = AdminResource.lbSendEmail;
            btFinanceManager.Text = AdminResource.lbFinanceManager;
            btBackToSearchPage.Text = AdminResource.lbSearchPage;

            btSendSms.Text = AdminResource.lbSendSms;
            btCancelSendMail.Text = AdminResource.lbSearchPage;
            btCancelSendSms.Text = AdminResource.lbSearchPage;
            btCancelFinanceManager.Text = AdminResource.lbSearchPage;

            gVMembers.Columns[0].HeaderText = AdminResource.lbActions;
            gVMembers.Columns[1].HeaderText = AdminResource.lbMemberNo;
            gVMembers.Columns[2].HeaderText = string.Format("{0} {1}", AdminResource.lbName, AdminResource.lbSurname);
            gVMembers.Columns[3].HeaderText = AdminResource.lbEmail;
            gVMembers.Columns[4].HeaderText = AdminResource.lbMemberRelType;
            gVMembers.Columns[5].HeaderText = AdminResource.lbAdmin;
            gVMembers.Columns[6].HeaderText = AdminResource.lbActive;

            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 27))
            {
                mvAuthoriztn.ActiveViewIndex = 0;
                _parametersCount = 0;
                if (!IsPostBack)
                {
                    btFinanceManager.Visible = RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 33);
                    btSendSms.Visible = RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 34);
                    btSendEmail.Visible = RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 35);

                    _searchCommandUsersWhere = new StringBuilder();
                    _searchCommandUserGeneralWhere = new StringBuilder();
                    _searchCommandUserMembershipWhere = new StringBuilder();

                    _oConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
                    _memberSearchDataTable = new DataTable();
                    Member = new MemberInfo();
                    BindSearchCriterias();
                    mvMemberSearch.SetActiveView(vSearchMember);
                }
                else
                {
                    cSendMail.MembersSqlQuery = hfSearchResaultCmd.Value;
                    cSendSms.MembersSqlQuery = hfSearchResaultCmd.Value;
                }
            }
            else
            {
                mvAuthoriztn.ActiveViewIndex = 1;
            }
        }

        protected void BtShowResultsClick(object sender, EventArgs e)
        {
            #region search members

            try
            {
                if (!string.IsNullOrWhiteSpace(hfSearchResaultCmd.Value))
                {
                    var searchCommandText = Crypto.Decrypt(hfSearchResaultCmd.Value);

                    var cmdSearchResault = new SqlCommand
                                               {
                                                   Connection = _oConnection,
                                                   CommandText = searchCommandText
                                               };

                    if (_oConnection.State == ConnectionState.Closed)
                        _oConnection.Open();

                    var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                    oAdaptor.Fill(_memberSearchDataTable);

                    gVMembers.DataSource = _memberSearchDataTable;
                    gVMembers.DataBind();
                    gVMembers.Visible = true;
                    btShowResults.Visible = false;
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

            #endregion
        }

        protected void BtDownloadUsersExcelList(object sender, EventArgs e)
        {
            DataTable _memberSearchExcellDataTable = new DataTable();
            try
            {
                var view = new GridView();
                if (!string.IsNullOrWhiteSpace(hfSearchResaultExcellCmd.Value))
                {
                    var searchCommandExcellText = Crypto.Decrypt(hfSearchResaultExcellCmd.Value);
                    tbSqlQueryOutput.Text = searchCommandExcellText;
                    var cmdSearchResaultExcell = new SqlCommand
                                                     {Connection = _oConnection, CommandText = searchCommandExcellText};
                    if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                    SqlDataAdapter oAdaptor = new SqlDataAdapter(cmdSearchResaultExcell);
                    oAdaptor.Fill(_memberSearchExcellDataTable);

                    if (_memberSearchExcellDataTable.Rows.Count > 0)
                    {
                        // yeni tablo oluşturulur
                        var xlsUsersDataTable = EnrollMembershipHelper.CreateUsersDataTable();

                        foreach (DataRow item in _memberSearchExcellDataTable.Rows)
                        {
                            var userId = Convert.ToInt32(item["uId"]);

                            var newRow = xlsUsersDataTable.NewRow();

                            #region işleri kolaylaştırmak için değişkenlerle, kullanıcıya ait veriler bulunur

                            var bloodId = ((item["BloodType"] != DBNull.Value && item["BloodType"] != null)
                                               ? Convert.ToInt32(item["BloodType"].ToString())
                                               : 0);
                            var bloodType = _entities.BloodTypes.FirstOrDefault(p => p.Id == bloodId);

                            var admin = ((item["isAdmin"] != DBNull.Value && item["isAdmin"] != null) &&
                                         Convert.ToBoolean(item["isAdmin"]));
                            var gender = ((item["Gender"] != DBNull.Value && item["Gender"] != null)
                                              ? Convert.ToInt32(item["Gender"].ToString())
                                              : 0);
                            var decease = ((item["Decease"] != DBNull.Value && item["Decease"] != null) &&
                                           Convert.ToBoolean(item["Decease"]));
                            var memberState = Convert.ToInt32(item["MemberState"]);
                            var termLeader = ((item["TermLeader"] != DBNull.Value && item["TermLeader"] != null) &&
                                              Convert.ToBoolean(item["TermLeader"]));
                            var vip = ((item["Vip"] != DBNull.Value && item["Vip"] != null) &&
                                       Convert.ToBoolean(item["Vip"]));

                            var maritalStatus = string.Empty;
                            if (item["MaritalStatus"] != DBNull.Value && item["MaritalStatus"] != null)
                            {
                                var maritalStatu = Convert.ToInt32(item["MaritalStatus"].ToString());
                                if (maritalStatu == 1) maritalStatus = AdminResource.lbMarried;
                                else if (maritalStatu == 2) maritalStatus = AdminResource.lbSingle;
                            }

                            var homeCountry = ((item["HomeCountry"] != DBNull.Value && item["HomeCountry"] != null)
                                                   ? Convert.ToInt32(item["HomeCountry"].ToString())
                                                   : 0);
                            var homeCity = ((item["HomeCity"] != DBNull.Value && item["HomeCity"] != null)
                                                ? Convert.ToInt32(item["HomeCity"].ToString())
                                                : 0);
                            var homeTown = ((item["HomeTown"] != DBNull.Value && item["HomeTown"] != null)
                                                ? Convert.ToInt32(item["HomeTown"].ToString())
                                                : 0);
                            var workCountry = ((item["WorkCountry"] != DBNull.Value &&
                                                !string.IsNullOrWhiteSpace(item["WorkCountry"].ToString()))
                                                   ? Convert.ToInt32(item["WorkCountry"])
                                                   : 0);
                            var workCity = ((item["WorkCity"] != DBNull.Value &&
                                             !string.IsNullOrWhiteSpace(item["WorkCity"].ToString()))
                                                ? Convert.ToInt32(item["WorkCity"].ToString())
                                                : 0);
                            var workTown = ((item["WorkCity"] != DBNull.Value &&
                                             !string.IsNullOrWhiteSpace(item["WorkTown"].ToString()))
                                                ? Convert.ToInt32(item["WorkTown"].ToString())
                                                : 0);
                            var jobNo = ((item["JobNo"] != DBNull.Value && item["JobNo"] != null)
                                             ? Convert.ToInt32(item["JobNo"].ToString())
                                             : 0);
                            var jobSectorNo = ((item["JobSectorNo"] != DBNull.Value && item["JobSectorNo"] != null)
                                                   ? Convert.ToInt32(item["JobSectorNo"])
                                                   : 0);
                            var memberRelType = ((item["MemberRelType"] != DBNull.Value && item["MemberRelType"] != null)
                                                     ? Convert.ToInt32(item["MemberRelType"])
                                                     : 0);

                            #endregion

                            #region yeni satıra sırayla kullanıcıların verileri eklenir, bunların bazılarının yine null kontrolü yapılır; bazılarının da yapılmasına gerek yoktur.

                            newRow[AdminResource.lbName] = item["uName"].ToString();
                            newRow[AdminResource.lbSurname] = item["uSurname"].ToString();
                            newRow[AdminResource.lbMainEmail] = EnrollMembershipHelper.GetUserMainEmail(userId);
                            newRow[AdminResource.lbOtherEmail] = EnrollMembershipHelper.GetUserOtherEmails(userId);
                            newRow[AdminResource.lbGsmNo] = item["GsmNo"].ToString();
                            newRow[AdminResource.lbWeb] = item["Web"].ToString();
                            newRow[AdminResource.lbFatherName] = item["FatherName"].ToString();
                            newRow[AdminResource.lbMotherName] = item["MotherName"].ToString();
                            newRow[AdminResource.lbMartialStatus] = maritalStatus;
                            newRow[AdminResource.lbHomePhone] = item["HomePhone"].ToString();
                            newRow[AdminResource.lbHomeAddress] = item["HomeAddress"].ToString();
                            newRow[AdminResource.lbTerm] = item["Term"].ToString();
                            newRow[AdminResource.lbLastSchool] = item["LastSchool"].ToString();
                            newRow[AdminResource.lbWorkPhone] = item["WorkPhone"].ToString();
                            newRow[AdminResource.lbWorkAddress] = item["WorkAddress"].ToString();
                            newRow[AdminResource.lbWorkFax] = item["WorkFax"].ToString();
                            newRow[AdminResource.lbMemberNo] = item["MemberNo"].ToString();
                            newRow[AdminResource.lbSpecialNo] = item["SpecialNo"].ToString();
                            newRow[AdminResource.lbMemberFoundation] = item["MemberFoundation"].ToString();
                            newRow[AdminResource.lbWorkCorporation] = item["WorkCorporation"].ToString();
                            newRow[AdminResource.lbWorkTitle] = item["WorkTitle"].ToString();
                            newRow[AdminResource.lbAdminNote] = item["AdminNote"].ToString();

                            newRow[AdminResource.lbGender] = (gender == 1
                                                                  ? AdminResource.lbMan
                                                                  : (gender == 2 ? AdminResource.lbWoman : ""));
                            newRow[AdminResource.lbMemberState] = (memberState == 1
                                                                       ? AdminResource.lbActive
                                                                       : (memberState == 0
                                                                              ? AdminResource.lbPassive
                                                                              : "Engellenmiş"));
                            newRow[AdminResource.lbTermLeader] = (termLeader ? AdminResource.lbYes : AdminResource.lbNo);
                            newRow[AdminResource.lbState] = (Convert.ToBoolean(item["uState"])
                                                                 ? AdminResource.lbActive
                                                                 : AdminResource.lbPassive);
                            newRow[AdminResource.lbDecease] = (decease ? AdminResource.lbYes : AdminResource.lbNo);
                            newRow[AdminResource.lbAdmin] = (admin ? AdminResource.lbYes : AdminResource.lbNo);
                            newRow[AdminResource.lbVip] = (vip ? AdminResource.lbYes : AdminResource.lbNo);

                            newRow[AdminResource.lbMemberRelType] = (memberRelType != 0
                                                                         ? _entities.FoundationRelType.First(
                                                                             p => p.Id == memberRelType).Name
                                                                         : "");
                            newRow[AdminResource.lbHomeCountry] = (homeCountry != 0
                                                                       ? _entities.Countries.First(
                                                                           p => p.Id == homeCountry).Name
                                                                       : "");
                            newRow[AdminResource.lbWorkCountry] = (workCountry != 0
                                                                       ? _entities.Countries.First(
                                                                           p => p.Id == workCountry).Name
                                                                       : "");
                            newRow[AdminResource.lbHomeCity] = (homeCity != 0
                                                                    ? _entities.Cities.First(p => p.Id == homeCity).Name
                                                                    : "");
                            newRow[AdminResource.lbWorkCity] = (workCity != 0
                                                                    ? _entities.Cities.First(p => p.Id == workCity).Name
                                                                    : "");
                            newRow[AdminResource.lbHomeTown] = (homeTown != 0
                                                                    ? _entities.Towns.First(p => p.Id == homeTown).Name
                                                                    : "");
                            newRow[AdminResource.lbWorkTown] = (workTown != 0
                                                                    ? _entities.Towns.First(p => p.Id == workTown).Name
                                                                    : "");
                            newRow[AdminResource.lbJobSector] = (jobSectorNo != 0
                                                                     ? _entities.JobSectors.First(
                                                                         p => p.Id == jobSectorNo).Name
                                                                     : "");
                            newRow[AdminResource.lbJob] = (jobNo != 0
                                                               ? _entities.Jobs.First(p => p.Id == jobNo).Name
                                                               : "");

                            newRow[AdminResource.lbBirthDate] = (!string.IsNullOrEmpty(item["BirthDate"].ToString())
                                                                     ? Convert.ToDateTime(item["BirthDate"]).
                                                                           ToShortDateString()
                                                                     : "");
                            newRow[AdminResource.lbMembershipDate] =
                                (!string.IsNullOrEmpty(item["MembershipDate"].ToString())
                                     ? Convert.ToDateTime(item["MembershipDate"]).ToShortDateString()
                                     : "");
                            newRow[AdminResource.lbMarriageDate] =
                                (!string.IsNullOrEmpty(item["MarriageDate"].ToString())
                                     ? Convert.ToDateTime(item["MarriageDate"]).ToShortDateString()
                                     : "");
                            newRow[AdminResource.lbDeceaseDate] = (!string.IsNullOrEmpty(item["DeceaseDate"].ToString())
                                                                       ? Convert.ToDateTime(item["DeceaseDate"]).
                                                                             ToShortDateString()
                                                                       : "");

                            newRow[AdminResource.lbLastSchoolGraduateDate] = ((item["LastSchoolGraduateDate"] !=
                                                                               DBNull.Value &&
                                                                               item["LastSchoolGraduateDate"] != null)
                                                                                  ? Convert.ToDateTime(
                                                                                      item["LastSchoolGraduateDate"])
                                                                                        .Year.ToString()
                                                                                  : "");
                            newRow["TC"] = ((item["TC"] != DBNull.Value && item["TC"] != null)
                                                ? item["TC"].ToString()
                                                : "");
                            newRow[AdminResource.lbBloodType] = (bloodType != null ? bloodType.Name : "");

                            #endregion

                            xlsUsersDataTable.Rows.Add(newRow);
                        }

                        #region oluşturulan dataTable gridView e bind edilerek download edilir

                        view.DataSource = xlsUsersDataTable;
                        view.DataBind();

                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();

                        Response.ContentEncoding = Encoding.GetEncoding("windows-1254");
                        Response.Charset = "windows-1254"; //ISO-8859-13 ISO-8859-9  windows-1254

                        Response.Buffer = true;
                        EnableViewState = false;
                        Response.ContentType = "application/vnd.xls";
                        Response.AddHeader("content-disposition",
                                           "attachment;filename=" + DateTime.Now.ToShortDateString() + ".xls");
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
        }

        protected void BtSendEmailClick(object sender, EventArgs e)
        {
            cSendMail.MembersSqlQuery = Crypto.Decrypt(hfSearchResaultCmd.Value);
            cSendMail.Conn = _oConnection;

            var tbJobName = ((TextBox) cSendMail.FindControl("tbJobName"));
            var dpMailSendDate = ((RadDatePicker) cSendMail.FindControl("dpMailSendDate"));
            var ddlMailTemplates = ((DropDownList) cSendMail.FindControl("ddlMailTemplates"));
            var tbMailSubject = ((TextBox) cSendMail.FindControl("tbMailSubject"));
            var hfSqlQuery = ((HiddenField) cSendMail.FindControl("hfSqlQuery"));

            hfSqlQuery.Value = hfSearchResaultCmd.Value;
            tbJobName.Text = string.Empty;
            tbMailSubject.Text = string.Empty;
            dpMailSendDate.SelectedDate = null;
            EnrollMembershipHelper.BindDDlMailTemplates(ddlMailTemplates);
            cSendMail.GetMembers();
            mvMemberSearch.SetActiveView(vSendEmail);
        }

        protected void BtSendSmsClick(object sender, EventArgs e)
        {
            cSendSms.MembersSqlQuery = Crypto.Decrypt(hfSearchResaultCmd.Value);
            cSendSms.Conn = _oConnection;

            var tbJobName = ((TextBox) cSendSms.FindControl("tbJobName"));
            var dpSmsSendDate = ((RadDatePicker) cSendSms.FindControl("dpSmsSendDate"));

            tbJobName.Text = string.Empty;
            dpSmsSendDate.SelectedDate = null;
            cSendSms.GetMembers();
            mvMemberSearch.SetActiveView(vSendSms);
        }

        protected void BtFinanceManagerClick(object sender, EventArgs e)
        {
            cMemberFinanceManager.MembersSqlQuery = Crypto.Decrypt(hfSearchResaultCmd.Value);
            cMemberFinanceManager.Conn = _oConnection;

            var hfSqlQuery = ((HiddenField) cMemberFinanceManager.FindControl("hfSqlQuery"));
            hfSqlQuery.Value = hfSearchResaultCmd.Value;

            var mvFinanceManager = ((MultiView) cMemberFinanceManager.FindControl("mvFinanceManager"));
            mvFinanceManager.ActiveViewIndex = 0;

            cMemberFinanceManager.BindDdlDuesTypes();
            cMemberFinanceManager.BindDdlPymtPaymentType();
            mvMemberSearch.SetActiveView(vFinanceManager);
            cMemberFinanceManager.BindMembers();
        }

        protected void BtBackToSearchPageClick(object sender, EventArgs e)
        {
            mvMemberSearch.SetActiveView(vSearchMember);
        }

        protected void BtCancelSendMailClick(object sender, EventArgs e)
        {
            mvMemberSearch.SetActiveView(vSearchMember);
        }

        protected void BtCancelSendSmsClick(object sender, EventArgs e)
        {
            mvMemberSearch.SetActiveView(vSearchMember);
        }

        protected void BtCancelFinanceManagerClick(object sender, EventArgs e)
        {
            mvMemberSearch.SetActiveView(vSearchMember);
        }

        protected void gVMembers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
            var imgBtnMemberDelete = e.Row.FindControl("imgBtnMemberDelete") as ImageButton;

            if (btnEdit != null)
            {
                var userId = Convert.ToInt32(btnEdit.CommandArgument);
                if (
                    !EnrollMembershipHelper.IsAuthForThisProcess(userId,
                                                                 EnrollMembershipHelper.GetUserIdFromEmail(
                                                                     HttpContext.Current.User.Identity.Name)))
                {
                    btnEdit.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;",
                                                          AdminResource.msgNoAuth);
                }
            }

            if (imgBtnMemberDelete != null)
            {
                var userId = Convert.ToInt32(imgBtnMemberDelete.CommandArgument);
                if (
                    !EnrollMembershipHelper.IsAuthForThisProcess(userId,
                                                                 EnrollMembershipHelper.GetUserIdFromEmail(
                                                                     HttpContext.Current.User.Identity.Name)) ||
                    EnrollMembershipHelper.AreYouActiveUser(userId))
                {
                    imgBtnMemberDelete.OnClientClick = string.Format(
                        "javascript:showWarningToast('{0}');return false;", AdminResource.msgNoAuth);
                }
                else
                {
                    imgBtnMemberDelete.OnClientClick = " return confirm('" + AdminResource.lbConfirmMsgDeleteMember +
                                                       "'); ";
                }
            }
        }

        public void GetSearchResult()
        {
            btShowResults.Visible = false;

            #region EXEC SQL <search result count>

            try
            {
                _searchResultCountCommandText = Crypto.Decrypt(hfSearchResaultCountCmd.Value);
                _searchResultCommandText = Crypto.Decrypt(hfSearchResaultCmd.Value);
                _searchResultExcellCommandText = Crypto.Decrypt(hfSearchResaultExcellCmd.Value);

                var cmdSearchResaultCount = new SqlCommand
                                                {Connection = _oConnection, CommandText = _searchResultCountCommandText};
                if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                var resaultCount = Convert.ToInt32(cmdSearchResaultCount.ExecuteScalar());

                #endregion

                #region show results

                if (resaultCount > 0)
                {
                    btShowResults.Visible = true;
                    btClearSearchCriterias.Visible = true;
                    dvMemberActions.Visible = true;

                    ltResults.Text = string.Format("{0} {1}", resaultCount, AdminResource.lbFoundSearchResult);
                    ltResults.Visible = true;

                    #region search members

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(hfSearchResaultCmd.Value))
                        {
                            var searchCommandText = Crypto.Decrypt(hfSearchResaultCmd.Value);

                            var cmdSearchResault = new SqlCommand
                                                       {
                                                           Connection = _oConnection,
                                                           CommandText = searchCommandText
                                                       };

                            if (_oConnection.State == ConnectionState.Closed)
                                _oConnection.Open();

                            var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                            oAdaptor.Fill(_memberSearchDataTable);

                            gVMembers.DataSource = _memberSearchDataTable;
                            gVMembers.DataBind();
                            gVMembers.Visible = true;
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

                    #endregion
                }
                else
                {
                    btClearSearchCriterias.Visible = true;
                    ltResults.Visible = true;
                    ltResults.Text = string.Format("{0}", AdminResource.lbNoResult);
                }

                #endregion
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

        protected void ddlDecease_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlDecease.SelectedIndex)
            {
                case 1:
                    dpDeceaseDate.SelectedDate = null;
                    dpDeceaseDate2.SelectedDate = null;
                    dpDeceaseDate.Enabled = true;
                    dpDeceaseDate2.Enabled = true;
                    break;
                default:
                    dpDeceaseDate.SelectedDate = null;
                    dpDeceaseDate2.SelectedDate = null;
                    dpDeceaseDate.Enabled = false;
                    dpDeceaseDate2.Enabled = false;
                    break;
            }
        }

        #region javascript show/hide - button/div

        public void ShowPersInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv1",
                                                    "<script> showUserInfo('dPersonalDetail');</script>");
        }

        public void ShowHomeInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv3",
                                                    "<script> showUserInfo('dHomeDetail');</script>");
        }

        public void ShowMemberInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv3",
                                                    "<script> showUserInfo('dMemberDetail');</script>");
        }

        public void ShowWorkInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv5",
                                                    "<script> showUserInfo('dWorkDetail');</script>");
        }

        #endregion

        #region *Search Member

        protected void BtSearchClick(object sender, EventArgs e)
        {
            try
            {
                #region Table 'Users' WHERE Params

                if (!string.IsNullOrWhiteSpace(tbName.Text))
                    AddWhereParams(DbTable.User, string.Format("u.Name Like '%{0}%'", tbName.Text));

                if (!string.IsNullOrWhiteSpace(tbName.Text))
                    AddWhereParams(DbTable.User, string.Format("u.Name Like '%{0}%'", tbName.Text));

                if (!string.IsNullOrWhiteSpace(tbSurname.Text))
                    AddWhereParams(DbTable.User, string.Format("u.Surname Like '%{0}%'", tbSurname.Text));

                if (!string.IsNullOrWhiteSpace(tbEmail.Text))
                    AddWhereParams(DbTable.User, string.Format("u.Email Like '%{0}%'", tbEmail.Text));

                if (ddlIsAdmin.SelectedIndex > 0)
                {
                    var isAdmin = "1";
                    if (ddlIsAdmin.SelectedIndex == 1)
                    {
                        isAdmin = "1";
                    }
                    else if (ddlIsAdmin.SelectedIndex == 2)
                    {
                        isAdmin = "0";
                    }
                    AddWhereParams(DbTable.User, string.Format("u.Admin = {0}", isAdmin));
                }

                #endregion

                #region Table 'UserGeneral' WHERE Params

                #region PersonalInfo

                if (ddlBloodType.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.BloodType={0}", ddlBloodType.SelectedItem.Value));

                if (ddlGender.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.Gender={0}", ddlGender.SelectedItem.Value));

                if (ddlMaritalStatus.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.MaritalStatus={0}", ddlMaritalStatus.SelectedItem.Value));

                if (!string.IsNullOrEmpty(tbBirthPlace.Text))
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.Birthplace LIKE '{0}'", tbBirthPlace.Text));

                #endregion

                #region HomeInfo

                if (ddlHomeCountry.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.HomeCountry={0}", ddlHomeCountry.SelectedItem.Value));
                if (ddlHomeCity.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.HomeCity={0}", ddlHomeCity.SelectedItem.Value));
                if (ddlHomeTown.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.HomeTown={0}", ddlHomeTown.SelectedItem.Value));

                #endregion

                #region WorkInfo

                if (ddlWorkCountry.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.WorkCountry={0}", ddlWorkCountry.SelectedItem.Value));
                if (ddlWorkCity.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.WorkCity={0}", ddlWorkCity.SelectedItem.Value));
                if (ddlWorkTown.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.WorkTown={0}", ddlWorkTown.SelectedItem.Value));
                if (ddlJobSectors.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.JobSectorNo={0}", ddlJobSectors.SelectedItem.Value));
                if (ddlJobs.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.JobNo={0}", ddlJobs.SelectedItem.Value));
                if (!string.IsNullOrEmpty(tbWorkTitle.Text))
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.WorkTitle LIKE '%{0}%'", tbWorkTitle.Text));
                if (!string.IsNullOrEmpty(tbWorkCorparation.Text))
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.WorkCorporation LIKE '%{0}%'", tbWorkCorparation.Text));

                #endregion

                #region EducationInfo

                if (!string.IsNullOrEmpty(tbLastSchool.Text))
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.LastSchool LIKE '%{0}%'", tbLastSchool.Text));

                if (dpLastSchoolGraduateDate.SelectedDate != null &&
                    !string.IsNullOrEmpty(dpLastSchoolGraduateDate.SelectedDate.Value.Year.ToString()))
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.LastSchoolGraduateDate >= '{0}'",
                                                 dpLastSchoolGraduateDate.SelectedDate.Value.Year.ToString()));
                if (dpLastSchoolGraduateDate2.SelectedDate != null &&
                    !string.IsNullOrEmpty(dpLastSchoolGraduateDate2.SelectedDate.Value.Year.ToString()))
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.LastSchoolGraduateDate <= '{0}'",
                                                 dpLastSchoolGraduateDate2.SelectedDate.Value.Year.ToString()));

                #endregion

                #region OtherInfo

                if (!string.IsNullOrEmpty(tbAdminNote.Text))
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.AdminNote like '%{0}%'", tbAdminNote.Text));

                if (ddlDecease.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserGeneral, string.Format("g.Decease={0}", ddlDecease.SelectedItem.Value));

                if (dpDeceaseDate.SelectedDate != null &&
                    !string.IsNullOrEmpty(dpDeceaseDate.SelectedDate.Value.Year.ToString()))
                {
                    DateTime date = dpDeceaseDate.SelectedDate.Value;
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.DeceaseDate >= '{0}.{1}{2}.{3}{4} 00:00:00.000'",
                                                 date.Year, date.Month < 10 ? "0" : "", date.Month,
                                                 date.Day < 10 ? "0" : "", date.Day));
                }

                if (dpDeceaseDate2.SelectedDate != null &&
                    !string.IsNullOrEmpty(dpDeceaseDate2.SelectedDate.Value.Year.ToString()))
                {
                    DateTime date = dpDeceaseDate2.SelectedDate.Value;
                    AddWhereParams(DbTable.UserGeneral,
                                   string.Format("g.DeceaseDate <= '{0}.{1}{2}.{3}{4} 23:59:59.999'",
                                                 date.Year, date.Month < 10 ? "0" : "", date.Month,
                                                 date.Day < 10 ? "0" : "", date.Day));
                }

                #endregion

                #endregion

                CreateWhereParamMemberState(); //üyelik durumuna göre filtreleme yapılıyor

                #region Table 'UserFoundation' WHERE Params

                if (!string.IsNullOrEmpty(tbMembershipNumber.Text))
                    AddWhereParams(DbTable.UserFoundation, string.Format("f.MemberNo >= '{0}'", tbMembershipNumber.Text));
                if (!string.IsNullOrEmpty(tbMembershipNumber2.Text))
                    AddWhereParams(DbTable.UserFoundation,
                                   string.Format("f.MemberNo <= '{0}'", tbMembershipNumber2.Text));

                if (!string.IsNullOrEmpty(tbSpecialNumber.Text))
                    AddWhereParams(DbTable.UserFoundation, string.Format("f.SpecialNo >= '{0}'", tbSpecialNumber.Text));
                if (!string.IsNullOrEmpty(tbSpecialNumber2.Text))
                    AddWhereParams(DbTable.UserFoundation, string.Format("f.SpecialNo <= '{0}'", tbSpecialNumber2.Text));

                if (dpTerm.SelectedDate != null && !string.IsNullOrEmpty(dpTerm.SelectedDate.Value.Year.ToString()))
                    AddWhereParams(DbTable.UserFoundation,
                                   string.Format("f.Term >= '{0}'", dpTerm.SelectedDate.Value.Year.ToString()));
                if (dpTerm2.SelectedDate != null && !string.IsNullOrEmpty(dpTerm2.SelectedDate.Value.Year.ToString()))
                    AddWhereParams(DbTable.UserFoundation,
                                   string.Format("f.Term <= '{0}'", dpTerm2.SelectedDate.Value.Year.ToString()));

                if (dpMembershipDate.SelectedDate != null &&
                    !string.IsNullOrWhiteSpace(dpMembershipDate.SelectedDate.Value.ToShortDateString()))
                {
                    DateTime date = dpMembershipDate.SelectedDate.Value;
                    AddWhereParams(DbTable.UserFoundation,
                                   string.Format("f.MembershipDate >= '{0}.{1}{2}.{3}{4} 00:00:00.000'",
                                                 date.Year, date.Month < 10 ? "0" : "", date.Month,
                                                 date.Day < 10 ? "0" : "", date.Day));
                }
                if (dpMembershipDate2.SelectedDate != null &&
                    !string.IsNullOrWhiteSpace(dpMembershipDate2.SelectedDate.Value.ToShortDateString()))
                {
                    DateTime date = dpMembershipDate2.SelectedDate.Value;
                    AddWhereParams(DbTable.UserFoundation,
                                   string.Format("f.MembershipDate <= '{0}.{1}{2}.{3}{4} 23:59:59.999'",
                                                 date.Year, date.Month < 10 ? "0" : "", date.Month,
                                                 date.Day < 10 ? "0" : "", date.Day));
                }

                if (ddlMembershipRelType.SelectedIndex > 0)
                    AddWhereParams(DbTable.UserFoundation,
                                   string.Format("f.MemberRelType = {0}", ddlMembershipRelType.SelectedItem.Value));

                #endregion

                #region Table 'UserFinance' WHERE Params

                if (ddlIsAutoPay.SelectedIndex > 0)
                {
                    var autoPay = "1";
                    if (ddlIsAutoPay.SelectedIndex == 1)
                    {
                        autoPay = "1";
                    }
                    else if (ddlIsAutoPay.SelectedIndex == 2)
                    {
                        autoPay = "0";
                    }
                    AddWhereParams(DbTable.UserFinance, string.Format("fin.AutoPay = {0}", autoPay));
                }

                #endregion

                #region sorgularla bağlanan tabloları Inner Join ile sorguya alırız

                _innerJoin += " Inner Join UserGeneral as g on u.Id=g.UserId ";
                _innerJoin += " Inner Join UserFoundation as f on u.Id=f.UserId ";
                _innerJoin += " Inner Join UserFinance as fin on u.Id=fin.UserId ";
                _innerJoin += " Inner Join UserEmails as e on (u.Id=e.UserId AND e.MainAddress=1) ";

                #endregion

                #region create SQL query

                //sonuçları ve sayısını döndüren sql sorugusunun tam hali oluşturulur
                if (!string.IsNullOrEmpty(_searchCommandUsersWhere.ToString()) ||
                    !string.IsNullOrEmpty(_searchCommandUserGeneralWhere.ToString()) ||
                    !string.IsNullOrEmpty(_searchCommandUserMembershipWhere.ToString())) _innerJoin += " WHERE ";
                _searchResultCommandText = string.Format(_sqlSearchQuery,
                                                         _innerJoin,
                                                         _searchCommandUsersWhere,
                                                         _searchCommandUserGeneralWhere,
                                                         _searchCommandUserMembershipWhere);
                _searchResultExcellCommandText = string.Format(_sqlSearchExcellQuery,
                                                               _innerJoin,
                                                               _searchCommandUsersWhere,
                                                               _searchCommandUserGeneralWhere,
                                                               _searchCommandUserMembershipWhere);
                _searchResultCountCommandText = string.Format(_sqlSearchCountQuery,
                                                              _innerJoin,
                                                              _searchCommandUsersWhere,
                                                              _searchCommandUserGeneralWhere,
                                                              _searchCommandUserMembershipWhere);

                #endregion

                #region sql sorguları hidden field da saklanır

                hfSearchResaultCmd.Value = Crypto.Encrypt(_searchResultCommandText);
                hfSearchResaultExcellCmd.Value = Crypto.Encrypt(_searchResultExcellCommandText);
                hfSearchResaultCountCmd.Value = Crypto.Encrypt(_searchResultCountCommandText);

                #endregion

                #region EXEC SQL

                tbSqlQueryOutput.Text = _searchResultCommandText;
                tbSqlQueryOutput.Visible = true;
                var cmdSearchResaultCount = new SqlCommand
                                                {Connection = _oConnection, CommandText = _searchResultCountCommandText};
                if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                var resaultCount = Convert.ToInt32(cmdSearchResaultCount.ExecuteScalar());

                #endregion

                #region show results

                if (resaultCount > 0)
                {
                    btShowResults.Visible = true;
                    btClearSearchCriterias.Visible = true;
                    dvMemberActions.Visible = true;

                    ltResults.Text = string.Format("{0} {1}", resaultCount, AdminResource.lbFoundSearchResult);
                    ltResults.Visible = true;
                }
                else
                {
                    btClearSearchCriterias.Visible = true;
                    ltResults.Visible = true;
                    ltResults.Text = string.Format("{0}", AdminResource.lbNoResult);
                }
                gVMembers.Visible = false;

                #endregion
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            finally
            {
                if (_oConnection.State == ConnectionState.Open)
                    _oConnection.Close();
            }
        }

        public void CreateWhereParamMemberState()
        {
            if (ddlMemberState.SelectedIndex > 0)
            {
                var data = Crypto.Decrypt(ddlMemberState.SelectedItem.Value).Split('|');
                //üye kontrolü => [User.State|UserFoundation.MemberState|UserEmails.Activated]
                var filterUserState = data[0];
                var filterMembersState = data[1];
                var filterMailActivation = data[2];

                //lbUnverified; [0|0|0]
                if (filterUserState == "0" && filterMembersState == "0" && filterMailActivation == "0")
                {
                    AddWhereParams(DbTable.User, "u.State=0 AND  f.MemberState=0 AND e.Activated=0");
                }
                    //lbWaitingForApproval; [0|0|1]
                else if (filterUserState == "0" && filterMembersState == "0" && filterMailActivation == "1")
                {
                    AddWhereParams(DbTable.User, "u.State=0 AND  f.MemberState=0 AND e.Activated=1");
                }
                    //lbActive; [1|1|1]
                else if (filterUserState == "1" && filterMembersState == "1" && filterMailActivation == "1")
                {
                    AddWhereParams(DbTable.User, "u.State=1 AND  f.MemberState=1 AND e.Activated=1");
                }
                    //üyelik pasif edilmiş;  [0|1|1]
                else if (filterUserState == "0" && filterMembersState == "1" && filterMailActivation == "1")
                {
                    AddWhereParams(DbTable.User, "u.State=0 AND  f.MemberState=1 AND e.Activated=1");
                }
            }
        }

        private void AddWhereParams(DbTable table, string sb)
        {
            switch (table)
            {
                case DbTable.User:
                    _searchCommandUsersWhere.AppendFormat(_parametersCount != 0 ? " and {0}" : "{0}", sb);
                    break;
                case DbTable.UserGeneral:
                    _searchCommandUserGeneralWhere.AppendFormat(_parametersCount != 0 ? " and {0}" : "{0}", sb);
                    break;
                case DbTable.UserFoundation:
                    _searchCommandUserMembershipWhere.AppendFormat(_parametersCount != 0 ? " and {0}" : "{0}", sb);
                    break;
                case DbTable.UserFinance:
                    _searchCommandUserMembershipWhere.AppendFormat(_parametersCount != 0 ? " and {0}" : "{0}", sb);
                    break;
            }

            _parametersCount++;
        }

        #endregion

        #region Update/Cancel/Delete Member

        protected void ImgBtnMemberEditClick(object sender, ImageClickEventArgs e)
        {
            var btnMemberEdit = sender as ImageButton;
            if (btnMemberEdit != null)
            {
                hfUserId.Value = btnMemberEdit.CommandArgument;
                int userId = Convert.ToInt32(hfUserId.Value);
                Member = cMemberAddEdit.BindForEditMember(userId);
                if (Member != null)
                {
                    var btnConfirmMemberActive = ((Button) cMemberAddEdit.FindControl("BtnConfirmMemberActive"));
                    btnConfirmMemberActive.Visible =
                        EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                        EnrollMembershipHelper.MembershipType.OnayBekleyen;

                    var btnSetPassive = ((Button) cMemberAddEdit.FindControl("BtnSetPassive"));
                    btnSetPassive.Visible =
                        EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                        EnrollMembershipHelper.MembershipType.Aktif;

                    var btnSetActive = ((Button) cMemberAddEdit.FindControl("BtnSetActive"));
                    btnSetActive.Visible =
                        EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                        EnrollMembershipHelper.MembershipType.Pasif;

                    var btnResendActivationCode = ((Button) cMemberAddEdit.FindControl("BtnResendActivationCode"));
                    btnResendActivationCode.Visible =
                        EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                        EnrollMembershipHelper.MembershipType.Hatali;

                    cMemberAddEdit.BindPersonalInfo();
                    cMemberAddEdit.BindMemberInfo();
                    cMemberAddEdit.BindHomeInfo();
                    cMemberAddEdit.BindWorkInfo();
                    cMemberAddEdit.BindEmailInfo();
                }
                mvMemberSearch.SetActiveView(vEditMember);
            }
        }

        protected void ImgBtnMemberDeleteClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                var userId = 0;
                var btnDelete = sender as ImageButton;
                if (btnDelete != null)
                {
                    var id = btnDelete.CommandArgument;
                    if (id != null) userId = Convert.ToInt32(id);
                    if (userId > 0)
                    {
                        var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                        if (user != null && KullanicininBilgileriniSil(user.Id, _entities))
                        {
                            _entities.Users.DeleteObject(user);
                            _entities.SaveChanges();
                            GetSearchResult();
                            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }

        #endregion

        #region Delete User Related Tables

        public bool KullanicininBilgileriniSil(int userId, Entities entities)
        {
            try
            {
                //UserEmail
                var userEmails = entities.UserEmails.Where(p => p.UserId == userId).ToList();
                if (userEmails.Count > 0)
                {
                    foreach (var userEmail in userEmails)
                    {
                        entities.DeleteObject(userEmail);
                    }
                    //entities.SaveChanges();
                }

                //UserFoundation
                var userFoundations = entities.UserFoundation.Where(p => p.UserId == userId).ToList();
                if (userFoundations.Count > 0)
                {
                    foreach (var userFoundation in userFoundations)
                    {
                        entities.DeleteObject(userFoundation);
                    }
                    //entities.SaveChanges();
                }

                //UserFinance
                var userFinances = entities.UserFinance.Where(p => p.UserId == userId).ToList();
                if (userFinances.Count > 0)
                {
                    foreach (var userFinance in userFinances)
                    {
                        entities.DeleteObject(userFinance);
                    }
                    //entities.SaveChanges();
                }

                //UserGeneral
                var userGenerals = entities.UserGeneral.Where(p => p.UserId == userId).ToList();
                if (userGenerals.Count > 0)
                {
                    foreach (var userGeneral in userGenerals)
                    {
                        entities.DeleteObject(userGeneral);
                    }
                    //entities.SaveChanges();
                }

                //UserRole
                var userRoles = entities.UserRole.Where(p => p.UserId == userId).ToList();
                if (userRoles.Count > 0)
                {
                    foreach (var userRole in userRoles)
                    {
                        entities.DeleteObject(userRole);
                    }
                    //entities.SaveChanges();
                }

                //UserDuesLog
                var userDuesLogs = entities.UserDuesLog.Where(p => p.UserId == userId).ToList();
                if (userDuesLogs.Count > 0)
                {
                    foreach (var userDuesLog in userDuesLogs)
                    {
                        entities.DeleteObject(userDuesLog);
                    }
                    //entities.SaveChanges();
                }

                //System_Logs
                var systemLogs = entities.System_Logs.Where(p => p.userId == userId).ToList();
                if (systemLogs.Count > 0)
                {
                    foreach (var systemLog in systemLogs)
                    {
                        entities.DeleteObject(systemLog);
                    }
                    //entities.SaveChanges();
                }
                return true;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                return false;
            }
        }

        #endregion

        #region Bind and Clear Search criterias

        private void BindSearchCriterias()
        {
            EnrollMembershipHelper.DataBindDdlMembershipRelType(ddlMembershipRelType);
            EnrollMembershipHelper.DataBindDdlGender(Member, ddlGender);
            EnrollMembershipHelper.DataBindDDlBloodType(Member, ddlBloodType);
            EnrollMembershipHelper.DataBindDdlMaritalStatus(Member, ddlMaritalStatus);
            EnrollMembershipHelper.DataBindDDlSectorsJobs(Member, ddlJobSectors, ddlJobs);
            EnrollMembershipHelper.DataBindDdlDecease(ddlDecease);
            EnrollMembershipHelper.DataBindIsAdmin(ddlIsAdmin);
            EnrollMembershipHelper.DataBindIsAutoPay(ddlIsAutoPay);
            EnrollMembershipHelper.BindDdlMemberStatesFilter(ddlMemberState);
            EnrollMembershipHelper.BindDdlCountries(ddlHomeCountry, ddlHomeCity, ddlHomeTown,
                                                    EnrollMembershipHelper.GetCountries());
            EnrollMembershipHelper.BindDdlCountries(ddlWorkCountry, ddlWorkCity, ddlWorkTown,
                                                    EnrollMembershipHelper.GetCountries());
        }

        protected void BtClearSearchCriteriasClick(object sender, EventArgs e)
        {
            //personal info
            ddlGender.SelectedIndex = 0;
            ddlBloodType.SelectedIndex = 0;
            ddlMaritalStatus.SelectedIndex = 0;
            ddlIsAdmin.SelectedIndex = 0;
            ddlIsAutoPay.SelectedIndex = 0;
            tbAdminNote.Text = string.Empty;
            ddlMemberState.SelectedIndex = 0;
            tbName.Text = string.Empty;
            tbSurname.Text = string.Empty;
            tbEmail.Text = string.Empty;
            tbBirthPlace.Text = string.Empty;

            //membership info
            tbMembershipNumber.Text = string.Empty;
            tbMembershipNumber2.Text = string.Empty;
            tbSpecialNumber.Text = string.Empty;
            tbSpecialNumber2.Text = string.Empty;
            ddlMembershipRelType.SelectedIndex = 0;
            dpTerm.SelectedDate = null;
            dpTerm2.SelectedDate = null;
            dpMembershipDate.SelectedDate = null;
            dpMembershipDate2.SelectedDate = null;
            btClearSearchCriterias.Visible = false;

            hfSearchResaultCmd.Value = string.Empty;
            hfSearchResaultCountCmd.Value = string.Empty;
            ltResults.Text = string.Empty;
            dvMemberActions.Visible = false;
            gVMembers.DataSource = string.Empty;
            gVMembers.DataBind();
            gVMembers.Visible = false;
        }

        #endregion

        #region OnSelectedIndexChanged home&work countries cities towns

        protected void ddlHomeCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfHomeCountry.Value = ddlHomeCountry.SelectedIndex > 0 ? ddlHomeCountry.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlCities(ddlHomeCity, ddlHomeTown,
                                                 EnrollMembershipHelper.GetCities(hfHomeCountry.Value));
        }

        protected void ddlHomeCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfHomeCity.Value = ddlHomeCity.SelectedIndex > 0 ? ddlHomeCity.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlTowns(ddlHomeTown,
                                                EnrollMembershipHelper.GetTowns(hfHomeCountry.Value, hfHomeCity.Value));
            ShowHomeInfo();
        }

        protected void ddlWorkCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCountry.Value = ddlWorkCountry.SelectedIndex > 0 ? ddlWorkCountry.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlCities(ddlWorkCity, ddlWorkTown,
                                                 EnrollMembershipHelper.GetCities(hfWorkCountry.Value));
        }

        protected void ddlWorkCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCity.Value = ddlWorkCity.SelectedIndex > 0 ? ddlWorkCity.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlTowns(ddlWorkTown,
                                                EnrollMembershipHelper.GetTowns(hfWorkCountry.Value, hfWorkCity.Value));
        }

        #endregion
    }
}