<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberActivation.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.MemberActivation" %>
<%@ Register Src="~/Admin/adminUserControls/MemberAddEdit.ascx" TagName="MemberAddEdit"
    TagPrefix="uc1" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="eNroll.Helpers" %>
<style type="text/css">
    .memberstateoption { float: left; }
</style>
<asp:MultiView runat="server" ID="mvAuthoriztn" ViewStateMode="Enabled">
    <asp:View runat="server" ID="vAuth">
        <asp:MultiView runat="server" ID="mvMembers" ActiveViewIndex="1">
            <asp:View runat="server" ID="vEditMembers">
                <uc1:MemberAddEdit runat="server" ID="cMemberAddEdit" />
            </asp:View>
            <asp:View runat="server" ID="vGridMembers">
                <%=AdminResource.lbMemberState %>:
                <asp:DropDownList ID="ddlMemberStatesFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMemberStatesFilter_OnSelectedIndexChanged" ViewStateMode="Enabled"/>
                <asp:Button runat="server" ID="btSendActivationCode" CssClass="SaveCancelBtn" Visible="False"
                    OnClick="BtSendActivationCode_OnClick" />
                <asp:Button runat="server" ID="BtnDeleteAll" CssClass="SaveCancelBtn" Visible="False"
                    OnClick="BtnDeleteAll_OnClick" />
                <div style="width: 100%; display: none;">
                    <asp:TextBox runat="server" Visible="False" ID="tbTestSqlSource" TextMode="MultiLine"
                        Width="400px" Height="100px"></asp:TextBox>
                </div>
                <asp:GridView ID="gVMembers" runat="server" AutoGenerateColumns="False" OnRowDataBound="gVMembers_OnRowDataBound"
                    CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                    SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                    EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                    DataKeyNames="Id" CellPadding="4" Width="100%" PageSize="15" ForeColor="#333333"
                    GridLines="None" AllowSorting="False">
                    <EmptyDataTemplate>
                        <%= AdminResource.lbNoRecord %>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                OnClick="ImgBtnMemberEditClick" CommandArgument='<%#Bind("Id") %>' />
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                OnClick="ImgBtnMemberDeleteClick" CommandArgument='<%#Bind("Id") %>' />
                                        </td>
                                        <td <%# (EnrollMembershipHelper.UserMembershipState(Eval("Id").ToString()) ==
                                                        EnrollMembershipHelper.MembershipType.OnayBekleyen ? 
                                                        "style='display:table;'":"style='display:none;'") %>>
                                            <asp:Button ID="BtnConfirmMemberActive" CssClass="SaveCancelBtn" runat="server" OnClick="BtnConfirmMemberActiveClick" CommandArgument='<%#Bind("Id") %>' />
                                        </td>
                                        <td <%# (EnrollMembershipHelper.UserMembershipState(Eval("Id").ToString()) ==
                                                        EnrollMembershipHelper.MembershipType.Aktif ? 
                                                        "style='display:table;'":"style='display:none;'") %>>
                                            <asp:Button ID="BtnSetPassive" CssClass="SaveCancelBtn" runat="server" OnClick="BtnSetPassiveClick"
                                                CommandArgument='<%#Bind("Id") %>' />
                                        </td>
                                        <td <%# (EnrollMembershipHelper.UserMembershipState(Eval("Id").ToString()) ==
                                                        EnrollMembershipHelper.MembershipType.Pasif ? 
                                                        "style='display:table;'":"style='display:none;'") %>>
                                            <asp:Button ID="BtnSetActive" CssClass="SaveCancelBtn" runat="server" OnClick="BtnSetActiveClick"
                                                CommandArgument='<%#Bind("Id") %>' />
                                        </td>
                                        <td <%# (EnrollMembershipHelper.UserMembershipState(Eval("Id").ToString()) ==
                                                        EnrollMembershipHelper.MembershipType.Hatali ? 
                                                        "style='display:table;'":"style='display:none;'") %>>
                                            <asp:Button ID="BtnResendActivationCode" CssClass="SaveCancelBtn" runat="server"
                                                OnClick="BtnResendActivationCodeClick" CommandArgument='<%#Bind("Id") %>' />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="85px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Surname" SortExpression="Surname" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="85px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#GetMembershipType(Eval("Id").ToString()) %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="90px" />
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="State" SortExpression="State" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="50">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Justify" Width="50px" />
                        </asp:CheckBoxField>
                    </Columns>
                </asp:GridView>
                <asp:EntityDataSource ID="edsMembers" runat="server" ConnectionString="name=Entities"
                    DefaultContainerName="Entities" EnableFlattening="False" EnableDelete="True"
                    EntitySetName="Users" OrderBy="it.State">
                </asp:EntityDataSource>
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
<asp:HiddenField runat="server" ID="hfUserStateFilter" Value="" />
<asp:HiddenField runat="server" ID="hfMemberStateFilter" Value="" />
<asp:HiddenField runat="server" ID="hfMemberEmailActivationFilter" Value="" />
