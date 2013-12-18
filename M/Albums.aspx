<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="MAlbums" Codebehind="Albums.aspx.cs" %>

<%@ Register Src="UserControls/galeri/Albums.ascx" TagName="FotoGalleryMain"
             TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:FotoGalleryMain ID="FotoGalleryMain1" runat="server" />
</asp:Content>