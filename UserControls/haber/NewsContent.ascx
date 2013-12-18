<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_NewsContent"
    CodeBehind="NewsContent.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lblBaslik" runat="server" />
    <asp:Label ID="lbDate" runat="server" CssClass="contentdate"/>
</h1>

<asp:Image ID="Image1" runat="server" CssClass="contentimage" />
<asp:Label ID="lblYazi" runat="server" />
<p class="contentbackcontainer">
    <a class="button" href="/Default.aspx"><%= Resource.lbHome %></a>
    <a class="button" href="/haberler-1"><%= Resource.lbAllNews %></a>
</p>
