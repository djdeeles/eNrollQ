<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RssManagement.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.RssManagement" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td colspan="3">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <asp:Button ID="btnAddNew" runat="server" CssClass="NewBtn" OnClick="btnAddNew_Click" />
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <table>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblUyariBos" runat="server" Style="color: #FF0000" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbName %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRss" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbRssUrl %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRssUrl" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbMaxItem %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxItem" onkeydown="return onlyNumber(event);" runat="server"
                                            Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbScrollRssNames %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbScrollRssNames" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
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
                                        <asp:CheckBox ID="cbRssState" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="btnSave_Click"
                                            ValidationGroup="vg1" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancel_Click" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="View3" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnEditSave">
                                <table>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbName %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRssEdit" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbRssUrl %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRssUrlEdit" runat="server" Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbMaxItem %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxItemEdit" onkeydown="return onlyNumber(event);" runat="server"
                                            Width="150px"></asp:TextBox>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%= AdminResource.lbScrollRssNames %>
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbScrollRssNamesEdit" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
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
                                        <asp:CheckBox ID="cbRssStateEdit" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:Button ID="btnEditSave" runat="server" CssClass="SaveCancelBtn" OnClick="btnEditSave_Click"
                                            ValidationGroup="valGrupEdit" />
                                        <asp:Button ID="btnEditCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnEditCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                            <asp:HiddenField ID="HiddenFieldBannerId" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvRss" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
            SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
            SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
            EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
            Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="gvRss_RowCommand"
            AllowSorting="True" OnRowDataBound="gvRss_RowDataBound" PageSize="15" AllowPaging="True">
            <EmptyDataTemplate>
                <%= AdminResource.lbNoRecord %>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="LinkButtonGuncelle" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                            CommandArgument='<%#Eval
                                                                                                   ("Id") %>'
                            CommandName="Guncelle" />
                        <asp:ImageButton ID="LinkButtonSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                            CommandArgument='<%#Eval("Id") %>' CommandName="Sil" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                </asp:TemplateField>
                <asp:BoundField DataField="Name" SortExpression="Name">
                    <HeaderStyle HorizontalAlign="Left" Width="150px" />
                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Url" SortExpression="Url">
                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="MaxItem" SortExpression="MaxItem">
                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                </asp:BoundField>
                <asp:CheckBoxField DataField="IsScroll" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                    SortExpression="IsScroll">
                    <HeaderStyle HorizontalAlign="Left" Width="55px" />
                    <ItemStyle HorizontalAlign="Left" Width="55px" />
                </asp:CheckBoxField>
                <asp:BoundField DataField="CreatedTime" SortExpression="CreatedTime">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Left"  Width="80px"/>
                </asp:BoundField>
                <asp:CheckBoxField DataField="State" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                    SortExpression="State">
                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                </asp:CheckBoxField>
            </Columns>
        </asp:GridView>
        <asp:EntityDataSource ID="EntityDataSourceRssManagement" runat="server" ConnectionString="name=Entities"
            DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Rss"
            EntityTypeFilter="" Select="" EnableDelete="True" EnableUpdate="True" Where="it.Language=@languageId">
            <WhereParameters>
                <asp:ControlParameter ControlID="hfLanguageId" Name="languageId" PropertyName="Value"
                    DbType="Int32" />
            </WhereParameters>
        </asp:EntityDataSource>
        <asp:HiddenField ID="hfLanguageId" runat="server" />
    </asp:View>
    <asp:View ID="noAuth" runat="server">
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