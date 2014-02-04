<%@ Control Language="C#" AutoEventWireup="true" Inherits="AdminIntroLightBox"
            CodeBehind="IntroLightBox.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView ID="MultiView2" runat="server">
    <asp:View ID="vLightBox" runat="server">
        <table class="rightcontenttable">
            <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="vNewBtn" runat="server">
                    <tr>
                        <td>
                            <asp:Button ID="btNewLightBox" runat="server" CssClass="NewBtn" OnClick="YeniEkleClick" />
                        </td>
                    </tr>
                </asp:View>
                <asp:View ID="vNew" runat="server">
                    <tr>
                        <td>
                            <asp:Panel runat="server" DefaultButton="btnKaydet">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100px;">
                                            <%= AdminResource.lbTitle %>
                                        </td>
                                        <td style="width: 10px;">:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbTitle" runat="server" Width="200px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbTitle"
                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><%= AdminResource.lbType %></td>
                                        <td>:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_OnSelectedIndexChanged" AutoPostBack="True"> 
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px;">
                                            <%= AdminResource.lbLink %>
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <div id="dvSayfa" runat="server">
                                                <table cellpadding="0" cellspacing="0" style="vertical-align: middle;">
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="tbLink" runat="server" Width="200px" />
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbLink"
                                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="dvGorsel" runat="server">
                                                <table cellpadding="0" cellspacing="0" style="vertical-align: middle;">
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="txtImage" runat="server" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="imgBtnImageSelect" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;"
                                                                        runat="server" /> 
                                                        </td>
                                                        <td>
                                                            <telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
                                                                               ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
                                                                               Modal="true" Behaviors="Close,Move,Resize,Maximize">
                                                            </telerik:RadWindow>
                                                            <script type="text/javascript">
                                                                //<![CDATA[
                                                                function OpenFileExplorerDialog() {
                                                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                                    wnd.show();
                                                                }

                                                                //This function is called from a code declared on the Explorer.aspx page

                                                                function OnFileSelected(fileSelected) {
                                                                    var textbox = $find("<%= txtImage.ClientID %>");
                                                                    textbox.set_value("~" + fileSelected);
                                                                }
                                                            //]]>
                                                            </script>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trWidth" runat="server">
                                        <td style="width: 100px;">
                                            <%= AdminResource.lbWidth %>
                                        </td>
                                        <td style="width: 10px;">:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbWidth" runat="server" Width="50px" /> px
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbWidth"
                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr id="trHeight" runat="server">
                                        <td style="width: 100px;">
                                            <%= AdminResource.lbHeight %>
                                        </td>
                                        <td style="width: 10px;">:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbHeight" runat="server" Width="50px" /> px
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbHeight"
                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px;">
                                            <%= AdminResource.lbRepeatDisplayTimeout %>
                                        </td>
                                        <td style="width: 10px;">:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbRepeatDisplayTimeout" runat="server" Width="50px" /> <%= AdminResource.lbHour %>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbRepeatDisplayTimeout"
                                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px;">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td style="width: 10px;">:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbState" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnKaydet" runat="server" CssClass="SaveCancelBtn" OnClick="KaydetClick"
                                                        ValidationGroup="g" />
                                            <asp:Button ID="btnIptal" runat="server" CssClass="SaveCancelBtn" OnClick="IptalClick" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hfLightBoxId" runat="server" />
                            </asp:Panel>
                        </td>
                    </tr>
                </asp:View>
            </asp:MultiView>
            <tr>
                <td>
                    <table class="rightcontenttable">
                        <tr>
                            <td>
                                <asp:GridView ID="gvIntroLightBoxes" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                              CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="EntityDataSource1"
                                              CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                              SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                              SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                              EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                              PageSize="15" AllowPaging="True" OnRowDataBound="GvIntroLightBoxes_OnRowDataBound"
                                              Width="100%" OnRowCommand="GvIntroLightBoxesRowCommand">
                                    <EmptyDataTemplate>
                                        <%= AdminResource.lbNoRecord %>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="../images/icon/edit.png"
                                                                             CommandArgument='<%#Bind
                                                                                                   ("Id") %>' CommandName="Guncelle" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="../images/icon/cop.png"
                                                                             CommandArgument='<%#Bind("Id") %>' CommandName="Sil" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Title" SortExpression="Title">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#                (Convert.ToInt32(Eval("Type")) == 0
                     ? "<a href='" + Eval("Link").ToString().Replace("~", "") +
                       "' target='_blank'>" +
                       "<img src='" + Eval("Link").ToString().Replace("~", "") +
                       "' height='50px' rel='prettyPhoto'/></a>"
                     : "<a href='" + Eval("Link").ToString().Replace("~", "") + "' target='_blank'>Link</a>") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#                (Convert.ToInt32(Eval("Type")) == 0 ? AdminResource.lbImage : AdminResource.lbPage) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CheckBoxField DataField="State" SortExpression="State">
                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                        </asp:CheckBoxField>
                                    </Columns>
                                </asp:GridView>
                                <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                                      DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="IntroLightBox">
                                </asp:EntityDataSource>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vNoAuth" runat="server">
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