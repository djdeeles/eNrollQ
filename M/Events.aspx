<%@ Page Title="" Language="C#" MasterPageFile="~/m/MasterPage.master" AutoEventWireup="true" Inherits="M_Events" Codebehind="Events.aspx.cs" %>

<%@ Register src="UserControls/etkinlik/EventList.ascx" tagname="EventList" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" Runat="Server">
    <uc1:EventList ID="EventList1" runat="server" />
</asp:Content>