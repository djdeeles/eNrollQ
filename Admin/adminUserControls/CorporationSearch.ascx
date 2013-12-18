<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CorporationSearch.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.CorporationSearch" %>
<%@ Register Src="~/Admin/adminUserControls/CorporationAddEdit.ascx" TagName="CorporationAddEdit"
    TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/CorporationFinanceManager.ascx" TagName="CorporationFinanceManager"
    TagPrefix="uc1" %>
<%@ Import Namespace="eNroll.App_Data" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuthoriztn">
    <asp:View runat="server" ID="vAuth">
        <asp:MultiView ID="mvCorporationSearch" runat="server" ActiveViewIndex="0">
            <asp:View ID="vSearchCorporation" runat="server">
                <asp:Panel runat="server" DefaultButton="btSearch">
                    <div id="corporationCriterias" style="width: 49%; float: left;">

                        <table style="width: 100%;">
                            <thead>
                                <th colspan="3" class="UstBar">
                                    <%=AdminResource.lbCorporationInfo %>
                                </th>
                            </thead>
                            <tbody>
                                <tr>
                                    <td width="100px">
                                        <%=Resources.Resource.lbName%>
                                    </td>
                                    <td width="10px">:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbName" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Vergi Dairesi
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbTaxDept" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%=Resources.AdminResource.lbTaxNumber%>
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbTaxNumber" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="corporationOtherCriterias" style="width: 49%; float: right;">
                        <table style="width: 100%;">
                            <thead>
                                <th colspan="3" class="UstBar">Adres Bilgileri
                               <%-- <%=AdminResource.lbAddressInfo%>--%>
                                </th>
                            </thead>
                            <tbody>
                                <tr>
                                    <td width="100px">
                                        <%= Resources.Resource.lbCountry%>
                                    </td>
                                    <td width="10px">:
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
                                    <td>:
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
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlWorkTown" Width="200px" />
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
                    <div runat="server" id="dvCorporationActions" visible="False" style="float: left; width: 100%">
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <%--<asp:Button runat="server" ID="btShowResults" CssClass="SaveCancelBtn" OnClick="BtShowResultsClick" />--%>
                                    <asp:Button runat="server" ID="btDownloadUsersExcelList" CssClass="SaveCancelBtn"
                                        OnClick="BtDownloadUsersExcelList" />
                                    <asp:Button runat="server" ID="btFinanceManager" CssClass="SaveCancelBtn" OnClick="BtFinanceManagerClick" />
                                    <%--<asp:Button runat="server" ID="btSendEmail" CssClass="SaveCancelBtn" OnClick="BtSendEmailClick" />
                                <asp:Button runat="server" ID="btSendSms" CssClass="SaveCancelBtn" OnClick="BtSendSmsClick" /> --%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left; width: 100%">
                        
                        <script type="text/javascript">
                            function confirmDeleteCorp() {
                                var r = confirm('<%=AdminResource.lbDeletingQuestion%>');
                                if (r == true) { 
                                    return confirm('<%=AdminResource.lbConfirmDeleteCorporation%>');
                                }
                                else {
                                    return false;
                                }
                            }
                        </script>

                        <asp:GridView ID="gVCorporations" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            OnRowDataBound="gVCorporations_OnRowDataBound" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                            SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                            EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                            DataKeyNames="cId" CellPadding="4" Width="100%" PageSize="15" ForeColor="#333333"
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
                                                        OnClick="ImgBtnMemberEditClick" CommandArgument='<%#Bind("cId") %>' />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnMemberDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                        CommandArgument='<%#Bind("cId") %>' OnClick="ImgBtnMemberDeleteClick" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnCorpFinance" runat="server" ImageUrl="~/Admin/images/icon/money.png"
                                                        CommandArgument='<%#Bind("cId") %>' OnClick="imgBtnCorpFinanceClick" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#Eval("cName")%>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%#GetUsersCountOnCorporation(Convert.ToInt32(Eval("cId"))) %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <span <%#(Convert.ToDecimal(Eval("finDept"))>0 ? "style='color:red;'":"style='color:green;'") %> >
                                            <%#GetCorporationBalance(Convert.ToInt32(Eval("cId"))) %>
                                        </span>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="cDescription" SortExpression="cDescription" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="cState" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Justify"
                                    ItemStyle-Width="50">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Justify" Width="50px" />
                                </asp:CheckBoxField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:View>
            <asp:View ID="vEditMember" runat="server">
                <uc1:CorporationAddEdit runat="server" ID="cCorporationAddEdit" />
                <br />
                <asp:Button runat="server" OnClick="BtBackToSearchPageClick" CssClass="SaveCancelBtn"
                    ID="btBackToSearchPage" />
            </asp:View>
            <asp:View ID="vFinanceManager" runat="server">
                <uc1:CorporationFinanceManager runat="server" ID="cCorporationFinanceManager" />
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
<asp:HiddenField runat="server" ID="hfCorporationId" />
<asp:HiddenField runat="server" ID="hfSearchResaultCmd" />
<asp:HiddenField runat="server" ID="hfSearchResaultCountCmd" />
<asp:HiddenField runat="server" ID="hfWorkCountry" />
<asp:HiddenField runat="server" ID="hfWorkCity" />
<asp:HiddenField runat="server" ID="hfWorkTown" />
<asp:HiddenField runat="server" ID="hfUserStateFilter" Value="" />
<asp:HiddenField runat="server" ID="hfMemberStateFilter" Value="" />
<asp:HiddenField runat="server" ID="hfMemberEmailActivationFilter" Value="" />
