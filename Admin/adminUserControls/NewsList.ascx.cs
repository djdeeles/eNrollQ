using System;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_NewsList : UserControl
{
    private readonly Entities entities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbNewsManagement;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 3))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }

        if (!IsPostBack)
        {
            ListNews();
        }
        btnNew.Text = AdminResource.lbNewNews;
        GridView1.Columns[0].HeaderText = AdminResource.lbActions;
        GridView1.Columns[1].HeaderText = AdminResource.lbTitle;
        GridView1.Columns[2].HeaderText = AdminResource.lbDesc;
        GridView1.Columns[3].HeaderText = AdminResource.lbDate;
        GridView1.Columns[4].HeaderText = AdminResource.lbState;
        GridView1.Columns[5].HeaderText = AdminResource.lbHeadLine;


        chkState.Text = AdminResource.lbActive;
        imgBtnImageSelect.Text = AdminResource.lbImageSelect;
        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;
    }

    public void ListNews()
    {
        try
        {
            EntityDataSource1.Where = " it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
            EntityDataSource1.OrderBy = "it.enterDate desc";
            if (Request.Cookies["haberPage"] != null)
            {
                if (Request.Cookies["haberPage"]["sayfa"] != null)
                {
                    int c = Convert.ToInt32(Request.Cookies["haberPage"]["sayfa"]);
                    GridView1.PageIndex = c;
                    GridView1.DataBind();
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnDelete = (ImageButton) e.Row.FindControl("imgBtnDelete");
                btnDelete.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var myButton = (ImageButton) sender;
            int id = Convert.ToInt32(myButton.CommandArgument);
            var oEntities = new Entities();
            var oNews = oEntities.News.Where("it.newsId=" + id).FirstOrDefault();
            if (oNews != null && oNews.thumbnailPath != "")
            {
                ImageHelper.DeleteImage(Server.MapPath(oNews.thumbnailPath.Replace("~", "..")));
            }
            if (oNews != null)
            {
                Logger.Add(3, 0, oNews.newsId, 2);
                oEntities.DeleteObject(oNews);
            }
            oEntities.SaveChanges();

            GridView1.DataBind();
            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ImageButton2Ara_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var aranan = txtAra.Text;

            EntityDataSource1.Where = "it.System_language.languageId=" +
                                      EnrollAdminContext.Current.DataLanguage.LanguageId.ToString() +
                                      " and (it.header like '%" + aranan + "%' OR it.brief like '%" + aranan + "%')";
            EntityDataSource1.DataBind();
            GridView1.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnSaveUpdateNews(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(hdnId.Value)) //insert
            {
                var oNews = new News();
                SetDataProccess(oNews, false);

                oNews.System_language = entities.System_language.Where(
                    "it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId).FirstOrDefault();
                entities.SaveChanges();

                Logger.Add(3, 0, oNews.newsId, 1);

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            else //update
            {
                var oNews = entities.News.Where("it.newsId=" + hdnId.Value).FirstOrDefault();
                SetDataProccess(oNews, true);
                entities.SaveChanges();
                Logger.Add(3, 0, oNews.newsId, 3);

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }

            GridView1.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        pnlNewsEdit.Visible = false;
        pnlNewList.Visible = true;
        ClearFormInputs();
    }

    private void SetDataProccess(News news, bool deleteOldImage)
    {
        try
        {
            var entities = new Entities();
            var oDetailsFck = ((RadEditor) Rtb1.FindControl("RadEditor1"));
            news.header = txtHeader.Text;
            news.brief = txtBrief.Text;
            news.details = oDetailsFck.Content;
            news.manset = cbManset.Checked;
            news.state = chkState.Checked;
            if (!deleteOldImage)
                news.CreatedTime = DateTime.Now;
            news.UpdatedTime = DateTime.Now;
            if (dpDate.SelectedDate != null)
                news.enterDate = Convert.ToDateTime(dpDate.SelectedDate.Value.ToShortDateString());
            if (!String.IsNullOrEmpty(txtImage.Text))
            {
                if (deleteOldImage && news.thumbnailPath != string.Empty)
                    ImageHelper.DeleteImage(Server.MapPath(news.thumbnailPath.Replace("~", "..")));
                var orj = new Bitmap(Server.MapPath(txtImage.Text.Replace("~", "..")));
                var i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                var thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                var dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                ImageHelper.SaveJpeg(dest, (Bitmap) i, 75);
                news.thumbnailPath = "~/FileManager/thumbnails/" + thumbName;
                news.imagePath = txtImage.Text;
            }
            else
            {
                news.imagePath = string.Empty;
                news.thumbnailPath = string.Empty;
            }
            entities.SaveChanges();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnAddNews(object sender, EventArgs e)
    {
        pnlNewList.Visible = false;
        pnlNewsEdit.Visible = true;
    }

    protected void btnCancelSaveUpdateNews(object sender, EventArgs e)
    {
        pnlNewList.Visible = true;
        pnlNewsEdit.Visible = false;
        ClearFormInputs();
    }

    protected void btnEditNewsClick(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        if (btn != null)
        {
            hdnId.Value = btn.CommandArgument;
            var newsId = Convert.ToInt32(btn.CommandArgument);
            var oNews = entities.News.First(p => p.newsId == newsId);
            GetDataProccess(oNews);
            pnlNewList.Visible = false;
            pnlNewsEdit.Visible = true;
        }
    }

    public void GetDataProccess(News news)
    {
        txtHeader.Text = news.header;
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        txtBrief.Text = news.brief;
        radEditor.Content = news.details;
        if (news.manset != null) cbManset.Checked = news.manset.Value;
        chkState.Checked = Convert.ToBoolean(news.state);
        dpDate.SelectedDate = news.enterDate;
        txtImage.Text = news.imagePath;
    }

    public void ClearFormInputs()
    {
        txtHeader.Text = string.Empty;
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        txtBrief.Text = string.Empty;
        radEditor.Content = string.Empty;
        cbManset.Checked = false;
        chkState.Checked = false;
        dpDate.SelectedDate = null;
        txtImage.Text = string.Empty;
        hdnId.Value = null;
    }
}