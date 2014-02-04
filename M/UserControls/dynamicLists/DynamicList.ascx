<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicList.ascx.cs" 
            Inherits="M_UserControls_DynamicList" %>
<% if (ListTitle != null)
   {%>
    <%= ListTitle %> 
<% } %> 
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" ItemPlaceholderID="PlaceHolder1"> 
    <ItemTemplate> 
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl='<%#Bind("Id") %>' OnDataBinding="HyperLink1_DataBinding"> 
            <table width="100%" border="0" cellpadding="0" cellspacing="0"> 
                <tr>
                    <td>
                        <asp:Image ID="Image2" runat="server" Width="125px" CssClass="contentimage" 
                                   ImageUrl='<%#(Eval("ThumbnailPath")
                                                                                                    .
                                                                                                    ToString
                                                                                                    () !=
                                                                                                ""
                                                                                                    ? Eval
                                                                                                          ("ThumbnailPath")
                                                                                                          .
                                                                                                          ToString
                                                                                                          ()
                                                                                                    : "../../../App_Themes/mainTheme/images/noimage.png") %>' GenerateEmptyAlternateText="True" />
                        <div class="listname">
                            <%#Eval("Title") %>
                        </div> 
                        <div class="listbrief">
                            <%#Eval("Description") %>
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
<asp:Button runat="server" ID="btOrderbyTitle" CssClass="button" data-corners="false"
            ViewStateMode="Enabled" OnClick="BtOrderbyTitleClick" />
<asp:Button runat="server" ID="btOrderbyUpdatedTime" CssClass="button" data-corners="false"
            OnClick="BtOrderbyUpdatedTimeClick" />
<asp:Button runat="server" ID="btOrderbyAscDesc" CssClass="buttonactive" data-corners="false"
            OnClick="BtOrderbyAscDescClick" />
<asp:HiddenField runat="server" ID="hfOrderBy" />