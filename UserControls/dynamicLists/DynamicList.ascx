<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicList.ascx.cs"
            Inherits="UserControls_DynamicList" %>
<%@ Import Namespace="Resources" %>
<% if (ListTitle != null)
   {%>
    <%= ListTitle %>
<% } %>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" ItemPlaceholderID="PlaceHolder1">
    <ItemTemplate>
        <div class="listitem">
            <div class="listimage">
                <asp:Image ID="Image1" runat="server" Width="150px" ImageAlign="Middle" ImageUrl='<%#(Eval("ThumbnailPath")
                                                                                                    .
                                                                                                    ToString
                                                                                                    () !=
                                                                                                ""
                                                                                                    ? Eval
                                                                                                          ("ThumbnailPath")
                                                                                                          .
                                                                                                          ToString
                                                                                                          ()
                                                                                                    : "../../App_Themes/mainTheme/images/noimage.png") %>' GenerateEmptyAlternateText="True" />
            </div>
            <div class="listname">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Bind("Id") %>' Text='<%#Eval("Title") %>'
                               OnDataBinding="HyperLink1_DataBinding"></asp:HyperLink>
            </div>
            <div class="listbrief">
                <%#Eval("Description") %>
            </div>
            <div class="listcontinue">
                <asp:HyperLink ID="HyperLink2" runat="server" Text='<%#Resource.details %>' NavigateUrl='<%#Bind("Id") %>'
                               OnDataBinding="HyperLink1_DataBinding"></asp:HyperLink>
            </div>
        </div>
    </ItemTemplate>
    <LayoutTemplate>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
</asp:ListView>
<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="ListData">
</asp:EntityDataSource>
<% if (DataPager1.TotalRowCount > 5)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5"
                   OnInit="DataPager1_Init" QueryStringField="listpage">
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
<div style="float: right;">
    <%= Resource.lbSort %>:&nbsp;
    <asp:Button runat="server" ID="btOrderbyTitle" CssClass="button"  ViewStateMode="Enabled"
                OnClick="BtOrderbyTitleClick" />
    <asp:Button runat="server" ID="btOrderbyUpdatedTime" CssClass="button"
                OnClick="BtOrderbyUpdatedTimeClick" />
    <asp:Button runat="server" ID="btOrderbyAscDesc" CssClass="buttonactive"
                OnClick="BtOrderbyAscDescClick" />
</div>
<asp:HiddenField runat="server" ID="hfOrderBy" />