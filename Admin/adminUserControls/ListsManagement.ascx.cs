﻿using System;
using System.Data;
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
using Image = System.Drawing.Image;

namespace eNroll.Admin.adminUserControls
{
    public partial class ListsManagement : UserControl
    {
        Entities _ent = new Entities();

        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbListsManagement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hfLanguageId.Value = EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
                if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 25))
                {
                    mvAuth.ActiveViewIndex = 0;
                }
                else
                {
                    mvAuth.ActiveViewIndex = 1;
                }
                if (!IsPostBack)
                {
                    mvLists.SetActiveView(View1);
                    gvListData.Visible = false;
                }

                btnAddNew.Text = AdminResource.lbNewList;
                cbState.Text = AdminResource.lbActive;
                cbStateEdit.Text = AdminResource.lbActive;
                cbListDataState.Text = AdminResource.lbActive;
                cbListDataStateEdit.Text = AdminResource.lbActive;

                btnSave.Text = AdminResource.lbSave;
                btnCancel.Text = AdminResource.lbCancel;
                btnEditSave.Text = AdminResource.lbSave;
                btnEditCancel.Text = AdminResource.lbCancel;

                gvLists.Columns[0].HeaderText = AdminResource.lbActions;
                gvLists.Columns[1].HeaderText = AdminResource.lbName;
                gvLists.Columns[2].HeaderText = AdminResource.lbDesc;
                gvLists.Columns[3].HeaderText = AdminResource.lbState;

                gvListData.Columns[0].HeaderText = AdminResource.lbActions;
                gvListData.Columns[1].HeaderText = AdminResource.lbTitle;
                gvListData.Columns[2].HeaderText = AdminResource.lbDesc;
                gvListData.Columns[3].HeaderText = AdminResource.lbPhoto;
                gvListData.Columns[4].HeaderText = AdminResource.lbState;

                btnAddListData.Text = AdminResource.lbNewListItem;
                BtnListDataSave.Text = AdminResource.lbSave;
                BtnListDataCancel.Text = AdminResource.lbCancel;
                BtnListDataEditSave.Text = AdminResource.lbSave;
                BtnListDataEditCancel.Text = AdminResource.lbCancel;

                btnImageSelect.Text = AdminResource.lbImageSelect;
                btnImageSelectEdit.Text = AdminResource.lbImageSelect;

            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
            
            ShowLists();
        }

        private void ShowLists()
        {
            try
            {
                gvLists.DataSourceID = "EntityDataSourceListManagement";
                gvLists.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        private void ListeGuncelleCommand()
        {
            try
            {
                Temizle();
                int listId = Convert.ToInt32(HiddenFieldListId.Value);
                if (listId > 0)
                {
                    Lists list = _ent.Lists.First(p => p.Id == listId);

                    txtNameEdit.Text = list.Name;
                    txtDescEdit.Text = list.Description;
                    cbStateEdit.Checked = list.State;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #region GvList

        protected void GvListsRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var myMastDelete = (ImageButton)e.Row.FindControl("LinkButtonSil");
                    myMastDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void GvListsRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Guncelle")
            {
                HiddenFieldListId.Value = e.CommandArgument.ToString();
                ListeGuncelleCommand();
                mvLists.SetActiveView(View3);
            }
            else if (e.CommandName == "Sil")
            {
                int listId = Convert.ToInt32(e.CommandArgument);
                Lists list = _ent.Lists.First(p => p.Id == listId);

                var listDatas = _ent.ListData.Where(p => p.ListId == listId).ToList();
                foreach (var data in listDatas)
                {
                    _ent.DeleteObject(data);
                }
                _ent.SaveChanges();
                _ent.DeleteObject(list);
                _ent.SaveChanges();
                Logger.Add(25, 1, listId, 2);
                ShowLists();
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            }
        }

        #endregion

        #region List addNew, save, update, cancel

        // btn add new click
        protected void BtnAddNewClick(object sender, EventArgs e)
        {
            mvLists.SetActiveView(View2);
        }

        // Save
        protected void BtnSaveClick(object sender, EventArgs e)
        {
            try
            {
                var list = new Lists();
                list.Name = txtName.Text;
                list.Description = txtDesc.Text;
                list.State = cbState.Checked;
                list.LanguageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                list.CreatedTime = DateTime.Now;
                list.UpdatedTime = DateTime.Now;

                _ent.AddToLists(list);
                _ent.SaveChanges();

                Logger.Add(25, 1, list.Id, 1);
                Temizle();
                ShowLists();
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                mvLists.SetActiveView(View1);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        //Save cancel
        protected void BtnCancelClick(object sender, EventArgs e)
        {
            mvLists.SetActiveView(View1);
            Temizle();
            HiddenFieldListId.Value = string.Empty;
        }

        //update
        protected void BtnEditSaveClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(HiddenFieldListId.Value))
            {
                try
                {
                    int listId = Convert.ToInt32(HiddenFieldListId.Value);
                    Lists list = _ent.Lists.First(p => p.Id == listId);

                    list.Name = txtNameEdit.Text;
                    list.Description = txtDescEdit.Text;
                    list.State = cbStateEdit.Checked;
                    list.UpdatedTime = DateTime.Now;
                    _ent.SaveChanges();
                    Logger.Add(25, 1, listId, 3);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
                HiddenFieldListId.Value = string.Empty;
                Temizle();
                ShowLists();
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
                mvLists.SetActiveView(View1);
            }
        }

        // Edit cancel
        protected void BtnEditCancelClick(object sender, EventArgs e)
        {
            mvLists.SetActiveView(View1);
            HiddenFieldListId.Value = string.Empty;
            Temizle();
        }

        #endregion

        #region GvListData

        protected void GvListDataRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Guncelle")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                HiddenFieldListDataId.Value = id.ToString();
                var listData = _ent.ListData.First(p => p.Id == id);
                txtListDataTitleEdit.Text = listData.Title;
                txtListDataDescEdit.Text = listData.Description;
                txtImageEdit.Text = listData.Image;
                var editor = ((RadEditor)RtbEdit.FindControl("RadEditor1"));
                editor.Content = listData.Detail;
                if (listData.State != null)
                    cbListDataStateEdit.Checked = (bool)listData.State;

                mvListData.Visible = true;
                mvListData.SetActiveView(vEditListData);
            }
            else if (e.CommandName == "Sil")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                var listData = _ent.ListData.First(p => p.Id == id);

                _ent.DeleteObject(listData);
                _ent.SaveChanges();

                Logger.Add(25, 2, listData.Id, 2);
                BindListData();
            }
        }

        protected void GvListDataRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e != null && e.Row.RowType == DataControlRowType.DataRow)
                {
                    var deleteBtn = (ImageButton)e.Row.FindControl("lbSil");
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

        protected void LbListSecClick(object sender, EventArgs e)
        {
            try
            {
                var lbListSec = (LinkButton)sender;
                HiddenFieldListId.Value = lbListSec.CommandArgument;
                gvListData.DataSourceID = "EntityDataSourceListData";
                EntityDataSourceListData.WhereParameters.Clear();
                EntityDataSourceListData.WhereParameters.Add("ListId", DbType.Int32, HiddenFieldListId.Value);
                gvListData.DataBind();
                gvListData.Visible = true;
                mvListData.Visible = true;
                mvListData.SetActiveView(vAddListDataBtn);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        #endregion

        #region ListData addNew, save, update, cancel

        // btn add new click
        protected void BtnAddListDataClick(object sender, EventArgs e)
        {
            mvListData.SetActiveView(vAddListData);
        }

        //save
        protected void BtnListDataSaveClick(object sender, EventArgs e)
        {
            try
            {
                var listData = new ListData();
                int listId = Convert.ToInt32(HiddenFieldListId.Value);
                if (listId > 0)
                {
                    listData.ListId = listId;
                    listData.Title = txtListDataTitle.Text;
                    listData.Description = txtListDataDesc.Text;
                    listData.Detail = ((RadEditor)Rtb.FindControl("RadEditor1")).Content;
                    listData.State = cbListDataState.Checked;
                    listData.CreatedTime = DateTime.Now;
                    listData.UpdatedTime = DateTime.Now;
                    listData.LanguageId = EnrollAdminContext.Current.DataLanguage.LanguageId;

                    #region save image

                    if (txtImage.Text != string.Empty)
                    {
                        var orj = new Bitmap(Server.MapPath(txtImage.Text.Replace("~", "..")));
                        Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                        string thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                        string dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                        var isphotoSaved = ImageHelper.SaveJpeg(dest, (Bitmap)i, 75);
                        if (isphotoSaved)
                        {
                            listData.ThumbnailPath = "~/FileManager/thumbnails/" + thumbName;
                            listData.Image = txtImage.Text;
                        }
                        else
                        {
                            listData.ThumbnailPath = string.Empty;
                            listData.Image = string.Empty;
                        }
                    }
                    else
                    {
                        listData.ThumbnailPath = string.Empty;
                        listData.Image = string.Empty;
                    }

                    #endregion

                    _ent.AddToListData(listData);
                    _ent.SaveChanges();

                    Logger.Add(25, 2, listData.Id, 1);

                    BindListData();
                    mvListData.SetActiveView(vAddListDataBtn);
                    Temizle();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        //Save cancel
        protected void BtnListDataCancelClick(object sender, EventArgs e)
        {
            BindListData();
            Temizle();
        }

        //Update
        protected void BtnListDataEditSaveClick(object sender, EventArgs e)
        {
            try
            {
                var id = Convert.ToInt32(HiddenFieldListDataId.Value);
                ListData listData = _ent.ListData.FirstOrDefault(p => p.Id == id);
                if (listData != null)
                {
                    listData.Title = txtListDataTitleEdit.Text;
                    listData.Description = txtListDataDescEdit.Text;
                    listData.State = cbListDataStateEdit.Checked;
                    listData.Detail = ((RadEditor)RtbEdit.FindControl("RadEditor1")).Content;
                    listData.UpdatedTime = DateTime.Now;

                    #region update image

                    // resim değiştiyse update yapılır
                    if (txtImageEdit.Text != string.Empty && txtImageEdit.Text != listData.Image)
                    {
                        if (listData.ThumbnailPath != string.Empty)
                        {
                            ImageHelper.DeleteImage(Server.MapPath(listData.ThumbnailPath.Replace("~", "..")));
                        }

                        var orj = new Bitmap(Server.MapPath(txtImageEdit.Text.Replace("~", "..")));
                        Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                        string thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                        string dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);

                        var isphotoSaved = ImageHelper.SaveJpeg(dest, (Bitmap)i, 75);
                        if (isphotoSaved)
                        {
                            listData.ThumbnailPath = "~/FileManager/thumbnails/" + thumbName;
                            listData.Image = txtImageEdit.Text;
                        }
                    }
                    else if (txtImageEdit.Text == string.Empty)
                    {
                        if (listData.ThumbnailPath != string.Empty)
                        {
                            ImageHelper.DeleteImage(Server.MapPath(listData.ThumbnailPath.Replace("~", "..")));
                        }

                        listData.ThumbnailPath = string.Empty;
                        listData.Image = string.Empty;
                    }

                    #endregion

                    _ent.SaveChanges();

                    Logger.Add(25, 2, listData.Id, 3);
                }
                BindListData();
                mvListData.SetActiveView(vAddListDataBtn);
                Temizle();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        //Edit cancel
        protected void BtnListDataEditCancelClick(object sender, EventArgs e)
        {
            BindListData();
            Temizle();
        }

        //bind gridview listData with selected listId
        public void BindListData()
        {
            gvListData.DataSourceID = "EntityDataSourceListData";
            EntityDataSourceListData.WhereParameters.Clear();
            EntityDataSourceListData.WhereParameters.Add("ListId", DbType.Int32, HiddenFieldListId.Value);
            gvListData.DataBind();
            gvListData.Visible = true;
            mvListData.SetActiveView(vAddListDataBtn);
        }

        #endregion

        #region clear inputs

        private void Temizle()
        {
            txtName.Text = string.Empty;
            txtDesc.Text = string.Empty;
            txtImage.Text = string.Empty;
            var radEditor = ((RadEditor)Rtb.FindControl("RadEditor1"));
            radEditor.Content = string.Empty;
            cbState.Checked = false;
            txtListDataTitle.Text = string.Empty;
            txtListDataDesc.Text = string.Empty;
            cbListDataState.Checked = false;

            txtNameEdit.Text = string.Empty;
            txtDescEdit.Text = string.Empty;
            cbStateEdit.Checked = false;
            txtListDataTitleEdit.Text = string.Empty;
            txtListDataDescEdit.Text = string.Empty;
            cbListDataStateEdit.Checked = false;
            radEditor = ((RadEditor)RtbEdit.FindControl("RadEditor1"));
            radEditor.Content = string.Empty;
        }

        #endregion

        protected void ImageButton2Ara_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var aranan = txtAra.Text;

                EntityDataSourceListData.Where = "it.Lists.LanguageId=" +
                                          EnrollAdminContext.Current.DataLanguage.LanguageId.ToString() +
                                          " and it.ListId=" + HiddenFieldListId.Value + " AND (it.Title like '%" + aranan + "%' OR it.Description like '%" + aranan + "%')";
                EntityDataSourceListData.DataBind();
                gvListData.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

        }
    }
}