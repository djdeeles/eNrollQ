<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefSectorJob.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.DefSectorJob" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%=AdminResource.lbJobSector%></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbSector" DataSourceID="edsSector" DataValueField="Id"
            DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single" AutoPostBack="True"
            Enabled="True" OnSelectedIndexChanged="lbSector_OnSelectedIndexChanged" />
    </div>
    <table style="float: left; width: 410px;">
        <tr>
            <td width="50px">
                <%=AdminResource.lbNew %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="175px">
                <asp:TextBox ID="tbNewSectorName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewSectorName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgSector1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveSector" ValidationGroup="vgSector1" OnClick="BtSaveSector_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
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
                <asp:TextBox ID="tbEditSectorName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditSectorName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgSector2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateSector" ValidationGroup="vgSector2" OnClick="BtUpdateSector_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btnDeleteSector" ValidationGroup="vgSector3" OnClick="BtDeleteSector_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rValSectorEdit" runat="server" InitialValue=""
        ControlToValidate="lbSector" ForeColor="Red" ValidationGroup="vgSector2"> 
    </asp:RequiredFieldValidator><br/>
    <asp:RequiredFieldValidator ID="rValSectorDelete" runat="server" InitialValue=""
        ControlToValidate="lbSector" ForeColor="Red" ValidationGroup="vgSector3"> 
    </asp:RequiredFieldValidator>
    <asp:EntityDataSource ID="edsSector" runat="server" ConnectionString="name=Entities"
        DefaultContainerName="Entities" EntitySetName="JobSectors" OrderBy="it.Name">
    </asp:EntityDataSource>
</fieldset>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%=AdminResource.lbJob%></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbJob" DataSourceID="edsJob" DataValueField="Id"
            DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single" AutoPostBack="True"
            Enabled="True" OnSelectedIndexChanged="lbJob_OnSelectedIndexChanged" />
    </div>
    <table style="float: left; width: 410px;">
        <tr>
            <td width="50px">
                <%=AdminResource.lbNew %>
            </td>
            <td width="10px">
                :
            </td>
            <td width="175px">
                <asp:TextBox ID="tbNewJobName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbNewJobName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgJob1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveJob" ValidationGroup="vgJob1" OnClick="BtSaveJob_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
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
                <asp:TextBox ID="tbEditJobName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbEditJobName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgJob2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateJob" ValidationGroup="vgJob2" OnClick="BtUpdateJob_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btnDeleteJob" ValidationGroup="vgJob3" OnClick="BtDeleteJob_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rValJobEdit" runat="server" InitialValue=""
        ControlToValidate="lbJob" ForeColor="Red" ValidationGroup="vgJob2"> 
    </asp:RequiredFieldValidator><br/>
     <asp:RequiredFieldValidator ID="rValJobDelete" runat="server" InitialValue=""
        ControlToValidate="lbJob" ForeColor="Red" ValidationGroup="vgJob3"> 
    </asp:RequiredFieldValidator>
    <asp:EntityDataSource ID="edsJob" runat="server" ConnectionString="name=Entities"
        DefaultContainerName="Entities" EntitySetName="Jobs" OrderBy="it.Name">
    </asp:EntityDataSource>
</fieldset>
<asp:HiddenField runat="server" ID="hfSelectedSector" />
<asp:HiddenField runat="server" ID="hfSelectedJob" />