<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="M_Products" Codebehind="Products.aspx.cs" %>

<%@ Register Src="UserControls/urun/Products.ascx" TagName="Products" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:Products ID="Products1" runat="server" />
</asp:Content>