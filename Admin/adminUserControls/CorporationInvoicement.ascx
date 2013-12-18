<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CorporationInvoicement.ascx.cs" Inherits="eNroll.Admin.adminUserControls.CorporationInvoicement" %>
<%@ Import Namespace="Resources" %>

<fieldset style="width: 97%">
    <legend>
        <span style="cursor: pointer;"><b><%=AdminResource.lbServiceInvoice %></b>
        </span>
    </legend>
    <table id="tblServiceInvoicing" style="float: left;" width="70%" cellpadding="0" cellspacing="5">
        <tr>
            <td width="120px"><%=AdminResource.lbCorporation %></td>
            <td width="10px">:
            </td>
            <td><b>
                <asp:Label runat="server" ID="lbCorporation" /></b>
            </td>
        </tr>
        <tr>
            <td width="120px"><%=AdminResource.lbService %></td>
            <td width="10px">:
            </td>
            <td>
                <asp:Literal runat="server" ID="ltService" />
            </td>
        </tr>
        <tr>
            <td><%=AdminResource.lbAmount%>
            </td>
            <td>:
            </td>
            <td>
                <asp:Literal runat="server" ID="ltServiceAmount" />
            </td>
        </tr>
        <tr>
            <td><%=AdminResource.lbProcessUser%>
            </td>
            <td>:
            </td>
            <td>
                <asp:Literal runat="server" ID="ltProcessUser" />
            </td>
        </tr>
        <tr>
            <td><%=AdminResource.lbProcessDate%>
            </td>
            <td>:
            </td>
            <td>
                <asp:Literal runat="server" ID="ltProcessDate" />
            </td>
        </tr>
        <tr>
            <td><%=AdminResource.lbInvoiceAddress%>
            </td>
            <td>:
            </td>
            <td>
                <asp:Literal runat="server" ID="ltInvoiceAddress" />
            </td>
        </tr>
        <tr>
            <td><%=AdminResource.lbReceiptInvoiceNumber%>
            </td>
            <td>:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbReceiptNo" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbReceiptNo"
                    ForeColor="Red" ValidationGroup="vlInv" ErrorMessage="!">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><%=AdminResource.lbReceiptInvoiceDate%>
            </td>
            <td>:
            </td>
            <td>
                <telerik:RadDatePicker ID="dpReceiptInvoiceDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                    runat="server" ZIndex="30001">
                </telerik:RadDatePicker>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dpReceiptInvoiceDate"
                    ForeColor="Red" ValidationGroup="vlInv" ErrorMessage="!">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><%=AdminResource.lbDesc%>
            </td>
            <td>:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbDesc" TextMode="MultiLine" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td>
                <asp:Button runat="server" ID="btInvoiceService" CssClass="SaveCancelBtn" ValidationGroup="vlInv" OnClick="btInvoiceService_OnClick" />
                <asp:Button runat="server" ID="btInvoiceServiceCancel" CssClass="SaveCancelBtn" OnClick="btInvoiceServiceCancel_OnClick" />
            </td>
        </tr>
    </table>
</fieldset>
<asp:HiddenField runat="server" ID="hfCorporationLogId" />
