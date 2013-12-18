<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true"
         Inherits="MNoticeDetails" CodeBehind="NoticeDetails.aspx.cs" %>

<%@ Register Src="UserControls/duyuru/NoticeContent.ascx" TagName="NoticeContent"
             TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:NoticeContent ID="NoticeContent1" runat="server" />
</asp:Content>