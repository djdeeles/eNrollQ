<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_SliderImageList"
            CodeBehind="SliderImageList.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <asp:Panel ID="pnlEdit" runat="server" Visible="False" DefaultButton="btnSave">
            <table class="rightcontenttable">
                <tr>
                    <td style="width: 100px;">
                        <%= AdminResource.lbName %>
                    </td>
                    <td style="width: 10px;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rf" runat="server" ControlToValidate="txtName" ErrorMessage="Başlık boş bırakılamaz."
                                                    ValidationGroup="vldGroup1">(!)</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbImage %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtImage" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="imgBtnImageSelect" CssClass="ImageSelectBtn" runat="server" OnClientClick="OpenFileExplorerDialog(); return false;" />
                                    <script type="text/javascript">
          //<![CDATA[
                                        function OpenFileExplorerDialog() {
                                            selectedFile = $find("<%= txtImage.ClientID %>");
                                            var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                            wnd.show();
                                        }
                                    </script>
                                    <telerik:RadWindow runat="server" Width="600px" Height="350px" VisibleStatusbar="false"
                                                       ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
                                                       Modal="true" Behaviors="Close,Move,Resize,Maximize">
                                    </telerik:RadWindow>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbLink %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtUrl" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbState %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:CheckBox ID="chkState" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                                    OnClick="btnSaveUpdateSliderImage" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancelSliderImage" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlList" runat="server">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <asp:Button ID="btnNewSliderImageAdd" runat="server" CssClass="NewBtn" OnClick="btnNewSliderImage"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gVRef" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                      PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                                      SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                                      SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                      SelectedRowStyle="selected" DataKeyNames="id" DataSourceID="EntityDataSource1"
                                      AllowSorting="True" CellPadding="4" OnRowDataBound="GridView1_RowDataBound" Width="100%"
                                      AllowPaging="True" PageSize="15" ForeColor="#333333" GridLines="None">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                                     OnClick="imgBtnEdit_Click" CommandArgument='<%#Bind
                                                                                                   ("id") %>' />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnDelete" ImageUrl="~/Admin/images/icon/cop.png" runat="server"
                                                                     CommandArgument='<%#Bind("id") %>' OnClick="imgBtnDelete_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="name" SortExpression="name">
                                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="url" SortExpression="url" HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="state" SortExpression="state">
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:CheckBoxField>
                            </Columns>
                        </asp:GridView>
                        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                              DefaultContainerName="Entities" EntitySetName="RefLogos" EntityTypeFilter=""
                                              Select="">
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