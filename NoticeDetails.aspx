<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="NoticeDetails" Codebehind="NoticeDetails.aspx.cs" %>
<%@ Register src="UserControls/duyuru/NoticeContent.ascx" tagname="NoticeContent" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <uc1:NoticeContent ID="NoticeContent1" runat="server" />

</asp:Content>