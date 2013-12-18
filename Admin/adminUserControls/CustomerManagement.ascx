<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_ChangePassword"
    CodeBehind="CustomerManagement.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:Panel runat="server" DefaultButton="btnChangeEmail">
    <table>
        <tr>
            <td width="100px">
                <%= AdminResource.lbUILanguage %>
            </td>
            <td width="10px">
                :
            </td>
            <td>
                <asp:DropDownList ID="ddlAdminLanguage" AutoPostBack="True" OnSelectedIndexChanged="ddlAdminLanguage_Changed"
                    runat="server">
                    <asp:ListItem Value="1">English</asp:ListItem>
                    <asp:ListItem Value="2">Türkçe</asp:ListItem>
                </asp:DropDownList>
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
                <%= AdminResource.lbName %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbName" Width="200px"></asp:TextBox>
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
                <asp:TextBox runat="server" ID="tbSurname" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <%= AdminResource.lbEPosta %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="tbEmaiAdress" runat="server" Width="200px" ValidationGroup="email"></asp:TextBox>
                <asp:Button ID="btnChangeEmail" runat="server" CssClass="SaveCancelBtn" ValidationGroup="email"
                    OnClick="btChangeName_Click" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEmaiAdress"
                    ErrorMessage="!" ForeColor="Red" ValidationGroup="email"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel runat="server" DefaultButton="btnChangePassword">
    <table>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td width="100px">
                <%= AdminResource.lbOldPwd %>
            </td>
            <td width="10px">
                :
            </td>
            <td>
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <%= AdminResource.lbNewPwd %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="txtNew" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <%= AdminResource.lbNewPwdAgain %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="txtReNew" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
                <asp:Button ID="btnChangePassword" runat="server" CssClass="SaveCancelBtn" OnClick="btnChangePassword_Click" />
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
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEmaiAdress"
                    SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ForeColor="Red" ValidationGroup="email"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:HiddenField ID="hfSelectedLanguageId" runat="server" />
