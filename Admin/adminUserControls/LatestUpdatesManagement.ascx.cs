using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class LatestUpdatesManagement : UserControl
    {
        private readonly Entities _entities = new Entities();

        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbLatestUpdatesManagement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ImageButtonKaydet.Text = AdminResource.lbSave;

            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 22))
            {
                var selectedModules = _entities.LatestUpdates.Select(p => p.ModuleId).ToList();
                var firstOrDefault = _entities.LatestUpdates.FirstOrDefault();
                if (firstOrDefault != null)
                    tbMaxCount.Text = firstOrDefault.MaxCount.ToString();
                MultiView2.ActiveViewIndex = 0;
                MultiView1.ActiveViewIndex = 0;
                var yaList = from p in _entities.AuthAreas
                             where p.InLatestUpdates == true
                             orderby p.AuthArea
                             select p;
                cbModuleList.Items.Clear();
                foreach (var authArease in yaList)
                {
                    var item = new ListItem();
                    item.Value = authArease.Id.ToString();
                    item.Text = Logger.GetModul(authArease.Id, 0, 0).ModulName;

                    if (selectedModules.Contains(authArease.Id))
                        item.Selected = true;


                    cbModuleList.Items.Add(item);
                }
            }
            else
            {
                MultiView2.ActiveViewIndex = 1;
            }
        }

        protected void ImageButtonKaydet_Click(object sender, EventArgs e)
        {
            var latestUpdatesList = _entities.LatestUpdates.ToList();
            foreach (LatestUpdates item in latestUpdatesList)
            {
                _entities.LatestUpdates.DeleteObject(item);
                _entities.SaveChanges();
            }
            foreach (ListItem li in cbModuleList.Items)
            {
                if (li.Selected)
                {
                    var updates = new LatestUpdates();
                    updates.ModuleId = Convert.ToInt32(li.Value);
                    updates.MaxCount = Convert.ToInt32(tbMaxCount.Text);
                    _entities.AddToLatestUpdates(updates);
                    _entities.SaveChanges();
                }
            }
            Logger.Add(22, 0, 0, 3);
            MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
        }
    }
}