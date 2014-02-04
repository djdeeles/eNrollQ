<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_AlbumPhotos" Codebehind="AlbumPhotos.ascx.cs" %>
<script src="App_Themes/mainTheme/js/klass.min.js" type="text/javascript"></script>
<script src="App_Themes/mainTheme/js/code.photoswipe-3.0.5.min.js" type="text/javascript"></script>
<link href="App_Themes/mainTheme/css/photoswipe.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    (function(window, PhotoSwipe) {
        document.addEventListener('DOMContentLoaded', function() {
            var options = { },
                instance = PhotoSwipe.attach(window.document.querySelectorAll('#Gallery a'), options);
        }, false);
    }(window, window.Code.PhotoSwipe));
</script>
<h1>
    <asp:Label ID="lblAlbumName" runat="server"></asp:Label>
</h1>
<h2>
    <asp:Label ID="lblAlbumNote" runat="server"></asp:Label>
</h2>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <ul id="Gallery" class="gallery">
                <asp:ListView ID="ListView1" runat="server" GroupItemCount="5" DataSourceID="EntityDataSource1"
                              GroupPlaceholderID="groupPlaceHolder" ItemPlaceholderID="itemPlaceHolder">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                    </LayoutTemplate>
                    <GroupTemplate>
                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                    </GroupTemplate>
                    <ItemTemplate>
                        <li><a href='<%#Eval(
                                            "photoPath").ToString().Replace("~", "..") %>'>
                                <asp:Image ID="Image1" CssClass="contentimage" runat="server" ImageUrl='<%#Eval("photoId") %>'
                                           OnDataBinding="Image1_DataBinding" />
                                <p><%#Eval("photoName") %></p></a> </li>
                    </ItemTemplate>
                </asp:ListView>
            </ul>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 10)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="10"
                   OnLoad="DataPager1_Init" QueryStringField="photoalbumpage">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="True" ShowNextPageButton="False"
                                        ShowPreviousPageButton="True" FirstPageText="First" LastPageText="Last" NextPageText="Next"
                                        PreviousPageText="Previous" />
            <asp:NumericPagerField />
            <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="True"
                                        ShowPreviousPageButton="False" FirstPageText="First" LastPageText="Last" NextPageText="Next"
                                        PreviousPageText="Previous" />
        </Fields>
    </asp:DataPager>
<% } %>
<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="PhotoAlbum"
                      Where="it.photoAlbumId=@photoAlbumId" OrderBy="it.photoId DESC">
    <WhereParameters>
        <asp:ControlParameter ControlID="HiddenField1" Name="photoAlbumId" PropertyName="Value"
                              DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />