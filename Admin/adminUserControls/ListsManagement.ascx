﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListsManagement.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.ListsManagement" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="uc1" TagName="Rtb" Src="~/Admin/adminUserControls/Rtb.ascx" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td>
                    <asp:MultiView ID="mvLists" runat="server">
                        <asp:View ID="vAddListBtn" runat="server">
                            <asp:Button ID="btnAddNew" runat="server" CssClass="NewBtn" OnClick="BtnAddNewClick" />
                        </asp:View>
                        <asp:View ID="vAddList" runat="server">
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
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" Width="250px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                                ForeColor="Red" ValidationGroup="vg1">(!)</asp:RequiredFieldValidator>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDesc" TextMode="MultiLine" runat="server" Width="250px"></asp:TextBox>
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
                                            <asp:CheckBox ID="cbState" runat="server" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveClick"
                                                ValidationGroup="vg1" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelClick" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="vEditList" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnEditSave">
                                <table>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNameEdit" runat="server" Width="250px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtNameEdit"
                                                runat="server" ForeColor="Red" ValidationGroup="vg2">(!)</asp:RequiredFieldValidator>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescEdit" TextMode="MultiLine" runat="server" Width="250px"></asp:TextBox>
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
                                            <asp:CheckBox ID="cbStateEdit" runat="server" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnEditSave" runat="server" CssClass="SaveCancelBtn" OnClick="BtnEditSaveClick"
                                                ValidationGroup="vg2" />
                                            <asp:Button ID="btnEditCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnEditCancelClick" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:HiddenField ID="HiddenFieldListId" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvLists" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                        CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        PageSize="15" AllowPaging="True" SortedAscendingCellStyle-CssClass="sortasc"
                        SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                        SelectedRowStyle="selected" Width="100%" CellPadding="4" ForeColor="#333333"
                        GridLines="None" OnRowCommand="GvListsRowCommand" AllowSorting="True" OnRowDataBound="GvListsRowDataBound">
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
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbListSec" runat="server" CausesValidation="False" CommandName="Select"
                                        CommandArgument='<%#Eval("Id") %>' Text='<%#Bind("Name") %>' OnClick="LbListSecClick" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbEditName" runat="server" Text='<%#Eval("albumName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Description" SortExpression="Description">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:CheckBoxField DataField="State" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                SortExpression="State">
                                <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>
                    <asp:EntityDataSource ID="EntityDataSourceListManagement" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Lists"
                        EntityTypeFilter="" Select="" EnableDelete="True" EnableUpdate="True" Where="it.LanguageId=@languageId"
                        OrderBy="it.UpdatedTime desc">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="hfLanguageId" Name="languageId" PropertyName="Value"
                                DbType="Int32" />
                        </WhereParameters>
                    </asp:EntityDataSource>
                    <asp:HiddenField ID="hfLanguageId" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:MultiView ID="mvListData" runat="server">
                        <asp:View ID="vAddListDataBtn" runat="server">
                            <asp:Panel runat="server" DefaultButton="ImageButton2Ara">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddListData" runat="server" CssClass="NewBtn" OnClick="BtnAddListDataClick" />
                                        </td>
                                        <td align="right">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td valign="top">
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
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="vAddListData" runat="server">
                            <asp:Panel runat="server" DefaultButton="BtnListDataSave">
                                <table cellpadding="1">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Label1" runat="server" Style="color: #FF0000" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="90px">
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtListDataTitle" runat="server" Width="400px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtListDataTitle"
                                                ForeColor="Red" ValidationGroup="vg1">
                                                (!)
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtListDataDesc" TextMode="MultiLine" runat="server" Width="400px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtListDataDesc"
                                                ForeColor="Red" ValidationGroup="vg1">
                                                (!)
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbDate %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dpDate" runat="server">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="dpDate"
                                                ForeColor="Red" ValidationGroup="vg1">(!)
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbImagePath %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtImage" runat="server" Width="400px" />
                                            <asp:Button ID="btnImageSelect" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialogAddImage(); return false;" />
                                            <script type="text/javascript">
                                                function OpenFileExplorerDialogAddImage() {
                                                    selectedFile = $find("<%= txtImage.ClientID %>");
                                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                    wnd.show();
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbDetail %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <uc1:Rtb ID="Rtb" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbAttachments %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <b>Eklenti ekleme işlemini, liste elemanını ekledikten sonra, liste elemanı düzenleme sayfasından yapabilirsiniz.</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbListDataState" Checked="False" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="BtnListDataSave" runat="server" CssClass="SaveCancelBtn" OnClick="BtnListDataSaveClick"
                                                ValidationGroup="vg1" />
                                            <asp:Button ID="BtnListDataCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnListDataCancelClick" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="vEditListData" runat="server">
                            <asp:Panel runat="server" DefaultButton="BtnListDataEditSave">
                                <table>
                                    <tr>
                                        <td width="90px">
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtListDataTitleEdit" runat="server" Width="400px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtListDataTitleEdit"
                                                runat="server" ForeColor="Red" ValidationGroup="vg2">(!)</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtListDataDescEdit" TextMode="MultiLine" runat="server" Width="400px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtListDataDescEdit"
                                                ForeColor="Red" ValidationGroup="vg2">
                                                (!)
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbDate %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dpDateEdit" runat="server">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="dpDateEdit"
                                                ForeColor="Red" ValidationGroup="vg2">(!)
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbImagePath %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtImageEdit" runat="server" Width="400px" />
                                            <asp:Button ID="btnImageSelectEdit" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog1(); return false;" />
                                            <script type="text/javascript">
                                                function OpenFileExplorerDialog1() {
                                                    selectedFile = $find("<%= txtImageEdit.ClientID %>");
                                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                    wnd.show();
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbDetail %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <uc1:Rtb ID="RtbEdit" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbAttachments %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <asp:DataList ID="dtlistAttachments" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                CellPadding="0" CellSpacing="5" OnItemDataBound="DtlistAttachmentsItemDataBound">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <ItemTemplate>
                                                    <div style="border: 1px solid #dfdfdf; padding: 5px 5px 10px 5px;">
                                                        <asp:Image ID="Image1" runat="server" Height="60px" max-width="100px"
                                                            ImageUrl='<%#((Eval("Thumbnail") != null && Eval("Thumbnail").ToString() != "") 
                                                        ? Eval("Thumbnail"):"~/Admin/images/noimage.png") %>' />
                                                    </div>
                                                    <div style="padding-top: 5px; text-align: left;">
                                                        <asp:ImageButton ID="ImgBtnDeleteAttach" runat="server" CommandArgument='<%#Eval("Id") %>'
                                                            Width="16px" OnClick="ImgBtnDeleteAttachClick" ImageUrl="~/Admin/images/icon/cop.png" />
                                                        <%#((Eval("Title").ToString().Length<12 ? Eval("Title"):Eval("Title").ToString().Substring(0,12)+"...")) %>
                                                    </div>
                                                    <br />
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbNewAttachment%>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="tbAtthcmtTitle" runat="server" Width="400px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbAtthcmtTitle"
                                                ForeColor="Red" ValidationGroup="groupImg" ErrorMessage="!" />
                                            <asp:Button ID="btnNewAtthcmnt" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog2(); return false;" />
                                            <asp:Button ID="btnAddAtthcmnt" ValidationGroup="groupImg" runat="server" CssClass="SaveCancelBtn"
                                                OnClick="BtnAddAtthcmntClick" />
                                            <script type="text/javascript">
                                                function OpenFileExplorerDialog2() {
                                                    selectedFile = $find("<%= tbAtthcmtTitle.ClientID %>");
                                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                    wnd.show();
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td valign="top">:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbListDataStateEdit" Checked="False" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Button ID="BtnListDataEditSave" runat="server" CssClass="SaveCancelBtn" OnClick="BtnListDataEditSaveClick"
                                                ValidationGroup="vg2" />
                                            <asp:Button ID="BtnListDataEditCancel" runat="server" CssClass="SaveCancelBtn" OnClick="BtnListDataEditCancelClick" />
                                        </td>
                                    </tr>
                                </table> 
                            </asp:Panel>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvListData" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                        CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                        EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                        PageSize="15" AllowPaging="True" Width="100%" CellPadding="4" ForeColor="#333333"
                        GridLines="None" OnRowCommand="GvListDataRowCommand" AllowSorting="True" OnRowDataBound="GvListDataRowDataBound">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButtonGuncelle" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                        CommandArgument='<%#Eval("Id") %>' CommandName="Guncelle" />
                                    <asp:ImageButton ID="lbSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                        CommandArgument='<%#Eval("Id") %>' CommandName="Sil" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%#Eval("Title") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbEditTitle" runat="server" Text='<%#Eval("Title") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Description" SortExpression="Description">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%#Convert.ToDateTime(Eval("Date").ToString()).ToShortDateString() %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                <ItemStyle Width="50px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <a href='<%#Eval("Image").ToString().Replace("~", "..") %>'
                                        rel="prettyPhoto">
                                        <asp:Image ID="Image1" runat="server" Width="50px" ImageUrl='<%#                                        Bind("ThumbnailPath") %>' /></a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                <ItemStyle Width="50px" HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:CheckBoxField DataField="State" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                SortExpression="State">
                                <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>
                    <asp:EntityDataSource ID="EntityDataSourceListData" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="ListData"
                        EntityTypeFilter="" Select="" EnableDelete="True" EnableUpdate="True" Where="it.ListId=@ListId"
                        OrderBy="it.UpdatedTime desc">
                        <WhereParameters>
                            <asp:Parameter Name="ListId" DefaultValue="0" DbType="Int32" />
                        </WhereParameters>
                    </asp:EntityDataSource>
                    <asp:HiddenField runat="server" ID="HiddenFieldListDataId" />
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
<telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
    ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
    Modal="true" Behaviors="Close,Move,Resize,Maximize">
</telerik:RadWindow>
