<%@ Page Language="C#" AutoEventWireup="true" Inherits="eNroll.Admin.File_Selector"
         CodeBehind="FileSelector.aspx.cs" %>
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
    </head>
    <body>
        <form id="form1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager1" ScriptMode="Auto" EnableCdn="true"
                                      runat="server" />
            <telerik:RadFileExplorer runat="server" ID="FileExplorer1" Width="100%" Height="500px"
                                     OnClientFileOpen="OnClientFileOpen">
                <Configuration ViewPaths="~/FileManager/" UploadPaths="~/FileManager/" DeletePaths="~/FileManager/" MaxUploadFileSize="10240000">
                </Configuration>
            </telerik:RadFileExplorer>
        </form>
    </body>
</html>