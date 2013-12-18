<%@ Page Language="C#" AutoEventWireup="true"
         Inherits="M_LLists" MasterPageFile="~/m/MasterPage.master" Codebehind="Lists.aspx.cs" %>

<%@ Register Src="UserControls/DynamicLists/DynamicList.ascx" TagName="Lists" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:Lists ID="Lists1" runat="server" />
</asp:Content>