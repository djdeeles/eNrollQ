using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Enroll.WebParts;
using Resources;

public partial class Admin_adminUserControls_FileManager : UserControl
{
    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbFileManager;

        grVFiles.Columns[0].HeaderText = AdminResource.lbPreview;
        grVFiles.Columns[1].HeaderText = AdminResource.lbFileName;
        grVFiles.Columns[2].HeaderText = AdminResource.lbDate;
        grVFiles.Columns[3].HeaderText = AdminResource.lbFileSizeByte;
        grVFiles.Columns[4].HeaderText = AdminResource.lbDelete;

        grVFiles.EmptyDataText = AdminResource.lbNoFileFound;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 11))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }

        if (!IsPostBack)
        {
            LoadTreeview();
        }
        lblError.Text = "";
        imgNewFolder.Text = AdminResource.lbAdd;
        imgButtonEditFolder.Text = AdminResource.lbEdit;
        btnDeleteFolder.Text = AdminResource.lbDelete;
        btnUpload.Text = AdminResource.lbUpload;
        btnDeleteFolder.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";
    }

    private void LoadTreeview()
    {
        try
        {
            TreeView1.Nodes.Clear();
            var oNode = new ImageTreeViewNode();
            oNode.Text = AdminResource.lbRoot;
            oNode.Value = WebConfigurationManager.AppSettings["UserFilesPath"];
            TreeView1.Nodes.Add(oNode);
            GenerateTreenode(WebConfigurationManager.AppSettings["UserFilesPath"], oNode.ChildNodes);
            hdnActiveDirectory.Value = WebConfigurationManager.AppSettings["UserFilesPath"];
            getCurrentPath.setCurrent = WebConfigurationManager.AppSettings["UserFilesPath"];
            ShowFiles();
            ClearTextBox();
            TreeView1.ExpandAll();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void ClearTextBox()
    {
        txtNewFolderName.Text = "";
        txtChangedFolderName.Text = "";
    }

    private void ShowFiles()
    {
        try
        {
            ListFiles();
            lblActiveDirectory.Text = hdnActiveDirectory.Value.Replace(
                WebConfigurationManager.AppSettings["UserFilesPath"], AdminResource.lbRoot);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }


    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        hdnActiveDirectory.Value = TreeView1.SelectedNode.Value;
        getCurrentPath.setCurrent = TreeView1.SelectedNode.Value;
        txtChangedFolderName.Text = TreeView1.SelectedNode.Text;
        ShowFiles();
    }

    private void GenerateTreenode(String Path, TreeNodeCollection nodes)
    {
        try
        {
            var directories = Directory.GetDirectories(Server.MapPath(Path));
            foreach (var directory in directories)
            {
                if (directory.Contains("Thumbnails")) continue;
                var oNode = new ImageTreeViewNode();
                var splitdirectories = directory.Split(System.IO.Path.DirectorySeparatorChar);
                oNode.Text = splitdirectories[splitdirectories.Length - 1];
                oNode.Value = Path + "/" + oNode.Text;
                nodes.Add(oNode);
                GenerateTreenode(oNode.Value, oNode.ChildNodes);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void ListFiles()
    {
        try
        {
            grVFiles.DataSource = GetFiles();
            grVFiles.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private List<System.IO.FileInfo> GetFiles()
    {
        var fileList = new List<System.IO.FileInfo>();
        var files = Directory.GetFiles(Server.MapPath(hdnActiveDirectory.Value));
        foreach (String file in files)
        {
            var oFileInfo = new System.IO.FileInfo(file);
            fileList.Add(oFileInfo);
        }
        return fileList;
    }

    protected void imgNewFolder_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtNewFolderName.Text))
            {
                Directory.CreateDirectory(Server.MapPath(hdnActiveDirectory.Value + "/" + txtNewFolderName.Text));
                LoadTreeview();
            }
        }
        catch (Exception exception)
        {
            lblError.Text = exception.Message;
        }
    }

    protected void imgButtonEditFolder_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            if (hdnActiveDirectory.Value != WebConfigurationManager.AppSettings["UserFilesPath"])
            {
                if (!string.IsNullOrEmpty(txtChangedFolderName.Text))
                {
                    var dirParent = Directory.GetParent(Server.MapPath(hdnActiveDirectory.Value));
                    var strParentPath = dirParent.FullName;
                    Directory.Move(Server.MapPath(hdnActiveDirectory.Value),
                                   strParentPath + "\\" + txtChangedFolderName.Text);
                    LoadTreeview();
                }
            }
            else
            {
                lblError.Text = AdminResource.msgRootRename;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
            lblError.Text = AdminResource.msgFolderRenameError;
        }
    }

    protected void BtnDeleteFolderClick(object sender, EventArgs eventArgs)
    {
        try
        {
            if (hdnActiveDirectory.Value != WebConfigurationManager.AppSettings["UserFilesPath"])
            {
                if (Directory.GetFiles(Server.MapPath(hdnActiveDirectory.Value)).Length>0 ||
                    Directory.GetDirectories(Server.MapPath(hdnActiveDirectory.Value)).Length > 0)
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.lbDirectoryDeleteFailed);
                    return;
                }
                    
                Directory.Delete(Server.MapPath(hdnActiveDirectory.Value));
                LoadTreeview();
            }
            else
            {
                lblError.Text = AdminResource.msgRootDelete;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
            lblError.Text = exception.Message;
        }
    }

    protected void BtnFileDelete(object sender, ImageClickEventArgs e)
    {
        var myButton = (ImageButton) sender;
        try
        {
            File.Delete(myButton.CommandArgument);
            ListFiles();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void grVFiles_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var deleteButton = (ImageButton) e.Row.FindControl("btnFileDelete");
            deleteButton.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "')";


            var myMulti = (MultiView) e.Row.FindControl("grdMultiFileName");
            if (hdnMode.Value == "selector")
            {
                myMulti.ActiveViewIndex = 1;
                var myLink = (LinkButton) e.Row.FindControl("LnkFileName");
                myLink.OnClientClick = "window.opener.update('" + Request.QueryString["returnField"] + "','" +
                                       hdnActiveDirectory.Value + "/" + myLink.Text + "');window.close();";
            }
            else
            {
                myMulti.ActiveViewIndex = 0;
            }
        }
    }

    protected void ImageButton2_Click(object sender, EventArgs eventArgs)
    {
        var uploadedFiles = HttpContext.Current.Request.Files;

        for (int i = 0; i < uploadedFiles.Count; i++)
        {
            var userPostedFile = uploadedFiles[i];
            if (userPostedFile.ContentLength == 0)
            {
                continue;
            }

            try
            {
                var filename = userPostedFile.FileName;
                String filePath = Server.MapPath(hdnActiveDirectory.Value);
                userPostedFile.SaveAs(filePath + "\\" + filename);
                ListFiles();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
                lblError.Text = exception.Message;
            }
        }
    }

    #region Nested type: getCurrentPath

    public static class getCurrentPath
    {
        public static string setCurrent { get; set; }

        public static string currentPath
        {
            get { return setCurrent; }
        }
    }

    #endregion
}