using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;
using eNroll.Helpers;
using Resources;
using Enroll.Managers;

namespace eNroll.Admin.adminUserControls
{
    public partial class CorporationAddEdit : System.Web.UI.UserControl
    {
        Entities _entities = new Entities();
        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbCorporationManagement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            mvAuthoriztn.SetActiveView(RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 39)
                                           ? vAuth
                                           : vNoAuth);
            if (!IsPostBack)
            {
                hfCorporationId.Value = null;
                hfCorporationAddressId.Value = null;
                BindDdlCountries(ddlNewCorpCountry, ddlNewCorpCity, ddlNewCorpTown, EnrollMembershipHelper.GetCountries());
                mvOperations.SetActiveView(vAdd);
            }

            #region resources

            btnSaveNewCorp.Text = AdminResource.lbSave;
            btnCancelNewCorp.Text = AdminResource.lbCancel;

            btnSaveUpdateAddress.Text = AdminResource.lbSave;
            btnCancelAddress.Text = AdminResource.lbCancel;
            btnUpdateCorporation.Text = AdminResource.lbUpdate;
            btnUpdateFoundCorporation.Text = AdminResource.lbUpdate;
            btnCancelCorporation.Text = AdminResource.lbCancel;
            btnSaveUpdateUsers.Text = AdminResource.lbSave;
            btnCancelUsers.Text = AdminResource.lbCancel;

            btnAddNewUsers.Text = AdminResource.lbAddNewUsers;
            btnAddNewAddress.Text = AdminResource.lbAddNewAddress;

            btEditAddress1.Text = AdminResource.lbEdit;
            btEditAddress2.Text = AdminResource.lbEdit;

            btNew.Text = AdminResource.lbNewCorporation;

            gvCorporations.Columns[0].HeaderText = AdminResource.lbActions;
            gvCorporations.Columns[1].HeaderText = AdminResource.lbName;
            gvCorporations.Columns[2].HeaderText = AdminResource.lbUserCount;
            gvCorporations.Columns[3].HeaderText = AdminResource.lbDesc;

            gvCorporationUsers.Columns[1].HeaderText = AdminResource.lbName + " " + AdminResource.lbSurname;
            gvCorporationUsers.Columns[2].HeaderText = AdminResource.lbEmail;


            #endregion
        }

        #region update&save

        protected void btnSaveNewCorp_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Yeni Kurum Kaydı
                var corporation = new Corporations();
                corporation.Name = tbNewCorpName.Text;
                corporation.Description = tbNewCorpDesc.Text;
                corporation.TaxDept = tbNewCorpTaxDept.Text;
                corporation.TaxNo = tbNewCorpTaxNumber.Text;
                corporation.State = true;
                corporation.CreatedTime = DateTime.Now;
                corporation.UpdatedTime= DateTime.Now;
                _entities.AddToCorporations(corporation);
                _entities.SaveChanges();

                //Yeni Adres Kaydı
                var corporationId = corporation.Id;
                var corporationAddress = new CorporationAddress();
                if (ddlNewCorpCountry.SelectedIndex > 0) corporationAddress.CountryId = Convert.ToInt32(ddlNewCorpCountry.SelectedItem.Value);
                if (ddlNewCorpCity.SelectedIndex > 0) corporationAddress.CityId = Convert.ToInt32(ddlNewCorpCity.SelectedItem.Value);
                if (ddlNewCorpTown.SelectedIndex > 0) corporationAddress.TownId = Convert.ToInt32(ddlNewCorpTown.SelectedItem.Value);

                corporationAddress.CorporationId = corporationId;
                corporationAddress.Title = tbNewCorpAddressTitle.Text;
                corporationAddress.Description = tbNewCorpAddressDesc.Text;
                corporationAddress.DetailedAddress = tbNewCorpDetailedAddress.Text;
                corporationAddress.ZipCode = tbNewCorpZipCode.Text;
                corporationAddress.Fax = tbNewCorpFax.Text;
                corporationAddress.Phone = tbNewCorpPhone.Text;
                corporationAddress.Email = tbNewCorpEmail.Text;
                corporationAddress.CreatedTime = DateTime.Now;
                corporationAddress.UpdatedTime = DateTime.Now;
                corporationAddress.Web = tbNewCorpWeb.Text;

                _entities.AddToCorporationAddress(corporationAddress);
                _entities.SaveChanges();

                corporation.ContactAddressId = corporationAddress.Id;
                corporation.InvoiceAddressId = corporationAddress.Id;
               
                //Yeni Finans Kaydı
                var corporationFinance = new CorporationFinance();
                corporationFinance.CorporationId = corporationId;
                corporationFinance.Dept = 0;
                _entities.AddToCorporationFinance(corporationFinance);
                _entities.SaveChanges();
                  
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                mvOperations.SetActiveView(vList);
                gvCorporations.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }
        protected void btnUpdateCorporation_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfCorporationId.Value))
                {
                    var corporationId = Convert.ToInt32(hfCorporationId.Value);
                    UpdateCorporation(corporationId); //Güncelleme İşlemi
                    ClearForm();
                    mvOperations.SetActiveView(vList);
                    gvCorporations.DataBind();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }
        protected void btnUpdateFoundCorporation_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfCorporationId.Value))
                {
                    var corporationId = Convert.ToInt32(hfCorporationId.Value);
                    UpdateCorporation(corporationId); //Güncelleme İşlemi
                    mvOperations.SetActiveView(vEdit);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }
        protected void btnSaveUpdateAddress_OnClick(object sender, EventArgs e)
        {
            try
            {
                CorporationAddress corporationAddress = null;
                if (!string.IsNullOrWhiteSpace(hfCorporationAddressId.Value)
                     && !string.IsNullOrWhiteSpace(hfCorporationId.Value))
                {
                    //Güncelleme
                    var corporationId = Convert.ToInt32(hfCorporationId.Value);
                    var corporationAddressId = Convert.ToInt32(hfCorporationAddressId.Value);
                    corporationAddress = _entities.CorporationAddress.FirstOrDefault(p => p.Id == corporationAddressId);
                    if (corporationAddress != null)
                    {
                        if (ddlCountry.SelectedIndex > 0) corporationAddress.CountryId = Convert.ToInt32(ddlCountry.SelectedItem.Value);
                        if (ddlCity.SelectedIndex > 0) corporationAddress.CityId = Convert.ToInt32(ddlCity.SelectedItem.Value);
                        if (ddlTown.SelectedIndex > 0) corporationAddress.TownId = Convert.ToInt32(ddlTown.SelectedItem.Value);

                        corporationAddress.Title = tbAddressTitle.Text;
                        corporationAddress.Description = tbAddressDesc.Text;
                        corporationAddress.DetailedAddress = tbDetailedAddress.Text;
                        corporationAddress.ZipCode = tbZipCode.Text;
                        corporationAddress.Fax = tbFax.Text;
                        corporationAddress.Phone = tbPhone.Text;
                        corporationAddress.Web = tbWeb.Text;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        BindInvoiceAddress(corporationId);
                        BindContactAddress(corporationId);
                    }
                }
                else
                {
                    //Yeni Kayıt
                    var corporationId = Convert.ToInt32(hfCorporationId.Value);
                    corporationAddress = new CorporationAddress();
                    if (ddlCountry.SelectedIndex > 0) corporationAddress.CountryId = Convert.ToInt32(ddlCountry.SelectedItem.Value);
                    if (ddlCity.SelectedIndex > 0) corporationAddress.CityId = Convert.ToInt32(ddlCity.SelectedItem.Value);
                    if (ddlTown.SelectedIndex > 0) corporationAddress.TownId = Convert.ToInt32(ddlTown.SelectedItem.Value);

                    corporationAddress.CorporationId = corporationId;
                    corporationAddress.Title = tbAddressTitle.Text;
                    corporationAddress.Description = tbAddressDesc.Text;
                    corporationAddress.DetailedAddress = tbDetailedAddress.Text;
                    corporationAddress.ZipCode = tbZipCode.Text;
                    corporationAddress.Fax = tbFax.Text;
                    corporationAddress.Email = tbEmail.Text;
                    corporationAddress.Phone = tbPhone.Text;
                    corporationAddress.Web = tbWeb.Text;
                    corporationAddress.CreatedTime= DateTime.Now;
                    corporationAddress.UpdatedTime = DateTime.Now;
                    _entities.AddToCorporationAddress(corporationAddress);
                    _entities.SaveChanges();
                      
                    MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                    BindInvoiceAddress(corporationId);
                    BindContactAddress(corporationId);
                } 
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }
        protected void btnSaveUpdateUsers_OnClick(object sender, EventArgs e)
        {
            var savedNewUser = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(hfCorporationId.Value))
                {
                    var corporationId = Convert.ToInt32(hfCorporationId.Value);

                    var oldCorpUsers = _entities.CorporationUser.Where(p => p.CorporationId == corporationId).ToList();

                    foreach (ListItem cbUser in cbUsers.Items)
                    {
                        if (cbUser.Selected)
                        {
                            var userId = Convert.ToInt32(cbUser.Value);
                            if (oldCorpUsers.All(p => p.UserId != userId))
                            {
                                var corporationUser = new CorporationUser();
                                corporationUser.UserId = Convert.ToInt32(cbUser.Value);
                                corporationUser.CorporationId = corporationId;
                                _entities.AddToCorporationUser(corporationUser);
                                _entities.SaveChanges();
                                savedNewUser = true;
                            }
                        }
                    }
                    gvCorporationUsers.DataBind();
                }
                if (savedNewUser)
                {
                    MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }
        public bool UpdateCorporation(int corporationId)
        {
            try
            {
                Corporations corporation = null;
                corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);
                if (corporation != null)
                {
                    corporation.Name = tbName.Text;
                    corporation.Description = tbDesc.Text;
                    corporation.TaxDept = tbTaxDept.Text;
                    corporation.TaxNo = tbTaxNumber.Text;
                    corporation.State = cbState.Checked;
                    if (ddlContactAddress.SelectedIndex > 0)
                        corporation.ContactAddressId = Convert.ToInt32(ddlContactAddress.SelectedItem.Value);
                    if (ddlInvoiceAddress.SelectedIndex > 0)
                        corporation.InvoiceAddressId = Convert.ToInt32(ddlInvoiceAddress.SelectedItem.Value);
                    corporation.UpdatedTime = DateTime.Now;
                    _entities.SaveChanges();

                    MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
            return false;
        }
        #endregion

        #region users
        public List<int> AddedUsers(List<CorporationUser> _oldUsers)
        {
            var addedUsers = new List<int>();
            var found = false;
            foreach (ListItem cbUser in cbUsers.Items)
            {
                foreach (CorporationUser oldUser in _oldUsers)
                {
                    if (oldUser.UserId == Convert.ToInt32(cbUser.Value))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    addedUsers.Add(Convert.ToInt32(cbUser.Value));
                }
                found = false;
            }
            return addedUsers;
        }
        public List<int> DeletedUsers(List<CorporationUser> _oldUsers)
        {
            var addedUsers = new List<int>();
            var found = false;
            foreach (ListItem cbUser in cbUsers.Items)
            {
                foreach (CorporationUser oldUser in _oldUsers)
                {
                    if (oldUser.UserId == Convert.ToInt32(cbUser.Value))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    addedUsers.Add(Convert.ToInt32(cbUser.Value));
                }
                found = false;
            }
            return addedUsers;
        }
        #endregion

        #region ddl selected Index Changed
        protected void ddlCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfCountry.Value = ddlCountry.SelectedIndex > 0 ? ddlCountry.SelectedItem.Value : string.Empty;
            BindDdlCities(ddlCity, ddlTown, EnrollMembershipHelper.GetCities(hfCountry.Value));
            Page.ClientScript.RegisterStartupScript(GetType(), "safdg1",
                                                            "<script> Show('fAddress');</script>");
        }
        protected void ddlCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfCity.Value = ddlCity.SelectedIndex > 0 ? ddlCity.SelectedItem.Value : string.Empty;
            BindDdlTowns(ddlTown, EnrollMembershipHelper.GetTowns(hfCountry.Value, hfCity.Value));
            Page.ClientScript.RegisterStartupScript(GetType(), "safdg2",
                                                            "<script> Show('fAddress');</script>");
        }
        protected void ddlNewCorpCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfCountry.Value = ddlNewCorpCountry.SelectedIndex > 0 ? ddlNewCorpCountry.SelectedItem.Value : string.Empty;
            BindDdlCities(ddlNewCorpCity, ddlNewCorpTown, EnrollMembershipHelper.GetCities(hfCountry.Value));
        }
        protected void ddlNewCorpCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfCity.Value = ddlNewCorpCity.SelectedIndex > 0 ? ddlNewCorpCity.SelectedItem.Value : string.Empty;
            BindDdlTowns(ddlNewCorpTown, EnrollMembershipHelper.GetTowns(hfCountry.Value, hfCity.Value));
        }
        #endregion

        #region bind Country-City-Town-Address-Email-Users-Corporation

        #region set selected item index <ddlCcountries, ddlCities, ddlTowns>
        public void BindHomeCountryCityTown(CorporationAddress corporationAddress, DropDownList ddlCountries, DropDownList ddlCities, DropDownList ddlTowns)
        {

            BindDdlCountries(ddlCountries, ddlCities, ddlTowns, EnrollMembershipHelper.GetCountries());
            if (corporationAddress.CountryId != null)
            {
                for (var i = 0; i < ddlCountries.Items.Count; i++)
                {
                    if (ddlCountries.Items[i].Value == corporationAddress.CountryId.ToString())
                    {
                        ddlCountries.SelectedIndex = i;
                        break;
                    }
                }
                BindDdlCities(ddlCities, ddlTowns, EnrollMembershipHelper.GetCities(corporationAddress.CountryId.ToString()));
                if (corporationAddress.CityId != null)
                {
                    for (var i = 0; i < ddlCities.Items.Count; i++)
                    {
                        if (ddlCities.Items[i].Value == corporationAddress.CityId.Value.ToString())
                        {
                            ddlCities.SelectedIndex = i;
                            break;
                        }
                    }

                    ddlTowns = BindDdlTowns(ddlTowns, EnrollMembershipHelper.GetTowns(corporationAddress.CountryId.ToString(),
                        corporationAddress.CityId.ToString()));
                    if (corporationAddress.TownId != null)
                    {
                        for (var i = 0; i < ddlTowns.Items.Count; i++)
                        {
                            if (ddlTowns.Items[i].Value == corporationAddress.TownId.Value.ToString())
                            {
                                ddlTowns.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region bind country-city-town
        public static void BindDdlCountries(DropDownList dropDownList, DropDownList ddlCity, DropDownList ddlTown, List<Countries> list)
        {
            dropDownList.Items.Clear();
            ddlCity.Items.Clear();
            ddlTown.Items.Clear();
            if (list != null)
                foreach (Countries item in list)
                {
                    dropDownList.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            dropDownList.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            ddlCity.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            ddlTown.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
        }
        public static void BindDdlCities(DropDownList dropDownList, DropDownList ddlTown, List<Cities> list)
        {
            dropDownList.Items.Clear();
            ddlTown.Items.Clear();
            if (list != null)
                foreach (Cities item in list)
                {
                    dropDownList.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            dropDownList.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            ddlTown.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
        }
        public static DropDownList BindDdlTowns(DropDownList dropDownList, List<Towns> list)
        {
            dropDownList.Items.Clear();
            if (list != null)
                foreach (Towns item in list)
                {
                    dropDownList.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            dropDownList.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            return dropDownList;
        }
        #endregion

        public void BindCorporationInfo(Corporations corporation)
        {
            tbName.Text = corporation.Name;
            tbDesc.Text = corporation.Description;
            tbTaxDept.Text = corporation.TaxDept;
            tbTaxNumber.Text = corporation.TaxNo;
            cbState.Checked = corporation.State;
        }
        public void BindContactAddress(int corporationId)
        {
            BindAddress(ddlContactAddress, corporationId);

            var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);
            if (corporation != null)
            {
                var index = 0;
                foreach (ListItem address in ddlContactAddress.Items)
                {
                    if (!string.IsNullOrEmpty(address.Value) && corporation.ContactAddressId != null && Convert.ToInt32(address.Value) == corporation.ContactAddressId)
                    {
                        ddlContactAddress.SelectedIndex = index;
                        break;
                    }
                    index++;
                }
            }
        }
        public void BindInvoiceAddress(int corporationId)
        {
            BindAddress(ddlInvoiceAddress, corporationId);

            var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);
            if (corporation != null)
            {
                var index = 0;
                foreach (ListItem address in ddlInvoiceAddress.Items)
                {
                    if (!string.IsNullOrEmpty(address.Value) && corporation.InvoiceAddressId != null && Convert.ToInt32(address.Value) == corporation.InvoiceAddressId)
                    {
                        ddlInvoiceAddress.SelectedIndex = index;
                        break;
                    }
                    index++;
                }
            }
        }
        public void BindAddress(DropDownList ddList, int corporationId)
        {
            ddList.Items.Clear();
            ddList.DataTextField = "Title";
            ddList.DataValueField = "Id";
            ddList.DataSource = _entities.CorporationAddress.Where(p => p.CorporationId == corporationId).ToList();
            ddList.DataBind();
            ddList.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
        }
        public void BindUsers()
        {
            var _users =
            (from users in _entities.Users
             select new { id = users.Id, name = users.Name + " " + users.Surname, }).ToArray();

            cbUsers.Items.Clear();
            foreach (var user in _users)
            {
                var item = new ListItem(user.name, user.id.ToString());
                cbUsers.Items.Add(item);
            }
        }

        #endregion

        #region grid

        #region grid Corporations

        protected void gvCorporations_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var corporationId = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Guncelle")
                {

                    btnAddNewAddress.Enabled = true;
                    btnAddNewUsers.Enabled = true;

                    hfCorporationId.Value = corporationId.ToString();
                    var corp = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);
                    BindCorporationInfo(corp);
                    BindContactAddress(corporationId);
                    BindInvoiceAddress(corporationId);
                    gvCorporationUsers.DataBind();
                    mvOperations.SetActiveView(vEdit);
                }
                if (e.CommandName == "Sil")
                {
                    var corporation = _entities.Corporations.FirstOrDefault(p => p.Id == corporationId);
                    var corporationAddressList = _entities.CorporationAddress.Where(p => p.CorporationId == corporationId).ToList();
                    if (corporationAddressList.Count > 0)
                    {
                        foreach (CorporationAddress address in corporationAddressList)
                        {
                            _entities.CorporationAddress.DeleteObject(address);
                            _entities.SaveChanges();
                        }
                    }
                    var corporationUserList = _entities.CorporationUser.Where(p => p.CorporationId == corporationId).ToList();
                    if (corporationUserList.Count > 0)
                    {
                        foreach (CorporationUser corporationUser in corporationUserList)
                        {
                            _entities.CorporationUser.DeleteObject(corporationUser);
                            _entities.SaveChanges();
                        }
                    }
                    var corporationDuesLogList = _entities.CorporationDuesLog.Where(p => p.CorporationId == corporationId).ToList();
                    if (corporationDuesLogList.Count > 0)
                    {
                        foreach (CorporationDuesLog corporationDuesLog in corporationDuesLogList)
                        {
                            _entities.CorporationDuesLog.DeleteObject(corporationDuesLog);
                            _entities.SaveChanges();
                        }
                    }
                    var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == corporationId);
                    if (corporationFinance != null)
                    {
                        _entities.CorporationFinance.DeleteObject(corporationFinance);
                        _entities.SaveChanges();
                    }

                    _entities.Corporations.DeleteObject(corporation);
                    _entities.SaveChanges();

                    //Logger.Add( ,  , corporationId, 2);

                    gvCorporations.DataBind();
                    MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        protected void gvCorporations_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnDelete = e.Row.FindControl("imgBtnDelete") as ImageButton;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }

        #endregion

        #region grid CorporationUsers

        protected void gvCorporationUsers_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var userId = Convert.ToInt32(e.CommandArgument);
                if (e.CommandName == "Sil")
                {
                    var corporationUser = _entities.CorporationUser.FirstOrDefault(p => p.UserId == userId);
                    if (corporationUser != null)
                    {
                        _entities.CorporationUser.DeleteObject(corporationUser);
                        _entities.SaveChanges();
                        gvCorporationUsers.DataBind();
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void gvCorporationUsers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var btnDelete = e.Row.FindControl("imgBtnDelete") as ImageButton;
            if (btnDelete != null)
            {
                btnDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        #endregion

        #endregion

        #region add new corporation
        protected void btNew_OnClick(object sender, EventArgs e)
        {
            ClearInputs();
            hfCorporationId.Value = null;
            hfCorporationAddressId.Value = null;
            BindDdlCountries(ddlNewCorpCountry, ddlNewCorpCity, ddlNewCorpTown, EnrollMembershipHelper.GetCountries());
            mvOperations.SetActiveView(vAdd);
        }
        #endregion

        #region cancel new corporation
        protected void btnCancelCorporation_OnClick(object sender, EventArgs e)
        {
            ClearForm();
            mvOperations.SetActiveView(vList);
        }
        public void ClearForm()
        {
            BindDdlCountries(ddlCountry, ddlCity, ddlTown, EnrollMembershipHelper.GetCountries());
            tbName.Text = string.Empty;
            tbDesc.Text = string.Empty;
            tbTaxDept.Text = string.Empty;
            tbTaxNumber.Text = string.Empty;
            tbDetailedAddress.Text = string.Empty;
            tbPhone.Text = string.Empty;
            tbZipCode.Text = string.Empty;

        }

        #endregion

        #region addNew Click <Address, Users>
        protected void btnAddNewAddress_OnClick(object sender, EventArgs e)
        {
            hfCorporationAddressId.Value = null;
            BindDdlCountries(ddlCountry, ddlCity, ddlTown, EnrollMembershipHelper.GetCountries());
            tbAddressTitle.Text = string.Empty;
            tbAddressDesc.Text = string.Empty;
            tbDetailedAddress.Text = string.Empty;
            tbFax.Text = string.Empty;
            tbPhone.Text = string.Empty;
            tbWeb.Text = string.Empty;
            tbZipCode.Text = string.Empty;

            Page.ClientScript.RegisterStartupScript(GetType(), "addnew1",
                                                            "<script> Show('fAddress');</script>");
        }
        protected void btnAddNewUsers_OnClick(object sender, EventArgs e)
        {
            cbUsers.Items.Clear();
            Page.ClientScript.RegisterStartupScript(GetType(), "addnew4",
                                                            "<script> Show('fUsers');</script>");
        }
        #endregion

        protected void BtEditAddressClick(object sender, EventArgs e)
        {
            DropDownList ddlist = null;
            var btn = sender as Button;
            if (btn != null)
            {
                var process = btn.CommandArgument;
                if (process == "Invoice")
                {
                    ddlist = ddlInvoiceAddress;
                }
                else if (process == "Contact")
                {
                    ddlist = ddlContactAddress;
                }
            }
            if (ddlist != null && ddlist.SelectedIndex > 0)
            {
                hfCorporationAddressId.Value = ddlist.SelectedItem.Value;
                var corporationAddressId = Convert.ToInt32(ddlist.SelectedItem.Value);
                var corporationAddress = _entities.CorporationAddress.FirstOrDefault(p => p.Id == corporationAddressId);
                if (corporationAddress != null)
                {
                    tbAddressTitle.Text = corporationAddress.Title;
                    tbAddressDesc.Text = corporationAddress.Description;
                    tbDetailedAddress.Text = corporationAddress.DetailedAddress;
                    tbZipCode.Text = corporationAddress.ZipCode;
                    tbFax.Text = corporationAddress.Fax;
                    tbPhone.Text = corporationAddress.Phone;
                    tbEmail.Text = corporationAddress.Email;
                    tbWeb.Text = corporationAddress.Web;

                    BindHomeCountryCityTown(corporationAddress, ddlCountry, ddlCity, ddlTown);

                    Page.ClientScript.RegisterStartupScript(GetType(), "dsadsad2",
                                                            "<script> Show('fAddress');</script>");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "dsadsad2",
                                                            "<script> Hide('fAddress');</script>");
            }
        }
        protected void IbFilterUsersClick(object sender, ImageClickEventArgs e)
        {
            var foundNewUser = false;
            try
            {
                var corporationId = 0;
                if (!string.IsNullOrEmpty(hfCorporationId.Value))
                {
                    corporationId = Convert.ToInt32(hfCorporationId.Value);
                }
                
                var aranan = txtAra.Text;
                var filteredUserList = (from users in _entities.Users
                                        select new
                                        {
                                            id = users.Id,
                                            name = users.Name + " " + users.Surname,
                                            email = users.EMail,
                                            state = users.State,
                                            isadmin = users.Admin
                                        }
                              ).Where(p => (p.isadmin==false && (p.name.Contains(aranan) || p.email.Contains(aranan))) ).ToArray();

                cbUsers.Items.Clear();
                foreach (var user in filteredUserList)
                {
                    var item = new ListItem(user.name+" ("+user.email+")", user.id.ToString());
                    if (_entities.CorporationUser.FirstOrDefault(p => p.UserId == user.id && p.CorporationId == corporationId) == null)
                    { 
                        cbUsers.Items.Add(item);
                        foundNewUser = true;
                    }
                }
                if (cbUsers.Items.Count < 3) cbUsers.RepeatColumns = cbUsers.Items.Count;
                gvCorporationUsers.DataBind();
                btnSaveUpdateUsers.Visible = foundNewUser;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "searfcdxfgew1",
                                                            "<script> Show('fUsers');</script>");
        }

        #region Helpers
        protected string GetUserName(int userId)
        {
            var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
            if (user != null)
            {
                return user.Name + " " + user.Surname;
            }
            return string.Empty;
        }
        protected string GetUserEmail(int userId)
        {
            var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
            if (user != null)
            {
                return user.EMail;
            }
            return string.Empty;
        }
        protected string GetUsersCountOnCorporation(int corporationId)
        {
            return _entities.CorporationUser.Count(p => p.CorporationId == corporationId).ToString();
        }

        protected void ClearInputs()
        {
            tbNewCorpName.Text = string.Empty;
            tbNewCorpTaxDept.Text = string.Empty;
            tbNewCorpTaxNumber.Text = string.Empty;
            tbNewCorpDesc.Text = string.Empty;

            tbNewCorpAddressTitle.Text = string.Empty;
            tbNewCorpAddressDesc.Text = string.Empty;
            
            BindDdlCountries(ddlNewCorpCountry,ddlNewCorpCity,ddlNewCorpTown,EnrollMembershipHelper.GetCountries());
             
            tbNewCorpDetailedAddress.Text = string.Empty;
            tbNewCorpZipCode.Text = string.Empty;
            tbNewCorpFax.Text = string.Empty;
            tbNewCorpPhone.Text = string.Empty;
            tbNewCorpEmail.Text = string.Empty;
            tbNewCorpWeb.Text = string.Empty;
            tbNewCorpFax.Text = string.Empty;
        }

        #endregion

    }
}