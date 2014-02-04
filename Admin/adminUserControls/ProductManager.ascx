<%@ Control Language="C#" AutoEventWireup="true" Inherits="admin_adminUserControls_ProductManager"
            CodeBehind="ProductManager.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <asp:HiddenField runat="server" ID="HiddenField1" />
        <table class="rightcontenttable">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <b>
                                    <%= AdminResource.lbCategories %></b>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Panel runat="server" ID="pnlSelecetedCategory" Visible="False">
                                    <%= AdminResource.lbSelectedCategory %>&nbsp;:&nbsp;
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True"></asp:Label>&nbsp;-&nbsp;
                                    <asp:LinkButton ID="btEditCategory" runat="server" OnClick="btShowPnlEditDeleteCategory" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; width: 155px; background-color: #f6f6f6; border: 1px solid #E5E5E5;">
                                <asp:Panel ID="Panel1" runat="server">
                                    <telerik:RadTreeView ID="TreeView2" runat="server" OnNodeClick="TreeView2_SelectedNodeChanged">
                                    </telerik:RadTreeView>
                                </asp:Panel>
                            </td>
                            <td width="5px">
                            </td>
                            <td style="vertical-align: top;">
                                <asp:Panel ID="pnlAddCategory" runat="server" Visible="False" DefaultButton="btnNewFolder" >
                                    <table width="565px">
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbCategoryName %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNew" runat="server" Width="220px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNew"
                                                                            ForeColor="Red" ValidationGroup="vg1">!</asp:RequiredFieldValidator>
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
                                                <asp:CheckBox ID="cbActive" runat="server" Checked="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnNewFolder" runat="server" CssClass="SaveCancelBtn" OnClick="btnNewFolder_Click"
                                                            Style="height: 20px" ValidationGroup="vg1" />
                                                <asp:Button ID="btnAddNewCategoryCancel" runat="server" CssClass="SaveCancelBtn"
                                                            Style="height: 20px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </asp:Panel>
                                <asp:Panel ID="pnlEditDeleteCategory" runat="server" Visible="False" DefaultButton="btnButtonEditFolder">
                                    <table width="565px">
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbCategoryName %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEdit" runat="server" Width="220px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEdit"
                                                                            ForeColor="Red" ValidationGroup="vg2">!</asp:RequiredFieldValidator>
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
                                                <asp:CheckBox ID="cbEditActive" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnButtonEditFolder" runat="server" CssClass="SaveCancelBtn" OnClick="btnButtonEditFolder_Click"
                                                            ValidationGroup="vg2" />
                                                <asp:Button ID="btnDelete" runat="server" CssClass="SaveCancelBtn" OnClick="btnDelete_Click" />
                                                <asp:Button ID="btnAddEditCategoryCancel" runat="server" CssClass="SaveCancelBtn"
                                                            OnClick="BtnAddEditCategoryCancelClick" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </asp:Panel>
                                <asp:Button ID="btNewCategory" runat="server" CssClass="NewBtn" OnClick="btShowPnlAddCategory" />
                                <asp:Button ID="btnAddProduct" runat="server" CssClass="NewBtn" OnClick="btnAddProduct_Click" />
                                <asp:Panel ID="pnlAddProduct" runat="server" Visible="False" DefaultButton="btnSave">
                                    <table>
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbName %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNameInsert" runat="server" Width="400px"></asp:TextBox>
                                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNameInsert"
                                                                                  ForeColor="Red" SetFocusOnError="True" ValidationGroup="aa">!</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <%= AdminResource.lbSelectCategory %>
                                            </td>
                                            <td valign="top">
                                                :
                                            </td>
                                            <td>
                                                <asp:MultiView ID="MultiView2" runat="server">
                                                    <asp:View ID="View4" runat="server">
                                                        <telerik:RadTreeView runat="server" ID="TreeView3Insert" runat="server" OnNodeClick="TreeView3Insert_SelectedNodeChanged">
                                                        </telerik:RadTreeView>
                                                    </asp:View>
                                                    <asp:View ID="View5" runat="server">
                                                        <asp:LinkButton ID="LinkButton1ShowTreeInsert" runat="server" OnClick="lbShowTree_Click">
                                                            <asp:Label ID="LabelKategoriInsert" runat="server" />
                                                        </asp:LinkButton></asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbProductImage %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="tbImageText" runat="server" Width="150px" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="ImageButton1Insert" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;" />
                                                            <script type="text/javascript">
          //<![CDATA[
                                                                function OpenFileExplorerDialog() {
                                                                    selectedFile = $find("<%= tbImageText.ClientID %>");
                                                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                                    wnd.show();
                                                                }
                                                            </script>
                                                            <telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
                                                                               ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
                                                                               Modal="true" Behaviors="Close,Move,Resize,Maximize">
                                                            </telerik:RadWindow>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <%= AdminResource.lbDesc %>
                                            </td>
                                            <td valign="top">
                                                :
                                            </td>
                                            <td>
                                                <uc1:Rtb ID="Rtb2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbPrice %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPriceInsert" runat="server" onkeydown="return priceInputsCharacters(event,this);" />
                                                <asp:DropDownList ID="ddlPriceCurrency" AutoPostBack="True" runat="server">
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
                                                <asp:CheckBox ID="cbActiveInsert" runat="server" Checked="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbFeaturedItem %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbVitrinInsert" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" ValidationGroup="aa"
                                                            OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlEditProduct" runat="server" Visible="False" DefaultButton="btnUpdate">
                                    <table>
                                        <tr>
                                            <td class="Sol">
                                                <%= AdminResource.lbName %>
                                            </td>
                                            <td class="Orta">
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBoxNameEdit" runat="server" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <%= AdminResource.lbSelectCategory %>
                                            </td>
                                            <td valign="top">
                                                :
                                            </td>
                                            <td>
                                                <asp:MultiView ID="mvKat" runat="server">
                                                    <asp:View ID="vTree" runat="server">
                                                        <telerik:RadTreeView ID="TreeView1Edit" runat="server" ImageSet="Arrows" OnNodeClick="TreeView1Edit_SelectedNodeChanged">
                                                        </telerik:RadTreeView>
                                                    </asp:View>
                                                    <asp:View ID="vLabel" runat="server">
                                                        <asp:LinkButton ID="LinkButtonShowTreeEdit" runat="server" OnClick="LinkButtonShowTreeEdit_Click">
                                                            <asp:Label ID="LabelKategoriEdit" runat="server"></asp:Label>
                                                        </asp:LinkButton></asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <%= AdminResource.lbProductImage %>
                                            </td>
                                            <td valign="top">
                                                :
                                            </td>
                                            <td>
                                                <asp:DataList ID="DataList1" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                              CellPadding="0" CellSpacing="5" OnItemDataBound="DataList1_ItemDataBound">
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <div style="border: 1px solid #dfdfdf; padding: 5px 5px 10px 5px;">
                                                            <asp:Image ID="Image1" runat="server" Height="60px" max-width="100px" ImageUrl='<%#Eval
                                                                                                   ("Thumbnail") %>' /></div>
                                                        <div style="padding-top: 5px; text-align: left;">
                                                            <asp:ImageButton ID="ImgBtnDeleteImage" runat="server" CommandArgument='<%#Eval("ProductImageId") %>'
                                                                             Width="16px" OnClick="ImgBtnDeleteImage_Click" ImageUrl="~/Admin/images/icon/cop.png" />
                                                            <asp:LinkButton ID="lbGorselSec" CommandArgument='<%#Eval("ProductImageId") %>' runat="server"
                                                                            OnClick="lbGorselSec_Click">
                                                                <asp:RadioButton ID="RadioButton1" runat="server" Font-Size="X-Small" GroupName="a"
                                                                                 TextAlign="Left" Enabled="True" AutoPostBack="True" Checked='<%#Eval("MainImage") %>' />
                                                                <%= AdminResource.lbMainImage %>
                                                            </asp:LinkButton>
                                                            <br />
                                                        </div>
                                                        <br />
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px;">
                                                <%= AdminResource.lbNewImage %>
                                            </td>
                                            <td style="width: 10px;">
                                                :
                                            </td>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="TextBox1" runat="server" Width="180px" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBox1"
                                                                                        ForeColor="Red" ValidationGroup="groupImg" ErrorMessage="!" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:Button ID="btnNewImage" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Button ID="btnAddImage" ValidationGroup="groupImg" runat="server" CssClass="SaveCancelBtn"
                                                                        OnClick="btnAddImageClick" />
                                                            <script type="text/javascript">
          //<![CDATA[
                                                                function OpenFileExplorerDialog() {
                                                                    selectedFile = $find("<%= TextBox1.ClientID %>");
                                                                    var wnd = $find("<%= ExplorerWindow2.ClientID %>");
                                                                    wnd.show();
                                                                }
                                                            </script>
                                                            <telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
                                                                               ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow2"
                                                                               Modal="true" Behaviors="Close,Move,Resize,Maximize">
                                                            </telerik:RadWindow>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <%= AdminResource.lbDesc %>
                                            </td>
                                            <td valign="top">
                                                :
                                            </td>
                                            <td>
                                                <uc1:Rtb ID="Rtb1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbPrice %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBoxPriceEdit" onkeydown="return priceInputsCharacters(event,this);"
                                                             runat="server"></asp:TextBox><asp:DropDownList ID="ddlPriceCurrencyEdit" AutoPostBack="True"
                                                                                                            runat="server">
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
                                                <asp:CheckBox ID="CheckBoxActiveEdit" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%= AdminResource.lbFeaturedItem %>
                                            </td>
                                            <td>
                                                :
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="CheckBoxVitrinEdit" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnUpdate" runat="server" OnClientClick='return priceInputsValidation(<%= TextBoxPriceEdit.ClientID %>, <%= ddlPriceCurrencyEdit.ClientID %>);'
                                                            CssClass="SaveCancelBtn" ValidationGroup="g1" OnClick="btnUpdate_Click" />
                                                <asp:Button ID="btnUpdateCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnUpdateCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:GridView ID="grvProducts" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                              CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                              SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                              PageSize="15" AllowPaging="True" SortedAscendingCellStyle-CssClass="sortasc"
                                              SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                              SelectedRowStyle="selected" Width="565px" ForeColor="#333333" GridLines="None"
                                              DataSourceID="edsUrunler" OnRowCommand="GridView1_RowCommand" OnRowDataBound="grvProducts_RowDataBound">
                                    <EmptyDataTemplate>
                                        <%= AdminResource.lbNoRecord %>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditProduct" runat="server" CausesValidation="False" ImageUrl="~/Admin/images/icon/edit.png"
                                                                 CommandName="Guncelle" CommandArgument='<%#Eval("ProductId") %>' Text="Düzenle" />
                                                <asp:ImageButton ID="btnDeleteProduct" runat="server" CausesValidation="False" ImageUrl="~/Admin/images/icon/cop.png"
                                                                 CommandName="Sil" CommandArgument='<%#Eval("ProductId") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label></ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:EntityDataSource ID="edsUrunler" runat="server" ConnectionString="name=Entities"
                                                      DefaultContainerName="Entities" Where="it.ProductCategoryId=@catId and it.languageId=@languageId"
                                                      OrderBy="it.ProductId DESC" EntitySetName="Products" EnableDelete="True" EnableUpdate="True">
                                    <WhereParameters>
                                        <asp:Parameter DbType="Int32" Name="catId" DefaultValue="0" />
                                    </WhereParameters>
                                </asp:EntityDataSource>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
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
<asp:HiddenField ID="HiddenField1ProductCategoryIdInsert" runat="server" />
<asp:HiddenField ID="HiddenField1ProductIdEdit" runat="server" />
<asp:HiddenField ID="HiddenField1ProductCategoryId" runat="server" />