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
    public partial class DefBloodType : System.Web.UI.UserControl
    {
        public enum Proccess
        {
            Save = 1,
            Update = 2
        }
        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindBloodTypes(lbBloodType);
            btSaveBloodType.Text = AdminResource.lbAdd;
            btUpdateBloodType.Text = AdminResource.lbUpdate;
            btDeleteBloodType.Text = AdminResource.lbDelete;
            rqValListBoxDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rqValListBoxEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btDeleteBloodType.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";

            ClearForm();
        }

        protected void lbBloodType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            BindBloodTypes(listbox);
            btUpdateBloodType.Enabled = true;
        }

        protected void BtSaveBloodType_OnClick(object sender, EventArgs e)
        {
            var bloodType = new BloodTypes();
            SaveUpdateBloodType(bloodType, Proccess.Save);
            lbBloodType.DataBind();
        }

        protected void BtUpdateBloodType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedBloodType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedBloodType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var bloodType = _entities.BloodTypes.FirstOrDefault(p => p.Id == id);
                    if (bloodType != null)
                    {
                        SaveUpdateBloodType(bloodType, Proccess.Update);
                        lbBloodType.DataBind();
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


        protected void BtDeleteBloodType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedBloodType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedBloodType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var bloodType = _entities.BloodTypes.FirstOrDefault(p => p.Id == id);
                    if (bloodType != null)
                    {
                        _entities.DeleteObject(bloodType);
                        _entities.SaveChanges();
                        lbBloodType.DataBind();
                        MessageBox.Show(MessageType.Success,AdminResource.msgDeleted);
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

        public void BindBloodTypes(ListBox listBox)
        {
            try
            {
                tbEditBloodType.Text = string.Empty;
                if (listBox != null && listBox.SelectedItem != null && !string.IsNullOrWhiteSpace(listBox.SelectedItem.Value))
                {
                    hfSelectedBloodType.Value = Crypto.Encrypt(listBox.SelectedItem.Value);
                    var bloodTypeId = Convert.ToInt32(listBox.SelectedItem.Value);
                    if (bloodTypeId > 0)
                    {
                        var bloodType = _entities.BloodTypes.FirstOrDefault(p => p.Id == bloodTypeId);
                        if (bloodType != null)
                        {
                            tbEditBloodType.Text = bloodType.Name;
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

        public void SaveUpdateBloodType(BloodTypes bloodType, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    bloodType.Name = tbNewBloodType.Text;
                    _entities.AddToBloodTypes(bloodType);
                    _entities.SaveChanges();
                    MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                    ClearForm();
                }
                else if (proccess == Proccess.Update)
                {
                    bloodType.Name = tbEditBloodType.Text;
                    _entities.SaveChanges();
                    MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                    ClearForm();
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
            tbEditBloodType.Text = string.Empty;
            tbNewBloodType.Text = string.Empty;
        }

    }
}