<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" Inherits="admin_enrollDestek" Codebehind="enrollDestek.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="rightcontenttable">
        <tr>
            <td colspan="3" style="text-align: center">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                <%= AdminResource.lbSeverityRating %>
            </td>
            <td style="width: 10px">
                :
            </td>
            <td>
                <asp:DropDownList ID="DropDownList1" runat="server" Width="150px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <%= AdminResource.lbSubject %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Width="500px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                            ControlToValidate="TextBox1" ErrorMessage="(!)" SetFocusOnError="True" ValidationGroup="g1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <%= AdminResource.lbMessage %>
            </td>
            <td valign="top">
                :
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server" Height="200px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2"
                                            ErrorMessage="(!)" SetFocusOnError="True" ForeColor="Red" ValidationGroup="g1"></asp:RequiredFieldValidator>
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
                <asp:Label ID="Label5" runat="server" Style="color: #FF0000"></asp:Label>
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
                <asp:Button ID="ImageButton1" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButton1_Click"
                            ValidationGroup="g1" />
            </td>
        </tr>
    </table>
</asp:Content>