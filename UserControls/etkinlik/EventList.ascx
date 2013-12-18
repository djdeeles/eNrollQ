<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_EventList" Codebehind="EventList.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<div id="activityList" runat="server">
    <asp:ListView ID="ListView1" runat="server" GroupPlaceholderID="groupPlaceHolder"
                  ItemPlaceholderID="itemPlaceHolder">
        <LayoutTemplate>
            <table cellpadding="3" border="0" cellspacing="" width="100%">
                <tr>
                    <td style="font-weight: bold; text-align: left; width: 25%;">
                        <asp:Label ID="lbActivityName" runat="server"><%= Resource.lbActivityName %></asp:Label>
                    </td>
                    <td style="font-weight: bold; text-align: left; width: 50%;">
                        <asp:Label ID="Label1" runat="server"><%= Resource.lbActivityDesc %></asp:Label>
                    </td>
                    <td style="font-weight: bold; text-align: left; width: 25%;">
                        <asp:Label ID="Label2" runat="server"><%= Resource.lbActivityDate %></asp:Label>
                    </td>
                </tr>
                <tbody>
                    <div>
                        <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
                    </div>
                </tbody>
            </table>
        </LayoutTemplate>
        <GroupTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
        </GroupTemplate>
        <ItemTemplate>
            <tr>
                <td valign="top">
                    <a <%#(Convert
                                                                                                    .
                                                                                                    ToDateTime
                                                                                                    (Eval
                                                                                                         ("EndDate")) <
                                                                                                DateTime
                                                                                                    .
                                                                                                    Now
                                                                                                    ? "style='text-decoration:line-through;'"
                                                                                                    : "") %>
                        href="/etkinlik-<%#Eval("id") %>-<%#                UrlMapping.cevir(Eval("Name").ToString()) %>">
                        <%#Eval("Name").ToString() %></a>
                </td>
                <td valign="top">
                    <a <%#                (Convert.ToDateTime(Eval("EndDate")) < DateTime.Now ? "style='text-decoration:line-through;'" : "") %>
                        href="/etkinlik-<%#Eval("id") %>-<%#                UrlMapping.cevir(Eval("Name").ToString()) %>">
                        <%#Eval("Description") %></a>
                </td>
                <td valign="top">
                    <a <%#                (Convert.ToDateTime(Eval("EndDate")) < DateTime.Now
                     ? "style='text-decoration:line-through;'"
                     : "") %>
                        href="/etkinlik-<%#Eval("id") %>-<%#                UrlMapping.cevir(Eval("Name").ToString()) %>">
                        <%#                Convert.ToDateTime(Eval("StartDate")).ToShortDateString() %>
                        &nbsp;-&nbsp;
                        <%#                Convert.ToDateTime(Eval("EndDate")).ToShortDateString() %></a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    <% if (DataPager1.TotalRowCount > 20)
       { %>
        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="20"
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
</div>
<div id="ActivityDetail" runat="server">
    <h1>
        <asp:Literal ID="lName" runat="server"></asp:Literal>
    </h1>
    <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td colspan="3">
                <asp:Literal ID="lDetails" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold;">
                <%= Resource.lbActivityStartDate %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:Literal ID="lStartDate" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold;">
                <%= Resource.lbActivityEndDate %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:Literal ID="lEndDate" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:HyperLink CssClass="button" ID="HyperLink1" runat="server" NavigateUrl="/etkinlikler-1"><%= Resource.lbBack %></asp:HyperLink>
            </td>
        </tr>
    </table>
</div>