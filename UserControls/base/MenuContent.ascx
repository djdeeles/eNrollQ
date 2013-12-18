<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_base_MenuContent" Codebehind="MenuContent.ascx.cs" %>
<%@ Register Src="MenuData.ascx" TagName="MenuData" TagPrefix="uc1" %>
<%@ Register Src="MenuSubMenu.ascx" TagName="MenuSubMenu" TagPrefix="uc2" %>
<%@ Register src="MenuDynamicList.ascx" tagname="MenuDynamicList" tagprefix="uc3" %>
<uc1:MenuData ID="MenuData1" runat="server" />
<uc2:MenuSubMenu ID="MenuSubMenu1" runat="server" />
<uc3:MenuDynamicList ID="MenuDynamicList1" runat="server" />