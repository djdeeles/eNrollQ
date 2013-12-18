<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="MemberLogin.aspx.cs" Inherits="eNroll.MemberLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <fieldset style="border: 1px solid #ccc;">
                    <legend><b>
                        <%=Resources.Resource.lbMemberLogin%></b> </legend>
                    <asp:Panel runat="server" DefaultButton="btnLogin">
                        <table cellpadding="3" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbUserName %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxUserName" runat="server" Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxUserName"
                                        ForeColor="Red" ValidationGroup="vldGroup1">!</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= Resources.Resource.lbPassword %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxPassword"
                                        ForeColor="Red" ValidationGroup="vldGroup1">!</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCaptcha ID="RadCaptcha1" runat="server" EnableRefreshImage="False" ImageStorageLocation="Cache"
                                        ValidationGroup="vldGroup1">
                                        <CaptchaImage Width="185" Height="35" />
                                    </telerik:RadCaptcha>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbRememberMe" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnLogin" CssClass="button" runat="server" OnClick="BtnLoginClick"
                                        ValidationGroup="vldGroup1" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Literal runat="server" ID="ltMemberLoginResult"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </td>
            <td style="width: 20px;">
            </td>
            <td valign="top">
                <fieldset style="border: 1px solid #ccc; height: 150px;">
                    <legend><b>
                        <%=Resources.Resource.lbPasswordReminder%></b> </legend>
                    <asp:Panel runat="server" DefaultButton="btnSendPwd">
                        <table cellpadding="3" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td width="75px">
                                    <%= Resources.Resource.lbUserName %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="tbUserName_" runat="server" Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbUserName_"
                                        ForeColor="Red" ValidationGroup="vldGroup2">!</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnSendPwd" CssClass="button" runat="server" ValidationGroup="vldGroup2"
                                        OnClick="BtnSendPwdClick" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Literal runat="server" ID="ltSendNewPasswordResult"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="vldGroup2" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </td>
        </tr>
    </table>
</asp:Content>
