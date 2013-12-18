<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_VideoManager"
    CodeBehind="VideoManager.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td>
                    <asp:MultiView ID="mvKat" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vvKatEkle" runat="server">
                            <asp:Button ID="btnNew" runat="server" CssClass="NewBtn" OnClick="btnNewCategory_Click" />
                        </asp:View>
                        <asp:View ID="vvYeniKat" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSave">
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td width="100px">
                                            <%= AdminResource.lbCategoryName %>
                                        </td>
                                        <td width="10px">
                                            :
                                        </td>
                                        <td width="200px">
                                            <asp:TextBox ID="txtNewCategory" runat="server" Width="190px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNewCategory"
                                                ForeColor="Red" ValidationGroup="gv1" SetFocusOnError="True">(!)</asp:RequiredFieldValidator>
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
                                            <asp:CheckBox ID="cbVideoState" runat="server" Checked="True" />
                                        </td>
                                        <td>
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
                                            <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="btnSave_Click"
                                                ValidationGroup="gv1" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancel_Click" />
                                        </td>
                                        <td>
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
                    <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                        EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle-CssClass="selected"
                        DataKeyNames="id" OnRowCommand="gvCategories_RowCommand" ForeColor="#333333"
                        GridLines="None" Width="100%" OnRowDataBound="gvCategories_RowDataBound" DataSourceID="edsCat"
                        AllowSorting="True" AllowPaging="False">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                        CommandName="Update" CausesValidation="True" CommandArgument='<%#                                        Eval("id") %>' />
                                    <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                        CommandName="Cancel" CausesValidation="False" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                        CommandName="Edit" CommandArgument='<%#Eval
                                                                                                   ("id") %>' />
                                    <asp:ImageButton ID="lbCatSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                        CausesValidation="False" OnClick="lbCatSil_Click" CommandArgument='<%#Eval("id") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbCatSec" runat="server" CausesValidation="False" CommandName="Select"
                                        CommandArgument='<%#Eval("id") %>' Text='<%#Bind("name") %>' OnClick="lbCatSec_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="Kategori" runat="server" Text='<%#Bind("name") %>'></asp:TextBox>
                                </EditItemTemplate>
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
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:EntityDataSource ID="edsCat" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="VideoCategories"
                        Where="it.languageId=@languageId" EnableFlattening="False" EntityTypeFilter="VideoCategories">
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
                    <asp:MultiView ID="mvVideos" runat="server" ActiveViewIndex="0" Visible="False">
                        <asp:View ID="vvVideoEkle" runat="server">
                            <asp:Button ID="btnNewVideo" runat="server" CssClass="NewBtn" OnClick="ImageButton2_Click" />
                        </asp:View>
                        <asp:View ID="vvYeniVideo" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSaveVideo">
                                <table>
                                    <tr>
                                        <td width="100">
                                            <%= AdminResource.lbVideoUrl %>
                                        </td>
                                        <td width="10">
                                            :
                                        </td>
                                        <td width="200">
                                            <asp:TextBox ID="txtLink" runat="server" MaxLength="500" Width="190px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLink"
                                                ForeColor="Red" SetFocusOnError="True" ValidationGroup="gk2">(!)</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbName %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVideoName" runat="server" MaxLength="250" Width="190px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVideoName"
                                                ForeColor="Red" ValidationGroup="gk2">(!)</asp:RequiredFieldValidator>
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
                                            <asp:TextBox ID="txtVideoDescription" runat="server" Height="100px" MaxLength="500"
                                                TextMode="MultiLine" Width="190px"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td valign="top">
                                            :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="videoState" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
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
                                            <asp:Button ID="btnSaveVideo" runat="server" CssClass="SaveCancelBtn" OnClick="btnSaveVideo_Click"
                                                ValidationGroup="gk2" />
                                            <asp:Button ID="btnCancelVideo" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancelVideo_Click" />
                                        </td>
                                        <td>
                                            &nbsp;
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
                    <asp:GridView ID="gvVideos" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        PageSize="15" AllowPaging="True" CssClass="GridViewStyle" PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                        SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                        SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                        SelectedRowStyle-CssClass="selected" DataKeyNames="id" ForeColor="#333333" GridLines="None"
                        Visible="False" DataSourceID="edsVideos" OnRowCommand="gvVideos_RowCommand" OnRowDataBound="gvVideos_RowDataBound"
                        Width="100%" AllowSorting="True">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="İşlemler">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                        CommandName="Update" CausesValidation="True" CommandArgument='<%#Eval("id") %>' />
                                    <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                        CommandName="Cancel" CausesValidation="False" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                        CommandName="Edit" CommandArgument='<%#Eval("id") %>' />
                                    <asp:ImageButton ID="lbAlbSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                        CausesValidation="False" OnClick="lbVideoSil_Click" CommandArgument='<%#Eval("id") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" SortExpression="Name">
                                <HeaderStyle HorizontalAlign="Left" Width="125px" />
                                <ItemStyle HorizontalAlign="Left" Width="125px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Description" SortExpression="Description">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle HorizontalAlign="Left" Width="150px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <a href='<%#Eval("videoURL") %>' rel="prettyPhoto">
                                        <%#Eval("videoURL") %></a>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbEditUrl" runat="server" Text='<%#Eval("videoURL") %>'></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategory" runat="server" Text='<%#                GetCategoryName(Convert.ToInt32(Eval("categoryId"))) %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="edsCat" DataTextField="name"
                                        DataValueField="id" SelectedValue='<%#Bind("categoryId") %>'>
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
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:EntityDataSource ID="edsVideos" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="Videos"
                        Where="it.categoryId=@catId">
                        <WhereParameters>
                            <asp:Parameter Name="catId" DbType="Int32" DefaultValue="0" />
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
