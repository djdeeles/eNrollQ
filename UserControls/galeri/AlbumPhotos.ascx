<%@ Control Language="C#" AutoEventWireup="true" Inherits="uye_userControls_photoAlbum" Codebehind="AlbumPhotos.ascx.cs" %>
<title id="PageTitle" runat="server"></title>
<h1>
    <asp:Label ID="lblAlbumName" runat="server"></asp:Label></h1>
<p>
    <asp:Label ID="lblAlbumNote" runat="server"></asp:Label></p>
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
                    <div class="albumphoto">
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="duyuru" NavigateUrl='<%#Eval
                                                                                                   ("photoId") %>'
                                       Font-Bold="true" rel="prettyPhoto[pp_gal]" Font-Size="14px" OnDataBinding="HyperLink1_DataBinding">
                            <asp:Image ID="Image1" runat="server" ImageUrl='<%#Eval("photoId") %>' OnDataBinding="Image1_DataBinding" />
                        </asp:HyperLink>
                        <br /> <%#                                        Eval("photoName") %>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 20)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="20"
                   OnInit="DataPager1_Init" QueryStringField="photoalbumpage">
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