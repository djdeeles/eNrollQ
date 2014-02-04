<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="Edit_Content" CodeBehind="EditContent.aspx.cs" %>
<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="<%= AdminResource.language %>" xml:lang="<%= AdminResource.language %>"
      dir="<%= AdminResource.PageDirection %>">
    <meta http-equiv="X-UA-Compatible" content="IE=9">
    <head id="Head1" runat="server">
        <title>
            <%= GetSiteTitle(EnrollAdminContext.Current.DataLanguage.LanguageId) %></title>
        <link href="style/admin.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" href="../../App_Themes/mainTheme/css/mainTheme.css" type="text/css" />
        <script src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js" type="text/javascript"></script>
        <script src="../../App_Themes/mainTheme/js/jquery.prettyPhoto.js" type="text/javascript"></script>
        <script src="js/admin.js" type="text/javascript"></script>
    </head>
    <body style="direction: <%= AdminResource.PageDirection %>">
        <form id="form1_AdminSub" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager_AdminSub" ScriptMode="Auto" EnableCdn="true"
                                      runat="server" />
            <div id="right">
                <div class="content">
                    <div class="righttop">
                        <div style="float: right; width: 210px; height: 25px; vertical-align: middle; text-align: center; margin-right: 28px; color: #fff; font-style: italic; margin-top: 10px;">
                            <asp:Label ID="lblLocation" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                    <div class="rightcontent">
                        <asp:PlaceHolder ID="PlaceHolder_AdminSub" runat="server"></asp:PlaceHolder>
                    </div>
                    <div class="rightbottom">
                    </div>
                </div>
            </div>
        </form>
    </body>
</html>