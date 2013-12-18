using System; 
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq; 
using System.Web; 
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;
using eNroll.Helpers;
using Enroll.Managers; 
using Resources;
using Telerik.Web.UI.Calendar;

namespace eNroll.Admin.adminUserControls
{
    public partial class CorporationFinanceLogs : System.Web.UI.UserControl
    {
        Entities _entities = new Entities();
        public SqlConnection _oConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
        public DataTable LogsDataTable = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 39))
            {
                mvAuth.SetActiveView(vAuth);
                if (!IsPostBack)
                {
                    #region resources

                    gvChargesForDues.Columns[0].HeaderText = AdminResource.lbDetail;
                    gvChargesForDues.Columns[1].HeaderText = AdminResource.lbCorporationName;
                    gvChargesForDues.Columns[2].HeaderText = AdminResource.lbReceiptInvoiceNumber;
                    gvChargesForDues.Columns[3].HeaderText = AdminResource.lbReceiptInvoiceDate;
                    gvChargesForDues.Columns[4].HeaderText = AdminResource.lbService;
                    gvChargesForDues.Columns[5].HeaderText = AdminResource.lbServiceAmount;
                    gvChargesForDues.Columns[6].HeaderText = AdminResource.lbProcessDate;

                    #endregion

                    BindDdlFinanceLogs();
                    BindDDlDdlProcess();
                    BindDdlCorporations();

                    hfFilterInvoice.Value = ddlFilterInvoice.SelectedItem.Value;
                    hfFilterProcces.Value = ddlProcess.SelectedItem.Value;
                    BindGridViewCorporationDuesLog();
                    Session["currentPath"] = AdminResource.lbFinanceLogs;
                    pServiceInvoicing.Visible = false;
                }
            }
        }

        #region gridview

        protected void gvChargesForDues_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnInvoicing = (Button)e.Row.FindControl("btnInvoicing");
            if (btnInvoicing != null)
            {
                btnInvoicing.Text = AdminResource.lbInvoice;
            }

            #region fatura iptal etme işlemi (Bind)
            var ibDeleteInvoice = (ImageButton)e.Row.FindControl("IbDeleteInvoice");
            if (ibDeleteInvoice != null)
            {
                if (ibDeleteInvoice.CommandArgument != null && !string.IsNullOrEmpty(ibDeleteInvoice.CommandArgument))
                {
                    var id = Convert.ToInt32(ibDeleteInvoice.CommandArgument);
                    var corpDuesLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == id);
                    if (corpDuesLog != null && corpDuesLog.IsInvoiced)
                    {
                        ibDeleteInvoice.OnClientClick = "var a=confirm('" + AdminResource.lbConfirmCancelInvoice + "');" +
                                                     "if(a){" +
                                                        "var b=confirm('" + AdminResource.msgConfirmOperationWillNotGetBack + "');" +
                                                        "if(b){" +
                                                            "return confirm('" + AdminResource.lbDeletingQuestion + "'); " +
                                                        "}" +
                                                            "else return false;" +
                                                     "}" +
                                                     "else return false;";
                    }
                    else
                    {
                        ibDeleteInvoice.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
                    }
                }
                else
                {
                    ibDeleteInvoice.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
                }

            }
            #endregion
             
            #region logsilme işlemi (Bind)
            var ibDeleteLog = (ImageButton)e.Row.FindControl("IbDeleteLog");
            if (ibDeleteLog != null)
            {
                if (ibDeleteLog.CommandArgument != null && !string.IsNullOrEmpty(ibDeleteLog.CommandArgument))
                {
                    var id = Convert.ToInt32(ibDeleteLog.CommandArgument);
                    var corpDuesLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == id);
                    if (corpDuesLog != null)
                    {
                        ibDeleteLog.OnClientClick = "var a=confirm('" + AdminResource.lbConfirmDeleteDuesLog + "');" +
                                                     "if(a){" +
                                                        "var b=confirm('" + AdminResource.msgConfirmOperationWillNotGetBack + "');" +
                                                        "if(b){" +
                                                            "return confirm('" + AdminResource.lbDeletingQuestion + "'); " +
                                                        "}" +
                                                            "else return false;" +
                                                     "}" +
                                                     "else return false;";
                    }
                }

            }
            #endregion

            #region ödeme işlemi (Bind)
            var ibDeletePayment = (ImageButton)e.Row.FindControl("IbDeletePayment");
            if (ibDeletePayment != null)
            {
                if (ibDeletePayment.CommandArgument != null && !string.IsNullOrEmpty(ibDeletePayment.CommandArgument))
                {
                    var id = Convert.ToInt32(ibDeletePayment.CommandArgument);
                    var corpDuesLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == id);
                    if (corpDuesLog != null)
                    {
                        ibDeletePayment.OnClientClick = "var a=confirm('" + AdminResource.lbConfirmDeletePayment + "');" +
                                                     "if(a){" +
                                                        "var b=confirm('" + AdminResource.msgConfirmOperationWillNotGetBack + "');" +
                                                        "if(b){" +
                                                            "return confirm('" + AdminResource.lbDeletingQuestion + "'); " +
                                                        "}" +
                                                            "else return false;" +
                                                     "}" +
                                                     "else return false;";
                    }
                }

            }
            #endregion 
        }

        protected void gvChargesForDues_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvChargesForDues.PageIndex = e.NewPageIndex;
            BindGridViewCorporationDuesLog();
        }

        public string CreateLogsWhereStmn()
        {
            #region create Where Stmn
            var where = " Where 1=1";
            if (ddlProcess.SelectedIndex > 0)
            {
                where += string.Format(" AND LogType={0} ", ddlProcess.SelectedItem.Value);
            }
            if (ddlFilterInvoice.SelectedIndex > 0)
            {
                where += string.Format(" AND IsInvoiced='{0}' ", ddlFilterInvoice.SelectedItem.Value);
            }
            if (ddlFilterCorporation.SelectedIndex > 0)
            {
                where += string.Format(" AND CorporationId={0} ", ddlFilterCorporation.SelectedItem.Value);
            }
            if (dpStartDate.SelectedDate != null && dpEndDate.SelectedDate != null)
            {
                var start = dpStartDate.SelectedDate.Value.ToString("u").Split(' ');
                var end = dpEndDate.SelectedDate.Value.ToString("u").Split(' ');
                where += string.Format(" AND ((ReceiptDate BETWEEN '{0}' AND '{1}') OR (PaymentDate BETWEEN '{0}' AND '{1}'))", start[0], end[0] + " 23:23:23.999");
            }
            else
            {
                if (dpStartDate.SelectedDate != null)
                {
                    var start = dpStartDate.SelectedDate.Value.ToString("u").Split(' ');
                    var end = DateTime.Now.ToString("u").Split(' ');
                    where += string.Format(" AND ((ReceiptDate BETWEEN '{0}' AND '{1}') OR (PaymentDate BETWEEN '{0}' AND '{1}'))", start[0], end[0]);
                }
                if (dpEndDate.SelectedDate != null)
                {
                    var start = DateTime.Now.AddYears(-50).ToString("u").Split(' ');
                    var end = dpEndDate.SelectedDate.Value.ToString("u").Split(' ');
                    where += string.Format(" AND ((ReceiptDate BETWEEN '{0}' AND '{1}') OR (PaymentDate BETWEEN '{0}' AND '{1}'))", start[0], end[0]);
                }
            }
            if (dpProccesStartDate.SelectedDate != null && dpProccesEndDate.SelectedDate != null)
            {
                var start = dpProccesStartDate.SelectedDate.Value.ToString("u").Split(' ');
                var end = dpProccesEndDate.SelectedDate.Value.ToString("u").Split(' ');
                where += string.Format(" AND (CorporationDuesLog.CreatedTime BETWEEN '{0}' AND '{1}')", start[0], end[0] + " 23:23:23.999");
            }
            else
            {
                if (dpProccesStartDate.SelectedDate != null)
                {
                    var start = dpProccesStartDate.SelectedDate.Value.ToString("u").Split(' ');
                    var end = DateTime.Now.ToString("u").Split(' ');
                    where += string.Format(" AND (CorporationDuesLog.CreatedTime BETWEEN '{0}' AND '{1}')", start[0], end[0]);
                }
                if (dpProccesEndDate.SelectedDate != null)
                {
                    var start = DateTime.Now.AddYears(-50).ToString("u").Split(' ');
                    var end = dpProccesEndDate.SelectedDate.Value.ToString("u").Split(' ');
                    where += string.Format(" AND (CorporationDuesLog.CreatedTime BETWEEN '{0}' AND '{1}')", start[0], end[0]);
                }
            }
            if (txtAra.Text != string.Empty)
            {
                var searchText = txtAra.Text.Replace("'", "");
                searchText = searchText.Replace("\"", "");
                where += " AND (" +
                         "ReceiptNo like '%" + searchText + "%' OR " +
                         "Service like '%" + searchText + "%' OR " +
                         "c.Name like '%" + searchText + "%')";
            }
            #endregion

            return where;
        }

        public void BindGridViewCorporationDuesLog()
        {
            try
            {
                var searchCommandText = string.Format("Select * From CorporationDuesLog " +
                                                      "Inner Join Corporations as c on c.Id=CorporationDuesLog.CorporationId " +
                                                      "{0} ORDER BY CorporationDuesLog.CreatedTime DESC", CreateLogsWhereStmn());
                #region sql query
                try
                {
                    
                    var cmdSearchResault = new SqlCommand
                    {
                        Connection = _oConnection,
                        CommandText = searchCommandText
                    };

                    if (_oConnection.State == ConnectionState.Closed)
                        _oConnection.Open();
                    LogsDataTable = new DataTable();
                    var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                    oAdaptor.Fill(LogsDataTable);
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

                gvChargesForDues.DataSource = LogsDataTable;
                gvChargesForDues.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #endregion

        #region Bind "Invoice Info" to Control
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

        #endregion

        #region Bind Ddl <ddlFinanceLogs, ddlProcess>
        public void BindDdlCorporations()
        {
            ddlFilterCorporation.Items.Clear();
            ddlFilterCorporation.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            var corporations = _entities.Corporations.Where(p => p.State).ToList().OrderBy(p => p.Name);
            var i = 1;
            foreach (var corporation in corporations)
            {
                ddlFilterCorporation.Items.Insert(i, new ListItem(corporation.Name, corporation.Id.ToString()));
                i++;
            }
        }
        public void BindDdlFinanceLogs()
        {
            ddlFilterInvoice.Items.Clear();
            ddlFilterInvoice.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            ddlFilterInvoice.Items.Insert(1, new ListItem(AdminResource.lbNotInvoiced, "false"));
            ddlFilterInvoice.Items.Insert(2, new ListItem(AdminResource.lbInvoiced, "true"));
        }
        public void BindDDlDdlProcess()
        {
            ddlProcess.Items.Clear();
            ddlProcess.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            ddlProcess.Items.Insert(1, new ListItem(AdminResource.lbDebiting, "0"));
            ddlProcess.Items.Insert(2, new ListItem(AdminResource.lbPayment, "1"));
        }
        #endregion

        #region ddl,datepicker OnSelectedIndexChanged
        protected void ddlFilterInvoice_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfFilterInvoice.Value = ddlFilterInvoice.SelectedItem.Value;
            BindGridViewCorporationDuesLog();
            pServiceInvoicing.Visible = false;
        }

        protected void ddlFilterCorporation_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfFilterCorporation.Value = ddlFilterCorporation.SelectedItem.Value;
            BindGridViewCorporationDuesLog();
            pServiceInvoicing.Visible = false;
        }

        protected void ddlProcess_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfFilterProcces.Value = ddlProcess.SelectedItem.Value;
            BindGridViewCorporationDuesLog();
            pServiceInvoicing.Visible = false;
        }

        protected void dpStartEndDate_OnSelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            BindGridViewCorporationDuesLog();
        }
        #endregion

        #region <fatura, borçlandırma, ödeme> işlemlerini silme
        
        //fatura silme
        protected void IbDeleteInvoiceClick(object sender, ImageClickEventArgs e)
        {
            var btnDelete = sender as ImageButton;
            if (btnDelete != null && !string.IsNullOrEmpty(btnDelete.CommandArgument))
            {
                try
                {
                    var id = Convert.ToInt32(btnDelete.CommandArgument);
                    var corporationDuesLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == id);
                    var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == corporationDuesLog.CorporationId);
                    if (corporationFinance != null && corporationDuesLog != null)
                    {
                        if (corporationDuesLog.IsInvoiced)
                        {
                            switch (corporationDuesLog.LogType)
                            {
                                case 0:
                                    corporationFinance.Dept -= corporationDuesLog.Amount;
                                    break;
                                case 1:
                                    corporationFinance.Dept += (corporationDuesLog.Amount);
                                    break;
                            }

                        }
                        corporationDuesLog.ReceiptNo = null;
                        corporationDuesLog.ReceiptDate = null;
                        corporationDuesLog.IsInvoiced = false;
                        
                        //Fatura İptal Etme,
                        Logger.Add(33, 4, corporationDuesLog.Id, 3);
                         
                        _entities.SaveChanges();

                        BindGridViewCorporationDuesLog();
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    MessageBox.Show(MessageType.Success, AdminResource.msgAnErrorOccurred);
                }
            }
        }

        //borçlandırma işlemi silme
        protected void IbDeleteLogClick(object sender, ImageClickEventArgs e)
        {
            var ibDeleteLog = sender as ImageButton;
            if (ibDeleteLog != null && !string.IsNullOrEmpty(ibDeleteLog.CommandArgument))
            {
                try
                {
                    var id = Convert.ToInt32(ibDeleteLog.CommandArgument);
                    if (DeleteCorpLog(id))
                    {
                        //Borçlandırma İşlem Silme,
                        Logger.Add(33, 5, id, 2);
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    } 
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
                }
            }
        }

        //ödeme işlmi silme
        protected void IbDeletePaymentClick(object sender, ImageClickEventArgs e)
        {
            var ibDeletePymtLog = sender as ImageButton;
            if (ibDeletePymtLog != null && !string.IsNullOrEmpty(ibDeletePymtLog.CommandArgument))
            {
                try
                {
                    var id = Convert.ToInt32(ibDeletePymtLog.CommandArgument);
                    if (DeleteCorpLog(id))
                    {
                        //Ödeme İşlemi Silme,
                        Logger.Add(33, 6, id, 2); 
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
                }
            }
        }

        private bool DeleteCorpLog(int corporationId)
        {
            try
            {
                var corporationDuesLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == corporationId);
                if (corporationDuesLog != null)
                {
                    _entities.DeleteObject(corporationDuesLog);
                    _entities.SaveChanges();
                    BindGridViewCorporationDuesLog();
                    return true;
                }
                else return false; 
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                return false;
            }
        }
        #endregion

        #region helpers
        protected string GetUserName(int userId)
        {
            var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
            return (user != null ? string.Format("{0} {1}", user.Name, user.Surname) : string.Empty);
        }

        protected string GetCorporationName(int corporationId)
        {
            var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);
            return (corporation != null ? corporation.Name : string.Empty);
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
        #endregion

        #region search
        protected void ImageButton2Ara_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BindGridViewCorporationDuesLog();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #endregion

    }
}