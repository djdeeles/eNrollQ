<%@ Page Title="" Language="C#"  AutoEventWireup="true" MasterPageFile="~/m/MasterPage.master" Inherits="MSearchResults" Codebehind="SearchResults.aspx.cs" %>
<%@ Register Src="UserControls/base/SearchResults.ascx" TagName="SearchResults" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_M" Runat="Server">
    <uc1:SearchResults ID="SearchResults1" runat="server" />
</asp:Content>