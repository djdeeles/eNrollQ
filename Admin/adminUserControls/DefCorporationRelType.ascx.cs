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
    public partial class DefCorporationRelType : System.Web.UI.UserControl
    {
        public enum Proccess
        {
            Save = 1,
            Update = 2
        }
        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            BindRelTypes(lbCorporationRelationType);
            btSaveRelType.Text = AdminResource.lbAdd;
            btUpdateRelType.Text = AdminResource.lbUpdate;
            btnDelete.Text = AdminResource.lbDelete;
            cbNewRelType.Text = AdminResource.lbActive;
            cbEditRelType.Text = AdminResource.lbActive;
            rqValidaterListBox.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btnDelete.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";

            ClearForm();
        }

        protected void lbCorporationRelationType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var listbox = sender as ListBox;
            BindRelTypes(listbox);
            btUpdateRelType.Enabled = true;
        }

        protected void BtSaveRelType_OnClick(object sender, EventArgs e)
        {
            var relType = new FoundationRelType();
            SaveUpdateRelType(relType, Proccess.Save); 
        }

        protected void BtUpdateRelType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedRelType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedRelType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var relType = _entities.FoundationRelType.FirstOrDefault(p => p.Id == id);
                    if (relType != null)
                    {
                        SaveUpdateRelType(relType, Proccess.Update);
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


        protected void BtDeleteType_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedRelType.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedRelType.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var relType = _entities.FoundationRelType.FirstOrDefault(p => p.Id == id);
                    if (relType != null)
                    {
                        _entities.DeleteObject(relType);
                        _entities.SaveChanges();
                        lbCorporationRelationType.DataBind();
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

        public void BindRelTypes(ListBox listBox)
        {
            try
            {
                tbEditRelType.Text = string.Empty;
                if (listBox != null && listBox.SelectedItem != null && !string.IsNullOrWhiteSpace(listBox.SelectedItem.Value))
                {
                    hfSelectedRelType.Value = Crypto.Encrypt(listBox.SelectedItem.Value);
                    var relTypeId = Convert.ToInt32(listBox.SelectedItem.Value);
                    if (relTypeId > 0)
                    {
                        var relType = _entities.FoundationRelType.FirstOrDefault(p => p.Id == relTypeId);
                        if (relType != null)
                        {
                            tbEditRelType.Text = relType.Name;
                            cbEditRelType.Checked = relType.State;
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

        public void SaveUpdateRelType(FoundationRelType relType, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!FoundationRelTypeIsExist(tbNewRelType.Text, -1))
                    {
                        relType.Name = tbNewRelType.Text;
                        relType.State = cbNewRelType.Checked;
                        relType.CreatedTime = DateTime.Now;
                        relType.UpdatedTime = DateTime.Now;
                        _entities.AddToFoundationRelType(relType);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                        lbCorporationRelationType.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbNewRelType.Text, AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!FoundationRelTypeIsExist(tbEditRelType.Text, relType.Id))
                    {
                        relType.Name = tbEditRelType.Text;
                        relType.State = cbEditRelType.Checked;
                        relType.UpdatedTime = DateTime.Now;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                        lbCorporationRelationType.DataBind();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                            tbEditRelType.Text, AdminResource.AlreadyExistInSystem));
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
            tbEditRelType.Text = string.Empty;
            tbNewRelType.Text = string.Empty;
            cbEditRelType.Checked = false;
            cbNewRelType.Checked = false;
        }


        private bool FoundationRelTypeIsExist(string text, int id)
        {
            FoundationRelType foundationRelType = null;
            try
            {
                if (id != -1)
                    foundationRelType = _entities.FoundationRelType.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    foundationRelType = _entities.FoundationRelType.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (foundationRelType != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }

    }
}