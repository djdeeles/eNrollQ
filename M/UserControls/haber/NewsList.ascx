<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_haber_News"
            CodeBehind="NewsList.ascx.cs" %>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" ItemPlaceholderID="PlaceHolder1">
    <ItemTemplate>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Bind
                                                                                                   ("newsId") %>'
                       OnDataBinding="HyperLink1_DataBinding">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%#                                        (Eval("thumbnailPath").ToString() != ""
                                             ? Eval("thumbnailPath").ToString()
                                             : "../../../App_Themes/mainTheme/images/noimage.png") %>' Width="125px"
                                   GenerateEmptyAlternateText="True" CssClass="contentimage" />
                        <div class="listname">
                            <%#                                        Eval("header") %>
                        </div>
                        <div class="listdate">
                            <%#                                        Convert.ToDateTime(Eval("enterDate")).ToShortDateString() + " " +
                                        Convert.ToDateTime(Eval("enterDate")).ToShortTimeString() %>
                        </div>
                        <div class="listbrief">
                            <%#Eval("brief") %>
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
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5"
                   OnLoad="DataPager1_OnLoad" QueryStringField="newslistpage">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="True" ShowNextPageButton="False"
                                        ShowPreviousPageButton="True" />
            <asp:NumericPagerField />
            <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowNextPageButton="True"
                                        ShowPreviousPageButton="False" />
        </Fields>
    </asp:DataPager>
<% } %><asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                             DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="News"
                             OrderBy="it.enterDate DESC" Where="it.state=true and it.languageId=@languageId"
                             EntityTypeFilter="" Select="">
           <WhereParameters>
               <asp:ControlParameter ControlID="HiddenField1" Name="languageId" PropertyName="Value"
                                     DbType="Int32" />
           </WhereParameters>
       </asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />