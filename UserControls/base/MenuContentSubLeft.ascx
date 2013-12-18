<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_MenuContentSubLeft" Codebehind="MenuContentSubLeft.ascx.cs" %>
<%@ Register src="MenuSubMenuLeftTree.ascx" tagname="MenuSubMenuLeftTree" tagprefix="uc1" %>
<%@ Register src="MenuContent.ascx" tagname="MenuContent" tagprefix="uc2" %>

<div style="float: left; padding: 0 10px 10px 0;"><uc1:MenuSubMenuLeftTree ID="MenuSubMenuLeftTree1" runat="server" /></div>
<div><uc2:MenuContent ID="MenuContent1" runat="server" /></div>