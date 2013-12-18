<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="M_NewsDetails" Codebehind="NewsDetails.aspx.cs" %>

<%@ Register Src="UserControls/haber/NewsContent.ascx" TagName="NewsContent" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:NewsContent ID="NewsContent1" runat="server" />
</asp:Content>