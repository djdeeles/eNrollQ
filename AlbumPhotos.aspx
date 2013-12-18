<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="AlbumPhotos" Codebehind="AlbumPhotos.aspx.cs" %>

<%@ Register Src="UserControls/galeri/AlbumPhotos.ascx" TagName="photoAlbum" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:photoAlbum ID="photoAlbum1" runat="server" />
</asp:Content>