using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;
using System.Drawing;
using Image = System.Drawing.Image;
using System.Web.Configuration;

namespace eNroll
{
    public partial class MemberProfile : System.Web.UI.Page
    {
        public static MemberInfo Member = new MemberInfo();
        readonly Entities _entities = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(),
                        "hideAllUserInfo11", "<script> hideAllUserInfo(); </script>");

                //login panelinden aidat linki ile 
                if (HttpContext.Current.Request.QueryString.Count > 0 && HttpContext.Current.Request.QueryString["show"] != null)
                {
                    var show = HttpContext.Current.Request.QueryString["show"];
                    ShowControlPanel(show);
                }
                #region Bind User and User Pers/Home/Job Info
                Member = EnrollMembershipHelper.BindUser(Member);
                var userId = Member.Users.Id;
                hfMemberId.Value = userId.ToString();
                BindPersonalInfo();
                BindHomeInfo();
                BindJobInfo();
                BindEmailInfo();
                BindMemberInfo();
                #endregion
                #region label/button set resource text
                lbMaidenName.Text = Resource.lbMaidenName;
                lbMarriageDate.Text = Resource.lbMarriageDate;
                btnSavePersonalInfo.Text = Resource.lbSave;
                btnSaveHomeInfo.Text = Resource.lbSave;
                btnSaveWorkInfo.Text = Resource.lbSave;
                btnEditPersonalInfo.Text = Resource.lbEdit;
                btnEditHomeInfo.Text = Resource.lbEdit;
                btnEditWorkInfo.Text = Resource.lbEdit;
                btnViewPersonalInfo.Text = Resource.lbCancel;
                btnViewHomeInfo.Text = Resource.lbCancel;
                btnViewWorkInfo.Text = Resource.lbCancel;

                btnViewEmailInfo.Text = Resource.lbCancel;
                btnEditEmailInfo.Text = Resource.lbEdit;
                btnSaveEmailInfo.Text = Resource.lbSave;

                btPayCertainAmount.Text = Resource.lbPay;
                btPayCertainAmount.ToolTip = Resource.lbWillPayYourSpecificAmount;
                btPayAllAmount.Text = Resource.lbPayAllAmount;
                btPayAllAmount.ToolTip = Resource.lbWillPayYourAllAmount;

                var genrlDept = _entities.UserFinance.FirstOrDefault(p => p.UserId == userId);


                if (genrlDept != null)
                {
                    ltCurrentDebt.Text = EnrollMembershipHelper.DebtValue(genrlDept.Dept);
                    ltAutoPaymentOrder.Text = Resource.lbAutoPaymentOrder + ": " + (genrlDept.AutoPay ? Resource.lbYes : Resource.lbNo);
                    if(genrlDept.AutoPay)
                    {
                        btPayCertainAmount.OnClientClick =
                            "if(document.getElementById('" + tbSpecificAmount.ClientID+ "').value=='' || " +
                            "document.getElementById('" + tbSpecificAmount.ClientID + "').value==undefined){" +
                                "alert('" + Resource.msgPayAmountIsRequired + "');" +
                                "return false; " +
                            "} " +
                            "else{ " +
                                "return confirm ('" + Resource.msgConfirmAutoPaymentOrderRemember + "');  " +
                            "} "; 
                        btPayAllAmount.OnClientClick =
                            "if(document.getElementById('" + tbSpecificAmount.ClientID + "').value=='' || " +
                            "document.getElementById('" + tbSpecificAmount.ClientID + "').value==undefined){" +
                                "alert('" + Resource.msgPayAmountIsRequired + "');" +
                                "return false; " +
                            "} " +
                            "else{ " +
                                "return confirm ('" + Resource.msgConfirmAutoPaymentOrderRemember + "');  " +
                            "} "; 
                    }
                }

                gvChargesForDues.Columns[0].HeaderText = Resource.lbDuesType;
                gvChargesForDues.Columns[1].HeaderText = Resource.lbPaymentType;
                gvChargesForDues.Columns[2].HeaderText = Resource.lbAmount;
                gvChargesForDues.Columns[3].HeaderText = Resource.lbProcessDate;
                gvChargesForDues.Columns[4].HeaderText = Resource.lbProcess;

                #endregion

            }
            ShowPersInfoViewMode();
            ShowMemberInfoViewMode();
            ShowHomeInfoViewMode();
            ShowWorkInfoViewMode();
            ShowEmailInfoViewMode();

            #region set page title
            string pageTitle = MetaGenerate.SetMetaTags(EnrollContext.Current.WorkingLanguage.LanguageId, Page);
            Page.Title = string.Format("{0} - {1}", pageTitle, Resource.lbMemberProfile);
            #endregion
        }

        private void ShowControlPanel(string control)
        {
            //login panelinden aidat linki ile 
            try
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "asdfg", "<script> showUserInfo('" + control + "'); </script>");
            }
            catch (Exception)
            {

            }
        }

        #region Bind PersonalInfo, HomeInfo, JobInfo, EmailInfo
        private void BindPersonalInfo()
        {
            try
            {
                //önce alanlar temizlenir
                ClearPersonalInfo();

                #region user general info
                lUserNameSurname.Text = string.Format("{0} {1}", Member.Users.Name, Member.Users.Surname);
                lUserEmail.Text = Member.Users.EMail;
                if (!string.IsNullOrWhiteSpace(Member.UserGeneral.PhotoUrl))
                    lUserPhoto.ImageUrl = Member.UserGeneral.PhotoUrl;
                else
                    lUserPhoto.ImageUrl = "/App_Themes/mainTheme/images/noimage.png";
                #endregion

                tbName.Text = Member.Users.Name;
                tbSurname.Text = Member.Users.Surname;

                if (Member.UserGeneral != null)
                {
                    tbTC.Text = Member.UserGeneral.TC ?? string.Empty;
                    tbFatherName.Text = Member.UserGeneral.FatherName;
                    lFatherName.Text = Member.UserGeneral.FatherName;
                    tbMotherName.Text = Member.UserGeneral.MotherName;
                    lFatherName.Text = Member.UserGeneral.MotherName;

                    #region cinsiyet
                    EnrollMembershipHelper.DataBindDdlGender(Member, ddlGender);
                    if (Member.UserGeneral.Gender != null && Convert.ToInt32(Member.UserGeneral.Gender.Value) > 0)
                    {
                        switch (Member.UserGeneral.Gender.Value)
                        {
                            case 1:
                                lGender.Text = Resource.lbMan;
                                break;
                            case 2:
                                lGender.Text = Resource.lbWoman;
                                break;
                        }
                    }
                    else
                    {
                        lGender.Text = string.Empty;
                    }
                    #endregion

                    #region evlilik durumuna göre kızlık soyadı evlilik tarihi gibi alanlar visible/invisible yapılarak alanlar doldurulur

                    EnrollMembershipHelper.DataBindDdlMaritalStatus(Member, ddlMaritalStatus);
                    if (Member.UserGeneral.MaritalStatus != null)
                    {
                        switch (Member.UserGeneral.MaritalStatus.Value)
                        {
                            case 1:
                                lMaritalStatus.Text = Resource.lbMarried;
                                if (Member.UserGeneral.MaidenName != null)
                                {
                                    lMaidenName.Text = Member.UserGeneral.MaidenName;
                                    tbMaidenName.Text = Member.UserGeneral.MaidenName;
                                    trMaidenName.Style.Remove("class");
                                }
                                if (Member.UserGeneral.MarriageDate != null)
                                {
                                    lMarriageDate.Text = Member.UserGeneral.MarriageDate.Value.ToString();
                                    dpMarriageDate.SelectedDate = Member.UserGeneral.MarriageDate.Value;
                                    trMarriageDate.Style.Remove("class");
                                }
                                break;
                            case 2:
                                lMaritalStatus.Text = Resource.lbSingle;
                                break;
                        }
                    }
                    else
                    {
                        lMaidenName.Text = string.Empty;
                        lMarriageDate.Text = string.Empty;
                        lMaritalStatus.Text = string.Empty;

                        trMaidenName.Attributes.Add("class", "hideaboutgender");
                        trMarriageDate.Attributes.Add("class", "hideaboutgender");
                    }
                    #endregion

                    #region doğum günü, doğum yeri, profil fotoğrafı
                    if (Member.UserGeneral.Birthdate != null)
                    {
                        var date = Member.UserGeneral.Birthdate.Value;
                        lBirthDate.Text = date.ToShortDateString();
                        dpBirthDate.SelectedDate = date;
                    }
                    else
                    {
                        lBirthDate.Text = string.Empty;
                        dpBirthDate.SelectedDate = null;
                    }

                    lBirthPlace.Text = Member.UserGeneral.Birthplace;
                    tbBirthPlace.Text = Member.UserGeneral.Birthplace;

                    if (!string.IsNullOrWhiteSpace(Member.UserGeneral.PhotoUrl))
                    {
                        hdnUserProfilPhotoUrl.Value = Member.UserGeneral.PhotoUrl;
                    }
                    else
                    {
                        hdnUserProfilPhotoUrl.Value = string.Empty;
                    }

                    #endregion

                    lHobbies.Text = Member.UserGeneral.Hobby;
                    tbHobbies.Text = Member.UserGeneral.Hobby;
                    lWeb.Text = Member.UserGeneral.Web;
                    tbWeb.Text = Member.UserGeneral.Web;
                    lGsmNo.Text = Member.UserGeneral.GsmNo;
                    tbGsmNo.Text = Member.UserGeneral.GsmNo;

                    tbMemberFoundation.Text = Member.UserGeneral.MemberFoundation;
                    lMemberFoundation.Text = Member.UserGeneral.MemberFoundation;

                    #region okul bilgileri
                    lLastSchool.Text = Member.UserGeneral.LastSchool;
                    tbLastSchool.Text = Member.UserGeneral.LastSchool;
                    if (Member.UserGeneral.LastSchoolGraduateDate != null)
                    {
                        lLastSchoolGraduateDate.Text = Member.UserGeneral.LastSchoolGraduateDate.Value.Year.ToString();
                        dpLastSchoolGraduateDate.SelectedDate = Member.UserGeneral.LastSchoolGraduateDate.Value;
                    }
                    else
                    {
                        lLastSchoolGraduateDate.Text = string.Empty;
                        dpLastSchoolGraduateDate.SelectedDate = null;
                    }
                    #endregion

                }

                EnrollMembershipHelper.DataBindDDlBloodType(Member, ddlBloodType);
                if (ddlBloodType.SelectedIndex > 0)
                {
                    int selected = Convert.ToInt32(ddlBloodType.SelectedItem.Value);
                    lBloodType.Text = _entities.BloodTypes.First(p => p.Id == selected).Name;
                }
                else lBloodType.Text = string.Empty;

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            lbError.Text = string.Empty;
        }
        private void BindHomeInfo()
        {
            try
            {
                ClearHomeInfo();

                Member = BindHomeCountryCityTown(Member, ddlHomeCountry, ddlHomeCity, ddlHomeTown);


                if (Member.UserGeneral.HomeCountry != null && !string.IsNullOrWhiteSpace(Member.UserGeneral.HomeCountry.Value.ToString()))
                {
                    hfHomeCountry.Value = Member.UserGeneral.HomeCountry.Value.ToString();
                    lHomeCountry.Text = _entities.Countries.First(p => p.Id == Member.UserGeneral.HomeCountry.Value).Name;
                }
                else
                {
                    hfHomeCountry.Value = string.Empty;
                    lHomeCountry.Text = string.Empty;
                }
                if (Member.UserGeneral.HomeCity != null && !string.IsNullOrWhiteSpace(Member.UserGeneral.HomeCity.Value.ToString()))
                {
                    hfHomeCity.Value = Member.UserGeneral.HomeCity.Value.ToString();
                    lHomeCity.Text = _entities.Cities.First(p => p.Id == Member.UserGeneral.HomeCity.Value).Name;
                }
                else
                {
                    hfHomeCity.Value = string.Empty;
                    lHomeCity.Text = string.Empty;
                }
                if (Member.UserGeneral.HomeTown != null && !string.IsNullOrWhiteSpace(Member.UserGeneral.HomeTown.Value.ToString()))
                {
                    hfHomeTown.Value = Member.UserGeneral.HomeTown.Value.ToString();
                    lHomeTown.Text = _entities.Towns.First(p => p.Id == Member.UserGeneral.HomeTown.Value).Name;
                }
                else
                {
                    hfHomeTown.Value = string.Empty;
                    lHomeTown.Text = string.Empty;
                }


                cbViewHidePersonalInfo.Checked = Member.UserGeneral.HidePersonalInfo;
                cbHidePersonalInfo.Checked = Member.UserGeneral.HidePersonalInfo;

                lHomeAddress.Text = Member.UserGeneral.HomeAddress;
                tbHomeAddress.Text = Member.UserGeneral.HomeAddress;
                lHomePhone.Text = Member.UserGeneral.HomePhone;
                tbHomePhone.Text = Member.UserGeneral.HomePhone;
                lHomeZipCode.Text = Member.UserGeneral.HomeZipCode;
                tbHomeZipCode.Text = Member.UserGeneral.HomeZipCode;

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        private void BindJobInfo()
        {
            try
            {
                ClearJobInfo();


                Member = BindWorkCountryCityTown(Member, ddlWorkCountry, ddlWorkCity, ddlWorkTown);

                if (Member.UserGeneral.WorkCountry != null && !string.IsNullOrWhiteSpace(Member.UserGeneral.WorkCountry.Value.ToString()))
                {
                    hfWorkCountry.Value = Member.UserGeneral.WorkCountry.Value.ToString();
                    lWorkCountry.Text = _entities.Countries.First(p => p.Id == Member.UserGeneral.WorkCountry.Value).Name;
                }
                else
                {
                    hfWorkCountry.Value = string.Empty;
                    lWorkCountry.Text = string.Empty;
                }
                if (Member.UserGeneral.WorkCity != null && !string.IsNullOrWhiteSpace(Member.UserGeneral.WorkCity.Value.ToString()))
                {
                    hfWorkCity.Value = Member.UserGeneral.WorkCity.Value.ToString();
                    lWorkCity.Text = _entities.Cities.First(p => p.Id == Member.UserGeneral.WorkCity.Value).Name;
                }
                else
                {
                    hfWorkCity.Value = string.Empty;
                    lWorkCity.Text = string.Empty;
                }
                if (Member.UserGeneral.WorkTown != null && !string.IsNullOrWhiteSpace(Member.UserGeneral.WorkTown.Value.ToString()))
                {
                    hfWorkTown.Value = Member.UserGeneral.WorkTown.Value.ToString();
                    lWorkTown.Text = _entities.Towns.First(p => p.Id == Member.UserGeneral.WorkTown.Value).Name;
                }
                else
                {
                    hfWorkTown.Value = string.Empty;
                    lWorkTown.Text = string.Empty;
                }


                lWorkAddress.Text = Member.UserGeneral.WorkAddress;
                tbWorkAddress.Text = Member.UserGeneral.WorkAddress;
                lWorkPhone.Text = Member.UserGeneral.WorkPhone;
                tbWorkPhone.Text = Member.UserGeneral.WorkPhone;
                lWorkZipCode.Text = Member.UserGeneral.WorkZipCode;
                tbWorkZipCode.Text = Member.UserGeneral.WorkZipCode;
                lWorkFax.Text = Member.UserGeneral.WorkFax;
                tbWorkFax.Text = Member.UserGeneral.WorkFax;
                lWorkTitle.Text = Member.UserGeneral.WorkTitle;
                tbWorkTitle.Text = Member.UserGeneral.WorkTitle;
                lWorkCorparation.Text = Member.UserGeneral.WorkCorporation;
                tbWorkCorparation.Text = Member.UserGeneral.WorkCorporation;

                EnrollMembershipHelper.DataBindDDlSectorsJobs(Member, ddlJobSectors, ddlJobs);
                lJobSectors.Text = Member.UserGeneral.JobSectorNo != null && Member.UserGeneral.JobSectorNo > 0
                                       ? _entities.JobSectors.First(p => p.Id == Member.UserGeneral.JobSectorNo).Name
                                       : string.Empty;
                lJobs.Text = Member.UserGeneral.JobNo != null && Member.UserGeneral.JobNo > 0
                                       ? _entities.Jobs.First(p => p.Id == Member.UserGeneral.JobNo).Name
                                       : string.Empty;
                cbViewHideJobInfo.Checked = Member.UserGeneral.HideJobInfo;
                cbHideJobInfo.Checked = Member.UserGeneral.HideJobInfo;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

        }

        private MemberInfo BindWorkCountryCityTown(MemberInfo memberInfo, DropDownList ddlCountry, DropDownList ddlCity, DropDownList ddlTown)
        {

            BindDdlCountries(ddlCountry, ddlCity, ddlTown, EnrollMembershipHelper.GetCountries());

            if (memberInfo.UserGeneral.WorkCountry != null)
            {
                for (var i = 0; i < ddlCountry.Items.Count; i++)
                {
                    if (ddlCountry.Items[i].Value == memberInfo.UserGeneral.WorkCountry.Value.ToString())
                    {
                        ddlCountry.SelectedIndex = i;
                        break;
                    }
                }
                BindDdlCities(ddlCity, ddlTown, EnrollMembershipHelper.GetCities(memberInfo.UserGeneral.WorkCountry.Value.ToString()));
                if (memberInfo.UserGeneral.WorkCity != null)
                {
                    for (var i = 0; i < ddlCity.Items.Count; i++)
                    {
                        if (ddlCity.Items[i].Value == memberInfo.UserGeneral.WorkCity.Value.ToString())
                        {
                            ddlCity.SelectedIndex = i;
                            break;
                        }
                    }

                    BindDdlTowns(ddlTown, EnrollMembershipHelper.GetTowns(memberInfo.UserGeneral.WorkCountry.Value.ToString(),
                        memberInfo.UserGeneral.WorkCity.Value.ToString()));
                    if (memberInfo.UserGeneral.WorkTown != null)
                    {
                        for (var i = 0; i < ddlTown.Items.Count; i++)
                        {
                            if (ddlTown.Items[i].Value == memberInfo.UserGeneral.WorkTown.Value.ToString())
                            {
                                ddlTown.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            return memberInfo;
        }

        private MemberInfo BindHomeCountryCityTown(MemberInfo memberInfo, DropDownList ddlCountry, DropDownList ddlCity, DropDownList ddlTown)
        {

            BindDdlCountries(ddlCountry, ddlCity, ddlTown, EnrollMembershipHelper.GetCountries());

            if (memberInfo.UserGeneral.HomeCountry != null)
            {
                for (var i = 0; i < ddlCountry.Items.Count; i++)
                {
                    if (ddlCountry.Items[i].Value == memberInfo.UserGeneral.HomeCountry.Value.ToString())
                    {
                        ddlCountry.SelectedIndex = i;
                        break;
                    }
                }
                BindDdlCities(ddlCity, ddlTown, EnrollMembershipHelper.GetCities(memberInfo.UserGeneral.HomeCountry.Value.ToString()));
                if (memberInfo.UserGeneral.HomeCity != null)
                {
                    for (var i = 0; i < ddlCity.Items.Count; i++)
                    {
                        if (ddlCity.Items[i].Value == memberInfo.UserGeneral.HomeCity.Value.ToString())
                        {
                            ddlCity.SelectedIndex = i;
                            break;
                        }
                    }

                    BindDdlTowns(ddlTown, EnrollMembershipHelper.GetTowns(memberInfo.UserGeneral.HomeCountry.Value.ToString(),
                        memberInfo.UserGeneral.HomeCity.Value.ToString()));
                    if (memberInfo.UserGeneral.HomeTown != null)
                    {
                        for (var i = 0; i < ddlTown.Items.Count; i++)
                        {
                            if (ddlTown.Items[i].Value == memberInfo.UserGeneral.HomeTown.Value.ToString())
                            {
                                ddlTown.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            return memberInfo;
        }

        private void BindMemberInfo()
        {
            try
            {
                ClearMembershipInfo();

                lMembershipNumber.Text = Member.UserFoundation.MemberNo;
                lSpecialNumber.Text = Member.UserFoundation.SpecialNo;

                if (Member.UserFoundation.MemberRelType > 0)
                {
                    var foundationRelType = _entities.FoundationRelType.FirstOrDefault(p => p.Id == Member.UserFoundation.MemberRelType);
                    lMemberRelationType.Text = foundationRelType != null ? foundationRelType.Name : string.Empty;
                }
                else
                    lMemberRelationType.Text = string.Empty;

                if (Member.UserFoundation.MembershipDate != null && Member.UserFoundation.MembershipDate.Value != null)
                    lMembershipDate.Text = Member.UserFoundation.MembershipDate.Value.ToShortDateString();
                else
                    lMembershipDate.Text = string.Empty;

                if (!string.IsNullOrWhiteSpace(Member.UserFoundation.Term))
                    lTerm.Text = Member.UserFoundation.Term;
                else
                    lTerm.Text = string.Empty;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        private void BindEmailInfo()
        {
            try
            {
                var userEmailList = _entities.UserEmails.Where(p => p.UserId == Member.Users.Id).ToList();
                ltUserEMailAddress.Text = EnrollMembershipHelper.CreateUserEmailsInfoTableProfile(Member, userEmailList);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        #endregion

        #region clear form inputs
        private void ClearPersonalInfo()
        {
            tbName.Text = string.Empty;
            tbSurname.Text = string.Empty;
            tbParola.Text = string.Empty;
            tbTC.Text = string.Empty;
            tbFatherName.Text = string.Empty;
            tbMotherName.Text = string.Empty;
            ddlGender.Items.Clear();
            ddlMaritalStatus.Items.Clear();
            tbMaidenName.Text = string.Empty;
            trMaidenName.Attributes.Add("class", "hideaboutgender");
            trMarriageDate.Attributes.Add("class", "hideaboutgender");
            dpMarriageDate.SelectedDate = null;
            ddlBloodType.Items.Clear();
            dpBirthDate.SelectedDate = null;
            tbBirthPlace.Text = string.Empty;
            hdnUserProfilPhotoUrl.Value = string.Empty;
            tbHobbies.Text = string.Empty;
            tbWeb.Text = string.Empty;
            tbGsmNo.Text = string.Empty;
            tbLastSchool.Text = string.Empty;
            dpLastSchoolGraduateDate.SelectedDate = null;

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
            tbMemberFoundation.Text = string.Empty;
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

            EnrollMembershipHelper.DataBindDDlSectorsJobs(Member, ddlJobSectors, ddlJobs);
            ddlJobSectors.SelectedIndex = 0;
            ddlJobs.SelectedIndex = 0;
        }
        private void ClearMembershipInfo()
        {
            lMemberRelationType.Text = string.Empty;
            lMembershipNumber.Text = string.Empty;
            lSpecialNumber.Text = string.Empty;
            lTerm.Text = string.Empty;
            lMembershipDate.Text = string.Empty;
        }
        #endregion

        #region generate password
        [WebMethod]
        public static string GeneratePassword()
        {
            var password = Guid.NewGuid().ToString("N");
            return password.Substring(0, 8);
        }
        #endregion

        #region bind Home countries cities towns
        public static void BindDdlCountries(DropDownList dropDownList, DropDownList ddlCity, DropDownList ddlTown, List<Countries> list)
        {
            dropDownList.Items.Clear();
            ddlCity.Items.Clear();
            ddlTown.Items.Clear();
            if (list != null)
                foreach (Countries item in list)
                {
                    dropDownList.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            dropDownList.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            ddlCity.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            ddlTown.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
        }
        public static void BindDdlCities(DropDownList dropDownList, DropDownList ddlTown, List<Cities> list)
        {
            dropDownList.Items.Clear();
            ddlTown.Items.Clear();
            if (list != null)
                foreach (Cities item in list)
                {
                    dropDownList.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            dropDownList.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            ddlTown.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
        }
        public static void BindDdlTowns(DropDownList dropDownList, List<Towns> list)
        {
            dropDownList.Items.Clear();
            if (list != null)
                foreach (Towns item in list)
                {
                    dropDownList.Items.Add(new ListItem(item.Name, item.Id.ToString()));
                }
            dropDownList.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
        }
        #endregion

        #region OnSelectedIndexChanged home&work countries cities towns

        protected void ddlHomeCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfHomeCountry.Value = ddlHomeCountry.SelectedIndex > 0 ? ddlHomeCountry.SelectedItem.Value : string.Empty;
            BindDdlCities(ddlHomeCity, ddlHomeTown, EnrollMembershipHelper.GetCities(hfHomeCountry.Value));
            ShowHomeInfoEditMode();
            Page.ClientScript.RegisterStartupScript(GetType(), "showUserInfo123edsaas", "<script> showUserInfo('dHomeDetail');</script>");
        }
        protected void ddlHomeCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHomeCity.SelectedIndex > 0)
            {
                hfHomeCity.Value = ddlHomeCity.SelectedItem.Value;
            }
            else
                hfHomeCity.Value = string.Empty;
            BindDdlTowns(ddlHomeTown, EnrollMembershipHelper.GetTowns(hfHomeCountry.Value, hfHomeCity.Value));
            ShowHomeInfoEditMode();
            Page.ClientScript.RegisterStartupScript(GetType(), "showUserInfo123edsaa1s", "<script> showUserInfo('dHomeDetail');</script>");
        }

        protected void ddlWorkCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCountry.Value = ddlWorkCountry.SelectedIndex > 0 ? ddlWorkCountry.SelectedItem.Value : string.Empty;
            BindDdlCities(ddlWorkCity, ddlWorkTown, EnrollMembershipHelper.GetCities(hfWorkCountry.Value));
            ShowWorkInfoEditMode();
            Page.ClientScript.RegisterStartupScript(GetType(), "showUserInfo123edsaas1", "<script> showUserInfo('dWorkDetail');</script>");
        }
        protected void ddlWorkCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfWorkCity.Value = ddlWorkCity.SelectedIndex > 0 ? ddlWorkCity.SelectedItem.Value : string.Empty;
            BindDdlTowns(ddlWorkTown, EnrollMembershipHelper.GetTowns(hfWorkCountry.Value, hfWorkCity.Value));
            ShowWorkInfoEditMode();
            Page.ClientScript.RegisterStartupScript(GetType(), "showUserInfo123edsaas12", "<script> showUserInfo('dWorkDetail');</script>");
        }

        #endregion

        #region edit user info changes <personal, home, work>
        protected void BtnSavePersonalInfoClick(object sender, EventArgs e)
        {
            try
            {
                var newUserGeneral = false;//userGeneral tablosu yeni oluştuysa
                if (Member != null && Member.Users != null && Member.Users.Id > 0)
                {
                    var user = _entities.Users.First(p => p.Id == Member.Users.Id);
                    user.Name = tbName.Text;
                    user.Surname = tbSurname.Text;

                    if (!string.IsNullOrWhiteSpace(tbParola.Text))
                    {
                        user.Password = Crypto.Encrypt(tbParola.Text);
                    }
                    user.UpdatedTime = DateTime.Now;

                    var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.Id == Member.UserGeneral.Id);
                    if (userGeneral == null)
                    {
                        userGeneral = new UserGeneral();
                        userGeneral.UserId = Member.Users.Id;
                        newUserGeneral = true;
                    }

                    userGeneral.Gender = ddlGender.SelectedIndex > 0
                                             ? (int?)Convert.ToInt32(ddlGender.SelectedItem.Value)
                                             : null;
                    userGeneral.MaritalStatus = ddlMaritalStatus.SelectedIndex > 0
                                                    ? (int?)Convert.ToInt32(ddlMaritalStatus.SelectedItem.Value)
                                                    : null;
                    userGeneral.BloodType = ddlBloodType.SelectedIndex > 0
                                                ? (int?)Convert.ToInt32(ddlBloodType.SelectedItem.Value)
                                                : null;
                    userGeneral.TC = tbTC.Text;
                    userGeneral.FatherName = tbFatherName.Text;
                    userGeneral.MotherName = tbMotherName.Text;
                    userGeneral.MarriageDate = dpMarriageDate.SelectedDate;
                    userGeneral.MaidenName = tbMaidenName.Text;
                    userGeneral.Birthdate = (dpBirthDate != null || dpBirthDate.SelectedDate != null ? dpBirthDate.SelectedDate : null);
                    userGeneral.Birthplace = tbBirthPlace.Text;
                    userGeneral.Hobby = tbHobbies.Text;
                    userGeneral.Web = tbWeb.Text;
                    userGeneral.GsmNo = tbGsmNo.Text;
                    userGeneral.LastSchool = tbLastSchool.Text;
                    if (dpLastSchoolGraduateDate.SelectedDate != null)
                        userGeneral.LastSchoolGraduateDate = Convert.ToDateTime("01.01." + dpLastSchoolGraduateDate.SelectedDate.Value.Year.ToString());

                    string photoUrl = SaveUserPhoto(user.Id);
                    if (!string.IsNullOrWhiteSpace(photoUrl))
                    {
                        hdnUserProfilPhotoUrl.Value = photoUrl;
                        userGeneral.PhotoUrl = photoUrl;
                    }
                    else
                    {
                        hdnUserProfilPhotoUrl.Value = string.Empty;
                    }

                    userGeneral.MemberFoundation = tbMemberFoundation.Text;
                    userGeneral.HidePersonalInfo = cbHidePersonalInfo.Checked;

                    // UserGeneral bilgisi yoksa önce dbye ekle sonra değişiklikleri kaydet
                    if (newUserGeneral)
                    {
                        _entities.AddToUserGeneral(userGeneral);
                    }

                    _entities.SaveChanges();
                    Member = EnrollMembershipHelper.BindUser(Member);
                    BindPersonalInfo();
                    ShowPersInfoViewMode();
                    Page.ClientScript.RegisterStartupScript(GetType(), "dPersonalDetail11", "<script> showUserInfo('dPersonalDetail');</script>");
                    MessageBox.Show(MessageType.jAlert, Resource.msgUpdated);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.jAlert, Resource.msgError);
            }
        }
        protected void BtnSaveHomeInfoClick(object sender, EventArgs e)
        {
            try
            {
                var newUserGeneral = false;//userGeneral tablosu yeni oluştuysa 

                if (Member != null && Member.Users != null && Member.Users.Id > 0)
                {

                    var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == Member.Users.Id);
                    if (userGeneral == null)
                    {
                        userGeneral = new UserGeneral();
                        userGeneral.UserId = Member.Users.Id;
                        newUserGeneral = true;
                    }
                    userGeneral.HomePhone = tbHomePhone.Text;
                    userGeneral.HomeAddress = tbHomeAddress.Text;
                    if (ddlHomeCountry.SelectedIndex > 0)
                        userGeneral.HomeCountry = Convert.ToInt32(ddlHomeCountry.SelectedItem.Value);
                    else
                        userGeneral.HomeCountry = null;

                    if (ddlHomeCity.SelectedIndex > 0)
                        userGeneral.HomeCity = Convert.ToInt32(ddlHomeCity.SelectedItem.Value);
                    else
                        userGeneral.HomeCity = null;

                    if (ddlHomeTown.SelectedIndex > 0)
                        userGeneral.HomeTown = Convert.ToInt32(ddlHomeTown.SelectedItem.Value);
                    else
                        userGeneral.HomeTown = null;
                    userGeneral.HomeZipCode = tbHomeZipCode.Text;

                    //// UserGeneral bilgisi yoksa önce dbye ekle sonra değişiklikleri kaydet
                    //if (userGeneral.UserId == 0)
                    //{
                    //    userGeneral.UserId = Member.Users.Id;
                    //    _entities.AddToUserGeneral(userGeneral);
                    //}
                    userGeneral.HidePersonalInfo = cbHidePersonalInfo.Checked;

                    // UserGeneral bilgisi yoksa önce dbye ekle sonra değişiklikleri kaydet
                    if (newUserGeneral)
                    {
                        _entities.AddToUserGeneral(userGeneral);
                    }

                    _entities.SaveChanges();
                    Member = EnrollMembershipHelper.BindUser(Member);
                    BindHomeInfo();
                    ShowHomeInfoViewMode();
                    Page.ClientScript.RegisterStartupScript(GetType(), "dHomeDetail11", "<script> showUserInfo('dHomeDetail');</script>");
                    MessageBox.Show(MessageType.jAlert, Resource.msgUpdated);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.jAlert, Resource.msgError);
            }
        }
        protected void BtnSaveWorkInfoClick(object sender, EventArgs e)
        {
            try
            {
                var newUserGeneral = false;//userGeneral tablosu yeni oluştuysa 

                if (Member != null && Member.Users != null && Member.Users.Id > 0 && Member.UserGeneral != null)
                {
                    var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == Member.Users.Id);
                    if (userGeneral == null)
                    {
                        userGeneral = new UserGeneral();
                        userGeneral.UserId = Member.Users.Id;
                        newUserGeneral = true;
                    }
                    userGeneral.WorkPhone = tbWorkPhone.Text;
                    userGeneral.WorkAddress = tbWorkAddress.Text;

                    if (ddlWorkCountry.SelectedIndex > 0)
                        userGeneral.WorkCountry = Convert.ToInt32(ddlWorkCountry.SelectedItem.Value);
                    else
                        userGeneral.WorkCountry = null;

                    if (ddlWorkCity.SelectedIndex > 0)
                        userGeneral.WorkCity = Convert.ToInt32(ddlWorkCity.SelectedItem.Value);
                    else
                        userGeneral.WorkCity = null;

                    if (ddlWorkTown.SelectedIndex > 0)
                        userGeneral.WorkTown = Convert.ToInt32(ddlWorkTown.SelectedItem.Value);
                    else
                        userGeneral.WorkTown = null;

                    userGeneral.WorkZipCode = tbWorkZipCode.Text;
                    userGeneral.WorkFax = tbWorkFax.Text;
                    userGeneral.WorkCorporation = tbWorkCorparation.Text;

                    userGeneral.WorkTitle = tbWorkTitle.Text;

                    if (ddlJobs.SelectedIndex > 0)
                    {
                        userGeneral.JobNo = Convert.ToInt32(ddlJobs.SelectedItem.Value);
                    }
                    else
                        userGeneral.JobNo = null;

                    if (ddlJobSectors.SelectedIndex > 0)
                    {
                        userGeneral.JobSectorNo = Convert.ToInt32(ddlJobSectors.SelectedItem.Value);
                    }
                    else
                        userGeneral.JobSectorNo = null;

                    userGeneral.HideJobInfo = cbHideJobInfo.Checked;


                    // UserGeneral bilgisi yoksa önce dbye ekle sonra değişiklikleri kaydet
                    if (newUserGeneral)
                    {
                        _entities.AddToUserGeneral(userGeneral);
                    }


                    _entities.SaveChanges();
                    Member = EnrollMembershipHelper.BindUser(Member);
                    BindJobInfo();
                    ShowWorkInfoViewMode();
                    Page.ClientScript.RegisterStartupScript(GetType(), "dWorkDetail11", "<script> showUserInfo('dWorkDetail');</script>");
                    MessageBox.Show(MessageType.jAlert, Resource.msgUpdated);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                MessageBox.Show(MessageType.jAlert, Resource.msgError);
            }
        }
        #endregion

        #region edit/createNew  email details <WebMethods>
        protected void BtnSaveEmailInfoClick(object sender, EventArgs e)
        {
            try
            {
                string email = tbNewEmailAddress.Text.TrimEnd(' ').TrimStart(' ');
                var checkEmailAddress = _entities.UserEmails.Where(p => p.Email == email).ToList();
                if (checkEmailAddress.Count > 0)
                {
                    tbNewEmailAddress.Text = string.Empty;
                    Page.ClientScript.RegisterStartupScript(GetType(),
                        "dEmailAddress11", "<script> showUserInfo('dEmailAddress');alert('" + AdminResource.lbMailAddressTakenAlready + "');</script>");
                    return;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(tbNewEmailAddress.Text))
                    {
                        var newEmailAddress = new UserEmails();
                        newEmailAddress.UserId = Member.Users.Id;
                        newEmailAddress.Email = email;
                        newEmailAddress.Activated = false;
                        newEmailAddress.AllowMailing = false;
                        newEmailAddress.MainAddress = false;
                        _entities.AddToUserEmails(newEmailAddress);
                        _entities.SaveChanges();

                        var activationMailResult = EnrollMembershipHelper.SendEmailActivationMail(Member.Users.Id, newEmailAddress.Id, "/App_Themes/mainTheme/mailtemplates/MemberActivationMailContent.htm");
                        tbNewEmailAddress.Text = string.Empty;
                        MessageBox.Show(MessageType.jAlert, activationMailResult);

                    }
                }

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            tbNewEmailAddress.Text = string.Empty;
            BindEmailInfo();
            ShowEmailInfoViewMode();
            Page.ClientScript.RegisterStartupScript(GetType(), "newEmail1",
                                                            "<script> showUserInfo('dEmailAddress');</script>");
        }
        [WebMethod]
        public static string ChangeMailing(string userIdStr, string emailIdStr, string islem)
        {
            var result = string.Empty;
            var e = new Entities();
            try
            {
                var userId = Convert.ToInt32(Crypto.Decrypt(userIdStr));
                var emailId = Convert.ToInt32(Crypto.Decrypt(emailIdStr));
                var islemId = Convert.ToInt32(islem);
                if (userId == Member.Users.Id)
                {
                    var email = e.UserEmails.FirstOrDefault(p => p.Id == emailId && p.UserId == userId);
                    if (email != null)
                    {
                        switch (islemId)
                        {
                            case 1:
                                email.AllowMailing = true;
                                break;
                            case 0:
                                email.AllowMailing = false;
                                break;
                        }
                        e.SaveChanges();

                        result = EnrollMembershipHelper.CreateUserEmailsInfoTableProfile(Member, e.UserEmails.Where(p => p.UserId == Member.Users.Id).ToList());
                        return result;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            return result;
        }
        [WebMethod]
        public static string ChangeMailingAdmn(string userIdStr, string emailIdStr, string islem)
        {
            var result = string.Empty;
            var e = new Entities();
            try
            {
                var userId = Convert.ToInt32(Crypto.Decrypt(userIdStr));
                var emailId = Convert.ToInt32(Crypto.Decrypt(emailIdStr));
                var islemId = Convert.ToInt32(islem);
                var user = e.Users.FirstOrDefault(p => p.Id == userId);
                if (user != null)
                {
                    var email = e.UserEmails.FirstOrDefault(p => p.Id == emailId && p.UserId == userId);
                    if (email != null && emailId == email.Id)
                    {
                        Member = EnrollMembershipHelper.BindUser(Member, userId);
                        switch (islemId)
                        {
                            case 1:
                                email.AllowMailing = true;
                                break;
                            case 0:
                                email.AllowMailing = false;
                                break;
                        }
                        e.SaveChanges();

                        result = EnrollMembershipHelper.CreateUserEmailsInfoTableAdmin(Member, e.UserEmails.Where(p => p.UserId == Member.Users.Id).ToList());
                        return result;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            return result;
        }
        [WebMethod]
        public static string DeleteEmail(string userIdStr, string emailIdStr)
        {
            //CheckCulture();
            var result = string.Empty;
            var e = new Entities();
            try
            {
                var userId = Convert.ToInt32(Crypto.Decrypt(userIdStr));
                var emailId = Convert.ToInt32(Crypto.Decrypt(emailIdStr));
                if (userId == Member.Users.Id)
                {
                    var email = e.UserEmails.FirstOrDefault(p => p.Id == emailId && p.UserId == userId);
                    if (email != null)
                    {
                        if (email.MainAddress)
                        {
                            return "-1";
                        }
                        e.DeleteObject(email);
                        e.SaveChanges();
                        result = EnrollMembershipHelper.CreateUserEmailsInfoTableProfile(Member, e.UserEmails.Where(p => p.UserId == Member.Users.Id).ToList());
                        return result;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            return result;
        }
        [WebMethod]
        public static string DeleteEmailAdmn(string userIdStr, string emailIdStr)
        {
            //EnrollMembershipHelper.CheckCulture();
            var result = string.Empty;
            var e = new Entities();
            try
            {
                var userId = Convert.ToInt32(Crypto.Decrypt(userIdStr));
                var emailId = Convert.ToInt32(Crypto.Decrypt(emailIdStr));

                var user = e.Users.FirstOrDefault(p => p.Id == userId);
                if (user != null)
                {
                    var email = e.UserEmails.FirstOrDefault(p => p.Id == emailId && p.UserId == userId);
                    if (email != null && emailId == email.Id)
                    {
                        Member = EnrollMembershipHelper.BindUser(Member, userId);
                        if (email.MainAddress)
                        {
                            return "-1";
                        }
                        e.DeleteObject(email);
                        e.SaveChanges();
                        result = EnrollMembershipHelper.CreateUserEmailsInfoTableAdmin(Member, e.UserEmails.Where(p => p.UserId == Member.Users.Id).ToList());
                        return result;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            return result;
        }
        [WebMethod]
        public static string ActivateMailAddress(string userIdStr, string emailIdStr)
        {

            var e = new Entities();
            var result = string.Empty;
            try
            {
                //EnrollMembershipHelper.CheckCulture();
                int userId = Convert.ToInt32(Crypto.Decrypt(userIdStr));
                int emailId = Convert.ToInt32(Crypto.Decrypt(emailIdStr));
                var user = e.Users.FirstOrDefault(p => p.Id == userId);
                if (user != null)
                {

                    result = EnrollMembershipHelper.SendEmailActivationMail(userId, emailId,
                                                "App_Themes/mainTheme/mailtemplates/MemberActivationMailContent.htm");
                }
                else
                {
                    result = AdminResource.lbUserInformationNotValid;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            return result;
        }
        [WebMethod]
        public static string ChangeActiveEmail(string userIdStr, string emailIdStr)
        {
            var e = new Entities();
            var result = string.Empty;
            bool processResult = false;
            try
            {
                //EnrollMembershipHelper.CheckCulture();
                var userId = Convert.ToInt32(Crypto.Decrypt(userIdStr));
                var emailId = Convert.ToInt32(Crypto.Decrypt(emailIdStr));
                var user = e.Users.FirstOrDefault(p => p.Id == userId);
                if (user != null && user.Id > 0)
                {
                    var userEmails = e.UserEmails.Where(p => p.UserId == userId).ToList();
                    var userSelectedEmail = e.UserEmails.FirstOrDefault(p => p.Id == emailId);
                    if (userSelectedEmail != null && userSelectedEmail.UserId == userId)
                    {
                        foreach (var email in userEmails)
                        {
                            email.MainAddress = false;
                        }
                        userSelectedEmail.MainAddress = true;
                        user.EMail = userEmails.First(p => p.Id == emailId).Email;
                        e.SaveChanges();
                        processResult = true;
                    }

                }
                else
                {
                    result = AdminResource.lbError;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            if (processResult)
            {
                if (Logout()) return "giris.aspx?ReturnUrl=~/profil.aspx";
            }
            return result;
        }
        [WebMethod]
        public static string ChangeActiveEmailAdmn(string userIdStr, string emailIdStr)
        {
            var e = new Entities();
            var result = string.Empty;
            bool processResult = false;
            try
            {
                //EnrollMembershipHelper.CheckCulture();
                var userId = Convert.ToInt32(Crypto.Decrypt(userIdStr));
                var emailId = Convert.ToInt32(Crypto.Decrypt(emailIdStr));
                var user = e.Users.FirstOrDefault(p => p.Id == userId);
                if (user != null && user.Id > 0)
                {
                    var userEmails = e.UserEmails.Where(p => p.UserId == userId).ToList();
                    var userSelectedEmail = e.UserEmails.FirstOrDefault(p => p.Id == emailId);
                    if (userSelectedEmail != null && userSelectedEmail.UserId == userId)
                    {
                        if (!userSelectedEmail.Activated)
                        {
                            MessageBox.Show(MessageType.Notice, AdminResource.msgMailAddressNotActivated);
                            return string.Empty;
                        }
                        foreach (var email in userEmails)
                        {
                            email.MainAddress = false;
                        }
                        userSelectedEmail.MainAddress = true;
                        user.EMail = userEmails.First(p => p.Id == emailId).Email;
                        e.SaveChanges();
                        processResult = true;
                    }

                }
                else
                {
                    result = AdminResource.lbError;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            if (processResult)
            {
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                result = EnrollMembershipHelper.CreateUserEmailsInfoTableAdmin(Member, e.UserEmails.Where(p => p.UserId == Member.Users.Id).ToList());

            }
            return result;
        }
        #endregion

        #region save user profil photo
        public string SaveUserPhoto(int userId)
        {
            bool isImageSaved = false;
            string resizedImagePath = string.Empty;
            hdnActiveDirectory.Value = WebConfigurationManager.AppSettings["UserFilesPath"];
            var uploadedFiles = HttpContext.Current.Request.Files;
            if (uploadedFiles.Count > 0)
            {
                var userPostedFile = uploadedFiles[0];
                if (userPostedFile.ContentLength > 1024 * 1024)
                {
                    MessageBox.Show(MessageType.Error, Resource.msgFileSizeTooLarge);
                    return string.Empty;
                }
                if (userPostedFile.ContentLength > 0 && userPostedFile.ContentLength < 1024 * 1024)
                {
                    try
                    {
                        string fileExtension = System.IO.Path.GetExtension(userPostedFile.FileName);
                        String filePath = Server.MapPath(hdnActiveDirectory.Value);
                        string orjinalImagePath = filePath + "\\UserImages\\temp_" + userId.ToString() + fileExtension;

                        userPostedFile.SaveAs(orjinalImagePath);

                        var orj = new Bitmap(orjinalImagePath);
                        lock (orj)
                        {
                            Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                            resizedImagePath = "~/FileManager/UserImages/userProfileImage_" + userId.ToString() + fileExtension;
                            isImageSaved = ImageHelper.SaveJpeg(filePath + "\\UserImages\\userProfileImage_" + userId.ToString() + fileExtension, (Bitmap)i, 75);
                            i.Dispose();
                        }
                        orj.Dispose();

                        if (isImageSaved)
                        {
                            hdnUserProfilPhotoUrl.Value = resizedImagePath;
                            Member.UserGeneral.PhotoUrl = resizedImagePath;
                        }
                        else
                        {
                            MessageBox.Show(MessageType.Error, Resource.msgPhotoNotSaved);
                        }

                        ImageHelper.DeleteImage(orjinalImagePath);
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.ManageException(exception);
                    }
                }

            }
            return resizedImagePath;
        }
        #endregion

        #region javascript show/hide - button/div
        public void ShowPersInfoViewMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "viewPersonalInfoClickweqrq2",
                                                            "<script> viewPersonalInfoClick();</script>");
        }
        public void ShowPersInfoEditMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(),
                "editPersonalInfoClick32423redf", "<script> editPersonalInfoClick();$(\"#results\").empty();</script>");
        }

        public void ShowHomeInfoViewMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "viewHomeInfoClick213123", "<script> viewHomeInfoClick();</script>");
        }
        public void ShowHomeInfoEditMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "viewHomeInfoClick34214d", "<script> editHomeInfoClick();</script>");
        }

        public void ShowMemberInfoViewMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "viewPersonalInfoClickwqerdwq", "<script> viewMemberInfoClick();</script>");
        }

        public void ShowWorkInfoViewMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "viewWorkInfoClicksadsad", "<script> viewWorkInfoClick();</script>");
        }
        public void ShowWorkInfoEditMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "editWorkInfoClick1234r", "<script> editWorkInfoClick();</script>");
        }

        public void ShowEmailInfoViewMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "viewEmailInfoClick123r5d", "<script> viewEmailInfoClick();</script>");
        }
        public void ShowEmailInfoEditMode()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "viewEmailInfoClickw4321ed", "<script> editEmailInfoClick();</script>");
        }
        #endregion

        private static bool Logout()
        {
            try
            {
                var cookie = HttpContext.Current.Response.Cookies["EnrollAuthentication"];
                if (cookie != null) cookie.Expires = DateTime.Now.AddMinutes(-1);

                cookie = HttpContext.Current.Response.Cookies["EnrollAdminLanguage"];
                if (cookie != null) cookie.Expires = DateTime.Now.AddMinutes(-1);

                cookie = HttpContext.Current.Response.Cookies["EnrollDataLanguage"];
                if (cookie != null) cookie.Expires = DateTime.Now.AddMinutes(-1);

                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                FormsAuthentication.SignOut();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        protected string GetDuesType(int duesTypeId)
        {
            try
            {
                var duesType = _entities.DuesTypes.FirstOrDefault(p => p.Id == duesTypeId);
                return duesType == null ? string.Empty : duesType.Title;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return string.Empty;
        }

        protected string GetUserName(int userId)
        {
            try
            {
                var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                if (user == null) return string.Empty;
                return string.Format("{0} {1}", user.Name, user.Surname);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return string.Empty;
        }

        protected string GetPaymentType(int paymentTypeId)
        {
            try
            {
                switch (paymentTypeId)
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

        protected void BtPayCertainAmount_OnClick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(),
                        "dFinanceDetail21", "<script> showUserInfo('dFinanceDetail'); </script>");
            return;
        }
        protected void BtPayAllAmount_OnClick(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(),
                        "dFinanceDetail22", "<script> showUserInfo('dFinanceDetail'); </script>");
            return;
        }
        protected void gvChargesForDues_OnPageIndexChanged(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(),
                        "dFinanceDetail23", "<script> showUserInfo('dFinanceDetail');</script>");
            return;
        }
    }
}