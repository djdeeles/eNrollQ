<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_layoutEditor"
            CodeBehind="layoutEditor.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td style="width: 170px">
                    <%= AdminResource.lbItemField %>
                </td>
                <td style="width: 10px">
                    :
                </td>
                <td style="width: 540px">
                    <asp:Label ID="lblLocation" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%= AdminResource.lbFieldName %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="150px" Font-Names="Calibri"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <%= AdminResource.lbControl %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:DropDownList ID="ddlKontroller" runat="server" Width="150px" Font-Bold="True"
                                      Font-Names="Calibri">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <table width="160px">
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButton1_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClientClick=" window.opener.location = window.opener.location;window.close(); " />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <%= AdminResource.lbAddProperty %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:ImageButton ID="ibtnArti" runat="server" ImageUrl="~/Admin/images/icon/arti.png"
                                     OnClick="btnArti_Click" Width="16px" Enabled="False" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="gvAttValues" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                  CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                  SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                  PageSize="15" AllowPaging="True" SortedAscendingCellStyle-CssClass="sortasc"
                                  SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                  SelectedRowStyle="selected" OnRowDataBound="gvAttValues_OnRowDataBound" DataKeyNames="id"
                                  ForeColor="#333333" GridLines="None" Width="100%" DataSourceID="edsEnrollPanelValues">
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
                                    <asp:ImageButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                     ImageUrl="~/Admin/images/icon/save.png" />
                                    <asp:ImageButton ID="btnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                     ImageUrl="~/Admin/images/icon/cancel.png" />
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle Width="75px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="cssAttDesc">
                                <ItemTemplate>
                                    <asp:Label ID="lblAttributeId" runat="server" Text='<%#Bind
                                                                                                   ("cssAttDesc") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="cssAttributeValue" SortExpression="cssAttributeValue">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="Panel1" runat="server" Visible="False" Width="275px">
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
                                    <asp:DropDownList ID="ddlUzunluk" runat="server" Visible="False" AutoPostBack="True"
                                                      Width="50px">
                                        <asp:ListItem Text="px" Value="px"></asp:ListItem>
                                        <asp:ListItem Text="%" Value="%"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="ibtnGozat" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;"
                                                Visible="False" />
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
                                    <asp:Button ID="btnSaveProperty" runat="server" CssClass="SaveCancelBtn" OnClick="btnSaveProperty_Click" />
                                </td>
                                <td style="width: 50px;">
                                    <asp:Button ID="btnCancelAddproperty" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancelAddproperty_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:RangeValidator ID="rwTxtAttValue" runat="server" ForeColor="Red" ControlToValidate="txtAttValue"
                                                        Enabled="False" Type="Integer" ValidationGroup="attValue" MaximumValue="960"
                                                        MinimumValue="1" SetFocusOnError="True"></asp:RangeValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
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
<asp:EntityDataSource ID="edsEnrollPanelValues" runat="server" ConnectionString="name=Entities"
                      DefaultContainerName="Entities" EntitySetName="EnrollHtmlPanelValues" Where="it.panelId= @panelId"
                      EnableUpdate="True" EntityTypeFilter="EnrollHtmlPanelValues" EnableDelete="True">
    <WhereParameters>
        <asp:QueryStringParameter DbType="Int32" Name="panelId" QueryStringField="id" />
    </WhereParameters>
</asp:EntityDataSource>