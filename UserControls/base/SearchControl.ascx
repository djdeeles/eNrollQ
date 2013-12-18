<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_SearchControl"
            CodeBehind="SearchControl.ascx.cs" %>
<script type="text/javascript">
    function yaz(obj) {
        var hfObj = document.getElementById("<%= hfSearchInSiteResource.ClientID %>");
        if (obj.value == "") {
            obj.value = hfObj.value;
            $("#<%= searchText.ClientID %>").addClass("searchbox");
        }

    }

    function temizle(obj) {
        if (obj.valueOf != "") {
            obj.value = "";
            $("#<%= searchText.ClientID %>").removeClass("searchbox");
        }
    }

    function Kontrol() {
        var errMsg = document.getElementById("<%= hfSearchWordMinLength.ClientID %>").value;
        var hfOb = document.getElementById("<%= hfSearchInSiteResource.ClientID %>");
        var obj = document.getElementById("<%= searchText.ClientID %>");
        if ((obj.value == "") || (obj.value == hfOb.value) || (obj.value.length < 3)) {
            alert(errMsg);
            return false;
        } else {
            return true;
        }
    }

    function focusSearchButton(e) {
        var evt = e ? e : window.event;
        var bt = document.getElementById('<%= ibSearchText.ClientID %>');
        if (bt) {
            if (evt.keyCode == 13) {
                if (Kontrol()) {
                    bt.click();
                    return false;
                }
            }
        }
    }

</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td height="20" valign="middle">
            <asp:TextBox ID="searchText" runat="server" Width="100%" Height="18px" 
                         onclick="temizle(this);" CssClass="searchbox" onmouseout="yaz(this);" onkeypress="return focusSearchButton(event)"></asp:TextBox>
        </td>
        <td align="left" valign="middle" height="20" width="27">
            <asp:ImageButton ID="ibSearchText" runat="server" ImageUrl="../../App_Themes/mainTheme/images/ara.png"
                             OnClick="ibSearchText_Click" OnClientClick=" return Kontrol(); " />
        </td>
    </tr>
</table>
<asp:HiddenField ID="hfSearchInSiteResource" runat="server" />
<asp:HiddenField ID="hfSearchWordMinLength" runat="server" />