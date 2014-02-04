using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_ChangePassword : UserControl
{
    private readonly Entities ent = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbSettings;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblError.Text = string.Empty;
            tbEmaiAdress.Text = KullaniciVer();
            RegularExpressionValidator1.ErrorMessage = AdminResource.errMsgWrongEMailType;

            int langIndex = 0;
            foreach (ListItem item in ddlAdminLanguage.Items)
            {
                if (Convert.ToInt32(item.Value) == EnrollAdminContext.Current.AdminLanguage.LanguageId)
                    ddlAdminLanguage.SelectedIndex = langIndex;
                langIndex++;
            }
        }

        btnChangePassword.Text = AdminResource.lbChange;
        btnChangeEmail.Text = AdminResource.lbChange;
        tbName.Text = KullaniciAdi();
        tbSurname.Text = KullaniciSoyadıAdi();
    }

    public void ChangeCulture(int lang)
    {
        var adminLanguage = ent.System_language.FirstOrDefault(p => p.languageId == lang);
        if (adminLanguage != null)
        {
            var culture = adminLanguage.languageCulture;
            EnrollAdminContext.Current.AdminLanguage.LanguageId = lang;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }
    }

    private string KullaniciVer()
    {
        var kullanici = string.Empty;

        try
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        var username = HttpContext.Current.User.Identity.Name;
                        //var cust = ent.System_customer.First(p => p.username == username);
                        //kullanici = cust.customerCode;
                        var cust = ent.Users.First(p => p.EMail == username);
                        kullanici = cust.EMail;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return kullanici;
    }

    private string KullaniciAdiVer()
    {
        var kullaniciAdi = string.Empty;
        try
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        string username = HttpContext.Current.User.Identity.Name;
                        //var cust = ent.System_customer.First(p => p.username == username);
                        //kullaniciAdi = cust.username;
                        var cust = ent.Users.First(p => p.EMail == username);
                        kullaniciAdi = cust.EMail;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return kullaniciAdi;
    }

    public string KullaniciAdi()
    {
        var kullanici = string.Empty;
        try
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        string username = HttpContext.Current.User.Identity.Name;
                        var cust = ent.Users.First(p => p.EMail == username);
                        kullanici = cust.Name;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return kullanici;
    }

    public string KullaniciSoyadıAdi()
    {
        var kullanici = string.Empty;
        try
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        string username = HttpContext.Current.User.Identity.Name;
                        var cust = ent.Users.First(p => p.EMail == username);
                        kullanici = cust.Surname;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return kullanici;
    }


    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        string username = KullaniciAdiVer();
        var users = ent.Users.First(p => p.EMail == username);
        if (Crypto.Encrypt(txtOldPassword.Text) == users.Password)
        {
            if (txtNew.Text.Length > 3)
            {
                if (txtNew.Text == txtReNew.Text)
                {
                    try
                    {
                        users.Password = Crypto.Encrypt(txtNew.Text);
                        ent.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgPwdChangeSucces);
                        lblError.Text = string.Empty;
                    }
                    catch (Exception exception)
                    {
                        ExceptionManager.ManageException(exception);
                    }
                }
                else
                {
                    lblError.Text = AdminResource.msgWrongPwd;
                }
            }
            else
            {
                lblError.Text = AdminResource.msgPwdLength;
            }
        }
        else
        {
            lblError.Text = AdminResource.msgOldPwdErr;
        }
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
        int langId = Convert.ToInt32(ddlAdminLanguage.SelectedValue);
        ChangeCulture(langId);
        Response.Redirect("Content.aspx?content=CustomerManagement");
    }

    protected void btChangeName_Click(object sender, EventArgs e)
    {
        bool emailChanged = false;
        var kullanici = string.Empty;
        try
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        string username = HttpContext.Current.User.Identity.Name;

                        var cust = ent.Users.First(p => p.EMail == username);
                        cust.Name = tbName.Text;
                        cust.Surname = tbSurname.Text;
                        string tbEmail = tbEmaiAdress.Text.Replace("'", "").Replace("/", "").Replace("\\", "");
                        if (cust.EMail != tbEmail) emailChanged = true;

                        cust.EMail = tbEmail;

                        ent.SaveChanges();
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);

                        if (emailChanged)
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
                        }

                        lblError.Text = string.Empty;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        if (emailChanged)
        {
            Response.Redirect("~/Login.aspx");
        }
    }
}