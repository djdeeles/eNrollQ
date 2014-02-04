<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinanceViewer.aspx.cs"
         Inherits="eNroll.Admin.FinanceViewer" %>
<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <link href="/Admin/style/admin.css" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" href="../App_Themes/mainTheme/css/mainTheme.css" type="text/css" />
        <script src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js" type="text/javascript"></script>
    </head>
    <body style="width: 100%; background-color: #fff; background-image: none;">
        <form id="form1" runat="server" viewstatemode="Enabled">
            <asp:MultiView runat="server" ID="mvFinance" ActiveViewIndex="0">
                <asp:View runat="server" ID="vPaymentDetail">
                    <table cellpadding="3" cellspacing="5" class="GridViewStyle" style="width: 250px;">
                        <th colspan="2">
                            <%= AdminResource.lbPayment %>
                        </th>
                        <tr>
                            <td width="115px">
                                <b><%= AdminResource.lbPaymentType %></b>
                            </td>
                            <td width="115px">
                                <asp:Label runat="server" ID="lbPaymentType"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbAmount %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbAmount"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbReceiptInvoiceNumber %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbReceiptNumber"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbReceiptInvoiceDate %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbReceiptDate"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbProcessUser %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbProcessUser"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbProcessDate %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbProcessDate"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View runat="server" ID="vDuesDetail">
                    <table cellpadding="3" cellspacing="5" class="GridViewStyle" style="width: 250px;">
                        <th colspan="2">
                            <%= AdminResource.lbDebiting %>
                        </th>
                        <tr>
                            <td width="110px">
                                <b><%= AdminResource.lbDuesType %></b>
                            </td>
                            <td width="115px">
                                <asp:Label runat="server" ID="lbDuesType"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbAmount %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbDuesAmount"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbProcessUser %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbDuesProcessUser"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b><%= AdminResource.lbProcessDate %></b>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbDuesProcessDate"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vNotFound" runat="server">
                    <p>
                        <%= AdminResource.lbNotFound %>
                    </p>
                </asp:View> 
            </asp:MultiView>
        </form>
    </body>
</html>