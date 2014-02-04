<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.master" AutoEventWireup="true"
         CodeBehind="Logs.aspx.cs" Inherits="eNroll.Admin.Logs" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="eNroll.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"
             class="rightcontenttable">
    <asp:MultiView runat="server" ID="mvAuth">
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                          CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                          DataKeyNames="id" DataSourceID="EntityDataSource1" AllowPaging="True" PageSize="30"
                          AllowSorting="True" CellPadding="4" Width="100%" ForeColor="#333333" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Id" SortExpression="id">
                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                        <ItemStyle VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="operation">
                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <%#                                        Logger.GetOperation(Convert.ToInt32(Eval("operation"))) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="moduleId">
                        <HeaderStyle HorizontalAlign="Left" Width="210px" />
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <%#                                        Logger.GetModul(Convert.ToInt32(Eval("moduleId")),
                                                        Convert.ToInt32(Eval("moduleContentId")), Eval("moduleSubId")).
                                            ModulName %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="moduleSubId">
                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <a target="_blank" <%#                                        (Convert.ToInt32(Eval("moduleContentId")) == 0 ? "style='display:none;'" : "") %>
                               href='<%#                                        Logger.GetModul(Convert.ToInt32(Eval("moduleId")),
                                                        Convert.ToInt32(Eval("moduleContentId")),
                                                        Eval("moduleSubId")).ModulContentUrl %> '>
                                <%#Eval("moduleContentId") %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="userId">
                        <HeaderStyle HorizontalAlign="Left" Width="160px" />
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <%#                                        Logger.GetUserEmail(Convert.ToInt32(Eval("userId"))) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ip" SortExpression="Ip">
                        <HeaderStyle HorizontalAlign="Left" Width="70px" />
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreatedTime" SortExpression="CreatedTime">
                        <HeaderStyle HorizontalAlign="Left" Width="120px" />
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                </Columns>
                <%-- <PagerStyle CssClass="PagerStyle" BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle CssClass="SelectedRowStyle" BackColor="#E2DED6" Font-Bold="True"
                    ForeColor="#333333" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle CssClass="HeaderStyle" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle CssClass="EditRowStyle" BackColor="#999999" />
                <AlternatingRowStyle CssClass="AltRowStyle" BackColor="White" ForeColor="#284775" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />--%>
            </asp:GridView>
            <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                  DefaultContainerName="Entities" EntitySetName="System_Logs" OrderBy="it.CreatedTime desc">
            </asp:EntityDataSource>
        </asp:View>
        <asp:View ID="View2" runat="server">
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
</asp:Content>