<%@ Page Language="C#" AutoEventWireup="true" Inherits="Admin_Login" Codebehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
    </head>
    <body>
        <form id="form1" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager1" ScriptMode="Auto" EnableCdn="false"
                                      runat="server" />
            <link rel="stylesheet" href="App_Themes/mainTheme/css/login.css" type="text/css" />
            <div class="loginust"></div>
            <div class="logincontainer">
                <a href="default.aspx"><div class="musterilogo"></div></a>
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <div class="loginpanel">
                            <table cellpadding="3" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td width="75px">
                                        <!--<%= Resources.Resource.lbUILanguage %>!-->
					<img src="App_Themes/mainTheme/images/login/language.png" height="25px" />
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAdminLanguage" AutoPostBack="True"  OnSelectedIndexChanged="ddlAdminLanguage_Changed"
                                                          runat="server" Width="185px">
                                            <asp:ListItem Value="1">English</asp:ListItem>
                                            <asp:ListItem Value="2">Türkçe</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
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
                                        <telerik:RadCaptcha ID="RadCaptcha1" runat="server" EnableRefreshImage="False"
                                                            ImageStorageLocation="Cache" 
                                                            ValidationGroup="vldGroup1">
                                            <CaptchaImage Width="185" Height="35"/>
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
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnLogin" CssClass="SaveCancelBtn" runat="server" OnClick="BtnWebLoginClick"
                                                    ValidationGroup="vldGroup1" />
                                        <asp:Button ID="btnForgetPwd" CssClass="SaveCancelBtn" runat="server" OnClick="LinkButton1_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <div class="loginpanel">
                            <table cellpadding="3" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td width="75px">
                                        <%= Resources.Resource.lbUserName %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1UserName" runat="server" Width="180px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox1UserName"
                                                                    ForeColor="Red" ValidationGroup="vldGroup2">!</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnBack" CssClass="SaveCancelBtn" runat="server" OnClick="LinkButton2_Click">
                                        </asp:Button>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSendPwd" CssClass="SaveCancelBtn" runat="server" ValidationGroup="vldGroup2"
                                                    OnClick="BtnSendPwdClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="vldGroup2" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
            <div class="loginalt"></div>
            <asp:HiddenField ID="hfSelectedLanguageId" runat="server" Value="0" />
        </form>
    </body>
</html>