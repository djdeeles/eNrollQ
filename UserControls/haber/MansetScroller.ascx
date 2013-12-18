<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_haber_MansetScroller" CodeBehind="MansetScroller.ascx.cs" %>
<script src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js" type="text/javascript"></script>
<asp:Panel ID="pnlPhoto" runat="server">
</asp:Panel>
<asp:Panel ID="pnlTabs" runat="server">
</asp:Panel>
<div style="clear: both;" >
</div>
<script type="text/javascript">
    $(function() {
        $(".slidetabs").tabs(".mansethaber > div", {
            effect: 'fade',
            fadeOutSpeed: "slow",
            rotate: true
        }).slideshow();
    });
</script>