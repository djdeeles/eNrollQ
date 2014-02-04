using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.UserControls.cm
{
    public partial class MemberPanel : UserControl
    {
        private const string CookieName = "EnrollAuthentication";
        private readonly Entities _entitites = new Entities();
        protected LoginType LoginType = 0;
        protected string MemberImage = string.Empty;
        protected bool MemberIsLogin = false;
        protected string MemberName = Resource.lbGuest;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //cookie kontrol ediliyor
                var checkUser = LoginContext.CookieControl();
                if (checkUser != null)
                {
                    var userGeneral = _entitites.UserGeneral.FirstOrDefault(p => p.UserId == checkUser.Id);
                    LoginType = checkUser.Admin == true ? LoginType.Admin : LoginType.Web;
                    LoginContext.CookieName = CookieName;
                    LoginContext.LoginProccess(checkUser.EMail, checkUser.Password, true, "2", true, LoginType);
                    MemberName = checkUser.Name + " " + checkUser.Surname;
                    MemberImage = string.Format("<img alt='{0}' src='{1}' class='memberpanelimage'/>", MemberName,
                                                (userGeneral != null && userGeneral.PhotoUrl != null
                                                     ? userGeneral.PhotoUrl.Replace("~/", "")
                                                     : "/App_Themes/mainTheme/images/noimage.png"));
                    MemberIsLogin = true;
                }
            }
            else
            {
                LoginType = HttpContext.Current.User.IsInRole("admin") ? LoginType.Admin : LoginType.Web;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var username = HttpContext.Current.User.Identity.Name;
                    var user = _entitites.Users.FirstOrDefault(p => p.EMail == username);
                    var userGeneral = _entitites.UserGeneral.FirstOrDefault(p => p.UserId == user.Id);
                    if (user != null)
                    {
                        MemberName = user.Name + " " + user.Surname;
                        MemberIsLogin = true;
                    }
                    MemberImage = string.Format("<img alt='{0}' src='{1}' class='memberpanelimage'/>",
                                                MemberName, ((userGeneral != null && userGeneral.PhotoUrl != null)
                                                                 ? userGeneral.PhotoUrl.Replace("~/", "")
                                                                 : "/App_Themes/mainTheme/images/noimage.png"));
                }
            }
        }
    }
}