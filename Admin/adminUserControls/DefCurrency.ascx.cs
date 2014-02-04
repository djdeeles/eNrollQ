using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class DefCurrency : UserControl
    {
        #region Proccess enum

        public enum Proccess
        {
            Save = 1,
            Update = 2
        }

        #endregion

        private readonly Entities _entities = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            BindCurrency(lbCurrency);
            btSaveCurrency.Text = AdminResource.lbAdd;
            btUpdateCurrency.Text = AdminResource.lbUpdate;
            btDeleteCurrency.Text = AdminResource.lbDelete;
            rqValListBoxDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rqValListBoxEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            ClearForm();
            BindRadioButtonDirection();

            btDeleteCurrency.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";
        }

        protected void lbCurrency_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            BindCurrency(listbox);
            btUpdateCurrency.Enabled = true;
        }

        protected void BtSaveCurrency_OnClick(object sender, EventArgs e)
        {
            var currency = new Currency();
            SaveUpdateCurrency(currency, Proccess.Save);
        }

        protected void BtUpdateCurrency_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedCurrency.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedCurrency.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var currency = _entities.Currency.FirstOrDefault(p => p.Id == id);
                    if (currency != null)
                    {
                        SaveUpdateCurrency(currency, Proccess.Update);
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

        protected void BtDeleteCurrency_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedCurrency.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedCurrency.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var currency = _entities.Currency.FirstOrDefault(p => p.Id == id);
                    if (currency != null)
                    {
                        _entities.DeleteObject(currency);
                        _entities.SaveChanges();
                        lbCurrency.DataBind();
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

        public void BindCurrency(ListBox listBox)
        {
            try
            {
                tbEditCurrency.Text = string.Empty;
                if (listBox != null && listBox.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(listBox.SelectedItem.Value))
                {
                    hfSelectedCurrency.Value = Crypto.Encrypt(listBox.SelectedItem.Value);
                    var currencyId = Convert.ToInt32(listBox.SelectedItem.Value);
                    if (currencyId > 0)
                    {
                        var currency = _entities.Currency.FirstOrDefault(p => p.Id == currencyId);
                        if (currency != null)
                        {
                            tbEditCurrency.Text = currency.Name;
                            tbEditSymbol.Text = currency.Symbol;
                            cbEditSiteMainCurrencyUnit.Checked = currency.Active;
                            rbEditDirection.SelectedIndex = currency.Position.Trim(' ') == "L" ? 0 : 1;
                            if (currency.Active)
                            {
                                cbEditSiteMainCurrencyUnit.Enabled = false;
                                btDeleteCurrency.OnClientClick = "showNoticeToast('" +
                                                                 AdminResource.lbCanNotDeleteDefaultCurrency +
                                                                 "');return false;";
                            }
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

        public void SaveUpdateCurrency(Currency currency, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!CurrencyIsExist(tbNewCurrency.Text, -1))
                    {
                        var activeCurrency = _entities.Currency.Count(p => p.Active);
                        currency.Name = tbNewCurrency.Text;
                        currency.Symbol = tbNewSymbol.Text;
                        currency.Position = rbNewDirection.SelectedIndex == 0 ? "L" : "R";
                        currency.Active = activeCurrency == 0 || cbSiteMainCurrencyUnit.Checked;
                        _entities.AddToCurrency(currency);
                        _entities.SaveChanges();
                        CheckDefaultCurrency(currency);
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                        lbCurrency.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                                                                           tbNewCurrency.Text,
                                                                           AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!CurrencyIsExist(tbEditCurrency.Text, currency.Id))
                    {
                        currency.Name = tbEditCurrency.Text;
                        currency.Symbol = tbEditSymbol.Text;
                        currency.Active = cbEditSiteMainCurrencyUnit.Checked;
                        currency.Position = rbEditDirection.SelectedIndex == 0 ? "L" : "R";
                        _entities.SaveChanges();
                        CheckDefaultCurrency(currency);
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbCurrency.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                                                                           tbEditCurrency.Text,
                                                                           AdminResource.AlreadyExistInSystem));
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
        }

        public void ClearForm()
        {
            tbEditCurrency.Text = string.Empty;
            tbEditSymbol.Text = string.Empty;
            rbEditDirection.SelectedIndex = 0;
            cbEditSiteMainCurrencyUnit.Checked = false;

            cbEditSiteMainCurrencyUnit.Enabled = true;
            btDeleteCurrency.OnClientClick = "";

            tbNewCurrency.Text = string.Empty;
            tbNewSymbol.Text = string.Empty;
            rbNewDirection.SelectedIndex = 0;
            cbSiteMainCurrencyUnit.Checked = false;
        }

        public void BindRadioButtonDirection()
        {
            rbEditDirection.Items.Clear();
            rbEditDirection.Items.Insert(0, new ListItem(AdminResource.lbLeft, "L"));
            rbEditDirection.Items.Insert(1, new ListItem(AdminResource.lbRight, "R"));

            rbNewDirection.Items.Clear();
            rbNewDirection.Items.Insert(0, new ListItem(AdminResource.lbLeft, "L"));
            rbNewDirection.Items.Insert(1, new ListItem(AdminResource.lbRight, "R"));


            rbEditDirection.SelectedIndex = 0;
            rbNewDirection.SelectedIndex = 0;
        }

        public void CheckDefaultCurrency(Currency eCurrency)
        {
            if (eCurrency.Active)
            {
                var currencies = _entities.Currency.Where(p => p.Id != eCurrency.Id).ToList();
                foreach (var currency in currencies)
                {
                    currency.Active = false;
                    _entities.SaveChanges();
                }
            }
        }

        private bool CurrencyIsExist(string text, int id)
        {
            Currency currency = null;
            try
            {
                if (id != -1)
                    currency = _entities.Currency.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    currency = _entities.Currency.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (currency != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }
    }
}