<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="M_Notices" Codebehind="Notices.aspx.cs" %>

<%@ Register Src="~/m/UserControls/duyuru/NoticeList.ascx" TagName="NoticeList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:NoticeList ID="NoticeList1" runat="server" />
</asp:Content>