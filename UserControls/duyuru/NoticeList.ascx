<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_duyuru_NoticeList"
            CodeBehind="NoticeList.ascx.cs" %>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" ItemPlaceholderID="PlaceHolder1">
    <ItemTemplate>
        <div class="noticelistitem">
            <div class="noticelistimage">
                <asp:Image ID="Image1" runat="server" ImageUrl='<%#(Eval
                                                                                                    ("thumbnailPath")
                                                                                                    .
                                                                                                    ToString
                                                                                                    () !=
                                                                                                ""
                                                                                                    ? Eval
                                                                                                          ("thumbnailPath")
                                                                                                          .
                                                                                                          ToString
                                                                                                          ()
                                                                                                    : "../../App_Themes/mainTheme/images/noimage.png") %>'
                           Width="150px" GenerateEmptyAlternateText="True" ImageAlign="Middle" />
            </div>
            <div class="noticelistname">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Bind("noticeId") %>'
                               Text='<%#Eval("header") %>' OnDataBinding="HyperLink1_DataBinding"></asp:HyperLink>
            </div>
            <div class="noticelistdate">
                <%#                Convert.ToDateTime(Eval("startDate")).ToShortDateString() + " " +
                Convert.ToDateTime(Eval("startDate")).ToShortTimeString() %>
            </div>
            <div class="noticelistbrief">
                <%#Eval("description") %>
            </div>
            <div class="noticelistcontinue">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%#Bind("noticeId") %>'
                               OnDataBinding="HyperLink2_DataBinding"></asp:HyperLink>
            </div>
        </div>
    </ItemTemplate>
    <LayoutTemplate>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
</asp:ListView>
<% if (DataPager1.TotalRowCount > 6)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
                   OnInit="DataPager1_Init" QueryStringField="noticelistpage">
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
                      Where="(it.state=true and it.languageId=@languageId and it.startDate<=@Tarih and (it.stopDate>=@Tarih or it.stopDate is null))"
                      OrderBy="it.startDate desc">
    <WhereParameters>
        <asp:ControlParameter ControlID="HiddenField1" Name="languageId" PropertyName="Value"
                              DbType="Int32" />
        <asp:ControlParameter ControlID="HiddenField2" Name="Tarih" DbType="DateTime" PropertyName="Value" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:HiddenField ID="HiddenField2" runat="server" />