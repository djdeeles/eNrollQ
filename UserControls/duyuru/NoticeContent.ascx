<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_duyuru_NewsContent"
    CodeBehind="NoticeContent.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lblBaslik" runat="server"/>
    <asp:Label ID="lbDate" runat="server" CssClass="contentdate"/>
</h1>
<asp:Image ID="Image1" runat="server" CssClass="contentimage" />
<asp:Label ID="lblYazi" runat="server"></asp:Label>
<p class="contentbackcontainer">
    <a class="button" href="/Default.aspx"><%= Resource.lbHome %></a> 
    <a class="button" href="/duyurular-1"><%= Resource.lbAllNotices %></a>
</p>