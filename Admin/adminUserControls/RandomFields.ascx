<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_RandomFields"
            CodeBehind="RandomFields.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <asp:Panel ID="pnlRandomFieldEdit" runat="server" Visible="False" DefaultButton="imBtSave">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <%= AdminResource.lbFieldName %>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtHeader" Width="300px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbSummary %>:
                    </td>
                    <td>
                        <uc1:Rtb ID="Rtb2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbContent %>:
                    </td>
                    <td>
                        <uc1:Rtb ID="Rtb1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbState %>:
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="cbState" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="imBtSave" runat="server" ValidationGroup="vldGroup1" OnClick="ImBtSaveClick"
                                    CssClass="SaveCancelBtn" />
                        <asp:Button ID="imBtCancel" runat="server" OnClick="ImgBtnCancel" CssClass="SaveCancelBtn" />
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
            <asp:Button runat="server" ID="btNewRandomField" OnClick="BtnNewRandomFieldOnClick"
                        CssClass="NewBtn" />
            <table class="rightcontenttable">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvRandomFields" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                      PagerStyle-CssClass="pgr" OnRowDataBound="gvRandomFields_OnRowDataBound" AlternatingRowStyle-CssClass="alt"
                                      SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                      SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                      EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                      PageSize="15" DataKeyNames="Id" DataSourceID="EntityDataSource1" AllowPaging="True"
                                      AllowSorting="True" CellPadding="4" Width="100%" ForeColor="#333333" GridLines="None">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                         OnClick="ImgBtnEditClick" CommandArgument='<%#Bind
                                                                                                   ("Id") %>' />
                                        <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                         OnClick="ImgBtnDeleteClick" CommandArgument='<%#Bind("Id") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Title" SortExpression="Title">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="State" SortExpression="State">
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:CheckBoxField>
                            </Columns>
                        </asp:GridView>
                        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                              DefaultContainerName="Entities" EntitySetName="Customer_Random">
                        </asp:EntityDataSource>
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