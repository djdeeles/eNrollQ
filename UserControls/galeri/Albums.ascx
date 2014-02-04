<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_Albums" Codebehind="Albums.ascx.cs" %>
<h1> <asp:Label ID="lblCategoryName" runat="server"></asp:Label></h1><h2>
                                                                         <asp:Label ID="lblCategoryNote" runat="server"></asp:Label></h2>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:ListView ID="ListView1" runat="server" GroupItemCount="5" DataSourceID="EntityDataSource1"
                          GroupPlaceholderID="groupPlaceHolder" ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </GroupTemplate>
                <ItemTemplate>
                    <div class="albums">
                        <div style="float: none; font-size: 12px; font-weight: bold; margin: 0 auto; text-align: center; width: 140px;">
                            <%#Eval
                                                                                                   ("albumName") %>
                        </div>
                        <div style="float: none; height: 100px; margin: 0 auto; overflow: hidden; padding: 5px 0 0 0; text-align: center; width: 140px;">
                            <a href="../../albumdetay-<%#                                        Eval("photoAlbumId") %>-1">
                                <asp:Image ID="imgAlbum" runat="server" Width="130px" ImageUrl='<%#                                        Eval("photoAlbumId") %>'
                                           OnDataBinding="imgAlbum_DataBinding" /></a>
                        </div>
                        <div style="float: none; font-size: 11px; font-weight: normal; padding: 3px 0 3px 0; text-align: center;">
                            <%#Eval("albumNote") %>
                        </div>
                        <div style="font-size: 11px; text-align: center;">
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("photoAlbumId") %>'
                                           OnDataBinding="HyperLink1_DataBinding" Width="120">[HyperLink1]</asp:HyperLink>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 9)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="9"
                   OnInit="DataPager1_Init" QueryStringField="albumpage">
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
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Def_photoAlbum"
                      Where="it.languageId=@languageId and it.photoAlbumCategoryId=@categoryid and it.State=True">
    <WhereParameters>
        <asp:ControlParameter Name="languageId" ControlID="HiddenField1" PropertyName="Value"
                              DbType="Int32" />
        <asp:ControlParameter ControlID="HiddenField2" Name="categoryid" PropertyName="Value"
                              DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:HiddenField ID="HiddenField2" runat="server" />