<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_Products"
            CodeBehind="Products.ascx.cs" %>
<h1>
    <asp:Label ID="Label1" runat="server"></asp:Label>
</h1>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Literal runat="server" ID="ltSubCategories"></asp:Literal>
        </td>
    </tr>
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
                    <a href="urundetay-<%#Eval(
                                              "ProductId") %>-<%#                UrlMapping.cevir(Eval("Name").ToString()) %>">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <img width="125px" class="contentimage" src="<%#                                        GetThumbnailPath(Eval("ProductId").ToString()) %>"
                                         alt="<%#Eval("Name") %>" />
                                    <div class="listname">
                                        <%#Eval("Name") %>
                                    </div>
                                    <div class="listbrief">
                                        <%#                GetPrice(Convert.ToInt32(Eval("ProductId"))) %>
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
<% if (DataPager1.TotalRowCount > 10)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="10"
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