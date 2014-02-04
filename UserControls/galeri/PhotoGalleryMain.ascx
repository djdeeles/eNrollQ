<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="UserControls_PhotoGalleryMain" Codebehind="PhotoGalleryMain.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <%= Resource.titlePhotoGalleryMain %></h1>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:ListView ID="ListView1" runat="server" GroupItemCount="5" DataSourceID="EntityDataSource1"
                          GroupPlaceholderID="groupPlaceHolder" ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td>
                                <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                            </td>
                        </tr>
                    </table>
                    <br />
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </GroupTemplate>
                <ItemTemplate>
                    <div class="albummaincat">
                        <div style="float: none; font-size: 12px; font-weight: bold; margin: 0 auto; text-align: center; width: 140px;">
                            <%#Eval
                                                                                                   ("categoryName") %>
                        </div>
                        <div style="float: none; height: 100px; margin: 0 auto; overflow: hidden; padding: 5px 0 0 0; text-align: center; width: 140px;">
                            <a href="../../albumler-<%#                                        Eval("photoAlbumCategoryId") %>-1">
                                <asp:Image ID="imgAlbum" runat="server" Width="130px" ImageUrl='<%#                                        Eval("photoAlbumCategoryId") %>'
                                           OnDataBinding="imgAlbum_DataBinding" /></a>
                        </div>
                        <div style="float: none; font-size: 11px; font-weight: normal; padding: 3px 0 3px 0; text-align: center;">
                            <%#                                        Eval("categoryNotes") %>
                        </div>
                        <div style="font-size: 11px; text-align: center;">
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#                Eval("photoAlbumCategoryId") %>'
                                           OnDataBinding="HyperLink1_DataBinding" Width="120">[HyperLink1]</asp:HyperLink>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 6)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
                   OnInit="DataPager1_Init" QueryStringField="photogaleripage">
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
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Def_photoAlbumCategory"
                      Where="it.languageId=@languageId and it.State=True">
    <WhereParameters>
        <asp:ControlParameter Name="languageId" ControlID="HiddenField1" PropertyName="Value"
                              DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />