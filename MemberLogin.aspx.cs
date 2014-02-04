using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll
{
    public partial class MemberLogin : Page
    {
        public static string CookieName = "EnrollAuthentication";
        private readonly Entities _entities = new Entities();
        public int LoginFailedCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains(HttpUtility.UrlEncode("/admin")))
            {
                string adminLoginUrl = "Login.aspx";
                try
                {
                    adminLoginUrl = HttpContext.Current.Request.RawUrl.Split('?')[1];
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }

                Response.Redirect("Login.aspx?" + adminLoginUrl);
            }

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Default.aspx");
            }

            var httpCookie = HttpContext.Current.Request.Cookies[CookieName];

            if (httpCookie != null && Convert.ToInt32(Crypto.Decrypt(httpCookie["errorCount"])) > 2)
            {
                RadCaptcha1.Style.Clear();
                RadCaptcha1.Enabled = true;
                RadCaptcha1.Visible = true;
                RadCaptcha1.TextBoxDecoration.CssClass = "CaptchaTextBox";
                RadCaptcha1.EnableEmbeddedBaseStylesheet = false;
            }
            else
            {
                RadCaptcha1.Enabled = false;
                RadCaptcha1.Visible = false;
            }

            if (!IsPostBack)
            {
                if (LoginContext.CookieControl(LoginType.Web))
                {
                    LoginProccess(LoginContext.GetUsername(), LoginContext.GetPassword(), true);
                }
            }

            string pageTitle = MetaGenerate.SetMetaTags(EnrollContext.Current.WorkingLanguage.LanguageId, Page);
            Page.Title = string.Format("{0} - {1}", pageTitle, Resource.lbNewMembership);

            RadCaptcha1.CaptchaTextBoxLabel = AdminResource.lbCapthcaMessage;
            cbRememberMe.Text = AdminResource.lbRememberMe;
            btnSendPwd.Text = Resource.lbSend;
            btnLogin.Text = Resource.lbLogin;
        }

        private void LoginProccess(String userName, String encryptedPassword, bool remembered)
        {
            LoginContext.CookieName = CookieName;
            LoginContext.LoginProccess(userName, encryptedPassword, remembered, "2", cbRememberMe.Checked, LoginType.Web);
            Response.Redirect(LoginContext.GetRetrunUrl(LoginType.Web), false);
        }

        protected void BtnLoginClick(object sender, EventArgs e)
        {
            try
            {
                var cookie = HttpContext.Current.Request.Cookies[CookieName];
                string encryptedPwd = Crypto.Encrypt(TextBoxPassword.Text);
                var oSystemCustomer =
                    _entities.Users.Where(x => x.EMail == TextBoxUserName.Text && x.Password == encryptedPwd && x.State)
                        .ToList();

                if (oSystemCustomer.Count > 0)
                {
                    if (RadCaptcha1.Enabled)
                    {
                        RadCaptcha1.Validate();
                        if (RadCaptcha1.IsValid)
                        {
                            LoginProccess(TextBoxUserName.Text, encryptedPwd, false);
                        }
                        else
                        {
                            LoginContext.LogPasswordErrorCount(cookie);
                            LoginFailedCount = LoginContext.LoginFailedCount;
                        }
                    }
                    else
                    {
                        LoginProccess(TextBoxUserName.Text, encryptedPwd, false);
                    }
                }
                else
                {
                    LoginContext.LogPasswordErrorCount(cookie);
                    LoginFailedCount = LoginContext.LoginFailedCount;
                    ltMemberLoginResult.Text = AdminResource.lbUserInformationNotValid;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        private bool KullaniciVarMi(string kullaniciAdi)
        {
            bool durum = false;
            try
            {
                var kulanici = (from p in _entities.Users
                                where p.EMail == kullaniciAdi
                                select p).FirstOrDefault();
                if (kulanici != null && !string.IsNullOrEmpty(kulanici.Name))
                {
                    durum = true;
                }
            }
            catch
            {
                durum = false;
            }
            return durum;
        }

        protected void BtnSendPwdClick(object sender, EventArgs eventArgs)
        {
            if (KullaniciVarMi(tbUserName_.Text))
            {
                try
                {
                    var result = EnrollMembershipHelper.SendForgetPasswordMail(tbUserName_.Text,
                                                                               "App_Themes/mainTheme/mailtemplates/LoginMailContent.htm");
                    Page.ClientScript.RegisterStartupScript(GetType(), "dsadas2",
                                                            "<script> alert('" + result + "');</script>");
                    ltSendNewPasswordResult.Text = result;
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    ltSendNewPasswordResult.Text = AdminResource.lbErrorOccurred;
                }
            }
            else
            {
                ltSendNewPasswordResult.Text = AdminResource.lbUserInformationNotValid;
            }
        }
    }
}