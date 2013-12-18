<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master"
         AutoEventWireup="true" Inherits="M_SiteMap" Codebehind="SiteMap.aspx.cs" %>

<%@ Register Src="UserControls/base/SiteMap.ascx" TagName="SiteMap" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:SiteMap ID="SiteMap1" runat="server" />
    <br />
    <br />
</asp:Content>