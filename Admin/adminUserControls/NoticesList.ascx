<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_NoticeList"
            CodeBehind="NoticesList.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <asp:Panel runat="server" ID="pnlNoticeEdit" Visible="False" DefaultButton="btnSave">
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
                        <%= AdminResource.lbImage %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0" style="vertical-align: middle;">
                            <tr>
                                <td>
                                    <telerik:RadTextBox ID="TextBox1" runat="server" Width="400px"/>
                                </td>
                                <td>
                                    <asp:Button ID="btnImageSelect" runat="server" CssClass="ImageSelectBtn" 
                                                OnClientClick=" OpenFileExplorerDialog(); return false; " Visible="True" />

                                    <script type="text/javascript">
                                        function OpenFileExplorerDialog() {
                                            selectedFile = $find("<%= TextBox1.ClientID %>");
                                            var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                            wnd.show();
                                        }
                                    </script>
                                    <telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
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
                        <%= AdminResource.lbStartDate %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="dpStartDate" runat="server">
                        </telerik:RadDatePicker>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dpStartDate"
                                                    ForeColor="Red" ValidationGroup="vldGroup1">(!)
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbEndDate %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <telerik:RadDatePicker ID="dpEndDate" runat="server">
                        </telerik:RadDatePicker>
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
                        <asp:TextBox ID="txtOzet" TextMode="MultiLine" runat="server" Width="560px"></asp:TextBox>
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
                        <uc1:Rtb ID="Rtb1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblState" runat="server"><%= AdminResource.lbState %></asp:Label>
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
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
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
                                    OnClick="btnSaveUpdateNotice" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" ImageUrl="~/Admin/images/iptal.png"
                                    OnClick="btnCancelEditNotice" />
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
        <asp:Panel runat="server" ID="pnlNoticeList" DefaultButton="ImageButton2Ara">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <asp:Button ID="btnNew" runat="server" CssClass="NewBtn"  OnClick="btnNewNotice"/>
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
                                      PageSize="15" AllowPaging="True" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                      SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                      SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                      EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                      DataKeyNames="noticeId" DataSourceID="EntityDataSource1" AllowSorting="True"
                                      CellPadding="4" OnRowDataBound="GridView1_RowDataBound" Width="100%" ForeColor="#333333"
                                      GridLines="None">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="75" HeaderText="İşlemler" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                                     OnClick="imgBtnEdit" CommandArgument='<%#Bind
                                                                                                   ("noticeId") %>' />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                                     CommandArgument='<%#Bind("noticeId") %>' OnClick="imgBtnDelete_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="header" HeaderText="Başlık" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" SortExpression="header">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="startDate" HeaderText="Başlangıç Tarihi" DataFormatString="{0:d}"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100"
                                                ItemStyle-Width="100" SortExpression="startDate">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="stopDate" HeaderText="Bitiş Tarihi" DataFormatString="{0:d}"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100"
                                                ItemStyle-Width="100" SortExpression="stopDate">
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="state" HeaderText="Durum" HeaderStyle-HorizontalAlign="Left"
                                                   ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50" ItemStyle-Width="50"
                                                   SortExpression="state">
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:CheckBoxField>
                            </Columns>
                        </asp:GridView>
                        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                              DefaultContainerName="Entities" EntitySetName="Notices" OrderBy="it.startDate desc">
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