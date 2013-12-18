<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_NoticeScroller" Codebehind="NoticeScroller.ascx.cs" %>
<asp:Literal ID="Literal1" runat="server"></asp:Literal>
<script type="text/javascript">
    $(document).ready(function() {
        $('#duyuru').jcarousel({ /* vertical: 'true' ,*/ scroll: 1, auto: 5, animation: 'slow', wrap: 'circular' });
    });
</script>