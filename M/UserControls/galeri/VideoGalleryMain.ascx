<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="M_userControls_VideoGalleryMain" Codebehind="VideoGalleryMain.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <%= Resource.titleVideoGalleryMain %></h1>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" GroupPlaceholderID="groupPlaceHolder"
              ItemPlaceholderID="itemPlaceHolder">
    <LayoutTemplate>
        <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
        <br />
    </LayoutTemplate>
    <GroupTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
    </GroupTemplate>
    <ItemTemplate>
        <a href='/m/albumvideolari-<%#Eval(
                                          "id") %>-1'>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <img src='/App_Themes/mainTheme/images/vid.png' class="contentimage" alt='<%#Eval("Name") %>' width='125px'
                             max-height='100px' />
                        <b>
                            <%#Eval("Name") %></b>
                    </td>
                </tr>
            </table>
        </a>
        <hr />
    </ItemTemplate>
</asp:ListView>
<% if (DataPager1.TotalRowCount > 6)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
                   OnLoad="DataPager1_Init" QueryStringField="videogallerypage">
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
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="VideoCategories"
                      Where="it.languageId=@languageId and it.State=True" OrderBy="it.Id DESC">
    <WhereParameters>
        <asp:ControlParameter Name="languageId" ControlID="HiddenField1" PropertyName="Value"
                              DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField runat="server" ID="HiddenField1" />