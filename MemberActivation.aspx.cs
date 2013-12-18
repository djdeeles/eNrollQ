using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll
{
    public partial class MemberActivation : System.Web.UI.Page
    {
        readonly Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString.Count>0 && Request.QueryString.Get("code")!=null)
            {
                CheckUser(Request.QueryString.Get("code"));
            }
            else
            {
                ltMessage.Text = Resources.Resource.msgMembershipNotAvaliable;
            }
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("MemberProfile.aspx");
            }
        }

        private void CheckUser(string encryptedCode)
        {
            string email = string.Empty;
            try
            {
                email = Crypto.Decrypt(encryptedCode);
                var userEmail = _entities.UserEmails.FirstOrDefault(p => p.Email == email);
                if (userEmail != null)
                {
                    if (!userEmail.Activated)
                    {
                        var user = _entities.Users.FirstOrDefault(p => p.Id == userEmail.UserId);
                        if (user!=null && user.Id > 0)
                        {
                            userEmail.Activated = true;
                            _entities.SaveChanges();
                            ltMessage.Text = string.Format(Resources.Resource.msgMembershipActivatedClickLogin, "/giris");
                        }
                    }
                    else
                    {
                        ltMessage.Text = string.Format(Resources.Resource.msgMembershipIsActivatedClickLogin, "giris");
                    }
                }
                else
                {
                    ltMessage.Text = Resources.Resource.msgMembershipNotAvaliable;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                ltMessage.Text = Resources.Resource.msgMembershipNotAvaliable;
            }
        }
    }
}