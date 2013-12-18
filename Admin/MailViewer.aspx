<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailViewer.aspx.cs" Inherits="eNroll.Admin.MailViewer" %>

<%@ Import Namespace="Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Admin/style/admin.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/admin/js/jquery.flot.min.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.flot.pie.min.js"></script>
    <style type="text/css">
        .jobsChart { float: left; }
    </style>
    <script>
        //******* PIE CHART
        var dataSet = [{
            label: "<%=AdminResource.lbRead%>",
            data: <%=hfReadCount.Value %>,
            color: "#4E81C9"
        },{
            label: "<%=AdminResource.lbNotRead%>",
            data: <%=hfNotReadCount.Value %>,
            color: "#337FED"
        },{
            label: "<%=AdminResource.lbNotSent %>",
            data: <%=hfNotSendCount.Value %>,
            color: "red"
        }];

        var options = {
            series: {
                pie: {
                    show: true,
                    label: {
                        show: true,
                        radius: 120,
                        formatter: function (label, series) {
                            return '<div style="border:1px solid grey;font-size:8pt;text-align:center;padding:5px;color:white;">' +
                                '' + label + ' : ' + Math.round(series.percent) + '%' +
                                '</div>';
                        },
                        background: {
                            opacity: 0.8,
                            color: '#000'
                        }
                    }
                }
            },
            legend: {
                show: true
            },
            grid: {
                hoverable: true
            }
        };

        $(document).ready(function () {
            $.plot($("#flot-placeholder"), dataSet, options);
        });

    </script>
</head>
<body style="width: 100%; background-color: #fff; background-image: none;">
    <form id="form1" runat="server" viewstatemode="Enabled">
    <div style="width: 100%;">
        <asp:Literal ID="FCliteral" runat="server"></asp:Literal>
        <asp:Literal runat="server" ID="ltMailContent" />
        <asp:Panel runat="server" ID="pTasksReportChart" Width="100%">
            <p>
                <h1 style="text-align: center;">
                    <%=GetTaskName() %></h1>
                <h3 style="text-align: center;">
                    <%=AdminResource.lbScheduledJobsStatusReport %></h3>
            </p>
            <div style="float: left; width: 55%">
                <fieldset style="float: right; border-style: solid; height: 200px; margin-right: 15px;">
                    <legend><%=AdminResource.lbGraphReport %></legend>
                    <div id="flot-placeholder" style="float: left; min-width: 420px; height: 200px;">
                    </div>
                </fieldset>
            </div>
            <div style="float: left; width: 45%">
                <fieldset style="float: left; border-style: solid; height: 200px; margin-left: 15px;">
                    <legend><%=AdminResource.lbFilterResults %></legend>
                    <table cellpadding="3" cellspacing="5">
                        <tr>
                            <td width="115px">
                                <%=AdminResource.lbState %>
                            </td>
                            <td width="10px">
                                :
                            </td>
                            <td>
                                <asp:DropDownList runat="server" Width="203px" ID="ddlFilterState" OnSelectedIndexChanged="ddlFilterState_OnSelectedIndexChanged"
                                    AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=AdminResource.lbFilterByEmail %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top">
                                            <asp:TextBox runat="server" Width="170px" Height="18" ID="tbFilterEmail"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="ibFilterEmail" ImageUrl="/Admin/images/icon/ara.png"
                                                OnClick="ibFilterEmail_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=AdminResource.lbEmailCount %>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbEmailCount"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button runat="server" ID="ibWriteResultExcel" CssClass="SaveCancelBtn" AlternateText="Excele Aktar"
                                    OnClick="IbWriteExcellClick" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pTasksReportList" Width="100%" CssClass="jobsChart">
            <br />
            <asp:GridView ID="gvJobReportList" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                SelectedRowStyle="selected" DataKeyNames="taskId" CellPadding="4" Width="100%"
                PageSize="15" ForeColor="#333333" GridLines="None" AllowSorting="False">
                <EmptyDataTemplate>
                    <%= AdminResource.lbNoRecord %>
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Surname" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Email" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%#(Convert.ToInt32(Eval("state")) != 0 ? AdminResource.lbSent : AdminResource.lbNotSent)%>,
                            <%#(Convert.ToInt32(Eval("state")) == 2 ? AdminResource.lbRead : AdminResource.lbNotKnown)%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="readDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <asp:Panel runat="server" ID="pTaskEdit" Width="100%">
            
        </asp:Panel>
    </div>
    <asp:HiddenField runat="server" ID="hfTaskId" />
    <asp:HiddenField runat="server" ID="hfSqlQuery" />
    <asp:HiddenField runat="server" ID="hfWhereParams" />
    <asp:HiddenField runat="server" ID="hfSearchParams" />
    <asp:HiddenField runat="server" ID="hfNotSendCount" />
    <asp:HiddenField runat="server" ID="hfReadCount" />
    <asp:HiddenField runat="server" ID="hfNotReadCount" />
    </form>
</body>
</html>
