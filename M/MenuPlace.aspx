<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true"
         ValidateRequest="false" Inherits="MMenuPlace" CodeBehind="MenuPlace.aspx.cs" %>

<%@ Register Src="UserControls/base/MenuPlace.ascx" TagName="MenuPlace" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:MenuPlace ID="MenuPlace1" runat="server" />
</asp:Content>