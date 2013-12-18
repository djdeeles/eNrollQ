<%@ Page Title="" Language="C#" MasterPageFile="~/M/MasterPage.master" AutoEventWireup="true"
    CodeBehind="RandomField.aspx.cs" Inherits="eNroll.m_RandomField" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="server">

    <asp:MultiView runat="server" ID="mvRandomFields">
        <asp:View runat="server" ID="vDetail">
            <h1>
                <asp:Literal runat="server" ID="ltTitle" /></h1>
            <p>
                <asp:Literal runat="server" ID="ltContent" />
            </p>
            <a data-role="button" data-corners="false" href='sayfa-r-1'>
                <%=Resource.lbViewAll%></a>
            <a href="/m/" data-role="button" data-corners="false">
                <%= Resource.lbHome %></a>
        </asp:View>
        <asp:View runat="server" ID="vAll">
            <asp:ListView ID="lvAllRandomFields" runat="server" DataSourceID="edsCustomerRandom"
                ItemPlaceholderID="PlaceHolder1">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%#Bind("Id") %>'
                        OnDataBinding="HyperLink2DataBinding">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="listname">
                                    <%#Eval("Title") %>
                                </div>
                                <div class="listbrief">
                                    <%#Eval("Summary") %>
                                </div>
                            </td>
                        </tr>
                    </table>
                    </asp:HyperLink>
                    <hr />
                </ItemTemplate>
                <LayoutTemplate>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                </LayoutTemplate>
            </asp:ListView>
            <% if (DataPager1.TotalRowCount > 5)
               { %>
            <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lvAllRandomFields"
                PageSize="5" OnInit="DataPager1Init" QueryStringField="randomfieldspage">
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
            <asp:EntityDataSource ID="edsCustomerRandom" runat="server" ConnectionString="name=Entities"
                DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Customer_Random"
                OrderBy="it.UpdatedTime DESC" Where="" EntityTypeFilter="" Select="">
                <WhereParameters>
                    <asp:ControlParameter ControlID="hfLanguageId" Name="languageId" PropertyName="Value"
                        DbType="Int32" />
                </WhereParameters>
            </asp:EntityDataSource>
            <asp:HiddenField ID="hfLanguageId" runat="server" />
        </asp:View>
    </asp:MultiView>

</asp:Content>
