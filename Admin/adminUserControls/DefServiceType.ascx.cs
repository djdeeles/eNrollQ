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
    public partial class DefServiceType : System.Web.UI.UserControl
    {
        public enum Proccess
        {
            Save = 1,
            Update = 2
        }
        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindServiceTypes(lbServiceType);
            btSaveServiceType.Text = AdminResource.lbAdd;
            btUpdateServiceType.Text = AdminResource.lbUpdate;
            btDeleteServiceType.Text = AdminResource.lbDelete;
            rqValListBoxDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rqValListBoxEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btDeleteServiceType.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";

            ClearForm();
        }

        protected void lbServiceType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            BindServiceTypes(listbox);
            btUpdateServiceType.Enabled = true;
        }

        protected void BtSaveServiceType_OnClick(object sender, EventArgs e)
        {
            var serviceTypes = new ServiceTypes();
            SaveUpdateServiceType(serviceTypes, Proccess.Save);
            
        }

        protected void BtUpdateServiceType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedServiceType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedServiceType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var serviceType = _entities.ServiceTypes.FirstOrDefault(p => p.Id == id);
                    if (serviceType != null)
                    {
                        SaveUpdateServiceType(serviceType, Proccess.Update); 
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


        protected void BtDeleteServiceType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedServiceType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedServiceType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var serviceType = _entities.ServiceTypes.FirstOrDefault(p => p.Id == id);
                    if (serviceType != null)
                    {
                        _entities.DeleteObject(serviceType);
                        _entities.SaveChanges();
                        lbServiceType.DataBind();
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

        public void BindServiceTypes(ListBox listBox)
        {
            try
            {
                tbEditServiceType.Text = string.Empty;
                if (listBox != null && listBox.SelectedItem != null && !string.IsNullOrWhiteSpace(listBox.SelectedItem.Value))
                {
                    hfSelectedServiceType.Value = Crypto.Encrypt(listBox.SelectedItem.Value);
                    var serviceTypeId = Convert.ToInt32(listBox.SelectedItem.Value);
                    if (serviceTypeId > 0)
                    {
                        var serviceType = _entities.ServiceTypes.FirstOrDefault(p => p.Id == serviceTypeId);
                        if (serviceType != null)
                        {
                            tbEditServiceType.Text = serviceType.Name;
                            tbEditServiceDesc.Text = serviceType.Description;
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

        public void SaveUpdateServiceType(ServiceTypes serviceType, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!ServisTypeIsExist(tbServiceType.Text, -1))
                    {
                        serviceType.Name = tbServiceType.Text;
                        serviceType.Description = tbServiceDesc.Text;
                        _entities.AddToServiceTypes(serviceType);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                        lbServiceType.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbServiceType.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!ServisTypeIsExist(tbEditServiceType.Text, serviceType.Id))
                    {
                        serviceType.Name = tbEditServiceType.Text;
                        serviceType.Description = tbEditServiceDesc.Text;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbServiceType.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbEditServiceType.Text, AdminResource.AlreadyExistInSystem));
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
            tbEditServiceType.Text = string.Empty;
            tbEditServiceDesc.Text = string.Empty;
            tbServiceType.Text = string.Empty;
            tbServiceDesc.Text = string.Empty;
        }


        private bool ServisTypeIsExist(string text, int id)
        {
            ServiceTypes serviceTypes = null;
            try
            {
                if (id != -1)
                    serviceTypes = _entities.ServiceTypes.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    serviceTypes = _entities.ServiceTypes.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (serviceTypes != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }

    }
}