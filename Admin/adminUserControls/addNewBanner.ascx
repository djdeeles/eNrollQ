<%@ Control Language="C#" AutoEventWireup="true" Inherits="admin_addNewBanner" CodeBehind="addNewBanner.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView ID="mvAuth" runat="server">
    <asp:View ID="vAuth" runat="server">
        <table class="rightcontenttable">
            <tr>
                <td>
                    <asp:MultiView ID="mvBanner" runat="server">
                        <asp:View ID="vList" runat="server">
                            <asp:Button ID="btnAddNewBanner" runat="server" CssClass="NewBtn" OnClick="btnAddNewBanner_Click" />
                            <asp:GridView ID="gvBanners" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                          CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                          SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                          PageSize="15" AllowPaging="True" SortedAscendingCellStyle-CssClass="sortasc"
                                          SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                          SelectedRowStyle="selected" DataKeyNames="bannersId" ForeColor="#333333" GridLines="None"
                                          Width="100%" DataSourceID="EntityDataSourceBanners" AllowSorting="True" OnRowDataBound="GvBannersRowDataBound"
                                          OnDataBound="GvBannersDataBound" OnRowCommand="GvBannersRowCommand">
                                <EmptyDataTemplate>
                                    <%= AdminResource.lbNoRecord %>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="İşlemler">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="False" ImageUrl="~/Admin/images/icon/edit.png"
                                                             CommandName="Guncelle" CommandArgument='<%#Eval
                                                                                                   ("bannersId") %>'
                                                             Text="Düzenle" />
                                            <asp:ImageButton ID="lbCatSil" runat="server" CausesValidation="False" ImageUrl="~/Admin/images/icon/cop.png"
                                                             CommandName="Sil" CommandArgument='<%#Eval("bannersId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="bannerName" HeaderText="İsim" SortExpression="bannerName">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Dosya Tipi">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelDosyaTipi" runat="server" Text='<%#                                        Bind("bannerFileTypeId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle Width="75px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:EntityDataSource ID="EntityDataSourceBanners" runat="server" ConnectionString="name=Entities"
                                                  DefaultContainerName="Entities" EnableDelete="True" EnableFlattening="False" OrderBy="it.CreatedTime desc"
                                                  EntitySetName="Banners">
                            </asp:EntityDataSource>
                        </asp:View>
                        <asp:View ID="vAdd" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="Label22" runat="server"><%= AdminResource.lbSelectFileType %></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100px">
                                            <%= AdminResource.lbFileType %>
                                        </td>
                                        <td width="10px">:
                                        </td>
                                        <td width="200">
                                            <asp:DropDownList ID="ddlDosyaTipi" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDosyaTipi_SelectedIndexChanged"
                                                              Width="205px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAd" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAd"
                                                                        ForeColor="Red" ValidationGroup="1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbImage %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtPath" runat="server" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSelectImage" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;" /><asp:RequiredFieldValidator
                                                                                                                                                                                   ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPath" ForeColor="Red"
                                                                                                                                                                                   ValidationGroup="1">!</asp:RequiredFieldValidator>
                                            <script type="text/javascript">
                                                function OpenFileExplorerDialog() {
                                                    selectedFile = $find("<%= txtPath.ClientID %>");
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
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbLink %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUrl" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbClickTAG %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCliktag" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbHeight %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtHeight" runat="server" Width="50px"></asp:TextBox>
                                            <asp:Label ID="Label9" runat="server" Text="px"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtHeight"
                                                                        ForeColor="Red" ValidationGroup="1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbWidth %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtWidth" runat="server" Width="50px"></asp:TextBox>
                                            <asp:Label ID="Label10" runat="server" Text="px"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtWidth"
                                                                        ForeColor="Red" ValidationGroup="1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveClick"
                                                        ValidationGroup="1" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="lblUyari" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="vEdit" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnEditSave">
                                <table>
                                    <tr>
                                        <td width="100px">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td width="10px">:
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="LabelDurum" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbViewCount %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="LabelGosterimSayisi" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txteAd" runat="server" Width="200px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txteAd"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="g1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbLink %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txteUrl" runat="server" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbClickTAG %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                        <asp:TextBox ID="txteClickTag" runat="server" Width="200px"></asp:TextBox>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbHeight %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txteHeight" runat="server" Width="50px"></asp:TextBox>
                                            <asp:Label ID="Label29" runat="server" Text="px"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txteHeight"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="g1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbWidth %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txteWidth" runat="server" Width="50px"></asp:TextBox>
                                            <asp:Label ID="Label31" runat="server" Text="px"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txteWidth"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="g1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnEditSave" runat="server" CssClass="SaveCancelBtn" ValidationGroup="g1"
                                                        OnClick="BtEditSaveClick" />
                                            <asp:Button ID="btnEditCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnEditCancelClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="Label32Durum" runat="server" Text=""></asp:Label>
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