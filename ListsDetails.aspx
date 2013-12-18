<%@ Page Language="C#" AutoEventWireup="true" Inherits="eNroll.ListsDetails" MasterPageFile="~/MasterPage.master"
         CodeBehind="ListsDetails.aspx.cs" %>

<%@ Register Src="UserControls/DynamicLists/DynamicListDetail.ascx" TagName="DynamicListDetail" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:DynamicListDetail ID="DynamicListDetail1" runat="server" />
</asp:Content>