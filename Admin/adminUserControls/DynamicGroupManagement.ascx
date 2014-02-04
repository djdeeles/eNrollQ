<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_DynamicListManagement"
            CodeBehind="DynamicGroupManagement.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <asp:Panel runat="server" DefaultButton="btSave">
            <table>
                <tr>
                    <td>
                        <%= AdminResource.lbName %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDynaDisplay" runat="server" Width="135px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDynaDisplay"
                                                    ForeColor="Red" ValidationGroup="vldGroup1">!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbType %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDynaDisType" runat="server" Width="140px">
                            <asp:ListItem Value="1">Combo</asp:ListItem>
                            <asp:ListItem Value="2">Grid</asp:ListItem>
                        </asp:DropDownList>
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
                        <asp:CheckBox ID="chkDyna" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbList %>
                    </td>
                    <td valign="top">
                        :
                    </td>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnAdd">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlDynamicGroup" runat="server" AutoPostBack="True" DataSourceID="EntityDynaGroup"
                                                          DataTextField="name" DataValueField="groupId" Width="180px" OnDataBound="ddlDynamicGroup_DataBound"
                                                          OnSelectedIndexChanged="ddlDynamicGroup_SelectedIndexChanged1">
                                        </asp:DropDownList>
                                        <asp:EntityDataSource ID="EntityDynaGroup" runat="server" ConnectionString="name=Entities"
                                                              DefaultContainerName="Entities" EntitySetName="Customer_Dynamic_Group">
                                        </asp:EntityDataSource>
                                        <br />
                                        <asp:ListBox ID="ListBoxDynaSource" runat="server" Width="180px" Height="150px" />
                                        <asp:EntityDataSource ID="EntityDynaData" runat="server" ConnectionString="name=Entities"
                                                              DefaultContainerName="Entities" EntitySetName="Customer_Dynamic">
                                        </asp:EntityDataSource>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td valign="top">
                                        <asp:ListBox ID="ListBoxRef" runat="server" Height="175px" Width="180px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                                                    OnClick="btnAdd_Click" />
                                        <asp:Button ID="btnRemove" runat="server" CssClass="SaveCancelBtn" OnClick="btnRemove_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btSave" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                                    OnClick="btSave_Click" />
                        <asp:Button ID="btCancel" runat="server" CssClass="SaveCancelBtn" OnClientClick=" window.close(); " />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
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
<asp:HiddenField ID="hdnMenuId" runat="server" />