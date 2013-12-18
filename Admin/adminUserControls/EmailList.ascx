<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_EmailList"
    CodeBehind="EmailList.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <asp:Panel runat="server" DefaultButton="btnExcel">
            <asp:Button ID="btnExcel" CssClass="SaveCancelBtn" runat="server" OnClick="DownloadExcel" />
            <table class="rightcontenttable">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td align="right">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GridEmails" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                            SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                            SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                            SelectedRowStyle-CssClass="selected" CellPadding="4" DataKeyNames="id" DataSourceID="edsEmailList"
                            ForeColor="#333333" GridLines="None" Width="100%" AllowPaging="True" AllowSorting="True"
                            OnRowCommand="GvEmailsRowCommand" OnRowDataBound="GvEmailsRowDataBound">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                            CommandName="Update" CausesValidation="True" CommandArgument='<%#Eval("id") %>' />
                                        <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                            CommandName="Cancel" CausesValidation="False" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="LinkButton3" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                            CommandName="Edit" CommandArgument='<%#Eval("id") %>' />
                                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                            CausesValidation="False" OnClick="imgBtnDelete_Click" CommandArgument='<%#Eval("id") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" SortExpression="Name" />
                                <asp:BoundField DataField="Surname" SortExpression="Surname" />
                                <asp:BoundField DataField="email" SortExpression="email" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%#Eval("Guid") %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:EntityDataSource ID="edsEmailList" runat="server" ConnectionString="name=Entities"
                            DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="EmailList" />
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