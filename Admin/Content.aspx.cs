using System;
using System.Web.UI;
using Enroll.Managers;

namespace eNroll.Admin
{
    public partial class Content : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var controlName = Request.QueryString["content"];
                var tempControl = LoadControl("adminUserControls/" + controlName + ".ascx");
                PlaceHolder1.Controls.Add(tempControl);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }
}