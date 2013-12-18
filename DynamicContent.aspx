<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="DynamicContent" Codebehind="DynamicContent.aspx.cs" %>

<%@ Register Src="UserControls/dinamikAlanlar/DynamicContent.ascx" TagName="DynamicContent_"
             TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:DynamicContent_ ID="DynamicContent_" runat="server" />
</asp:Content>