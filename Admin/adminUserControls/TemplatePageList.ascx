<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_TemplatePageList"
    CodeBehind="TemplatePageList.ascx.cs" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <asp:MultiView runat="server" ID="mvTemplatePageList">
            <asp:View runat="server" ID="vAddEdit">
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSave">
                    <table class="rightcontenttable">
                        <tr>
                            <td class="Sol">
                                <%= AdminResource.lbName %>
                            </td>
                            <td class="Orta">
                                :
                            </td>
                            <td>
                                <table style="width: 645px;">
                                    <tr>
                                        <td style="width: 560px;">
                                            <asp:TextBox ID="txtName" runat="server" Width="560px"></asp:TextBox>
                                        </td>
                                        <td style="width: 100px;">
                                            <asp:RequiredFieldValidator ID="rf" runat="server" ControlToValidate="txtName" ForeColor="Red"
                                                ValidationGroup="vldGroup1" Display="Dynamic">(!)</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <%= AdminResource.lbContent %>
                            </td>
                            <td style="vertical-align: top;">
                                :
                            </td>
                            <td>
                                <uc1:Rtb ID="Rtb1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                                    OnClick="BtnSaveClick" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelClick" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:View>
            <asp:View runat="server" ID="vGrid">
                <table class="rightcontenttable">
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnNew" runat="server" CssClass="NewBtn" OnClick="BtnNewClick"/>
                        </td>
                        <td align="right">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gVTemplates" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                    PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                    SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                    SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                    SelectedRowStyle="selected" DataKeyNames="name" DataSourceID="EntityDataSource1"
                    AllowPaging="True" AllowSorting="True" CellPadding="4" OnRowDataBound="GridView1RowDataBound"
                    Width="100%" PageSize="15" ForeColor="#333333" GridLines="None">
                    <EmptyDataTemplate>
                        <%= AdminResource.lbNoRecord %>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="İşlemler">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                CommandArgument='<%#Bind("id") %>' OnClick="ImgBtnEditClick" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                CommandArgument='<%#Bind("id") %>' OnClick="ImgBtnDeleteClick" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="name" HeaderText="Şablon Adı" SortExpression="name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                    DefaultContainerName="Entities" EntitySetName="TemplatePages" Where="" EntityTypeFilter=""
                    Select="">
                </asp:EntityDataSource>
            </asp:View>
        </asp:MultiView>
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