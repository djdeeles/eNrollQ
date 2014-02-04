<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberFinanceManager.ascx.cs"
            Inherits="eNroll.Admin.adminUserControls.MemberFinanceManager" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="eNroll.Helpers" %>
<script type="text/javascript">
    function hideTable(control, data) {
        var table = document.getElementById(data);
        if (table.style.display == "none") {
            table.style.display = "table";
            control.attr("title", "<%= AdminResource.lbHide %>");
        } else {
            table.style.display = "none";
            control.attr("title", "<%= AdminResource.lbShow %>");
        }
    }
</script>
<style type="text/css">
    .deptAmount { text-align: right; }
</style>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View ID="vAuth" runat="server">
        <asp:MultiView runat="server" ID="mvFinanceManager">
            <asp:View runat="server" ID="vDept">
                <fieldset>
                    <legend><span style="cursor: pointer;" onclick=" hideTable($(this), 'tbChargeForDues') ">
                                <%= AdminResource.lbAddDeptMember %></span></legend>
                    <table id="tbChargeForDues">
                        <tr>
                            <td style="width: 100px;">
                                <%= AdminResource.lbDuesType %>
                            </td>
                            <td style="width: 10px;">
                                :
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlDuesType" AutoPostBack="True" OnSelectedIndexChanged="ddlDuesType_OnSelectedIndexChanged" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlDuesType"
                                                            ForeColor="Red" ValidationGroup="g1" InitialValue="" ErrorMessage="!">
                                </asp:RequiredFieldValidator>
                                <asp:Button runat="server" ID="btAddDues" CssClass="SaveCancelBtn" ValidationGroup="g1"
                                            OnClick="BtnAddDuesClick" />
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
                                <asp:Literal runat="server" ID="ltDues"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:GridView runat="server" ID="gvFinanceManagement" AllowSorting="False" AutoGenerateColumns="False"
                              CellPadding="4" ForeColor="#333333" GridLines="None" Visible="True" CssClass="GridViewStyle"
                              PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                              SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                              SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                              SelectedRowStyle="selected" PageSize="15" AllowPaging="True" OnRowDataBound="GvFinanceManagement_OnRowDataBound"
                              Width="100%">
                    <EmptyDataTemplate>
                        <%= AdminResource.lbNoRecord %>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnGoFinanceDetail" runat="server" OnClick="BtnGoFinanceDetailClick"
                                                        CssClass="SaveCancelBtn" CommandArgument='<%#Eval("finId") %>' />
                                        </td>
                                        <td width="5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGoTakePayment" runat="server" CommandArgument='<%#Eval("finId") %>'
                                                        CssClass="SaveCancelBtn" OnClick="BtnGoTakePaymentClick" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="220" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#                GetUserName(Convert.ToInt32(Eval("uId"))) %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#                EnrollMembershipHelper.DebtValue(Convert.ToDecimal(Eval("finDept"))) %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:View>
            <asp:View runat="server" ID="vFinanceDetail">
                <fieldset>
                    <legend>
                        <h1>
                            <b>
                                <asp:Label runat="server" ID="lbFDNameSurname"></asp:Label></b></h1>
                    </legend>
                    <div style="float: left; width: 20%; margin-right: 10px; margin-left: 5px;">
                        <asp:Image runat="server" ID="iUserPhoto" />
                    </div>
                    <table style="float: left;" width="70%" cellpadding="0" cellspacing="5">
                        <tr>
                            <td width="120px">
                                <%= AdminResource.lbRelationType %>
                            </td>
                            <td width="10px">
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbFDRelationship"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbMemberNo %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbFDCorporationNumber"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbGraduateDate %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbFDGraduationYear" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbCurrentDebtAmount %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <b>
                                    <asp:Label runat="server" ID="lbFDDeptAmount"></asp:Label></b>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend><span style="cursor: pointer;" onclick=" hideTable($(this), '<%= gvChargesForDues.ClientID %>') "
                                  title='<%= AdminResource.lbHide %>'>
                                <%= AdminResource.lbChargesForDues %>
                            </span></legend>
                    <asp:GridView runat="server" ID="gvChargesForDues" AllowSorting="False" AutoGenerateColumns="False"
                                  CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="edsChargesForDues"
                                  CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                  SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                  SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                  EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                  PageSize="15" AllowPaging="True" Width="100%">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="20" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <img width="20" src="../../Admin/images/icon/zoom.png" style="cursor: pointer;" onclick=" OpenHelpDialog(
    <%#                (Eval("LogType") != null && Convert.ToInt32(Eval("LogType")) == 0 ? "0" : "1") %>,
    <%#Eval("UserId").ToString() %>,
    '<%#Eval("Id").ToString() %>'); return false; " />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="20px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#                                        (Eval("LogType") != null && Convert.ToInt32(Eval("LogType")) == 0
                                             ? GetDuesType(Convert.ToInt32(Eval("DuesType")))
                                             : "") %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#                (Eval("LogType") != null && Convert.ToInt32(Eval("LogType")) == 1
                     ? GetPaymentType(Convert.ToInt32(Eval("PaymentTypeId")))
                     : "") %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#                EnrollMembershipHelper.AmountValue(Convert.ToDecimal(Eval("Amount")), Convert.ToInt32(Eval("LogType"))) %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#                Convert.ToDateTime(Eval("CreatedTime")).ToShortDateString() + " " +
                Convert.ToDateTime(Eval("CreatedTime")).ToShortTimeString() %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#                GetUserName(Convert.ToInt32(Eval("CreatedUser"))) %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#                (Convert.ToInt32(Eval("LogType")) == 0 ? AdminResource.lbDebiting : AdminResource.lbPayment) %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:EntityDataSource ID="edsChargesForDues" runat="server" ConnectionString="name=Entities"
                                          DefaultContainerName="Entities" EntitySetName="UserDuesLog" Where="it.UserId=@UserId"
                                          OrderBy="it.CreatedTime desc">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="hfMemberId" Name="UserId" DbType="Int32" />
                        </WhereParameters>
                    </asp:EntityDataSource>
                    <script type="text/javascript">
                        function OpenHelpDialog(process, userId, data) {
                            var wnd = $find("<%= rWDetail.ClientID %>");
                            var url = "FinanceViewer.aspx?type=0&process=" + process + "&userId=" + userId + "&data=" + data;
                            wnd.setUrl(url);
                            wnd.show();
                        }
                    </script>
                    <telerik:RadWindow runat="server" AutoSize="True" Width="330" VisibleStatusbar="false"
                                       ShowContentDuringLoad="false" ID="rWDetail" Modal="true" Behaviors="Close,Move,Resize,Maximize">
                    </telerik:RadWindow>
                    <asp:Button runat="server" ID="btnExportExcel" CssClass="SaveCancelBtn" OnClick="BtnExportExcel" />
                </fieldset>
                <br />
                <asp:Button runat="server" ID="btnBackToDept" CssClass="SaveCancelBtn" OnClick="BackToDept" />
            </asp:View>
            <asp:View runat="server" ID="vTakePayment">
                <fieldset>
                    <legend>
                        <h1>
                            <b>
                                <asp:Label runat="server" ID="lbPymtNameSurname" /></b></h1>
                    </legend>
                    <div style="float: left; width: 20%; margin-right: 10px; margin-left: 5px;">
                        <asp:Image runat="server" ID="iUserPhoto2" />
                    </div>
                    <table width="70%" cellpadding="0" cellspacing="5">
                        <tr>
                            <td width="120px;">
                                <%= AdminResource.lbRelationType %>
                            </td>
                            <td width="10px;">
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbPymtRelationship" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbMemberNo %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbPymtCorpNumber" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbGraduateDate %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbPymtGraduateDate" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbCurrentDebtAmount %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbPymtDeptAmount" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset>
                    <legend><span style="cursor: pointer;" onclick=" hideTable($(this), 'tbPaymentInfo') ">
                                <%= AdminResource.lbPaymentInfo %></span></legend>
                    <table width="100%" cellpadding="0" cellspacing="5" id="tbPaymentInfo">
                        <tr>
                            <td width="120px;">
                                <%= AdminResource.lbPaymentType %>
                            </td>
                            <td width="10px;">
                                :
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPymtPaymentType" Width="205px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPymtPaymentType"
                                                            ForeColor="Red" ValidationGroup="vldGroup1" InitialValue="" ErrorMessage="!">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbPaymentDate %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpPymtPaymentDate" runat="server" Width="230px">
                                </telerik:RadDatePicker>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dpPymtPaymentDate"
                                                            ForeColor="Red" ValidationGroup="vldGroup1" ErrorMessage="!">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbPaymentAmount %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <%= (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                              ? EnrollCurrency.SiteDefaultCurrencyUnit()
                              : "") %>
                                <asp:TextBox runat="server" CssClass="deptAmount" onkeydown="return priceInputsCharacters(event);"
                                             ID="tbPymtPaymentAmount" Width="200px" />
                                <%= (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                     : "") %>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbPymtPaymentAmount"
                                                            ForeColor="Red" ValidationGroup="vldGroup1" ErrorMessage="!">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbReceiptInvoiceNumber %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPymtReceiptNumber" Width="200px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPymtReceiptNumber"
                                                            ForeColor="Red" ValidationGroup="vldGroup1" ErrorMessage="!">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbReceiptInvoiceDate %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpPymtReceiptDate" runat="server" Width="230px">
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%= AdminResource.lbProvisionNumber %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPymtProvisionNo" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button runat="server" ID="btnBackToDept2" CssClass="SaveCancelBtn" OnClick="BackToDept" />
                                <asp:Button runat="server" ID="btPymtAddNewPayment" ValidationGroup="vldGroup1" CssClass="SaveCancelBtn"
                                            OnClick="BtPymtAddNewPaymentOnClick" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                &nbsp;
            </asp:View>
        </asp:MultiView>
        <asp:HiddenField runat="server" ID="hfMemberId" />
        <asp:HiddenField runat="server" ID="hfSqlQuery" />
        <asp:HiddenField runat="server" ID="hfDues" />
        <asp:HiddenField runat="server" ID="hfUserFinanceId" />
    </asp:View>
    <asp:View ID="vNotAuth" runat="server">
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