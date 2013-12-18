<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" ValidateRequest="false" Inherits="MenuPlace" Codebehind="MenuPlace.aspx.cs" %>

<%@ Register Src="UserControls/base/MenuPlace.ascx" TagName="MenuPlace" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:MenuPlace ID="MenuPlace1" runat="server" />
</asp:Content>