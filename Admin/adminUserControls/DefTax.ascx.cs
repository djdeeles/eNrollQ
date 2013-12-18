using System;
using System.Collections.Generic;
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
    public partial class DefTax : System.Web.UI.UserControl
    {
        public enum Proccess
        {
            Save = 1,
            Update = 2
        }
        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindTaxTypes(lbTaxType);
            btSaveTaxType.Text = AdminResource.lbAdd;
            btUpdateTaxType.Text = AdminResource.lbUpdate;
            btDeleteTaxType.Text = AdminResource.lbDelete;
            rqValListBoxDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rqValListBoxEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btDeleteTaxType.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";

            ClearForm();
        }

        protected void lbTaxType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            BindTaxTypes(listbox);
            btUpdateTaxType.Enabled = true;
        }

        protected void BtSaveTaxType_OnClick(object sender, EventArgs e)
        {
            var TaxTypes = new TaxTypes();
            SaveUpdateTaxType(TaxTypes, Proccess.Save); 
        }

        protected void BtUpdateTaxType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedTaxType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedTaxType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var TaxType = _entities.TaxTypes.FirstOrDefault(p => p.Id == id);
                    if (TaxType != null)
                    {
                        SaveUpdateTaxType(TaxType, Proccess.Update);
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


        protected void BtDeleteTaxType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedTaxType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedTaxType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var TaxType = _entities.TaxTypes.FirstOrDefault(p => p.Id == id);
                    if (TaxType != null)
                    {
                        _entities.DeleteObject(TaxType);
                        _entities.SaveChanges();
                        lbTaxType.DataBind();
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

        public void BindTaxTypes(ListBox listBox)
        {
            try
            {
                tbEditTaxType.Text = string.Empty;
                if (listBox != null && listBox.SelectedItem != null && !string.IsNullOrWhiteSpace(listBox.SelectedItem.Value))
                {
                    hfSelectedTaxType.Value = Crypto.Encrypt(listBox.SelectedItem.Value);
                    var TaxTypeId = Convert.ToInt32(listBox.SelectedItem.Value);
                    if (TaxTypeId > 0)
                    {
                        var TaxType = _entities.TaxTypes.FirstOrDefault(p => p.Id == TaxTypeId);
                        if (TaxType != null)
                        {
                            tbEditTaxType.Text = TaxType.Name;
                            tbEditTaxValue.Text = TaxType.Value.ToString("0.00");
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

        public void SaveUpdateTaxType(TaxTypes taxType, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!TaxTypesIsExist(tbTaxType.Text, -1))
                    {
                        taxType.Name = tbTaxType.Text;
                        taxType.Value = Convert.ToDecimal(tbTaxValue.Text);
                        _entities.AddToTaxTypes(taxType);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                        lbTaxType.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbTaxType.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!TaxTypesIsExist(tbEditTaxType.Text, taxType.Id))
                    {
                        taxType.Name = tbEditTaxType.Text;
                        taxType.Value = Convert.ToDecimal(tbEditTaxValue.Text);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbTaxType.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbEditTaxType.Text, AdminResource.AlreadyExistInSystem));
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
            tbEditTaxType.Text = string.Empty;
            tbEditTaxValue.Text = string.Empty;
            tbTaxType.Text = string.Empty;
            tbTaxValue.Text = string.Empty;
        }


        private bool TaxTypesIsExist(string text, int id)
        {
            TaxTypes taxTypes = null;
            try
            {
                if (id != -1)
                    taxTypes = _entities.TaxTypes.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    taxTypes = _entities.TaxTypes.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (taxTypes != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }

    }
}