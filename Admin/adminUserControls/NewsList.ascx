<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_NewsList"
    CodeBehind="NewsList.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <asp:Panel ID="pnlNewsEdit" runat="server" Visible="False">
            <asp:Panel runat="server" DefaultButton="btnSave">
                <table class="rightcontenttable">
                <tr>
                    <td style="width: 100px;">
                        <%= AdminResource.lbTitle %>
                    </td>
                    <td style="width: 10px;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtHeader" runat="server" Width="560px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rf" runat="server" ControlToValidate="txtHeader"
                            ForeColor="Red" ValidationGroup="vldGroup1">(!)</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbDate %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="dpDate" runat="server">
                        </telerik:RadDatePicker>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dpDate"
                            ForeColor="Red" ValidationGroup="vldGroup1">(!)
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbSummary %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBrief" runat="server" Height="100px" MaxLength="250" TextMode="MultiLine"
                            Width="560px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <%= AdminResource.lbDetail %>
                    </td>
                    <td style="vertical-align: top;">
                        :
                    </td>
                    <td>
                        <uc1:rtb id="Rtb1" runat="server" />
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
                        <table cellpadding="0" cellspacing="0" style="vertical-align: middle;">
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="txtImage" runat="server" />
                                <td>
                                    <asp:Button ID="imgBtnImageSelect" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
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
                        <asp:CheckBox ID="chkState" runat="server" Checked="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbHeadLine %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:CheckBox ID="cbManset" runat="server" Checked="False" />
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
                        <asp:Button ID="btnSave" CssClass="SaveCancelBtn" runat="server" ValidationGroup="vldGroup1"
                            OnClick="btnSaveUpdateNews" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancelSaveUpdateNews"/>
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
            <script type="text/javascript">
          //<![CDATA[
                function OpenFileExplorerDialog() {
                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                    wnd.show();
                }

                //This function is called from a code declared on the Explorer.aspx page

                function OnFileSelected(fileSelected) {
                    var textbox = $find("<%= txtImage.ClientID %>");
                    textbox.set_value("~" + fileSelected);
                }
        //]]>
            </script>
            <telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
                ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
                Modal="true" Behaviors="Close,Move,Resize,Maximize">
            </telerik:RadWindow>
        </asp:Panel>
        <asp:Panel ID="pnlNewList" runat="server" CssClass="Etiketler" DefaultButton="ImageButton2Ara">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <asp:Button ID="btnNew" runat="server" CssClass="NewBtn" OnClick="btnAddNews"/>
                    </td>
                    <td align="right" valign="top">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top">
                                    <asp:TextBox ID="txtAra" runat="server" Height="18px" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImageButton2Ara" runat="server" ImageUrl="~/Admin/images/icon/ara.png"
                                        OnClick="ImageButton2Ara_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                            SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                            SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                            SelectedRowStyle="selected" PageSize="15" AllowPaging="True" DataKeyNames="newsId"
                            DataSourceID="EntityDataSource1" AllowSorting="True" CellPadding="4" Width="100%"
                            OnRowDataBound="GridView1_RowDataBound" ForeColor="#333333" GridLines="None">
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
                                                       OnClick="btnEditNewsClick" CommandArgument='<%#Bind("newsId") %>' />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                        CommandArgument='<%#Bind("newsId") %>' OnClick="imgBtnDelete_Click" OnClientClick=" return confirm('Silmek istediğinizden emin misiniz?'); " />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="header" HeaderText="Başlık" SortExpression="header">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="brief" HeaderText="Özet" SortExpression="brief">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="enterDate" HeaderText="Giriş Tarihi" SortExpression="enterDate"
                                    DataFormatString="{0:d}" HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="state" HeaderText="Durum" SortExpression="state">
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField DataField="manset" HeaderText="Manşet" SortExpression="manset">
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:CheckBoxField>
                            </Columns>
                        </asp:GridView>
                        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                            DefaultContainerName="Entities" EntitySetName="News" OrderBy="it.enterDate DESC">
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