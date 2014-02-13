<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicListDetail.ascx.cs" Inherits="UserControls_DynamicListDetail" %>
<%@ Import Namespace="Resources" %>
<h1>
    <asp:Label ID="lblBaslik" runat="server" /></h1>
<asp:Image ID="Image1" runat="server" CssClass="contentimage" />
<asp:Label ID="lblYazi" runat="server" />
<div class="attachments">
    <asp:Literal ID="ltAttachments" runat="server"/> 
    <asp:LinkButton ID="lbDownloadAll"  runat="server" OnClick="btnDownloadAll_OnClick"/>
</div>

<p class="contentbackcontainer">
    <a class="button" href="/Default.aspx"><%= Resource.lbHome %></a>
    <a class="button" href='/listeler-<%= hfListId.Value %>-<%= hfPageIndex.Value%>'><%= Resource.lbBack %></a>
</p>

<asp:HiddenField runat="server" ID="hfListId"/>
<asp:HiddenField runat="server" ID="hfListDataId"/>
<asp:HiddenField runat="server" ID="hfPageIndex"/>