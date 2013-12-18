<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormManager.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.FormManager" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="MultiView2">
    <asp:View ID="View1" runat="server">
        <asp:Panel ID="pBtnAddNewForm" runat="server">
            <table class="rightcontenttable">
                <tr>
                    <td colspan="3">
                        <asp:Button ID="btNewForm" OnClick="btNewForm_Click" runat="server" CssClass="NewBtn" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pAddNewForm" runat="server" DefaultButton="btSaveNewForm">
            <table class="rightcontenttable">
                <tr>
                    <td width="75px">
                        <%= AdminResource.lbName %>
                    </td>
                    <td width="10px">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbFormName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbFormName"
                            ForeColor="Red" ValidationGroup="g1" SetFocusOnError="True">!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="75px">
                        <%= AdminResource.lbEmail %>
                    </td>
                    <td width="10px">
                        :
                    </td>
                    <td align="left">
                        <asp:TextBox runat="server" ID="tbEmail"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbEmail"
                            ForeColor="Red" ValidationGroup="g1" SetFocusOnError="True">!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="75px">
                        <%= AdminResource.lbState %>
                    </td>
                    <td width="10px">
                        :
                    </td>
                    <td align="left">
                        <asp:CheckBox ID="cbFormState" runat="server"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Button ID="btSaveNewForm" CssClass="SaveCancelBtn" OnClick="btSaveNewForm_Click"
                            ValidationGroup="g1" runat="server"></asp:Button>
                        <asp:Button ID="btCancelNewForm" CssClass="SaveCancelBtn" OnClick="btCancelNewForm_Click"
                            runat="server"></asp:Button>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pForms" runat="server">
            <table class="rightcontenttable">
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="gvForms" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                            PageSize="15" AllowPaging="True" SortedAscendingCellStyle-CssClass="sortasc"
                            SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                            SelectedRowStyle-CssClass="selected" DataKeyNames="Id" OnRowCommand="gvForms_RowCommand"
                            ForeColor="#333333" GridLines="None" Width="100%" OnRowDataBound="gvForms_RowDataBound"
                            DataSourceID="edsForms" AllowSorting="True">
                            <EmptyDataTemplate>
                                <%= AdminResource.lbNoRecord %>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                            CommandName="Update" CausesValidation="True" CommandArgument='<%#                                        Eval("Id") %>' />
                                        <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                            CommandName="Cancel" CausesValidation="False" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                            CommandName="Edit" CommandArgument='<%#Eval("Id") %>' />
                                        <asp:ImageButton ID="lbFormSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                            CausesValidation="False" OnClick="lbFormSil_Click" CommandArgument='<%#Eval("Id") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbFormSec" runat="server" CausesValidation="False" CommandName="Select"
                                            CommandArgument='<%#Eval("Id") %>' Text='<%#Bind("Name") %>' OnClick="lbFormSec_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbFormName" runat="server" Text='<%#Eval("Name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%#Eval("EmailAddress") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="tbEmailAddress" runat="server" Text='<%#Eval("EmailAddress") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:CheckBoxField DataField="State" SortExpression="State">
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                </asp:CheckBoxField>
                            </Columns>
                        </asp:GridView>
                        <asp:EntityDataSource ID="edsForms" runat="server" ConnectionString="name=Entities"
                            DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="Forms"
                            Where="it.LanguageId=@languageId" EnableFlattening="False" EntityTypeFilter="Forms">
                            <WhereParameters>
                                <asp:ControlParameter Name="languageId" ControlID="HiddenField1" PropertyName="Value"
                                    DbType="Int32" DefaultValue="1" />
                            </WhereParameters>
                        </asp:EntityDataSource>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pFormContents" runat="server">
            <asp:Panel runat="server">
                <table class="rightcontenttable">
                    <tr>
                        <td colspan="3">
                            <p style="font-size: 14px; font-weight: bold; margin: 0 0 10px 0;">
                                <asp:Label ID="lbSelectedFormName" runat="server" />
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:GridView ID="gvFormContents" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                PagerStyle-CssClass="pgr" CssClass="GridViewStyle" AlternatingRowStyle-CssClass="alt"
                                SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle-CssClass="selected"
                                CellPadding="0" GridLines="None" ShowHeader="True" OnRowCommand="gvFormContents_RowCommand"
                                OnRowDataBound="gvFormContents_RowDataBound" DataSourceID="dataSourceGvFormContents">
                                <Columns>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="btnUpdateContentForm" runat="server" ImageUrl="~/Admin/images/icon/save.png"
                                                CommandName="Update" CausesValidation="True" CommandArgument='<%#Eval("Id") %>' />
                                            <asp:ImageButton ID="LinkButton2" runat="server" ImageUrl="~/Admin/images/icon/cancel.png"
                                                CommandName="Cancel" CausesValidation="False" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                CommandName="Edit" CommandArgument='<%#Eval("Id") %>' />
                                            <asp:ImageButton ID="gvFormContentsSil" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                CausesValidation="False" OnClick="gvFormContentsSil_Click" CommandArgument='<%#Eval("Id") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%#Eval("OrderId") %>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="tbOrderId" runat="server" Text='<%#Eval("OrderId") %>' />
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:CheckBoxField DataField="Required" SortExpression="Required">
                                        <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:CheckBoxField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div <%#((Convert.ToInt32(Eval("FieldType")) == 4 || 
                                                Convert.ToInt32(Eval("FieldType")) == 5 || Convert.ToInt32(Eval("FieldType")) == 6) ? 
                                                "style='margin-left:10px;'" : "style='display:none;'") %>>
                                                <asp:ImageButton ID="imgBtnAddNewOption" Width="16px" ImageUrl="~/Admin/images/icon/arti.png"
                                                    CommandName="AddNewOption" CommandArgument='<%#Container.DataItemIndex + "|" + Eval("Id") %>'
                                                    runat="server" />
                                                <%#GetFormContentElementsDeleteButton(Convert.ToInt32(Eval("Id")), Convert.ToInt32(Eval("FieldType"))) %>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%#Eval("FieldName") %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="FormItem" />
                                        <EditItemTemplate>
                                            <asp:TextBox ID="tbFormContentName" runat="server" Text='<%#Eval("FieldName") %>'
                                                />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            :
                                        </ItemTemplate>
                                        <ItemStyle CssClass="FormItem" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%#                GetFormContentElement(Convert.ToInt32(Eval("Id")), Convert.ToInt32(Eval("FieldType"))) %>
                                            <asp:TextBox ID="txtNewOption" Visible="False" runat="server" onkeydown="return cancelEnterKey(event,this);"  />
                                            <asp:Button ID="btnSaveOption" Visible="False" CssClass="SaveCancelBtn" runat="server"
                                                CommandName="SaveAddNewOption" CommandArgument='<%#                Container.DataItemIndex + "|" + Eval("Id") %>' />
                                            <asp:Button ID="btnCancelOption" Visible="False" CssClass="SaveCancelBtn" runat="server"
                                                CommandName="CancelAddNewOption" CommandArgument='<%#Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="FormItem" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:EntityDataSource ID="dataSourceGvFormContents" runat="server" ConnectionString="name=Entities"
                                DefaultContainerName="Entities" EnableDelete="True" EnableUpdate="True" EntitySetName="FormContents"
                                Where="it.FormId=@formId" EnableFlattening="False" EntityTypeFilter="FormContents"
                                OrderBy="it.OrderId ASC">
                                <WhereParameters>
                                    <asp:ControlParameter Name="formId" ControlID="hfFormId" PropertyName="Value" DbType="Int32"
                                        DefaultValue="1" />
                                </WhereParameters>
                            </asp:EntityDataSource>
                            <asp:HiddenField ID="hfFormId" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" DefaultButton="btSaveFormContent">
                <table class="rightcontenttable">
                    <tr>
                        <td width="75px">
                            <%= AdminResource.lbNewField %>
                        </td>
                        <td width="10px">
                            :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbFieldName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbFieldName"
                                ForeColor="Red" ValidationGroup="g2" SetFocusOnError="True">!</asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlFieldType" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbIndex %>
                        </td>
                        <td>
                            :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBoxSiraNo" runat="server" onkeydown="return onlyNumber(event);"
                                Width="50px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TextBoxSiraNo"
                                Display="Dynamic" ForeColor="Red" ValidationGroup="g2">!</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbRequiredField %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox ID="cbRequiredField" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button ID="btSaveFormContent" CssClass="SaveCancelBtn" OnClick="btSaveFormContent_Click"
                                ValidationGroup="g2" runat="server"></asp:Button>
                            <asp:Button ID="btCancelFormContent" CssClass="SaveCancelBtn" OnClick="btCancelFormContent_Click"
                                runat="server"></asp:Button>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </asp:View>
    <asp:View ID="View2" runat="server">
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