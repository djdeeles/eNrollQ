<%@ Control Language="C#" AutoEventWireup="true" Inherits="adminUserControls_Rtb"
    CodeBehind="Rtb.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<script language="javascript" type="text/javascript">
    var editor = null;

    function OnClientLoad(sender) {
        editor = sender;
    }

    function modulekle() {
        var e = document.getElementById("editorOptions");
        var data = e.options[e.selectedIndex].value;
        var editor = $find("<%= RadEditor1.ClientID %>"); //get a reference to RadEditor client object
        editor.pasteHtml(data);
    }
</script>
<telerik:RadEditor ID="RadEditor1" runat="server" AllowScripts="True" Width="100%" Height="500px" ContentAreaMode="Iframe">
    <CssFiles>
        <telerik:EditorCssFile Value="" />
    </CssFiles>
    <DocumentManager ViewPaths="~/FileManager" UploadPaths="~/FileManager"
        DeletePaths="~/FileManager" MaxUploadFileSize="10240000" />
    <FlashManager ViewPaths="~/FileManager" UploadPaths="~/FileManager" DeletePaths="~/FileManager"
        MaxUploadFileSize="10240000" />
    <ImageManager ViewPaths="~/FileManager" ViewMode="Grid" UploadPaths="~/FileManager"
        DeletePaths="~/FileManager/Image" />
    <MediaManager ViewPaths="~/FileManager" UploadPaths="~/FileManager" DeletePaths="~/FileManager"
        MaxUploadFileSize="10240000" />
    <Modules>
        <telerik:EditorModule Name="RadEditorHtmlInspector" Visible="false" />
        <telerik:EditorModule Name="RadEditorNodeInspector" Visible="true" />
        <telerik:EditorModule Name="RadEditorDomInspector" Visible="false" />
        <telerik:EditorModule Name="RadEditorStatistics" Visible="false" />
    </Modules>
    <Tools>
        <telerik:EditorToolGroup Tag="MainToolbar">
            <telerik:EditorTool Name="FindAndReplace" ShortCut="CTRL+F" />
            <telerik:EditorTool Name="SelectAll" ShortCut="CTRL+A" />
            <telerik:EditorTool Name="Cut" />
            <telerik:EditorTool Name="Copy" ShortCut="CTRL+C" />
            <telerik:EditorTool Name="Paste" ShortCut="CTRL+V" />
            <telerik:EditorTool Name="PasteStrip" />
            <telerik:EditorTool Name="Undo" />
            <telerik:EditorTool Name="Redo" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="ImageManager" ShortCut="CTRL+G" />
            <telerik:EditorTool Name="DocumentManager" />
            <telerik:EditorTool Name="FlashManager" />
            <telerik:EditorTool Name="MediaManager" />
            <telerik:EditorTool Name="LinkManager" ShortCut="CTRL+K" />
            <telerik:EditorTool Name="Unlink" ShortCut="CTRL+SHIFT+K" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="Superscript" />
            <telerik:EditorTool Name="Subscript" />
            <telerik:EditorTool Name="ConvertToLower" />
            <telerik:EditorTool Name="ConvertToUpper" />
            <telerik:EditorTool Name="InsertParagraph" />
            <telerik:EditorTool Name="InsertGroupbox" />
            <telerik:EditorTool Name="InsertHorizontalRule" />
            <telerik:EditorTool Name="FormatCodeBlock" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="FormatBlock" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="FontName" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="RealFontSize" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="Bold" ShortCut="CTRL+B" />
            <telerik:EditorTool Name="Italic" ShortCut="CTRL+I" />
            <telerik:EditorTool Name="Underline" ShortCut="CTRL+U" />
            <telerik:EditorTool Name="StrikeThrough" />
            <telerik:EditorTool Name="JustifyLeft" />
            <telerik:EditorTool Name="JustifyCenter" />
            <telerik:EditorTool Name="JustifyRight" />
            <telerik:EditorTool Name="JustifyFull" />
            <telerik:EditorTool Name="JustifyNone" />
            <telerik:EditorTool Name="Indent" />
            <telerik:EditorTool Name="Outdent" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="AbsolutePosition" />
            <telerik:EditorTool Name="InsertOrderedList" />
            <telerik:EditorTool Name="InsertUnorderedList" />
            <telerik:EditorTool Name="ToggleTableBorder" />
            <telerik:EditorTool Name="ForeColor" />
            <telerik:EditorTool Name="BackColor" />
            <telerik:EditorTool Name="FormatStripper" />
        </telerik:EditorToolGroup>
        <telerik:EditorToolGroup>
            <telerik:EditorTool Name="InsertSymbol"></telerik:EditorTool>
            <telerik:EditorTool Name="InsertTable"></telerik:EditorTool>
            <telerik:EditorTool Name="InsertFormElement"></telerik:EditorTool>
            <telerik:EditorTool Name="ImageMapDialog" />
            <telerik:EditorTool Name="ModuleManager" />
            <telerik:EditorTool Name="Zoom" />
            <telerik:EditorTool Name="ToggleScreenMode" ShortCut="F11" />
        </telerik:EditorToolGroup>
    </Tools>
</telerik:RadEditor>
<asp:Panel ID="ChooseTemplate" runat="server" Visible="False">
    <br />
    <table>
        <tr>
            <td>
                <%= AdminResource.lbAddModul %>
                :
            </td>
            <td>
                <select id="editorOptions" onchange=" modulekle();">
                    <option value="">
                        <%= AdminResource.lbChoose %></option>
                    <option value="[MenuContent[">
                        <%= AdminResource.lbContent %></option>
                    <option value="[SiteMapPath[">
                        <%= AdminResource.lbSiteMap %></option>
                    <option value="[ShareSocial[">
                        <%= AdminResource.lbSocialShare %></option>
                    <option value="[NoticeScroller[">
                        <%= AdminResource.lbNoticeScroller %></option>
                    <option value="[NoticeList[">
                        <%= AdminResource.lbNotice %></option>
                    <option value="[MansetScroller[">
                        <%= AdminResource.lbNewsScroller %></option>
                    <option value="[NewsList-Manset[">
                        <%= AdminResource.lbMansetNewsList %></option>
                    <option value="[NewsList[">
                        <%= AdminResource.lbNewsList %></option>
                    <option value="[EmailAdd[">
                        <%= AdminResource.lbEmailAdd %></option>
                    <option value="[RefLogos[">
                        <%= AdminResource.lbSliderImages%></option>
                    <option value="[Survey[">
                        <%= AdminResource.lbSurvey %></option>
                    <option value="[VideoGallery[">
                        <%= AdminResource.lbVideoGallery %></option> 
                    <% if (VCategoriesList != null)
                       {
                           foreach (var item in VCategoriesList)
                           {%>
                    <option value='[vCategory-<%= item.id %>['>
                        <%= item.name %>&nbsp;(<%=AdminResource.lbVideoGallery%>-<%=AdminResource.lbCategory%>)</option>
                    <% }
                       }%>
                    <option value="[PhotoAlbum[">
                        <%= AdminResource.lbPhotoGallery %></option>
                    <% if (PCategoriesList != null)
                       {
                           foreach (var item in PCategoriesList)
                           {%>
                    <option value='[pCategory-<%= item.photoAlbumCategoryId %>['>
                        <%= item.categoryName %>&nbsp;(<%=AdminResource.lbPhotoGallery%>-<%=AdminResource.lbCategory%>)</option>
                    <% }
                       }%>
                    <% if (PAlbumsList != null)
                       {
                           foreach (var item in PAlbumsList)
                           {%>
                    <option value='[pAlbum-<%= item.photoAlbumId %>['>
                        <%= item.albumName %>&nbsp;(<%=AdminResource.lbPhotoGallery%>-<%=AdminResource.lbAlbum%>)</option>
                    <% }
                       }%>
                    <option value="[Products[">
                        <%= AdminResource.lbProduct %></option>
                    <option value="[ProductScroller[">
                        <%= AdminResource.lbProductScroller %></option>
                    <option value="[SSS[">
                        <%= AdminResource.lbFaq %></option>
                    <option value="[Calender[">
                        <%= AdminResource.lbCalender %></option>
                    <option value="[EventList[">
                        <%= AdminResource.lbEvents %></option>
                    <option value="[Doviz[">
                        <%= AdminResource.lbCurrency %></option>
                    <option value="[LatestUpdates[">
                        <%= AdminResource.lbLatestUpdates %></option>
                    <option value="[RandomField[">
                        <%= AdminResource.lbRandomField %></option>
                    <option value="[MainPageSpecial1[">
                        <%= AdminResource.lbStaticField %>-1</option>
                    <option value="[MainPageSpecial2[">
                        <%= AdminResource.lbStaticField %>-2</option>
                    <option value="[MainPageSpecial3[">
                        <%= AdminResource.lbStaticField %>-3</option>
                    <option value="[BannerField1[">
                        <%= AdminResource.lbBannerField %>-1</option>
                    <option value="[BannerField2[">
                        <%= AdminResource.lbBannerField %>-2</option>
                    <option value="[BannerField3[">
                        <%= AdminResource.lbBannerField %>-3</option>
                    <% if (RssList != null)
                       {
                           foreach (var item in RssList)
                           {%>
                    <option value='[Rss-<%= item.Id %>['><%= item.Name%></option>
                    <% }
                       }%>
                    <% if (FormList != null)
                       {
                           foreach (var item in FormList)
                           {%>
                    <option value='[DynamicForm-<%= item.Id %>['>
                        <%= item.Name %></option>
                    <% }
                       }%>
                    <% if (LList != null)
                       {
                           foreach (var item in LList)
                           {%>
                    <option value='[List-<%= item.Id %>['>
                        <%= item.Name %></option>
                    <% }
                       }%>
                </select>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
