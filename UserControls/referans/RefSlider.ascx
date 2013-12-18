<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_RefSlider" Codebehind="RefSlider.ascx.cs" %>
<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
<script type="text/javascript">
    $(document).ready(function() {
        $('#refslider').jcarousel({ vertical: 'true', scroll: 1, auto: 3, animation: 'slow', wrap: 'circular' });
    });
</script>