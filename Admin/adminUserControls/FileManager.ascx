<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_FileManager"
    CodeBehind="FileManager.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<style type="text/css">
    .TabloSol { text-align: left; vertical-align: top; width: 220px; margin-right:10px; }
    
    .TabloSag { text-align: left; vertical-align: top; width: 520px; }
</style>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td class="TabloSol">
                    <asp:Panel ID="Panel1" runat="server"  ScrollBars="Auto" Width="225px" CssClass="GridViewStyle" Direction="LeftToRight">
                        <asp:TreeView ID="TreeView1" runat="server" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                            <ParentNodeStyle ImageUrl="~/admin/images/icon/folder.png" />
                            <RootNodeStyle ImageUrl="~/admin/images/icon/home.png" />
                            <LeafNodeStyle ImageUrl="~/admin/images/icon/folder.png" />
                            <SelectedNodeStyle ForeColor="#881F57" />
                        </asp:TreeView>
                    </asp:Panel>
                </td>
                <td class="TabloSag">
                    <asp:Panel ID="pnlProccess" runat="server" Width="520px" DefaultButton="btnUpload">
                        <table class="GridViewStyle">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="4" class="UstBar">
                                                <%= AdminResource.lbFolderActions %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <%= AdminResource.lbSelectedFolder %>:
                                                <asp:Label ID="lblActiveDirectory" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px;">
                                                <asp:Label ID="lblNewwFolder" runat="server">
                                                    <%= AdminResource.lbNewFolder %>
                                                </asp:Label>
                                            </td>
                                            <td style="width: 10px;">
                                                :
                                            </td>
                                            <td style="width: 270px;">
                                                <asp:TextBox ID="txtNewFolderName" runat="server" Width="260px"></asp:TextBox>
                                            </td>
                                            <td style="width: 140px;">
                                                <asp:Button ID="imgNewFolder" runat="server" CssClass="SaveCancelBtn" OnClick="imgNewFolder_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblNewwFolder0" runat="server">
                                                    <%= AdminResource.lbEdit %>/
                                                    <%= AdminResource.lbDelete %>

                                                </asp:Label>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtChangedFolderName" runat="server" Width="260px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="imgButtonEditFolder" runat="server" CssClass="SaveCancelBtn" OnClick="imgButtonEditFolder_Click" />
                                                <asp:Button ID="btnDeleteFolder" runat="server" CssClass="SaveCancelBtn" OnClick="BtnDeleteFolderClick" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height: 10px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="UstBar">
                                                <%= AdminResource.lbFileActions %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblFileDownload" runat="server">
                                                    <%= AdminResource.lbFileUpload %>:
                                                </asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <input type="file" id="FileUpload1" runat="server" name="FileUpload1" multiple="true" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnUpload" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButton2_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height: 10px;">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto">
                                        <asp:GridView ID="grVFiles" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                            CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                            SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                            SortedAscendingCellStyle-CssClass="sortasc"
                                            SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                            SelectedRowStyle="selected" Width="100%" OnRowDataBound="grVFiles_RowDataBound"
                                            ForeColor="#333333" GridLines="None">
                                            <EmptyDataTemplate>
                                                <%= AdminResource.lbNoRecord %>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <a href='../<%= hdnActiveDirectory.Value.Replace("~/", "") %>/<%#                                        Eval("Name") %>'
                                                                        rel="prettyPhoto">
                                                                        <img src="../../Admin/images/icon/zoom.png" width="26px" />
                                                                    </a>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%#Bind
                                                                                                   ("Name") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:MultiView ID="grdMultiFileName" runat="server">
                                                            <asp:View ID="View1" runat="server">
                                                                <asp:Label ID="Label1" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                            </asp:View>
                                                            <asp:View ID="View2" runat="server">
                                                                <asp:LinkButton ID="LnkFileName" runat="server" Text='<%#Bind("Name") %>'></asp:LinkButton>
                                                            </asp:View>
                                                        </asp:MultiView>
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
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnFileDelete" runat="server" CommandArgument='<%#Bind("FullName") %>'
                                                            ImageUrl="~/Admin/images/icon/cop.png" OnClick="BtnFileDelete" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hdnActiveDirectory" runat="server" />
                    <asp:HiddenField ID="hdnMode" runat="server" />
                </td>
                <td>
                </td>
            </tr>
        </table>
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