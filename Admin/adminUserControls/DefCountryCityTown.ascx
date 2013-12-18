<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefCountryCityTown.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.DefCountryCityTown" %>
<%@ Import Namespace="Resources" %>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%=AdminResource.lbCountry%></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbCountry" DataSourceID="edsCountry" DataValueField="Id"
            DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single" AutoPostBack="True"
            Enabled="True" OnSelectedIndexChanged="lbCountry_OnSelectedIndexChanged" />
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
                <asp:TextBox ID="tbNewCountryName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewCountryName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgCountry1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveCountry" ValidationGroup="vgCountry1" OnClick="BtSaveCountry_OnClick"
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
                <asp:TextBox ID="tbEditCountryName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbEditCountryName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgCountry2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateCountry" ValidationGroup="vgCountry2" OnClick="BtUpdateCountry_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btnDeleteCountry" ValidationGroup="vgCountry3" OnClick="BtDeleteCountry_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rValCountryEdit" runat="server" InitialValue=""
        ControlToValidate="lbCountry" ForeColor="Red" ValidationGroup="vgCountry2"> 
    </asp:RequiredFieldValidator><br/>
    <asp:RequiredFieldValidator ID="rValCountryDelete" runat="server" InitialValue=""
        ControlToValidate="lbCountry" ForeColor="Red" ValidationGroup="vgCountry3"> 
    </asp:RequiredFieldValidator>
    <asp:EntityDataSource ID="edsCountry" runat="server" ConnectionString="name=Entities"
        DefaultContainerName="Entities" EntitySetName="Countries" OrderBy="it.Name">
    </asp:EntityDataSource>
</fieldset>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%=AdminResource.lbCity%></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbCity" DataSourceID="edsCity" DataValueField="Id"
            DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single" AutoPostBack="True"
            Enabled="True" OnSelectedIndexChanged="lbCity_OnSelectedIndexChanged" />
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
                <asp:TextBox ID="tbNewCityName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbNewCityName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgCity1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveCity" ValidationGroup="vgCity1" OnClick="BtSaveCity_OnClick"
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
                <asp:TextBox ID="tbEditCityName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbEditCityName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgCity2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateCity" ValidationGroup="vgCity2" OnClick="BtUpdateCity_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btnDeleteCity" ValidationGroup="vgCity3" OnClick="BtDeleteCity_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rValCityEdit" runat="server" InitialValue=""
        ControlToValidate="lbCity" ForeColor="Red" ValidationGroup="vgCity2"> 
    </asp:RequiredFieldValidator><br/>
     <asp:RequiredFieldValidator ID="rValCityDelete" runat="server" InitialValue=""
        ControlToValidate="lbCity" ForeColor="Red" ValidationGroup="vgCity3"> 
    </asp:RequiredFieldValidator>
    <asp:EntityDataSource ID="edsCity" runat="server" ConnectionString="name=Entities"
        DefaultContainerName="Entities" EntitySetName="Cities" OrderBy="it.Name" Where="it.CountryId=@countryId">
        <WhereParameters>
            <asp:Parameter Name="countryId" DbType="Int32" DefaultValue="0" />
        </WhereParameters>
    </asp:EntityDataSource>
</fieldset>
<fieldset style="margin: 10px 0 0 0;">
    <legend>
        <%=AdminResource.lbTown%></legend>
    <div style="float: left; margin-right: 15px; color: #ccc;">
        <asp:ListBox runat="server" ID="lbTown" DataSourceID="edsTown" DataValueField="Id"
            DataTextField="Name" Width="300px" Height="150px" SelectionMode="Single" AutoPostBack="True"
            Enabled="True" OnSelectedIndexChanged="lbTown_OnSelectedIndexChanged" />
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
                <asp:TextBox ID="tbNewTownName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbNewTownName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgTown1">!</asp:RequiredFieldValidator>
            </td>
            <td width="130px">
                <asp:Button runat="server" ID="btSaveTown" ValidationGroup="vgTown1" OnClick="BtSaveTown_OnClick"
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
                <asp:TextBox ID="tbEditTownName" Width="170px" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbEditTownName"
                    ForeColor="Red" Display="Dynamic" ValidationGroup="vgTown2">!</asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button runat="server" ID="btUpdateTown" ValidationGroup="vgTown2" OnClick="BtUpdateTown_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
                <asp:Button runat="server" ID="btnDeleteTown" ValidationGroup="vgTown3" OnClick="BtDeleteTown_OnClick"
                    Width="60px" CssClass="SaveCancelBtn" />
            </td>
        </tr>
    </table>
    <asp:RequiredFieldValidator ID="rValTownEdit" runat="server" InitialValue=""
        ControlToValidate="lbTown" ForeColor="Red" ValidationGroup="vgTown2"> 
    </asp:RequiredFieldValidator><br/>
    <asp:RequiredFieldValidator ID="rValTownDelete" runat="server" InitialValue=""
        ControlToValidate="lbTown" ForeColor="Red" ValidationGroup="vgTown3"> 
    </asp:RequiredFieldValidator>
    <asp:EntityDataSource ID="edsTown" runat="server" ConnectionString="name=Entities"
        DefaultContainerName="Entities" EntitySetName="Towns" OrderBy="it.Name" Where="it.CityId=@cityId">
        <WhereParameters>
            <asp:Parameter Name="cityId" DbType="Int32" DefaultValue="0" />
        </WhereParameters>
    </asp:EntityDataSource>
</fieldset>
<asp:HiddenField runat="server" ID="hfSelectedCountry" />
<asp:HiddenField runat="server" ID="hfSelectedCity" />
<asp:HiddenField runat="server" ID="hfSelectedTown" />
