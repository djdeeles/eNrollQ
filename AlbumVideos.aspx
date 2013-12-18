<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="AlbumVideos" Codebehind="AlbumVideos.aspx.cs" %>

<%@ Register src="UserControls/galeri/AlbumVideos.ascx" tagname="VideoGallery" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:VideoGallery ID="VideoGallery1" runat="server" />
</asp:Content>