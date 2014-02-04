using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using Enroll.Managers;
using eNroll.App_Data;

namespace eNroll.Helpers
{
    public enum LoginType
    {
        Admin = 1,
        Web = 2
    }

    public class LoginContext
    {
        private static string _username;
        private static string _password;
        public static string CookieName = "EnrollAuthentication";
        public static string RedirectUrl;
        public static int LoginFailedCount;

        private static readonly Entities _entities = new Entities();

        #region Cookie ile login

        public static bool CookieControl(LoginType loginType)
        {
            var c = HttpContext.Current.Request.Cookies[CookieName]; // login bilgileri
            if (c != null && c["username"] != null && c["parola"] != null)
            {
                try
                {
                    List<Users> user = null;
                    var uname = Crypto.Decrypt(Convert.ToString(c["username"]));
                    var pwd = Convert.ToString(c["parola"]);
                    LoginFailedCount = Convert.ToInt32(Crypto.Decrypt(Convert.ToString(c["errorCount"])));

                    switch (loginType)
                    {
                        case LoginType.Admin:
                            user =
                                _entities.Users.Where(
                                    x => x.EMail == uname && x.Password == pwd && x.State && x.Admin.Value).ToList();
                            break;
                        case LoginType.Web:
                            user = _entities.Users.Where(x => x.EMail == uname && x.Password == pwd && x.State).ToList();
                            break;
                    }

                    if (user != null && user.Count > 0)
                    {
                        SetUsername(uname);
                        SetPassword(pwd);

                        return true;
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            return false;
        }

        public static Users CookieControl()
        {
            var c = HttpContext.Current.Request.Cookies[CookieName]; // login bilgileri
            if (c != null && c["username"] != null && c["parola"] != null)
            {
                try
                {
                    var uname = Crypto.Decrypt(Convert.ToString(c["username"]));
                    var pwd = Convert.ToString(c["parola"]);
                    LoginFailedCount = Convert.ToInt32(Crypto.Decrypt(Convert.ToString(c["errorCount"])));

                    var user = _entities.Users.Where(x => x.EMail == uname && x.Password == pwd && x.State).ToList();
                    if (user.Count > 0)
                    {
                        return user[0];
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            return null;
        }

        #endregion

        #region Login, crate formTicket, write cookie,

        public static void LoginProccess(String userName, String encryptedPassword, bool isCookieAvaliable,
                                         string adminLanguage, bool cbRememberMe, LoginType loginType)
        {
            try
            {
                CreateEnrollAuthenticationCookie(userName, encryptedPassword, cbRememberMe);
                CreateEnrollAdminLanguageCookie(adminLanguage);
                CreateFormsAuthenticationTicket(userName, loginType);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #endregion

        public static void CreateFormsAuthenticationTicket(string userName, LoginType loginType)
        {
            var roles = GetRoles(userName, loginType);
            FormsAuthentication.Initialize();
            var formsAuthenticationTicket = new FormsAuthenticationTicket(1,
                                                                          userName,
                                                                          DateTime.Now,
                                                                          DateTime.Now.AddDays(1),
                                                                          false,
                                                                          roles,
                                                                          FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                        FormsAuthentication.Encrypt(formsAuthenticationTicket));
            if (formsAuthenticationTicket.IsPersistent) cookie.Expires = formsAuthenticationTicket.Expiration;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void CreateEnrollAuthenticationCookie(string userName, string encryptedPassword, bool cbRememberMe)
        {
            //'string CookieName' değişkeninde cookie varsa, cookie yi değiştirmek için alıyoruz
            // yoksa, 'string CookieName' adında yeni cookie oluşturuyoruz
            var c = HttpContext.Current.Request.Cookies[CookieName];
            if (c == null || c["username"] == null) c = new HttpCookie(CookieName);
            if (cbRememberMe)
            {
                c["username"] = Crypto.Encrypt(userName);
                c["parola"] = encryptedPassword;
            }
            c["errorCount"] = Crypto.Encrypt("0");
            c.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(c);
        }

        public static void CreateEnrollAdminLanguageCookie(string adminLanguage)
        {
            var systemLanguage = _entities.System_language.FirstOrDefault(
                p => p.languageId == EnrollAdminContext.Current.AdminLanguage.LanguageId);

            if (systemLanguage != null)
            {
                var httpCookie = HttpContext.Current.Response.Cookies["EnrollAdminLanguage"];

                if (httpCookie != null)
                    httpCookie.Value = adminLanguage;

                var cultureName = systemLanguage.languageCulture;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            }
        }

        public static string GetRoles(string username, LoginType loginType)
        {
            var admin = _entities.Users.First(p => p.EMail == username).Admin;
            var isadmin = admin != null && admin.Value;
            var roles = isadmin ? "Admin" : "User";
            return roles;
        }

        public static string GetRetrunUrl(LoginType loginType)
        {
            string returnUrl = string.Empty;
            switch (loginType)
            {
                case LoginType.Web:
                    returnUrl = "Default.aspx";
                    break;
                case LoginType.Admin:
                    returnUrl = "Admin/Default.aspx";
                    break;
            }

            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                returnUrl = string.Format(HttpContext.Current.Request.QueryString.Get(0));
            }
            return returnUrl;
        }

        #region catcha hatalı giriş logu

        public static void LogPasswordErrorCount(HttpCookie cookie)
        {
            if (cookie == null || cookie["errorCount"] == null)
            {
                var c = new HttpCookie(CookieName);
                c["errorCount"] = Crypto.Encrypt("1");
                LoginFailedCount = 1;
                c.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Current.Response.Cookies.Add(c);
            }
            else
            {
                LoginFailedCount = Convert.ToInt32(Crypto.Decrypt(cookie["errorCount"])) + 1;
                cookie["errorCount"] = Crypto.Encrypt(LoginFailedCount.ToString());
                cookie.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        #endregion

        #region getter setter

        public static string GetUsername()
        {
            return _username;
        }

        public static void SetUsername(string username)
        {
            _username = username;
        }

        public static void SetPassword(string password)
        {
            _password = password;
        }

        public static string GetPassword()
        {
            return _password;
        }

        #endregion

        #region check admin role

        public static void CheckAdminUser()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var _user = _entities.Users.First(p => p.EMail == HttpContext.Current.User.Identity.Name);
                if (!Convert.ToBoolean(_user.Admin))
                {
                    HttpContext.Current.Response.Redirect("../Default.aspx");
                }
            }
        }

        #endregion
    }
}