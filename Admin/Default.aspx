<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true" Inherits="Admin_Default" Codebehind="Default.aspx.cs" %>
<%@ Import Namespace="Resources" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="rightcontenttable GridViewStyle">
        <tr>
            <th align="left" valign="middle" style="width: 49%; font-size: 14px;" >
                <b>eNroll Yönetim Paneline Hoşgeldiniz</b>
            </th>
            <td style="width: 2%; border: 0px;" ></td>
            <th align="left" valign="middle" style="width: 49%; font-size: 14px;">
                <b>eNroll'dan Haberler</b>
            </th>
        </tr>
        <tr>
            <td style="width: 49%; border: 1px solid #ccc; padding: 5px; vertical-align: top !important;" >
                <%= AdminResource.lbWelcome %>
            </td>
            <td style="width: 2%; border: 0px;" ></td>
            <td style="width: 49%; border: 1px solid #ccc; padding: 5px; vertical-align: top !important;">
                <marquee width="100%" direction="up" scrollamount="1" onmouseover=" this.stop(); " onmouseout=" this.start(); "> 
                    <asp:Panel ID="Panel1" runat="server"></asp:Panel></marquee>
            </td>
        </tr>
    </table>
</asp:Content>