<%@ Control Language="C#" AutoEventWireup="true" Inherits="admin_bannerManagement"
            CodeBehind="bannerManagement.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td colspan="3">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <asp:Button ID="btnAddNew" runat="server" CssClass="NewBtn" OnClick="btnAddNew_Click" />
                            <asp:GridView ID="gvBanners" runat="server" AutoGenerateColumns="False" DataKeyNames="bannerManagementId"
                                          CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                          SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                          SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                          EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                          OnDataBound="gvBanners_DataBound" Width="100%" CellPadding="4" ForeColor="#333333"
                                          PageSize="15" AllowPaging="True" GridLines="None" OnRowCommand="gvBanners_RowCommand"
                                          AllowSorting="True" OnRowDataBound="gvBanners_RowDataBound">
                                <EmptyDataTemplate>
                                    <%= AdminResource.lbNoRecord %>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="LinkButtonGuncelle" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                             CommandArgument='<%#Eval
                                                                                                   ("bannerManagementId") %>'
                                                             CommandName="Guncelle" />
                                            <asp:ImageButton ID="LinkButtonSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                             CommandArgument='<%#                                        Eval("bannerManagementId") %>'
                                                             CommandName="Sil" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="bannerLocationId" SortExpression="bannerLocationId">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField SortExpression="bannersId">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%#Bind("bannersId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="bannerLimit" SortExpression="bannerLimit">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="bannerCounter" SortExpression="bannerCounter">
                                        <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                        <ItemStyle HorizontalAlign="Left" Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="bannerStartDate" DataFormatString="{0:d}" SortExpression="bannerStartDate">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="bannerEndDate" DataFormatString="{0:d}" SortExpression="bannerEndDate">
                                        <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:CheckBoxField DataField="state" SortExpression="state">
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                                    </asp:CheckBoxField>
                                </Columns>
                            </asp:GridView>
                            <asp:EntityDataSource ID="EntityDataSourceBannerManagement" runat="server" ConnectionString="name=Entities"
                                                  DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="BannerManagement"
                                                  EntityTypeFilter="" Select="" EnableDelete="True" EnableUpdate="True">
                            </asp:EntityDataSource>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblUyariBos" runat="server" Style="color: #FF0000"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <%= AdminResource.lbBannerField %>
                                        </td>
                                        <td style="width: 10px">:
                                        </td>
                                        <td style="width: 250px">
                                            <asp:DropDownList ID="DropDownListReklamAlanlari1" runat="server" Width="250">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownListReklamAlanlari1"
                                                                        Display="Dynamic" ForeColor="Red" InitialValue="Seçiniz" ValidationGroup="vg1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <%= AdminResource.lbBanner %>
                                        </td>
                                        <td style="width: 10px">:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListReklamlar" runat="server" Width="250">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DropDownListReklamlar"
                                                                        Display="Dynamic" ForeColor="Red" InitialValue="Seçiniz" ValidationGroup="vg1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbViewLimit %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLimit" onkeydown="return onlyNumber(event);" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbStartDate %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dpStartDate" runat="server">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="dpStartDate"
                                                                        ForeColor="Red" ValidationGroup="vg1">!
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbEndDate %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dpEndDate" runat="server">
                                            </telerik:RadDatePicker>
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
                                            <asp:CheckBox runat="server" ID="cbAddState" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="btnSave_Click"
                                                        ValidationGroup="vg1" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancel_Click" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="View3" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnEditSave">
                                <table>
                                    <tr>
                                        <td style="width: 100px">
                                            <%= AdminResource.lbBanner %>
                                        </td>
                                        <td style="width: 10px">:
                                        </td>
                                        <td style="width: 250px">
                                            <asp:Label ID="LabelReklamAdi" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbViewLimit %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxLimit" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbStartDate %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dpStartDate2" runat="server">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="dpStartDate2"
                                                                        ForeColor="Red" ValidationGroup="valGrupEdit">!
                                            </asp:RequiredFieldValidator>
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbEndDate %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dpEndDate2" runat="server">
                                            </telerik:RadDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="cbEditState" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnEditSave" runat="server" CssClass="SaveCancelBtn" OnClick="btnEditSave_Click"
                                                        ValidationGroup="valGrupEdit" />
                                            <asp:Button ID="btnEditCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnEditCancel_Click" />
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