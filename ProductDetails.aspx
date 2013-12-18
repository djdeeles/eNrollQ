<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ProductDetails" Codebehind="ProductDetails.aspx.cs" %>

<%@ Register Src="UserControls/Urun/ProductDetails.ascx" TagName="ProductDetails"
             TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:ProductDetails ID="ProductDetails1" runat="server" />
</asp:Content>