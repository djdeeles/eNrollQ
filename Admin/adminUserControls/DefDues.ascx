<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefDues.ascx.cs" Inherits="eNroll.Admin.adminUserControls.DefDues" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>Aidat</legend>
    <div style="float: left; margin-right: 15px;">
        <asp:ListBox runat="server" ID="lbDues" DataSourceID="edsDues" DataValueField="Id"
                     DataTextField="Title" Width="300px" Height="150px" SelectionMode="Single" AutoPostBack="True"
                     Enabled="True" OnSelectedIndexChanged="lbDuesTypes_OnSelectedIndexChanged" />
    </div>
    <style type="text/css">
        .deptAmount { text-align: right; }
    </style>
    <div style="float: left; width: 350px;">
        <table style="width: 100%">
            <tr>
                <td width="50px" valign="top">
                    <%= AdminResource.lbNew %>
                </td>
                <td width="10px">
                    &nbsp;
                </td>
                <td width="285px">
                    <table width="285px" border="0">
                        <tr>
                            <td style="width: 100px;">
                                <%= AdminResource.lbTitle %>
                            </td>
                            <td style="width: 10px;">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="tbNewDues" Width="140px" runat="server" />
                                <label style="width: 50px;" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewDues"
                                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g1" ErrorMessage="!" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbDues %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltNewAmountSymbolL" />
                                <asp:TextBox ID="tbNewAmount" Width="50px" runat="server" onkeydown="return onlyNumber(event);"
                                             CssClass="deptAmount" />,
                                <asp:TextBox ID="tbNewAmountKrs" Width="18px" runat="server" onkeydown="return onlyNumber(event);"
                                             CssClass="deptAmount" MaxLength="2" Text="00" />
                                <asp:Literal runat="server" ID="ltNewAmountSymbolR" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbNewAmount"
                                                            SetFocusOnError="True" ForeColor="Red" Display="Dynamic" ValidationGroup="g1"
                                                            ErrorMessage="!" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" SetFocusOnError="True"
                                                            ControlToValidate="tbNewAmountKrs" ForeColor="Red" Display="Dynamic" ValidationGroup="g1"
                                                            ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tekil
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <label style="width: 50px;" />
                                <asp:CheckBox runat="server" ID="cbUniqe" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button runat="server" ID="btSaveDues" ValidationGroup="g1" OnClick="BtSaveDuesTypes_OnClick"
                                            Width="50px" CssClass="SaveCancelBtn" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <%= AdminResource.lbEdit %>
                </td>
                <td>
                    &nbsp;
                </td>
                <td width="285px">
                    <table width="285px" border="0">
                        <tr>
                            <td style="width: 100px;">
                                <%= AdminResource.lbTitle %>
                            </td>
                            <td style="width: 10px;">
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="tbEditDues" Width="140px" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditDues"
                                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g2" ErrorMessage="!" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbDues %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltEditAmountSymbolL" />
                                <asp:TextBox ID="tbEditAmount" Width="50px" runat="server" onkeydown="return onlyNumber(event);"
                                             CssClass="deptAmount" />,
                                <asp:TextBox ID="tbEditAmountKrs" Width="18px" runat="server" onkeydown="return onlyNumber(event);"
                                             CssClass="deptAmount" MaxLength="2" Text="00" />
                                <asp:Literal runat="server" ID="ltEditAmountSymbolR" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbEditAmount"
                                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g2" ErrorMessage="!" SetFocusOnError="True" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbEditAmountKrs"
                                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g2" ErrorMessage="!" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Tekil
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="cbEditUniqe" Enabled="False" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button runat="server" ID="btUpdateDues" ValidationGroup="g2" OnClick="BtUpdateDuesTypes_OnClick"
                                            Width="50px" CssClass="SaveCancelBtn" />
                                <asp:Button runat="server" ID="btDeleteDues" ValidationGroup="gDelete" OnClick="BtDeleteDuesTypes_OnClick"
                                            Width="50px" CssClass="SaveCancelBtn" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <asp:RequiredFieldValidator ID="rqValListBoxEdit" runat="server" InitialValue=""
                                ControlToValidate="lbDues" ForeColor="Red" ValidationGroup="g2" />
    <br />
    <asp:RequiredFieldValidator ID="rqValListBoxDelete" runat="server" InitialValue=""
                                ControlToValidate="lbDues" ForeColor="Red" ValidationGroup="gDelete" />
    <asp:EntityDataSource ID="edsDues" runat="server" ConnectionString="name=Entities"
                          DefaultContainerName="Entities" EntitySetName="DuesTypes" OrderBy="it.Title">
    </asp:EntityDataSource>
    <asp:HiddenField runat="server" ID="hfSelectedDues" />
    <asp:HiddenField runat="server" ID="hfSelectedUser" />
</fieldset>