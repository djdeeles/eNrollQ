<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Albums" Codebehind="Albums.aspx.cs" %>

<%@ Register Src="UserControls/galeri/Albums.ascx" TagName="FotoGalleryMain"
             TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:FotoGalleryMain ID="FotoGalleryMain1" runat="server" />
</asp:Content>