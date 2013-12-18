<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
         CodeBehind="404.aspx.cs" Inherits="_404" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="UserControls/base/SearchControl.ascx" TagName="searchControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="hata">
        <p class="hatatitle">
            <%= Resource.lb404Title %></p>
        <p class="hatamsg">
            <%= Resource.lb404msg %></p>
	<p><uc1:searchControl ID="searchControl1" runat="server" /></p>
    </div>
</asp:Content>