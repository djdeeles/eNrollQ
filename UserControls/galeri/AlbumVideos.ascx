<%@ Control Language="C#" AutoEventWireup="true" Inherits="userControls_AlbumVideos" Codebehind="AlbumVideos.ascx.cs" %>
<h1>
    <asp:Label ID="lblAlbumName" runat="server"></asp:Label></h1>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" GroupPlaceholderID="groupPlaceHolder"
                          ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </GroupTemplate>
                <ItemTemplate>
                    <div class="videos">
                        <div style="float: left; height: 87px; margin: 10px; overflow: hidden; width: 100px;">
                            <img src='/App_Themes/mainTheme/images/vid.png' alt='<%#Eval
                                                                                                   ("Name") %>' width='100px'
                                 height='87px' />
                        </div>
                        <div style="float: left; margin: 10px; width: 500px;">
                            <a href='<%#                                        Eval("videoURL") %>' rel="prettyPhoto[pp_gal]" title='<%#Eval("Name") %>'>
                                <b>
                                    <%#Eval("Name") %></b></a>
                            <br />
                            <br />
                            <%#                                        Eval("Description") %>
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
                   QueryStringField="videopage" OnInit="DataPager1_Init">
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
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Videos"
                      Where="it.categoryId=@categoryId and it.languageId=@languageId and it.State=True" OrderBy="it.Id DESC">
    <WhereParameters>
        <asp:ControlParameter ControlID="HiddenField1" Name="categoryId" PropertyName="Value"
                              DbType="Int32" />
        <asp:ControlParameter ControlID="HiddenField2" Name="languageId" PropertyName="Value"
                              DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:HiddenField ID="HiddenField2" runat="server" />