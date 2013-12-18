<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CorporationFinanceManager.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.CorporationFinanceManager" %>
<%@ Register Src="~/Admin/adminUserControls/CorporationInvoicement.ascx" TagName="CorporationInvoicement"
    TagPrefix="uc1" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="eNroll.Helpers" %>
<script type="text/javascript">
    function hideTable(control, data) {
        var table = document.getElementById(data);
        if (table.style.display != "none") {
            table.style.display = "none";
            control.attr("title", "<%=AdminResource.lbShow %>"); 
        } else {
            table.style.display = "table";
            control.attr("title", "<%=AdminResource.lbHide %>");
        }
    }
    function ShowHide(data) {
        var table = document.getElementById(data);
        if (table.style.display != "none") {
            table.style.display = "none";
        } else {
            table.style.display = "table";
        }
    }

    var totalDept = 0;
    function calculateDeptWithTax() {
        $.ajax({
            type: "POST",
            url: "FinanceViewer.aspx/CalculateDeptWithTax",
            data: "{  debt:'" + document.getElementById('<%=tbDeptAmount.ClientID%>').value + "'," +
                    "   tax:'" + document.getElementById('<%=ddlTaxType.ClientID%>').value + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                totalDept = msg.d;
                document.getElementById('spnDebtWithTax').innerHTML = msg.d;
            }
        });
        return "";
    }

    function confirmDeptWithTax() {
        if (document.getElementById('<%=tbDeptAmount.ClientID%>').value == "" || document.getElementById('<%=tbDeptAmount.ClientID%>').value == null) return;
        if (document.getElementById('<%=ddlTaxType.ClientID%>').value == "" || document.getElementById('<%=ddlTaxType.ClientID%>').value == null) return;

        return confirm('<%=AdminResource.confirmMessageInvoicing%>'.replace("{0}", totalDept));
    }

</script>
<style type="text/css">
    .deptAmount {
        text-align: right;
    }
</style>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View ID="vAuth" runat="server">
        <asp:MultiView runat="server" ID="mvFinanceManager">
            <asp:View runat="server" ID="vDept">
                <fieldset id="fAddDept">
                    <legend><span style="cursor: pointer;" onclick="hideTable($(this),'tbChargeForDues')">
                        <%=AdminResource.lbAddDeptCorporation %></span></legend>
                    <table id="tbChargeForDues">
                        <tr>
                            <td style="width: 70px;">
                                <%=AdminResource.lbAmount%>
                            </td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <%=(EnrollCurrency.SiteDefaultCurrency().Position == "L" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "") %>
                                <asp:TextBox runat="server" ID="tbDeptAmount" onchange="calculateDeptWithTax();return false;"
                                    onkeydown="return priceInputsCharacters(event,this);" Width="75px" CssClass="deptAmount" />
                                <%=(EnrollCurrency.SiteDefaultCurrency().Position == "R" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "") %>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbDeptAmount"
                                    ForeColor="Red" ValidationGroup="g1" ErrorMessage="!" SetFocusOnError="True">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px;">
                                <%=AdminResource.lbTaxType%>
                            </td>
                            <td style="width: 10px;">:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlTaxType" onchange="calculateDeptWithTax();" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" onchange="calculateDeptWithTax();return false;"
                                    ControlToValidate="ddlTaxType"
                                    ForeColor="Red" ValidationGroup="g1" ErrorMessage="!" SetFocusOnError="True" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbService %></td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlServiceTypes" Width="170px" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlServiceTypes"
                                    ForeColor="Red" ValidationGroup="g1" ErrorMessage="!" SetFocusOnError="True" InitialValue="">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top"><%=AdminResource.lbDesc%></td>
                            <td valign="top">:</td>
                            <td>
                                <asp:TextBox runat="server" ID="tbAddDeptDescription" TextMode="MultiLine" Width="170px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                            <td>
                                <asp:Button runat="server" ID="btAddDues" CssClass="SaveCancelBtn" ValidationGroup="g1"
                                    OnClick="BtnAddDuesClick" OnClientClick="if(Page_ClientValidate()) return confirmDeptWithTax();" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>
                                <span id="spnDebtWithTax" />
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
                                        <td width="5px">&nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGoPayment" runat="server" CommandArgument='<%#Eval("finId") %>'
                                                CssClass="SaveCancelBtn" OnClick="BtnGoPaymentClick" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="220" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#Eval("cName")%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#GetCorporationAddress(Convert.ToInt32(Eval("cContactId")))%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#GetCorporationAddress(Convert.ToInt32(Eval("cIncoiceId")))%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#EnrollMembershipHelper.DebtValue(Convert.ToDecimal(Eval("finDept")))%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:View>
            <asp:View runat="server" ID="vFinanceDetail">
                <fieldset id="fFinanceDetail">
                    <legend>
                        <span style="cursor: pointer;" onclick="hideTable($(this),'tblFinanceDetail')"
                            title='<%=AdminResource.lbHide %>'><b><%=AdminResource.lbAboutCorporationInfo %></b>
                        </span>
                    </legend>
                    <table id="tblFinanceDetail" style="float: left;" width="70%" cellpadding="0" cellspacing="5">
                        <tr>
                            <td width="120px"><%=AdminResource.lbCorporation%></td>
                            <td width="10px">:
                            </td>
                            <td><b>
                                <asp:Label runat="server" ID="lbCorporationName" /></b>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbDesc%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbCorporationDesc" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbTaxDept%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbTaxDept" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbTaxNumber%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbTaxNo" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbContactAddress%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbContactAddress" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbInvoiceAddress%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbInvoiceAddress" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=AdminResource.lbCurrentDebtAmount %>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <b>
                                    <asp:Label runat="server" ID="lbFDDeptAmount"></asp:Label></b>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Panel runat="server" ID="pServiceInvoicing" Visible="False">
                    <uc1:CorporationInvoicement runat="server" ID="cCorporationInvoicement" /> 
                </asp:Panel>
                <fieldset id="FinansGecmisi">
                    <legend><span style="cursor: pointer;" onclick="hideTable($(this),'<%=gvChargesForDues.ClientID %>')"
                        title='<%=AdminResource.lbHide %>'>Finans Geçmişi
                    </span></legend>
                    <asp:GridView runat="server" ID="gvChargesForDues" AllowSorting="False" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="edsChargesForDues"
                        CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                        SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                        EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                        PageSize="15" AllowPaging="True" Width="100%" OnRowDataBound="gvChargesForDues_OnRowDataBound">
                        <EmptyDataTemplate>
                            <%= AdminResource.lbNoRecord %>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="20" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <img width="20" src="../../Admin/images/icon/zoom.png" style="cursor: pointer;" onclick="OpenHelpDialog(
                                        <%#(Eval("LogType") != null && Convert.ToInt32(Eval("LogType")) == 0 ? "0":"1")%>,
                                        <%#Eval("CorporationId").ToString()%>, 
                                        '<%#Eval("Id").ToString()%>'); return false;" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="20px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#(Convert.ToBoolean(Eval("IsInvoiced")) ? Eval("ReceiptNo") :"") %>
                                    <asp:Button runat="server" ID="btnInvoicing" OnClick="btnInvoicing_OnClick"
                                        Visible='<%#(!Convert.ToBoolean(Eval("IsInvoiced")) && Convert.ToInt32(Eval("LogType"))==0)%>' CommandArgument='<%#Eval("Id")%>' CssClass="SaveCancelBtn" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#(Convert.ToInt32(Eval("LogType"))==0
                                        ? ((Eval("ReceiptDate")!=null) ? 
                                            Convert.ToDateTime(Eval("ReceiptDate")).ToString("dd/MM/yyyy"): "")
                                        :
                                        ((Eval("PaymentDate")!=null) ? 
                                        Convert.ToDateTime(Eval("PaymentDate")).ToString("dd/MM/yyyy"): "")) %> 
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#EnrollMembershipHelper.AmountValue(Convert.ToDecimal(Eval("Amount")), Convert.ToInt32(Eval("LogType")))%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Convert.ToDateTime(Eval("CreatedTime")).ToShortDateString() + " " + 
                            Convert.ToDateTime(Eval("CreatedTime")).ToShortTimeString()%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#GetUserName(Convert.ToInt32(Eval("CreatedUser")))%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#(Convert.ToInt32(Eval("LogType")) == 0 ? AdminResource.lbDebiting : AdminResource.lbPayment)%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:EntityDataSource ID="edsChargesForDues" runat="server" ConnectionString="name=Entities"
                        DefaultContainerName="Entities" EntitySetName="CorporationDuesLog" Where="it.CorporationId=@CorporationId"
                        OrderBy="it.CreatedTime desc">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="hfCorporationId" Name="CorporationId" DbType="Int32" />
                        </WhereParameters>
                    </asp:EntityDataSource>
                    <script type="text/javascript">
                        function OpenHelpDialog(process, corporationId, data) {
                            var wnd = $find("<%= rWDetail.ClientID %>");
                            var url = "FinanceViewer.aspx?type=1&process=" + process + "&corporationId=" + corporationId + "&data=" + data;
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
            <asp:View runat="server" ID="vPayment">
                <fieldset id="fCorporationInfo">
                    <legend>
                        <h1>
                            <b>
                                <asp:Label runat="server" ID="lbCorpName" /></b></h1>
                    </legend>
                    <table id="tbFinanceDetail" style="float: left;" width="70%" cellpadding="0" cellspacing="5">
                        <tr>
                            <td width="120px"><%=AdminResource.lbCorporation%></td>
                            <td width="10px">:
                            </td>
                            <td><b>
                                <asp:Literal runat="server" ID="ltCorporationName" /></b>
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbDesc%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltCorporationDesc" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbTaxDept%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltTaxDept" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbTaxNumber%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltTaxNo" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbContactAddress%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltContactAddres" />
                            </td>
                        </tr>
                        <tr>
                            <td><%=AdminResource.lbInvoiceAddress%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="ltInvoiceAddres" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=AdminResource.lbCurrentDebtAmount %>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <b>
                                    <asp:Literal runat="server" ID="ltFDDeptAmount"></asp:Literal></b>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="fPayment">
                    <legend><span style="cursor: pointer;" onclick="hideTable($(this),'tbPaymentInfo')">
                        <%=AdminResource.lbPaymentInfo%></span></legend>
                    <table width="100%" cellpadding="0" cellspacing="5" id="tbPaymentInfo">
                        <tr>
                            <td width="120px;">
                                <%=AdminResource.lbPaymentType%>
                            </td>
                            <td width="10px;">:
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
                                <%=AdminResource.lbPaymentDate%>
                            </td>
                            <td>:
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
                                <%=AdminResource.lbPaymentAmount%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <%=(EnrollCurrency.SiteDefaultCurrency().Position == "L" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "") %>
                                <asp:TextBox runat="server" CssClass="deptAmount" onkeydown="return priceInputsCharacters(event,this);"
                                    ID="tbPymtPaymentAmount" Width="200px" />
                                <%=(EnrollCurrency.SiteDefaultCurrency().Position == "R" ? EnrollCurrency.SiteDefaultCurrencyUnit() : "") %>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbPymtPaymentAmount"
                                    ForeColor="Red" ValidationGroup="vldGroup1" ErrorMessage="!">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr> 
                        <tr>
                            <td>
                                <%=AdminResource.lbProvisionNumber%>
                            </td>
                            <td>:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPymtProvisionNo" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <%=AdminResource.lbNote%>
                            </td>
                            <td valign="top">:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPymtNote" Width="200px" TextMode="MultiLine" />
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
        <asp:HiddenField runat="server" ID="hfCorporationId" />
        <asp:HiddenField runat="server" ID="hfSqlQuery" />
        <asp:HiddenField runat="server" ID="hfCorporationFinanceId" />
        <asp:HiddenField runat="server" ID="hfCorporationLogId" />
        <asp:HiddenField runat="server" ID="hfDebtAmountWithTax" />
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