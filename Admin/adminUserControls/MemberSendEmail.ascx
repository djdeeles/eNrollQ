<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberSendEmail.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.MemberSendEmail" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/adminUserControls/Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View ID="vAuth" runat="server">
        <table>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblUyari" runat="server" ForeColor="#FF0000">
                
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 100px;">
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
                    <%=AdminResource.lbSendTime %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <telerik:RadDatePicker ID="dpMailSendDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                        runat="server" ZIndex="30001" Width="100px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dpMailSendDate"
                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                    <telerik:RadTimePicker runat="server" ID="tpMailSendTime" Width="100px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tpMailSendTime"
                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=AdminResource.lbTemplate %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <p>
                        <asp:DropDownList runat="server" ID="ddlMailTemplates" AutoPostBack="True" OnSelectedIndexChanged="ddlMailTemplates_OnSelectedIndexChanged" />
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <%=AdminResource.lbSubject %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbMailSubject" Width="200px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbMailSubject"
                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td>
                    <%=AdminResource.lbContent %>
                </td>
                <td>
                    :
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <uc1:Rtb runat="server" ID="RtbMailContent" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <%=AdminResource.lbAddInfoToContent %>
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
                <td style="width: 210px">
                    <%=AdminResource.lbEmailReadReport %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rbSendReport" RepeatColumns="2" />
                </td>
            </tr>
            <tr>
                <td style="width: 210px">
                    <%=AdminResource.lbSaveEmailAsTemplate %>
                </td>
                <td>
                    :
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbSaveAsTemplate" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Button runat="server" ID="btSendReport" CssClass="SaveCancelBtn" ValidationGroup="g1"
            OnClick="BtnSendMailClick" />
        <asp:Button runat="server" ID="btUpdateTask" CssClass="SaveCancelBtn" ValidationGroup="g1"
            Visible="False" OnClick="btUpdateTaskClick" />
        <asp:HiddenField runat="server" ID="hfTaskId" />
        <asp:HiddenField runat="server" ID="hfSqlQuery" />
        <asp:HiddenField runat="server" ID="hfTaskName" />
        <asp:HiddenField runat="server" ID="hfMailSubject" />
        <asp:HiddenField runat="server" ID="hfMailContent" />
        <asp:HiddenField runat="server" ID="hfMailSendDate" />
        <asp:HiddenField runat="server" ID="hfMailSendTime" />
        <asp:HiddenField runat="server" ID="hfSendReport" />
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