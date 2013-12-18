<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="MAlbumVideos" Codebehind="AlbumVideos.aspx.cs" %>

<%@ Register src="UserControls/galeri/AlbumVideos.ascx" tagname="VideoGallery" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_M" Runat="Server">
    <uc1:VideoGallery ID="VideoGallery1" runat="server" />
</asp:Content>