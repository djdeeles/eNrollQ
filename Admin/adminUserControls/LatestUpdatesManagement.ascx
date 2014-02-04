<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LatestUpdatesManagement.ascx.cs"
            Inherits="eNroll.Admin.adminUserControls.LatestUpdatesManagement" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView ID="MultiView2" runat="server">
    <asp:View ID="View3" runat="server">
        <table class="rightcontenttable">
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View2" runat="server">
                    <tr>
                        <td>
                            <asp:Panel runat="server" DefaultButton="ImageButtonKaydet">
                                <table class="rightcontenttable">
                                    <tr>
                                        <td colspan="3" class="UstBar">
                                            <%= AdminResource.lbLatestUpdatesTitle %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="vertical-align: top;">
                                            <asp:CheckBoxList ID="cbModuleList" runat="server" RepeatColumns="4" CssClass="GridViewStyle">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 60px;">
                                            <%= AdminResource.lbMaxItem %>
                                        </td>
                                        <td style="width: 5px;">
                                            :
                                        </td>
                                        <td align="left">
                                            <asp:TextBox runat="server" onkeydown="return onlyNumber(event);" ID="tbMaxCount"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="g1"
                                                                        ErrorMessage="!" ForeColor="Red" ControlToValidate="tbMaxCount" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="ImageButtonKaydet" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButtonKaydet_Click"
                                                        ValidationGroup="g1" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </asp:View>
            </asp:MultiView>
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
<asp:HiddenField ID="HiddenFieldId" runat="server" />