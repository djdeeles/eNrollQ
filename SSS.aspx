<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SSS" Codebehind="SSS.aspx.cs" %>

<%@ Register Src="UserControls/SSS/SSS.ascx" TagName="SSS" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:SSS ID="SSS1" runat="server" />
</asp:Content>