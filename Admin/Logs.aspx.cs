using System;
using System.Web;
using System.Web.UI;
using Resources;

namespace eNroll.Admin
{
    public partial class Logs : Page
    {
        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbLogs;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 21))
            {
                mvAuth.ActiveViewIndex = 0;
            }
            else
            {
                mvAuth.ActiveViewIndex = 1;
            }

            GridView1.Columns[0].HeaderText = AdminResource.lbNumber;
            GridView1.Columns[1].HeaderText = AdminResource.lbOperation;
            GridView1.Columns[2].HeaderText = AdminResource.lbModule;
            GridView1.Columns[3].HeaderText = AdminResource.lbIcerik + " #";
            GridView1.Columns[4].HeaderText = AdminResource.lbUser;
            GridView1.Columns[5].HeaderText = AdminResource.lbIpAdress;
            GridView1.Columns[6].HeaderText = AdminResource.lbDate;

            GridView1.EmptyDataText = AdminResource.lbNoRecord;
        }
    }
}