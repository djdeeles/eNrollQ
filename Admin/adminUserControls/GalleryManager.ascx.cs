﻿using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;
using Image = System.Drawing.Image;

public partial class Admin_adminUserControls_GalleryManager : UserControl
{
    private readonly Entities _ent = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbGalleryManagement;
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

        #region resources

        cbCatActive.Text = AdminResource.lbActive;
        cbAlbActive.Text = AdminResource.lbActive;
        photoState.Text = AdminResource.lbActive;


        btnNew.Text = AdminResource.lbNewCategory;
        btnNewAlbum.Text = AdminResource.lbNewAlbum;
        btnNewImage.Text = AdminResource.lbNewImage;
        btnNewMultipleImage.Text = AdminResource.lbbtnNewMultipleImage;

        btnImageSelect.Text = AdminResource.lbImageSelect;

        btnSave.Text = AdminResource.lbSave;
        BtnSaveMultiplePhoto.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;
        BtnCancelMultiplePhoto.Text = AdminResource.lbCancel;

        btnSaveAlbum.Text = AdminResource.lbSave;
        btnCancelAlbum.Text = AdminResource.lbCancel;

        btnSavePhoto.Text = AdminResource.lbSave;
        btnCancelPhoto.Text = AdminResource.lbCancel;

        gvCategories.Columns[0].HeaderText = AdminResource.lbActions;
        gvCategories.Columns[1].HeaderText = AdminResource.lbCategoryName;
        gvCategories.Columns[2].HeaderText = AdminResource.lbDesc;
        gvCategories.Columns[3].HeaderText = AdminResource.lbState;

        gvCategories.EmptyDataText = AdminResource.msgNotFoundCategory;

        gvAlb.Columns[0].HeaderText = AdminResource.lbActions;
        gvAlb.Columns[1].HeaderText = AdminResource.lbAlbumName;
        gvAlb.Columns[2].HeaderText = AdminResource.lbDesc;
        gvAlb.Columns[3].HeaderText = AdminResource.lbCategoryName;
        gvAlb.Columns[4].HeaderText = AdminResource.lbState;

        gvAlb.EmptyDataText = AdminResource.msgNotFoundAlbum;

        gvPhotos.Columns[0].HeaderText = AdminResource.lbActions;
        gvPhotos.Columns[1].HeaderText = AdminResource.lbImages;
        gvPhotos.Columns[2].HeaderText = AdminResource.lbName;
        gvPhotos.Columns[3].HeaderText = AdminResource.lbDesc;
        gvPhotos.Columns[4].HeaderText = AdminResource.lbAlbumName;
        gvPhotos.Columns[5].HeaderText = AdminResource.lbState;
        gvPhotos.Columns[6].HeaderText = AdminResource.lbMainImage;

        gvPhotos.EmptyDataText = AdminResource.msgNotFoundPhoto;

        #endregion
    }

    public string GetCategoryName(int id)
    {
        string catName = string.Empty;
        var cat = _ent.Def_photoAlbumCategory.FirstOrDefault(p => p.photoAlbumCategoryId == id);
        if (cat != null) catName = cat.categoryName;
        return catName;
    }

    public string GetAlbumName(int id)
    {
        string albumName = string.Empty;
        var album = _ent.Def_photoAlbum.FirstOrDefault(p => p.photoAlbumId == id);
        if (album != null) albumName = album.albumName;
        return albumName;
    }

    #region Category

    protected void BtnNewCategoryClick(object sender, EventArgs eventArgs)
    {
        mvKat.SetActiveView(vvYeniKat);
    }

    protected void BtnCategorySaveClick(object sender, EventArgs eventArgs)
    {
        using (var ent = new Entities())
        {
            try
            {
                var cat = new Def_photoAlbumCategory();
                cat.categoryName = txtCatName.Text;
                cat.categoryNotes = txtCatDesc.Text;
                cat.state = cbCatActive.Checked;
                cat.CreatedTime = DateTime.Now;
                cat.UpdatedTime = DateTime.Now;
                cat.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                ent.AddToDef_photoAlbumCategory(cat);
                ent.SaveChanges();

                Logger.Add(5, 1, cat.photoAlbumCategoryId, 1);

                gvCategories.DataBind();
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                mvKat.SetActiveView(vvKatEkle);
                txtCatDesc.Text = "";
                txtCatName.Text = "";
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void GvCategoriesRowCommand(object sender, GridViewCommandEventArgs e) // on imagebutton click
    {
        if (e.CommandName == "Update")
        {
            int id = int.Parse(e.CommandArgument.ToString());

            Def_photoAlbumCategory category = _ent.Def_photoAlbumCategory.First(p => p.photoAlbumCategoryId == id);

            var v = (GridView)sender;
            var tbName = (TextBox)gvCategories.Rows[v.EditIndex].FindControl("tbEdidCategoryName");
            category.categoryName = tbName.Text;

            category.UpdatedTime = DateTime.Now;

            Logger.Add(5, 1, category.photoAlbumCategoryId, 3);

            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            _ent.SaveChanges();


        }
    }

    protected void BtnSaveCategoryCancelClick(object sender, EventArgs eventArgs)
    {
        mvKat.SetActiveView(vvKatEkle);
        txtCatName.Text = "";
        txtCatDesc.Text = "";
    }

    protected void GvCategoriesRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteBtn = (ImageButton)e.Row.FindControl("lbCatSil");
                if (deleteBtn != null)
                {
                    deleteBtn.OnClientClick =
                        " return confirm('" + AdminResource.lbConfirmMsgDeleteCategory + "'); ";
                    deleteBtn.ToolTip = AdminResource.lbDelete;
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void LbCatSilClick(object sender, EventArgs e)
    {
        var lbCatSil = sender as ImageButton;
        if (lbCatSil != null)
        {
            var catId = Convert.ToInt32(lbCatSil.CommandArgument);
            using (var ent = new Entities())
            {
                try
                {
                    var cat = ent.Def_photoAlbumCategory.First(p => p.photoAlbumCategoryId == catId);
                    var albumler = ent.Def_photoAlbum.Where(p => p.photoAlbumCategoryId == catId).ToList();
                    foreach (Def_photoAlbum album in albumler)
                    {
                        var fotolar = ent.PhotoAlbum.Where(p => p.photoAlbumId == album.photoAlbumId).ToList();
                        foreach (var photo in fotolar)
                        {
                            if (!string.IsNullOrWhiteSpace(photo.thumbnailPath))
                                ImageHelper.DeleteImage(Server.MapPath(photo.thumbnailPath.Replace("~", "..")));
                            ent.DeleteObject(photo);
                        }
                        ent.DeleteObject(album);
                    }

                    Logger.Add(5, 1, cat.photoAlbumCategoryId, 2);

                    ent.DeleteObject(cat);
                    ent.SaveChanges();
                    gvCategories.DataBind();
                    MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
        }
    }

    protected void LbCatSecClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["catId"] != null) ViewState.Remove("catId");
            var lbCatSec = (LinkButton)sender;
            int categoryId = Convert.ToInt32(lbCatSec.CommandArgument);
            hfSelectedCategory.Value = lbCatSec.CommandArgument;
            ViewState.Add("catId", categoryId);
            edsPhotoAlbum.WhereParameters.Clear();
            edsPhotoAlbum.WhereParameters.Add("catId", DbType.Int32, categoryId.ToString());

            gvAlb.SelectedIndex = 0;
            gvAlb.PageIndex = 0;
            gvAlb.DataBind();

            if (ViewState["albId"] != null) ViewState.Remove("albId");
            edsPhotos.WhereParameters.Clear();
            edsPhotos.WhereParameters.Add("albId", DbType.Int32, "0");

            gvPhotos.SelectedIndex = 0;
            gvPhotos.PageIndex = 0;
            gvPhotos.DataBind();

            mvPhoto.Visible = false;
            gvAlb.Visible = true;
            mvAlb.Visible = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    #endregion

    #region Album

    protected void BtnNewAlbumClick(object sender, EventArgs eventArgs)
    {
        mvAlb.SetActiveView(vvYeniAlb);
    }

    protected void BtnSaveAlbumClick(object sender, EventArgs eventArgs)
    {
        using (var ent = new Entities())
        {
            try
            {
                var categoryId = Convert.ToInt32(ViewState["catId"]);
                var cat = ent.Def_photoAlbumCategory.First(p => p.photoAlbumCategoryId == categoryId);
                var alb = new Def_photoAlbum();
                alb.albumName = txtAlbName.Text;
                alb.albumNote = txtAlbDesc.Text;
                alb.photoAlbumCategoryId = cat.photoAlbumCategoryId;
                alb.languageId = cat.languageId;
                alb.state = cbAlbActive.Checked;
                alb.CreatedTime = DateTime.Now;
                alb.UpdatedTime = DateTime.Now;
                ent.AddToDef_photoAlbum(alb);
                ent.SaveChanges();

                Logger.Add(5, 2, alb.photoAlbumId, 1);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                gvCategories.DataBind();

                edsPhotoAlbum.WhereParameters.Clear();
                edsPhotoAlbum.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
                gvAlb.DataBind();

                txtAlbDesc.Text = "";
                txtAlbName.Text = "";
                mvAlb.SetActiveView(vvAlbEkle);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void BtnCancelAlbumClick(object sender, EventArgs eventArgs)
    {
        mvAlb.SetActiveView(vvAlbEkle);
        txtAlbDesc.Text = "";
        txtAlbName.Text = "";
    }

    protected void LbAlbSecClick(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["albId"] != null)
                ViewState.Remove("albId");
            var lbAlbSec = (LinkButton)sender;
            var albId = Convert.ToInt32(lbAlbSec.CommandArgument);
            hfSelectedAlbum.Value = lbAlbSec.CommandArgument;
            ViewState.Add("albId", albId);
            edsPhotos.WhereParameters.Clear();
            edsPhotos.WhereParameters.Add("albId", DbType.Int32, albId.ToString());
            gvPhotos.SelectedIndex = 0;
            gvPhotos.PageIndex = 0;
            gvPhotos.DataBind();
            gvPhotos.Visible = true;
            mvPhoto.Visible = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void LbAlbSilClick(object sender, EventArgs e)
    {
        try
        {
            var lbAlbSil = sender as ImageButton;
            int albId = Convert.ToInt32(lbAlbSil.CommandArgument);
            using (var ent = new Entities())
            {
                Def_photoAlbum alb = ent.Def_photoAlbum.Where(p => p.photoAlbumId == albId).First();
                var fotolar = ent.PhotoAlbum.Where(p => p.photoAlbumId == albId).ToList();
                foreach (PhotoAlbum p in fotolar)
                {
                    if (string.IsNullOrWhiteSpace(p.thumbnailPath))
                    {
                        ImageHelper.DeleteImage(Server.MapPath(p.thumbnailPath.Replace("~", "..")));
                    }

                    ent.DeleteObject(p);
                }
                Logger.Add(5, 2, albId, 2);
                ent.DeleteObject(alb);
                ent.SaveChanges();

                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            }
            gvCategories.DataBind();
            edsPhotoAlbum.WhereParameters.Clear();
            edsPhotoAlbum.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
            gvAlb.DataBind();
            gvPhotos.Visible = false;
            mvPhoto.Visible = false;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GvAlbRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteBtn = (ImageButton)e.Row.FindControl("lbAlbSil");
                if (deleteBtn != null)
                {
                    deleteBtn.OnClientClick =
                        " return confirm('" + AdminResource.lbConfirmMsgDeleteAlbum + "')";
                    deleteBtn.ToolTip = AdminResource.lbDelete;
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GvAlbRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            int id = int.Parse(e.CommandArgument.ToString());
            Def_photoAlbum photoAlbum = _ent.Def_photoAlbum.First(p => p.photoAlbumId == id);
            photoAlbum.UpdatedTime = DateTime.Now;

            var v = (GridView)sender;
            var tbName = (TextBox)v.Rows[v.EditIndex].FindControl("tbEditName");
            photoAlbum.albumName = tbName.Text;

            _ent.SaveChanges();

            Logger.Add(5, 2, id, 3);

            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
        }

        gvCategories.DataBind();
        edsPhotoAlbum.WhereParameters.Clear();
        edsPhotoAlbum.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
        gvAlb.DataBind();
    }

    #endregion

    #region Photo

    protected void BtnNewImageClick(object sender, EventArgs eventArgs)
    {
        mvPhoto.SetActiveView(vvYeniPhoto);
    }

    protected void BtnNewMultipleImageClick(object sender, EventArgs eventArgs)
    {
        mvPhoto.SetActiveView(vvYeniMultiplePhoto);
    }

    protected void BtnSavePhotoClick(object sender, EventArgs eventArgs)
    {
        try
        {
            bool isNewRecord = true;
            if (ViewState["photoId"] != null) isNewRecord = false;
            PhotoAlbum photo;
            if (!isNewRecord)
            {
                int photoId = Convert.ToInt32(ViewState["photoId"]);
                photo = _ent.PhotoAlbum.First(p => p.photoId == photoId);
                if (txtPhotoPath.Text != photo.photoPath)
                {
                    var orj = new Bitmap(Server.MapPath(txtPhotoPath.Text.Replace("~", "..")));
                    Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                    string thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                    string dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                    ImageHelper.SaveJpeg(dest, (Bitmap)i, 75);
                    photo.thumbnailPath = "~/FileManager/thumbnails/" + thumbName;
                }
            }
            else
            {
                photo = new PhotoAlbum();
                var orj = new Bitmap(Server.MapPath(txtPhotoPath.Text.Replace("~", "..")));
                Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                string thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                string dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                ImageHelper.SaveJpeg(dest, (Bitmap)i, 75);
                photo.thumbnailPath = "~/FileManager/thumbnails/" + thumbName;
            }

            photo.photoPath = txtPhotoPath.Text;
            if (!string.IsNullOrEmpty(Request.QueryString["alb"]))
                photo.photoAlbumId = Convert.ToInt32(Request.QueryString["alb"]);
            else photo.photoAlbumId = Convert.ToInt32(ViewState["albId"]);
            photo.photoName = txtPhotoName.Text;
            photo.photoNote = txtPhotoDesc.Text;
            photo.UpdatedTime = DateTime.Now;
            photo.State = photoState.Checked;

            #region albüm kapak fotoğrafı yap

            if (cbMainImage.Checked)
            {
                var e = new Entities();
                var albumAllPhotos = e.PhotoAlbum.Where(p => p.photoAlbumId == photo.photoAlbumId);
                foreach (var allPhoto in albumAllPhotos)
                {
                    allPhoto.mainPhoto = false;
                }
                photo.mainPhoto = cbMainImage.Checked;
                e.SaveChanges();
            }

            #endregion

            if (isNewRecord)
            {
                photo.CreatedTime = DateTime.Now;
                _ent.AddToPhotoAlbum(photo);
                _ent.SaveChanges();
                Logger.Add(5, 3, photo.photoId, 1);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            else
            {
                _ent.SaveChanges();
                Logger.Add(5, 3, photo.photoId, 3);
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
             
            if (photo.State.Value && photo.mainPhoto)
                SetAllPhotosNotMainPhoto(photo.photoAlbumId, photo.photoId);
            else
            {
                CheckMainPhotoAdd(photo.photoId);
            }

            gvCategories.DataBind();

            edsPhotoAlbum.WhereParameters.Clear();
            edsPhotoAlbum.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
            gvAlb.DataBind();

            edsPhotos.WhereParameters.Clear();
            edsPhotos.WhereParameters.Add("albId", DbType.Int32, ViewState["albId"].ToString());
            gvPhotos.DataBind();

            mvPhoto.SetActiveView(vvPhotoEkle);
            txtPhotoDesc.Text = "";
            txtPhotoName.Text = "";
            txtPhotoPath.Text = "";
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnCancelPhotoClick(object sender, EventArgs eventArgs)
    {
        mvPhoto.SetActiveView(vvPhotoEkle);
        txtPhotoDesc.Text = "";
        txtPhotoName.Text = "";
        txtPhotoPath.Text = "";
    }

    protected void LbGorselSilClick(object sender, EventArgs e)
    {
        try
        {
            var lbGorselSil = (ImageButton)sender;
            var photoId = Convert.ToInt32(lbGorselSil.CommandArgument);
            var photo = _ent.PhotoAlbum.FirstOrDefault(p => p.photoId == photoId);

            if (photo != null)
            {
                var path = photo.thumbnailPath;
                if (!string.IsNullOrWhiteSpace(path)) ImageHelper.DeleteImage(Server.MapPath(path.Replace("~", "..")));
                var albumId = photo.photoAlbumId;
                _ent.DeleteObject(photo);
                _ent.SaveChanges();

                CheckMainPhoto(albumId);

                Logger.Add(5, 3, photoId, 2);
                
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                
                edsPhotos.WhereParameters.Clear();
                edsPhotos.WhereParameters.Add("albId", DbType.Int32, hfSelectedAlbum.Value);
                gvPhotos.DataBind();

            }
            else
            {
                MessageBox.Show(MessageType.Warning, AdminResource.lbNotFound);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GvPhotosRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteBtn = (ImageButton)e.Row.FindControl("lbGorselSil");
                if (deleteBtn != null)
                {
                    deleteBtn.OnClientClick =
                        " return confirm('" + AdminResource.lbConfirmMsgDeletePhoto + "') ";
                    deleteBtn.ToolTip = AdminResource.lbDelete;
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GvPhotosRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            var gv = sender as GridView;
            if (gv != null)
            {
                var editIndex = gv.EditIndex;
                var gvLine = gv.Rows[editIndex];

                #region edit template controls
                var cbMainPhoto = (CheckBox)gvLine.FindControl("cbMainPhoto");
                var ddlNewAlbum = (DropDownList)gvLine.FindControl("ddlNewAlbum");
                #endregion

                #region update photo
                var editPhotoId = int.Parse(e.CommandArgument.ToString());
                var editPhoto = _ent.PhotoAlbum.First(p => p.photoId == editPhotoId);
                editPhoto.mainPhoto = cbMainPhoto.Checked;
                editPhoto.UpdatedTime = DateTime.Now;
                _ent.SaveChanges();
                var newAlbumId = Convert.ToInt32(ddlNewAlbum.SelectedItem.Value);
                 
                #endregion

                ChechkMainPhotoUpdate(editPhoto.photoAlbumId, newAlbumId, editPhotoId);

                Logger.Add(5, 3, editPhotoId, 3);
            }
            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
        }
        gvCategories.DataBind();

        edsPhotoAlbum.WhereParameters.Clear();
        edsPhotoAlbum.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
        gvAlb.DataBind();

        edsPhotos.WhereParameters.Clear();
        edsPhotos.WhereParameters.Add("albId", DbType.Int32, ViewState["albId"].ToString());
        gvPhotos.DataBind();
    }

    #endregion

    protected void BtnSaveMultiplePhotoClick(object sender, EventArgs e)
    {
        try
        {
            var dataPhotoPaths = hfSelectedFiles.Value.Split('|');
            foreach (var dataPhotoPath in dataPhotoPaths)
            {
                var photo = new PhotoAlbum();
                var orj = new Bitmap(Server.MapPath(dataPhotoPath.Replace("~", "..")));
                var i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                var thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                var dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                ImageHelper.SaveJpeg(dest, (Bitmap)i, 75);
                photo.thumbnailPath = "~/FileManager/thumbnails/" + thumbName;
                photo.photoPath = dataPhotoPath;

                if (!string.IsNullOrEmpty(Request.QueryString["alb"]))
                    photo.photoAlbumId = Convert.ToInt32(Request.QueryString["alb"]);
                else photo.photoAlbumId = Convert.ToInt32(ViewState["albId"]);

                if (cbUseFilesName.Checked)
                {
                    var data = dataPhotoPath.Split('/');
                    var fileName = data[data.Length - 1].Split('.')[0];
                    photo.photoName = fileName;
                }
                else
                {
                    photo.photoName = "";
                }

                photo.photoNote = "";
                photo.UpdatedTime = DateTime.Now;
                photo.State = cbMultiFilesState.Checked;
                photo.mainPhoto = cbMainImage.Checked;
                photo.CreatedTime = DateTime.Now;
                _ent.AddToPhotoAlbum(photo);
                _ent.SaveChanges();

                CheckMainPhotoAdd(photo.photoId);
            }

            MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            gvCategories.DataBind();

            edsPhotoAlbum.WhereParameters.Clear();
            edsPhotoAlbum.WhereParameters.Add("catId", DbType.Int32, ViewState["catId"].ToString());
            gvAlb.DataBind();

            edsPhotos.WhereParameters.Clear();
            edsPhotos.WhereParameters.Add("albId", DbType.Int32, ViewState["albId"].ToString());
            gvPhotos.DataBind();

            mvPhoto.SetActiveView(vvPhotoEkle);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnCancelMultiplePhotoClick(object sender, EventArgs e)
    {
        mvPhoto.SetActiveView(vvPhotoEkle);
        cbMultiFilesState.Checked = false; 
    }

    private void CheckMainPhotoAdd(int addedPhotoId)
    {
        var editPhoto = _ent.PhotoAlbum.FirstOrDefault(p => p.photoId == addedPhotoId);
        if (editPhoto != null)
        {
            #region editphoto => active&main : diğer fotoğraflar mainPhoto='false' olur
            if (editPhoto.State != null && (editPhoto.mainPhoto && editPhoto.State.Value))
            {
                SetAllPhotosNotMainPhoto(editPhoto.photoAlbumId, addedPhotoId); 
            }
            #endregion

            #region editphoto => !(active&main) : active fotoğraf var ise rastgele ilk fotoğraf mainPhoto olur
 
            #region album hasn't active&main photo
            else if (_ent.PhotoAlbum.Count(p => p.photoAlbumId == editPhoto.photoAlbumId && p.State.Value && p.mainPhoto) == 0)
            {
                var randomFirstActivePhoto = _ent.PhotoAlbum.FirstOrDefault(p => p.photoAlbumId == editPhoto.photoAlbumId && p.State.Value);
                if (randomFirstActivePhoto != null)
                {
                    randomFirstActivePhoto.mainPhoto = true;
                    _ent.SaveChanges();
                    SetAllPhotosNotMainPhoto(editPhoto.photoAlbumId, randomFirstActivePhoto.photoId);
                }
            }
            #endregion
            #region album has active&main photo
            else 
            {
                var randomActiveMainPhotoPhoto = _ent.PhotoAlbum.FirstOrDefault(p => p.photoAlbumId == editPhoto.photoAlbumId && p.State.Value && p.mainPhoto);
                if (randomActiveMainPhotoPhoto != null)
                {
                    SetAllPhotosNotMainPhoto(editPhoto.photoAlbumId, randomActiveMainPhotoPhoto.photoId);
                }
            }
            #endregion
             
            #endregion
             
        }

    }

    private void ChechkMainPhotoUpdate(int oldPhotoAlbumId, int newPhotoAlbumId, int editPhotoId)
    {
        var photo = _ent.PhotoAlbum.FirstOrDefault(p => p.photoId == editPhotoId);
        if(photo!=null)
        { 
            if (photo.State != null && (photo.State.Value && photo.mainPhoto))
            {
                #region albüm değişmediyse
                if (oldPhotoAlbumId== newPhotoAlbumId)
                {
                    SetAllPhotosNotMainPhoto(photo.photoAlbumId, photo.photoId); 
                }
                #endregion

                #region albüm değiştiyse
                else if (oldPhotoAlbumId != newPhotoAlbumId)
                {
                    #region yeni albüm
                    SetAllPhotosNotMainPhoto(newPhotoAlbumId, editPhotoId); 
                    #endregion

                    #region eski albüm
                    var randomFirstActivePhoto = _ent.PhotoAlbum.FirstOrDefault(p => p.photoAlbumId == oldPhotoAlbumId && p.State.Value && p.photoId != editPhotoId);
                    if (randomFirstActivePhoto != null)
                    {
                        randomFirstActivePhoto.mainPhoto = true;
                        _ent.SaveChanges();

                        SetAllPhotosNotMainPhoto(oldPhotoAlbumId, randomFirstActivePhoto.photoId); 
                    }
                    #endregion
                }
                #endregion
            } 
        } 
    }

    private void CheckMainPhoto(int albumId)
    {
        var main = _ent.PhotoAlbum.FirstOrDefault(p => p.photoAlbumId == albumId && p.State.Value && p.mainPhoto);
        if ( main == null)
        {
            var randomFirstActivePhoto = _ent.PhotoAlbum.FirstOrDefault(p => p.photoAlbumId == albumId && p.State.Value);
            if (randomFirstActivePhoto != null)
            {
                randomFirstActivePhoto.mainPhoto = true;
                _ent.SaveChanges();

                SetAllPhotosNotMainPhoto(albumId, randomFirstActivePhoto.photoId); 
            }
        }
        else
        {
            var activeMainPhotoId = main.photoId;
            SetAllPhotosNotMainPhoto(albumId, activeMainPhotoId); 
        }
    }

    private void SetAllPhotosNotMainPhoto(int albumId, int photoId)
    {
        var otherPhotos = _ent.PhotoAlbum.Where(p => p.photoAlbumId == albumId && p.photoId != photoId).ToList();
        foreach (var otherPhoto in otherPhotos)
        {
            otherPhoto.mainPhoto = false;
            _ent.SaveChanges();
        }
    }
}