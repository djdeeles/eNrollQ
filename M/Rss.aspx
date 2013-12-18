<%@ Page Title="" Language="C#" AutoEventWireup="true"
         Inherits="eNroll.M_Rss" MasterPageFile="~/m/MasterPage.master" Codebehind="Rss.aspx.cs" %>
<%@ Register Src="UserControls/rss/Rss.ascx" TagName="Rss"
             TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_M" runat="Server">
    <uc1:Rss ID="Rss_m" runat="server" />
</asp:Content>