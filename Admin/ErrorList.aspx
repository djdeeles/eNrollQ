<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/MasterPage.master"
         Inherits="Admin_ErrorList" CodeBehind="ErrorList.aspx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"
             class="rightcontenttable">
    <style type="text/css">
        .GridViewStyle tr td { word-break: break-all; }

        .button { margin: 5px 0; }
    </style>
    <asp:MultiView runat="server" ID="mvAuth">
        <asp:View ID="View1" runat="server">
            <table border="0" cellpadding="3" cellspacing="0" class="rightcontenttable">
                <tr>
                    <td valign="top" align="left">
                        <asp:Button runat="server" ID="btDeleteErrors" OnClick="btDeleteErrors_Click" CssClass="SaveCancelBtn" />
                    </td>
                    <td valign="top" align="right">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtAra" runat="server" Height="18px" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="ImageButton2Ara" runat="server" ImageUrl="~/Admin/images/icon/ara.png"
                                                     OnClick="ImageButton2Ara_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                      PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                                      SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                                      SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                      SelectedRowStyle="selected" DataKeyNames="id" DataSourceID="EntityDataSource1"
                                      AllowPaging="True" PageSize="15" AllowSorting="True" CellPadding="4" Width="100%"
                                      ForeColor="#333333" GridLines="None">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField DataField="id" SortExpression="id">
                                    <HeaderStyle HorizontalAlign="Left" Width="20px" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ErrorMessage" SortExpression="ErrorMessage">
                                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StackTrace" SortExpression="StackTrace" DataFormatString="{0:d}"
                                                HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Url" SortExpression="Url"  
                                                HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Left" Width="110px" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Date" SortExpression="Date" HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Ip" SortExpression="Ip" HtmlEncode="False">
                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                              DefaultContainerName="Entities" EntitySetName="System_Errors" OrderBy="it.id desc">
                        </asp:EntityDataSource>
                    </td>
                </tr>
            </table>
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