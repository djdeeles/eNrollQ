<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberSendSms.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.MemberSendSms" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View ID="vAuth" runat="server">
        <table>
            <tr>
                <td style="width: 165px;">
                    <%=AdminResource.lbJobName %>
                </td>
                <td style="width: 10px;">
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbJobName" Width="200px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbJobName"
                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=AdminResource.lbSendTime%>
                </td>
                <td>
                    :
                </td>
                <td>
                    <telerik:RadDatePicker ID="dpSmsSendDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                        runat="server" ZIndex="30001" Width="100px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dpSmsSendDate"
                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                    <telerik:RadTimePicker runat="server" ID="tpSmsSendTime" Width="100px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tpSmsSendTime"
                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <%=AdminResource.lbContent%>
                </td>
                <td valign="top">
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbSmsContent" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <%=AdminResource.lbAddInfoToContent%>
                </td>
                <td>
                    :
                </td>
                <td>
                    <select id="editorOptions" onchange="modulekle();">
                        <option value="">
                            <%=AdminResource.lbChoose %></option>
                        <option value="%%name%%">
                            <%=AdminResource.lbName%></option>
                        <option value="%%surname%%">
                            <%=AdminResource.lbSurname%></option>
                        <option value="%%generaldebt%%">
                            <%=AdminResource.lbGeneralDebt%></option>
                        <option value="%%clubnumber%%">
                            <%=AdminResource.lbClubNumber%></option>
                    </select>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblUyari"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button runat="server" ID="btSendReport" CssClass="SaveCancelBtn" ValidationGroup="g1"
            OnClick="BtnSendSmsClick" />
        <asp:HiddenField runat="server" ID="hfSqlQuery" />
    </asp:View>
    <asp:View ID="vNotAuth" runat="server">
        <table class="rightcontenttable">
            <tr>
                <td>
                    <p class="noauth">
                        <%= AdminResource.msgUnauthorizedUser %>
                    </p>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>