<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefTax.ascx.cs"
            Inherits="eNroll.Admin.adminUserControls.DefTax" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%= AdminResource.lbTaxType %></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbTaxType" DataSourceID="edsTaxType"
                     DataValueField="Id" DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single"
                     AutoPostBack="True" Enabled="True" OnSelectedIndexChanged="lbTaxType_OnSelectedIndexChanged" />
    </div>
    <table style="float: left; width: 410px;">
        <tr>
            <td align="left" colspan="4"><%= AdminResource.lbNew %></td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td width="50px">
                <%= AdminResource.lbName %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="155px">
                <asp:TextBox ID="tbTaxType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbTaxType"
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px"> 
            </td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td width="50px">
                <%= AdminResource.lbValue %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="155px">
                <asp:TextBox ID="tbTaxValue" Width="140px" runat="server" onkeydown="return priceInputsCharacters(event,this);"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbTaxValue"
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveTaxType" ValidationGroup="g1" OnClick="BtSaveTaxType_OnClick"
                            Width="60px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
        <tr>
            <td colspan="4">&nbsp;</td>
        </tr>
        <tr>
            <td align="left" colspan="4"><%= AdminResource.lbEdit %></td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td>
                <%= AdminResource.lbName %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="tbEditTaxType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbEditTaxType"
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g2">!</asp:RequiredFieldValidator>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td>
                <%= AdminResource.lbValue %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="tbEditTaxValue" Width="140px" runat="server" onkeydown="return priceInputsCharacters(event,this);"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbEditTaxValue"
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateTaxType" ValidationGroup="g2" OnClick="BtUpdateTaxType_OnClick"
                            Width="60px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btDeleteTaxType" ValidationGroup="g3" OnClick="BtDeleteTaxType_OnClick" Width="60px"
                            CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rqValListBoxEdit" runat="server" InitialValue=""
                                ControlToValidate="lbTaxType" ForeColor="Red"
                                ValidationGroup="g2"/> <br/>
    <asp:RequiredFieldValidator ID="rqValListBoxDelete" runat="server" InitialValue=""
                                ControlToValidate="lbTaxType" ForeColor="Red"
                                ValidationGroup="g3"/> 
    
    <asp:EntityDataSource ID="edsTaxType" runat="server" ConnectionString="name=Entities"
                          DefaultContainerName="Entities" EntitySetName="TaxTypes" OrderBy="it.Name">
    </asp:EntityDataSource>
    <asp:HiddenField runat="server" ID="hfSelectedTaxType" />
</fieldset>