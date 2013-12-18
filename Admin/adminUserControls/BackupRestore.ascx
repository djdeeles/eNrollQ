<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BackupRestore.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.BackupRestore" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View ID="View1" runat="server">
        <asp:Button ID="btBackUp" runat="server" CssClass="SaveCancelBtn" OnClick="BackupDatabaseClick" />
        <asp:Button ID="btFullBackup" runat="server" CssClass="SaveCancelBtn" OnClick="BackupFilesClick" />
        <asp:GridView ID="grVFiles" runat="server" AutoGenerateColumns="False" CellPadding="4"
            CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
            SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
            SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
            EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle-CssClass="selected"
            PageSize="15" AllowPaging="True" Width="100%" OnRowDataBound="GrVFilesRowDataBound"
            ForeColor="#333333" GridLines="None">
            <EmptyDataTemplate>
                <%= AdminResource.lbNoRecord %>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnFileDelete" runat="server" CommandArgument='<%#Bind("FullName") %>'
                            ImageUrl="~/Admin/images/icon/cop.png" OnClick="BtnFileDelete" />
                        <asp:ImageButton ID="btnDBRestore" runat="server" CommandArgument='<%#Bind("FullName") %>'
                            ImageUrl="~/Admin/images/icon/geriyukle.png" OnClick="RestoreDatabase" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%--<a href='<%#BackupFolder+Eval("Name")%>'>--%><%#Eval("Name") %><%--</a>--%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="CreationTime">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Length">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </asp:View>
    <asp:View ID="View2" runat="server">
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