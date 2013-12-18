<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_base_MenuContent" Codebehind="MenuContent.ascx.cs" %>

<%@ Register Src="MenuData.ascx" TagName="MenuData" TagPrefix="uc2" %>
<%@ Register src="MenuDynamicList.ascx" tagname="MenuDynamicList" tagprefix="uc3" %>


<uc2:MenuData ID="MenuData_M" runat="server" />
<uc3:MenuDynamicList ID="MenuDynamicList_M" runat="server" />