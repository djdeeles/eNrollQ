using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;
using eNroll.Helpers;
using Enroll.Managers;
using Resources;

namespace eNroll.Admin.adminUserControls
{
    public partial class CorporationInvoicement : System.Web.UI.UserControl
    {

        Entities _entities = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            btInvoiceService.Text = AdminResource.lbInvoice;
            btInvoiceServiceCancel.Text = AdminResource.lbCancel;
        }

        protected void btInvoiceService_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hfCorporationLogId.Value))
                {
                    var corporationLogId = Convert.ToInt32(hfCorporationLogId.Value);
                    var corporationLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == corporationLogId);
                    if (corporationLog != null)
                    {
                        var beforeSavedReceipt = _entities.CorporationDuesLog.FirstOrDefault(p => p.ReceiptNo == tbReceiptNo.Text);
                        if (beforeSavedReceipt != null)
                        {
                            if (beforeSavedReceipt.ReceiptDate != dpReceiptInvoiceDate.SelectedDate)
                            {
                                MessageBox.Show(MessageType.jAlert,
                                    string.Format("{0} numaralı faturanın tarihi sistemde {1} olarak kayıtlı. Lütfen bilgileri kontrol ediniz.",
                                                  beforeSavedReceipt.ReceiptNo, Convert.ToDateTime(beforeSavedReceipt.ReceiptDate).ToShortDateString()));
                            }
                            else
                            {
                                if (!ReceiptService(corporationLog)) return;
                            }
                        }
                        else
                        {
                            if (!ReceiptService(corporationLog)) return;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
            }
        }

        private bool ReceiptService(CorporationDuesLog log)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbReceiptNo.Text) || dpReceiptInvoiceDate.SelectedDate == null) return false;
                log.ReceiptDate = dpReceiptInvoiceDate.SelectedDate;
                if (tbDesc.Text != string.Empty) log.Note += " - " + tbDesc.Text;
                log.ReceiptNo = tbReceiptNo.Text;
                log.IsInvoiced = true;

                var generalDept = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == log.CorporationId);
                if (generalDept != null)
                {
                    #region sorgu sonucu ile gelen kurumun genel borcu güncellenir
                    decimal newDept = 0;

                    var corporationFinance = _entities.CorporationFinance.FirstOrDefault(p => p.CorporationId == log.CorporationId);
                    if (corporationFinance != null)
                    {
                        corporationFinance.Dept += log.Amount;
                        newDept = corporationFinance.Dept;
                    }
                    else
                    {
                        corporationFinance = new CorporationFinance();
                        corporationFinance.CorporationId = log.CorporationId;
                        corporationFinance.Dept = newDept;
                        _entities.AddToCorporationFinance(corporationFinance);
                    }
                    #endregion

                    log.IsInvoiced = true;
                    _entities.SaveChanges();

                    //Faturalandırma,
                    Logger.Add(33, 3, log.Id, 1);

                    MessageBox.Show(MessageType.Success, AdminResource.msgServiceInvoicedSuccesfully);
                    ClearFormInputs();

                    var _pServiceInvoicing = (Panel)Parent.FindControl("pServiceInvoicing");
                    _pServiceInvoicing.Visible = false;

                    var _gvChargesForDues = (GridView)Parent.FindControl("gvChargesForDues");
                    if (_gvChargesForDues != null) _gvChargesForDues.DataBind();

                    var _lbFDDeptAmount = (Label)Parent.FindControl("lbFDDeptAmount");
                    if (_lbFDDeptAmount != null) _lbFDDeptAmount.Text = newDept.ToString("0.00");
                    return true;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

            return false;
        }

        protected void btInvoiceServiceCancel_OnClick(object sender, EventArgs e)
        {
            hfCorporationLogId.Value = null;
            ClearFormInputs();
            var _pServiceInvoicing = (Panel)Parent.FindControl("pServiceInvoicing");
            _pServiceInvoicing.Visible = false;
        }
        public void ClearFormInputs()
        {
            tbDesc.Text = string.Empty;
            tbReceiptNo.Text = string.Empty;
            dpReceiptInvoiceDate.SelectedDate = null;
        }
    }
}