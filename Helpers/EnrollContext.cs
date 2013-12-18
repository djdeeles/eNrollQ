using System;
using System.Linq;
using System.Web;
using eNroll.App_Data;

public class EnrollContext
{
    private readonly CookieeLanguage _workingLanguage;

    public EnrollContext()
    {
        _workingLanguage = new CookieeLanguage("EnrollWorkingLanguage");
    }

    public static EnrollContext Current
    {
        get
        {
            if (HttpContext.Current.Session["EnrollContext"] == null)
            {
                var enContext = new EnrollContext();
                HttpContext.Current.Session.Add("EnrollContext", enContext);
            }

            return (EnrollContext) HttpContext.Current.Session["EnrollContext"];
        }
    }

    public CookieeLanguage WorkingLanguage
    {
        get { return _workingLanguage; }
    }
}

public class EnrollAdminContext
{
    private readonly CookieeLanguage _adminLanguage;
    private readonly CookieeLanguage _dataLanguage;

    public EnrollAdminContext()
    {
        _dataLanguage = new CookieeLanguage("EnrollDataLanguage");
        _adminLanguage = new CookieeLanguage("EnrollAdminLanguage");
    }

    public static EnrollAdminContext Current
    {
        get
        {
            if (HttpContext.Current.Session["EnrollAdminContext"] == null)
            {
                var enAdminContext = new EnrollAdminContext();
                HttpContext.Current.Session.Add("EnrollAdminContext", enAdminContext);
            }
            return (EnrollAdminContext) HttpContext.Current.Session["EnrollAdminContext"];
        }
    }

    public CookieeLanguage DataLanguage
    {
        get { return _dataLanguage; }
    }

    public CookieeLanguage AdminLanguage
    {
        get { return _adminLanguage; }
    }
}

public class CookieeLanguage
{
    private readonly string _strcookieName;
    private int _intlanguageId;

    public CookieeLanguage(String cookieName)
    {
        _strcookieName = cookieName;
        if (CheckCookieNull())
        {
            CreateCookie();
        }
        else
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[_strcookieName];
            if (httpCookie != null)
                _intlanguageId = Convert.ToInt32(httpCookie.Value);
        }
    }

    public Int32 LanguageId
    {
        get { return _intlanguageId; }
        set
        {
            HttpCookie oCookie = HttpContext.Current.Request.Cookies[_strcookieName];
            if (oCookie != null)
            {
                oCookie.Expires = DateTime.Now.AddMonths(1);
                oCookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Set(oCookie);
                _intlanguageId = value;
            }
        }
    }

    private bool CheckCookieNull()
    {
        bool blnReturn = HttpContext.Current.Request.Cookies[_strcookieName] == null;

        return blnReturn;
    }

    private void CreateCookie()
    {
        var ent = new Entities();
        System_language systemLanguage = ent.System_language.FirstOrDefault(p => p.state == true);
        if (systemLanguage != null)
        {
            var oLang = systemLanguage.languageId;

            String strDefaultLanguageId = oLang.ToString();
            var oCookie = new HttpCookie(_strcookieName, strDefaultLanguageId);
            oCookie.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(oCookie);
            _intlanguageId = oLang;
        }
    }
}

