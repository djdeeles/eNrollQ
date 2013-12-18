<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="eNroll.Events" Codebehind="Events.aspx.cs" %>

<%@ Register src="UserControls/etkinlik/EventList.ascx" tagname="EventList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:EventList ID="EventList1" runat="server" />
</asp:Content>