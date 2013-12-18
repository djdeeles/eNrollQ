<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_IpFilter"
    CodeBehind="IpFilter.ascx.cs" %>
<script type="text/javascript" src="http://ajax.microsoft.com/ajax/jQuery.Validate/1.11.1/jQuery.Validate.min.js"></script>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td colspan="3">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <table class="rightcontenttable">
                                <tr>
                                    <td style="width: 100px">
                                        <asp:Button ID="btnNewIpAddress" runat="server" CssClass="NewBtn" OnClick="BtnNewIpAddressClick"
                                            Height="32px" />
                                    </td>
                                    <td align="center">
                                        <table style="width: 330px; border: 1px solid #ccc; padding: 2px 7px 2px 7px;">
                                            <tr>
                                                <td>
                                                    <b>
                                                        <%=AdminResource.lbIpFilter %>&nbsp;:&nbsp;</b>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbIpFilterOnOff_Off" GroupName="rbIpFilterOnOff" AutoPostBack="True"
                                                        OnCheckedChanged="rbIpFilterOnOffChanged" runat="server" />
                                                <td>
                                                    <asp:RadioButton ID="rbIpFilterOnOff_BlackList" GroupName="rbIpFilterOnOff" AutoPostBack="True"
                                                        OnCheckedChanged="rbIpFilterOnOffChanged" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbIpFilterOnOff_WhiteList" GroupName="rbIpFilterOnOff" AutoPostBack="True"
                                                        OnCheckedChanged="rbIpFilterOnOffChanged" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="text-align: right; width: 60px;">
                                        <%= AdminResource.lbFilterType%>
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:DropDownList ID="ddlIpFilterType" runat="server" Width="100px" AutoPostBack="True"
                                            OnSelectedIndexChanged="DdlIpFilterTypeSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <asp:Panel runat="server" DefaultButton="BtnSaveUpdateIp">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 125px;">
                                            <%= AdminResource.lbBlackList %>&nbsp;/&nbsp;<%= AdminResource.lbWhiteList%>
                                        </td>
                                        <td width="10">
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlIpFilterType_New" runat="server" Width="250px" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbName%>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTitle" runat="server" Width="245px"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbIpAdress %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIpAddress" runat="server" Width="245px"></asp:TextBox>
                                            <%--<asp:RegularExpressionValidator ValidationExpression="
                                        \b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)
                                        \.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)
                                        \.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)
                                        \.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b" 
                                        ID="RegularExpressionValidator1" runat="server"  ForeColor="Red" ValidationGroup="g1" 
                                        ErrorMessage="!" ControlToValidate="txtIpAddress"/>--%>
                                            <asp:LinkButton ID="lbCopy" OnClick="lbCopyClick" runat="server"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtIpAddress"
                                                Display="Dynamic" ForeColor="Red" ValidationGroup="g1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbState" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnSaveUpdateIp" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveUpdateIp_Click"
                                                ValidationGroup="g1" />
                                            <asp:Button ID="BtnCancelIp" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelIp_Click" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvIpFilterList" DataSourceID="EntityDataSource1" runat="server"
            AutoGenerateColumns="False" CssClass="GridViewStyle" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
            SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
            SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
            SelectedRowStyle="selected" OnRowDataBound="gvIpFilterList_OnRowDataBound" Width="100%"
            CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="gvIpFilterList_RowCommand"
            PageSize="15" AllowPaging="True" AllowSorting="True">
            <EmptyDataTemplate>
                <%= AdminResource.lbNoRecord %>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="LinkButtonGuncelle" runat="server" CommandArgument='<%#Eval("Id") %>'
                            ImageUrl="~/Admin/images/icon/edit.png" CommandName="Guncelle" />
                        <asp:ImageButton ID="LinkButtonSil" runat="server" CommandArgument='<%#Eval ("Id") %>'
                            ImageUrl="~/Admin/images/icon/cop.png" CommandName="Sil" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                </asp:TemplateField>
                <asp:BoundField DataField="Title" SortExpression="Title">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IpAddress" SortExpression="IpAddress">
                    <HeaderStyle HorizontalAlign="Left" Width="125px" />
                    <ItemStyle HorizontalAlign="Left" Width="125px" />
                </asp:BoundField>
                <asp:BoundField DataField="CreatedTime" SortExpression="CreatedTime">
                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                </asp:BoundField>
                <asp:CheckBoxField DataField="State" SortExpression="State">
                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                </asp:CheckBoxField>
            </Columns>
        </asp:GridView>
        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
            DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="IpFilterList"
            EntityTypeFilter="" Select="" OrderBy="it.CreatedTime desc" Where="it.BlackList=@BlackList">
            <WhereParameters>
                <asp:ControlParameter ControlID="ddlIpFilterType" DbType="Boolean" Name="BlackList"
                    PropertyName="SelectedValue" />
            </WhereParameters>
        </asp:EntityDataSource>
    </asp:View>
    <asp:View runat="server">
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
<asp:HiddenField ID="hfIpAddress" runat="server" />
