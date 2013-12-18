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
    public partial class DefDues : System.Web.UI.UserControl
    {
        public enum Proccess
        {
            Save = 1,
            Update = 2
        }
        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDuesTypes(lbDues);
            btSaveDues.Text = AdminResource.lbAdd;
            btUpdateDues.Text = AdminResource.lbUpdate;
            btDeleteDues.Text = AdminResource.lbDelete;
            cbUniqe.Text = AdminResource.lbUniqeDues;
            cbEditUniqe.Text = AdminResource.lbUniqeDues;
            rqValListBoxDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rqValListBoxEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            ClearForm();
            btDeleteDues.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";

            ltNewAmountSymbolL.Text = (EnrollCurrency.SiteDefaultCurrency().Position == "L" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "");
            ltNewAmountSymbolR.Text = (EnrollCurrency.SiteDefaultCurrency().Position == "R" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "");
            ltEditAmountSymbolL.Text = (EnrollCurrency.SiteDefaultCurrency().Position == "L" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "");
            ltEditAmountSymbolR.Text = (EnrollCurrency.SiteDefaultCurrency().Position == "R" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "");
        }

        protected void lbDuesTypes_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            BindDuesTypes(listbox);
            btUpdateDues.Enabled = true;
        }

        protected void BtSaveDuesTypes_OnClick(object sender, EventArgs e)
        {
            var duesTypes = new DuesTypes();
            SaveUpdateDuesTypes(duesTypes, Proccess.Save); 
        }

        protected void BtUpdateDuesTypes_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedDues.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedDues.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var duesTypes = _entities.DuesTypes.FirstOrDefault(p => p.Id == id);
                    if (duesTypes != null)
                    {
                        SaveUpdateDuesTypes(duesTypes, Proccess.Update);
                        
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

        protected void BtDeleteDuesTypes_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedDues.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedDues.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var duesTypes = _entities.DuesTypes.FirstOrDefault(p => p.Id == id);
                    if (duesTypes != null)
                    {
                        _entities.DeleteObject(duesTypes);
                        _entities.SaveChanges();
                        ClearForm();
                        lbDues.DataBind();
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

        public void BindDuesTypes(ListBox listBox)
        {
            try
            {
                tbEditDues.Text = string.Empty;
                if (listBox != null && listBox.SelectedItem != null && !string.IsNullOrWhiteSpace(listBox.SelectedItem.Value))
                {
                    hfSelectedDues.Value = Crypto.Encrypt(listBox.SelectedItem.Value);
                    var duesTypesId = Convert.ToInt32(listBox.SelectedItem.Value);
                    if (duesTypesId > 0)
                    {
                        var duesTypes = _entities.DuesTypes.FirstOrDefault(p => p.Id == duesTypesId);
                        if (duesTypes != null)
                        {
                            tbEditDues.Text = duesTypes.Title;

                            tbEditAmount.Text = duesTypes.Amount.ToString(".");
                            try
                            {
                                tbEditAmountKrs.Text = duesTypes.Amount.ToString(".00").Split('.')[1];
                            }
                            catch (Exception)
                            {

                            }
                            cbEditUniqe.Checked = duesTypes.Uniqe;
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

        public void SaveUpdateDuesTypes(DuesTypes duesTypes, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!DuesTypeIsExist(tbNewDues.Text, -1))
                    {
                        duesTypes.Title = tbNewDues.Text;
                        duesTypes.Amount = Convert.ToDecimal(tbNewAmount.Text + "." + tbNewAmountKrs.Text);
                        duesTypes.State = true;
                        duesTypes.Uniqe = cbUniqe.Checked;
                        duesTypes.Date = DateTime.Now;
                        _entities.AddToDuesTypes(duesTypes);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                        lbDues.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbNewDues.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!DuesTypeIsExist(tbEditDues.Text, duesTypes.Id))
                    {
                        duesTypes.Title = tbEditDues.Text;
                        duesTypes.Amount = Convert.ToDecimal(tbEditAmount.Text + "." + tbEditAmountKrs.Text);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbDues.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbEditDues.Text, AdminResource.AlreadyExistInSystem));
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
            tbEditDues.Text = string.Empty;
            tbEditAmount.Text = string.Empty;
            tbEditAmountKrs.Text = "00";
            cbEditUniqe.Checked = false;

            tbNewDues.Text = string.Empty;
            tbNewAmount.Text = string.Empty;
            tbNewAmountKrs.Text = "00";
            cbUniqe.Checked = false;
        }


        private bool DuesTypeIsExist(string text, int id)
        {
            DuesTypes duesTypes = null;
            try
            {
                if (id != -1)
                    duesTypes = _entities.DuesTypes.FirstOrDefault(p => p.Title.ToLower() == text.ToLower() && p.Id != id);
                else
                    duesTypes = _entities.DuesTypes.FirstOrDefault(p => p.Title.ToLower() == text.ToLower());

                if (duesTypes != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }

    }
}