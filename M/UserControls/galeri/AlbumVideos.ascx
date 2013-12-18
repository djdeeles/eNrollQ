<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_AlbumVideos" Codebehind="AlbumVideos.ascx.cs" %>
<h1>
    <asp:Label ID="lblAlbumName" runat="server"></asp:Label></h1>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" GroupPlaceholderID="groupPlaceHolder"
              ItemPlaceholderID="itemPlaceHolder">
    <LayoutTemplate>
        <div>
            <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
        </div>
        <br />
    </LayoutTemplate>
    <GroupTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
    </GroupTemplate>
    <ItemTemplate>
        <a href='<%#Eval
                                                                                                   ("videoURL") %>' rel="prettyPhoto[pp_gal]" title='<%#Eval("Name") %>'>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <img src='/App_Themes/mainTheme/images/vid.png' class="contentimage" alt='<%#Eval("Name") %>'
                             width='125px' max-height='100px' />
                        <b>
                            <%#Eval("Name") %></b><br />
                        <%#                                        Eval("Description") %>
                    </td>
                </tr>
            </table>
        </a>
        <hr />
    </ItemTemplate>
</asp:ListView>
<% if (DataPager1.TotalRowCount > 9)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="9"
                   QueryStringField="videopage" OnLoad="DataPager1_Init">
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
                      Where="it.categoryId=@categoryId and it.languageId=@languageId and it.State=True"
                      OrderBy="it.Id DESC">
    <WhereParameters>
        <asp:ControlParameter ControlID="HiddenField1" Name="categoryId" PropertyName="Value"
                              DbType="Int32" />
        <asp:ControlParameter ControlID="HiddenField2" Name="languageId" PropertyName="Value"
                              DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:HiddenField ID="HiddenField2" runat="server" />