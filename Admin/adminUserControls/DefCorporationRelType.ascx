<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefCorporationRelType.ascx.cs"
            Inherits="eNroll.Admin.adminUserControls.DefCorporationRelType" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%= AdminResource.lbCorporationRelTypes %></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbCorporationRelationType" DataSourceID="edsCorporationRelationType"
                     DataValueField="Id" DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single"
                     AutoPostBack="True" Enabled="True" OnSelectedIndexChanged="lbCorporationRelationType_OnSelectedIndexChanged" />
    </div>
    <table style="float: left; width: 410px;">
        <tr>
            <td width="50px">
                <%= AdminResource.lbNew %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="155px">
                <asp:TextBox ID="tbNewRelType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewRelType"
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g1">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:CheckBox ID="cbNewRelType" runat="server" />
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveRelType" ValidationGroup="g1" OnClick="BtSaveRelType_OnClick"
                            Width="50px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
        <tr>
            <td>
                <%= AdminResource.lbEdit %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="tbEditRelType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditRelType"
                                            ForeColor="Red" Display="Dynamic" ValidationGroup="g2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:CheckBox ID="cbEditRelType" runat="server" />
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateRelType" ValidationGroup="g2" OnClick="BtUpdateRelType_OnClick"
                            Width="50px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btnDelete" ValidationGroup="g2" OnClick="BtDeleteType_OnClick" Width="50px"
                            CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rqValidaterListBox" runat="server" InitialValue=""
                                ControlToValidate="lbCorporationRelationType" ForeColor="Red"
                                ValidationGroup="g2"> 
    </asp:RequiredFieldValidator>
    <asp:EntityDataSource ID="edsCorporationRelationType" runat="server" ConnectionString="name=Entities"
                          DefaultContainerName="Entities" EntitySetName="FoundationRelType" OrderBy="it.UpdatedTime desc">
    </asp:EntityDataSource>
    <asp:HiddenField runat="server" ID="hfSelectedRelType" />
</fieldset>