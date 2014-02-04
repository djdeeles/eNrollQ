<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefCurrency.ascx.cs"
            Inherits="eNroll.Admin.adminUserControls.DefCurrency" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%= AdminResource.lbCurrency %></legend>
    <div style="float: left; margin-right: 15px;">
        <asp:ListBox runat="server" ID="lbCurrency" DataSourceID="edsCurrency" DataValueField="Id"
                     DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single" AutoPostBack="True"
                     Enabled="True" OnSelectedIndexChanged="lbCurrency_OnSelectedIndexChanged" />
    </div>
    <table style="float: left; width: 410px;">
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
                            <asp:TextBox ID="tbNewCurrency" Width="140px" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewCurrency"
                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g1" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbSymbol %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbNewSymbol" Width="140px" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbNewSymbol"
                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g1" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbSymbolPosition %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbNewDirection" runat="server" RepeatColumns="2" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="rbNewDirection"
                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g1" InitialValue="" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td><%= AdminResource.lbSiteMainCurrencyUnit %></td>
                        <td>:</td>
                        <td><asp:CheckBox runat="server" ID="cbSiteMainCurrencyUnit"/></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btSaveCurrency" ValidationGroup="g1" OnClick="BtSaveCurrency_OnClick"
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
                            <asp:TextBox ID="tbEditCurrency" Width="140px" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditCurrency"
                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g2" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbSymbol %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditSymbol" Width="140px" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbEditSymbol"
                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g2" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbSymbolPosition %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbEditDirection" runat="server" RepeatColumns="2" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="rbEditDirection"
                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g2" InitialValue="" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td><%= AdminResource.lbSiteMainCurrencyUnit %></td>
                        <td>:</td>
                        <td><asp:CheckBox runat="server" ID="cbEditSiteMainCurrencyUnit"/></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btUpdateCurrency" ValidationGroup="g2" OnClick="BtUpdateCurrency_OnClick"
                                        Width="50px" CssClass="SaveCancelBtn" />
                            <asp:Button runat="server" ID="btDeleteCurrency" ValidationGroup="g3" OnClick="BtDeleteCurrency_OnClick"
                                        Width="50px" CssClass="SaveCancelBtn" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rqValListBoxEdit" runat="server" InitialValue=""
                                ControlToValidate="lbCurrency" ForeColor="Red" ValidationGroup="g2" />
    <br />
    <asp:RequiredFieldValidator ID="rqValListBoxDelete" runat="server" InitialValue=""
                                ControlToValidate="lbCurrency" ForeColor="Red" ValidationGroup="g3" />
    <asp:EntityDataSource ID="edsCurrency" runat="server" ConnectionString="name=Entities"
                          DefaultContainerName="Entities" EntitySetName="Currency" OrderBy="it.Name">
    </asp:EntityDataSource>
    <asp:HiddenField runat="server" ID="hfSelectedCurrency" />
</fieldset>