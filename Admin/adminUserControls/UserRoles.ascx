<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_UserRoles"
    CodeBehind="UserRoles.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView ID="MultiView2" runat="server">
    <asp:View ID="View3" runat="server">
        <table class="AnaTablo">
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <tr>
                        <td>
                            <asp:Button ID="btNewUserRole" runat="server" CssClass="NewBtn" OnClick="ImageButtonYeniEkle_Click" />
                        </td>
                    </tr>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <script type="text/javascript">
                        function confirmRemoveAuthority(control) { 
                            var x = document.getElementById(control);
                            var checkBoxList = x.getElementsByTagName("input"); 
                            if (!checkBoxList[0].checked) {
                                if (!confirm('<%=AdminResource.msgConfirmRemoveOwnRole %>')) {
                                    checkBoxList[0].checked = "checked";
                                }
                            }
                        }
                    </script>
                    <tr>
                        <td>
                            <asp:Panel runat="server" DefaultButton="ImageButtonKaydet">
                                <table class="rightcontenttable">
                                    <tr>
                                        <td style="width: 100px;">
                                            <%= AdminResource.lbRoleName %>
                                        </td>
                                        <td style="width: 10px;">
                                            :
                                        </td>
                                        <td style="width: 200px;">
                                            <asp:TextBox ID="TextBoxRolAdi" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td style="width: 390px;">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxRolAdi"
                                                ErrorMessage="!" ForeColor="Red" ValidationGroup="g1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <%= AdminResource.lbAuthAreas %>
                                        </td>
                                        <td style="vertical-align: top;">
                                            :
                                        </td>
                                        <td colspan="2" style="vertical-align: top;">
                                            <asp:CheckBoxList ID="CheckBoxListYetkiAlanlari" runat="server" RepeatColumns="3"
                                                CssClass="GridViewStyle">
                                            </asp:CheckBoxList>
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
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="ImageButtonKaydet" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButtonKaydet_Click"
                                                ValidationGroup="g1" />
                                            <asp:Button ID="ImageButtonIptal" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButtonIptal_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td colspan="2">
                                            <asp:HiddenField ID="HiddenFieldId" runat="server" />
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
                                <asp:GridView ID="GridViewRoller" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                    PageSize="15" AllowPaging="True" SortedAscendingCellStyle-CssClass="sortasc"
                                    SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                    SelectedRowStyle="selected" CellPadding="4" ForeColor="#333333" GridLines="None"
                                    DataSourceID="EntityDataSource1" OnRowDataBound="GridViewRoller_OnRowDataBound"
                                    Width="100%" OnRowCommand="GridViewRoller_RowCommand">
                                    <EmptyDataTemplate>
                                        <%= AdminResource.lbNoRecord %>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="75" HeaderText="İşlemler" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../images/icon/edit.png"
                                                                CommandArgument='<%#                                        Bind("Id") %>' CommandName="Guncelle" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="../images/icon/cop.png"
                                                                CommandArgument='<%#Bind
                                                                                                   ("Id") %>' CommandName="Sil" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RoleName" HeaderText="Rol Adı" SortExpression="RoleName">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:CheckBoxField DataField="State" HeaderText="Durum" SortExpression="State">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:CheckBoxField>
                                    </Columns>
                                </asp:GridView>
                                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                    DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Roles"
                                    Where="">
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