<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="MProductCategoryMain" Codebehind="ProductCategoryMain.aspx.cs" %>
    
<%@ Register Src="UserControls/Urun/ProductCategoryMain.ascx" TagName="ProductCategoryMain"
             TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:ProductCategoryMain ID="p" runat="server" />
</asp:Content>