<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_Products"
            CodeBehind="Products.ascx.cs" %>
<h1>
    <asp:Label ID="Label1" runat="server"></asp:Label></h1>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td valign="top">
            <asp:Literal runat="server" ID="ltSubCategories"></asp:Literal>
        </td>
        <td valign="top">
            <asp:ListView ID="ListView1" runat="server" GroupItemCount="5" GroupPlaceholderID="groupPlaceHolder"
                          ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </GroupTemplate>
                <ItemTemplate>
                    <div class="products">
                        <a href="urundetay-<%#Eval(
                                                  "ProductId") %>-<%#                                        UrlMapping.cevir(Eval("Name").ToString()) %>">
                            <img width="100px" style="margin: 0; padding: 0;" src="<%#                                        GetThumbnailPath(Eval("ProductId").ToString()) %>"
                                 alt="<%#Eval("Name") %>" />
                        </a>
                        <div style="float: left; margin: 6px; width: 110px;">
                            <a href="urundetay-<%#                                        Eval("ProductId") %>-<%#                                        UrlMapping.cevir(Eval("Name").ToString()) %>">
                                <%#                Eval("Name").ToString().Replace('"', ' ').Replace("'", "") %>
                            </a>
                            <%#                GetPrice(Convert.ToInt32(Eval("ProductId"))) %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 16)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="16"
                   QueryStringField="page" OnInit="DataPager1_Init">
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
<asp:HiddenField ID="hfCategoryId" runat="server" />