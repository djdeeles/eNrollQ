using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

namespace eNroll.Helpers
{
    public enum Proccess
    {
        Admin = 1,
        Web = 2
    }
    enum DbTable
    {
        User = 1,
        UserGeneral = 2,
        UserFoundation = 3,
        UserFinance = 4,
        Corporation = 5,
        CorporationAddress = 6

    }
    public class MemberInfo
    {
        public Users Users;
        public UserGeneral UserGeneral;
        public UserEmails UserEmails;
        public UserFoundation UserFoundation;
        public UserFinance UserFinance;
        public MemberInfo()
        {
            Users = new Users();
            UserGeneral = new UserGeneral();
            UserEmails = new UserEmails();
            UserFoundation = new UserFoundation();
            UserFinance = new UserFinance();
        }
    }
     
    public class EnrollMembershipHelper
    {
        #region bind User for edit & GetMembers type of List<Users>
        public static MemberInfo BindUser(MemberInfo memberInfo)
        {
            var entities = new Entities();
            Users user = null;
            UserGeneral userGeneral = null;
            UserEmails userEmail = null;
            UserFoundation userFoundation = null;
            UserFinance userFinance = null;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                user = entities.Users.FirstOrDefault(p => p.EMail == HttpContext.Current.User.Identity.Name);
                userGeneral = entities.UserGeneral.FirstOrDefault(p => p.UserId == user.Id);
                userEmail = entities.UserEmails.FirstOrDefault(p => p.UserId == user.Id);
                userFoundation = entities.UserFoundation.FirstOrDefault(p => p.UserId == user.Id);
                userFinance = entities.UserFinance.FirstOrDefault(p => p.UserId == user.Id);
            }
            else
            {
                HttpContext.Current.Response.Redirect("giris.aspx?ReturnUrl=~/profil.aspx");
            }

            if (user != null)
            {
                if (userGeneral == null)
                {
                    userGeneral = new UserGeneral { UserId = user.Id };
                }
                if (userEmail == null)
                {
                    userEmail = new UserEmails { UserId = user.Id };
                }
                if (userFoundation == null)
                {
                    userFoundation = new UserFoundation { UserId = user.Id };
                }
                if (userFinance == null)
                {
                    userFinance = new UserFinance { UserId = user.Id };
                }
            }


            memberInfo.Users = user;
            memberInfo.UserGeneral = userGeneral;
            memberInfo.UserEmails = userEmail;
            memberInfo.UserFoundation = userFoundation;
            memberInfo.UserFinance = userFinance;

            return memberInfo;
        }
        public static MemberInfo BindUser(MemberInfo memberInfo, int userId)
        {
            var entities = new Entities();
            try
            {
                if (userId > 0)
                {
                    var user = entities.Users.FirstOrDefault(p => p.Id == userId);
                    var userGeneral = entities.UserGeneral.FirstOrDefault(p => p.UserId == userId);
                    var userEmail = entities.UserEmails.FirstOrDefault(p => p.UserId == userId);
                    var userFoundation = entities.UserFoundation.FirstOrDefault(p => p.UserId == userId);
                    var userFinance = entities.UserFinance.FirstOrDefault(p => p.UserId == userId);

                    if (user != null)
                    {

                        if (userGeneral == null)
                        {
                            userGeneral = new UserGeneral { UserId = user.Id };
                        }
                        if (userEmail == null)
                        {
                            userEmail = new UserEmails { UserId = user.Id };
                        }
                        if (userFoundation == null)
                        {
                            userFoundation = new UserFoundation { UserId = user.Id };
                        }
                        if (userFinance == null)
                        {
                            userFinance = new UserFinance { UserId = user.Id };
                        }

                        memberInfo.Users = user;
                        memberInfo.UserGeneral = userGeneral;
                        memberInfo.UserEmails = userEmail;
                        memberInfo.UserFoundation = userFoundation;
                        memberInfo.UserFinance = userFinance;
                        return memberInfo;
                    }
                }

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return null;
        } 
        public static List<Users> GetMembers(SqlConnection conn, string membersSqlQuery)
        {
            var entities = new Entities();
            List<Users> users = null;
            if (conn != null)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(membersSqlQuery))
                    {
                        var cmdSearchResault = new SqlCommand
                        {
                            Connection = conn,
                            CommandText = membersSqlQuery
                        };

                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                        var members = new DataTable();
                        oAdaptor.Fill(members);
                        if (members.Rows.Count > 0) users = new List<Users>();
                        foreach (DataRow user in members.Rows)
                        {
                            var userId = 0;
                            var id = user["uId"].ToString();
                            if (!string.IsNullOrWhiteSpace(id)) userId = Convert.ToInt32(id);
                            if (userId > 0) if (users != null) users.Add(entities.Users.First(p => p.Id == userId));
                        }
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
                finally
                {
                    conn.Close();
                }
            }
            return users;
        } 

        #endregion

        #region get countries cities towns
        public static List<Countries> GetCountries()
        {
            Entities _entities = new Entities();
            List<Countries> listCountry = _entities.Countries.OrderBy(p => p.Name).ToList();
            return listCountry;
        }
        public static List<Cities> GetCities(string countryIdStr)
        {
            Entities _entities = new Entities();
            List<Cities> listCity = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(countryIdStr))
                {
                    int countryId = Convert.ToInt32(countryIdStr);
                    listCity = _entities.Cities.Where(p => p.CountryId == countryId).OrderBy(p => p.Name).ToList();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

            return listCity;
        }
        public static List<Towns> GetTowns(string countryIdStr, string cityIdStr)
        {
            Entities _entities = new Entities();
            List<Towns> listTowns = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(countryIdStr) && !string.IsNullOrWhiteSpace(cityIdStr))
                {
                    int countryId = Convert.ToInt32(countryIdStr);
                    int cityId = Convert.ToInt32(cityIdStr);
                    listTowns = _entities.Towns.Where(p => p.CountryId == countryId && p.CityId == cityId).OrderBy(p => p.Name).ToList();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return listTowns;
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
            dropDownList.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            ddlCity.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            ddlTown.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
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
            dropDownList.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
        }
        #endregion

        #region bind country&city&town, blood types, jobs, sectors, gender, martial status, membershipRelationTypes
        public static void DataBindDdlGender(MemberInfo memberInfo, DropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Insert(0, new ListItem(Resources.AdminResource.lbChoose, ""));
            dropDownList.Items.Insert(1, new ListItem(Resources.AdminResource.lbMan, "1"));
            dropDownList.Items.Insert(2, new ListItem(Resources.AdminResource.lbWoman, "2"));

            if (memberInfo.UserGeneral.Gender != null && Convert.ToInt32(memberInfo.UserGeneral.Gender.Value) > 0)
            {
                dropDownList.SelectedIndex = Convert.ToInt32(memberInfo.UserGeneral.Gender.Value);
            }

        }
        public static void DataBindDDlBloodType(MemberInfo memberInfo, DropDownList dropDownList)
        {
            var entities = new Entities();
            var bloodTypes = entities.BloodTypes.ToList();
            dropDownList.DataSource = bloodTypes;
            dropDownList.DataTextField = "Name";
            dropDownList.DataValueField = "Id";
            dropDownList.DataBind();
            dropDownList.Items.Insert(0, new ListItem(Resources.Resource.lbChoose, ""));

            if (memberInfo != null && (memberInfo.UserGeneral.BloodType != null && memberInfo.UserGeneral.BloodType.Value > 0))
            {
                var i = 0;
                foreach (ListItem bloodType in dropDownList.Items)
                {
                    if (bloodType.Value == memberInfo.UserGeneral.BloodType.Value.ToString())
                    {
                        dropDownList.SelectedIndex = i;
                    }
                    i++;
                }
            }
        }
        public static void DataBindDdlMaritalStatus(MemberInfo memberInfo, DropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Insert(0, new ListItem(Resource.lbChoose, ""));
            dropDownList.Items.Insert(1, new ListItem(Resource.lbMarried, "1"));
            dropDownList.Items.Insert(2, new ListItem(Resource.lbSingle, "2"));

            if (memberInfo.UserGeneral.MaritalStatus != null)
            {
                dropDownList.SelectedIndex = Convert.ToInt32(memberInfo.UserGeneral.MaritalStatus.Value);
            }

        }
        public static void DataBindDDlSectorsJobs(MemberInfo memberInfo, DropDownList ddlSectors, DropDownList ddlJobs)
        {
            var entities = new Entities();

            var jobs = entities.Jobs.OrderBy(p => p.Name).ToList();
            var jobSectors = entities.JobSectors.OrderBy(p => p.Name).ToList();

            ddlJobs.DataSource = jobs;
            ddlJobs.DataTextField = "Name";
            ddlJobs.DataValueField = "Id";
            ddlJobs.DataBind();
            ddlJobs.Items.Insert(0, new ListItem(Resources.Resource.lbChoose, ""));


            ddlSectors.DataSource = jobSectors;
            ddlSectors.DataTextField = "Name";
            ddlSectors.DataValueField = "Id";
            ddlSectors.DataBind();
            ddlSectors.Items.Insert(0, new ListItem(Resources.Resource.lbChoose, ""));

            if (memberInfo != null)
            {
                if (memberInfo.UserGeneral.JobSectorNo != null && memberInfo.UserGeneral.JobSectorNo > 0)
                {
                    for (int i = 0; i < ddlSectors.Items.Count; i++)
                    {
                        if (ddlSectors.Items[i].Value == memberInfo.UserGeneral.JobSectorNo.Value.ToString())
                        {
                            ddlSectors.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else  ddlSectors.SelectedIndex = 0;
                
                if (memberInfo.UserGeneral.JobNo != null && memberInfo.UserGeneral.JobNo > 0)
                {
                    for (int i = 0; i < ddlJobs.Items.Count; i++)
                    {
                        if (ddlJobs.Items[i].Value == memberInfo.UserGeneral.JobNo.Value.ToString())
                        {
                            ddlJobs.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else ddlJobs.SelectedIndex = 0;

            } 
        }
        public static void DataBindDdlDecease(DropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            dropDownList.Items.Insert(1, new ListItem(AdminResource.lbYes, "1"));
            dropDownList.Items.Insert(2, new ListItem(AdminResource.lbNo, "0"));
        }
        public static void DataBindDdlMembershipRelType(DropDownList dropDownList)
        {
            var entities = new Entities();
            var foundationRelTypes = entities.FoundationRelType.Where(p => p.State).OrderBy(p => p.Name).ToList();
            dropDownList.DataSource = foundationRelTypes;
            dropDownList.DataTextField = "Name";
            dropDownList.DataValueField = "Id";
            dropDownList.DataBind();
            dropDownList.Items.Insert(0, new ListItem(Resources.Resource.lbChoose, ""));
        }
        public static void DataBindIsAdmin(DropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            dropDownList.Items.Insert(1, new ListItem(AdminResource.lbAdmin, "1"));
            dropDownList.Items.Insert(2, new ListItem(AdminResource.lbMember, "0")); 
        }
        public static void DataBindIsAutoPay(DropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            dropDownList.Items.Insert(1, new ListItem(AdminResource.lbYes, "1"));
            dropDownList.Items.Insert(2, new ListItem(AdminResource.lbNo, "0"));
        }
        public static MemberInfo BindCountryCityTown(MemberInfo memberInfo, DropDownList ddlCountry, DropDownList ddlCity, DropDownList ddlTown)
        {
            BindDdlCountries(ddlCountry, ddlCity, ddlTown, GetCountries());
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
                BindDdlCities(ddlCity, ddlTown, GetCities(memberInfo.UserGeneral.HomeCountry.Value.ToString()));
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

                    BindDdlTowns(ddlTown, GetTowns(memberInfo.UserGeneral.HomeCountry.Value.ToString(),
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

        #endregion

        #region Create User Emails List html table 

        public static string CreateUserEmailsInfoTableProfile(MemberInfo memberInfo, List<UserEmails> userEmailList)
        {
            var sb = new StringBuilder();
            try
            {
                //CheckCulture();
                sb.Append(CreateUserEmailForEdit(memberInfo, userEmailList, Proccess.Web));
                sb.Append(CreateUserEmailForView(memberInfo, userEmailList));
                return sb.ToString();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                return AdminResource.lbErrorOccurred;
            }
        }
        public static string CreateUserEmailsInfoTableAdmin(MemberInfo memberInfo, List<UserEmails> userEmailList)
        {

            var sb = new StringBuilder();
            try
            {
                //CheckCulture();
                sb.Append(CreateUserEmailForEdit(memberInfo, userEmailList, Proccess.Admin));
                return sb.ToString();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                return AdminResource.lbErrorOccurred;
            }
        }
        public static string CreateUserEmailForEdit(MemberInfo memberInfo, List<UserEmails> userEmailList, Proccess proccess)
        {

            var sb = new StringBuilder();
            try
            {
                sb.AppendFormat("<table class='editEmailInfo'><thead>" +
                              "<th min-width='170px'>{0}</th>" +
                              "<th width='150px' align='center'>{1}</th>" +
                              "<th width='180px'  align='center'>{2}</th>" +
                              "<th width='250px' ></th></thead><tbody>",
                              string.Empty,
                              AdminResource.lbMainEmail,
                              AdminResource.lbReceiveEmail);
                foreach (var email in userEmailList)
                {
                    var strActivateYourEmail = (email.Activated
                                                       ? ""
                                                       : (string.Format(
                                                           "&nbsp<span id='activateMail{0}{1}'>" +
                                                           "<input type='button' onclick='onayMailiGonder(\"{0}\",\"{1}\");return false;'" +
                                                           " style='cursor:pointer;' class='{3}' value='{2}'/></span>",
                                                           Crypto.Encrypt(email.UserId.ToString()),
                                                           Crypto.Encrypt(email.Id.ToString()), AdminResource.lbResendActivationMail,
                                                           proccess == Proccess.Admin ? "SaveCancelBtn" : "button")));

                    var strAllowMailing = (!email.Activated
                                                  ? "disabled='disabled'"
                                                  : (email.AllowMailing
                                                         ? string.Format(
                                                             " onclick='changeMailing(\"{0}\", \"{1}\",\"0\",\"{2}\");return false;' ",
                                                             Crypto.Encrypt(email.UserId.ToString()),
                                                             Crypto.Encrypt(email.Id.ToString()),
                                                             "changeMailing" + Crypto.Encrypt(email.Id.ToString()))
                                                         : string.Format(
                                                             " onclick='changeMailing(\"{0}\", \"{1}\",\"1\",\"{2}\");return false;' ",
                                                             Crypto.Encrypt(email.UserId.ToString()),
                                                             Crypto.Encrypt(email.Id.ToString()),
                                                             "changeMailing" + Crypto.Encrypt(email.Id.ToString()))));

                    var strChangeActiveMailAddress = (!email.Activated
                                                          ? "disabled='disabled'"
                                                          : (email.MainAddress
                                                                 ? ""
                                                                 : string.Format(
                                                                     " onclick='changeActiveEmail(\"{0}\", \"{1}\",\"{2}\");" +
                                                                     "return false;' ",
                                                                     Crypto.Encrypt(email.UserId.ToString()),
                                                                     Crypto.Encrypt(email.Id.ToString()),
                                                                     "checkActiveMail" +
                                                                     Crypto.Encrypt(email.Id.ToString()))));
                    var strDeleteEmail = (email.MainAddress
                                              ? ""
                                              : string.Format(
                                                  "<input type='button' onclick='deleteEmail(\"{0}\",\"{1}\");return false;' " +
                                                  "style='cursor:pointer;' value='{2}' class='{3}'/>",
                                                  Crypto.Encrypt(memberInfo.Users.Id.ToString()),
                                                  Crypto.Encrypt(email.Id.ToString()),
                                                  AdminResource.lbDelete,
                                                  proccess == Proccess.Admin ? "SaveCancelBtn" : "button"));

                    sb.AppendFormat("<tr><td>{0}</td>" +
                                    "<td align='center'><input type='checkbox' {1} {2} {3} id='{4}'/></td>" +
                                    "<td align='center'><input type='checkbox' {5} {6}/></td>" +
                                    "<td align='left' style='margin-left:20px;'>{7} {8}</td></tr>",
                                        email.Email,
                                        (email.MainAddress ? "checked" : ""),
                                        strChangeActiveMailAddress,
                                        ((email.Activated && email.MainAddress) ? "disabled='disabled'" : ""),
                                        "checkActiveMail" + Crypto.Encrypt(email.Id.ToString()),
                                        (email.AllowMailing ? "checked" : ""),
                                        strAllowMailing,
                                        strDeleteEmail,
                                        strActivateYourEmail);

                }
                sb.Append("</tbody></table>");

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                return AdminResource.lbErrorOccurred;
            }
            return sb.ToString();
        }
        public static string CreateUserEmailForView(MemberInfo memberInfo, List<UserEmails> userEmailList)
        {
            var sb = new StringBuilder();
            try
            {
                sb.AppendFormat("<table class='viewEmailInfo'><thead>" +
                              "<th min-width='170px'>{0}</th>" +
                              "<th width='150px' align='center'>{1}</th>" +
                              "<th width='180px'  align='center'>{2}</th>" +
                              "<th width='250px' ></th></thead><tbody>",
                              string.Empty,
                              AdminResource.lbMainEmail,
                              AdminResource.lbReceiveEmail);
                foreach (var email in userEmailList)
                {
                    sb.AppendFormat("<tr>" +
                                    "<td>{0}</td>" +
                                    "<td align='center'><input type='checkbox' disabled='disabled' {1}/></td>" +
                                    "<td align='center'><input type='checkbox' disabled='disabled' {2}/></td>" +
                                    "</tr>",
                                        email.Email,
                                        (email.MainAddress ? "checked" : ""),
                                        (email.AllowMailing ? "checked" : ""));

                }
                sb.Append("</tbody></table>");
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return sb.ToString();
        }

        #endregion

        public static string SendEmailActivationMail(int userId, int emailId, string memberActivationMailContentUrl)
        {
            //CheckCulture();
            var e = new Entities();
            var result = string.Empty;
            try
            {

                var memberActivationLink = "{0}/MemberActivation.aspx?code={1}";
                var user = e.Users.FirstOrDefault(p => p.Id == userId);
                if (user != null)
                {
                    #region sendMail

                    var email = user.EMail;

                    var dilId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                    var siteGeneralInfo = e.SiteGeneralInfo.FirstOrDefault(p => p.languageId == dilId);
                    if (siteGeneralInfo != null)
                    {
                        string baslik = siteGeneralInfo.title;
                        var md = new MailDefinition();
                        var replacements = new ListDictionary();
                        if (user.Id > 0)
                            replacements.Add("%%KullaniciAdi%%", email);

                        replacements.Add("%%Parola%%", Crypto.Decrypt(user.Password));

                        memberActivationLink = string.Format(memberActivationLink,
                                                                HttpContext.Current.Request.Url.Authority,
                                                                Crypto.Encrypt(email));
                        replacements.Add("%%ActivationLink%%", memberActivationLink);

                        md.IsBodyHtml = true;
                        md.BodyFileName = memberActivationMailContentUrl;
                        md.Subject = baslik;
                        md.From = md.From = "noreply@" + HttpContext.Current.Request.Url.Host;
                        string c = email;
                        var msg = md.CreateMailMessage(c, replacements, new System.Web.UI.Control());
                        msg.BodyEncoding = Encoding.Default;
                        msg.SubjectEncoding = Encoding.Default;
                        msg.Priority = MailPriority.High;
                        var smtp = new SmtpClient("localhost", 25);
                        smtp.Send(msg);
                        result = AdminResource.msgAccountActivationEMailSent;
                        MessageBox.Show(MessageType.Notice, result);
                    }
                    #endregion
                }
                else
                {
                    result = AdminResource.msgNoMemberFound; 
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                result = AdminResource.lbErrorOccurred;
            }
            return result;
        }


        public static string SendForgetPasswordMail(string userName, string memberActivationMailContentUrl)
        {
            string result = string.Empty;
            //CheckCulture();
            var e = new Entities();
            try
            {
                int dilId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                SiteGeneralInfo siteGeneralInfo = e.SiteGeneralInfo.FirstOrDefault(p => p.languageId == dilId);
                if (siteGeneralInfo != null)
                {
                    string baslik = siteGeneralInfo.title;
                    var kullanici = e.Users.Where(p => p.EMail == userName).ToList();
                    var md = new MailDefinition();
                    var replacements = new ListDictionary();
                    var customer = kullanici.FirstOrDefault();
                    if (customer != null)
                    {
                        replacements.Add("%%KullaniciAdi%%", customer.EMail);
                        var users = kullanici.FirstOrDefault();
                        if (users != null)
                            replacements.Add("%%Parola%%", Crypto.Decrypt(users.Password));
                        md.IsBodyHtml = true;
                        md.BodyFileName = "App_Themes/mainTheme/mailtemplates/LoginMailContent.htm";
                        md.Subject = baslik;
                        md.From = md.From = "noreply@" + HttpContext.Current.Request.Url.Host;
                        string c = customer.EMail;

                        var msg = md.CreateMailMessage(c, replacements, new System.Web.UI.Control());
                        msg.BodyEncoding = Encoding.Default;
                        msg.SubjectEncoding = Encoding.Default;
                        msg.Priority = MailPriority.High;
                        var smtp = new SmtpClient("localhost", 25);
                        smtp.Send(msg);
                        result = AdminResource.msgSentEMailInfo;
                    }
                    else
                    {
                        result = AdminResource.lbUserInformationNotValid;
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


        #region kullanıcının emailleri bulunur & kullanıcıların excel olarak indirebilmesi için gerekli datatable oluşturulur

        public static DataTable CreateUsersDataTable()
        {
            var xlsUsersDataTable = new DataTable();
            DataColumn newColumn;
            try
            {

                // ad    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbName };
                xlsUsersDataTable.Columns.Add(newColumn);

                // soyad    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbSurname };
                xlsUsersDataTable.Columns.Add(newColumn);

                // main email    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMainEmail };
                xlsUsersDataTable.Columns.Add(newColumn);

                // other emails
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbOtherEmail };
                xlsUsersDataTable.Columns.Add(newColumn);

                // cell number
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbGsmNo };
                xlsUsersDataTable.Columns.Add(newColumn);

                // web    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWeb };
                xlsUsersDataTable.Columns.Add(newColumn);

                // durum=>aktif/pasif
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbState };
                xlsUsersDataTable.Columns.Add(newColumn);

                // admin=>1/0
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbAdmin };
                xlsUsersDataTable.Columns.Add(newColumn);

                // TC
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "TC" };
                xlsUsersDataTable.Columns.Add(newColumn);

                // gender=>erkek/kadın
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbGender };
                xlsUsersDataTable.Columns.Add(newColumn);

                // bloodtype
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbBloodType };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Father Name
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbFatherName };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MotherName
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMotherName };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MartialStatus
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMartialStatus };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MarriageDate
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMarriageDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Birthdate
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbBirthDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Hobby
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbHobby };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Decease
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbDecease };
                xlsUsersDataTable.Columns.Add(newColumn);

                // DeceaseDate
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbDeceaseDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // LastSchool
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbLastSchool };
                xlsUsersDataTable.Columns.Add(newColumn);

                // LastSchoolGraduateDate
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbLastSchoolGraduateDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // HomePhone
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbHomePhone };
                xlsUsersDataTable.Columns.Add(newColumn);

                // HomeAddress
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbHomeAddress };
                xlsUsersDataTable.Columns.Add(newColumn);

                // HomeCountry
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbHomeCountry };
                xlsUsersDataTable.Columns.Add(newColumn);

                // HomeCity
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbHomeCity };
                xlsUsersDataTable.Columns.Add(newColumn);

                // HomeTown
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbHomeTown };
                xlsUsersDataTable.Columns.Add(newColumn);

                // HomeZipCode
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbHomeZipCode };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MemberFoundation
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMemberFoundation };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkPhone
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkPhone };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkAddress
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkAddress };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkCountry
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkCountry };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkCity
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkCity };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkTown
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkTown };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkZipCode
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkZipCode };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkFax
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkFax };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkCorporation
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkCorporation };
                xlsUsersDataTable.Columns.Add(newColumn);

                // WorkTitle
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbWorkTitle };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Job
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbJob };
                xlsUsersDataTable.Columns.Add(newColumn);

                // JobSector
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbJobSector };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MemberNo
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMemberNo };
                xlsUsersDataTable.Columns.Add(newColumn);

                // SpecialNo
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbSpecialNo };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MemberRelType
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMemberRelType };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MemberState
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMemberState };
                xlsUsersDataTable.Columns.Add(newColumn);

                // MembershipDate
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbMembershipDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Term
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbTerm };
                xlsUsersDataTable.Columns.Add(newColumn);

                // TermLeader
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbTermLeader };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Vip
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbVip };
                xlsUsersDataTable.Columns.Add(newColumn);

                // AdminNote
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbAdminNote};
                xlsUsersDataTable.Columns.Add(newColumn);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return xlsUsersDataTable;
        }

        public static DataTable CreateCorporationsDataTable()
        {
            var xlsCorporationsDataTable = new DataTable();
            DataColumn newColumn;
            try
            { 
                // ad    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbName };
                xlsCorporationsDataTable.Columns.Add(newColumn);

                // açıklama
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbDesc };
                xlsCorporationsDataTable.Columns.Add(newColumn);
                
                // kurumdaki kişi sayısı
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbUserCount };
                xlsCorporationsDataTable.Columns.Add(newColumn);
                 
                // Vergi Dairesi
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbTaxDept };
                xlsCorporationsDataTable.Columns.Add(newColumn);
                 
                // Vergi Numarası
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbTaxNumber };
                xlsCorporationsDataTable.Columns.Add(newColumn);
                
                // İletişim Adresi
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbContactAddress };
                xlsCorporationsDataTable.Columns.Add(newColumn);
                 
                // Fatura Adresi
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbInvoiceAddress };
                xlsCorporationsDataTable.Columns.Add(newColumn);
                 
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return xlsCorporationsDataTable;
        }
        public static DataTable CreateScheduledJobsDataTable()
        {
            var xlsUsersDataTable = new DataTable();
            DataColumn newColumn;
            try
            {

                // ad    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbName };
                xlsUsersDataTable.Columns.Add(newColumn);

                // soyad    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbSurname };
                xlsUsersDataTable.Columns.Add(newColumn);

                // email    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbEmail };
                xlsUsersDataTable.Columns.Add(newColumn);

                // state    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbState };
                xlsUsersDataTable.Columns.Add(newColumn);

                // date  
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbDate };
                xlsUsersDataTable.Columns.Add(newColumn);

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return xlsUsersDataTable;
        }

        public static DataTable CreateFinanceDetailDataTable()
        {
            var xlsUsersDataTable = new DataTable();
            DataColumn newColumn;
            try
            {
                // Dues Type    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbDuesType };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Payment Type    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbPaymentType };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Amount
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbAmount };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Receipt Invoice Number    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbReceiptInvoiceNumber };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Receipt Invoice Date    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbReceiptInvoiceDate };
                xlsUsersDataTable.Columns.Add(newColumn);
                
                // payment date  
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbPaymentDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // process
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbProcess };
                xlsUsersDataTable.Columns.Add(newColumn);
                
                // ProcessUser
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbProcessUser };
                xlsUsersDataTable.Columns.Add(newColumn);
                
                // process date  
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbProcessDate };
                xlsUsersDataTable.Columns.Add(newColumn);

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return xlsUsersDataTable;
        }

        public static DataTable CreateCorporationFinanceDetailDataTable()
        {
            var xlsUsersDataTable = new DataTable();
            DataColumn newColumn;
            try
            { 
                // Payment Type    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbPaymentType };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Amount
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbAmount };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Receipt Invoice Number    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbReceiptInvoiceNumber };
                xlsUsersDataTable.Columns.Add(newColumn);

                // Receipt Invoice Date    
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbReceiptInvoiceDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // payment date  
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbPaymentDate };
                xlsUsersDataTable.Columns.Add(newColumn);

                // process
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbProcess };
                xlsUsersDataTable.Columns.Add(newColumn);

                // ProcessUser
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbProcessUser };
                xlsUsersDataTable.Columns.Add(newColumn);

                // process date  
                newColumn = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = AdminResource.lbProcessDate };
                xlsUsersDataTable.Columns.Add(newColumn);

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return xlsUsersDataTable;
        }

        public static string GetUserEmails(int userId)
        {
            var e = new Entities();
            var userEmails = string.Empty;
            var emails = e.UserEmails.Where(p => p.UserId == userId).ToList();

            userEmails = emails.Aggregate(userEmails, (current, email) => current + (email.Email + ";"));
            return userEmails.TrimEnd(';');
        }
        #endregion
           
        public enum MembershipType
        {
            Aktif = 1,
            Pasif = 2,
            OnayBekleyen = 3,
            Hatali = 4
        }

        public static MembershipType UserMembershipState(string userIdStr)
        {
            var _entities = new Entities();
            MembershipType membershipType = 0;
            if (!string.IsNullOrEmpty(userIdStr))
            {
                var userId = Convert.ToInt32(userIdStr);
                if (userId > 0)
                {
                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    var userEmail = _entities.UserEmails.FirstOrDefault(p => p.UserId == userId && p.MainAddress);
                    var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == userId);
                    if (userFoundation != null)
                    {
                        if (user != null && user.State)
                        {
                            membershipType = MembershipType.Aktif;
                        }
                        else
                        {
                            if (userFoundation.MemberState)
                            {
                                membershipType = MembershipType.Pasif;
                            }
                            else
                            {
                                if (userEmail != null && userEmail.Activated)
                                {
                                    membershipType = MembershipType.OnayBekleyen;
                                }
                                else
                                {
                                    membershipType = MembershipType.Hatali;
                                }
                            }
                        }
                    }
                }
            }
            return membershipType;
        }

        public static void BindDDlMailTemplates(DropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
            var entities = new Entities();
            var mailTemplates = entities.MailTemplates.OrderBy(p => p.Title).ToList();
            foreach (var mailTemplate in mailTemplates)
            {
                dropDownList.Items.Add(new ListItem(mailTemplate.Title, mailTemplate.Id.ToString()));
            }
        }

        public static void BindRbSendReport(RadioButtonList radioButton)
        {
            radioButton.Items.Clear();
            radioButton.Items.Insert(0, new ListItem(AdminResource.lbWantTo, "1"));
            radioButton.Items.Insert(1, new ListItem(AdminResource.lbDontWantTo, "0"));
            radioButton.SelectedIndex = 0;
        }

        public static object GetUserMainEmail(int userId)
        {

            var e = new Entities();
            var emails = e.UserEmails.SingleOrDefault(p => p.UserId == userId && p.MainAddress);

            return emails != null ? emails.Email : null;
        }

        public static object GetUserOtherEmails(int userId)
        {
            var e = new Entities();
            var userEmails = string.Empty;
            var emails = e.UserEmails.Where(p => p.UserId == userId && p.MainAddress==false).ToList();

            userEmails = emails.Aggregate(userEmails, (current, email) => current + (email.Email + ";"));
            return userEmails.TrimEnd(';');
        }

        public static string DebtValue(decimal debt)
        {
            var debtValue = string.Format("<span style='color:green;'> {0}0{1}</span>",
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""),
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""));
            try
            {
                if (debt != 0)
                {
                    debtValue = string.Format("<span style='color:{0};'> {1}{2}{3}{4}</span>",
                                                        (debt < 0 ? "green" : "red"),
                                                        (debt < 0 ? "-" : ""),
                                                        (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                             ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                             : ""),
                                                        (debt < 0
                                                             ? (debt * -1).ToString("0.00")
                                                             : (debt).ToString("0.00")),
                                                        (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                             ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                             : ""));
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return debtValue;
        }

        public static string AmountValue(decimal debt, int paymentOrDebt) //1:payment, 0:debt
        {
            var color = (paymentOrDebt == 1 ? "green" : "red");
            var temp = (paymentOrDebt == 1 ? "-" : "");

            var debtValue = string.Format("<span style='color:{0};'> {1}0{2}</span>",
                                                color,
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""),
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""));
            try
            {
                if (debt != 0)
                {
                    debtValue = string.Format("<span style='color:{0};'> {1}{2}{3}{4}</span>",
                                                        color,
                                                        temp,
                                                        (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                             ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                             : ""),
                                                        (debt < 0
                                                             ? (debt * -1).ToString(".00")
                                                             : (debt).ToString(".00")),
                                                        (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                             ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                             : ""));
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return debtValue;
        }

        public static string AmountValueExcel(decimal debt, int paymentOrDebt) //1:payment, 0:debt
        { 
            var temp = (paymentOrDebt == 1 ? "-" : "");

            var debtValue = string.Format("{0}0{1}",
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""),
                                                (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                     : ""));
            try
            {
                if (debt != 0)
                {
                    debtValue = string.Format("{0}{1}{2}{3}",
                                                        temp,
                                                        (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                                                             ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                             : ""),
                                                        (debt < 0
                                                             ? (debt * -1).ToString(".00")
                                                             : (debt).ToString(".00")),
                                                        (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                                             ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                                             : ""));
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return debtValue;
        }
         
        public static string GetMembershipType(string strMembershipId)
        {
            var entities = new Entities();
            if (!string.IsNullOrEmpty(strMembershipId))
            {
                var membershipId = Convert.ToInt32(strMembershipId);
                var foundationRelType= entities.FoundationRelType.FirstOrDefault(p => p.Id == membershipId);
                return foundationRelType != null ? foundationRelType.Name : string.Empty;
            }
            return string.Empty;
        }

        public static void BindDdlMemberStatesFilter(DropDownList dropDownList)
        {
            //üye kontrolü => [User.State|UserFoundation.MemberState|UserEmails.Activated]
            dropDownList.Items.Clear();
            dropDownList.Items.Insert(0, new ListItem(AdminResource.lbAll, Crypto.Encrypt("")));
            
            dropDownList.Items.Insert(1, new ListItem(AdminResource.lbWaitingForApproval, Crypto.Encrypt("0|0|1"))); 
            dropDownList.Items.Insert(2, new ListItem(AdminResource.lbActive, Crypto.Encrypt("1|1|1"))); 
            dropDownList.Items.Insert(3, new ListItem(AdminResource.lbPassive, Crypto.Encrypt("0|1|1"))); 
            dropDownList.Items.Insert(4, new ListItem(AdminResource.lbInvalidMembers, Crypto.Encrypt("0|0|0"))); 
        }
         
        public static bool IsAuthForThisProcess(int editUserId, int loginUserId)
        {
            var roleAuthAreaCount = UserActiveAuthAreaIds(editUserId);
            var activeUserRoleAuthAreaCount = UserActiveAuthAreaIds(loginUserId);
            if (activeUserRoleAuthAreaCount.Count < roleAuthAreaCount.Count)
            {
                return false;
            }
            return true;
        }

        public static bool AreYouActiveUser(int id)
        {
            var entities = new Entities();
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var currentEmail = HttpContext.Current.User.Identity.Name;
                var user = entities.Users.FirstOrDefault(p => p.EMail == currentEmail && p.Id == id);
                if (user != null && user.Id > 0)
                    return true;
            }
            return false;
        }
        // kullanıcı yetki alanı Id leri
        public static List<int> UserActiveAuthAreaIds(int editUserId)
        {
            var entities = new Entities();
            var list = new List<int>();
            try
            {
                var userRole = entities.UserRole.FirstOrDefault(p => p.UserId == editUserId);
                if (userRole != null)
                {
                    int rolId = userRole.RoleId;
                    list = entities.RoleAuthAreas.Where(p => p.RoleId == rolId).Select(p => p.AuthAreaId).ToList();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return list;
        }

        public static int GetUserIdFromEmail(string email)
        {
            var entities = new Entities();
            var user = entities.Users.FirstOrDefault(p => p.EMail == email);
            return user != null ? user.Id : 0;
        }
    }

}