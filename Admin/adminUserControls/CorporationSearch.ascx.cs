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

    public partial class CorporationSearch : System.Web.UI.UserControl
    {
        private static Corporations _corporation = new Corporations();
        Entities _entities = new Entities();
        DataTable _corporationSearchDataTable = new DataTable();

        #region create search sql variables

        private string _sqlSearchQuery = "SELECT distinct(c.Id) as cId, c.Name as cName, c.Description as cDescription, " +
                                        "c.TaxDept as cTaskDept, c.TaxNo as cTaskNo, c.State as cState , " +
                                        "c.ContactAddressId as cContactId, c.InvoiceAddressId as cIncoiceId, " +
                                        "fin.Id as finId, fin.Dept as finDept " +
                                        "FROM Corporations as c {0} {1} {2}";
        private string _sqlSearchCountQuery = "SELECT Count(distinct(c.Id)) FROM Corporations as c {0} {1} {2}";
        public SqlConnection _oConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
        string _searchResultCommandText = string.Empty;
        string _searchResultCountCommandText = string.Empty;
        string _innerJoin = string.Empty;
        int _parametersCount = 0;
        StringBuilder _searchCommandCorporationsWhere = new StringBuilder();
        StringBuilder _searchCommandCorporationAddressWhere = new StringBuilder();
        #endregion

        protected override void OnInit(EventArgs e)
        {

            Session["currentPath"] = AdminResource.lbCorporationSearch;

            #region resources

            btSearch.Text = AdminResource.lbSearch;
            btClearSearchCriterias.Text = AdminResource.lbClearSearchCriterias;
            btDownloadUsersExcelList.Text = AdminResource.lbDownloadAsExcel;
            btFinanceManager.Text = AdminResource.lbFinanceManager;
            btBackToSearchPage.Text = AdminResource.lbSearchPage;
            btCancelFinanceManager.Text = AdminResource.lbSearchPage;

            gVCorporations.Columns[0].HeaderText = AdminResource.lbActions;
            gVCorporations.Columns[1].HeaderText = AdminResource.lbCorporationName;
            gVCorporations.Columns[2].HeaderText = AdminResource.lbUserCount;
            gVCorporations.Columns[3].HeaderText = AdminResource.lbCurrentDebtAmount;
            gVCorporations.Columns[4].HeaderText = AdminResource.lbDesc;
            gVCorporations.Columns[5].HeaderText = AdminResource.lbState;

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
                    _searchCommandCorporationsWhere = new StringBuilder();
                    _searchCommandCorporationAddressWhere = new StringBuilder();

                    _oConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
                    _corporationSearchDataTable = new DataTable();
                    _corporation = new Corporations();
                    BindSearchCriterias();
                    mvCorporationSearch.SetActiveView(vSearchCorporation);
                }
            }
            else
            {
                mvAuthoriztn.ActiveViewIndex = 1;
            }
        }
        protected void BtSearchClick(object sender, EventArgs e)
        {
            try
            {
                #region Table 'Corporations' WHERE Params
                if (!string.IsNullOrWhiteSpace(tbName.Text))
                    AddWhereParams(DbTable.Corporation, string.Format("c.Name Like '%{0}%'", tbName.Text));

                if (!string.IsNullOrWhiteSpace(tbTaxDept.Text))
                    AddWhereParams(DbTable.Corporation, string.Format("c.TaxDept Like '%{0}%'", tbTaxDept.Text));

                if (!string.IsNullOrWhiteSpace(tbTaxNumber.Text))
                    AddWhereParams(DbTable.Corporation, string.Format("c.TaxNo Like '%{0}%'", tbTaxNumber.Text));
                #endregion
                #region AddressInfo

                var addressFiltered = false;
                if (ddlWorkCountry.SelectedIndex > 0)
                {
                    AddWhereParams(DbTable.CorporationAddress, string.Format("a.CountryId={0}", ddlWorkCountry.SelectedItem.Value));
                    addressFiltered = true;
                }
                if (ddlWorkCity.SelectedIndex > 0)
                {
                    AddWhereParams(DbTable.CorporationAddress, string.Format("a.CityId={0}", ddlWorkCity.SelectedItem.Value));
                    addressFiltered = true;
                }
                if (ddlWorkTown.SelectedIndex > 0)
                {
                    AddWhereParams(DbTable.CorporationAddress, string.Format("a.TownId={0}", ddlWorkTown.SelectedItem.Value));
                    addressFiltered = true;
                }
                #endregion

                #region sorgularla bağlanan tabloları Inner Join ile sorguya alırız
                _innerJoin += " Inner Join CorporationFinance as fin on c.Id=fin.CorporationId ";
                if (addressFiltered) _innerJoin += " Inner Join CorporationAddress as a on a.CorporationId=c.Id ";
                #endregion

                #region create SQL query
                //sonuçları ve sayısını döndüren sql sorugusunun tam hali oluşturulur
                _innerJoin += " WHERE 1=1 ";
                _searchResultCommandText = string.Format(_sqlSearchQuery,
                                            _innerJoin,
                                            _searchCommandCorporationsWhere,
                                            _searchCommandCorporationAddressWhere);
                _searchResultCountCommandText = string.Format(_sqlSearchCountQuery,
                                        _innerJoin,
                                        _searchCommandCorporationsWhere,
                                        _searchCommandCorporationAddressWhere);

                #endregion

                #region sql sorguları hidden field da saklanır
                hfSearchResaultCmd.Value = Crypto.Encrypt(_searchResultCommandText);
                hfSearchResaultCountCmd.Value = Crypto.Encrypt(_searchResultCountCommandText);
                #endregion

                #region EXEC SQL

                tbSqlQueryOutput.Text = _searchResultCommandText;
                tbSqlQueryOutput.Visible = true;
                var cmdSearchResaultCount = new SqlCommand { Connection = _oConnection, CommandText = _searchResultCountCommandText };
                if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                var resaultCount = Convert.ToInt32(cmdSearchResaultCount.ExecuteScalar());
                #endregion

                #region show results
                if (resaultCount > 0)
                {
                    //btShowResults.Visible = true;
                    btClearSearchCriterias.Visible = true;
                    dvCorporationActions.Visible = true;

                    ltResults.Text = string.Format("{0} {1}", resaultCount, Resources.AdminResource.lbFoundSearchResult);
                    ltResults.Visible = true;
                }
                else
                {
                    btClearSearchCriterias.Visible = true;
                    ltResults.Visible = true;
                    ltResults.Text = string.Format("{0}", Resources.AdminResource.lbNoResult);
                }
                ShowResults();

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
        private void AddWhereParams(DbTable table, string sb)
        {
            switch (table)
            {
                case DbTable.Corporation:
                    _searchCommandCorporationsWhere.AppendFormat(" and {0}", sb);
                    break;
                case DbTable.CorporationAddress:
                    _searchCommandCorporationAddressWhere.AppendFormat(" and {0}", sb);
                    break;
            }

            _parametersCount++;
        }
         
        #region Update/Cancel/Delete _corporation

        protected void ImgBtnMemberEditClick(object sender, ImageClickEventArgs e)
        {
            var btnMemberEdit = sender as ImageButton;
            if (btnMemberEdit != null)
            {
                hfCorporationId.Value = btnMemberEdit.CommandArgument;
                var corporationId = Convert.ToInt32(hfCorporationId.Value);

                var mvOperations = (MultiView)cCorporationAddEdit.FindControl("mvOperations");
                var btnAddNewAddress = (Button)cCorporationAddEdit.FindControl("btnAddNewAddress");
                var btnCancelCorporation = (Button)cCorporationAddEdit.FindControl("btnCancelCorporation");
                var btnAddNewUsers = (Button)cCorporationAddEdit.FindControl("btnAddNewUsers");
                var btnUpdateCorporation = (Button)cCorporationAddEdit.FindControl("btnUpdateCorporation");
                var btnUpdateFoundCorporation = (Button)cCorporationAddEdit.FindControl("btnUpdateFoundCorporation");
                var _hfCorporationId = (HiddenField)cCorporationAddEdit.FindControl("hfCorporationId");
                var gvCorporationUsers = (GridView)cCorporationAddEdit.FindControl("gvCorporationUsers");

                var corp = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);
                cCorporationAddEdit.BindCorporationInfo(corp);
                cCorporationAddEdit.BindContactAddress(corporationId);
                cCorporationAddEdit.BindInvoiceAddress(corporationId);

                btnAddNewAddress.Enabled = true;
                btnAddNewUsers.Enabled = true;
                btnCancelCorporation.Enabled = false;
                btnCancelCorporation.Visible = false;
                btnUpdateCorporation.Enabled = false;
                btnUpdateCorporation.Visible = false;
                btnUpdateFoundCorporation.Enabled = true;
                btnUpdateFoundCorporation.Visible = true;
                _hfCorporationId.Value = corporationId.ToString();
                gvCorporationUsers.DataBind();
                mvOperations.ActiveViewIndex = 1;
                mvCorporationSearch.SetActiveView(vEditMember);
            }
        }
        protected void imgBtnCorpFinanceClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                var btnCorpFinance = sender as ImageButton;
                if (btnCorpFinance != null)
                {
                    hfCorporationId.Value = btnCorpFinance.CommandArgument;
                    var corporationId = Convert.ToInt32(hfCorporationId.Value);

                    cCorporationFinanceManager.CorporationsSqlQuery = Crypto.Decrypt(hfSearchResaultCmd.Value) + " and c.Id=" + btnCorpFinance.CommandArgument;
                    cCorporationFinanceManager.Conn = _oConnection;

                    var hfSqlQuery = ((HiddenField)cCorporationFinanceManager.FindControl("hfSqlQuery"));
                    hfSqlQuery.Value = Crypto.Encrypt(Crypto.Decrypt(hfSearchResaultCmd.Value) + " and c.Id=" + btnCorpFinance.CommandArgument);
                    var mvFinanceManager = ((MultiView)cCorporationFinanceManager.FindControl("mvFinanceManager"));
                    mvFinanceManager.ActiveViewIndex = 0;

                    cCorporationFinanceManager.BindDdlPymtPaymentType();
                    mvCorporationSearch.SetActiveView(vFinanceManager);
                    cCorporationFinanceManager.BindCorporations();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }
        protected void ImgBtnMemberDeleteClick(object sender, ImageClickEventArgs e)
        {

            try
            {
                var corporationId = 0;
                var btnDelete = sender as ImageButton;
                if (btnDelete != null)
                {
                    var id = btnDelete.CommandArgument;
                    if (id != null) corporationId = Convert.ToInt32(id);
                    if (corporationId > 0)
                    {
                        var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);

                        // delete corporationFinance
                        var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == corporationId);
                        if (corporationFinance != null && corporationFinance.Id > 0) _entities.CorporationFinance.DeleteObject(corporationFinance);

                        // delete corporationDuesLog
                        var corporationDuesLogList = _entities.CorporationDuesLog.Where(p => p.CorporationId == corporationId).ToList();
                        if (corporationDuesLogList.Count > 0)
                        {
                            foreach (CorporationDuesLog corporationDuesLog in corporationDuesLogList)
                            {
                                _entities.CorporationDuesLog.DeleteObject(corporationDuesLog);
                                //_entities.SaveChanges();
                            }
                        }

                        // delete corporationAddress
                        var corporationAddressList = _entities.CorporationAddress.Where(p => p.CorporationId == corporationId).ToList();
                        if (corporationAddressList.Count > 0)
                        {
                            foreach (CorporationAddress address in corporationAddressList)
                            {
                                _entities.CorporationAddress.DeleteObject(address);
                                //_entities.SaveChanges();
                            }
                        }

                        // delete corporationUsers
                        var corporationUserList = _entities.CorporationUser.Where(p => p.CorporationId == corporationId).ToList();
                        if (corporationUserList.Count > 0)
                        {
                            foreach (CorporationUser corporationUser in corporationUserList)
                            {
                                _entities.CorporationUser.DeleteObject(corporationUser);
                                //_entities.SaveChanges();
                            }
                        }

                        // delete corporation
                        _entities.Corporations.DeleteObject(corporation);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);

                        #region Bind Corporations

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
                                oAdaptor.Fill(_corporationSearchDataTable);

                                gVCorporations.DataSource = _corporationSearchDataTable;
                                gVCorporations.DataBind();
                                gVCorporations.Visible = true;
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
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }

        #endregion

        #region Bind and Clear Search criterias
        private void BindSearchCriterias()
        {
            EnrollMembershipHelper.BindDdlCountries(ddlWorkCountry, ddlWorkCity, ddlWorkTown, EnrollMembershipHelper.GetCountries());
        }
        protected void BtClearSearchCriteriasClick(object sender, EventArgs e)
        {
            tbName.Text = string.Empty;
            tbTaxDept.Text = string.Empty;
            tbTaxNumber.Text = string.Empty;
            BindSearchCriterias();
            //membership info 
            btClearSearchCriterias.Visible = false;

            hfSearchResaultCmd.Value = string.Empty;
            hfSearchResaultCountCmd.Value = string.Empty;
            ltResults.Text = string.Empty;
            dvCorporationActions.Visible = false;
            gVCorporations.DataSource = string.Empty;
            gVCorporations.DataBind();
            gVCorporations.Visible = false;
        }
        #endregion

        #region OnSelectedIndexChanged countries cities towns
        protected void ddlWorkCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCountry.Value = ddlWorkCountry.SelectedIndex > 0 ? ddlWorkCountry.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlCities(ddlWorkCity, ddlWorkTown, EnrollMembershipHelper.GetCities(hfWorkCountry.Value));
        }
        protected void ddlWorkCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCity.Value = ddlWorkCity.SelectedIndex > 0 ? ddlWorkCity.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlTowns(ddlWorkTown, EnrollMembershipHelper.GetTowns(hfWorkCountry.Value, hfWorkCity.Value));
        }
        #endregion

        protected void ShowResults()
        {
            //btShowResults.Visible = false;

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
                    oAdaptor.Fill(_corporationSearchDataTable);

                    gVCorporations.DataSource = _corporationSearchDataTable;
                    gVCorporations.DataBind();
                    gVCorporations.Visible = true;
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

            try
            {
                var view = new GridView();
                if (!string.IsNullOrWhiteSpace(hfSearchResaultCmd.Value))
                {
                    var searchCommandText = Crypto.Decrypt(hfSearchResaultCmd.Value);
                    var cmdSearchResault = new SqlCommand { Connection = _oConnection, CommandText = searchCommandText };
                    if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                    var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                    oAdaptor.Fill(_corporationSearchDataTable);

                    if (_corporationSearchDataTable.Rows.Count > 0)
                    {
                        // yeni tablo oluşturulur
                        var xlsCorporationsDataTable = EnrollMembershipHelper.CreateCorporationsDataTable();

                        foreach (DataRow item in _corporationSearchDataTable.Rows)
                        {
                            var newRow = xlsCorporationsDataTable.NewRow();

                            #region yeni satıra sırayla kurum verileri eklenir
                            newRow[AdminResource.lbName] = item["cName"].ToString();
                            newRow[AdminResource.lbDesc] = item["cDescription"].ToString();
                            newRow[AdminResource.lbUserCount] = GetUsersCountOnCorporation(Convert.ToInt32(item["cId"]));
                            newRow[AdminResource.lbTaxDept] = item["cTaskDept"].ToString();
                            newRow[AdminResource.lbTaxNumber] = item["cTaskNo"].ToString();
                            newRow[AdminResource.lbContactAddress] = string.Empty;
                            newRow[AdminResource.lbInvoiceAddress] = string.Empty;
                            #endregion

                            xlsCorporationsDataTable.Rows.Add(newRow);
                        }

                        #region oluşturulan dataTable gridView e bind edilerek download edilir
                        view.DataSource = xlsCorporationsDataTable;
                        view.DataBind();

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
        protected void BtBackToSearchPageClick(object sender, EventArgs e)
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
                    oAdaptor.Fill(_corporationSearchDataTable);

                    gVCorporations.DataSource = _corporationSearchDataTable;
                    gVCorporations.DataBind();
                    gVCorporations.Visible = true;
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

            mvCorporationSearch.SetActiveView(vSearchCorporation);
        }
        protected void BtCancelFinanceManagerClick(object sender, EventArgs e)
        {
            mvCorporationSearch.SetActiveView(vSearchCorporation);
        }
        protected void gVCorporations_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
            var imgBtnMemberDelete = e.Row.FindControl("imgBtnMemberDelete") as ImageButton;

            if (btnEdit != null)
            {
                var userId = Convert.ToInt32(btnEdit.CommandArgument);
                if (!EnrollMembershipHelper.IsAuthForThisProcess(userId, EnrollMembershipHelper.GetUserIdFromEmail(HttpContext.Current.User.Identity.Name)))
                {
                    btnEdit.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;", AdminResource.msgNoAuth);
                }
            }

            if (imgBtnMemberDelete != null)
            {
                var userId = Convert.ToInt32(imgBtnMemberDelete.CommandArgument);
                if (!EnrollMembershipHelper.IsAuthForThisProcess(userId, EnrollMembershipHelper.GetUserIdFromEmail(HttpContext.Current.User.Identity.Name)) ||
                    EnrollMembershipHelper.AreYouActiveUser(userId))
                {
                    imgBtnMemberDelete.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;", AdminResource.msgNoAuth);
                }
                else
                {
                    imgBtnMemberDelete.OnClientClick = " return confirmDeleteCorp();";
                }
            }
        }
        public void GetSearchResult()
        {
            //btShowResults.Visible = false;

            #region EXEC SQL <search result count>
            try
            {
                _searchResultCountCommandText = Crypto.Decrypt(hfSearchResaultCountCmd.Value);
                _searchResultCommandText = Crypto.Decrypt(hfSearchResaultCmd.Value);

                var cmdSearchResaultCount = new SqlCommand { Connection = _oConnection, CommandText = _searchResultCountCommandText };
                if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                var resaultCount = Convert.ToInt32(cmdSearchResaultCount.ExecuteScalar());

            #endregion

                #region show results

                if (resaultCount > 0)
                {
                    //btShowResults.Visible = true;
                    btClearSearchCriterias.Visible = true;
                    dvCorporationActions.Visible = true;

                    ltResults.Text = string.Format("{0} {1}", resaultCount, Resources.AdminResource.lbFoundSearchResult);
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
                            oAdaptor.Fill(_corporationSearchDataTable);

                            gVCorporations.DataSource = _corporationSearchDataTable;
                            gVCorporations.DataBind();
                            gVCorporations.Visible = true;
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
                    ltResults.Text = string.Format("{0}", Resources.AdminResource.lbNoResult);
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
        protected string GetUsersCountOnCorporation(int corporationId)
        {
            return _entities.CorporationUser.Count(p => p.CorporationId == corporationId).ToString();
        }
        protected string GetCorporationBalance(int corporationId)
        {
            var corpBalance = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == corporationId);
            if (corpBalance != null)
            {
                var currency = _entities.Currency.First(p => p.Active);
                return string.Format("{0}{1}{2}",
                    (currency.Position == "L" ? currency.Symbol + " " : ""),
                    corpBalance.Dept.ToString("0.00"),
                    (currency.Position == "R" ? " " + currency.Symbol : ""));
            }
            return string.Empty;
        }
        protected void BtFinanceManagerClick(object sender, EventArgs e)
        {
            try
            {
                cCorporationFinanceManager.CorporationsSqlQuery = Crypto.Decrypt(hfSearchResaultCmd.Value);
                cCorporationFinanceManager.Conn = _oConnection;

                var hfSqlQuery = ((HiddenField)cCorporationFinanceManager.FindControl("hfSqlQuery"));
                hfSqlQuery.Value = hfSearchResaultCmd.Value;

                var mvFinanceManager = ((MultiView)cCorporationFinanceManager.FindControl("mvFinanceManager"));
                mvFinanceManager.ActiveViewIndex = 0;

                cCorporationFinanceManager.BindDdlPymtPaymentType();
                mvCorporationSearch.SetActiveView(vFinanceManager);
                cCorporationFinanceManager.BindCorporations();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }

        }

    }
}