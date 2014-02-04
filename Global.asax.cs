using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace eNroll
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        private void Application_End(object sender, EventArgs e)
        {
        }

        private void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError().GetBaseException();
            //ExceptionManager.ManageException(exception);

            var httpEx = exception as HttpException;
            if (httpEx != null && httpEx.GetHttpCode() == 404)
            {
                Response.Redirect("~/404.aspx");
            }
            /*
            else
                Response.Redirect("~/Error.html");*/
        }

        private void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Add("Id", HttpContext.Current.Session.SessionID);
        }

        private void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        var id = (FormsIdentity) HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        string userData = ticket.UserData;
                        var roles = userData.Split(',');
                        HttpContext.Current.User = new GenericPrincipal(id, roles);
                    }
                }
            }
        }
    }
}