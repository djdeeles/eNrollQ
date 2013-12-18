using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_Login : Page
{
    //bu cookie ile login bilgileri tutulur
    public static string CookieName = "EnrollAuthentication";
    private readonly Entities _oEntities = new Entities();
    public int LoginFailedCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //login panelinden çıkış linki ile "sing out"
        if (HttpContext.Current.Request.QueryString.Count > 0 && HttpContext.Current.Request.QueryString["process"] != null)
        {
            var process = HttpContext.Current.Request.QueryString["process"];
            if (process == "0")
            {
                var cookie = Response.Cookies["EnrollAuthentication"];
                if (cookie != null) cookie.Expires = DateTime.Now.AddMinutes(-1);

                cookie = Response.Cookies["EnrollAdminLanguage"];
                if (cookie != null) cookie.Expires = DateTime.Now.AddMinutes(-1);
                cookie = Response.Cookies["EnrollDataLanguage"];
                if (cookie != null) cookie.Expires = DateTime.Now.AddMinutes(-1);

                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Default.aspx");
            }
        }

        //Ip Filtreleme kontrolü
        IpFilterControl();

        var httpCookie = HttpContext.Current.Request.Cookies[CookieName];
        if (httpCookie != null && Convert.ToInt32(Crypto.Decrypt(httpCookie["errorCount"])) > 2)
        {
            RadCaptcha1.Style.Clear();
            RadCaptcha1.Enabled = true;
            RadCaptcha1.Visible = true;
            RadCaptcha1.CaptchaTextBoxCssClass = "CaptchaTextBox";
            RadCaptcha1.EnableEmbeddedBaseStylesheet = false;
        }
        else
        {
            RadCaptcha1.Enabled = false;
            RadCaptcha1.Visible = false;
        }

        if (!IsPostBack) // sayfa cookie aracılığıyla login olma işlemini geçebilir
        {
            SetComboBoxSelectedAdminLanguage();

            // kullanıcı daha önceden login olarak "beni hatırla" seçeneğini aktif hale getirdiyse
            if (LoginContext.CookieControl(LoginType.Admin))
            {
                LoginProccess(LoginContext.GetUsername(), LoginContext.GetPassword(), true);
            }

            ChangeCulture(EnrollAdminContext.Current.AdminLanguage.LanguageId);
            MultiView1.ActiveViewIndex = 0;
        }
        else // login olma işlemi
        {
            int langId = Convert.ToInt32(ddlAdminLanguage.SelectedItem.Value);
            hfSelectedLanguageId.Value = langId.ToString();
            ChangeCulture(langId);
        }

        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = _oEntities.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title;
        MetaGenerate.SetMetaTags(site, Page);

        RadCaptcha1.CaptchaTextBoxLabel = AdminResource.lbCapthcaMessage;
        cbRememberMe.Text = AdminResource.lbRememberMe;
        btnBack.Text = Resource.lbBack;
        btnSendPwd.Text = Resource.lbSend;
        btnForgetPwd.Text = Resource.lbLostPassWord;
        btnLogin.Text = Resource.lbLogin;
        btnBack.Text = Resource.lbBack;
    }

    #region IP filtreleme kontrolü

    private void IpFilterControl()
    {
        var login = false;
        var siteGeneralInfo = _oEntities.SiteGeneralInfo.First();
        if (siteGeneralInfo.IpFilter > 0)
        {
            string userIp = ExceptionManager.GetUserIp();

            #region Black List kontrolü
            login = true;
            if (siteGeneralInfo.IpFilter == 1)
            {
                var ipFilterList = _oEntities.IpFilterList.Where(p => p.BlackList && p.State).Select(p => p.IpAddress).ToList();
                foreach (var item in ipFilterList)
                {
                    var ipAddress = ParseIpAddress(item);
                    if (userIp.Contains(ipAddress))
                    {
                        login = false;
                    }
                }
                if (!login) Response.Redirect("Default.aspx");
                return;
            }
            #endregion

            #region White List kontrolü
            login = false;
            if (siteGeneralInfo.IpFilter == 2)
            {
                var ipFilterList = _oEntities.IpFilterList.Where(p => p.BlackList == false && p.State).Select(p => p.IpAddress).ToList();
                foreach (var item in ipFilterList)
                {
                    var ipAddress = ParseIpAddress(item);
                    ipAddress = ipAddress.TrimEnd('.');
                    if (userIp.Contains(ipAddress))
                    {
                        login = true;
                        break;
                    }
                }
                if (!login) Response.Redirect("Default.aspx");
                return;
            }
            
            #endregion

        }
    }
    private string ParseIpAddress(string item)
    {
        string[] data = item.Split('.');
        string ipAddress = string.Empty;
        foreach (var s in data)
        {
            if (s == "*")
                break;
            ipAddress += s + ".";
        }

        ipAddress = ipAddress.TrimEnd('.');

        return ipAddress;
    }

    #endregion

    protected void BtnWebLoginClick(object sender, EventArgs eventArgs)
    {
        try
        {
            var cookie = HttpContext.Current.Request.Cookies[CookieName];
            string encryptedPwd = Crypto.Encrypt(TextBoxPassword.Text);
            var oSystemCustomer = _oEntities.Users.Where(x => x.EMail == TextBoxUserName.Text && x.Password == encryptedPwd && x.State && x.Admin.Value).ToList();

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
                Page.ClientScript.RegisterStartupScript(GetType(), "vvv",
                    string.Format("<script> alert('{0}');</script>", AdminResource.lbUserInformationNotValid));
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void ChangeCulture(int lang)
    {
        var entities = new Entities();
        var adminLanguage = entities.System_language.FirstOrDefault(p => p.languageId == lang);
        if (adminLanguage != null)
        {
            var culture = adminLanguage.languageCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }
    }

    private void LoginProccess(String userName, String encryptedPassword, bool isCookieAvaliable)
    {
        LoginContext.CookieName = CookieName;
        LoginContext.LoginProccess(userName, encryptedPassword, isCookieAvaliable, ddlAdminLanguage.SelectedValue, cbRememberMe.Checked, LoginType.Admin);
        Response.Redirect(LoginContext.GetRetrunUrl(LoginType.Admin), false);
    }

    private bool KullaniciVarMi(string kullaniciAdi)
    {
        bool durum = false;
        try
        {
            var kulanici = (from p in _oEntities.Users
                            where p.EMail == kullaniciAdi
                            select p).FirstOrDefault();
            if (kulanici != null && !string.IsNullOrEmpty(kulanici.Name))
            {
                durum = true;
            }
            else
            {
                durum = false;
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
        string result;
        if (KullaniciVarMi(TextBox1UserName.Text))
        {
            try
            {
                result = EnrollMembershipHelper.SendForgetPasswordMail(TextBox1UserName.Text,
                                                              "App_Themes/mainTheme/mailtemplates/LoginMailContent.htm");
                Page.ClientScript.RegisterStartupScript(GetType(), "dsadas2", "<script> alert('" + result + "');</script>");
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                Page.ClientScript.RegisterStartupScript(GetType(), "vvv2",
                                                        "<script> alert('" + AdminResource.lbErrorOccurred +
                                                        "');</script>");
            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "vvvv", "<script> alert('" + AdminResource.lbUserInformationNotValid + ".');</script>");
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
    }

    protected void ddlAdminLanguage_Changed(object sender, EventArgs e)
    {
        int langIndex = 0;
        foreach (ListItem item in ddlAdminLanguage.Items)
        {
            if (item.Value == hfSelectedLanguageId.Value)
                ddlAdminLanguage.SelectedIndex = langIndex;
            langIndex++;
        }
    }

    private void SetComboBoxSelectedAdminLanguage()
    {
        int langIndex = 0;
        foreach (ListItem item in ddlAdminLanguage.Items)
        {
            if (item.Value == EnrollAdminContext.Current.AdminLanguage.LanguageId.ToString())
                ddlAdminLanguage.SelectedIndex = langIndex;
            langIndex++;
        }
    }
}