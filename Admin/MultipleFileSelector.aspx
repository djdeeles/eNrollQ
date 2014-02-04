<%@ Page Language="C#" AutoEventWireup="true" Inherits="eNroll.Admin.MultipleFileSelector"
         CodeBehind="MultipleFileSelector.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%= AdminResource.language %>"
      xml:lang="<%= AdminResource.language %>" dir="<%= AdminResource.PageDirection %>">
    <meta http-equiv="X-UA-Compatible" content="IE=9">
    <head id="Head1" runat="server">
        <title>
            <%= GetSiteTitle(EnrollAdminContext.Current.DataLanguage.LanguageId) %></title>
        <script src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js" type="text/javascript"></script>
        <script src="js/admin.js" type="text/javascript"></script>
        <script type="text/javascript">

            function OnClientItemSelected(sender, args) {// Called when a file is open.
                
                var item = args.get_item();
                //If file (and not a folder) is selected - call the OnFileSelected method on the parent page
                if (item.get_type() == Telerik.Web.UI.FileExplorerItemType.File) {
                    // Cancel the default dialog;
                    args.set_cancel(true);

                    // get reference to the RadWindow
                    var wnd = getRadWindow();

                    //Get a reference to the opener parent page using RadWndow
                    var openerPage = wnd.BrowserWindow;
                      
                    var selectedItems = sender.get_selectedItems(); 
                    var hfResult = "";
                    for (var i = 0; i < selectedItems.length; i++) { 
                        hfResult += "~" + selectedItems[i].get_url() + "|";
                    } 
                    hfResult = hfResult.slice(0, -1);
                    openerPage.OnMultipleFileSelected(i+" dosya seçildi", hfResult); // Call the method declared on the parent page
                     
                    ////Close the window which hosts this page
                    //wnd.close();
                }
            }
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager1" ScriptMode="Auto" EnableCdn="true"
                                      runat="server" />
            <telerik:RadFileExplorer runat="server" ID="FileExplorer1" Width="100%" Height="500px"  OnClientItemSelected="OnClientItemSelected" >
                <Configuration ViewPaths="~/FileManager/" UploadPaths="~/FileManager/" DeletePaths="~/FileManager/" 
                     SearchPatterns="*.jpg, *.png, *.jpeg, *.gif" AllowMultipleSelection="True" MaxUploadFileSize="10240000">
                </Configuration>
            </telerik:RadFileExplorer>
        </form>
    </body>
</html>