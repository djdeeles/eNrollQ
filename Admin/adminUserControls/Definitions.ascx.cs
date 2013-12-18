using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class Definitions : System.Web.UI.UserControl
    {
        public enum SelectedProccess
        {
            CorporationRelationType = 1,
            CountryCityTown = 2
        }
        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbDefinitions;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            mvAuthoriztn.SetActiveView(RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 30)
                                           ? vAuth
                                           : vNoAuth);

            #region resources
            btnCorporationRelType.Text = AdminResource.lbCorporationRelTypes;
            btnBloodType.Text = AdminResource.lbBloodType;
            btnCurrency.Text = AdminResource.lbCurrencyUnit;
            btnDuesType.Text = AdminResource.lbDuesType;
            btnServiceType.Text = AdminResource.lbServiceType;
            btnTaxType.Text = AdminResource.lbTaxType;
            btnCountryCityTown.Text = string.Format("{0}/{1}/{2}", AdminResource.lbCountry, AdminResource.lbCity, AdminResource.lbTown);
            btnSectorJob.Text = string.Format("{0}/{1}", AdminResource.lbJobSector, AdminResource.lbJob);

            #endregion
        }

        protected void BtnCorporationRelType_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefCorporationRelType);
        }

        protected void BtnCountryCityTown_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefCountryCityTown);
        }

        protected void BtnSectorJob_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefSectorJob);
        }

        protected void BtnBloodType_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefBloodType);
        }

        protected void BtnCurrency_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefCurrency);
        }

        protected void BtnDuesType_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefDues);
        }

        protected void BtnServiceType_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefServiceType);
        }

        protected void BtnTaxType_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefTax);
        }
    }
}