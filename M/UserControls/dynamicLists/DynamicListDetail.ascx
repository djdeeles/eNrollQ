<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicListDetail.ascx.cs"
    Inherits="M_UserControls_DynamicListDetail" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lblBaslik" runat="server" /></h1>
<p><asp:Image ID="Image1" runat="server" Width="100%"/></p>
<p><asp:Label ID="lblYazi" runat="server" /></p>
<a data-role="button" data-corners="false" href='/m/' href="/Default.aspx"><%= Resource.lbHome %></a>