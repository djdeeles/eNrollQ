<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ProductCategoryMain" Codebehind="ProductCategoryMain.aspx.cs" %>
    
<%@ Register Src="UserControls/Urun/ProductCategoryMain.ascx" TagName="ProductCategoryMain"
             TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:ProductCategoryMain ID="p" runat="server" />
</asp:Content>