<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SearchResults" Codebehind="SearchResults.aspx.cs" %>
<%@ Register Src="UserControls/base/SearchResults.ascx" TagName="SearchResults" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:SearchResults ID="SearchResults1" runat="server" />
</asp:Content>