<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CorporationAddEdit.ascx.cs" Inherits="eNroll.Admin.adminUserControls.CorporationAddEdit" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuthoriztn">
    <asp:View runat="server" ID="vAuth">
        <script type="text/javascript">
            function onlyNumber(event) {
                var keyCode = event.keyCode;
                if ((keyCode < 46 || keyCode > 57) && keyCode != 8 && keyCode != 9 &&
                    keyCode != 0 && keyCode != 47 && (keyCode < 96 || keyCode > 105)) return false;
                return true;
            }
            function Show(data) {
                document.getElementById("fAddress").style.display = "none";

                var control = document.getElementById(data);
                control.style.display = "block";
            }
            function Hide(data) {
                var control = document.getElementById(data);
                control.style.display = "none";
            }
        </script>

        <asp:MultiView runat="server" ID="mvOperations">
            <asp:View runat="server" ID="vAdd">
                <fieldset>
                    <legend><b><%= AdminResource.lbNewCorporation%></b></legend>
                    <table cellpadding="3" style="float: left;">
                        <tr>
                            <td style="width: 120px;">
                                <%= AdminResource.lbName%>
                            </td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpName" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rf" runat="server" ControlToValidate="tbNewCorpName"
                                    ForeColor="Red" ValidationGroup="vgNew" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td>Vergi Dairesi
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpTaxDept" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbNewCorpTaxDept"
                                    ForeColor="Red" ValidationGroup="vgNew" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top"><%= AdminResource.lbTaxNumber%>
                            </td>
                            <td valign="top">:
                            </td>
                            <td>
                                <asp:TextBox runat="server" onkeydown="return onlyNumber(event);" ID="tbNewCorpTaxNumber" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbNewCorpTaxNumber"
                                    ForeColor="Red" ValidationGroup="vgNew" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbDesc%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpDesc" Width="200px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="fNewCorpAddress">
                    <legend><b><%=AdminResource.lbAddress %></b></legend>
                    <table cellpadding="3">
                        <tr>
                            <td style="width: 110px;"><%=AdminResource.lbName %></td>
                            <td style="width: 10px;">:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpAddressTitle"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbNewCorpAddressTitle"
                                    ForeColor="Red" ValidationGroup="vgNew" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top"><%=AdminResource.lbDesc %></td>
                            <td valign="top">:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpAddressDesc" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbCountry%></td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlNewCorpCountry" AutoPostBack="True" OnSelectedIndexChanged="ddlNewCorpCountry_OnSelectedIndexChanged" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbCity %></td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlNewCorpCity" AutoPostBack="True" OnSelectedIndexChanged="ddlNewCorpCity_OnSelectedIndexChanged" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbTown %></td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlNewCorpTown" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbAddressDetail %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpDetailedAddress" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbZipCode %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpZipCode"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbFax %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpFax"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbPhone %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpPhone"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbEmail %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpEmail"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbWeb %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbNewCorpWeb"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Button ID="btnSaveNewCorp" runat="server" ValidationGroup="vgNew" CssClass="SaveCancelBtn" OnClick="btnSaveNewCorp_OnClick" />
                <asp:Button ID="btnCancelNewCorp" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancelCorporation_OnClick" />
            </asp:View>
            <asp:View runat="server" ID="vEdit">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button runat="server" ID="btnAddNewUsers" CssClass="SaveCancelBtn" OnClick="btnAddNewUsers_OnClick" />
                            <asp:Button runat="server" ID="btnAddNewAddress" CssClass="SaveCancelBtn" OnClick="btnAddNewAddress_OnClick" />
                        </td>
                    </tr>
                </table>
                <fieldset id="fEditCorporation">
                    <legend><b><%=AdminResource.lbCorporationInfo %></b></legend>
                    <table cellpadding="3" style="width: 49%; float: left;">
                        <tr>
                            <td style="width: 120px;">
                                <%= AdminResource.lbName%>
                            </td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbName" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbName"
                                    ForeColor="Red" ValidationGroup="vgCorp" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <%= AdminResource.lbDesc%>
                            </td>
                            <td valign="top">:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbDesc" Width="200px" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><%= AdminResource.lbTaxDept%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbTaxDept" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbTaxDept"
                                    ForeColor="Red" ValidationGroup="vgCorp" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td><%= AdminResource.lbTaxNumber%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox runat="server" onkeydown="return onlyNumber(event);" ID="tbTaxNumber" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbTaxNumber"
                                    ForeColor="Red" ValidationGroup="vgCorp" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td><%= AdminResource.lbState%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="cbState" Checked="True" />
                            </td>
                        </tr>
                        <tr>
                            <td><%= AdminResource.lbContactAddress%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlContactAddress" />
                                <asp:Button runat="server" ID="btEditAddress1" CssClass="SaveCancelBtn" OnClick="BtEditAddressClick" CommandArgument="Contact" />
                            </td>
                        </tr>
                        <tr>
                            <td><%= AdminResource.lbInvoiceAddress%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlInvoiceAddress" />
                                <asp:Button runat="server" ID="btEditAddress2" CssClass="SaveCancelBtn" OnClick="BtEditAddressClick" CommandArgument="Invoice" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnUpdateCorporation" runat="server" ValidationGroup="vgCorp" CssClass="SaveCancelBtn" OnClick="btnUpdateCorporation_OnClick" />
                                <asp:Button ID="btnUpdateFoundCorporation" runat="server" ValidationGroup="vgCorp" Visible="False" CssClass="SaveCancelBtn" OnClick="btnUpdateFoundCorporation_OnClick" />
                                <asp:Button ID="btnCancelCorporation" runat="server" CssClass="SaveCancelBtn" OnClick="btnCancelCorporation_OnClick" />
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="3" style="width: 49%; float: right;">
                        <tr>
                            <td width="100%">
                                <asp:GridView ID="gvCorporationUsers" runat="server" AllowSorting="False" AutoGenerateColumns="False"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="edsCorporationUser"
                                    CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                    SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                    EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                    PageSize="15" AllowPaging="True" OnRowDataBound="gvCorporationUsers_OnRowDataBound"
                                    Width="100%" OnRowCommand="gvCorporationUsers_OnRowCommand">
                                    <EmptyDataTemplate>
                                        <%= AdminResource.lbNoRecord %>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="35" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="../images/icon/cop.png"
                                                    CommandArgument='<%#Bind("UserId") %>' CommandName="Sil" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="35px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="35" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#GetUserName(Convert.ToInt32(Eval("UserId")))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="35" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%#GetUserEmail(Convert.ToInt32(Eval("UserId")))%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                <asp:EntityDataSource ID="edsCorporationUser" runat="server" ConnectionString="name=Entities"
                                    DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="CorporationUser" Where="it.CorporationId=@CorporationId">
                                    <WhereParameters>
                                        <asp:ControlParameter ControlID="hfCorporationId" Name="CorporationId" PropertyName="Value"
                                            DbType="Int32" />
                                    </WhereParameters>
                                </asp:EntityDataSource>
                            </td>
                        </tr>
                    </table>

                </fieldset>
                <fieldset id="fAddress" style="display: none;">
                    <legend><b><%=AdminResource.lbAddress %></b></legend>
                    <table width="100%">
                        <tr>
                            <td style="width: 110px;"><%=AdminResource.lbTitle %></td>
                            <td style="width: 10px;">:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbAddressTitle" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tbAddressTitle"
                                    ForeColor="Red" ValidationGroup="vgAddress" SetFocusOnError="True" ErrorMessage="!" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbDesc %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbAddressDesc" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbCountry %></td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_OnSelectedIndexChanged" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbCity %></td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCity" AutoPostBack="True" OnSelectedIndexChanged="ddlCity_OnSelectedIndexChanged" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbTown %></td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTown" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbAddressDetail %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbDetailedAddress" TextMode="MultiLine"/></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbZipCode %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbZipCode" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbFax %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbFax" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbPhone %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPhone" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbEmail %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbEmail" /></td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbWeb %></td>
                            <td>:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbWeb" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSaveUpdateAddress" runat="server" ValidationGroup="vgAddress" CssClass="SaveCancelBtn" OnClick="btnSaveUpdateAddress_OnClick" />
                                <asp:Button ID="btnCancelAddress" runat="server" CssClass="SaveCancelBtn" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="fUsers" style="display: none;">
                    <legend><b><%=AdminResource.lbAddUsers %></b></legend>
                    <asp:Panel ID="pSearch" runat="server" CssClass="Etiketler" DefaultButton="ImageButton2Ara">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="100px"><%=AdminResource.lbFilterUsers %></td>
                                <td width="10px">:</td>
                                <td style="width: 140px;">
                                    <asp:TextBox ID="txtAra" runat="server" Width="140px" Height="18px" /> 
                                </td>
                                <td style="width: 24px;">
                                    <asp:ImageButton ID="ImageButton2Ara" runat="server" ValidationGroup="vgUserSearch" ImageUrl="~/Admin/images/icon/ara.png"
                                        OnClick="IbFilterUsersClick" />
                                </td>
                                <td align="left" style="padding: 4px;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtAra"
                                        ForeColor="Red" ValidationGroup="vgUserSearch" SetFocusOnError="True" ErrorMessage="!" />
                                    <asp:Button ID="btnSaveUpdateUsers" runat="server" Width="58px" CssClass="SaveCancelBtn"
                                         OnClick="btnSaveUpdateUsers_OnClick" />
                                    <asp:Button ID="btnCancelUsers" runat="server" Width="58px" CssClass="SaveCancelBtn" OnClick="btnAddNewUsers_OnClick" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:CheckBoxList runat="server" ID="cbUsers" RepeatColumns="3" />
                                    <asp:Label runat="server" ID="lbUserNotFound"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </asp:View>
            <asp:View runat="server" ID="vList">
                <asp:Button runat="server" ID="btNew" OnClick="btNew_OnClick" CssClass="NewBtn" />
                <asp:GridView ID="gvCorporations" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="edsCorporations"
                    CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                    SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                    EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                    PageSize="15" AllowPaging="True" OnRowDataBound="gvCorporations_OnRowDataBound"
                    Width="100%" OnRowCommand="gvCorporations_OnRowCommand">
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
                                                CommandArgument='<%#Bind("Id") %>' CommandName="Guncelle" />
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
                        <asp:BoundField DataField="Name" SortExpression="Name">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#GetUsersCountOnCorporation(Convert.ToInt32(Eval("Id"))) %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="75px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Description" SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:EntityDataSource ID="edsCorporations" runat="server" ConnectionString="name=Entities"
                    DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Corporations">
                </asp:EntityDataSource>
            </asp:View>
        </asp:MultiView>
        <asp:HiddenField runat="server" ID="hfCorporationId" />
        <asp:HiddenField runat="server" ID="hfCorporationAddressId" />
        <asp:HiddenField runat="server" ID="hfCountry" />
        <asp:HiddenField runat="server" ID="hfCity" />
    </asp:View>
    <asp:View runat="server" ID="vNoAuth">
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