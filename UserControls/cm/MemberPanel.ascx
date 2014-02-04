<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberPanel.ascx.cs"
            Inherits="eNroll.UserControls.cm.MemberPanel" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="eNroll.Helpers" %>
<span class="memberpanelinfo"><b>
                                  <%= Resource.lbWelcome %></b>, <%= MemberName %>
</span>
<%= MemberImage %>
<ul class="memberpanel">
    <% if (!MemberIsLogin)
       {%>
        <li><a id='btSingIn' class="memberpanelbutton" href='/giris'>
                <%= Resource.lbLogin %></a> </li>
    <% }
       else
       {%>
        <li><a class="memberpanelbutton" href='/profil'><%= Resource.lbMyProfileInfo %></a> </li>
        <li><a class="memberpanelbutton" href='/finans'><%= Resource.lbDuesInfo %></a> </li>
        <% if (LoginType == LoginType.Admin)
           {%>
            <li><a id='btAdminPanel' href='/Admin' class="memberpanelbutton" target='_blank'>
                    <%= Resource.lbAdminPanel %></a></li>
        <% }%>
        <li><a id='btSingOut' class="memberpanelbutton" href='/Login.aspx?process=0'>
                <%= Resource.lbLogout %></a> </li>
    <% }%>
</ul>