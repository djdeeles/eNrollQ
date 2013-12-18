<%@ Page Language="C#" AutoEventWireup="true"
         Inherits="eNroll.LLists" MasterPageFile="~/MasterPage.master" Codebehind="Lists.aspx.cs" %>

<%@ Register Src="UserControls/DynamicLists/DynamicList.ascx" TagName="Lists" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:Lists ID="Lists1" runat="server" />
</asp:Content>