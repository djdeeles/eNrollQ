<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_duyuru_NoticeList"
            CodeBehind="NoticeList.ascx.cs" %>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" ItemPlaceholderID="PlaceHolder1">
    <ItemTemplate>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Bind
                                                                                                   ("noticeId") %>'
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
                            <%#                                        Convert.ToDateTime(Eval("startDate")).ToShortDateString() + " " +
                                        Convert.ToDateTime(Eval("startDate")).ToShortTimeString() %>
                        </div>
                        <div class="listbrief">
                            <%#Eval("description") %>
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
<% if (DataPager1.TotalRowCount > 6)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
                   OnLoad="DataPager1_OnLoad" QueryStringField="noticelistpage">
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
<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Notices"
                      Where="(it.state=true and it.languageId=@languageId and (it.stopDate>=@Tarih or it.stopDate is null))"
                      OrderBy="it.startDate desc">
    <WhereParameters>
        <asp:ControlParameter ControlID="HiddenField1" Name="languageId" PropertyName="Value"
                              DbType="Int32" />
        <asp:ControlParameter ControlID="HiddenField2" Name="Tarih" DbType="DateTime" PropertyName="Value" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:HiddenField ID="HiddenField2" runat="server" />