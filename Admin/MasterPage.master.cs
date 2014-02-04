using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        private readonly Entities _entities = new Entities();

        protected override void OnInit(EventArgs e)
        {
            try
            {
                var userRoles = UserActiveAuthAreaIds();
                var ser = new JavaScriptSerializer();
                hfUserRoleAuthAreas.Value = Crypto.Encrypt(ser.Serialize(userRoles));

                var adminLanguage =
                    _entities.System_language.FirstOrDefault(
                        p => p.languageId == EnrollAdminContext.Current.AdminLanguage.LanguageId);
                if (adminLanguage != null)
                {
                    var culture = adminLanguage.languageCulture;

                    Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

            var user = _entities.Users.First(p => p.EMail == HttpContext.Current.User.Identity.Name);
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}, {1} {2}", AdminResource.lbWelcomeMessage, user.Name, user.Surname);
            ltWellcameMessage_Admin.Text = stringBuilder.ToString();

            btnExit.OnClientClick = " return confirm('" + AdminResource.lbConfirmMsgExit + "'); ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["currentPath"] != null &&
                        !(string.IsNullOrWhiteSpace(Session["currentPath"].ToString())))
                        lblLocation_Admin.Text = Session["currentPath"].ToString();
                    else
                        lblLocation_Admin.Text = string.Empty;

                    LabelSiteTitle.Text = GetSiteTitle(EnrollAdminContext.Current.DataLanguage.LanguageId);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
        }

        protected void btnCikis_Click_Admin(object sender, EventArgs e)
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

        protected string GetSiteTitle(int DilId)
        {
            string Title = string.Empty;
            try
            {
                var SiteTile = (from p in _entities.SiteGeneralInfo
                                where p.languageId == DilId
                                select new
                                           {
                                               p.title
                                           }).FirstOrDefault();
                if (SiteTile != null && !string.IsNullOrEmpty(SiteTile.title))
                {
                    Title = SiteTile.title;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            return Title;
        }

        public bool DisplayHelp()
        {
            try
            {
                var filePath = string.Format("{0}/help/{1}.html", Server.MapPath("."), GetHelpFileNameFromUrl());
                if (File.Exists(filePath))
                {
                    rWHelp.NavigateUrl = string.Format("help/{0}.html", GetHelpFileNameFromUrl());
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception, "MasterPage:DisplayHelp()");
                return false;
            }
        }

        public string GetHelpFileNameFromUrl()
        {
            string rawUrl = HttpContext.Current.Request.RawUrl;
            try
            {
                var data = rawUrl.Split('=');
                int length = data.Length;
                if (length > 1)
                {
                    var raw = data[1];
                    var paramss = raw.Split('&');
                    return paramss.Count() > 1 && !string.IsNullOrWhiteSpace(paramss[0]) ? paramss[0] : raw;
                }
                else
                {
                    data = rawUrl.Split('/');
                    if (data.Count() > 1)
                    {
                        var lastIndex = data.Length - 1;
                        var paramss = data[lastIndex].Split('.');
                        if (paramss.Count() > 1)
                            return paramss[0];
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception, "MasterPage:GetHelpFileNameFromUrl()");
            }

            return string.Empty;
        }


        public bool CheckAuth(int authId)
        {
            var ser = new JavaScriptSerializer();
            var decriptedHf = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(hfUserRoleAuthAreas.Value))
                {
                    var roles = UserActiveAuthAreaIds();
                    hfUserRoleAuthAreas.Value = ser.Serialize(roles);
                }
                decriptedHf = Crypto.Decrypt(hfUserRoleAuthAreas.Value);
                var userRoles = ser.Deserialize<List<int>>(decriptedHf);
                if (userRoles.Count == 0)
                {
                    return false;
                }
                return userRoles.Contains(authId);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception, "MasterPage:CheckAuth(int authId)");
            }
            return false;
        }

        // aktif kullanıcının yetki alanı Id leri
        public List<int> UserActiveAuthAreaIds()
        {
            var list = new List<int>();
            try
            {
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = _entities.Users.FirstOrDefault(p => p.EMail == HttpContext.Current.User.Identity.Name);
                    var userRole = _entities.UserRole.FirstOrDefault(p => p.UserId == user.Id);
                    if (userRole != null)
                    {
                        int rolId = userRole.RoleId;
                        var roleAuthAreas = _entities.RoleAuthAreas.Where(p => p.RoleId == rolId).ToList();
                        if (roleAuthAreas.Count > 0)
                        {
                            foreach (var roleAuthArea in roleAuthAreas)
                            {
                                list.Add(roleAuthArea.AuthAreaId);
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception, "MasterPage:UserActiveAuthAreaIds()");
            }

            return list;
        }
    }
}