<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="PhotoGalleryMain" Codebehind="PhotoGalleryMain.aspx.cs" %>

<%@ Register Src="UserControls/galeri/PhotoGalleryMain.ascx" TagName="FotoGalleryMain"
             TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:FotoGalleryMain ID="PhotoGalleryMain1" runat="server" />
</asp:Content>