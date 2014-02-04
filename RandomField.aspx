<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
         CodeBehind="RandomField.aspx.cs" Inherits="eNroll.RandomField" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:MultiView runat="server" ID="mvRandomFields">
        <asp:View runat="server" ID="vDetail">
            <h1>
                <asp:Literal runat="server" ID="ltTitle" /></h1>
            <asp:Literal runat="server" ID="ltContent" /><br />
            <br />
            <a class="button" href='sayfa-r-1'>
                <%= Resource.lbViewAll %></a>
        </asp:View>
        <asp:View runat="server" ID="vAll">
            <asp:ListView ID="lvAllRandomFields" runat="server" DataSourceID="edsCustomerRandom"
                          ItemPlaceholderID="PlaceHolder1">
                <ItemTemplate>
                    <div class="newslistitem">
                        <div class="newslistname">
                            <%#Eval(
                                   "Title") %>
                        </div>
                        <div class="newslistbrief">
                            <%#Eval("Summary") %>
                        </div>
                        <div class="newslistcontinue">
                            <asp:HyperLink ID="HyperLink2" runat="server" CssClass="button" NavigateUrl='<%#Bind("Id") %>'
                                           OnDataBinding="HyperLink2DataBinding"></asp:HyperLink>
                        </div>
                    </div>
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