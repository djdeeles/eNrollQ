<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_SubMenu"
            EnableViewState="false" CodeBehind="SubMenu.ascx.cs" %>
<% if (MenuList != null)
   {%>
    <ul data-role="listview" data-corners="false" data-inset="true">
        <% foreach (var menu in MenuList)
           {%>
            <%--<li><a href="<%= DetectMobileMenuType(menu) %>" data-transition="flip">--%>
            <li><a href="<%= DetectMobileMenuType(menu) %>" >
                    <%= menu.name %>
                </a></li>
        <% } %>
    </ul>
<% }%>