<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="eNroll.Notices" Codebehind="Notices.aspx.cs" %>

<%@ Register Src="~/UserControls/duyuru/NoticeList.ascx" TagName="NoticeList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:NoticeList ID="NoticeList1" runat="server" />
</asp:Content>