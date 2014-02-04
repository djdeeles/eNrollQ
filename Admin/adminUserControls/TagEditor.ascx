<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_TagEditor"
            CodeBehind="TagEditor.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td colspan="2">
                    <%= AdminResource.lbEditingPlace %>
                    &nbsp;<asp:Label ID="lblHtmlTag" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvTagValues" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                  CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                  SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                  SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                  EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                  PageSize="15" AllowPaging="True" DataKeyNames="id" DataSourceID="edsTagValues"
                                  ForeColor="#333333" GridLines="None" OnRowDataBound="gvTagValues_RowDataBound"
                                  Width="100%" BorderStyle="None">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" ImageUrl="~/Admin/images/icon/edit.png"
                                                     CommandName="Edit" />
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" ImageUrl="~/Admin/images/icon/cop.png"
                                                     CommandName="Delete" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="btnUpdate" runat="server" CausesValidation="True" ImageUrl="~/Admin/images/icon/save.png"
                                                     CommandName="Update" />
                                    <asp:ImageButton ID="btnCancel" runat="server" CausesValidation="False" ImageUrl="~/Admin/images/icon/cancel.png"
                                                     CommandName="Cancel" />
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle Width="75px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="cssAttDesc">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%#Bind
                                                                                                   ("cssAttDesc") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="value" SortExpression="value">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:EntityDataSource ID="edsTagValues" runat="server" ConnectionString="name=Entities"
                                          DefaultContainerName="Entities" EnableDelete="True" EnableFlattening="False"
                                          EnableUpdate="True" EntitySetName="EnrollHtmlTagValues" Where="it.htmlTagId=@htmlTag">
                        <WhereParameters>
                            <asp:QueryStringParameter DbType="Int32" Name="htmlTag" QueryStringField="htmlTag" />
                        </WhereParameters>
                    </asp:EntityDataSource>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="Panel1" runat="server" Visible="True" Width="275px" DefaultButton="btnAddProperty">
                        <table>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlAttList" runat="server" Font-Bold="True" Font-Names="Calibri"
                                                      Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlAttList_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <telerik:RadColorPicker ID="ColorPicker" ShowIcon="True" Enabled="True" EnableCustomColor="True"
                                                            runat="server">
                                    </telerik:RadColorPicker>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCssAttValues" runat="server" Visible="False" Width="150px">
                                    </asp:DropDownList>
                                    <telerik:RadTextBox ID="txtAttValue" runat="server" Font-Bold="False" Font-Names="Calibri"
                                                        Width="150px" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUzunluk" runat="server" Visible="False" Width="50">
                                        <asp:ListItem Text="px" Value="px"></asp:ListItem>
                                        <asp:ListItem Text="%" Value="%"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="ibtnGozat" runat="server" CssClass="ImageSelectBtn" Visible="False"
                                                OnClientClick="OpenFileExplorerDialog(); return false;" />
                                    <script type="text/javascript">
                                        function OpenFileExplorerDialog() {
                                            selectedFile = $find("<%= txtAttValue.ClientID %>");
                                            var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                            wnd.show();
                                        }
                                    </script>
                                    <telerik:RadWindow runat="server" Width="600px" Height="400px" VisibleStatusbar="false"
                                                       ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
                                                       Modal="true" Behaviors="Close,Move,Resize,Maximize">
                                    </telerik:RadWindow>
                                </td>
                                <td style="width: 50px;">
                                    <asp:Button ID="btnAddProperty" runat="server" CssClass="SaveCancelBtn" OnClick="btnAddProperty_Click" />
                                </td>
                                <td style="width: 50px;">
                                    <asp:Button ID="btnClose" runat="server" CssClass="SaveCancelBtn" OnClientClick=" window.opener.location = window.opener.location;window.close(); " />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:RangeValidator ID="rwTxtAttValue" runat="server" ForeColor="Red" ControlToValidate="txtAttValue"
                                                        Enabled="False" Type="Integer" ValidationGroup="attValue" MaximumValue="100"
                                                        MinimumValue="0">!</asp:RangeValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
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