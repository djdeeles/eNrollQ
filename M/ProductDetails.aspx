<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="MProductDetails" Codebehind="ProductDetails.aspx.cs" %>

<%@ Register Src="UserControls/Urun/ProductDetails.ascx" TagName="ProductDetails"
             TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:ProductDetails ID="ProductDetails1" runat="server" />
</asp:Content>