<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="UserControls_DynamicDataList" Codebehind="DynamicContent.ascx.cs" %>
<table style="width: 100%;">
    <tr>
        <td valign="top" width="200px">
            <asp:DataList ID="DataList1" runat="server" DataKeyField="dynamicId" DataSourceID="EntityDataSource1"
                          OnItemDataBound="DataList1_ItemDataBound">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <img height="10px" src="/App_Themes/MainTheme/images/ok.png" width="8px" />
                            </td>
                            <td>
                                <asp:HyperLink ToolTip='<%#                                        Eval("dynamicId") %>' ID="hlMenu" runat="server" Text='<%#Eval
                                                                                                   ("name") %>'
                                    ></asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                  DefaultContainerName="Entities" EntitySetName="Customer_Dynamic"
                                  Where="it.Customer_Dynamic_Group.groupId=@paramGroupId" OrderBy="it.name">
                <WhereParameters>
                    <asp:ControlParameter ControlID="hdnGroupId" DbType="Decimal" Name="paramGroupId"
                                          PropertyName="Value" />
                </WhereParameters>
            </asp:EntityDataSource>
        </td>
        <td valign="middle" width="6px">
            &nbsp;
        </td>
        <td valign="top">
            <asp:Label ID="lblDetails" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnGroupId" runat="server" />