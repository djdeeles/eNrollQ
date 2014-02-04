using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Resources;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class BackupRestore : UserControl
    {
        public static string Path = string.Empty;
        public static string DatabaseName = string.Empty;
        public static string ServerInstance = string.Empty;
        public static string Uid = string.Empty;
        public static string Pwd = string.Empty;
        public static string BackupFolder = "dbBackup/";

        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbBackupRestore;

            grVFiles.Columns[0].HeaderText = AdminResource.lbActions;
            grVFiles.Columns[1].HeaderText = AdminResource.lbFileName;
            grVFiles.Columns[2].HeaderText = AdminResource.lbDate;
            grVFiles.Columns[3].HeaderText = AdminResource.lbFileSizeByte;

            btBackUp.Text = AdminResource.lbDbBackUp;
            btFullBackup.Text = AdminResource.lbFileBackUp;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 23))
            {
                MultiView2.ActiveViewIndex = 0;
            }
            else
            {
                MultiView2.ActiveViewIndex = 1;
            }

            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ConnectionString;

            DatabaseName = builder["database"] as string;
            ServerInstance = builder["server"] as string;
            Uid = builder["uid"] as string;
            Pwd = builder["pwd"] as string;
            var datetimeNowString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(":", "-").Replace(" ", "_");
            Path = Server.MapPath(".") + "/" + BackupFolder + DatabaseName + "_" + datetimeNowString + ".bak";
            ListFiles();
        }

        public void BackupDatabaseClick(object sender, EventArgs eventArgs)
        {
            try
            {
                var fileList = GetFiles();
                if (fileList.Count < 5)
                {
                    BackupDatabase(Path);
                    Logger.Add(23, 2, 0, 1);
                    MessageBox.Show(MessageType.Success, AdminResource.lbBackupSuccessful);
                    ListFiles();
                }
                else
                {
                    MessageBox.Show(MessageType.Success, AdminResource.lbMaxBackupReached);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
                ExceptionManager.ManageException(exception);
            }
        }

        public void BackupDatabase(string path_)
        {
            var sqlBackup = new Backup();
            sqlBackup.Action = BackupActionType.Database;
            sqlBackup.BackupSetDescription = DatabaseName + ":" + DateTime.Now.ToShortDateString();
            sqlBackup.BackupSetName = DatabaseName;
            sqlBackup.Database = DatabaseName;
            sqlBackup.Incremental = false;

            var deviceItem = new BackupDeviceItem(path_, DeviceType.File);
            var connection = new ServerConnection(ServerInstance, Uid, Pwd);
            var sqlServer = new Server(connection);

            connection.LoginSecure = true; //*


            sqlBackup.Initialize = true;
            sqlBackup.Checksum = true;
            sqlBackup.ContinueAfterError = true;

            sqlBackup.Devices.Add(deviceItem);
            sqlBackup.Incremental = false;

            sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
            sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

            sqlBackup.FormatMedia = false;
            sqlBackup.SqlBackup(sqlServer);
            sqlBackup.Devices.Remove(deviceItem);
        }

        protected void BackupFilesClick(object sender, EventArgs eventArgs)
        {
            var zipFileName = DatabaseName + "_" +
                              DateTime.Now.ToString().Replace(".", "_").Replace(" ", "-").Replace(":", "_");

            Logger.Add(23, 3, 0, 1);

            var zipLibrary = new ZipLibrary(zipFileName);
            zipLibrary.ZipFolder(
                HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["UserFilesPath"]), string.Empty);
        }

        protected void RestoreDatabase(object sender, EventArgs eventArgs)
        {
            try
            {
                var btRestoreDb = (ImageButton) sender;
                Path = btRestoreDb.CommandArgument;

                try
                {
                    var connection = new ServerConnection(ServerInstance, Uid, Pwd);
                    var svr = new Server(connection);

                    var backupltem = new BackupDeviceItem(Path, DeviceType.File);
                    var restore = new Restore
                                      {
                                          Database = DatabaseName,
                                          NoRecovery = false,
                                          ReplaceDatabase = true
                                      };

                    svr.Refresh();
                    svr.KillDatabase(DatabaseName);

                    restore.Devices.Add(backupltem);
                    restore.Action = RestoreActionType.Database; //*
                    restore.ReplaceDatabase = true; //*
                    restore.SqlRestore(svr);

                    var db = svr.Databases[DatabaseName];
                    db.SetOnline();
                    db.Refresh();
                    svr.Refresh();

                    Logger.Add(23, 1, 0, 1);
                    MessageBox.Show(MessageType.Success, AdminResource.lbRecoverySuccessful);
                    ListFiles();
                }
                catch (Exception ex)
                {
                    ExceptionManager.ManageException(ex);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
                ExceptionManager.ManageException(exception);
            }

            #region eski

            /*
            try
            {
                var connection = new ServerConnection(serverInstance, uid, pwd);
                var server = new Server(connection);
                server.Refresh();

                var btRestoreDb = (ImageButton)sender;
                Path = btRestoreDb.CommandArgument;
                server.KillDatabase(DatabaseName);
                var backupltem = new BackupDeviceItem(Path, DeviceType.File);
                var restore = new Restore
                                  {
                                      Database = DatabaseName,
                                      NoRecovery = false,
                                      ReplaceDatabase = true
                                  };
                restore.Action = RestoreActionType.Database;
                restore.Devices.Add(backupltem);
                try
                {
                    restore.SqlRestore(server);

                    restore.Action = RestoreActionType.Database;//*
                    restore.ReplaceDatabase = true;//*

                    var db = server.Databases[DatabaseName];
                    db.SetOnline();
                    server.Refresh();

                    Logger.Add(23, 1, 0, 1);
                    MessageBox.Show(MessageType.Success, AdminResource.lbRecoverySuccessful);
                    ListFiles();
                }
                catch (Exception ex)
                {
                    ExceptionManager.ManageException(ex);
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }*/

            #endregion
        }

        protected void GrVFilesRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e != null && e.Row.DataItem != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteButton = (ImageButton) e.Row.FindControl("btnFileDelete");
                deleteButton.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "')";
                var restoreButton = (ImageButton) e.Row.FindControl("btnDBRestore");
                restoreButton.OnClientClick = " return confirm('" + AdminResource.lbRestoreQuestion + "')";
            }
        }

        private void ListFiles()
        {
            try
            {
                // create backup dir if not exist
                string path = BackupFolder;
                bool isExists = Directory.Exists(path);
                if (!isExists)
                    Directory.CreateDirectory(Server.MapPath(path));

                grVFiles.DataSource = GetFiles();
                grVFiles.DataBind();
            }
            catch (Exception exception)
            {
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
                ExceptionManager.ManageException(exception);
            }
        }

        private List<System.IO.FileInfo> GetFiles()
        {
            var fileList = new List<System.IO.FileInfo>();
            var files = Directory.GetFiles(Server.MapPath(".") + "/" + BackupFolder);
            foreach (String file in files)
            {
                var oFileInfo = new System.IO.FileInfo(file);
                fileList.Add(oFileInfo);
            }

            // order by CreatedTime
            var sortedFileList = new List<System.IO.FileInfo>();
            if (fileList.Count > 0)
            {
                for (int i = (fileList.Count - 1); i >= 0; i--)
                {
                    sortedFileList.Add(fileList[i]);
                }
                return sortedFileList;
            }
            else
                return fileList;
        }

        protected void BtnFileDelete(object sender, ImageClickEventArgs e)
        {
            var myButton = (ImageButton) sender;
            try
            {
                string path = myButton.CommandArgument;
                if (!string.IsNullOrWhiteSpace(path))
                {
                    File.Delete(path);
                    MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    Logger.Add(23, 2, 0, 2);
                    ListFiles();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(MessageType.Error, AdminResource.msgAnErrorOccurred);
                ExceptionManager.ManageException(exception);
            }
        }
    }
}