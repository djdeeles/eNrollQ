<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberSearch.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.MemberSearch" %>
<%@ Register Src="~/Admin/adminUserControls/MemberSendEmail.ascx" TagName="SendMail"
    TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/MemberSendSms.ascx" TagName="SendSms"
    TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/MemberAddEdit.ascx" TagName="MemberAddEdit"
    TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/MemberFinanceManager.ascx" TagName="MemberFinanceManager"
    TagPrefix="uc1" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuthoriztn">
    <asp:View runat="server" ID="vAuth">
        <asp:MultiView ID="mvMemberSearch" runat="server" ActiveViewIndex="0">
            <asp:View ID="vSearchMember" runat="server">
                <div id="personalCriterias" style="width: 51%; float: left;">
                    
                    <table style="width: 100%;">
                        <thead>
                            <th colspan="3" class="UstBar">
                                <%=AdminResource.lbPersonalInfo %>
                            </th>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="100px">
                                    <%=Resources.Resource.lbName%>
                                </td>
                                <td width="10px">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbName" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.lbSurname%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbSurname" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.lbEmail%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbEmail" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbGender%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGender" Width="200px" runat="server" onchange="showHideAboutGender();" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbBloodType%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" Width="200px" ID="ddlBloodType" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbMaritalStatus%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMaritalStatus" Width="200px" runat="server" onchange="showHideAboutGender();" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbBirthPlace%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbBirthPlace" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="membershipCriterias" style="width: 49%; float: left;">
                    <table style="width: 100%;">
                        <thead>
                            <th colspan="3" class="UstBar">
                                <%=AdminResource.lbMembershipInfo%>
                            </th>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="100px">
                                    <%=Resources.Resource.lbMemberNumber %>
                                </td>
                                <td width="10px">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbMembershipNumber" runat="server" Width="100px" />&nbsp;-
                                    <asp:TextBox ID="tbMembershipNumber2" runat="server" Width="100px" />&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.lbSpecialNumber %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbSpecialNumber" runat="server" Width="100px"></asp:TextBox>&nbsp;-
                                    <asp:TextBox ID="tbSpecialNumber2" runat="server" Width="100px  "></asp:TextBox>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.lbRelationType %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMembershipRelType" Width="224px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.lbTerm %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <telerik:RadMonthYearPicker ID="dpTerm" runat="server" MaxDate="01-2200" MinDate="01-1900"
                                        Width="100px">
                                        <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                        </DateInput>
                                    </telerik:RadMonthYearPicker>
                                    &nbsp;-&nbsp;
                                    <telerik:RadMonthYearPicker ID="dpTerm2" runat="server" MaxDate="01-2200" MinDate="01-1900"
                                        Width="100px">
                                        <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                        </DateInput>
                                    </telerik:RadMonthYearPicker>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=Resources.Resource.lbEnterDate %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dpMembershipDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                                        runat="server" ZIndex="30001" Width="100px">
                                    </telerik:RadDatePicker>
                                    &nbsp;-&nbsp;
                                    <telerik:RadDatePicker ID="dpMembershipDate2" MaxDate="01-01-2200" MinDate="01-01-1900"
                                        runat="server" ZIndex="30001" Width="100px">
                                    </telerik:RadDatePicker>
                                </td>
                            </tr> 
                            <tr>
                                <td>
                                    <%=AdminResource.lbMemberState%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td >
                                    <asp:DropDownList runat="server" ID="ddlMemberState" Width="220px"/>
                                </td>
                            </tr>
                            <tr>
                                <td>    
                                    <%=AdminResource.lbMemberType%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td >
                                    <asp:DropDownList runat="server" ID="ddlIsAdmin" Width="220px"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=AdminResource.lbAutoPaymentOrder%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td >
                                    <asp:DropDownList runat="server" ID="ddlIsAutoPay" Width="220px"/>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <%=AdminResource.lbAdminNote %>
                                </td>
                                <td valign="top">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbAdminNote" TextMode="MultiLine" Width="220px" MaxLength="200"/>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="workInfoCriterias" style="width: 51%; float: left;">
                    <table style="width: 100%;">
                        <thead>
                            <th colspan="3" class="UstBar">
                                <%=AdminResource.lbWorkInfo%>
                            </th>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="100px">
                                    <%= Resources.Resource.lbCountry%>
                                </td>
                                <td width="10px">
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlWorkCountry" AutoPostBack="true" Width="200px"
                                        OnSelectedIndexChanged="ddlWorkCountry_OnSelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbCity%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlWorkCity" AutoPostBack="true" Width="200px"
                                        OnSelectedIndexChanged="ddlWorkCity_OnSelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbTown%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlWorkTown" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbJobSector%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlJobSectors" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbJob%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlJobs" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbWorkTitle%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbWorkTitle" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbWorkCorporation%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbWorkCorparation" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="homeInfoCriterias" style="width: 49%; float: left;">
                    <table style="width: 100%;">
                        <thead>
                            <th colspan="3" class="UstBar">
                                <%=AdminResource.lbHomeInfo%>
                            </th>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="100px">
                                    <%= Resources.Resource.lbCountry%>
                                </td>
                                <td width="10px">
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlHomeCountry" Width="200px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlHomeCountry_OnSelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbCity%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlHomeCity" AutoPostBack="true" Width="200px"
                                        OnSelectedIndexChanged="ddlHomeCity_OnSelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbTown%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlHomeTown" Width="200px" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="educationInfoCriterias" style="width: 51%; float: left;">
                    <table style="width: 100%;">
                        <thead>
                            <th colspan="3" class="UstBar">
                                <%=AdminResource.lbEducationInfo%>
                            </th>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="100px">
                                    <%=Resources.Resource.lbLastSchool %>
                                </td>
                                <td width="10px">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbLastSchool" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=AdminResource.lbGraduation%>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <telerik:RadMonthYearPicker ID="dpLastSchoolGraduateDate" runat="server" MaxDate="01-2200"
                                        Width="80px" MinDate="01-1900">
                                        <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                        </DateInput>
                                    </telerik:RadMonthYearPicker>
                                    &nbsp;-&nbsp;
                                    <telerik:RadMonthYearPicker ID="dpLastSchoolGraduateDate2" runat="server" MaxDate="01-2200"
                                        Width="80px" MinDate="01-1900">
                                        <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                        </DateInput>
                                    </telerik:RadMonthYearPicker>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="otherInfoCriterias" style="width: 49%; float: left;">
                    <table style="width: 100%;">
                        <thead>
                            <th colspan="3" class="UstBar">
                                <%=AdminResource.lbOtherInfo%>
                            </th>
                        </thead>
                        <tbody>
                            <tr>
                                <td width="100px">
                                    <%=AdminResource.lbDecease %>
                                </td>
                                <td width="10px">
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlDecease" Width="220px" AutoPostBack="True" OnSelectedIndexChanged="ddlDecease_OnSelectedIndexChanged"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=AdminResource.lbDeceaseDate %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dpDeceaseDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                                        runat="server" ZIndex="30001" Width="100px" Enabled="False">
                                    </telerik:RadDatePicker>
                                    &nbsp;-&nbsp;
                                    <telerik:RadDatePicker ID="dpDeceaseDate2" MaxDate="01-01-2200" MinDate="01-01-1900"
                                        runat="server" ZIndex="30001" Width="100px" Enabled="False">
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="dvSearchResults" style="float: left; width: 100%">
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btSearch" CssClass="SaveCancelBtn" OnClick="BtSearchClick" />
                                <asp:Button runat="server" ID="btClearSearchCriterias" CssClass="SaveCancelBtn" Visible="False"
                                    OnClick="BtClearSearchCriteriasClick" />
                                <asp:Literal runat="server" ID="ltResults" Visible="False" />
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                <asp:TextBox runat="server" TextMode="MultiLine" ID="tbSqlQueryOutput" Width="700px"
                                    Height="96px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div runat="server" id="dvMemberActions" visible="False" style="float: left; width: 100%">
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="btShowResults" CssClass="SaveCancelBtn" OnClick="BtShowResultsClick" />
                                <asp:Button runat="server" ID="btDownloadUsersExcelList" CssClass="SaveCancelBtn"
                                    OnClick="BtDownloadUsersExcelList" />
                                <asp:Button runat="server" ID="btSendEmail" CssClass="SaveCancelBtn" OnClick="BtSendEmailClick" />
                                <asp:Button runat="server" ID="btSendSms" CssClass="SaveCancelBtn" OnClick="BtSendSmsClick" />
                                <asp:Button runat="server" ID="btFinanceManager" CssClass="SaveCancelBtn" OnClick="BtFinanceManagerClick" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: left; width: 100%">
                    <asp:GridView ID="gVMembers" runat="server" Visible="True" AutoGenerateColumns="False" CssClass="GridViewStyle"
                        OnRowDataBound="gVMembers_OnRowDataBound" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                        EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                        DataKeyNames="uId" CellPadding="4" Width="100%" PageSize="15" ForeColor="#333333"
                        GridLines="None" AllowSorting="False">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                    OnClick="ImgBtnMemberEditClick" CommandArgument='<%#Eval("uId") %>' />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgBtnMemberDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                    CommandArgument='<%#Eval("uId") %>' OnClick="ImgBtnMemberDeleteClick" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("fMembershipNo")%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#string.Format("{0} {1}",Eval("uName"),Eval("uSurname") )%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="uEmail" SortExpression="uEmail" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#eNroll.Helpers.EnrollMembershipHelper.GetMembershipType(Eval("fMembershipType").ToString())%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="isAdmin" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Justify"
                                ItemStyle-Width="50">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Justify" Width="50px" />
                            </asp:CheckBoxField>
                            <asp:CheckBoxField DataField="uState" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Justify"
                                ItemStyle-Width="50">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Justify" Width="50px" />
                            </asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:View>
            <asp:View ID="vEditMember" runat="server">
                <uc1:MemberAddEdit runat="server" ID="cMemberAddEdit" />
                <asp:Button runat="server" OnClick="BtBackToSearchPageClick" CssClass="SaveCancelBtn"
                    ID="btBackToSearchPage" />
            </asp:View>
            <asp:View ID="vSendEmail" runat="server">
                <uc1:SendMail runat="server" ID="cSendMail" />
                <asp:Button runat="server" ID="btCancelSendMail" CssClass="SaveCancelBtn" OnClick="BtCancelSendMailClick" />
            </asp:View>
            <asp:View ID="vSendSms" runat="server">
                <uc1:SendSms runat="server" ID="cSendSms" />
                <asp:Button runat="server" ID="btCancelSendSms" CssClass="SaveCancelBtn" OnClick="BtCancelSendSmsClick" />
            </asp:View>
            <asp:View ID="vFinanceManager" runat="server">
                <uc1:MemberFinanceManager runat="server" ID="cMemberFinanceManager" />
                <asp:Button runat="server" ID="btCancelFinanceManager" CssClass="SaveCancelBtn" OnClick="BtCancelFinanceManagerClick" />
            </asp:View>
        </asp:MultiView>
    </asp:View>
    <asp:View runat="server" ID="vNoAuth">
        <table class="rightcontenttable">
            <tr>
                <td>
                    <p class="noauth">
                        <%= AdminResource.msgUnauthorizedUser %>
                    </p>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
<asp:HiddenField runat="server" ID="hfUserId" />
<asp:HiddenField runat="server" ID="hfSearchResaultCmd" />
<asp:HiddenField runat="server" ID="hfSearchResaultExcellCmd" />
<asp:HiddenField runat="server" ID="hfSearchResaultCountCmd" />
<asp:HiddenField runat="server" ID="hfHomeCountry" />
<asp:HiddenField runat="server" ID="hfHomeCity" />
<asp:HiddenField runat="server" ID="hfHomeTown" />
<asp:HiddenField runat="server" ID="hfWorkCountry" />
<asp:HiddenField runat="server" ID="hfWorkCity" />
<asp:HiddenField runat="server" ID="hfWorkTown" />
<asp:HiddenField runat="server" ID="hfUserStateFilter" Value="" />
<asp:HiddenField runat="server" ID="hfMemberStateFilter" Value="" />
<asp:HiddenField runat="server" ID="hfMemberEmailActivationFilter" Value="" />
