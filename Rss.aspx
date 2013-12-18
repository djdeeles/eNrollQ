<%@ Page Title="" Language="C#" AutoEventWireup="true"
         Inherits="eNroll.Rss" MasterPageFile="~/MasterPage.master" Codebehind="Rss.aspx.cs" %>

<%@ Register Src="UserControls/rss/Rss.ascx" TagName="Rss"
             TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:Rss ID="Rss1" runat="server" />
</asp:Content>