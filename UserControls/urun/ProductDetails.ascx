<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ProductDetails"
            ViewStateMode="Enabled" CodeBehind="ProductDetails.ascx.cs" %>
<script type="text/javascript">
    function ResimDegistir(Resim) {
        document.getElementById('<%= Image1.ClientID %>').src = "../../" + Resim;
    }
</script>
<h1>
    <asp:Label ID="ltName" runat="server"></asp:Label></h1>
<div class="productdetail">
    <asp:Panel ID="PanelResimDiv" runat="server">
        <div id="preview" class="productmainimage">
            <asp:Image ID="Image1" runat="server" Width="270" />
        </div>
        <asp:DataList ID="DataList1" runat="server" RepeatColumns="2">
            <ItemTemplate>
                <div onclick=" ResimDegistir('<%#                                        (Eval("ProductImage") != null
                                             ? Eval("ProductImage").ToString().Replace("~", "")
                                             : "") %>') "
                     style="height: 100px; overflow: hidden; width: 134px;">
                    <img src='<%#                                        (Eval("Thumbnail") != null
                                             ? Eval("ProductImage").ToString().Replace("~", "")
                                             : "") %>'
                         width="134" style="padding: 3px;" />
                </div>
            </ItemTemplate>
        </asp:DataList>
    </asp:Panel>
    <div style="margin-top: 15px;">
        <asp:Label ID="Label1" runat="server" Font-Bold="False"></asp:Label>
        <asp:Panel ID="Panel3" runat="server" Width="270px" Wrap="False">
            <asp:Literal runat="server" ID="DigerUrunLink"></asp:Literal>
        </asp:Panel>
    </div>
</div>
<asp:Literal ID="ltDesc" runat="server"></asp:Literal>
<asp:Literal ID="ltPrice" runat="server"></asp:Literal>
<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=EnrollEntities"
                      DefaultContainerName="EnrollEntities" EntitySetName="Product_Images" Where="it.ProductId=@productId">
    <WhereParameters>
        <asp:QueryStringParameter DbType="Int32" Name="ProductId" QueryStringField="id" />
    </WhereParameters>
</asp:EntityDataSource>