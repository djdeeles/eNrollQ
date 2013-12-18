<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_haber_NewsList"
    CodeBehind="NewsList.ascx.cs" %>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" ItemPlaceholderID="PlaceHolder1">
    <ItemTemplate>
        <div class="newslistitem">
            <div class="newslistimage">
                <asp:Image ID="Image1" runat="server" ImageUrl='<%#(Eval("thumbnailPath").ToString() != "" ? 
                    Eval("thumbnailPath").ToString(): "../../App_Themes/mainTheme/images/noimage.png") %>'
                    Width="150px" GenerateEmptyAlternateText="True" ImageAlign="Middle" />
            </div>
            <div class="newslistname">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Bind("newsId") %>'
                    Text='<%#Eval("header") %>' OnDataBinding="HyperLink1_DataBinding"></asp:HyperLink>
            </div>
            <div class="newslistdate">
                <%#Convert.ToDateTime(Eval("enterDate")).ToShortDateString() + " " + Convert.ToDateTime(Eval("enterDate")).ToShortTimeString()%>
            </div>
            <div class="newslistbrief">
                <%#Eval("brief") %>
            </div>
            <div class="newslistcontinue">
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%#Bind("newsId") %>'
                    OnDataBinding="HyperLink2_DataBinding"></asp:HyperLink>
            </div>
        </div>
    </ItemTemplate>
    <LayoutTemplate>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
</asp:ListView>
<% if (DataPager1.TotalRowCount > 5)
   { %>
<asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="5"
    OnInit="DataPager1_Init" QueryStringField="newslistpage">
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
    DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="News"
    OrderBy="it.enterDate DESC" Where="" EntityTypeFilter="" Select="">
    <WhereParameters>
        <asp:ControlParameter ControlID="HiddenField1" Name="languageId" PropertyName="Value"
            DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:HiddenField ID="hfManset" runat="server" />
