using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class RssManagement : UserControl
    {
        private readonly Entities ent = new Entities();

        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbRssManagement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hfLanguageId.Value = EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 20))
            {
                MultiView2.ActiveViewIndex = 0;
            }
            else
            {
                MultiView2.ActiveViewIndex = 1;
            }
            if (!IsPostBack)
            {
                MultiView1.SetActiveView(View1);
            }
            btnAddNew.Text = AdminResource.lbNewRss;
            btnSave.Text = AdminResource.lbSave;
            btnCancel.Text = AdminResource.lbCancel;

            cbRssState.Text = AdminResource.lbActive;
            cbRssStateEdit.Text = AdminResource.lbActive;

            gvRss.Columns[0].HeaderText = AdminResource.lbActions;
            gvRss.Columns[1].HeaderText = AdminResource.lbName;
            gvRss.Columns[2].HeaderText = AdminResource.lbRssUrl;
            gvRss.Columns[3].HeaderText = AdminResource.lbMaxItem;
            gvRss.Columns[4].HeaderText = AdminResource.lbScrollRssNames;
            gvRss.Columns[5].HeaderText = AdminResource.lbDate;
            gvRss.Columns[6].HeaderText = AdminResource.lbState;

            btnEditSave.Text = AdminResource.lbSave;
            btnEditCancel.Text = AdminResource.lbCancel;

            RssleriVer();
        }

        private void RssleriVer()
        {
            try
            {
                gvRss.DataSourceID = "EntityDataSourceRssManagement";
                gvRss.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs eventArgs)
        {
            MultiView1.SetActiveView(View2);
        }

        protected void btnSave_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                var rss = new App_Data.Rss();
                if (txtMaxItem.Text != "")
                {
                    rss.MaxItem = Convert.ToInt32(txtMaxItem.Text);
                }
                rss.Name = txtRss.Text;
                rss.Url = txtRssUrl.Text;
                rss.Language = EnrollAdminContext.Current.DataLanguage.LanguageId;
                rss.State = cbRssState.Checked;
                rss.IsScroll = cbScrollRssNames.Checked;
                rss.CreatedTime = DateTime.Now;
                rss.UpdatedTime = DateTime.Now;

                ent.AddToRss(rss);
                ent.SaveChanges();

                Logger.Add(20, 0, rss.Id, 1);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                RssleriVer();
                MultiView1.SetActiveView(View1);
                Temizle();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs eventArgs)
        {
            MultiView1.SetActiveView(View1);
            Temizle();
        }

        protected void gvRss_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            App_Data.Rss rss;
            int rssId = 0;
            if (e.CommandName == "Guncelle")
            {
                rssId = Convert.ToInt32(e.CommandArgument);
                rss = ent.Rss.First(p => p.Id == rssId);
                RssAlaniGuncelle(rss);
                MultiView1.SetActiveView(View3);
            }
            else if (e.CommandName == "Sil")
            {
                rssId = Convert.ToInt32(e.CommandArgument);
                rss = ent.Rss.First(p => p.Id == rssId);
                ent.DeleteObject(rss);
                ent.SaveChanges();
                Logger.Add(20, 0, rssId, 2);
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                RssleriVer();
            }
        }

        private void RssAlaniGuncelle(App_Data.Rss rss)
        {
            try
            {
                Temizle();
                txtRssEdit.Text = rss.Name;
                txtRssUrlEdit.Text = rss.Url;
                if (rss.State != null) cbRssStateEdit.Checked = rss.State.Value;
                cbScrollRssNamesEdit.Checked = (bool) rss.IsScroll;
                if (rss.MaxItem != null) txtMaxItemEdit.Text = rss.MaxItem.Value.ToString();
                HiddenFieldBannerId.Value = rss.Id.ToString();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        private void Temizle()
        {
            txtMaxItem.Text = string.Empty;
            txtMaxItemEdit.Text = string.Empty;
            txtRss.Text = string.Empty;
            txtRssEdit.Text = string.Empty;
            txtRssUrl.Text = string.Empty;
            txtRssUrlEdit.Text = string.Empty;
            cbRssState.Checked = false;
            cbRssStateEdit.Checked = false;
            HiddenFieldBannerId.Value = string.Empty;
        }

        protected void btnEditSave_Click(object sender, EventArgs eventArgs)
        {
            App_Data.Rss rss;
            int rssId = 0;
            if (!string.IsNullOrEmpty(HiddenFieldBannerId.Value))
            {
                try
                {
                    rssId = Convert.ToInt32(HiddenFieldBannerId.Value);
                    rss = ent.Rss.First(p => p.Id == rssId);
                    if (!string.IsNullOrEmpty(txtMaxItemEdit.Text))
                    {
                        rss.MaxItem = Convert.ToInt32(txtMaxItemEdit.Text);
                    }
                    else
                    {
                        rss.MaxItem = null;
                    }
                    rss.Name = txtRssEdit.Text;
                    rss.Url = txtRssUrlEdit.Text;
                    rss.Language = EnrollAdminContext.Current.DataLanguage.LanguageId;
                    rss.State = cbRssStateEdit.Checked;
                    rss.IsScroll = cbScrollRssNamesEdit.Checked;
                    rss.UpdatedTime = DateTime.Now;
                    ent.SaveChanges();
                    Logger.Add(20, 0, rssId, 3);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                RssleriVer();
                txtMaxItem.Text = string.Empty;
                MultiView1.SetActiveView(View1);
            }
        }

        protected void btnEditCancel_Click(object sender, EventArgs eventArgs)
        {
            MultiView1.SetActiveView(View1);
        }

        protected void gvRss_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var myMastDelete = (ImageButton) e.Row.FindControl("LinkButtonSil");
                    myMastDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }
}