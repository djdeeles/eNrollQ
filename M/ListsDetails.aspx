<%@ Page Language="C#" AutoEventWireup="true" Inherits="M_ListsDetails" MasterPageFile="~/m/MasterPage.master"
         CodeBehind="ListsDetails.aspx.cs" %>

<%@ Register Src="UserControls/DynamicLists/DynamicListDetail.ascx" TagName="DynamicListDetail" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:DynamicListDetail ID="DynamicListDetail1" runat="server" />
</asp:Content>