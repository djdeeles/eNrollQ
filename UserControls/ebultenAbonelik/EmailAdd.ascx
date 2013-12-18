<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_EmailAdd" CodeBehind="EmailAdd.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<script type="text/javascript">
    function yaz(obj, controls) {
        var hfObj = "";

        if (controls == "1")
            hfObj = document.getElementById("<%= hfNameResource.ClientID %>");
        if (controls == "2")
            hfObj = document.getElementById("<%= hfSurnameResource.ClientID %>");
        if (controls == "3")
            hfObj = document.getElementById("<%= hfEmailResource.ClientID %>");

        if (obj.value == "") {
            obj.value = hfObj.value;
        }
    }

    function temizle(obj) {
        obj.value = "";
    }
</script>
<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0"> 
    <asp:View ID="View1" runat="server">
        <table border="0" cellpadding="3" cellspacing="0" style="max-width: 300px;">
            <tr>
                <td align="left" valign="top">
                    <asp:TextBox runat="server" ID="tbName" Font-Italic="True" Font-Size="Small" ForeColor="#CCCCCC"
                                 onclick="temizle(this);" Width="100%" />
                </td>
                <td align="left" valign="top">   
                    <asp:TextBox runat="server" ID="tbSurname" Font-Italic="True" Font-Size="Small" ForeColor="#CCCCCC"
                                 onclick="temizle(this);" Width="100%" />
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" colspan="2">
                    <asp:TextBox ID="TextBoxEmail" Font-Italic="True" Font-Size="Small" ForeColor="#CCCCCC"
                                 onclick="temizle(this);" runat="server" Width="100%" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxEmail"
                                                ValidationGroup="g1" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxEmail"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="g1"
                                                    Display="None"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top"  colspan="2">
                    <asp:Label ID="lblMessage1" runat="server"><%= Resource.lbEmailAdd %></asp:Label>
                    <asp:ImageButton ID="ImageButtonEkle" runat="server" ValidationGroup="g1" OnClick="ImageButtonEkle_Click" />
                </td>
            </tr>
            <tr>
                <td align="left" valign="top"  colspan="2">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="g1" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="View2" runat="server">
        <asp:Label ID="lblMessage2" runat="server"><%= Resource.lbEmailAddSuccess %>
        </asp:Label>
    </asp:View>
</asp:MultiView>
<asp:HiddenField ID="hfNameResource" runat="server"/>
<asp:HiddenField ID="hfSurnameResource" runat="server"/>
<asp:HiddenField ID="hfEmailResource" runat="server"/>