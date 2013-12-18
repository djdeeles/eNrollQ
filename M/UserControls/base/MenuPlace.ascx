<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_MenuPlace"
            CodeBehind="MenuPlace.ascx.cs" %>
<%@ Register Src="SubMenu.ascx" TagName="SubMenu" TagPrefix="uc1" %>
<uc1:SubMenu ID="SubMenu_M" runat="server" />
<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>