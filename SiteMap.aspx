<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"
         AutoEventWireup="true" Inherits="siteHaritasi" Codebehind="SiteMap.aspx.cs" %>

<%@ Register Src="UserControls/base/SiteMap.ascx" TagName="SiteMap" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:SiteMap ID="SiteMap1" runat="server" />
    <br />
    <br />
</asp:Content>