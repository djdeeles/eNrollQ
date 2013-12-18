<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="VideoGalleryMain" Codebehind="VideoGalleryMain.aspx.cs" %>

<%@ Register src="UserControls/galeri/VideoGalleryMain.ascx" tagname="VideoGallery" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:VideoGallery ID="VideoGallery1" runat="server" />
</asp:Content>