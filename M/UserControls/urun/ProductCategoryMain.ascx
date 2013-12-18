<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_ProductCategoryMain"
    CodeBehind="ProductCategoryMain.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lbTitle" runat="server"><%= Resource.titleProductCategoryMain %></asp:Label>
</h1>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:ListView ID="ListView1" runat="server" GroupItemCount="5" GroupPlaceholderID="groupPlaceHolder"
                ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </GroupTemplate>
                <ItemTemplate>
                    <a href="urunler-<%#Eval("ProductCategoryId") %>-1">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <img width="125px" class="contentimage" src="<%#GetThumbnailPath(Eval("ProductCategoryId").ToString()) %>"
                                        alt="<%#Eval("Name") %>" />
                                    <div class="listname">
                                        <%#Eval("Name")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <hr />
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 6)
   { %>
<asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
    QueryStringField="page" OnLoad="DataPager1_Init">
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