<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CorporationFinanceLogs.ascx.cs" Inherits="eNroll.Admin.adminUserControls.CorporationFinanceLogs" %>
<%@ Import Namespace="eNroll.Helpers" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="~/Admin/adminUserControls/CorporationInvoicement.ascx" TagName="CorporationInvoicement"
    TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server" ID="vAuth">
        <script type="text/javascript">
            function ShowHide(table, control) {
                var fTable = document.getElementById(control);
                if (fTable.style.display == "none") {
                    fTable.style.display = "table";
                    table.attr("title", "<%=AdminResource.lbHide %>");
                } else {
                    fTable.style.display = "none";
                    table.attr("title", "<%=AdminResource.lbShow %>");
                }
            }
        </script>
        <asp:Panel runat="server" ID="pServiceInvoicing" Visible="False">
            <uc1:CorporationInvoicement runat="server" ID="cCorporationInvoicement" /> 
        </asp:Panel>
        <asp:Panel ID="pFilter" runat="server" DefaultButton="ImageButton2Ara"> 
            <fieldset style="width: 97%">
                <legend>
                    <span style="cursor: pointer;" onclick="ShowHide($(this),'dFilter');"><b><%=AdminResource.lbFilterOptions %></b>
                    </span>
                </legend>
                <div id="dFilter">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="right">
                                <asp:TextBox ID="txtAra" runat="server" Height="18px" />
                            </td>
                            <td align="right" width="15px;">
                                <asp:ImageButton ID="ImageButton2Ara" runat="server" ImageUrl="~/Admin/images/icon/ara.png"
                                    OnClick="ImageButton2Ara_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr style="font-weight:bold;">
                            <td align="left" valign="bottom"><%=AdminResource.lbCorporationName%>:</td>
                            <td align="left" valign="bottom"><%=AdminResource.lbState%>:</td>
                            <td align="left" valign="bottom"><%=AdminResource.lbProcess%>:</td>
                            <td align="left" valign="bottom"><%=AdminResource.lbInvoiceDate + "-" + AdminResource.lbStart%>:</td>
                            <td align="left" valign="bottom"><%=AdminResource.lbInvoiceDate + "-" + AdminResource.lbEnd%>:</td>
                            <td align="left" valign="bottom"><%=AdminResource.lbProcessDate + "-" + AdminResource.lbStart%>:</td>
                            <td align="left" valign="bottom"><%=AdminResource.lbProcessDate + "-" + AdminResource.lbEnd%>:</td>
                        </tr> 
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFilterCorporation" AutoPostBack="True" Width="80px" OnSelectedIndexChanged="ddlFilterCorporation_OnSelectedIndexChanged" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFilterInvoice" AutoPostBack="True" Width="80px" OnSelectedIndexChanged="ddlFilterInvoice_OnSelectedIndexChanged" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlProcess" AutoPostBack="True" Width="80px" OnSelectedIndexChanged="ddlProcess_OnSelectedIndexChanged" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpStartDate" Width="110px" runat="server" OnSelectedDateChanged="dpStartEndDate_OnSelectedDateChanged"
                                    MaxDate="01-01-2200" MinDate="01-01-1900" ZIndex="30001" AutoPostBack="True" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpEndDate" Width="110px" runat="server" OnSelectedDateChanged="dpStartEndDate_OnSelectedDateChanged"
                                    MaxDate="01-01-2200" MinDate="01-01-1900" ZIndex="30001" AutoPostBack="True" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpProccesStartDate" Width="110px" runat="server" OnSelectedDateChanged="dpStartEndDate_OnSelectedDateChanged"
                                    MaxDate="01-01-2200" MinDate="01-01-1900" ZIndex="30001" AutoPostBack="True" />
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dpProccesEndDate" Width="110px" runat="server" OnSelectedDateChanged="dpStartEndDate_OnSelectedDateChanged"
                                    MaxDate="01-01-2200" MinDate="01-01-1900" ZIndex="30001" AutoPostBack="True" />
                            </td>
                        </tr> 
                    </table>
                </div>
            </fieldset>
        </asp:Panel>
        <div style="width: 100%;">
            <asp:Panel ID="pnlNewList" runat="server">
                <asp:GridView runat="server" ID="gvChargesForDues" AutoGenerateColumns="False"
                    CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvChargesForDues_OnPageIndexChanging"
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
                                <table>
                                    <tr>
                                        <td>
                                            <img width="20" src="../../Admin/images/icon/zoom.png" style="cursor: pointer;" onclick="OpenHelpDialog(
        <%#(Eval("LogType") != null && Convert.ToInt32(Eval("LogType")) == 0 ? "0":"1")%>,
        <%#Eval("CorporationId").ToString()%>, '<%#Eval("Id").ToString()%>'); return false;" />
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="IbDeleteInvoice" ImageUrl="~/Admin/images/icon/cop.png" OnClick="IbDeleteInvoiceClick" 
                                                Visible='<%#Convert.ToInt32(Eval("LogType"))==0 && Convert.ToBoolean(Eval("IsInvoiced"))%>' CommandArgument='<%#Bind("Id") %>' />
                                            <asp:ImageButton runat="server" ID="IbDeleteLog" ImageUrl="~/Admin/images/icon/cop.png" OnClick="IbDeleteLogClick"
                                                Visible='<%#Convert.ToInt32(Eval("LogType"))==0 && !Convert.ToBoolean(Eval("IsInvoiced"))%>' CommandArgument='<%#Bind("Id") %>' />
                                            <asp:ImageButton runat="server" ID="IbDeletePayment" ImageUrl="~/Admin/images/icon/cop.png" OnClick="IbDeletePaymentClick"
                                                Visible='<%#Convert.ToInt32(Eval("LogType"))==1 && !Convert.ToBoolean(Eval("IsInvoiced"))%>' CommandArgument='<%#Bind("Id") %>' />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="140" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#GetCorporationName(Convert.ToInt32(Eval("CorporationId")))%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="140px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="30" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#(Convert.ToBoolean(Eval("IsInvoiced")) ? Eval("ReceiptNo") :"") %>
                                <asp:Button runat="server" ID="btnInvoicing" OnClick="btnInvoicing_OnClick"
                                    Visible='<%#(!Convert.ToBoolean(Eval("IsInvoiced"))&& Convert.ToInt32(Eval("LogType"))==0)%>' CommandArgument='<%#Eval("Id")%>' CssClass="SaveCancelBtn" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="55" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#(Convert.ToBoolean(Eval("IsInvoiced"))&& Convert.ToInt32(Eval("LogType"))==0 
                                    ? ((Eval("ReceiptDate")!=null && !string.IsNullOrEmpty(Eval("ReceiptDate").ToString())) 
                                            ? Convert.ToDateTime(Eval("ReceiptDate")).ToString("dd/MM/yyyy")
                                            : "")
                                    :((Eval("PaymentDate")!=null && !string.IsNullOrEmpty(Eval("PaymentDate").ToString())) 
                                            ? Convert.ToDateTime(Eval("PaymentDate")).ToString("dd/MM/yyyy")
                                            : ""))
                                %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="55px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#Eval("Service") %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="40" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%#EnrollMembershipHelper.AmountValue(Convert.ToDecimal(Eval("Amount")), Convert.ToInt32(Eval("LogType")))%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="70" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#Convert.ToDateTime(Eval("CreatedTime")).ToShortDateString() + " " + 
                            Convert.ToDateTime(Eval("CreatedTime")).ToShortTimeString()%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
            </asp:Panel>
        </div>
        <asp:HiddenField runat="server" ID="hfFilterCorporation" />
        <asp:HiddenField runat="server" ID="hfFilterProcces" />
        <asp:HiddenField runat="server" ID="hfFilterInvoice" />

        <asp:HiddenField runat="server" ID="hfCorporationLogId" />
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
