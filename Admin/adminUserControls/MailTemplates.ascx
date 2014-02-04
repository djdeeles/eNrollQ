<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailTemplates.ascx.cs"
            Inherits="eNroll.Admin.adminUserControls.MailTemplates" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="uc1" TagName="rtb" Src="~/Admin/adminUserControls/Rtb.ascx" %>
<asp:MultiView runat="server" ID="mvAuthoriztn">
    <asp:View runat="server" ID="vAuth">
        <asp:MultiView ID="mvMailTemplates" runat="server" >
            <asp:View ID="vEditTemplate" runat="server">
                <table>
                    <tr>
                        <td style="width: 100px;">
                            <%= AdminResource.lbTemplateName %>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbTemplateTitle"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbTemplateTitle"
                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbContent %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <uc1:rtb runat="server" ID="RtbTemplateContent" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btSaveTemplate" CssClass="SaveCancelBtn" ValidationGroup="g1"
                                        OnClick="BtSaveTemplateClick" />
                            <asp:Button runat="server" ID="btCancelSaveTemplate" CssClass="SaveCancelBtn" OnClick="BtCancelSaveTemplateClick" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Literal runat="server" ID="ltErrorMessage"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vGridTemplate" runat="server">
                <asp:Button ID="btNewTemplate" runat="server" CssClass="NewBtn" OnClick="ImageButtonbtNewTemplate_Click" />
                <asp:GridView ID="gVMailTemplates" runat="server" DataSourceID="EntityDataSource1"
                              AutoGenerateColumns="False" CssClass="GridViewStyle" PagerStyle-CssClass="pgr"
                              OnRowDataBound="gVMailTemplates_OnRowDataBound" AlternatingRowStyle-CssClass="alt"
                              SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                              SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                              EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                              DataKeyNames="Id" CellPadding="4" Width="100%" PageSize="15" ForeColor="#333333"
                              GridLines="None" AllowSorting="False">
                    <EmptyDataTemplate>
                        <%= AdminResource.lbNoRecord %>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                             OnClick="ImgBtnMemberEditClick" CommandArgument='<%#Bind
                                                                                                   ("Id") %>' />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgBtnMemberDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                             CommandArgument='<%#Bind("Id") %>' OnClick="ImgBtnMemberDeleteClick" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Title" SortExpression="Title" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UpdatedTime" SortExpression="UpdatedTime" HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" Width="130px" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="MailTemplates"
                                      OrderBy="it.Title">
                </asp:EntityDataSource>
            </asp:View>
        </asp:MultiView>
        <div>
            <asp:HiddenField runat="server" ID="hfMailTemplate" />
        </div>
    </asp:View>
    <asp:View runat="server" ID="vNoAuth">
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