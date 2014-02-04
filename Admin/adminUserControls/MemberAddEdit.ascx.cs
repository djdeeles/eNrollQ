using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;
using Image = System.Drawing.Image;

namespace eNroll.Admin.adminUserControls
{
    public partial class MemberAddEdit : UserControl
    {
        private static MemberInfo Member = new MemberInfo();
        private readonly Entities _entities = new Entities();
        private DataTable _memberSearchDataTable = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            btnUpdatePersonalInfo.Text = AdminResource.lbUpdate;
            btnUpdateMemberInfo.Text = AdminResource.lbUpdate;
            BtnUpdateHomeInfo.Text = AdminResource.lbUpdate;
            BtnUpdateWorkInfo.Text = AdminResource.lbUpdate;
            btnSaveEmailInfo.Text = AdminResource.lbAdd;
            BtnSetPassive.Text = AdminResource.lbSetPassive;
            BtnSetActive.Text = AdminResource.lbSetActive;
            BtnResendActivationCode.Text = AdminResource.lbSendActivationCode;
            BtnConfirmMemberActive.Text = AdminResource.lbConfirmMember;
            BindAll();
        }

        public void BindAll()
        {
            if (!string.IsNullOrWhiteSpace(hfUserId.Value))
            {
                Member = BindForEditMember(Convert.ToInt32(hfUserId.Value));
                BindPersonalInfo();
                BindMemberInfo();
                BindHomeInfo();
                BindWorkInfo();
                BindEmailInfo();
            }
        }

        public MemberInfo BindForEditMember(int userId)
        {
            hfUserId.Value = userId.ToString();
            Member = EnrollMembershipHelper.BindUser(Member, userId);
            return Member;
        }

        public MemberInfo BindForNewMember()
        {
            return Member;
        }

        public void BindPersonalInfo()
        {
            //önce alanlar temizlenir
            ClearPersonalInfo();
            var userId = Convert.ToInt32(hfUserId.Value);
            Member = BindForEditMember(userId);
            try
            {
                tbEditName.Text = Member.Users.Name;
                tbEditSurname.Text = Member.Users.Surname;

                if (Member.UserGeneral != null)
                {
                    #region user general info

                    lUserNameSurname.Text = string.Format("{0} {1}", Member.Users.Name, Member.Users.Surname);
                    lUserEmail.Text = Member.Users.EMail;
                    lUserPhoto.ImageUrl = !string.IsNullOrWhiteSpace(Member.UserGeneral.PhotoUrl)
                                              ? Member.UserGeneral.PhotoUrl
                                              : "/App_Themes/mainTheme/images/noimage.png";

                    tbEditHobbies.Text = Member.UserGeneral.Hobby;
                    tbEditWeb.Text = Member.UserGeneral.Web;
                    tbEditGsmNo.Text = Member.UserGeneral.GsmNo;

                    if (Member.UserGeneral.TC != null) tbEditTC.Text = Member.UserGeneral.TC;
                    tbEditFatherName.Text = Member.UserGeneral.FatherName;
                    tbEditMotherName.Text = Member.UserGeneral.MotherName;

                    #endregion

                    // cinsiyet
                    EnrollMembershipHelper.DataBindDdlGender(Member, ddlEditGender);

                    //kan grubu alanı doldurulur ve seçilir
                    EnrollMembershipHelper.DataBindDDlBloodType(Member, ddlEditBloodType);

                    #region evlilik durumuna göre kızlık soyadı evlilik tarihi gibi alanlar visible/invisible yapılarak alanlar doldurulur

                    EnrollMembershipHelper.DataBindDdlMaritalStatus(Member, ddlEditMaritalStatus);

                    if (Member.UserGeneral.MaritalStatus != null)
                    {
                        switch (Member.UserGeneral.MaritalStatus.Value)
                        {
                            case 1:
                                if (Member.UserGeneral.MaidenName != null)
                                {
                                    tbEditMaidenName.Text = Member.UserGeneral.MaidenName;
                                    trMaidenName.Style.Remove("class");
                                }
                                if (Member.UserGeneral.MarriageDate != null)
                                {
                                    dpEditMarriageDate.SelectedDate = Member.UserGeneral.MarriageDate.Value;
                                    trMarriageDate.Style.Remove("class");
                                }
                                break;
                            case 2:
                                break;
                        }
                    }
                    else
                    {
                        trMaidenName.Attributes.Add("class", "hideaboutgender");
                        trMarriageDate.Attributes.Add("class", "hideaboutgender");
                    }

                    #endregion

                    #region doğum günü, doğum yeri, profil fotoğrafı

                    if (Member.UserGeneral.Birthdate != null)
                    {
                        var date = Member.UserGeneral.Birthdate.Value;
                        dpEditBirthDate.SelectedDate = date;
                    }

                    tbEditBirthPlace.Text = Member.UserGeneral.Birthplace;

                    hdnUserProfilPhotoUrl.Value = !string.IsNullOrWhiteSpace(Member.UserGeneral.PhotoUrl)
                                                      ? Member.UserGeneral.PhotoUrl
                                                      : string.Empty;

                    #endregion

                    #region okul bilgileri

                    tbEditLastSchool.Text = Member.UserGeneral.LastSchool;
                    if (Member.UserGeneral.LastSchoolGraduateDate != null)
                    {
                        dpEditLastSchoolGraduateDate.SelectedDate = Member.UserGeneral.LastSchoolGraduateDate.Value;
                    }

                    #endregion

                    tbEditMemberFoundation.Text = Member.UserGeneral.MemberFoundation;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public void BindMemberInfo()
        {
            try
            {
                //önce alanlar temizlenir
                ClearMemberInfo();
                var userId = Convert.ToInt32(hfUserId.Value);
                Member = BindForEditMember(userId);

                EnrollMembershipHelper.DataBindDdlMembershipRelType(ddlEditMemberRelationType);
                if (!string.IsNullOrWhiteSpace(Member.UserFoundation.MemberRelType.ToString()))
                {
                    var relTypeIndex = Convert.ToInt32(Member.UserFoundation.MemberRelType);
                    if (ddlEditMemberRelationType.Items.Count >= relTypeIndex)
                        ddlEditMemberRelationType.SelectedIndex = Convert.ToInt32(Member.UserFoundation.MemberRelType);
                }

                if (Member.UserFoundation.MembershipDate != null && Member.UserFoundation.MembershipDate.Value != null)
                    dpEditMembershipDate.SelectedDate = Member.UserFoundation.MembershipDate;
                if (Member.UserFoundation.Term != null && !string.IsNullOrEmpty(Member.UserFoundation.Term))
                    dpEditTerm.SelectedDate = Convert.ToDateTime(string.Format("01.01.{0}", Member.UserFoundation.Term));

                if (Member.UserFinance != null)
                    cbEditAutoPaymentOrder.Checked = Member.UserFinance.AutoPay;
                if (Member.UserFoundation.SpecialNo != null)
                    tbEditSpecialNumber.Text = Member.UserFoundation.SpecialNo;
                if (Member.UserFoundation.MemberNo != null)
                    tbEditMembershipNumber.Text = Member.UserFoundation.MemberNo;
                if (Member.UserGeneral.AdminNote != null)
                    tbAdminNote.Text = Member.UserGeneral.AdminNote;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public void BindHomeInfo()
        {
            try
            {
                ClearHomeInfo();
                var userId = Convert.ToInt32(hfUserId.Value);
                Member = BindForEditMember(userId);

                Member = BindHomeCountryCityTown(Member, ddlEditHomeCountry, ddlEditHomeCity, ddlEditHomeTown);

                hfEditHomeCountry.Value = Member.UserGeneral.HomeCountry != null
                                              ? Member.UserGeneral.HomeCountry.Value.ToString()
                                              : string.Empty;
                hfEditHomeCity.Value = Member.UserGeneral.HomeCity != null
                                           ? Member.UserGeneral.HomeCity.Value.ToString()
                                           : string.Empty;
                hfEditHomeTown.Value = Member.UserGeneral.HomeTown != null
                                           ? Member.UserGeneral.HomeTown.Value.ToString()
                                           : string.Empty;

                tbEditHomeAddress.Text = Member.UserGeneral.HomeAddress;
                tbEditHomePhone.Text = Member.UserGeneral.HomePhone;
                tbEditHomeZipCode.Text = Member.UserGeneral.HomeZipCode;
                cbEditHidePersonalInfo.Checked = Member.UserGeneral.HidePersonalInfo;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public void BindWorkInfo()
        {
            try
            {
                //önce alanlar temizlenir
                ClearWorkInfo();

                #region bind country, city, town

                var userId = Convert.ToInt32(hfUserId.Value);
                Member = BindForEditMember(userId);

                Member = BindWorkCountryCityTown(Member, ddlEditWorkCountry, ddlEditWorkCity, ddlEditWorkTown);
                hfEditWorkCountry.Value = Member.UserGeneral.WorkCountry != null
                                              ? Member.UserGeneral.WorkCountry.Value.ToString()
                                              : string.Empty;
                hfEditWorkCity.Value = Member.UserGeneral.WorkCity != null
                                           ? Member.UserGeneral.WorkCity.Value.ToString()
                                           : string.Empty;
                hfEditWorkTown.Value = Member.UserGeneral.WorkTown != null
                                           ? Member.UserGeneral.WorkTown.Value.ToString()
                                           : string.Empty;

                #endregion

                //bind jobs & jobsectors
                EnrollMembershipHelper.DataBindDDlSectorsJobs(Member, ddlEditJobSectors, ddlEditJobs);

                tbEditWorkAddress.Text = Member.UserGeneral.WorkAddress;
                tbEditWorkPhone.Text = Member.UserGeneral.WorkPhone;
                tbEditWorkZipCode.Text = Member.UserGeneral.WorkZipCode;
                tbEditWorkFax.Text = Member.UserGeneral.WorkFax;
                tbEditWorkCorparation.Text = Member.UserGeneral.WorkCorporation;
                tbEditWorkTitle.Text = Member.UserGeneral.WorkTitle;

                cbEditHidePersonalInfo.Checked = Member.UserGeneral.HidePersonalInfo;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public void BindEmailInfo()
        {
            try
            {
                var userId = Convert.ToInt32(hfUserId.Value);
                Member = BindForEditMember(userId);

                var userEmailList = _entities.UserEmails.Where(p => p.UserId == Member.Users.Id).ToList();
                ltUserEMailAddress.Text = EnrollMembershipHelper.CreateUserEmailsInfoTableAdmin(Member, userEmailList);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public static MemberInfo BindHomeCountryCityTown(MemberInfo memberInfo, DropDownList ddlCountry,
                                                         DropDownList ddlCity, DropDownList ddlTown)
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
                BindDdlCities(ddlCity, ddlTown,
                              EnrollMembershipHelper.GetCities(memberInfo.UserGeneral.HomeCountry.Value.ToString()));
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

                    BindDdlTowns(ddlTown,
                                 EnrollMembershipHelper.GetTowns(memberInfo.UserGeneral.HomeCountry.Value.ToString(),
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

        public static MemberInfo BindWorkCountryCityTown(MemberInfo memberInfo, DropDownList ddlCountry,
                                                         DropDownList ddlCity, DropDownList ddlTown)
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
                BindDdlCities(ddlCity, ddlTown,
                              EnrollMembershipHelper.GetCities(memberInfo.UserGeneral.WorkCountry.Value.ToString()));
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

                    BindDdlTowns(ddlTown,
                                 EnrollMembershipHelper.GetTowns(memberInfo.UserGeneral.WorkCountry.Value.ToString(),
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

        protected void BtnConfirmMemberActiveClick(object sender, EventArgs eventArgs)
        {
            if (Member.Users != null && Member.Users.Id > 0)
            {
                try
                {
                    var userId = Member.Users.Id;
                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == userId);

                    if (user != null && userFoundation != null)
                    {
                        user.State = true;
                        userFoundation.MemberState = true;
                        _entities.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindAll();
        }

        protected void BtnResendActivationCodeClick(object sender, EventArgs eventArgs)
        {
            if (Member.Users != null && Member.Users.Id > 0)
            {
                try
                {
                    var userId = Member.Users.Id;

                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    if (user != null)
                    {
                        var userEmail = _entities.UserEmails.FirstOrDefault(p => p.UserId == userId && p.MainAddress);
                        if (userEmail != null)
                        {
                            EnrollMembershipHelper.SendEmailActivationMail(user.Id, userEmail.Id,
                                                                           "../App_Themes/mainTheme/mailtemplates/MemberActivationMailContent.htm");
                        }
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
            BindAll();
        }

        protected void BtnSetActiveClick(object sender, EventArgs eventArgs)
        {
            if (Member.Users != null && Member.Users.Id > 0)
            {
                try
                {
                    var userId = Member.Users.Id;
                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    if (user != null)
                    {
                        user.State = true;
                        BtnSetActive.Visible = false;
                        BtnSetPassive.Visible = true;
                        BtnConfirmMemberActive.Visible = false;
                        BtnResendActivationCode.Visible = false;
                        _entities.Users.ApplyCurrentValues(user);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Notice, AdminResource.msgMemberActivated);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindAll();
        }

        protected void BtnSetPassiveClick(object sender, EventArgs eventArgs)
        {
            if (Member.Users != null && Member.Users.Id > 0)
            {
                try
                {
                    var userId = Member.Users.Id;
                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    if (user != null)
                    {
                        user.State = false;
                        BtnSetActive.Visible = true;
                        BtnSetPassive.Visible = false;
                        BtnConfirmMemberActive.Visible = false;
                        BtnResendActivationCode.Visible = false;
                        _entities.Users.ApplyCurrentValues(user);
                        _entities.SaveChanges();
                        MessageBox.Show(MessageType.Notice, AdminResource.msgMemberDeactivated);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindAll();
        }

        #region bind Home countries cities towns

        public static void BindDdlCountries(DropDownList dropDownList, DropDownList ddlCity, DropDownList ddlTown,
                                            List<Countries> list)
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

        #region Clear edit members inputs

        private void ClearPersonalInfo()
        {
            tbEditTC.Text = string.Empty;
            tbEditName.Text = string.Empty;
            tbEditSurname.Text = string.Empty;
            tbEditParola.Text = string.Empty;
            ddlEditGender.Items.Clear();
            ddlEditBloodType.Items.Clear();
            ddlEditMaritalStatus.Items.Clear();
            dpEditMarriageDate.SelectedDate = null;
            dpEditLastSchoolGraduateDate.SelectedDate = null;
            dpEditBirthDate.SelectedDate = null;
            tbEditFatherName.Text = string.Empty;
            tbEditMotherName.Text = string.Empty;
            tbEditMaidenName.Text = string.Empty;
            trMaidenName.Attributes.Add("class", "hideaboutgender");
            trMarriageDate.Attributes.Add("class", "hideaboutgender");
            tbEditBirthPlace.Text = string.Empty;
            tbEditHobbies.Text = string.Empty;
            tbEditMemberFoundation.Text = string.Empty;
            tbEditWeb.Text = string.Empty;
            tbEditGsmNo.Text = string.Empty;
            tbEditLastSchool.Text = string.Empty;
            cbMemberDecease.Checked = false;
            dpMemberDeceaseDate.SelectedDate = null;
        }

        private void ClearMemberInfo()
        {
            tbEditMembershipNumber.Text = string.Empty;
            ddlEditMemberRelationType.Items.Clear();
            dpEditMembershipDate.SelectedDate = null;
            tbEditSpecialNumber.Text = string.Empty;
            dpEditTerm.SelectedDate = null;
            cbEditAutoPaymentOrder.Checked = false;
            tbAdminNote.Text = string.Empty;
        }

        private void ClearHomeInfo()
        {
            ddlEditHomeCountry.Items.Clear();
            ddlEditHomeCity.Items.Clear();
            ddlEditHomeTown.Items.Clear();
            tbEditHomeAddress.Text = string.Empty;
            tbEditHomeZipCode.Text = string.Empty;
            tbEditHomePhone.Text = string.Empty;
            cbEditHidePersonalInfo.Checked = false;
        }

        private void ClearWorkInfo()
        {
            ddlEditWorkCountry.Items.Clear();
            ddlEditWorkCity.Items.Clear();
            ddlEditWorkTown.Items.Clear();
            ddlEditJobSectors.Items.Clear();
            ddlEditJobs.Items.Clear();
            tbEditWorkAddress.Text = string.Empty;
            tbEditWorkZipCode.Text = string.Empty;
            tbEditWorkPhone.Text = string.Empty;
            tbEditWorkFax.Text = string.Empty;
            tbEditWorkTitle.Text = string.Empty;
            tbEditWorkCorparation.Text = string.Empty;
            cbEditHideJobInfo.Checked = false;
        }

        #endregion

        #region javascript show/hide - button/div

        public void ShowPersInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv1",
                                                    "<script> showUserInfo('dPersonalDetail');</script>");
        }

        public void ShowHomeInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv3",
                                                    "<script> showUserInfo('dHomeDetail');</script>");
        }

        public void ShowMemberInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv3",
                                                    "<script> showUserInfo('dMemberDetail');</script>");
        }

        public void ShowWorkInfo()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvv5",
                                                    "<script> showUserInfo('dWorkDetail');</script>");
        }

        #endregion

        #region OnSelectedIndexChanged home&work countries cities towns

        protected void ddlEditHomeCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfEditHomeCountry.Value = ddlEditHomeCountry.SelectedIndex > 0
                                          ? ddlEditHomeCountry.SelectedItem.Value
                                          : string.Empty;
            BindDdlCities(ddlEditHomeCity, ddlEditHomeTown, EnrollMembershipHelper.GetCities(hfEditHomeCountry.Value));
            ShowHomeInfo();
        }

        protected void ddlEditHomeCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEditHomeCity.SelectedIndex > 0)
            {
                hfEditHomeCity.Value = ddlEditHomeCity.SelectedItem.Value;
            }
            else
                hfEditHomeCity.Value = string.Empty;
            BindDdlTowns(ddlEditHomeTown, EnrollMembershipHelper.GetTowns(hfEditHomeCountry.Value, hfEditHomeCity.Value));

            ShowHomeInfo();
        }

        protected void ddlEditWorkCountry_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfEditWorkCountry.Value = ddlEditWorkCountry.SelectedIndex > 0
                                          ? ddlEditWorkCountry.SelectedItem.Value
                                          : string.Empty;
            BindDdlCities(ddlEditWorkCity, ddlEditWorkTown, EnrollMembershipHelper.GetCities(hfEditWorkCountry.Value));
            ShowWorkInfo();
        }

        protected void ddlEditWorkCity_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            hfEditWorkCity.Value = ddlEditWorkCity.SelectedIndex > 0 ? ddlEditWorkCity.SelectedItem.Value : string.Empty;
            BindDdlTowns(ddlEditWorkTown, EnrollMembershipHelper.GetTowns(hfEditWorkCountry.Value, hfEditWorkCity.Value));
            ShowWorkInfo();
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

                if (userPostedFile.ContentLength > 0)
                {
                    try
                    {
                        string fileExtension = Path.GetExtension(userPostedFile.FileName);
                        String filePath = Server.MapPath(hdnActiveDirectory.Value);
                        string orjinalImagePath = filePath + "\\UserImages\\temp_" + userId.ToString() + fileExtension;

                        userPostedFile.SaveAs(orjinalImagePath);

                        var orj = new Bitmap(orjinalImagePath);
                        lock (orj)
                        {
                            Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                            resizedImagePath = "~/FileManager/UserImages/userProfileImage_" + userId.ToString() +
                                               fileExtension;
                            isImageSaved =
                                ImageHelper.SaveJpeg(
                                    filePath + "\\UserImages\\userProfileImage_" + userId.ToString() + fileExtension,
                                    (Bitmap) i, 75);
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

        #region <update> Personal&Member&Home&Work Info and save new email

        protected void BtnUpdatePersonalInfoClick(object sender, EventArgs e)
        {
            try
            {
                var user = _entities.Users.First(p => p.Id == Member.Users.Id);
                var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == Member.Users.Id);
                if (userGeneral == null)
                {
                    MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
                    return;
                }

                Member.Users.Name = tbEditName.Text;
                Member.Users.Surname = tbEditSurname.Text;

                Member.UserGeneral.TC = tbEditTC.Text;
                Member.UserGeneral.FatherName = tbEditFatherName.Text;
                Member.UserGeneral.MotherName = tbEditMotherName.Text;
                Member.UserGeneral.MaidenName = tbEditMaidenName.Text;
                Member.UserGeneral.Birthplace = tbEditBirthPlace.Text;
                Member.UserGeneral.Hobby = tbEditHobbies.Text;
                Member.UserGeneral.Web = tbEditWeb.Text;
                Member.UserGeneral.GsmNo = tbEditGsmNo.Text;
                Member.UserGeneral.LastSchool = tbEditLastSchool.Text;
                Member.UserGeneral.MemberFoundation = tbEditMemberFoundation.Text;
                if (!string.IsNullOrEmpty(tbEditParola.Text)) user.Password = Crypto.Encrypt(tbEditName.Text);

                string photoUrl = SaveUserPhoto(Member.Users.Id);
                if (!string.IsNullOrWhiteSpace(photoUrl))
                {
                    hdnUserProfilPhotoUrl.Value = photoUrl;
                    Member.UserGeneral.PhotoUrl = photoUrl;
                }
                else
                {
                    hdnUserProfilPhotoUrl.Value = string.Empty;
                }

                Member.UserGeneral.MarriageDate = dpEditMarriageDate.SelectedDate != null &&
                                                  dpEditMarriageDate.SelectedDate.Value != null
                                                      ? dpEditMarriageDate.SelectedDate
                                                      : null;
                Member.UserGeneral.Birthdate = dpEditBirthDate.SelectedDate != null &&
                                               dpEditBirthDate.SelectedDate.Value != null
                                                   ? dpEditBirthDate.SelectedDate
                                                   : null;

                if (dpEditLastSchoolGraduateDate.SelectedDate != null)
                    userGeneral.LastSchoolGraduateDate =
                        Convert.ToDateTime("01.01." + dpEditLastSchoolGraduateDate.SelectedDate.Value.Year.ToString());


                Member.UserGeneral.Gender = ddlEditGender.SelectedIndex > 0
                                                ? Convert.ToInt32(ddlEditGender.SelectedItem.Value)
                                                : 0;
                Member.UserGeneral.BloodType = ddlEditBloodType.SelectedIndex > 0
                                                   ? Convert.ToInt32(ddlEditBloodType.SelectedItem.Value)
                                                   : 0;
                Member.UserGeneral.MaritalStatus = ddlEditMaritalStatus.SelectedIndex > 0
                                                       ? Convert.ToInt32(ddlEditMaritalStatus.SelectedItem.Value)
                                                       : 0;
                Member.UserGeneral.Decease = cbMemberDecease.Checked;
                if (cbMemberDecease.Checked)
                {
                    Member.UserGeneral.Decease = true;
                    if ((dpMemberDeceaseDate.SelectedDate != null && dpMemberDeceaseDate.SelectedDate.Value != null))
                    {
                        Member.UserGeneral.DeceaseDate =
                            Convert.ToDateTime(dpMemberDeceaseDate.SelectedDate.Value.ToShortDateString());
                    }
                    else
                    {
                        Member.UserGeneral.DeceaseDate = null;
                    }
                }
                else
                {
                    Member.UserGeneral.Decease = false;
                    Member.UserGeneral.DeceaseDate = null;
                }
                if (Member.Users.Id > 0)
                {
                    Member.UserGeneral.UserId = Member.Users.Id;
                }

                _entities.Users.ApplyCurrentValues(Member.Users);
                _entities.UserGeneral.ApplyCurrentValues(Member.UserGeneral);

                _entities.SaveChanges();
                BindAll();
                ShowPersInfo();

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void BtnUpdateMemberInfoClick(object sender, EventArgs e)
        {
            try
            {
                var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == Member.Users.Id);
                var userFinance = _entities.UserFinance.FirstOrDefault(p => p.UserId == Member.Users.Id);
                var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == Member.Users.Id);

                if (userFoundation == null || userGeneral == null)
                {
                    MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
                    return;
                }
                userGeneral.AdminNote = tbAdminNote.Text;

                if (dpEditMembershipDate.SelectedDate != null)
                    userFoundation.MembershipDate =
                        Convert.ToDateTime(string.Format("01.01.{0}",
                                                         dpEditMembershipDate.SelectedDate.Value.Year.ToString()));

                userFoundation.MemberNo = tbEditMembershipNumber.Text;
                userFoundation.MemberRelType = ddlEditMemberRelationType.SelectedIndex > 0
                                                   ? Convert.ToInt32(ddlEditMemberRelationType.SelectedItem.Value)
                                                   : 0;
                userFoundation.SpecialNo = tbEditSpecialNumber.Text;
                userFoundation.Term = dpEditTerm.SelectedDate != null
                                          ? dpEditTerm.SelectedDate.Value.Year.ToString()
                                          : string.Empty;

                if (userFoundation.UserId == 0)
                {
                    userFoundation.UserId = Member.Users.Id;
                    _entities.AddToUserFoundation(userFoundation);
                }

                if (userFinance != null)
                {
                    userFinance.AutoPay = cbEditAutoPaymentOrder.Checked;
                    _entities.UserFinance.ApplyCurrentValues(userFinance);
                }

                _entities.UserFoundation.ApplyCurrentValues(userFoundation);
                _entities.SaveChanges();


                BindAll();
                ShowMemberInfo();
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void BtnUpdateHomeInfoClick(object sender, EventArgs e)
        {
            try
            {
                var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == Member.Users.Id);
                if (userGeneral == null)
                {
                    MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
                    return;
                }
                userGeneral.HomeCountry = ddlEditHomeCountry.SelectedIndex > 0
                                              ? (int?) Convert.ToInt32(ddlEditHomeCountry.SelectedItem.Value)
                                              : null;
                userGeneral.HomeCity = ddlEditHomeCity.SelectedIndex > 0
                                           ? (int?) Convert.ToInt32(ddlEditHomeCity.SelectedItem.Value)
                                           : null;
                userGeneral.HomeTown = ddlEditHomeTown.SelectedIndex > 0
                                           ? (int?) Convert.ToInt32(ddlEditHomeTown.SelectedItem.Value)
                                           : null;

                userGeneral.HomeAddress = tbEditHomeAddress.Text;
                userGeneral.HomeZipCode = tbEditHomeZipCode.Text;
                userGeneral.HomePhone = tbEditHomePhone.Text;
                userGeneral.HidePersonalInfo = cbEditHidePersonalInfo.Checked;

                _entities.UserGeneral.ApplyCurrentValues(userGeneral);
                _entities.SaveChanges();
                BindAll();
                ShowHomeInfo();
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void BtnUpdateWorkInfoClick(object sender, EventArgs e)
        {
            try
            {
                var userGeneral = _entities.UserGeneral.FirstOrDefault(p => p.UserId == Member.Users.Id);
                if (userGeneral == null)
                {
                    MessageBox.Show(MessageType.Error, AdminResource.lbErrorOccurred);
                    return;
                }
                userGeneral.WorkCountry = ddlEditWorkCountry.SelectedIndex == 0
                                              ? null
                                              : (ddlEditWorkCountry.SelectedIndex > 0
                                                     ? (int?) Convert.ToInt32(ddlEditWorkCountry.SelectedItem.Value)
                                                     : null);

                userGeneral.WorkCity = ddlEditWorkCity.SelectedIndex == 0
                                           ? null
                                           : (ddlEditWorkCity.SelectedIndex > 0
                                                  ? (int?) Convert.ToInt32(ddlEditWorkCity.SelectedItem.Value)
                                                  : null);

                userGeneral.WorkTown = ddlEditWorkTown.SelectedIndex == 0
                                           ? null
                                           : (ddlEditWorkTown.SelectedIndex > 0
                                                  ? (int?) Convert.ToInt32(ddlEditWorkTown.SelectedItem.Value)
                                                  : null);

                userGeneral.JobSectorNo = ddlEditJobSectors.SelectedIndex == 0
                                              ? null
                                              : (ddlEditJobSectors.SelectedIndex > 0
                                                     ? (int?) Convert.ToInt32(ddlEditJobSectors.SelectedItem.Value)
                                                     : null);
                userGeneral.JobNo = ddlEditJobs.SelectedIndex == 0
                                        ? null
                                        : (ddlEditJobs.SelectedIndex > 0
                                               ? (int?) Convert.ToInt32(ddlEditJobs.SelectedItem.Value)
                                               : null);

                userGeneral.WorkAddress = tbEditWorkAddress.Text;
                userGeneral.WorkZipCode = tbEditWorkZipCode.Text;
                userGeneral.WorkPhone = tbEditWorkPhone.Text;
                userGeneral.WorkFax = tbEditWorkFax.Text;
                userGeneral.WorkTitle = tbEditWorkTitle.Text;
                userGeneral.WorkCorporation = tbEditWorkCorparation.Text;
                userGeneral.HideJobInfo = cbEditHideJobInfo.Checked;

                _entities.UserGeneral.ApplyCurrentValues(userGeneral);
                _entities.SaveChanges();
                Member.UserGeneral = userGeneral;
                BindAll();
                ShowWorkInfo();
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

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
                                                            "haveAlreadyEmai2",
                                                            "<script> showUserInfo('dEmailAddress');alert('" +
                                                            AdminResource.lbMailAddressTakenAlready + "');</script>");
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
                        BindEmailInfo();
                        Page.ClientScript.RegisterStartupScript(GetType(),
                                                                "haveAlreadyEmai2",
                                                                "<script> showUserInfo('dEmailAddress');</script>");
                        /*
                         *  
                         var activationMailResult = EnrollMembershipHelper.SendEmailActivationMail(Member.Users.Id, newEmailAddress.Id,
                            "../App_Themes/mainTheme/mailtemplates/MemberActivationMailContent.htm");*/

                        tbNewEmailAddress.Text = string.Empty;
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #endregion
    }
}