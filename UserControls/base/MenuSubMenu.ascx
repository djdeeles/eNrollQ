<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_MenuSubMenu" Codebehind="MenuSubMenu.ascx.cs" %>
<asp:DataList ID="DataList1" runat="server" CellPadding="0" OnItemDataBound="DataList1_ItemDataBound" RepeatColumns="3" Width="100%">
    <ItemTemplate>
        <h1>
            <asp:Label ID="lblMenuName" runat="server" Text='<%#                                        Bind("name") %>'></asp:Label>
        </h1>
        <asp:Literal ID="Literal1" runat="server" OnDataBinding="Literal1_DataBinding" Text='<%#Bind
                                                                                                   ("masterImage") %>'></asp:Literal>
        <asp:Label ID="lblMenuBrief" runat="server" Text='<%#Bind("brief") %>' OnDataBinding="lblMenuBrief_DataBinding"></asp:Label><br />
        <asp:HyperLink ID="HyperLink1" runat="server">[HyperLink1]</asp:HyperLink>
        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Bind("menuId") %>' />
    </ItemTemplate>
    <ItemStyle VerticalAlign="Top" />
</asp:DataList>