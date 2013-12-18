<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_MenuManager"
    CodeBehind="MenuManager.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<asp:HiddenField ID="hdnActiveMenuId" runat="server" />
<asp:HiddenField ID="hdnLocationId" runat="server" />
<asp:HiddenField ID="hdnParentId" runat="server" />
<asp:HiddenField ID="hdnEditMode" runat="server" />
<asp:HiddenField ID="hdnMenuThema" runat="server" EnableViewState="False" Value="Default" />
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <div style="float: left; width: 210px; height: 400px; margin: 0 15px 10px 0; overflow: auto;
            background-color: #f6f6f6; border: 1px solid #E5E5E5">
            <asp:Panel ID="Panel1" runat="server">
                <telerik:RadTreeView runat="server" ID="RadTreeViewMenuler" EnableDragAndDrop="true"
                    EnableDragAndDropBetweenNodes="true" MultipleSelect="False" OnNodeClick="RadTreeViewMenulerNodeClick"
                    OnNodeDrop="RadTreeViewMenulerNodeDrop" IsExpanded="True" Font-Size="11px">
                    <DataBindings>
                        <telerik:RadTreeNodeBinding TextField="name" Expanded="True" Checkable="true" />
                        <telerik:RadTreeNodeBinding Checkable="false" TextField="name" Expanded="true" CssClass="rootNode" />
                    </DataBindings>
                </telerik:RadTreeView>
            </asp:Panel>
        </div>
        <div style="margin-bottom: 10px;">
            <asp:Button ID="BtnAddNewItem" CssClass="NewBtn" runat="server" OnClick="BtnAddNewItemClick" />
            <asp:Button ID="BtnDeleteItem" CssClass="DeleteBtn" runat="server" OnClick="BtnDeleteItemClick"
                Visible="False" />
        </div>
        <asp:Panel ID="pnlDynamicField" runat="server" Visible="False" DefaultButton="btSave">
            <table>
                <tr>
                    <td>
                        <%= AdminResource.lbName %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDynaDisplay" runat="server" Width="135px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDynaDisplay"
                            ForeColor="Red" ValidationGroup="vldGroup1">!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbType %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDynaDisType" runat="server" Width="140px">
                            <asp:ListItem Value="1">Combo</asp:ListItem>
                            <asp:ListItem Value="2">Grid</asp:ListItem>
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
                        <asp:CheckBox ID="chkDyna" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <%= AdminResource.lbList %>
                    </td>
                    <td valign="top">
                        :
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td valign="top">
                                    <asp:DropDownList ID="ddlDynamicGroup" runat="server" AutoPostBack="True" DataSourceID="EntityDynaGroup"
                                        DataTextField="name" DataValueField="groupId" Width="180px" OnDataBound="ddlDynamicGroup_DataBound"
                                        OnSelectedIndexChanged="ddlDynamicGroup_SelectedIndexChanged1">
                                    </asp:DropDownList>
                                    <asp:EntityDataSource ID="EntityDynaGroup" runat="server" ConnectionString="name=Entities"
                                        DefaultContainerName="Entities" EntitySetName="Customer_Dynamic_Group">
                                    </asp:EntityDataSource>
                                    <br />
                                    <asp:ListBox ID="ListBoxDynaSource" runat="server" Width="180px" Height="150px" />
                                    <asp:EntityDataSource ID="EntityDynaData" runat="server" ConnectionString="name=Entities"
                                        DefaultContainerName="Entities" EntitySetName="Customer_Dynamic">
                                    </asp:EntityDataSource>
                                </td>
                                <td width="5px">
                                </td>
                                <td valign="top">
                                    <asp:ListBox ID="ListBoxRef" runat="server" Height="175px" Width="180px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td width="5px">
                                </td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                                        OnClick="btnAdd_Click" />
                                    <asp:Button ID="btnRemove" runat="server" CssClass="SaveCancelBtn" OnClick="btnRemove_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btSave" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                            OnClick="btSaveDynamicMenu_Click" />
                        <asp:Button ID="btCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btEditCancelDynamicMenu_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="vldGroup1" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlMenu" runat="server" DefaultButton="BtnUpdateMenu">
            <asp:Panel ID="pnlEdit" runat="server">
                <table style="float: left; width: 525px;">
                    <tr>
                        <td style="width: 100px">
                            <%= AdminResource.lbTitle %>
                        </td>
                        <td style="width: 10px">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Width="400px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfMenuName" runat="server" ControlToValidate="txtName"
                                ValidationGroup="vldGroup1" ForeColor="Red" Display="Dynamic">(!)</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbType %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                <asp:ListItem Value="Seçiniz" Text="Seçiniz" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="0"></asp:ListItem>
                                <asp:ListItem Value="1"></asp:ListItem>
                                <asp:ListItem Value="3"></asp:ListItem>
                                <asp:ListItem Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlType"
                                ValidationGroup="vldGroup1" ForeColor="Red" Display="Dynamic">(!)</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            
                        </td>
                        <td>&nbsp;
                            
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubMenuType" runat="server" Width="250px">
                            </asp:DropDownList>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbMenuImage %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMenuImage" runat="server" Width="245px" />
                            <asp:Button ID="imgBtnMenuImageSelect" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog2(); return false;" />
                            <script type="text/javascript">
                                function OpenFileExplorerDialog2() {
                                    selectedFile = $find("<%= txtMenuImage.ClientID %>");
                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                    wnd.show();
                                }
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbMenuHoverImage %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMenuImageHover" runat="server" Width="245px" />
                            <asp:Button ID="imgBtnMenuImageHover" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog3(); return false;" />
                            <script type="text/javascript">
                                function OpenFileExplorerDialog3() {
                                    selectedFile = $find("<%= txtMenuImageHover.ClientID %>");
                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                    wnd.show();
                                }
                            </script>
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
                            <asp:CheckBox ID="cbState" runat="server" Checked="True" />
                        </td>
                    </tr>
                </table>
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <table style="float: left; width: 525px;">
                            <tr>
                                <td style="width: 100px">
                                    Url
                                </td>
                                <td style="width: 10px">
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUrl" runat="server" Width="250px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfMenuName0" runat="server" ControlToValidate="txtUrl"
                                        ForeColor="Red" ValidationGroup="vldGroup1" Display="Dynamic">!</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <table style="float: left; width: 525px;">
                            <tr valign="top">
                                <td style="width: 100px">
                                    <%= AdminResource.lbOptions %>
                                </td>
                                <td style="width: 10px">
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkSetStartPage" runat="server" Text="Baþlangýç sayfasý" />
                                    &nbsp;
                                    <asp:CheckBox ID="chkisHideName" runat="server" Text="Menü ismini gizle" />
                                    &nbsp;
                                    <asp:CheckBox ID="chkisHideToMenu" runat="server" Text="Menüde gösterme" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= AdminResource.lbTitleImage %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <telerik:RadTextBox ID="txtImage" runat="server" Width="245px" />
                                    <asp:Button ID="imgBtnImageSelect" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;" />
                                    <script type="text/javascript">
                                        function OpenFileExplorerDialog() {
                                            selectedFile = $find("<%= txtImage.ClientID %>");
                                            var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                            wnd.show();
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= AdminResource.lbTemplate %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlThemas" runat="server" DataSourceID="EntityDataSource1"
                                        DataTextField="name" DataValueField="id" Width="250px">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnShowDynamicFieldManager" runat="server" CssClass="SaveCancelBtn" OnClick="btnShowDynamicFieldManagerClick" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%= AdminResource.lbKeyWords %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtKeys" runat="server" Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <%= AdminResource.lbSummary %>&nbsp;/<br/>
									<%= AdminResource.lbDesc %>
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtHeader" runat="server" Height="75px" Rows="5" TextMode="MultiLine"
                                        Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table style="float: left;" width="100%">
                            <tr>
                                <td>
                                    <%= AdminResource.lbContent %>
                                    :
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <uc1:Rtb ID="Rtb1" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                            DefaultContainerName="Entities" EntitySetName="TemplatePages">
                        </asp:EntityDataSource>
                    </asp:View>
                </asp:MultiView>
                <table style="float: left;">
                    <tr>
                        <td>
                            <asp:Button ID="BtnUpdateMenu" runat="server" CssClass="SaveCancelBtn" OnClick="BtnUpdateMenuClick"
                                ValidationGroup="vldGroup1" />
                            <asp:Button ID="BtnCancelUpdateMenu" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelUpdateMenuClick" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
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
<telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
    ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
    Modal="true" Behaviors="Close,Move,Resize,Maximize">
</telerik:RadWindow>