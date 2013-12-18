<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_PhotoGalleryMain"
    CodeBehind="PhotoGalleryMain.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <%= Resource.titlePhotoGalleryMain %></h1>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:ListView ID="ListView1" runat="server" GroupItemCount="5" DataSourceID="EntityDataSource1"
                GroupPlaceholderID="groupPlaceHolder" ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                    <br />
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </GroupTemplate>
                <ItemTemplate>
                    <a href="../../m/albumler-<%#Eval(
                                                     "photoAlbumCategoryId") %>-1">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Image ID="imgAlbum" runat="server" Width="125px" ImageUrl='<%#                                        Eval("photoAlbumCategoryId") %>'
                                        CssClass="contentimage" OnDataBinding="imgAlbum_DataBinding" />
                                    <div class="listname">
                                        <%#Eval("categoryName")%>
                                    </div>
                                    <div class="listbrief">
                                        <%#Eval("categoryNotes")%>
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
<% if (DataPager1.TotalRowCount > 6)
   { %>
<asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
    OnLoad="DataPager1_Init" QueryStringField="photogaleripage">
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
