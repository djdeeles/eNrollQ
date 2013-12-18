<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Rss.ascx.cs" Inherits="M_UserControls_Rss" %>
<h1>
    <%= SelectedRss.Name %></h1>
<asp:ListView ID="ListView1" runat="server" ItemPlaceholderID="PlaceHolder1">
    <ItemTemplate>
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank" NavigateUrl='<%#Bind("Link") %>'>
                        <div class="listname">
                            <%#Eval("Title")%>
                        </div>
                    </asp:HyperLink>
                    <div class="listbrief">
                        <%#Eval("Description")%>
                    </div>
                </td>
            </tr>
        </table>
        <hr />
    </ItemTemplate>
    <LayoutTemplate>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
</asp:ListView>
<% if (DataPager1.TotalRowCount > 5)
   { %>
<asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5"
    OnInit="DataPager1_Init" QueryStringField="page">
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
<asp:Literal ID="ltRssItemsScroll" runat="server">
</asp:Literal>