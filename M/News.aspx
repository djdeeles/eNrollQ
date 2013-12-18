<%@ Page Title="Haberler" Language="C#" AutoEventWireup="true"
         Inherits="M_News" MasterPageFile="~/m/MasterPage.master" Codebehind="News.aspx.cs" %>

<%@ Register Src="UserControls/haber/NewsList.ascx" TagName="NewsList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:NewsList ID="NewsList1" runat="server" />
</asp:Content>