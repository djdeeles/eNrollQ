<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_Users"
            CodeBehind="Users.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView ID="MultiView2" runat="server">
    <asp:View ID="View3" runat="server">
        <table class="rightcontenttable">
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <tr>
                        <td>
                            <asp:Button ID="btNewUser" runat="server" CssClass="NewBtn" OnClick="ImageButtonYeniEkle_Click" />
                        </td>
                    </tr>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <script type="text/javascript">
                        function confirmRemoveRole(control) {
                            var x = document.getElementById(control);
                            var checkBoxList = x.getElementsByTagName("input");
                            if (!checkBoxList[0].checked) {
                                if (!confirm('<%= AdminResource.msgConfirmRemoveOwnRole %>')) {
                                    checkBoxList[0].checked = "checked";
                                }
                            }
                        }
                    </script>
                    <tr>
                        <td>
                            <asp:Panel runat="server" DefaultButton="btnKaydet">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 75px;">
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td style="width: 10px;">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbName" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbName"
                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbSurname %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbSurname" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbSurname"
                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbEmail %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxEPosta" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxEPosta"
                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxEPosta"
                                                                            Display="Dynamic" ErrorMessage="!" ForeColor="Red" ValidationGroup="g" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbPassword %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxParola" runat="server" Width="200px"></asp:TextBox>
                                            <asp:LinkButton ID="lbGeneratePwd" runat="server" OnClick="lbGeneratePwdClick"></asp:LinkButton>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="CheckBoxDurum" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbRoles %>
                                        </td>
                                        <td valign="top">
                                            :
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBoxList ID="CheckBoxListRoller" runat="server" RepeatColumns="3" Width="100%" CssClass="GridViewStyle">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnKaydet" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButtonKaydet_Click"
                                                        ValidationGroup="g" />
                                            <asp:Button ID="btnIptal" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButtonIptal_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="2">
                                            <asp:HiddenField ID="hfUserId" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </asp:View>
            </asp:MultiView>
            <tr>
                <td>
                    <table class="rightcontenttable">
                        <tr>
                            <td>
                                <asp:GridView ID="GridViewKullanicilar" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                              CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="EntityDataSource1"
                                              CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                              SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                              SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                              EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                              PageSize="15" AllowPaging="True" OnRowDataBound="GridViewKullanicilar_OnRowDataBound"
                                              Width="100%" OnRowCommand="GridViewKullanicilar_RowCommand">
                                    <EmptyDataTemplate>
                                        <%= AdminResource.lbNoRecord %>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../images/icon/edit.png"
                                                                             CommandArgument='<%#Bind
                                                                                                   ("Id") %>' CommandName="Guncelle" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="../images/icon/cop.png"
                                                                             CommandArgument='<%#Bind("Id") %>' CommandName="Sil" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EMail" SortExpression="EMail">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Name" SortExpression="Name">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Surname" SortExpression="Surname">
                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:BoundField>
                                        <asp:CheckBoxField DataField="State" SortExpression="State">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:CheckBoxField>
                                    </Columns>
                                </asp:GridView>
                                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Users"
                                                      Where="it.Admin=True">
                                </asp:EntityDataSource>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="View4" runat="server">
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