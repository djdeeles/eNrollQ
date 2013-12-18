using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

namespace eNroll.Admin
{
    public partial class FinanceViewer : System.Web.UI.Page
    {

        Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckCulture();
            try
            {
                var userId = 0;
                var corporationId = 0;
                if (HttpContext.Current.Request.QueryString.Count > 0)
                {
                    if (HttpContext.Current.Request.QueryString["type"] != null &&
                        HttpContext.Current.Request.QueryString["data"] != null &&
                        (HttpContext.Current.Request.QueryString["process"] != null))
                    {
                        if (HttpContext.Current.Request.QueryString["userId"] != null)
                            userId = Convert.ToInt32(HttpContext.Current.Request.QueryString["userId"]);
                        if (HttpContext.Current.Request.QueryString["corporationId"] != null)
                            corporationId = Convert.ToInt32(HttpContext.Current.Request.QueryString["corporationId"]);
                        var data = Convert.ToInt32(HttpContext.Current.Request.QueryString["data"]);
                        var process = HttpContext.Current.Request.QueryString["process"];
                        var type = HttpContext.Current.Request.QueryString["type"];

                        switch (type)
                        {
                            case "0": //üye
                                switch (process)
                                {
                                    case "1":
                                        ShowPaymentDetail(userId, data);
                                        break;
                                    case "0":
                                        ShowDuesDetail(userId, data);
                                        break;
                                }
                                break;
                            case "1": //kurum
                                switch (process)
                                {
                                    case "1":
                                        ShowCorporationPaymentDetail(corporationId, data);
                                        break;
                                    case "0":
                                        ShowCorporationDuesDetail(corporationId, data);
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        mvFinance.SetActiveView(vNotFound);
                    }
                }
                else
                {
                    mvFinance.SetActiveView(vNotFound);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public void ShowPaymentDetail(int userId, int data)
        {
            if (userId > 0 && data > 0)
            {
                var userDuesLog = _entities.UserDuesLog.FirstOrDefault(p => p.Id == data && p.UserId == userId);
                if (userDuesLog != null)
                {
                    var duesPaymentType = _entities.DuesPaymentTypes.FirstOrDefault(x => x.Id == userDuesLog.PaymentTypeId);
                    var user = _entities.Users.FirstOrDefault(x => x.Id == userDuesLog.UserId);

                    if (duesPaymentType != null) lbPaymentType.Text = GetPaymentType(duesPaymentType.Id);
                    if (user != null) lbProcessUser.Text = user.Name + " " + user.Surname;
                    if (userDuesLog.ReceiptDate != null)
                        lbReceiptDate.Text = userDuesLog.ReceiptDate.Value.ToShortDateString() + " " +
                            userDuesLog.ReceiptDate.Value.ToShortTimeString();

                    lbProcessDate.Text = userDuesLog.CreatedTime.ToShortDateString() + " " + userDuesLog.CreatedTime.ToShortTimeString();
                    lbAmount.Text = string.Format("{0}{1}{2}",
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""),
                                                            userDuesLog.Amount.ToString("0.00"),
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""));
                    lbReceiptNumber.Text = userDuesLog.ReceiptNo;

                }
                mvFinance.SetActiveView(vPaymentDetail);
            }
            else
            {
                mvFinance.SetActiveView(vNotFound);
            }
        }

        public void ShowCorporationPaymentDetail(int corporationId, int data)
        {
            if (corporationId > 0 && data > 0)
            {
                var corporationLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == data && p.CorporationId == corporationId);
                if (corporationLog != null)
                {
                    var duesPaymentType = _entities.DuesPaymentTypes.FirstOrDefault(x => x.Id == corporationLog.PaymentTypeId);
                    var user = _entities.Users.FirstOrDefault(x => x.Id == corporationLog.CreatedUser);

                    if (duesPaymentType != null) lbCorporationPaymentType.Text = GetPaymentType(duesPaymentType.Id);
                    if (user != null) lbCorporationProcessUser.Text = user.Name + " " + user.Surname;
                    //if (corporationLog.ReceiptDate != null)
                    //    lbCorporationReceiptDate.Text = corporationLog.ReceiptDate.Value.ToShortDateString() + " " +
                    //        corporationLog.ReceiptDate.Value.ToShortTimeString();

                    lbCorporationProcessDate.Text = corporationLog.CreatedTime.ToShortDateString() + " " + corporationLog.CreatedTime.ToShortTimeString();
                    lbCorporationAmount.Text = string.Format("{0}{1}{2}",
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""),
                                                            corporationLog.Amount.ToString("0.00"),
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""));
                    //lbCorporationReceiptNumber.Text = corporationLog.ReceiptNo;
                    lbCorporationDesc.Text = corporationLog.Note;
                }
                mvFinance.SetActiveView(vCorporationPaymentDetail);
            }
            else
            {
                mvFinance.SetActiveView(vNotFound);
            }
        }

        private string GetPaymentType(int id)
        {
            try
            {
                switch (id)
                {
                    case 1: return AdminResource.lbPaymentType1;
                    case 2: return AdminResource.lbPaymentType2;
                    case 3: return AdminResource.lbPaymentType3;
                    case 4: return AdminResource.lbPaymentType4;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return string.Empty;
        }

        public void ShowDuesDetail(int userId, int data)
        {
            if (userId > 0 && data > 0)
            {
                var userDuesLog = _entities.UserDuesLog.FirstOrDefault(p => p.Id == data && p.UserId == userId);
                if (userDuesLog != null)
                {
                    var duesTypes = _entities.DuesTypes.FirstOrDefault(x => x.Id == userDuesLog.DuesType);
                    var user = _entities.Users.FirstOrDefault(x => x.Id == userDuesLog.UserId);

                    if (duesTypes != null) lbDuesType.Text = duesTypes.Title;
                    if (user != null) lbDuesProcessUser.Text = user.Name + " " + user.Surname;
                    lbDuesProcessDate.Text = userDuesLog.CreatedTime.ToShortDateString() + " " + userDuesLog.CreatedTime.ToShortTimeString();
                    lbDuesAmount.Text = string.Format("{0}{1}{2}",
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""),
                                                            userDuesLog.Amount.ToString("0.00"),
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""));
                }
                mvFinance.SetActiveView(vDuesDetail);
            }
            else
            {
                mvFinance.SetActiveView(vNotFound);
            }
        }

        public void ShowCorporationDuesDetail(int corporationId, int data)
        {
            if (corporationId > 0 && data > 0)
            {
                var corporationDuesLog = _entities.CorporationDuesLog.FirstOrDefault(p => p.Id == data && p.CorporationId == corporationId);
                if (corporationDuesLog != null)
                {
                    var user = _entities.Users.FirstOrDefault(x => x.Id == corporationDuesLog.CreatedUser);

                    if (user != null) lbCorporationDuesProcessUser.Text = user.Name + " " + user.Surname;
                    lbCorporationDuesProcessDate.Text = corporationDuesLog.CreatedTime.ToShortDateString() + " " + corporationDuesLog.CreatedTime.ToShortTimeString();
                    lbCorporationDuesService.Text = corporationDuesLog.Service;
                    lbCorporationDuesDesc.Text = corporationDuesLog.Note;
                    if (corporationDuesLog.ReceiptNo != null)
                        lbCorporationReceiptNo.Text = corporationDuesLog.ReceiptNo;
                    else lbCorporationReceiptNo.Text = AdminResource.lbNotInvoiced;
                    lbCorporationReceiptDate.Text = ((corporationDuesLog.ReceiptDate != null &&
                                                      corporationDuesLog.ReceiptDate.Value != null)
                        ? Convert.ToDateTime(corporationDuesLog.ReceiptDate).ToString("dd/MM/yyyy")
                        : string.Empty);
                    lbCorporationDuesAmount.Text = string.Format("{0}{1}{2}",
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""),
                                                            corporationDuesLog.Amount.ToString("0.00"),
                                                    (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                            ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                            : ""));
                    lbCorporationDuesDesc.Text = corporationDuesLog.Note;
                }
                mvFinance.SetActiveView(vCorporationDuesDetail);
            }
            else
            {
                mvFinance.SetActiveView(vNotFound);
            }
        }

        #region set page culture
        public void CheckCulture()
        {
            var ent = new EnrollAdminContext();
            var entities = new Entities();
            var lang = ent.AdminLanguage.LanguageId;
            var system = entities.System_language.FirstOrDefault(p => p.languageId == lang);
            if (system != null)
            {
                var cultureName = system.languageCulture;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            }
        }
        #endregion

        #region calculate DebtWithTask
        [WebMethod]
        public static string CalculateDeptWithTax(string debt, string tax)
        {
            
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(debt) && !string.IsNullOrEmpty(tax))
            {
                var taxAmount = Convert.ToDecimal(tax);
                var debtAmount = Convert.ToDecimal(debt);
                var deptWithTax = (debtAmount + debtAmount * taxAmount).ToString("0.00");

                var symbolLeft = (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                    ? EnrollCurrency.SiteDefaultCurrencyUnit() + " "
                    : "");
                var symbolRight = (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                    ? " " + EnrollCurrency.SiteDefaultCurrencyUnit()
                    : "");

                sb.AppendFormat("{0}{1}{2}", symbolLeft, deptWithTax, symbolRight);
                
                var ret = new string[2];
                ret[0] = deptWithTax;
                ret[1] = sb.ToString();
                return ret[1];
            }
            return string.Empty;
        }
        #endregion

    }
}