using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_VideoManager : UserControl
{
    private readonly Entities _ent = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbVideoGalleryManagement;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 5))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }

        if (!IsPostBack)
        {
            try
            {
                HiddenField1.Value = EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }


        cbVideoState.Text = AdminResource.lbActive;
        videoState.Text = AdminResource.lbActive;

        btnNew.Text = AdminResource.lbNewCategory;
        btnNewVideo.Text = AdminResource.lbNewVideo;

        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;

        btnSaveVideo.Text = AdminResource.lbSave;
        btnCancelVideo.Text = AdminResource.lbCancel;

        gvCategories.Columns[0].HeaderText = AdminResource.lbActions;
        gvCategories.Columns[1].HeaderText = AdminResource.lbCategory;
        gvCategories.Columns[2].HeaderText = AdminResource.lbState;


        gvCategories.EmptyDataText = AdminResource.msgNotFoundCategory;

        gvVideos.Columns[0].HeaderText = AdminResource.lbActions;
        gvVideos.Columns[1].HeaderText = AdminResource.lbName;
        gvVideos.Columns[2].HeaderText = AdminResource.lbDesc;
        gvVideos.Columns[3].HeaderText = AdminResource.lbVideoUrl;
        gvVideos.Columns[4].HeaderText = AdminResource.lbCategory;
        gvVideos.Columns[5].HeaderText = AdminResource.lbState;

        gvVideos.EmptyDataText = AdminResource.msgNotFoundAlbum;
    }

    protected void btnNewCategory_Click(object sender, EventArgs eventArgs)
    {
        mvKat.SetActiveView(vvYeniKat);
    }

    protected void btnSave_Click(object sender, EventArgs eventArgs)
    {
        using (var ent = new Entities())
        {
            try
            {
                var vCat = new VideoCategories();
                vCat.name = txtNewCategory.Text;
                vCat.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                vCat.State = cbVideoState.Checked;
                vCat.CreatedTime = DateTime.Now;
                vCat.UpdatedTime = DateTime.Now;
                ent.AddToVideoCategories(vCat);
                ent.SaveChanges();

                Logger.Add(6, 1, vCat.id, 1);

                gvCategories.DataBind();
                txtNewCategory.Text = string.Empty;
                mvKat.SetActiveView(vvKatEkle);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e) // on imagebutton click
    {
        if (e.CommandName == "Update")
        {
            int id = int.Parse(e.CommandArgument.ToString());
            VideoCategories category = _ent.VideoCategories.First(p => p.id == id);
            category.UpdatedTime = DateTime.Now;
            _ent.SaveChanges();
            Logger.Add(6, 1, category.id, 3);

            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs eventArgs)
    {
        mvKat.SetActiveView(vvKatEkle);
        txtNewCategory.Text = string.Empty;
    }

    protected void gvCategories_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteBtn = (ImageButton) e.Row.FindControl("lbCatSil");
                if (deleteBtn != null)
                {
                    deleteBtn.OnClientClick =
                        " return confirm('" + AdminResource.lbConfirmDeleteVideoCategory + "'); ";
                    deleteBtn.ToolTip = AdminResource.lbDelete;
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void lbCatSil_Click(object sender, EventArgs e)
    {
        var lbCatSil = sender as ImageButton;
        if (lbCatSil != null)
        {
            var catId = Convert.ToInt32(lbCatSil.CommandArgument);
            using (var ent = new Entities())
            {
                try
                {
                    var cat = ent.VideoCategories.First(p => p.id == catId);
                    var albumler = ent.Videos.Where(p => p.categoryId == catId).ToList();
                    foreach (Videos album in albumler)
                    {
                        ent.DeleteObject(album);
                    }

                    Logger.Add(6, 1, cat.id, 2);

                    ent.DeleteObject(cat);
                    ent.SaveChanges();

                    gvCategories.DataBind();
                    gvVideos.DataBind();

                    if (ent.Videos.Count(p => p.categoryId == catId) == 0)
                    {
                        mvVideos.Visible = false;
                        gvVideos.Visible = false;
                    }
                    MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
        }
    }

    protected void lbCatSec_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["catId"] != null) ViewState.Remove("catId");
            var lbCatSec = (LinkButton) sender;
            int categoryId = Convert.ToInt32(lbCatSec.CommandArgument);
            hfSelectedCategory.Value = lbCatSec.CommandArgument;
            ViewState.Add("catId", categoryId);
            edsVideos.WhereParameters.Clear();
            edsVideos.WhereParameters.Add("catId", DbType.Int32, categoryId.ToString());
            gvVideos.SelectedIndex = 0;
            gvVideos.PageIndex = 0;
            gvVideos.DataBind();
            if (ViewState["albId"] != null) ViewState.Remove("albId");
            gvVideos.Visible = true;
            mvVideos.Visible = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ImageButton2_Click(object sender, EventArgs eventArgs)
    {
        mvVideos.SetActiveView(vvYeniVideo);
    }

    protected void btnSaveVideo_Click(object sender, EventArgs eventArgs)
    {
        using (var ent = new Entities())
        {
            try
            {
                var vid = new Videos();
                vid.Name = txtVideoName.Text;
                vid.categoryId = Convert.ToInt32(hfSelectedCategory.Value);
                vid.videoURL = txtLink.Text;
                vid.Description = txtVideoDescription.Text;
                vid.State = videoState.Checked;
                vid.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                vid.CreatedTime = DateTime.Now;
                vid.UpdatedTime = DateTime.Now;
                ent.AddToVideos(vid);
                ent.SaveChanges();

                Logger.Add(6, 2, vid.id, 1);

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                gvCategories.DataBind();
                if (hfSelectedCategory.Value != "")
                {
                    edsVideos.WhereParameters.Clear();
                    edsVideos.WhereParameters.Add("catId", DbType.Int32, hfSelectedCategory.Value);
                    edsVideos.DataBind();
                }
                mvVideos.SetActiveView(vvVideoEkle);
                txtVideoName.Text = string.Empty;
                txtLink.Text = string.Empty;
                txtVideoDescription.Text = string.Empty;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void btnCancelVideo_Click(object sender, EventArgs eventArgs)
    {
        mvVideos.SetActiveView(vvVideoEkle);
    }

    protected void lbAlbSec_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["albId"] != null)
                ViewState.Remove("albId");
            var lbAlbSec = (LinkButton) sender;
            var albId = Convert.ToInt32(lbAlbSec.CommandArgument);
            ViewState.Add("albId", albId);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void lbVideoSil_Click(object sender, EventArgs e)
    {
        try
        {
            var lbAlbSil = sender as ImageButton;
            int albId = Convert.ToInt32(lbAlbSil.CommandArgument);
            using (var ent = new Entities())
            {
                Videos alb = ent.Videos.First(p => p.id == albId);
                ent.DeleteObject(alb);
                ent.SaveChanges();
            }
            Logger.Add(6, 2, albId, 2);
            MessageBox.Show(MessageType.Success, AdminResource.lbMsgDeleteVideo);
            gvCategories.DataBind();
            edsVideos.WhereParameters.Clear();
            edsVideos.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
            gvVideos.DataBind();

            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void gvVideos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteBtn = (ImageButton) e.Row.FindControl("lbAlbSil");
                if (deleteBtn != null)
                {
                    deleteBtn.OnClientClick =
                        " return confirm('" + AdminResource.lbConfirmDeleteVideo + "')";
                    deleteBtn.ToolTip = AdminResource.lbDelete;
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void gvVideos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            int id = int.Parse(e.CommandArgument.ToString());
            Videos videos = _ent.Videos.First(p => p.id == id);
            videos.UpdatedTime = DateTime.Now;

            var v = (GridView) sender;
            var tbUrl = (TextBox) v.Rows[v.EditIndex].FindControl("tbEditUrl");
            videos.videoURL = tbUrl.Text;

            _ent.SaveChanges();

            Logger.Add(6, 2, videos.id, 3);

            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
        }

        gvCategories.DataBind();

        edsVideos.WhereParameters.Clear();
        edsVideos.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
        gvVideos.DataBind();
    }

    public string GetCategoryName(int id)
    {
        string catName = string.Empty;
        var cat = _ent.VideoCategories.FirstOrDefault(p => p.id == id);
        if (cat != null) catName = cat.name;
        return catName;
    }
}