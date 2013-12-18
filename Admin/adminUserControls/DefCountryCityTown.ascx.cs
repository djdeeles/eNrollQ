using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class DefCountryCityTown : System.Web.UI.UserControl
    {
        public enum Proccess
        {
            Save = 1,
            Update = 2
        }
        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindCountries(lbCountry);
            btSaveCountry.Text = AdminResource.lbAdd;
            btUpdateCountry.Text = AdminResource.lbUpdate;
            btnDeleteCountry.Text = AdminResource.lbDelete;
            rValCountryEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rValCountryDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btSaveCity.Text = AdminResource.lbAdd;
            btUpdateCity.Text = AdminResource.lbUpdate;
            btnDeleteCity.Text = AdminResource.lbDelete;
            rValCityEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rValCityDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btSaveTown.Text = AdminResource.lbAdd;
            btUpdateTown.Text = AdminResource.lbUpdate;
            btnDeleteTown.Text = AdminResource.lbDelete;
            rValTownEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rValTownDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btnDeleteCountry.OnClientClick = "return confirm('" + AdminResource.lbConfirmMsgDeleteCountry + "');";
            btnDeleteCity.OnClientClick = "return confirm('" + AdminResource.lbConfirmMsgDeleteCity + "');";
            btnDeleteTown.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";

            ClearForm();
        }

        #region ListBox_SelectedIndexChanged
        protected void lbCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var listbox = sender as ListBox;
                edsCity.WhereParameters.Clear();
                if (listbox != null)
                {
                    var countryId = listbox.SelectedItem.Value;
                    hfSelectedCountry.Value = Crypto.Encrypt(countryId);
                    edsCity.WhereParameters.Add("countryId", DbType.Int32, countryId);
                    BindCountries(lbCountry);
                }
                btUpdateCountry.Enabled = true;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        protected void lbCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                edsTown.WhereParameters.Clear();
                if (lbCity != null)
                {
                    var cityId = lbCity.SelectedItem.Value;
                    hfSelectedCity.Value = Crypto.Encrypt(cityId);
                    edsTown.WhereParameters.Add("cityId", DbType.Int32, cityId);
                    BindCities(lbCity);
                }
                btUpdateCity.Enabled = true;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

        }
        protected void lbTown_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var townId = lbTown.SelectedItem.Value;
                hfSelectedTown.Value = Crypto.Encrypt(townId);
                btUpdateTown.Enabled = true;
                BindTowns(lbTown);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

        }

        #endregion

        #region BindCountryCityTown
        private void BindCountries(ListBox listbox)
        {
            try
            {
                tbEditCountryName.Text = string.Empty;
                if (listbox != null && listbox.SelectedItem != null && !string.IsNullOrWhiteSpace(listbox.SelectedItem.Value))
                {
                    var id = Convert.ToInt32(listbox.SelectedItem.Value);
                    if (id > 0)
                    {
                        var country = _entities.Countries.FirstOrDefault(p => p.Id == id);
                        if (country != null)
                        {
                            tbEditCountryName.Text = country.Name;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        private void BindCities(ListBox listbox)
        {
            try
            {
                tbEditCityName.Text = string.Empty;
                if (listbox != null && listbox.SelectedItem != null && !string.IsNullOrWhiteSpace(listbox.SelectedItem.Value))
                {
                    var id = Convert.ToInt32(listbox.SelectedItem.Value);
                    if (id > 0)
                    {
                        var city = _entities.Cities.FirstOrDefault(p => p.Id == id);
                        if (city != null)
                        {
                            tbEditCityName.Text = city.Name;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        private void BindTowns(ListBox listbox)
        {
            try
            {
                tbEditTownName.Text = string.Empty;
                if (listbox != null && listbox.SelectedItem != null && !string.IsNullOrWhiteSpace(listbox.SelectedItem.Value))
                {
                    //hfSelectedTown.Value = Crypto.Encrypt(listbox.SelectedItem.Value);
                    var id = Convert.ToInt32(listbox.SelectedItem.Value);
                    if (id > 0)
                    {
                        var town = _entities.Towns.FirstOrDefault(p => p.Id == id);
                        if (town != null)
                        {
                            tbEditTownName.Text = town.Name;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        #endregion

        #region SaveClick
        protected void BtSaveCountry_OnClick(object sender, EventArgs e)
        {
            var countries = new Countries();
            SaveUpdateCountry(countries, Proccess.Save);
            
        }
        protected void BtSaveCity_OnClick(object sender, EventArgs e)
        {
            var cities = new Cities();
            SaveUpdateCity(cities, Proccess.Save);
        }
        protected void BtSaveTown_OnClick(object sender, EventArgs e)
        {
            var towns = new Towns();
            SaveUpdateTown(towns, Proccess.Save); 
        }
        #endregion

        #region UpdateClick
        protected void BtUpdateCountry_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedCountry.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedCountry.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var country = _entities.Countries.FirstOrDefault(p => p.Id == id);
                    if (country != null)
                    {
                        SaveUpdateCountry(country, Proccess.Update);
                    }
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }

        protected void BtUpdateCity_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedCity.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedCity.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var city = _entities.Cities.FirstOrDefault(p => p.Id == id);
                    if (city != null)
                    {
                        SaveUpdateCity(city, Proccess.Update); 
                    }
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        protected void BtUpdateTown_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedTown.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedTown.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var town = _entities.Towns.FirstOrDefault(p => p.Id == id);
                    if (town != null)
                    {
                        SaveUpdateTown(town, Proccess.Update); 
                    }
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        #endregion

        #region DeleteClick
        protected void BtDeleteCountry_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedCountry.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedCountry.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var country = _entities.Countries.FirstOrDefault(p => p.Id == id);
                    if (country != null)
                    {
                        var towns = _entities.Towns.Where(p => p.CountryId == country.Id);
                        foreach (var town in towns)
                        {
                            _entities.DeleteObject(town);
                        }
                        _entities.SaveChanges();

                        var cities = _entities.Cities.Where(p => p.CountryId == country.Id);
                        foreach (var city in cities)
                        {
                            _entities.DeleteObject(city);
                        }
                        _entities.SaveChanges();

                        _entities.DeleteObject(country);
                        _entities.SaveChanges();
                        lbCountry.DataBind();
                        lbCity.DataBind();
                        lbTown.DataBind();
                        ClearForm();
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    }
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        protected void BtDeleteCity_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedCity.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedCity.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var city = _entities.Cities.FirstOrDefault(p => p.Id == id);
                    if (city != null)
                    {
                        var towns = _entities.Towns.Where(p => p.CityId == city.Id);
                        foreach (var town in towns)
                        {
                            _entities.DeleteObject(town);
                        }
                        _entities.SaveChanges();

                        _entities.DeleteObject(city);
                        _entities.SaveChanges();
                        lbCity.DataBind();
                        lbTown.DataBind();
                        ClearForm();
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    }
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        protected void BtDeleteTown_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedTown.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedTown.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var town = _entities.Towns.FirstOrDefault(p => p.Id == id);
                    if (town != null)
                    {
                        _entities.DeleteObject(town);
                        _entities.SaveChanges();
                        lbTown.DataBind();
                        ClearForm();
                        MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    }
                }
                else
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        #endregion

        #region SaveUpdateProccess
        private void SaveUpdateCountry(Countries countries, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!CountriesIsExist(tbNewCountryName.Text, -1))
                    {
                        var id = 0;
                        var lastCountry = _entities.Countries.Take(1).OrderByDescending(p => p.Id).Single();
                        if (lastCountry != null)
                        {
                            var lastId = lastCountry.Id;
                            if (!string.IsNullOrWhiteSpace(lastId.ToString()))
                            {
                                id = Convert.ToInt32(lastId) + 1;
                            }
                        }
                        countries.Id = id;
                        countries.Name = tbNewCountryName.Text;
                        _entities.AddToCountries(countries);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm(); 
                        lbCountry.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbNewCountryName.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!CountriesIsExist(tbEditCountryName.Text, countries.Id))
                    {
                        countries.Name = tbEditCountryName.Text;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbCountry.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                               tbEditCountryName.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        private void SaveUpdateCity(Cities cities, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!CitiesIsExist(tbNewCityName.Text, -1))
                    {
                        var cityId = 0;
                        var lastCity = _entities.Cities.Take(1).OrderByDescending(p => p.Id).Single();
                        if (lastCity != null)
                        {
                            var lastId = lastCity.Id;
                            if (!string.IsNullOrWhiteSpace(lastId.ToString()))
                            {
                                cityId = Convert.ToInt32(lastId) + 1;
                            }
                        }
                        cities.Id = cityId;
                        cities.CountryId = Convert.ToInt32(lbCountry.SelectedItem.Value);
                        cities.Name = tbNewCityName.Text;
                        _entities.AddToCities(cities);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                        lbCity.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbNewCityName.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!CitiesIsExist(tbEditCityName.Text, cities.Id))
                    {
                        cities.Name = tbEditCityName.Text;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbCity.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbEditCityName.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        private void SaveUpdateTown(Towns towns, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!TownsIsExist(tbNewTownName.Text, towns.Id))
                    {
                        var id = 0;
                        var lastTown = _entities.Towns.Take(1).OrderByDescending(p => p.Id).Single();
                        if (lastTown != null)
                        {
                            var lastId = lastTown.Id;
                            if (!string.IsNullOrWhiteSpace(lastId.ToString()))
                            {
                                id = Convert.ToInt32(lastId) + 1;
                            }
                        }
                        towns.Id = id;
                        towns.CountryId = Convert.ToInt32(lbCountry.SelectedItem.Value);
                        towns.CityId = Convert.ToInt32(lbCity.SelectedItem.Value);
                        towns.Name = tbNewTownName.Text;
                        _entities.AddToTowns(towns);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                        lbTown.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                           tbNewTownName.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!TownsIsExist(tbEditTownName.Text, towns.Id))
                    {
                        towns.Name = tbEditTownName.Text;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbTown.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                           tbEditTownName.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }
        #endregion

        private void ClearForm()
        {
            tbEditCountryName.Text = string.Empty;
            tbNewCountryName.Text = string.Empty;

            tbEditCityName.Text = string.Empty;
            tbNewCityName.Text = string.Empty;

            tbEditTownName.Text = string.Empty;
            tbNewTownName.Text = string.Empty;
        }


        private bool CountriesIsExist(string text, int id)
        {
            Countries countries = null;
            try
            {
                if (id != -1)
                    countries = _entities.Countries.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    countries = _entities.Countries.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (countries != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }


        private bool CitiesIsExist(string text, int id)
        {
            Cities cities = null;
            try
            {
                if (id != -1)
                    cities = _entities.Cities.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    cities = _entities.Cities.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (cities != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }


        private bool TownsIsExist(string text, int id)
        {
            Towns towns = null;
            try
            {
                if (id != -1)
                    towns = _entities.Towns.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    towns = _entities.Towns.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (towns != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }


    }
}