<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefServiceType.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.DefServiceType" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%=AdminResource.lbServiceType%></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbServiceType" DataSourceID="edsServiceType"
            DataValueField="Id" DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single"
            AutoPostBack="True" Enabled="True" OnSelectedIndexChanged="lbServiceType_OnSelectedIndexChanged" />
    </div>
    <table style="float: left; width: 410px;">
        <tr>
            <td align="left" colspan="4"><%=AdminResource.lbNew %></td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td width="50px">
                <%=AdminResource.lbName %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="155px">
                <asp:TextBox ID="tbServiceType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbServiceType"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="g1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
            </td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td width="50px">
                <%=AdminResource.lbDesc %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="155px">
                <asp:TextBox ID="tbServiceDesc" Width="140px" runat="server" /> 
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveServiceType" ValidationGroup="g1" OnClick="BtSaveServiceType_OnClick"
                    Width="50px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
        <tr>
            <td colspan="4">&nbsp; </td>
        </tr>
        <tr>
            <td align="left" colspan="4"><%=AdminResource.lbEdit %></td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td>
                <%=AdminResource.lbName %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="tbEditServiceType" Width="140px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditServiceType"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="g2">!</asp:RequiredFieldValidator>
            </td>
            <td> 
            </td>
        </tr>
        <tr>
            <td width="50px"></td>
            <td>
                <%=AdminResource.lbDesc %>
            </td>
            <td>
                :
            </td>
            <td>
                <asp:TextBox ID="tbEditServiceDesc" Width="140px" runat="server" /> 
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateServiceType" ValidationGroup="g2" OnClick="BtUpdateServiceType_OnClick"
                    Width="50px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btDeleteServiceType" ValidationGroup="g3" OnClick="BtDeleteServiceType_OnClick" Width="50px"
                    CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rqValListBoxEdit" runat="server" InitialValue=""
                                ControlToValidate="lbServiceType" ForeColor="Red"
                                ValidationGroup="g2"/> <br/>
    <asp:RequiredFieldValidator ID="rqValListBoxDelete" runat="server" InitialValue=""
                                ControlToValidate="lbServiceType" ForeColor="Red"
                                ValidationGroup="g3"/> 
    
    <asp:EntityDataSource ID="edsServiceType" runat="server" ConnectionString="name=Entities"
        DefaultContainerName="Entities" EntitySetName="ServiceTypes" OrderBy="it.Name">
    </asp:EntityDataSource>
    <asp:HiddenField runat="server" ID="hfSelectedServiceType" />
</fieldset>
