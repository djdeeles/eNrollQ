<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="MAlbumPhotos" Codebehind="AlbumPhotos.aspx.cs" %>

<%@ Register Src="UserControls/galeri/AlbumPhotos.ascx" TagName="photoAlbum" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:photoAlbum ID="photoAlbum1" runat="server" />
</asp:Content>