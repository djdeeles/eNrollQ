<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Rss.ascx.cs" Inherits="UserControls_Rss" %>
<h1>
    <%= SelectedRss.Name %></h1>
<asp:ListView ID="ListView1" runat="server" ItemPlaceholderID="PlaceHolder1">
    <ItemTemplate>
        <div class="rsslistitem">
            <div class='rsslistname'>
                <a href='<%#Eval(
                                "Link") %>' target='_blank'>
                    <%#Eval("Title") %></a>
            </div>
            <div class='rsslistbrief'>
                <%#Eval("Description") %></div>
        </div>
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