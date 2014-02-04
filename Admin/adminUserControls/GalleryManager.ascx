<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_GalleryManager"
    CodeBehind="GalleryManager.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td>
                    <asp:MultiView ID="mvKat" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vvKatEkle" runat="server">
                            <asp:Button ID="btnNew" runat="server" CssClass="NewBtn" OnClick="BtnNewCategoryClick" />
                        </asp:View>
                        <asp:View ID="vvYeniKat" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td width="100px">
                                            <%= AdminResource.lbCategoryName %>
                                        </td>
                                        <td width="10px">:
                                        </td>
                                        <td width="200px">
                                            <asp:TextBox ID="txtCatName" runat="server" Width="190px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCatName"
                                                ForeColor="Red" ValidationGroup="gk1" SetFocusOnError="True">(!)</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCatDesc" runat="server" Height="70px" Width="188px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbCatActive" runat="server" Checked="True" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCategorySaveClick"
                                                ValidationGroup="gk1" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveCategoryCancelClick" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        PageSize="15" AllowPaging="False" SortedAscendingCellStyle-CssClass="sortasc"
                        SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                        SelectedRowStyle-CssClass="selected" DataKeyNames="photoAlbumCategoryId" OnRowCommand="GvCategoriesRowCommand"
                        ForeColor="#333333" GridLines="None" Width="100%" OnRowDataBound="GvCategoriesRowDataBound"
                        DataSourceID="edsCat" AllowSorting="True">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                        CommandName="Update" CausesValidation="True" CommandArgument='<%#                                        Eval("photoAlbumCategoryId") %>' />
                                    <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                        CommandName="Cancel" CausesValidation="False" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                        CommandName="Edit" CommandArgument='<%#                                        Eval("photoAlbumCategoryId") %>' />
                                    <asp:ImageButton ID="lbCatSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                        CausesValidation="False" OnClick="LbCatSilClick" CommandArgument='<%#                                        Eval("photoAlbumCategoryId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbCatSec" runat="server" CausesValidation="False" CommandName="Select"
                                        CommandArgument='<%#                                        Eval("photoAlbumCategoryId") %>'
                                        Text='<%#Bind("categoryName") %>' OnClick="LbCatSecClick"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbEdidCategoryName" runat="server" Text='<%#Eval("categoryName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="categoryNotes" SortExpression="categoryNotes">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:CheckBoxField DataField="state" SortExpression="state">
                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                            </asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:EntityDataSource ID="edsCat" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="Def_photoAlbumCategory"
                        Where="it.languageId=@languageId" EnableFlattening="False" EntityTypeFilter="Def_photoAlbumCategory" OrderBy="it.CreatedTime desc">
                        <WhereParameters>
                            <asp:ControlParameter Name="languageId" ControlID="HiddenField1" PropertyName="Value"
                                DbType="Int32" DefaultValue="1" />
                        </WhereParameters>
                    </asp:EntityDataSource>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:MultiView ID="mvAlb" runat="server" ActiveViewIndex="0" Visible="False">
                        <asp:View ID="vvAlbEkle" runat="server">
                            <asp:Button ID="btnNewAlbum" runat="server" CssClass="NewBtn" OnClick="BtnNewAlbumClick" />
                        </asp:View>
                        <asp:View ID="vvYeniAlb" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSaveAlbum">
                                <table>
                                    <tr>
                                        <td width="100px">
                                            <%= AdminResource.lbAlbumName %>
                                        </td>
                                        <td width="10px">:
                                        </td>
                                        <td width="200px">
                                            <asp:TextBox ID="txtAlbName" runat="server" Width="190px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAlbName"
                                                ForeColor="Red" ValidationGroup="gk2" SetFocusOnError="True">(!)</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAlbDesc" runat="server" Height="70px" Width="188px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbAlbActive" runat="server" Checked="True" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSaveAlbum" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveAlbumClick"
                                                ValidationGroup="gk2" />
                                            <asp:Button ID="btnCancelAlbum" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelAlbumClick" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvAlb" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                        EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle-CssClass="selected"
                        DataKeyNames="photoAlbumId" ForeColor="#333333" GridLines="None" Visible="False"
                        DataSourceID="edsPhotoAlbum" OnRowCommand="GvAlbRowCommand" OnRowDataBound="GvAlbRowDataBound"
                        AllowPaging="False" Width="100%" AllowSorting="True">
                        <Columns>
                            <asp:TemplateField HeaderText="İşlemler">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                        CommandName="Update" CausesValidation="True" CommandArgument='<%#Eval("photoAlbumId") %>' />
                                    <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                        CommandName="Cancel" CausesValidation="False" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                        CommandName="Edit" CommandArgument='<%#Eval("photoAlbumId") %>' />
                                    <asp:ImageButton ID="lbAlbSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                        CausesValidation="False" OnClick="LbAlbSilClick" CommandArgument='<%#Eval("photoAlbumId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Albüm Adı">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbAlbSec" runat="server" CausesValidation="False" CommandName="Select"
                                        CommandArgument='<%#Eval("photoAlbumId") %>' Text='<%#Bind("albumName") %>' OnClick="LbAlbSecClick"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbEditName" runat="server" Text='<%#Eval("albumName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="albumNote" HeaderText="Açıklama" SortExpression="albumNote">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%#                                        GetCategoryName(Convert.ToInt32(Eval("photoAlbumCategoryId"))) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="edsCat" DataTextField="categoryName"
                                        DataValueField="photoAlbumCategoryId" SelectedValue='<%#                                        Bind("photoAlbumCategoryId") %>'>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="125px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="125px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="state" HeaderText="Durum" SortExpression="state">
                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                            </asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:EntityDataSource ID="edsPhotoAlbum" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="Def_photoAlbum"
                        Where="it.photoAlbumCategoryId=@catId" OrderBy="it.CreatedTime desc">
                        <WhereParameters>
                            <asp:Parameter Name="catId" DbType="Int32" DefaultValue="0" />
                        </WhereParameters>
                    </asp:EntityDataSource>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:MultiView ID="mvPhoto" runat="server" ActiveViewIndex="0" Visible="False">
                        <asp:View ID="vvPhotoEkle" runat="server">
                            <asp:Button ID="btnNewImage" runat="server" CssClass="NewBtn" OnClick="BtnNewImageClick" />
                            <asp:Button ID="btnNewMultipleImage" runat="server" CssClass="NewBtn" OnClick="BtnNewMultipleImageClick" />
                        </asp:View>
                        <asp:View ID="vvYeniPhoto" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSavePhoto">
                                <table>
                                    <tr>
                                        <td width="100px">
                                            <%= AdminResource.lbImagePath %>
                                        </td>
                                        <td width="10px">:
                                        </td>
                                        <td width="200px">
                                            <telerik:RadTextBox ID="txtPhotoPath" runat="server" Width="190px" />
                                        </td>
                                        <td width="200px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnImageSelect" OnClientClick="OpenFileExplorerDialog(); return false;"
                                                            runat="server" CssClass="ImageSelectBtn" />
                                                    </td>
                                                    <td width="100%">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPhotoPath"
                                                            SetFocusOnError="True" ValidationGroup="gk3">
                                                
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <script type="text/javascript">
                                                        //<![CDATA[
                                                        function OpenFileExplorerDialog() {
                                                            var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                            wnd.show();
                                                        }

                                                        //This function is called from a code declared on the Explorer.aspx page

                                                        function OnFileSelected(fileSelected) {
                                                            var textbox = $find("<%= txtPhotoPath.ClientID %>");
                                                            textbox.set_value("~" + fileSelected);
                                                        }


                                                        //]]>
                                                    </script>
                                                    <telerik:RadWindow ID="ExplorerWindow" runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
                                                        ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx"
                                                        Modal="true" Behaviors="Close,Move,Resize,Maximize">
                                                    </telerik:RadWindow>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhotoName" runat="server" Width="190px"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td>
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhotoDesc" runat="server" Height="70px" Width="188px" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="photoState" Checked="False" runat="server" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbMainImage %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbMainImage" Checked="False" runat="server" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSavePhoto" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSavePhotoClick"
                                                ValidationGroup="gk3" />
                                            <asp:Button ID="btnCancelPhoto" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelPhotoClick" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="vvYeniMultiplePhoto" runat="server">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSavePhoto">
                                <table>
                                    <tr>
                                        <td width="100px" valign="top">
                                            <%= AdminResource.lbImagePath %>
                                        </td>
                                        <td width="10px" valign="top">:
                                        </td>
                                        <td width="620px" colspan="2">
                                            <span id="spSelectedFilesInfo">Dosya seçilmedi</span>
                                            <br />
                                            <telerik:RadFileExplorer runat="server" ID="FileExplorer1" Width="100%" Height="300px" OnClientItemSelected="OnClientItemSelected">
                                                <Configuration ViewPaths="~/FileManager/" UploadPaths="~/FileManager/" DeletePaths="~/FileManager/"
                                                    SearchPatterns="*.jpg, *.png, *.jpeg, *.gif" AllowMultipleSelection="True" MaxUploadFileSize="10240000"></Configuration>
                                            </telerik:RadFileExplorer>
                                            <script type="text/javascript">

                                                function OnClientItemSelected(sender, args) {// Called when a file is open.

                                                    var item = args.get_item();
                                                    //If file (and not a folder) is selected - call the OnFileSelected method on the parent page
                                                    if (item.get_type() == Telerik.Web.UI.FileExplorerItemType.File) {
                                                        // Cancel the default dialog;
                                                        args.set_cancel(true);

                                                        var selectedItems = sender.get_selectedItems();
                                                        var hfResult = "";
                                                        for (var i = 0; i < selectedItems.length; i++) {
                                                            hfResult += "~" + selectedItems[i].get_url() + "|";
                                                        }
                                                        hfResult = hfResult.slice(0, -1);
                                                        OnMultipleFileSelected(i + " dosya seçildi", hfResult); // Call the method declared on the parent page

                                                    }
                                                }


                                                function OnMultipleFileSelected(fileSelectedInfo, selectedFiles) {
                                                    var hfSelectedFiles = document.getElementById('<%= hfSelectedFiles.ClientID %>');
                                                                hfSelectedFiles.value = selectedFiles;

                                                                var spSelectedFilesInfo = document.getElementById('spSelectedFilesInfo');
                                                                spSelectedFilesInfo.innerHTML = fileSelectedInfo;
                                                            }
                                                            //]]>
                                            </script>
                                            <asp:HiddenField runat="server" ID="hfSelectedFiles" /> 
                                        </td>
                                    </tr>

                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbMultiFilesState" Checked="False" runat="server" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbUseFilesName %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbUseFilesName" Checked="True" runat="server" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnSaveMultiplePhoto" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveMultiplePhotoClick"
                                                ValidationGroup="gk3" />
                                            <asp:Button ID="BtnCancelMultiplePhoto" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelMultiplePhotoClick" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvPhotos" runat="server" AutoGenerateColumns="False" Visible="False"
                        PageSize="15" CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                        EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle-CssClass="selected"
                        CellPadding="4" DataKeyNames="photoId" DataSourceID="edsPhotos" ForeColor="#333333"
                        GridLines="None" Width="100%" AllowPaging="True" AllowSorting="True" OnRowCommand="GvPhotosRowCommand"
                        OnRowDataBound="GvPhotosRowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="İşlemler">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                        CommandName="Update" CausesValidation="True" CommandArgument='<%#Eval("photoId") %>' />
                                    <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                        CommandName="Cancel" CausesValidation="False" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                        CommandName="Edit" CommandArgument='<%#Eval("photoId") %>' />
                                    <asp:ImageButton ID="lbGorselSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                        CommandName="Delete" CausesValidation="False" OnClick="LbGorselSilClick" CommandArgument='<%#Eval("photoId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Görsel">
                                <ItemTemplate>
                                    <a href='<%#                                        Eval("photoPath").ToString().Replace("~", "..") %>' rel="prettyPhoto">
                                        <asp:Image ID="Image1" runat="server" Width="50px" ImageUrl='<%#Bind("thumbnailPath") %>' /></a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle Width="75px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="photoName" HeaderText="Görsel Adı" SortExpression="photoName">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="photoNote" HeaderText="Açıklama" SortExpression="photoNote">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lbAlbum" runat="server" Text='<%#                GetAlbumName(Convert.ToInt32(Eval("photoAlbumId"))) %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlNewAlbum" runat="server" DataSourceID="edsPhotoAlbum"
                                        DataTextField="albumName" DataValueField="photoAlbumId" SelectedValue='<%#Bind("photoAlbumId") %>'>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="125px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="125px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:CheckBox ID="State" runat="server" Enabled="False" Checked='<%#Bind("State") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%#Bind("State") %>' />
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="50px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:CheckBox ID="MainPhoto" runat="server" Enabled="False" Checked='<%#Bind("mainPhoto") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cbMainPhoto" runat="server" Enabled='<%#                !Convert.ToBoolean(Eval("mainPhoto")) %>'
                                        Checked='<%#Bind("mainPhoto") %>' />
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" Width="70px"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:EntityDataSource ID="edsPhotos" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="PhotoAlbum"
                        Where="it.photoAlbumId=@albId" OrderBy="it.CreatedTime desc">
                        <WhereParameters>
                            <asp:Parameter DefaultValue="0" DbType="Int32" Name="albId" />
                        </WhereParameters>
                    </asp:EntityDataSource>
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
<asp:HiddenField ID="hfSelectedCategory" runat="server" />
<asp:HiddenField ID="hfSelectedAlbum" runat="server" />
