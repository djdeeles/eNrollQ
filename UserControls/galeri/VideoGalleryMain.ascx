<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="userControls_VideoGalleryMain" Codebehind="VideoGalleryMain.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1><%= Resource.titleVideoGalleryMain %></h1>
<asp:ListView ID="ListView1" runat="server" DataSourceID="EntityDataSource1" GroupPlaceholderID="groupPlaceHolder"
              ItemPlaceholderID="itemPlaceHolder">
    <LayoutTemplate>
        <div>
            <asp:PlaceHolder ID="groupPlaceHolder" runat="server"></asp:PlaceHolder>
        </div>
        <br />
    </LayoutTemplate>
    <GroupTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
    </GroupTemplate>
    <ItemTemplate>
        <div class="videomaincat">
            <div>
                <a href='/albumvideolari-<%#Eval(
                                                "id") %>-1'>
                    <img src='/App_Themes/mainTheme/images/vid.png' alt='<%#Eval("Name") %>' width='130px'
                         height='100px' /></a></div>
            <span><a href='../albumvideolari-<%#Eval("id") %>-1'>
                      <%#Eval("Name") %></a></span>
        </div>
    </ItemTemplate>
</asp:ListView>
<% if (DataPager1.TotalRowCount > 6)
   { %>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="6"
                   OnInit="DataPager1_Init" QueryStringField="videogallerypage">
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
                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="VideoCategories"
                      Where="it.languageId=@languageId and it.State=True"
                      OrderBy="it.Id DESC">
    <WhereParameters>
        <asp:ControlParameter Name="languageId" ControlID="HiddenField1" PropertyName="Value"
                              DbType="Int32" />
    </WhereParameters>
</asp:EntityDataSource>

<asp:HiddenField runat="server" ID="HiddenField1"/>