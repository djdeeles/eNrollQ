using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;
using Image = System.Drawing.Image;

namespace eNroll
{

    public partial class NewMember : System.Web.UI.Page
    {
        public static MemberInfo _newMember = new MemberInfo();

        readonly Entities _entities = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(HttpContext.Current.Request.IsAuthenticated) Response.Redirect("Default.aspx");
            if (!IsPostBack)
            {
                ClearPersonalInfo();
                ClearHomeInfo();
                ClearJobInfo();
                ClearMembershipInfo();

                lbMaidenName.Text = Resource.lbMaidenName;
                lbMarriageDate.Text = Resource.lbMarriageDate;
                btnSavePersonalInfo.Text = Resource.lbNext;
                btnSaveHomeInfo.Text = Resource.lbNext;
                btnHomeInfoGoBack.Text = Resource.lbBack;
                btnSaveJobInfo.Text = Resource.lbSave;
                btnJobInfoGoBack.Text = Resource.lbBack;
                btnMemberInfoGoBack.Text = Resource.lbBack;
                btnSaveMemberInfo.Text = Resource.lbNext;

                mvCreateMember.SetActiveView(vPersonalDetail);
            }

            #region set page title
            string pageTitle = MetaGenerate.SetMetaTags(EnrollContext.Current.WorkingLanguage.LanguageId, Page);
            Page.Title = string.Format("{0} - {1}", pageTitle, Resource.lbNewMembership);
            #endregion
        }

        private bool SendEmailAccountActivation(int userId)
        { 
            var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
            if (user!=null)
            {
                var userEmail = _entities.UserEmails.FirstOrDefault(p => p.UserId== userId && p.MainAddress);
                try
                {
                    if (userEmail != null){
                        EnrollMembershipHelper.SendEmailActivationMail(user.Id, userEmail.Id,
                                                                       "/App_Themes/mainTheme/mailtemplates/MemberActivationMailContent.htm");
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    return false;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "vvvv",
                                                        "<script> alert('" + AdminResource.lbUserInformationNotValid +
                                                        ".');</script>");

            }
            return true;
        }
         
        #region generete password
        [WebMethod]
        public static string GeneratePassword()
        {
            var password = Guid.NewGuid().ToString("N");
            return password.Substring(0, 8);
        }
        #endregion

        #region clear form inputs
        private void ClearPersonalInfo()
        {
            tbName.Text = string.Empty;
            tbSurname.Text = string.Empty;
            tbEPosta.Text = string.Empty;
            tbParola.Text = string.Empty;
            tbTC.Text = string.Empty;
            EnrollMembershipHelper.DataBindDdlGender(_newMember, ddlGender);

            EnrollMembershipHelper.DataBindDdlMaritalStatus(_newMember, ddlMaritalStatus);
            tbMaidenName.Text = string.Empty;
            dpMarriageDate = null;
            trMaidenName.Attributes.Add("class", "hideaboutgender");
            trMarriageDate.Attributes.Add("class", "hideaboutgender");
            EnrollMembershipHelper.DataBindDDlBloodType(_newMember, ddlBloodType);
            dpBirthDate = null;
            tbBirthPlace.Text = string.Empty;
            tbGsmNo.Text = string.Empty;
            tbLastSchool.Text = string.Empty;
            dpLastSchoolGraduateDate.SelectedDate = null;
            tbMemberFoundation.Text = string.Empty;
            lbError.Text = string.Empty;
        }
        private void ClearHomeInfo()
        {
            EnrollMembershipHelper.BindDdlCountries(ddlHomeCountry, ddlHomeCity, ddlHomeTown, EnrollMembershipHelper.GetCountries());
            hfHomeCountry.Value = null;
            hfHomeCity.Value = null;
            hfHomeTown.Value = null;

            tbHomeAddress.Text = string.Empty;
            tbHomeZipCode.Text = string.Empty;
            tbHomePhone.Text = string.Empty;

        }
        private void ClearJobInfo()
        {
            EnrollMembershipHelper.BindDdlCountries(ddlWorkCountry, ddlWorkCity, ddlWorkTown, EnrollMembershipHelper.GetCountries());
            hfWorkCountry.Value = null;
            hfWorkCity.Value = null;
            hfWorkTown.Value = null;

            tbWorkAddress.Text = string.Empty;
            tbWorkZipCode.Text = string.Empty;
            tbWorkPhone.Text = string.Empty;
            tbWorkFax.Text = string.Empty;
            tbWorkTitle.Text = string.Empty;
            tbWorkCorparation.Text = string.Empty;

            EnrollMembershipHelper.DataBindDDlSectorsJobs(_newMember, ddlJobSectors, ddlJobs);
            ddlJobSectors.SelectedIndex = 0;
            ddlJobs.SelectedIndex = 0;
        }
        private void ClearMembershipInfo()
        {
            EnrollMembershipHelper.DataBindDdlMembershipRelType(ddlMembershipRelType);
            ddlMembershipRelType.SelectedIndex = 0;
            tbMembershipNumber.Text = string.Empty;
            cbIsTermLider.Checked = false;
            tbSpecialNumber.Text = string.Empty;
            dpTerm.SelectedDate = null;
            dpMembershipDate.SelectedDate = null;
        }
        #endregion

        #region OnSelectedIndexChanged countries cities towns
        
        //Home
        protected void ddlHomeCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfHomeCountry.Value = ddlHomeCountry.SelectedIndex > 0 ? ddlHomeCountry.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlCities(ddlHomeCity, ddlHomeTown, EnrollMembershipHelper.GetCities(hfHomeCountry.Value));
        }
        protected void ddlHomeCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfHomeCity.Value = ddlHomeCity.SelectedIndex > 0 ? ddlHomeCity.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlTowns(ddlHomeTown, EnrollMembershipHelper.GetTowns(hfHomeCountry.Value, hfHomeCity.Value));
        }
        
        //Work
        protected void ddlWorkCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCountry.Value = ddlWorkCountry.SelectedIndex > 0 ? ddlWorkCountry.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlCities(ddlWorkCity, ddlWorkTown, EnrollMembershipHelper.GetCities(hfWorkCountry.Value));
        }
        protected void ddlWorkCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCity.Value = ddlWorkCity.SelectedIndex > 0 ? ddlWorkCity.SelectedItem.Value : string.Empty;
            EnrollMembershipHelper.BindDdlTowns(ddlWorkTown, EnrollMembershipHelper.GetTowns(hfWorkCountry.Value, hfWorkCity.Value));
        }

        #endregion
        
        protected void BtnSavePersonalInfoClick(object sender, EventArgs e)
        {
            try
            {
                string email = tbEPosta.Text;
                List<Users> users = _entities.Users.Where(p => p.EMail == email).ToList();
                if (users.Count == 0 && tbParola.Text != string.Empty)
                {
                    _newMember.Users.EMail = email;
                    _newMember.Users.Name = tbName.Text;
                    _newMember.Users.Surname = tbSurname.Text;
                    _newMember.Users.Password = Crypto.Encrypt(tbParola.Text);
                    _newMember.Users.Admin = false;
                    _newMember.Users.State = false;
                    _newMember.Users.CreatedTime = DateTime.Now;
                    _newMember.Users.UpdatedTime = DateTime.Now;

                    _newMember.UserEmails.Email = tbEPosta.Text;
                    _newMember.UserEmails.MainAddress = true;
                    _newMember.UserEmails.Activated= false;
                    _newMember.UserEmails.AllowMailing = true;

                    if (ddlGender.SelectedIndex > 0)
                        _newMember.UserGeneral.Gender = Convert.ToInt32(ddlGender.SelectedItem.Value);
                    if (ddlMaritalStatus.SelectedIndex > 0)
                        _newMember.UserGeneral.MaritalStatus = Convert.ToInt32(ddlMaritalStatus.SelectedItem.Value);
                    if (ddlBloodType.SelectedIndex > 0)
                        _newMember.UserGeneral.BloodType = Convert.ToInt32(ddlBloodType.SelectedItem.Value);

                    _newMember.UserGeneral.TC = tbTC.Text;
                    _newMember.UserGeneral.MemberFoundation = tbMemberFoundation.Text;
                    _newMember.UserGeneral.FatherName = string.Empty;
                    _newMember.UserGeneral.MotherName = string.Empty;
                    _newMember.UserGeneral.MarriageDate = dpMarriageDate.SelectedDate;
                    _newMember.UserGeneral.MaidenName = tbMaidenName.Text;
                    _newMember.UserGeneral.Birthdate = dpBirthDate.SelectedDate;
                    _newMember.UserGeneral.Birthplace = tbBirthPlace.Text;
                    _newMember.UserGeneral.Hobby = string.Empty;
                    _newMember.UserGeneral.Web = string.Empty;
                    _newMember.UserGeneral.GsmNo = tbGsmNo.Text;
                    _newMember.UserGeneral.LastSchool = tbLastSchool.Text;

                    if (dpLastSchoolGraduateDate.SelectedDate != null)
                    {
                        _newMember.UserGeneral.LastSchoolGraduateDate = Convert.ToDateTime("01.01." + dpLastSchoolGraduateDate.SelectedDate.Value.Year.ToString());
                    }
                    _newMember.UserGeneral.PhotoUrl = null;
                    mvCreateMember.SetActiveView(vMemberInfo);

                }
                else if(email.Any())
                {
                    MessageBox.Show(MessageType.jAlert, Resource.lbMailAddressTakenAlready);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.jAlert, Resource.msgError);
            }

        }

        protected void BtnSaveMemberInfoClick(object sender, EventArgs e)
        {
            try
            {
                _newMember.UserFoundation.MemberState = false;
                if (!string.IsNullOrWhiteSpace(tbMembershipNumber.Text))
                    _newMember.UserFoundation.MemberNo = tbMembershipNumber.Text;
                if (!string.IsNullOrWhiteSpace(tbSpecialNumber.Text))
                    _newMember.UserFoundation.SpecialNo = tbSpecialNumber.Text;
                if (ddlMembershipRelType.SelectedIndex > 0)
                    _newMember.UserFoundation.MemberRelType = Convert.ToInt32(ddlMembershipRelType.SelectedItem.Value);
                if (dpMembershipDate != null && dpMembershipDate.SelectedDate != null)
                    _newMember.UserFoundation.MembershipDate = dpMembershipDate.SelectedDate;
                if (dpTerm.SelectedDate != null)
                    _newMember.UserFoundation.Term = dpTerm.SelectedDate.Value.Year.ToString();
                _newMember.UserFoundation.TermLeader = cbIsTermLider.Checked;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.jAlert, Resource.msgError);
            }

            mvCreateMember.SetActiveView(vHomeDetail);
        }

        protected void BtnSaveHomeInfoClick(object sender, EventArgs e)
        {
            try
            {
                _newMember.UserGeneral.HomePhone = tbHomePhone.Text;
                _newMember.UserGeneral.HomeAddress = tbHomeAddress.Text;
                if (ddlHomeCountry.SelectedIndex > 0)
                    _newMember.UserGeneral.HomeCountry = Convert.ToInt32(ddlHomeCountry.SelectedItem.Value);
                if (ddlHomeCity.SelectedIndex > 0)
                    _newMember.UserGeneral.HomeCity = Convert.ToInt32(ddlHomeCity.SelectedItem.Value);
                if (ddlHomeTown.SelectedIndex > 0)
                    _newMember.UserGeneral.HomeTown = Convert.ToInt32(ddlHomeTown.SelectedItem.Value);
                _newMember.UserGeneral.HomeZipCode = tbHomeZipCode.Text;

                mvCreateMember.SetActiveView(vJobDetail);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.jAlert, Resource.msgError);
                ltNewMemberResult.Text = Resource.msgError;
            }
        }

        protected void BtnSaveJobInfoClick(object sender, EventArgs e)
        {
            try
            {
                _newMember.UserGeneral.WorkPhone = tbWorkPhone.Text;
                _newMember.UserGeneral.WorkAddress = tbWorkAddress.Text;
                if (ddlWorkCountry.SelectedIndex > 0)
                    _newMember.UserGeneral.WorkCountry = Convert.ToInt32(ddlWorkCountry.SelectedItem.Value);
                if (ddlWorkCity.SelectedIndex > 0)
                    _newMember.UserGeneral.WorkCity = Convert.ToInt32(ddlWorkCity.SelectedItem.Value);
                if (ddlWorkTown.SelectedIndex > 0)
                    _newMember.UserGeneral.WorkTown = Convert.ToInt32(ddlWorkTown.SelectedItem.Value);
                _newMember.UserGeneral.WorkZipCode = tbWorkZipCode.Text;
                _newMember.UserGeneral.WorkFax = tbWorkFax.Text;
                if (!string.IsNullOrWhiteSpace(tbWorkCorparation.Text))
                {
                    _newMember.UserGeneral.WorkCorporation = tbWorkCorparation.Text;
                }
                _newMember.UserGeneral.WorkTitle = tbWorkTitle.Text;

                if (ddlJobs.SelectedIndex > 0)
                {
                    _newMember.UserGeneral.JobNo = Convert.ToInt32(ddlJobs.SelectedItem.Value);
                }
                if (ddlJobSectors.SelectedIndex > 0)
                {
                    _newMember.UserGeneral.JobSectorNo = Convert.ToInt32(ddlJobSectors.SelectedItem.Value);
                }

                _entities.AddToUsers(_newMember.Users);

                _newMember.UserGeneral.UserId = _newMember.Users.Id;
                _entities.AddToUserGeneral(_newMember.UserGeneral);

                _newMember.UserEmails.UserId = _newMember.Users.Id;
                _entities.AddToUserEmails(_newMember.UserEmails);

                _newMember.UserFoundation.UserId = _newMember.Users.Id;
                _entities.AddToUserFoundation(_newMember.UserFoundation);

                _newMember.UserFinance.UserId = _newMember.Users.Id;
                _entities.AddToUserFinance(_newMember.UserFinance);

                _entities.SaveChanges();


                if (SendEmailAccountActivation(_newMember.Users.Id))
                {
                    ltNewMemberResult.Text = Resource.msgMembershipCreated;
                }
                else
                {
                    MessageBox.Show(MessageType.jAlert, Resource.msgError);
                    return;
                }

                ClearPersonalInfo();
                ClearHomeInfo();
                ClearJobInfo();
                ClearMembershipInfo();

                mvCreateMember.SetActiveView(vCreateMemberResult);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.jAlert, Resource.msgError);
            }
        }

        protected void BtnMemberInfoGoBackClick(object sender, EventArgs e)
        {
            mvCreateMember.SetActiveView(vPersonalDetail);
        }

        protected void BtnHomeInfoGoBackClick(object sender, EventArgs e)
        {
            mvCreateMember.SetActiveView(vMemberInfo);
        }

        protected void BtnJobInfoGoBackClick(object sender, EventArgs e)
        {
            mvCreateMember.SetActiveView(vHomeDetail);
        }
    }
}