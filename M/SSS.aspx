<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="M_SSS" Codebehind="SSS.aspx.cs" %>

<%@ Register Src="UserControls/SSS/SSS.ascx" TagName="SSS" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:SSS ID="SSS1" runat="server" />
</asp:Content>