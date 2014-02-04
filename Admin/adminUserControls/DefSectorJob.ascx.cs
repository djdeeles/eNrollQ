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
    public partial class DefSectorJob : UserControl
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
            BindSectors(lbSector);
            BindJobs(lbJob);
            btSaveSector.Text = AdminResource.lbAdd;
            btUpdateSector.Text = AdminResource.lbUpdate;
            btnDeleteSector.Text = AdminResource.lbDelete;
            rValSectorEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rValSectorDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btSaveJob.Text = AdminResource.lbAdd;
            btUpdateJob.Text = AdminResource.lbUpdate;
            btnDeleteJob.Text = AdminResource.lbDelete;
            rValJobEdit.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;
            rValJobDelete.ErrorMessage = AdminResource.msgSelectAtLeastOneElement;

            btnDeleteSector.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";
            btnDeleteJob.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";

            ClearForm();
        }

        private void ClearForm()
        {
            tbEditSectorName.Text = string.Empty;
            tbNewSectorName.Text = string.Empty;

            tbEditJobName.Text = string.Empty;
            tbNewJobName.Text = string.Empty;
        }

        private bool SectorIsExist(string text, int id)
        {
            JobSectors jobSectors = null;
            try
            {
                if (id != -1)
                    jobSectors =
                        _entities.JobSectors.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    jobSectors = _entities.JobSectors.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (jobSectors != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }

        private bool JobIsExist(string text, int id)
        {
            Jobs jobs = null;
            try
            {
                if (id != -1)
                    jobs = _entities.Jobs.FirstOrDefault(p => p.Name.ToLower() == text.ToLower() && p.Id != id);
                else
                    jobs = _entities.Jobs.FirstOrDefault(p => p.Name.ToLower() == text.ToLower());

                if (jobs != null) return true;
            }
            catch (Exception e)
            {
                ExceptionManager.ManageException(e);
            }
            return false;
        }

        #region ListBox_SelectedIndexChanged

        protected void lbSector_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var listbox = sender as ListBox;
                if (listbox != null)
                {
                    var sectorId = listbox.SelectedItem.Value;
                    hfSelectedSector.Value = Crypto.Encrypt(sectorId);
                    BindSectors(lbSector);
                }
                btUpdateSector.Enabled = true;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void lbJob_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbJob != null)
                {
                    var jobId = lbJob.SelectedItem.Value;
                    hfSelectedJob.Value = Crypto.Encrypt(jobId);
                    BindJobs(lbJob);
                }
                btUpdateJob.Enabled = true;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #endregion

        #region BindSectorJobTown

        private void BindSectors(ListBox listbox)
        {
            try
            {
                tbEditSectorName.Text = string.Empty;
                if (listbox != null && listbox.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(listbox.SelectedItem.Value))
                {
                    var id = Convert.ToInt32(listbox.SelectedItem.Value);
                    if (id > 0)
                    {
                        var sector = _entities.JobSectors.FirstOrDefault(p => p.Id == id);
                        if (sector != null)
                        {
                            tbEditSectorName.Text = sector.Name;
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

        private void BindJobs(ListBox listbox)
        {
            try
            {
                tbEditJobName.Text = string.Empty;
                if (listbox != null && listbox.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(listbox.SelectedItem.Value))
                {
                    var id = Convert.ToInt32(listbox.SelectedItem.Value);
                    if (id > 0)
                    {
                        var job = _entities.Jobs.FirstOrDefault(p => p.Id == id);
                        if (job != null)
                        {
                            tbEditJobName.Text = job.Name;
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

        protected void BtSaveSector_OnClick(object sender, EventArgs e)
        {
            var sectors = new JobSectors();
            SaveUpdateSector(sectors, Proccess.Save);
        }

        protected void BtSaveJob_OnClick(object sender, EventArgs e)
        {
            var jobs = new Jobs();
            SaveUpdateJob(jobs, Proccess.Save);
        }

        #endregion

        #region UpdateClick

        protected void BtUpdateSector_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedSector.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedSector.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var sector = _entities.JobSectors.FirstOrDefault(p => p.Id == id);
                    if (sector != null)
                    {
                        SaveUpdateSector(sector, Proccess.Update);
                        ClearForm();
                        lbSector.DataBind();
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

        protected void BtUpdateJob_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedJob.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedJob.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var job = _entities.Jobs.FirstOrDefault(p => p.Id == id);
                    if (job != null)
                    {
                        SaveUpdateJob(job, Proccess.Update);
                        ClearForm();
                        lbJob.DataBind();
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

        protected void BtDeleteSector_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedSector.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedSector.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var sector = _entities.JobSectors.FirstOrDefault(p => p.Id == id);
                    if (sector != null)
                    {
                        _entities.DeleteObject(sector);
                        _entities.SaveChanges();
                        lbSector.DataBind();
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

        protected void BtDeleteJob_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(hfSelectedJob.Value))
                {
                    var decryptedId = Crypto.Decrypt(hfSelectedJob.Value);
                    var id = Convert.ToInt32(decryptedId);
                    var job = _entities.Jobs.FirstOrDefault(p => p.Id == id);
                    if (job != null)
                    {
                        _entities.DeleteObject(job);
                        _entities.SaveChanges();
                        lbJob.DataBind();
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

        private void SaveUpdateSector(JobSectors sectors, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!SectorIsExist(tbNewSectorName.Text, -1))
                    {
                        var id = 0;
                        var lastSector = _entities.JobSectors.Take(1).OrderByDescending(p => p.Id).Single();
                        if (lastSector != null)
                        {
                            var lastId = lastSector.Id;
                            if (!string.IsNullOrWhiteSpace(lastId.ToString()))
                            {
                                id = Convert.ToInt32(lastId) + 1;
                            }
                        }
                        sectors.Id = id;
                        sectors.Name = tbNewSectorName.Text;
                        _entities.AddToJobSectors(sectors);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                                                                           tbNewSectorName.Text,
                                                                           AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!SectorIsExist(tbEditSectorName.Text, sectors.Id))
                    {
                        sectors.Name = tbEditSectorName.Text;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                                                                           tbEditSectorName.Text,
                                                                           AdminResource.AlreadyExistInSystem));
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }

            lbSector.DataBind();
        }

        private void SaveUpdateJob(Jobs jobs, Proccess proccess)
        {
            try
            {
                if (proccess == Proccess.Save)
                {
                    if (!JobIsExist(tbNewJobName.Text, -1))
                    {
                        var jobId = 0;
                        var lastJob = _entities.Jobs.Take(1).OrderByDescending(p => p.Id).Single();
                        if (lastJob != null)
                        {
                            var lastId = lastJob.Id;
                            if (!string.IsNullOrWhiteSpace(lastId.ToString()))
                            {
                                jobId = Convert.ToInt32(lastId) + 1;
                            }
                        }
                        jobs.Id = jobId;
                        jobs.Name = tbNewJobName.Text;
                        _entities.AddToJobs(jobs);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                                                                           tbNewJobName.Text,
                                                                           AdminResource.AlreadyExistInSystem));
                    }
                }
                else if (proccess == Proccess.Update)
                {
                    if (!JobIsExist(tbEditJobName.Text, jobs.Id))
                    {
                        jobs.Name = tbEditJobName.Text;
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, string.Format("{0} {1}",
                                                                           tbEditJobName.Text,
                                                                           AdminResource.AlreadyExistInSystem));
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
            }
            lbJob.DataBind();
        }

        #endregion
    }
}