<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_SSS"
            CodeBehind="SSS.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register TagPrefix="uc1" TagName="Rtb" Src="~/Admin/adminUserControls/Rtb.ascx" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <table class="rightcontenttable">
            <tr>
                <td colspan="3">
                    <asp:MultiView ID="mvFaqCategories" runat="server">
                        <asp:View ID="vNewBtnFaqCat" runat="server">
                            <table class="rightcontenttable">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnNewCategory" runat="server" CssClass="NewBtn" OnClick="btnNewCategory_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vAddEditFaqCat" runat="server">
                            <asp:Panel runat="server" DefaultButton="btnSaveCat">
                                <table>
                                    <tr>
                                        <td width="100">
                                            <%= AdminResource.lbCategoryName %>
                                        </td>
                                        <td width="10">
                                            :
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="TextBoxKategoriAdi" runat="server" Width="250px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxKategoriAdi"
                                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g2">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100">
                                            <%= AdminResource.lbState %>
                                        </td>
                                        <td width="10">
                                            :
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="CheckBoxDurum" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbIndex %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxSiraNo" onkeydown="return onlyNumber(event);" runat="server"
                                                         Width="50px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="TextBoxSiraNo"
                                                                        ForeColor="Red" Display="Dynamic" ValidationGroup="g2">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSaveCat" runat="server" CssClass="SaveCancelBtn" OnClick="btnSaveCat_Click"
                                                        ValidationGroup="g2" />
                                            <asp:Button ID="btnCancelCat" runat="server" CssClass="SaveCancelBtn" OnClick="ImageButton3_Click" />
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
                <td colspan="3">
                    <asp:GridView ID="GridViewSSSKategoriler" runat="server" AutoGenerateColumns="False"
                                  PageSize="15" AllowPaging="True" CssClass="GridViewStyle" PagerStyle-CssClass="pgr"
                                  AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                                  SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                                  SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                  SelectedRowStyle="selected" DataKeyNames="faqCategoryId" Width="100%" CellPadding="4"
                                  ForeColor="#333333" OnRowDataBound="GridViewSSSKategoriler_onDataRowBound" GridLines="None"
                                  AllowSorting="True" DataSourceID="EntityDataSourceFaqCat" OnRowCommand="GridViewSSSKategoriler_RowCommand">
                        <EmptyDataTemplate>
                            <%= AdminResource.msgNotFoundCategory %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="BtnGuncelle" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                     CommandArgument='<%#Eval
                                                                                                   ("faqCategoryId") %>'
                                                     CommandName="Guncelle" />
                                    <asp:ImageButton ID="BtnDeleteCategory" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                     CommandArgument='<%#                                        Eval("faqCategoryId") %>'
                                                     CommandName="Sil" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbCatSec" runat="server" CausesValidation="False" CommandName="Select"
                                                    CommandArgument='<%#                                        Eval("faqCategoryId") %>' Text='<%#Bind("faqCategory") %>'
                                                    OnClick="lbCatSec_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="orderId" SortExpression="orderId">
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:BoundField>
                            <asp:CheckBoxField DataField="active" SortExpression="active">
                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                            </asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:MultiView ID="mvFaqs" runat="server">
                        <asp:View ID="vNewBtnFaq" runat="server">
                            <table class="rightcontenttable">
                                <tr>
                                    <td>
                                        <asp:Button ID="BtnAddNewFaq" runat="server" CssClass="NewBtn" OnClick="BtnAddNewFaq_Click"
                                                    Height="32px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vAddEditFaq" runat="server">
                            <asp:Panel runat="server" DefaultButton="BtnSaveQuest">
                                <table>
                                    <tr>
                                        <td width="100">
                                            <%= AdminResource.lbCategoryName %>
                                        </td>
                                        <td width="10">
                                            :
                                        </td>
                                        <td width="250">
                                            <asp:DropDownList ID="ddlAddNew" runat="server" Width="250px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlAddNew"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="g1" InitialValue="">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbQuestion %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBoxSoru" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="TextBoxSoru"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="g1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbAnsver %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td colspan="2">
                                            <uc1:Rtb ID="Rtb1" runat="server" />
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
                                            <asp:CheckBox ID="cbFaqDurum" runat="server" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%= AdminResource.lbIndex %>
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbIndex" runat="server" onkeydown="return onlyNumber(event);" Width="50px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tbIndex"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="g1">!</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnSaveQuest" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveQuest_Click"
                                                        ValidationGroup="g1" />
                                            <asp:Button ID="BtnCancelQuest" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCancelQuest_Click" />
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
                <td colspan="3">
                    <asp:GridView ID="GridViewSSS" runat="server" DataSourceID="EntityDataSourceSSS"
                                  AutoGenerateColumns="False" CssClass="GridViewStyle" PagerStyle-CssClass="pgr"
                                  AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                                  SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                                  SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                  SelectedRowStyle="selected" OnRowDataBound="GridViewSSS_OnRowDataBound" Width="100%"
                                  CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="GridViewSSS_RowCommand"
                                  PageSize="15" AllowPaging="True" AllowSorting="True">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="LinkButtonGuncelle" runat="server" CommandArgument='<%#Eval("faqId") %>'
                                                     ImageUrl="~/Admin/images/icon/edit.png" CommandName="Guncelle" />
                                    <asp:ImageButton ID="LinkButtonSil" runat="server" CommandArgument='<%#                                        Eval
                                            ("faqId") %>' ImageUrl="~/Admin/images/icon/cop.png"
                                                     CommandName="Sil" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="faqQuestion" SortExpression="faqQuestion">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="faqAnswer" SortExpression="faqAnswer">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="faqOrderId" SortExpression="faqOrderId">
                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:BoundField>
                            <asp:CheckBoxField DataField="active" SortExpression="active">
                                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                <ItemStyle HorizontalAlign="Left" Width="50px" />
                            </asp:CheckBoxField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <asp:EntityDataSource ID="EntityDataSourceFaqCat" runat="server" ConnectionString="name=Entities"
                              DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="FaqCategories"
                              Where="it.languageId=@languageId" OrderBy="it.orderId asc" EntityTypeFilter=""
                              Select="">
            <WhereParameters>
                <asp:ControlParameter ControlID="hfLanguageId" DbType="Int32" Name="languageId" PropertyName="Value" />
            </WhereParameters>
        </asp:EntityDataSource>
        <asp:EntityDataSource ID="EntityDataSourceSSS" runat="server" ConnectionString="name=Entities"
                              DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Faq"
                              Where="it.faqCategoryId=@catId" EntityTypeFilter="" Select="" OrderBy="it.faqOrderId asc">
            <WhereParameters>
                <asp:Parameter Name="catId" DbType="Int32" DefaultValue="0" />
            </WhereParameters>
        </asp:EntityDataSource>
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
<asp:HiddenField ID="hfLanguageId" runat="server" />
<asp:HiddenField ID="hfCategoryId" runat="server" />
<asp:HiddenField ID="hfFaqId" runat="server" />