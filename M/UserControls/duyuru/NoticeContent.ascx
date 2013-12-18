<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_duyuru_NewsContent"
    CodeBehind="NoticeContent.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lblBaslik" runat="server"/>
     <asp:Label ID="lbDate" CssClass="contentdate" runat="server"/>
</h1>
<p>
    <asp:Image ID="Image1" runat="server" Width="100%" /></p>
<p>
    <asp:Label ID="lblYazi" runat="server"/></p>
<a href="/m/" data-role="button" data-corners="false"><%= Resource.lbHome %></a>
<a href="/m/duyurular-1" data-role="button" data-corners="false"><%= Resource.lbAllNotices %></a>