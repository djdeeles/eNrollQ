using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Database Table 'UserDuesLog' LogType(int) field is used for type of process
    /// LogType-> 0 : Borç
    /// LogType-> 1 : Ödeme
    /// </summary>

    public partial class CorporationFinanceManager : System.Web.UI.UserControl
    {
        Entities _entities = new Entities();
        public SqlConnection _oConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
        public string CorporationsSqlQuery = string.Empty;
        public SqlConnection Conn;
        protected decimal DeptWithTax = 0; 
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 39))
            {
                mvAuth.SetActiveView(vAuth);
                if (!IsPostBack)
                {
                    mvFinanceManager.SetActiveView(vDept);
                }

                BindServiceTypes();
                BindTaxTypes();

                gvFinanceManagement.Columns[0].HeaderText = AdminResource.lbActions;
                gvFinanceManagement.Columns[1].HeaderText = AdminResource.lbName;
                gvFinanceManagement.Columns[2].HeaderText = AdminResource.lbContactAddress;
                gvFinanceManagement.Columns[3].HeaderText = AdminResource.lbInvoiceAddress;
                gvFinanceManagement.Columns[4].HeaderText = AdminResource.lbGeneralDebt;

                gvChargesForDues.Columns[0].HeaderText = AdminResource.lbDetail;
                gvChargesForDues.Columns[1].HeaderText = AdminResource.lbReceiptInvoiceNumber;
                gvChargesForDues.Columns[2].HeaderText = AdminResource.lbReceiptInvoiceDate;
                gvChargesForDues.Columns[3].HeaderText = AdminResource.lbPaymentType;
                gvChargesForDues.Columns[4].HeaderText = AdminResource.lbAmount;
                gvChargesForDues.Columns[5].HeaderText = AdminResource.lbProcessDate;
                gvChargesForDues.Columns[6].HeaderText = AdminResource.lbProcessUser;
                gvChargesForDues.Columns[7].HeaderText = AdminResource.lbProcess;


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

        public void BindServiceTypes()
        {
            ddlServiceTypes.Items.Clear();
            ddlServiceTypes.DataSource = _entities.ServiceTypes.OrderBy(p => p.Name).ToList();
            ddlServiceTypes.DataTextField = "Name";
            ddlServiceTypes.DataValueField = "Name";
            ddlServiceTypes.DataBind();
            ddlServiceTypes.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
        }

        public void BindTaxTypes()
        {
            ddlTaxType.Items.Clear();
            ddlTaxType.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            var taxTypes = _entities.TaxTypes.OrderBy(p => p.Value).ToList();
            var i = 1;
            foreach (var taxType in taxTypes)
            {
                ddlTaxType.Items.Insert(i, new ListItem(taxType.Value.ToString("0.00") + " (" + taxType.Name + ") ", taxType.Value.ToString("0.00")));
                i++;
            }
        }

        public void BindCorporations()
        {
            gvFinanceManagement.DataSource = GetCorporationsTable();
            gvFinanceManagement.DataBind();
        }

        public DataTable GetCorporationsTable()
        {
            var table = new DataTable();
            if (!string.IsNullOrWhiteSpace(hfSqlQuery.Value))
            {
                CorporationsSqlQuery = Crypto.Decrypt(hfSqlQuery.Value);
                if (Conn == null || CorporationsSqlQuery == string.Empty)
                    Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());

                var cmdSearchResault = new SqlCommand
                {
                    Connection = _oConnection,
                    CommandText = CorporationsSqlQuery
                };

                if (_oConnection.State == ConnectionState.Closed)
                    _oConnection.Open();

                var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                oAdaptor.Fill(table);
            }
            return table;
        }

        #region new Dues&payment
        protected void BtnAddDuesClick(object sender, EventArgs e)
        {
            decimal duesAmount = 0;
            var countUpdatedCorporation = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(tbDeptAmount.Text))
                {
                    var corporations = GetCorporationsTable();
                    if (corporations != null)
                    {
                        foreach (DataRow item in corporations.Rows)
                        {
                            var corporationId = Convert.ToInt32(item["cId"]); 
                            duesAmount = Convert.ToDecimal(tbDeptAmount.Text) + 
                                (Convert.ToDecimal(tbDeptAmount.Text) * Convert.ToDecimal(ddlTaxType.Items[ddlTaxType.SelectedIndex].Value)) ;
                           
                            #region sorgu sonucu ile gelen listedeki tüm kullanıcıların borç logları tutulur
                            var corporationDuesLog = new CorporationDuesLog();
                            corporationDuesLog.CorporationId = corporationId;

                            corporationDuesLog.Amount = duesAmount;
                            corporationDuesLog.CreatedTime = DateTime.Now;
                            corporationDuesLog.UpdatedTime = DateTime.Now;
                            corporationDuesLog.LogType = 0;
                            corporationDuesLog.Service = ddlServiceTypes.SelectedItem.Value;
                            corporationDuesLog.Note = tbAddDeptDescription.Text;
                            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                            {
                                var loginUser = _entities.Users.First(p => p.EMail == HttpContext.Current.User.Identity.Name);
                                if (loginUser != null) corporationDuesLog.CreatedUser = loginUser.Id;
                            }
                            else
                            {
                                corporationDuesLog.CreatedUser = 0;
                            }
                            _entities.AddToCorporationDuesLog(corporationDuesLog);
                            #endregion

                            countUpdatedCorporation++;
                            _entities.SaveChanges();

                            //Borçlandırma,
                            Logger.Add(33, 1, corporationDuesLog.Id, 1);
                            
                            tbDeptAmount.Text = string.Empty;
                            tbAddDeptDescription.Text = string.Empty;
                            ddlServiceTypes.SelectedIndex = 0;
                        }
                        UpdateCorporationsCurrentDept();
                        if (countUpdatedCorporation>1) MessageBox.Show(MessageType.Success, AdminResource.lbAllCorporationsChargedDues);
                        else MessageBox.Show(MessageType.Success, AdminResource.lbCorporationChargedDues);
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
        protected void BtPymtAddNewPaymentOnClick(object sender, EventArgs e)
        {
            var corporationsDuesLog = new CorporationDuesLog();
            try
            {
                if (!string.IsNullOrWhiteSpace(hfCorporationFinanceId.Value))
                {
                    Corporations corporation = null;
                    string corporationFinanceMessage;

                    #region yoksa finans tablosu oluşturulur
                    var id = Convert.ToInt32(Crypto.Decrypt(hfCorporationFinanceId.Value));
                    var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.Id == id);
                    if (corporationFinance == null)
                    {
                        corporationFinance = new CorporationFinance();
                        corporationFinance.CorporationId = Convert.ToInt32(Crypto.Decrypt(hfCorporationId.Value));
                        _entities.AddToCorporationFinance(corporationFinance);
                        _entities.SaveChanges();
                    }
                    #endregion
                    
                    corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationFinance.CorporationId);
                    corporationsDuesLog.CorporationId = corporation != null ? corporation.Id : 0;
                    corporationFinance.Dept -= ((!string.IsNullOrWhiteSpace(tbPymtPaymentAmount.Text) ? Convert.ToDecimal(tbPymtPaymentAmount.Text) : 0));

                    if (corporationFinance.Dept > 0)
                    {
                        corporationFinanceMessage = string.Format(AdminResource.msgUpdateGeneralDept,
                            (EnrollCurrency.SiteDefaultCurrency().Position == "L" ? EnrollCurrency.SiteDefaultCurrencyUnit() : ""),
                            corporationFinance.Dept.ToString("0.00"),
                            (EnrollCurrency.SiteDefaultCurrency().Position == "R" ? EnrollCurrency.SiteDefaultCurrencyUnit() : ""));
                    }
                    else
                    {
                        corporationFinanceMessage = AdminResource.msgNoUnpaidDept;
                    }
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var loginUser = _entities.Users.First(p => p.EMail == HttpContext.Current.User.Identity.Name);
                        if (loginUser != null) corporationsDuesLog.CreatedUser = loginUser.Id;
                    }
                    else
                    {
                        corporationsDuesLog.CreatedUser = 0;
                    }

                    corporationsDuesLog.PaymentTypeId = ddlPymtPaymentType.SelectedIndex > 0 ? Convert.ToInt32(ddlPymtPaymentType.SelectedItem.Value) : 0;
                    corporationsDuesLog.Amount = ((!string.IsNullOrWhiteSpace(tbPymtPaymentAmount.Text) ? Convert.ToDecimal(tbPymtPaymentAmount.Text) : 0));
                    corporationsDuesLog.ProvisionNo = tbPymtProvisionNo.Text; 
                    corporationsDuesLog.Note = tbPymtNote.Text;
                    corporationsDuesLog.CreatedTime = DateTime.Now;
                    corporationsDuesLog.UpdatedTime = DateTime.Now;
                    corporationsDuesLog.LogType = 1;
                    corporationsDuesLog.IsInvoiced= true;
                    if (dpPymtPaymentDate.SelectedDate != null)
                        corporationsDuesLog.PaymentDate = dpPymtPaymentDate.SelectedDate.Value; 


                    _entities.AddToCorporationDuesLog(corporationsDuesLog);
                    _entities.SaveChanges();
                    UpdateCorporationsCurrentDept(corporationFinance.Dept);

                    //Ödeme,
                    Logger.Add(33, 2, corporationsDuesLog.Id, 1);

                    MessageBox.Show(MessageType.Notice, corporationFinanceMessage);
                    MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                    ClearFormInputs();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        #endregion

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
                        case 1: type = AdminResource.lbPaymentType1; break;
                        case 2: type = AdminResource.lbPaymentType2; break;
                        case 3: type = AdminResource.lbPaymentType3; break;
                        case 4: type = AdminResource.lbPaymentType4; break;
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

        protected void GvFinanceManagement_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnGoFinanceDetail = e.Row.FindControl("btnGoFinanceDetail") as Button;
            var btnGoPayment = e.Row.FindControl("btnGoPayment") as Button;

            if (btnGoFinanceDetail != null) btnGoFinanceDetail.Text = AdminResource.lbFinanceHistory;
            if (btnGoPayment != null) btnGoPayment.Text = AdminResource.lbAddPaymentData;
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
                        hfCorporationFinanceId.Value = Crypto.Encrypt(id.ToString());
                        var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.Id == id);
                        if (corporationFinance != null)
                        {
                            hfCorporationId.Value = corporationFinance.CorporationId.ToString();
                            var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationFinance.CorporationId);
                            lbFDDeptAmount.Text = EnrollMembershipHelper.DebtValue(corporationFinance.Dept);
                            if (corporation != null)
                            {
                                hfCorporationId.Value = corporation.Id.ToString();
                                lbCorporationName.Text = corporation.Name;
                                lbCorporationDesc.Text = corporation.Description;
                                lbTaxDept.Text = corporation.TaxDept;
                                lbTaxNo.Text = corporation.TaxNo;
                                if (corporation.ContactAddressId != null)
                                {
                                    var contactAddrId = Convert.ToInt32(corporation.ContactAddressId);
                                    lbContactAddress.Text = GetCorporationAddress(contactAddrId).ToString();
                                }
                                if (corporation.InvoiceAddressId != null)
                                {
                                    var invoiceAddressId = Convert.ToInt32(corporation.InvoiceAddressId);
                                    lbInvoiceAddress.Text = GetCorporationAddress(invoiceAddressId).ToString();
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

        protected void BtnGoPaymentClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var btnGoPayment = sender as Button;
                if (btnGoPayment != null)
                {
                    var id = Convert.ToInt32(btnGoPayment.CommandArgument);
                    if (id > 0)
                    {
                        hfCorporationFinanceId.Value = Crypto.Encrypt(id.ToString());
                        var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.Id == id);
                        if (corporationFinance != null)
                        {
                            hfCorporationId.Value = corporationFinance.CorporationId.ToString();
                            var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationFinance.CorporationId);

                            ltFDDeptAmount.Text = EnrollMembershipHelper.DebtValue(corporationFinance.Dept);
                            if (corporation != null)
                            {
                                lbCorpName.Text = corporation.Name;
                                hfCorporationId.Value = corporation.Id.ToString();
                                ltCorporationName.Text = corporation.Name;
                                ltCorporationDesc.Text = corporation.Description;
                                ltTaxDept.Text = corporation.TaxDept;
                                ltTaxNo.Text = corporation.TaxNo;
                                if (corporation.ContactAddressId != null)
                                {
                                    var contactAddrId = Convert.ToInt32(corporation.ContactAddressId);
                                    ltContactAddres.Text = GetCorporationAddress(contactAddrId).ToString();
                                }
                                if (corporation.InvoiceAddressId != null)
                                {
                                    var invoiceAddressId = Convert.ToInt32(corporation.InvoiceAddressId);
                                    ltInvoiceAddres.Text = GetCorporationAddress(invoiceAddressId).ToString();
                                }
                            }
                        }

                        dpPymtPaymentDate.SelectedDate = DateTime.Now;
                        //dpPymtReceiptDate.SelectedDate = DateTime.Now;
                        mvFinanceManager.SetActiveView(vPayment);
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
                    case 1: return AdminResource.lbPaymentType1;
                    case 2: return AdminResource.lbPaymentType2;
                    case 3: return AdminResource.lbPaymentType3;
                    case 4: return AdminResource.lbPaymentType4;
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

        public void UpdateCorporationsCurrentDept(decimal currentDept)
        {
            try
            {
                lbFDDeptAmount.Text = EnrollMembershipHelper.DebtValue(currentDept);
                ltFDDeptAmount.Text = EnrollMembershipHelper.DebtValue(currentDept);

                gvFinanceManagement.DataSource = GetCorporationsTable();
                gvFinanceManagement.DataBind();
                gvChargesForDues.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public void UpdateCorporationsCurrentDept()
        {
            gvFinanceManagement.DataSource = GetCorporationsTable();
            gvFinanceManagement.DataBind();
            gvChargesForDues.DataBind();
        }

        public void ClearFormInputs()
        {
            tbPymtPaymentAmount.Text = string.Empty;
            tbPymtProvisionNo.Text = string.Empty; 
            dpPymtPaymentDate.SelectedDate = DateTime.Now;  
            BindDdlPymtPaymentType();
        }

        protected void BtnExportExcel(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(hfCorporationId.Value))
            {
                var memberId = Convert.ToInt32(hfCorporationId.Value);
                var corporationDuesLogs = _entities.CorporationDuesLog.Where(p => p.CorporationId == memberId).ToList();
                var view = new GridView();
                try
                {
                    // yeni tablo oluşturulur
                    var xlsJobsDataTable = EnrollMembershipHelper.CreateCorporationFinanceDetailDataTable();
                    foreach (var corporationLog in corporationDuesLogs)
                    {
                        var newRow = xlsJobsDataTable.NewRow();

                        var paymentType = _entities.DuesPaymentTypes.FirstOrDefault(p => p.Id == corporationLog.PaymentTypeId);
                        var createdUser = _entities.Users.FirstOrDefault(p => p.Id == corporationLog.CreatedUser);

                        newRow[AdminResource.lbPaymentType] = paymentType != null ? paymentType.Title : "";
                        newRow[AdminResource.lbAmount] = EnrollMembershipHelper.AmountValueExcel(corporationLog.Amount, corporationLog.LogType);
                        if (corporationLog.LogType == 0)
                        {
                            newRow[AdminResource.lbReceiptInvoiceNumber] = (!string.IsNullOrWhiteSpace(corporationLog.ReceiptNo)
                                ? corporationLog.ReceiptNo
                                : AdminResource.lbNotInvoiced);
                        }
                        else
                            newRow[AdminResource.lbReceiptInvoiceNumber] = (!string.IsNullOrWhiteSpace(corporationLog.ReceiptNo)
                                ? corporationLog.ReceiptNo
                                : string.Empty);

                        newRow[AdminResource.lbReceiptInvoiceDate] = corporationLog.ReceiptDate;
                        newRow[AdminResource.lbPaymentDate] = corporationLog.PaymentDate;
                        newRow[AdminResource.lbProcess] = corporationLog.LogType == 0 ? AdminResource.lbDebiting : AdminResource.lbPayment;
                        newRow[AdminResource.lbProcessDate] = corporationLog.CreatedTime.ToShortDateString() + " " + corporationLog.CreatedTime.ToShortTimeString();
                        newRow[AdminResource.lbProcessUser] = createdUser != null ? createdUser.Name : "";

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
                    this.EnableViewState = false;
                    Response.ContentType = "application/vnd.xls";
                    Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToShortDateString() + ".xls");
                    const string header = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n" +
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

        protected object GetCorporationAddress(int addressId)
        {
            var address = string.Empty;
            var coprationAddress = _entities.CorporationAddress.FirstOrDefault(p => p.Id == addressId);
            if (coprationAddress != null)
            {
                if (coprationAddress.CountryId != null && coprationAddress.CountryId > 0)
                {
                    var country = _entities.Countries.FirstOrDefault(p => p.Id == coprationAddress.CountryId);
                    if (country != null) address += country.Name;
                }
                if (coprationAddress.CityId != null && coprationAddress.CityId > 0)
                {
                    if (address != string.Empty) address += ", ";
                    var city = _entities.Cities.FirstOrDefault(p => p.Id == coprationAddress.CityId);
                    if (city != null) address += city.Name;
                }
                if (coprationAddress.TownId != null && coprationAddress.TownId > 0)
                {
                    if (address != string.Empty) address += ", ";
                    var town = _entities.Towns.FirstOrDefault(p => p.Id == coprationAddress.TownId);
                    if (town != null) address += town.Name;
                }
                return address;
            }
            return string.Empty;
        }

        protected void btnInvoicing_OnClick(object sender, EventArgs e)
        {
            pServiceInvoicing.Visible = true;
            try
            {
                var btnInvoicing = sender as Button;
                if (btnInvoicing != null && !string.IsNullOrEmpty(btnInvoicing.CommandArgument))
                {
                    var corporationLogId = Convert.ToInt32(btnInvoicing.CommandArgument);
                    var corporationLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == corporationLogId);
                    var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationLog.CorporationId);
                    if (corporation != null && corporationLog != null)
                    {
                        var _hfCorporationLogId = (HiddenField)cCorporationInvoicement.FindControl("hfCorporationLogId");
                        _hfCorporationLogId.Value = btnInvoicing.CommandArgument;
                        var _lbCorporation = (Label)cCorporationInvoicement.FindControl("lbCorporation");
                        var _ltService = (Literal)cCorporationInvoicement.FindControl("ltService");
                        var _ltServiceAmount = (Literal)cCorporationInvoicement.FindControl("ltServiceAmount");
                        var _ltProcessUser = (Literal)cCorporationInvoicement.FindControl("ltProcessUser");
                        var _ltProcessDate = (Literal)cCorporationInvoicement.FindControl("ltProcessDate");
                        var _ltInvoiceAddress = (Literal)cCorporationInvoicement.FindControl("ltInvoiceAddress");
                        var _tbReceiptNo = (TextBox)cCorporationInvoicement.FindControl("tbReceiptNo");

                        _lbCorporation.Text = corporation.Name;
                        _ltService.Text = corporationLog.Service;
                        _ltServiceAmount.Text = corporationLog.Amount.ToString(".00");
                        _ltProcessUser.Text = GetUserName(corporationLog.CreatedUser);
                        _ltProcessDate.Text = corporationLog.CreatedTime.ToShortDateString() + " " +
                                             corporationLog.CreatedTime.ToShortTimeString();
                        var address = _entities.CorporationAddress.FirstOrDefault(p => p.Id == corporation.InvoiceAddressId);
                        if (address != null)
                        {
                            _ltInvoiceAddress.Text = GetCorporationAddress(address.Id).ToString();
                        }
                        _tbReceiptNo.Text = string.Empty;
                        pServiceInvoicing.Visible = true;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void gvChargesForDues_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnInvoicing = (Button)e.Row.FindControl("btnInvoicing");
            if (btnInvoicing != null) btnInvoicing.Text = AdminResource.lbInvoice;
        }

        //#region faturalandırma işlemi
        //protected void btInvoiceService_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(hfCorporationLogId.Value))
        //        {
        //            var corporationLogId = Convert.ToInt32(hfCorporationLogId.Value);
        //            var corporationLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == corporationLogId);
        //            if (corporationLog != null)
        //            {
        //                var beforeSavedReceipt = _entities.CorporationDuesLog.FirstOrDefault(p => p.ReceiptNo == tbReceiptNo.Text);
        //                if (beforeSavedReceipt != null)
        //                {
        //                    if (beforeSavedReceipt.ReceiptDate != dpReceiptInvoiceDate.SelectedDate)
        //                    {
        //                        MessageBox.Show(MessageType.jAlert,
        //                            string.Format("{0} numaralı faturanın tarihi sistemde {1} olarak kayıtlı. Lütfen bilgileri kontrol ediniz.",
        //                                          beforeSavedReceipt.ReceiptNo, Convert.ToDateTime(beforeSavedReceipt.ReceiptDate).ToShortDateString()));
        //                    }
        //                    else
        //                    {
        //                        ReceiptService(corporationLog);
        //                    }
        //                }
        //                else
        //                {
        //                    ReceiptService(corporationLog);
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        ExceptionManager.ManageException(exception);
        //        MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
        //    }
        //} 
        //private bool ReceiptService(CorporationDuesLog log)
        //{

        //    log.ReceiptDate = dpReceiptInvoiceDate.SelectedDate;
        //    log.Note = tbDesc.Text;
        //    log.ReceiptNo = tbReceiptNo.Text;
        //    log.IsInvoiced = true;

        //    var generalDept = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == log.CorporationId);
        //    if (generalDept != null)
        //    {
        //        #region sorgu sonucu ile gelen kurumun genel borcu güncellenir
        //        decimal newDept = 0;

        //        var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == log.CorporationId);
        //        if (corporationFinance != null)
        //        {
        //            corporationFinance.Dept += log.Amount;
        //        }
        //        else
        //        {
        //            corporationFinance = new CorporationFinance();
        //            corporationFinance.CorporationId = log.CorporationId;
        //            corporationFinance.Dept = newDept;
        //            _entities.AddToCorporationFinance(corporationFinance);
        //        }
        //        #endregion

        //        log.IsInvoiced = true;
        //        _entities.SaveChanges();
        //        MessageBox.Show(MessageType.Success, AdminResource.msgServiceInvoicedSuccesfully);
        //        ClearFormInputs();
        //        pServiceInvoicing.Visible = false;
        //        return true;
        //    }
        //    return false;
        //} 
        //protected void btInvoiceServiceCancel_OnClick(object sender, EventArgs e)
        //{
        //    hfCorporationLogId.Value = null;
        //}
        //#endregion

        ////protected void btInvoiceService_OnClick(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (!string.IsNullOrEmpty(hfCorporationLogId.Value))
        ////        {
        ////            var corporationLogId = Convert.ToInt32(hfCorporationLogId.Value);
        ////            var corporationLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == corporationLogId);
        ////            if (corporationLog != null)
        ////            {
        ////                var corporationId = corporationLog.CorporationId;
        ////                corporationLog.ReceiptDate = dpReceiptInvoiceDate.SelectedDate;
        ////                corporationLog.Note = tbDesc.Text;
        ////                corporationLog.ReceiptNo = tbReceiptNo.Text;
        ////                corporationLog.IsInvoiced = true;

        ////                var generalDept = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == corporationId);
        ////                if (generalDept != null)
        ////                {
        ////                    #region sorgu sonucu ile gelen listedeki tüm kurumların genel borçları güncellenir
        ////                    decimal newDept = 0;

        ////                    var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == corporationId);
        ////                    if (corporationFinance != null)
        ////                    {
        ////                        corporationFinance.Dept += corporationLog.Amount;
        ////                    }
        ////                    else
        ////                    {
        ////                        corporationFinance = new CorporationFinance();
        ////                        corporationFinance.CorporationId = corporationId;
        ////                        corporationFinance.Dept = newDept;
        ////                        _entities.AddToCorporationFinance(corporationFinance);
        ////                    }
        ////                    #endregion

        ////                    corporationLog.IsInvoiced = true;
        ////                    _entities.SaveChanges();
        ////                    UpdateCorporationsCurrentDept(corporationFinance.Dept);


        ////                    MessageBox.Show(MessageType.Success, AdminResource.msgServiceInvoicedSuccesfully);
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch (Exception exception)
        ////    {
        ////        ExceptionManager.ManageException(exception);
        ////        MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
        ////    }
        ////}

        ////protected void btInvoiceServiceCancel_OnClick(object sender, EventArgs e)
        ////{
        ////    hfCorporationLogId.Value = null;
        ////}

    }
}