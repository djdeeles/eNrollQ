<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ProductScroller" Codebehind="ProductScroller.ascx.cs" %>
<div id="productscrollerdiv">
    <ul id="productscroller" class="productscrollerskin">
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </ul>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#productscroller').jcarousel({
            vertical: false,
            scroll: 1,
            auto: 5,
            animation: 400,
            wrap: 'circular'
        });
    });
</script>