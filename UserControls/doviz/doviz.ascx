<%@ Control Language="C#" AutoEventWireup="true" Inherits="doviz" Codebehind="doviz.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<table class="doviz">
    <tr>
        <td colspan="3" class="dovizTepe">
            <asp:Label ID="lbExcTitle" runat="server"><%= Resource.lbExcTitle %></asp:Label></td>
    </tr>
    <tr>
        <td colspan="3" >&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;
        </td>
        <td>
            <b><asp:Label ID="lbExcBuy" runat="server"><%= Resource.lbExcBuy %></asp:Label></b>
        </td>
        <td>
            <b><asp:Label ID="lbExcSale" runat="server"><%= Resource.lbExcSale %></asp:Label></b>
        </td>
    </tr>
    <tr>
        <td>
            <b>Dolar</b></td>
        <td>
            <asp:Label ID="Label1" runat="server"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label2" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <b>Euro</b></td>
        <td>
            <asp:Label ID="Label3" runat="server"></asp:Label>
        </td>
        <td>
            <asp:Label ID="Label4" runat="server"></asp:Label>
        </td>
    </tr>
</table>