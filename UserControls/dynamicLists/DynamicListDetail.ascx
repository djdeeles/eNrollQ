<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicListDetail.ascx.cs" Inherits="UserControls_DynamicListDetail" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lblBaslik" runat="server" /></h1>
<asp:Image ID="Image1" runat="server" CssClass="contentimage" />
<asp:Label ID="lblYazi" runat="server" />
<p class="contentbackcontainer">
    <a class="button" href="/Default.aspx"><%= Resource.lbHome %></a>
    <a class="button" href='/listeler-<%= Session["listItemshfListId"] %>-<%= Session["listItemPageIndex"] %>'><%= Resource.lbBack %></a>
</p>