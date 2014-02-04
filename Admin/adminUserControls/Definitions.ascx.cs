using System;
using System.Web;
using System.Web.UI;
using Resources;

namespace eNroll.Admin.adminUserControls
{
    public partial class Definitions : UserControl
    {
        #region SelectedProccess enum

        public enum SelectedProccess
        {
            CorporationRelationType = 1,
            CountryCityTown = 2
        }

        #endregion

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
            btnTaxType.Text = AdminResource.lbTaxType;
            btnCountryCityTown.Text = string.Format("{0}/{1}/{2}", AdminResource.lbCountry, AdminResource.lbCity,
                                                    AdminResource.lbTown);
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

        protected void BtnTaxType_OnClick(object sender, EventArgs e)
        {
            mvDefinations.SetActiveView(vDefTax);
        }
    }
}