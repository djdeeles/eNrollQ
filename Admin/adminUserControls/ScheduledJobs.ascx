<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduledJobs.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.ScheduledJobs" %>
<%@ Register Src="~/Admin/adminUserControls/MemberSendEmail.ascx" TagName="EditTask"
    TagPrefix="uc1" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="eNroll.Helpers" %>
<asp:MultiView runat="server" ID="mvAuthoriztn" >
    <asp:View runat="server" ID="vAuth">
        <asp:MultiView runat="server" ID="mvScheduledJobs">
            <asp:View runat="server" ID="vGridViewScheduledJobs">
                <asp:GridView ID="gVScheduledJobs" runat="server" AutoGenerateColumns="False" DataSourceID="edsTask"
                    CssClass="GridViewStyle" OnRowDataBound="gVSchduledJobs_OnRowDataBound" PagerStyle-CssClass="pgr"
                    AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                    SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                    SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                    SelectedRowStyle="selected" DataKeyNames="taskId" CellPadding="4" Width="100%"
                    PageSize="15" ForeColor="#333333" GridLines="None" AllowSorting="True">
                    <EmptyDataTemplate>
                        <%= AdminResource.lbNoRecord %>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                    OnClick="ImgBtnMemberEditClick" CommandArgument='<%#Bind("taskId") %>' />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgBtnTaskDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                CommandArgument='<%#Bind("taskId") %>' OnClick="ImgBtnTaskDeleteClick" />
                                        </td>
                                        <td>
                                            <img class="icon" style="cursor: pointer;" src="/Admin/images/icon/detay.png" alt='<%=AdminResource.lbShowMail %>'
                                                onclick="OpenHelpDialog(1, '<%#Eval("taskId").ToString()%>');return false;" title='<%=AdminResource.lbShowMail %>' />
                                        </td>
                                        <td <%#HaveTaskReport(Eval("taskId").ToString())%>>
                                            <img class="icon" style="cursor: pointer;" src="/Admin/images/icon/grafik.png" alt='<%=AdminResource.lbShowReports %>'
                                                onclick="OpenHelpDialog(2, '<%#Eval("taskId").ToString()%>');return false;" title='<%=AdminResource.lbShowReports%>' />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Subject" SortExpression="Subject" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#(Convert.ToInt32(Eval("Type"))==0 ? AdminResource.lbEmail : AdminResource.lbSms)%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#(Convert.ToDateTime(Eval("StartDate")).ToShortDateString() + " " + 
                                Convert.ToDateTime(Eval("StartDate")).ToShortTimeString())%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="110px" />
                            <ItemStyle HorizontalAlign="Left" Width="110px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                
                                <%#(Convert.ToInt32(Eval("State"))==2 ? AdminResource.lbSent: (Convert.ToInt32(Eval("State"))==1 ?
                                                                    AdminResource.lbJobCreated : AdminResource.lbWaiting))%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                            <ItemStyle HorizontalAlign="Left" Width="75px"/>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:EntityDataSource ID="edsTask" runat="server" ConnectionString="name=Entities"
                    DefaultContainerName="Entities" EnableFlattening="False" EntitySetName="Task">
                </asp:EntityDataSource>
                <div>
                    <script type="text/javascript">
                        function OpenHelpDialog(proccess, data) {
                            var wnd = $find("<%= rWHelp.ClientID %>");
                            var url = "MailViewer.aspx?proccess=" + proccess + "&task=" + data;
                            wnd.setUrl(url);
                            wnd.show();
                        }
                    </script>
                    <telerik:RadWindow runat="server" AutoSize="False" Height="600" Width="850" VisibleStatusbar="false"
                        ShowContentDuringLoad="false" ID="rWHelp" Modal="true" Behaviors="Close,Move,Resize,Maximize">
                    </telerik:RadWindow>
                </div>
            </asp:View>
            <asp:View ID="vEditTask" runat="server">
                <uc1:EditTask runat="server" ID="cEditTask" />
                <asp:Button runat="server" ID="btCancelEditTask" CssClass="SaveCancelBtn" Visible="False"
                    OnClick="BtCancelEditTaskClick" />
            </asp:View>
        </asp:MultiView>
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