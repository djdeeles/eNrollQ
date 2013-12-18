<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefBloodType.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.DefBloodType" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%=AdminResource.lbBloodType%></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbBloodType" DataSourceID="edsBloodType"
            DataValueField="Id" DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single"
            AutoPostBack="True" Enabled="True" OnSelectedIndexChanged="lbBloodType_OnSelectedIndexChanged" />
    </div>
    <table style="float: left; width: 410px;">
        <tr>
            <td width="50px">
                <%=AdminResource.lbNew %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="155px">
                <asp:TextBox ID="tbNewBloodType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewBloodType"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="g1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveBloodType" ValidationGroup="g1" OnClick="BtSaveBloodType_OnClick"
                    Width="50px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
        <tr>
            <td>
                <%=AdminResource.lbEdit %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="tbEditBloodType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditBloodType"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="g2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateBloodType" ValidationGroup="g2" OnClick="BtUpdateBloodType_OnClick"
                    Width="50px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btDeleteBloodType" ValidationGroup="g3" OnClick="BtDeleteBloodType_OnClick" Width="50px"
                    CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rqValListBoxEdit" runat="server" InitialValue=""
                                ControlToValidate="lbBloodType" ForeColor="Red"
                                ValidationGroup="g2"/> <br/>
    <asp:RequiredFieldValidator ID="rqValListBoxDelete" runat="server" InitialValue=""
                                ControlToValidate="lbBloodType" ForeColor="Red"
                                ValidationGroup="g3"/> 
    
    <asp:EntityDataSource ID="edsBloodType" runat="server" ConnectionString="name=Entities"
        DefaultContainerName="Entities" EntitySetName="BloodTypes" OrderBy="it.Name">
    </asp:EntityDataSource>
    <asp:HiddenField runat="server" ID="hfSelectedBloodType" />
</fieldset>
