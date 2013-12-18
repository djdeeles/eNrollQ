﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_MenuDynamicList"
            CodeBehind="MenuDynamicList.ascx.cs" %>
<table cellspacing="0" cellpadding="0" border="0" width="100%">
    <tr>
        <td>
            <h2>
                <%= Title %></h2>
        </td>
    </tr>
    <tr>
        <td>
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="View1" runat="server">
                    <asp:DropDownList ID="ddlDynamicList" runat="server" OnSelectedIndexChanged="ddlDynamicList_SelectedIndexChanged"
                                      AutoPostBack="True">
                    </asp:DropDownList>
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <asp:DataList ID="DataListMenuDynamic" runat="server" OnItemDataBound="DataListMenuDynamic_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%#                                        Bind("Id") %>'
                                            Text='<%#Bind
                                                                                                   ("Name") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:DataList>
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
</table>