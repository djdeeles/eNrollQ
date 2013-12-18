<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uye_UserControls_EventManager"
    CodeBehind="EventManager.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td colspan="3">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <asp:Panel runat="server" DefaultButton="ImageButton2Ara">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddNew" runat="server" CssClass="NewBtn" OnClick="btnAddNew_Click" />
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
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSave2">
                                <table>
                                    <tr>
                                        <td width="100">
                                            <%= AdminResource.lbTitle %>
                                        </td>
                                        <td width="10">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="150" Width="290px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                                SetFocusOnError="True" ValidationGroup="g1" ControlToValidate="txtName">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td valign="top">
                                            :
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="txtDesc" runat="server" Height="70px" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                                SetFocusOnError="True" ValidationGroup="g1" ControlToValidate="txtDesc">!</asp:RequiredFieldValidator>
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
                                            <telerik:RadDatePicker ID="dpStartDate" runat="server" ZIndex="30001">
                                            </telerik:RadDatePicker>
                                            <telerik:RadTimePicker ID="tpStartTime" runat="server">
                                            </telerik:RadTimePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                                                ControlToValidate="dpStartDate" ValidationGroup="g1">!
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                                                ControlToValidate="tpStartTime" ValidationGroup="g1">!
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
                                            <telerik:RadDatePicker ID="dpEndDate" runat="server" ZIndex="30001">
                                            </telerik:RadDatePicker>
                                            <telerik:RadTimePicker ID="tpEndTime" runat="server">
                                            </telerik:RadTimePicker>
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
                                            <asp:CheckBox ID="cbActive" runat="server" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td valign="top">
                                            :
                                        </td>
                                        <td valign="top">
                                            <uc1:Rtb ID="Rtb1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                        </td>
                                        <td valign="top">
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave2" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButton2_Click"
                                                ValidationGroup="g1" />
                                            <asp:Button ID="btnCancel2" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButton3_Click"
                                                OnClientClick="ImageButton3_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="View3" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <table>
                                    <tr>
                                        <td width="100">
                                            <%= AdminResource.lbTitle %>
                                        </td>
                                        <td width="10">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox1Baslik" runat="server" MaxLength="150" Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                                SetFocusOnError="True" ValidationGroup="g2" ControlToValidate="TextBox1Baslik">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td valign="top">
                                            :
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="TextBox2Ozet" runat="server" Height="70px" TextMode="MultiLine"
                                                Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                                                SetFocusOnError="True" ValidationGroup="g2" ControlToValidate="TextBox2Ozet">!</asp:RequiredFieldValidator>
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
                                            <telerik:RadDatePicker ID="editDpStartDate" runat="server" ZIndex="30001">
                                            </telerik:RadDatePicker>
                                            <telerik:RadTimePicker ID="editTpStartTime" runat="server">
                                            </telerik:RadTimePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="editDpStartDate"
                                                ForeColor="Red" ValidationGroup="g2">!
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="editTpStartTime"
                                                ForeColor="Red" ValidationGroup="g2">!
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
                                            <telerik:RadDatePicker ID="editDpEndDate" runat="server" ZIndex="30001">
                                            </telerik:RadDatePicker>
                                            <telerik:RadTimePicker ID="editTpEndTime" runat="server">
                                            </telerik:RadTimePicker>
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
                                            <asp:CheckBox ID="CheckBox1Durum" runat="server" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td valign="top">
                                            :
                                        </td>
                                        <td valign="top">
                                            <uc1:Rtb ID="Rtb2" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                        </td>
                                        <td valign="top">
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveClick"
                                                ValidationGroup="g2" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:HiddenField ID="HiddenFieldEtkinlikId" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvEventList" runat="server" AutoGenerateColumns="False" CellPadding="4"
            PageSize="15" AllowPaging="True" CssClass="GridViewStyle" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
            SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
            SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
            SelectedRowStyle="selected" DataKeyNames="id" DataSourceID="dsEvents" GridLines="None"
            Width="100%" ForeColor="#333333" AllowSorting="True" OnRowCommand="GridView1_RowCommand"
            OnRowDataBound="gvEventList_onRowDataBound">
            <EmptyDataTemplate>
                <%= AdminResource.lbNoRecord %>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" ImageUrl="~/Admin/images/icon/edit.png" runat="server"
                            CommandArgument='<%#Eval
                                                                                                   ("id") %>'
                            CommandName="Guncelle" />
                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                            CommandArgument='<%#Eval("id") %>' CommandName="Sil" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                </asp:TemplateField>
                <asp:BoundField DataField="Name" SortExpression="Name">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="StartDate" SortExpression="StartDate">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="EndDate" SortExpression="EndDate">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <asp:CheckBoxField DataField="State" SortExpression="State">
                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                </asp:CheckBoxField>
            </Columns>
        </asp:GridView>
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
<asp:EntityDataSource ID="dsEvents" runat="server" ConnectionString="name=Entities"
    DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Events"
    Where="it.languageId=@languageId">
    <WhereParameters>
        <asp:ControlParameter Name="languageId" ControlID="hfLanguageId" PropertyName="Value"
            DbType="Int32" DefaultValue="1" />
    </WhereParameters>
</asp:EntityDataSource>
<asp:HiddenField ID="hfLanguageId" runat="server" />
