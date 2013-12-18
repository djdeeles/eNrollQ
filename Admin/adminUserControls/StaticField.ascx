<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_StaticField"
    CodeBehind="StaticFields.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
    DefaultContainerName="Entities" EntitySetName="Customer_Special">
</asp:EntityDataSource>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <asp:Panel ID="pnlSpecialFieldEdit" runat="server" Visible="False" DefaultButton="imBtSave">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <%=AdminResource.lbFieldName %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtHeader" Width="300px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbContent %>:
                    </td>
                    <td>
                        <uc1:rtb id="Rtb1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="imBtSave" runat="server" ValidationGroup="vldGroup1" OnClick="imBtSave_Click"
                            CssClass="SaveCancelBtn" />
                        <asp:Button ID="imBtCancel" runat="server" OnClick="imgBtnCancel" CssClass="SaveCancelBtn" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlNewList" runat="server">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <asp:GridView ID="gvStaticFields" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                            SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                            SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                            SelectedRowStyle="selected" PageSize="15" DataKeyNames="specialId" DataSourceID="EntityDataSource1"
                            AllowPaging="True" AllowSorting="True" CellPadding="4"
                            Width="100%" ForeColor="#333333" GridLines="None">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png" OnClick="imgBtnEditClick"
                                            CommandArgument='<%#Bind("specialId") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="header" SortExpression="header">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
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
<asp:HiddenField ID="hdnId" runat="server" />