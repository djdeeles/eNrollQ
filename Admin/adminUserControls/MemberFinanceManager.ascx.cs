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
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    /// <summary>
    /// 	Database Table 'UserDuesLog' LogType(int) field is used for type of process
    /// 	LogType-> 0 : Borç
    /// 	LogType-> 1 : Ödeme
    /// </summary>
    public partial class MemberFinanceManager : UserControl
    {
        private readonly Entities _entities = new Entities();

        public SqlConnection Conn;
        public string MembersSqlQuery = string.Empty;

        public SqlConnection _oConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 33))
            {
                mvAuth.SetActiveView(vAuth);
                if (!IsPostBack)
                {
                    mvFinanceManager.SetActiveView(vDept);
                }

                gvFinanceManagement.Columns[0].HeaderText = AdminResource.lbActions;
                gvFinanceManagement.Columns[1].HeaderText = string.Format("{0} {1}", AdminResource.lbName,
                                                                          AdminResource.lbSurname);
                gvFinanceManagement.Columns[2].HeaderText = AdminResource.lbGeneralDebt;

                gvChargesForDues.Columns[0].HeaderText = AdminResource.lbDetail;
                gvChargesForDues.Columns[1].HeaderText = AdminResource.lbDuesType;
                gvChargesForDues.Columns[2].HeaderText = AdminResource.lbPaymentType;
                gvChargesForDues.Columns[3].HeaderText = AdminResource.lbAmount;
                gvChargesForDues.Columns[4].HeaderText = AdminResource.lbProcessDate;
                gvChargesForDues.Columns[5].HeaderText = AdminResource.lbProcessUser;
                gvChargesForDues.Columns[6].HeaderText = AdminResource.lbProcess;

                btnBackToDept.Text = AdminResource.lbBack;
                btnBackToDept2.Text = AdminResource.lbBack;
                btAddDues.Text = AdminResource.lbAddDept;
                btPymtAddNewPayment.Text = AdminResource.lbAdd;
                btnExportExcel.Text = AdminResource.lbExcelExport;
            }
            else
            {
                mvAuth.SetActiveView(vNotAuth);
            }
        }

        public void BindMembers()
        {
            gvFinanceManagement.DataSource = GetMembersTable();
            gvFinanceManagement.DataBind();
        }

        public DataTable GetMembersTable()
        {
            var table = new DataTable();
            if (!string.IsNullOrWhiteSpace(hfSqlQuery.Value))
            {
                MembersSqlQuery = Crypto.Decrypt(hfSqlQuery.Value);
                if (Conn == null || MembersSqlQuery == string.Empty)
                    Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());

                var cmdSearchResault = new SqlCommand
                                           {
                                               Connection = _oConnection,
                                               CommandText = MembersSqlQuery
                                           };

                if (_oConnection.State == ConnectionState.Closed)
                    _oConnection.Open();

                var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                oAdaptor.Fill(table);
            }
            return table;
        }

        public void BindDdlDuesTypes()
        {
            var duesTypes = _entities.DuesTypes.OrderBy(p => p.Title);
            ddlDuesType.Items.Clear();

            var i = 1;
            ddlDuesType.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            foreach (var duesType in duesTypes)
            {
                ddlDuesType.Items.Insert(i, new ListItem(duesType.Title, Crypto.Encrypt(duesType.Id.ToString())));
                i++;
            }
        }

        public void BindDdlPymtPaymentType()
        {
            ddlPymtPaymentType.Items.Clear();
            var paymentTypes = _entities.DuesPaymentTypes.Where(p => p.State).OrderBy(p => p.Title).ToList();
            ddlPymtPaymentType.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            var i = 1;
            foreach (var paymentType in paymentTypes)
            {
                var type = string.Empty;
                try
                {
                    switch (paymentType.Id)
                    {
                        case 1:
                            type = AdminResource.lbPaymentType1;
                            break;
                        case 2:
                            type = AdminResource.lbPaymentType2;
                            break;
                        case 3:
                            type = AdminResource.lbPaymentType3;
                            break;
                        case 4:
                            type = AdminResource.lbPaymentType4;
                            break;
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
                ddlPymtPaymentType.Items.Insert(i, new ListItem(type, paymentType.Id.ToString()));
                i++;
            }
        }

        protected void ddlDuesType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlDuesType.SelectedIndex > 0)
                {
                    hfDues.Value = ddlDuesType.SelectedItem.Value;
                    var duesId = Convert.ToInt32(Crypto.Decrypt(ddlDuesType.SelectedItem.Value));
                    ltDues.Text = string.Empty;
                    ltDues.Text = string.Format("{0}{1}{2}",
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""),
                                                _entities.DuesTypes.Single(p => p.Id == duesId).Amount.ToString(".00"),
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""));
                }
                else
                {
                    hfDues.Value = string.Empty;
                    ltDues.Text = string.Empty;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void GvFinanceManagement_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnGoFinanceDetail = e.Row.FindControl("btnGoFinanceDetail") as Button;
            if (btnGoFinanceDetail != null) btnGoFinanceDetail.Text = AdminResource.lbDuesHistory;

            var btnGoTakePayment = e.Row.FindControl("btnGoTakePayment") as Button;
            if (btnGoTakePayment != null) btnGoTakePayment.Text = AdminResource.lbAddPaymentData;
        }

        protected void BtnGoFinanceDetailClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var btnGoFinancalDetail = sender as Button;
                if (btnGoFinancalDetail != null)
                {
                    var id = Convert.ToInt32(btnGoFinancalDetail.CommandArgument);
                    if (id > 0)
                    {
                        hfUserFinanceId.Value = Crypto.Encrypt(id.ToString());
                        var userFinance = _entities.UserFinance.FirstOrDefault(p => p.Id == id);
                        if (userFinance != null)
                        {
                            hfMemberId.Value = userFinance.UserId.ToString();

                            var user = _entities.Users.FirstOrDefault(p => p.Id == userFinance.UserId);
                            var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == userFinance.UserId);
                            var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == user.Id);

                            lbFDDeptAmount.Text = EnrollMembershipHelper.DebtValue(userFinance.Dept);

                            iUserPhoto.ImageUrl = (userGeneral != null && !string.IsNullOrEmpty(userGeneral.PhotoUrl)
                                                       ? userGeneral.PhotoUrl
                                                       : "/App_Themes/mainTheme/images/noimage.png");
                            iUserPhoto2.ImageUrl = (userGeneral != null && !string.IsNullOrEmpty(userGeneral.PhotoUrl)
                                                        ? userGeneral.PhotoUrl
                                                        : "/App_Themes/mainTheme/images/noimage.png");


                            if (userFoundation != null)
                            {
                                var userRelationType =
                                    _entities.FoundationRelType.FirstOrDefault(p => p.Id == userFoundation.MemberRelType);

                                lbFDCorporationNumber.Text = userFoundation.MemberNo;
                                if (userRelationType != null) lbFDRelationship.Text = userRelationType.Name;
                            }
                            if (user != null)
                            {
                                hfMemberId.Value = user.Id.ToString();
                                lbFDNameSurname.Text = user.Name + " " + user.Surname;

                                if (userGeneral != null && userGeneral.LastSchoolGraduateDate != null)
                                {
                                    lbFDGraduationYear.Text = userGeneral.LastSchoolGraduateDate.Value.Year.ToString();
                                }
                            }
                        }
                        mvFinanceManager.SetActiveView(vFinanceDetail);
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void BtnGoTakePaymentClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var btnGoTakePayment = sender as Button;
                if (btnGoTakePayment != null)
                {
                    var id = Convert.ToInt32(btnGoTakePayment.CommandArgument);
                    if (id > 0)
                    {
                        hfUserFinanceId.Value = Crypto.Encrypt(id.ToString());
                        var userFinance = _entities.UserFinance.FirstOrDefault(p => p.Id == id);
                        if (userFinance != null)
                        {
                            hfMemberId.Value = userFinance.UserId.ToString();

                            var user = _entities.Users.FirstOrDefault(p => p.Id == userFinance.UserId);
                            var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == userFinance.UserId);
                            var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == user.Id);
                            if (!string.IsNullOrWhiteSpace(tbPymtReceiptNumber.Text))
                            {
                                var recepitNumber = tbPymtReceiptNumber.Text.Trim('\'').Trim('"').Trim(' ');
                                var receipt = _entities.UserDuesLog.FirstOrDefault(p => p.ReceiptNo == recepitNumber);
                                if (receipt != null)
                                {
                                    MessageBox.Show(MessageType.Warning,
                                                    AdminResource.msgTheReceiptInvoiceNumberUsedBefore);
                                }
                            }

                            lbPymtDeptAmount.Text = EnrollMembershipHelper.DebtValue(userFinance.Dept);
                            if (userFoundation != null)
                            {
                                var userRelationType =
                                    _entities.FoundationRelType.FirstOrDefault(p => p.Id == userFoundation.MemberRelType);

                                lbPymtCorpNumber.Text = userFoundation.MemberNo;
                                if (userRelationType != null) lbPymtRelationship.Text = userRelationType.Name;
                            }
                            if (user != null)
                            {
                                lbPymtNameSurname.Text = user.Name + " " + user.Surname;

                                if (userGeneral != null && userGeneral.LastSchoolGraduateDate != null)
                                {
                                    lbPymtGraduateDate.Text = userGeneral.LastSchoolGraduateDate.Value.Year.ToString();

                                    iUserPhoto.ImageUrl = !string.IsNullOrWhiteSpace(userGeneral.PhotoUrl)
                                                              ? userGeneral.PhotoUrl
                                                              : "/App_Themes/mainTheme/images/noimage.png";
                                    iUserPhoto2.ImageUrl = !string.IsNullOrWhiteSpace(userGeneral.PhotoUrl)
                                                               ? userGeneral.PhotoUrl
                                                               : "/App_Themes/mainTheme/images/noimage.png";
                                }
                            }
                        }

                        dpPymtPaymentDate.SelectedDate = DateTime.Now;
                        dpPymtReceiptDate.SelectedDate = DateTime.Now;
                        mvFinanceManager.SetActiveView(vTakePayment);
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected string GetUserName(int userId)
        {
            try
            {
                var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                if (user == null) return string.Empty;
                return string.Format("{0} {1}", user.Name, user.Surname);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return string.Empty;
        }

        protected string GetDuesType(int duesTypeId)
        {
            try
            {
                var duesType = _entities.DuesTypes.FirstOrDefault(p => p.Id == duesTypeId);
                return duesType == null ? string.Empty : duesType.Title;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return string.Empty;
        }

        protected string GetPaymentType(int paymentTypeId)
        {
            try
            {
                switch (paymentTypeId)
                {
                    case 1:
                        return AdminResource.lbPaymentType1;
                    case 2:
                        return AdminResource.lbPaymentType2;
                    case 3:
                        return AdminResource.lbPaymentType3;
                    case 4:
                        return AdminResource.lbPaymentType4;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return string.Empty;
        }

        protected void BackToDept(object sender, EventArgs e)
        {
            mvFinanceManager.SetActiveView(vDept);
        }

        public void UpdateUsersCurrentDept(string currentDept)
        {
            var dept = Convert.ToDecimal(currentDept);

            lbPymtDeptAmount.Text = lbFDDeptAmount.Text = EnrollMembershipHelper.DebtValue(dept);
            gvFinanceManagement.DataSource = GetMembersTable();
            gvFinanceManagement.DataBind();
        }

        public void UpdateUsersCurrentDept()
        {
            gvFinanceManagement.DataSource = GetMembersTable();
            gvFinanceManagement.DataBind();
            gvChargesForDues.DataBind();
        }

        public void ClearFormInputs()
        {
            tbPymtPaymentAmount.Text = string.Empty;
            tbPymtProvisionNo.Text = string.Empty;
            tbPymtReceiptNumber.Text = string.Empty;
            dpPymtPaymentDate.SelectedDate = DateTime.Now;
            dpPymtReceiptDate.SelectedDate = DateTime.Now;
            BindDdlPymtPaymentType();
        }

        protected void BtnExportExcel(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(hfMemberId.Value))
            {
                var memberId = Convert.ToInt32(hfMemberId.Value);
                var userDuesLogs = _entities.UserDuesLog.Where(p => p.UserId == memberId).ToList();
                var view = new GridView();
                try
                {
                    // yeni tablo oluşturulur
                    var xlsJobsDataTable = EnrollMembershipHelper.CreateFinanceDetailDataTable();
                    foreach (var userDuesLog in userDuesLogs)
                    {
                        var newRow = xlsJobsDataTable.NewRow();

                        var duesType = _entities.DuesTypes.FirstOrDefault(p => p.Id == userDuesLog.DuesType);
                        var paymentType =
                            _entities.DuesPaymentTypes.FirstOrDefault(p => p.Id == userDuesLog.PaymentTypeId);
                        var createdUser = _entities.Users.FirstOrDefault(p => p.Id == userDuesLog.CreatedUser);

                        newRow[AdminResource.lbDuesType] = duesType != null ? duesType.Title : "";
                        newRow[AdminResource.lbPaymentType] = paymentType != null ? paymentType.Title : "";

                        newRow[AdminResource.lbAmount] = EnrollMembershipHelper.AmountValueExcel(userDuesLog.Amount,
                                                                                                 userDuesLog.LogType);
                        newRow[AdminResource.lbReceiptInvoiceNumber] = userDuesLog.ReceiptNo;
                        newRow[AdminResource.lbReceiptInvoiceDate] = userDuesLog.ReceiptDate;
                        newRow[AdminResource.lbPaymentDate] = userDuesLog.PaymentDate;
                        newRow[AdminResource.lbProcess] = userDuesLog.LogType == 0
                                                              ? AdminResource.lbDebiting
                                                              : AdminResource.lbPayment;
                        newRow[AdminResource.lbProcessDate] = userDuesLog.CreatedTime.ToShortDateString() + " " +
                                                              userDuesLog.CreatedTime.ToShortTimeString();
                        newRow[AdminResource.lbProcessUser] = createdUser != null
                                                                  ? createdUser.Name + " " + createdUser.Surname
                                                                  : "";

                        xlsJobsDataTable.Rows.Add(newRow);
                    }

                    #region oluşturulan dataTable gridView e bind edilir

                    view.DataSource = xlsJobsDataTable;
                    view.DataBind();

                    #endregion

                    #region gridView download edilir

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
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
                finally
                {
                    if (_oConnection.State == ConnectionState.Open) _oConnection.Close();
                }
            }
        }

        public bool BeforeChargedDuesType(int userId, int duesId)
        {
            return _entities.UserDuesLog.FirstOrDefault(p => p.UserId == userId && p.DuesType == duesId) != null;
        }

        #region new Dues&payment

        protected void BtnAddDuesClick(object sender, EventArgs e)
        {
            var membersBeforeInDeptCount = 0;
            var membersNotBeforeDeptCount = 0;
            if (!string.IsNullOrWhiteSpace(hfDues.Value))
            {
                try
                {
                    var selectedDuesTypeId = Convert.ToInt32(Crypto.Decrypt(hfDues.Value));
                    var userId = 0;
                    var duesType = _entities.DuesTypes.FirstOrDefault(p => p.Id == selectedDuesTypeId);
                    if (duesType != null && !string.IsNullOrWhiteSpace(hfSqlQuery.Value))
                    {
                        var isUniqe = duesType.Uniqe;
                        var members = GetMembersTable();
                        if (members != null)
                        {
                            foreach (DataRow member in members.Rows)
                            {
                                userId = Convert.ToInt32(member["uId"]);
                                if (isUniqe && BeforeChargedDuesType(userId, duesType.Id))
                                {
                                    membersBeforeInDeptCount++;
                                }
                                else
                                {
                                    #region sorgu sonucu ile gelen listedeki tüm kullanıcıların genel borçları güncellenir

                                    decimal newDept = 0;
                                    var userFinance = _entities.UserFinance.FirstOrDefault(p => p.UserId == userId);
                                    if (userFinance != null)
                                    {
                                        newDept = userFinance.Dept + duesType.Amount;
                                        userFinance.Dept = newDept;
                                    }
                                    else
                                    {
                                        newDept = duesType.Amount;
                                        userFinance = new UserFinance();
                                        userFinance.UserId = userId;
                                        userFinance.Dept = newDept;
                                        _entities.AddToUserFinance(userFinance);
                                    }

                                    #endregion

                                    #region sorgu sonucu ile gelen listedeki tüm kullanıcıların borç logları tutulur

                                    var userDuesLog = new UserDuesLog();
                                    userDuesLog.UserId = userId;
                                    userDuesLog.Amount = duesType.Amount;
                                    userDuesLog.CreatedTime = DateTime.Now;
                                    userDuesLog.UpdatedTime = DateTime.Now;
                                    userDuesLog.LogType = 0;
                                    if (HttpContext.Current.User != null &&
                                        HttpContext.Current.User.Identity.IsAuthenticated)
                                    {
                                        var loginUser =
                                            _entities.Users.First(p => p.EMail == HttpContext.Current.User.Identity.Name);
                                        if (loginUser != null)
                                            userDuesLog.CreatedUser = loginUser.Id;
                                    }
                                    else
                                    {
                                        userDuesLog.CreatedUser = 0;
                                    }
                                    userDuesLog.DuesType = selectedDuesTypeId;
                                    _entities.AddToUserDuesLog(userDuesLog);

                                    #endregion

                                    _entities.SaveChanges();

                                    membersNotBeforeDeptCount++;
                                }
                            }

                            UpdateUsersCurrentDept();
                            if (membersBeforeInDeptCount > 0 && membersNotBeforeDeptCount > 0)
                            {
                                MessageBox.Show(MessageType.Success, AdminResource.lbSomeMembersBeforeChargedDues);
                            }
                            else if (membersBeforeInDeptCount > 0 && membersNotBeforeDeptCount == 0)
                            {
                                MessageBox.Show(MessageType.Notice, AdminResource.lbAllMembersBeforeChargedDues);
                            }
                            else if (membersBeforeInDeptCount == 0 && membersNotBeforeDeptCount > 0)
                            {
                                MessageBox.Show(MessageType.Success, AdminResource.lbAllMembersChargedDues);
                            }
                        }
                        else
                        {
                            MessageBox.Show(MessageType.Warning, AdminResource.lbNoMembersFound);
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

        protected void BtPymtAddNewPaymentOnClick(object sender, EventArgs e)
        {
            var userDuesLog = new UserDuesLog();
            try
            {
                if (!string.IsNullOrWhiteSpace(hfUserFinanceId.Value))
                {
                    Users user = null;
                    string UserFinanceMessage;

                    var controlReceipts =
                        _entities.UserDuesLog.Where(p => p.ReceiptNo == tbPymtReceiptNumber.Text).ToList();
                    if (controlReceipts.Count > 0)
                    {
                        MessageBox.Show(MessageType.Warning, AdminResource.msgReceiptNumberAlreadySaved);
                        return;
                    }

                    var id = Convert.ToInt32(Crypto.Decrypt(hfUserFinanceId.Value));
                    var userFinance = _entities.UserFinance.FirstOrDefault(p => p.Id == id);
                    if (userFinance == null)
                    {
                        userFinance = new UserFinance();
                        userFinance.UserId = Convert.ToInt32(Crypto.Decrypt(hfMemberId.Value));
                        _entities.AddToUserFinance(userFinance);
                        _entities.SaveChanges();
                    }
                    user = _entities.Users.FirstOrDefault(p => p.Id == userFinance.UserId);
                    userDuesLog.UserId = user != null ? user.Id : 0;
                    userFinance.Dept -= ((!string.IsNullOrWhiteSpace(tbPymtPaymentAmount.Text)
                                              ? Convert.ToDecimal(tbPymtPaymentAmount.Text)
                                              : 0));

                    if (userFinance.Dept > 0)
                    {
                        UserFinanceMessage = string.Format(AdminResource.msgUpdateGeneralDept,
                                                           (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                                ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                                : ""),
                                                           userFinance.Dept.ToString(".00"),
                                                           (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                                ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                                : ""));
                    }
                    else
                    {
                        UserFinanceMessage = AdminResource.msgNoUnpaidDept;
                    }
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var loginUser = _entities.Users.First(p => p.EMail == HttpContext.Current.User.Identity.Name);
                        if (loginUser != null)
                            userDuesLog.CreatedUser = loginUser.Id;
                    }
                    else
                    {
                        userDuesLog.CreatedUser = 0;
                    }

                    userDuesLog.PaymentTypeId = ddlPymtPaymentType.SelectedIndex > 0
                                                    ? Convert.ToInt32(ddlPymtPaymentType.SelectedItem.Value)
                                                    : 0;
                    userDuesLog.Amount = ((!string.IsNullOrWhiteSpace(tbPymtPaymentAmount.Text)
                                               ? Convert.ToDecimal(tbPymtPaymentAmount.Text)
                                               : 0));
                    userDuesLog.ProvisionNo = tbPymtProvisionNo.Text;
                    userDuesLog.ReceiptNo = tbPymtReceiptNumber.Text;
                    userDuesLog.CreatedTime = DateTime.Now;
                    userDuesLog.UpdatedTime = DateTime.Now;
                    userDuesLog.LogType = 1;
                    if (dpPymtPaymentDate.SelectedDate != null)
                        userDuesLog.PaymentDate = dpPymtPaymentDate.SelectedDate.Value;
                    if (dpPymtReceiptDate.SelectedDate != null)
                        userDuesLog.ReceiptDate = dpPymtReceiptDate.SelectedDate.Value;

                    _entities.AddToUserDuesLog(userDuesLog);

                    _entities.SaveChanges();
                    MessageBox.Show(MessageType.Notice, UserFinanceMessage);
                    MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                    ClearFormInputs();
                    UpdateUsersCurrentDept(userFinance.Dept.ToString());
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }

        #endregion
    }
}