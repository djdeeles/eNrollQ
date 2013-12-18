<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="eNroll.Products" Codebehind="Products.aspx.cs" %>

<%@ Register Src="UserControls/urun/Products.ascx" TagName="Products" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:Products ID="Products1" runat="server" />
</asp:Content>