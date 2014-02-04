<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_DynamicField"
            CodeBehind="DynamicField.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="uc1" TagName="rtb" Src="~/Admin/adminUserControls/Rtb.ascx" %>
<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                      DefaultContainerName="Entities" EntitySetName="Customer_Dynamic_Group">
</asp:EntityDataSource>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <asp:Panel ID="pnlNewDynamicGroup" runat="server" Visible="False" DefaultButton="btnCustomerDynamicGroupSaveUpdate">
            <table class="rightcontenttable">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblName" runat="server"><%= AdminResource.lbName %></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rf" runat="server" ControlToValidate="txtName" ForeColor="Red"
                                                    ValidationGroup="vldGroup1">(!)</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbContent %>
                    </td>
                    <td>
                        <uc1:rtb ID="Rtb1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblState" runat="server"><%= AdminResource.lbState %></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkState" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnCustomerDynamicGroupSaveUpdate" runat="server" ValidationGroup="vldGroup1"
                                    CssClass="SaveCancelBtn" OnClick="btnCustomerDynamicGroupSaveUpdateClick" />
                        <asp:Button ID="btnCustomerDynamicGroupEditCancel" runat="server" CssClass="SaveCancelBtn"
                                    OnClick="btnCustomerDynamicGroupEditCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlCustomerDynamic" runat="server" Visible="False" DefaultButton="btnCustomerDynamicSaveUpdate">
            <table class="rightcontenttable">
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label1" runat="server"><%= AdminResource.lbName %></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerDynamic_name" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCustomerDynamic_name"
                                                    ForeColor="Red" ValidationGroup="vldGroup1">(!)</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbContent %>
                    </td>
                    <td>
                        <uc1:rtb ID="Rtb_CustomerDynamic_details" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnCustomerDynamicSaveUpdate" runat="server" ValidationGroup="vldGroup1"
                                    CssClass="SaveCancelBtn" OnClick="btnCustomerDynamicSaveUpdateClick" />
                        <asp:Button ID="btnCustomerDynamicEditCancel" runat="server" CssClass="SaveCancelBtn"
                                    OnClick="btnCustomerDynamicEditCancel_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlNewList" runat="server" DefaultButton="imgBtnNew">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <asp:Button ID="imgBtnNew" runat="server" CssClass="NewBtn" OnClick="btnNewDynamicGroupList" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                      PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                                      SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                                      SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                      SelectedRowStyle="selected" DataKeyNames="groupId" DataSourceID="EntityDataSource1"
                                      AllowPaging="True" AllowSorting="True" CellPadding="4" OnRowDataBound="GridView1_RowDataBound"
                                      Width="100%" PageSize="15" GridLines="None">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="İşlemler">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                                     CommandArgument='<%#Bind
                                                                                                   ("groupId") %>' OnClick="imgBtnEdit_Click" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                                     CommandArgument='<%#Bind("groupId") %>' OnClick="imgBtnDelete_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="name" HeaderText="İsim" SortExpression="name">
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="state" HeaderText="Durum" SortExpression="state">
                                    <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                </asp:CheckBoxField>
                                <asp:TemplateField HeaderText="Seçenekler">
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="imgBtnInnerNew" runat="server" CssClass="NewBtn" CommandArgument='<%#Bind("groupId") %>'
                                                                OnClick="btnNewOptions_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grdSecenek" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                  CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                                  SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                                                  SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                                                  EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                                                  DataKeyNames="dynamicId" OnRowDataBound="grdSecenek_RowDataBound" PageSize="8"
                                                                  GridLines="None" Width="100%">
                                                        <RowStyle CssClass="RowStyle" />
                                                        <EmptyDataRowStyle CssClass="EmptyRowStyle" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText=''>
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgBtnInnerEdit" runat="server" CommandArgument='<%#Bind("dynamicId") %>'
                                                                                                 ImageUrl="~/Admin/images/icon/edit.png" OnClick="imgBtnEditOptions_Click" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgBtnInerDelete" runat="server" CommandArgument='<%#Bind("dynamicId") %>'
                                                                                                 ImageUrl="~/Admin/images/icon/cop.png" OnClick="imgBtnInerDeleteOptions_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="name" HeaderText="İsim" SortExpression="name" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <asp:EntityDataSource ID="EntityDataSource3" runat="server" ConnectionString="name=Entities"
                                  DefaultContainerName="Entities" EntitySetName="Customer_Dynamic">
            </asp:EntityDataSource>
        </asp:Panel>
    </asp:View>
    <asp:View runat="server">
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
<asp:HiddenField ID="hfDynamicGroupId" runat="server" />
<asp:HiddenField ID="hfDynamicId" runat="server" />