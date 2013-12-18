<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="NewsDetails" Codebehind="NewsDetails.aspx.cs" %>

<%@ Register Src="UserControls/haber/NewsContent.ascx" TagName="NewsContent" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:NewsContent ID="NewsContent1" runat="server" />
</asp:Content>