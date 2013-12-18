<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="UserControls_ProductCategoryMain" Codebehind="ProductCategoryMain.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lbTitle" runat="server"><%= Resource.titleProductCategoryMain %></asp:Label></h1>
<table border="0" cellpadding="0" cellspacing="0">
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
                    <div class="productsmain">
                        <a href="urunler-<%#Eval("ProductCategoryId") %>-1">
                            <img width="100px" style="margin: 0; padding: 0;" src="<%#GetThumbnailPath
                                                                                                   (Eval
                                                                                                        ("ProductCategoryId")
                                                                                                        .
                                                                                                        ToString
                                                                                                        ()) %>"
                                 alt="<%#Eval("Name") %>" />
                        </a>
                        <div style="float: left; margin: 6px; width: 110px;">
                            <a href="urunler-<%#Eval("ProductCategoryId") %>-1">
                                <%#                Eval("Name").ToString().Replace('"', ' ').Replace("'", "") %><br />
                            </a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>
<% if (DataPager1.TotalRowCount > 6)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
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