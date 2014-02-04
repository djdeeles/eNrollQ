<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_Albums" CodeBehind="Albums.ascx.cs" %>
<h1>
    <asp:Label ID="lblCategoryName" runat="server"></asp:Label></h1>
<h2>
    <asp:Label ID="lblCategoryNote" runat="server"></asp:Label></h2>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
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
                    <a href="/m/albumdetay-<%#Eval(
                                                  "photoAlbumId") %>-1">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Image ID="Image1" runat="server" Width="130px" ImageUrl='<%#Eval("photoAlbumId") %>'
                                               CssClass="contentimage" OnDataBinding="imgAlbum_DataBinding" />
                                    <div class="listname">
                                        <%#Eval("albumName") %>
                                    </div>
                                    <div class="listbrief">
                                        <%#Eval("albumNote") %>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </a>
                    <hr />
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 9)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="9"
                   OnLoad="DataPager1_Init" QueryStringField="albumpage">
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