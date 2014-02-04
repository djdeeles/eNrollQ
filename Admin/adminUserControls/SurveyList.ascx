<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_SurveyList"
            CodeBehind="SurveyList.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <asp:Panel ID="pnlSurveyOptionEdit" runat="server" Visible="False" DefaultButton="btnSaveUpdateSurveyOption">
            <table class="rightcontenttable">
                <tr>
                    <td style="width: 100px;">
                        <%= AdminResource.lbSurveyAnsver %>
                    </td>
                    <td style="width: 10px;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtOption" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOption"
                                                    ForeColor="Red" ValidationGroup="vldGroup1">(!)</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= AdminResource.lbCounter %>
                    </td>
                    <td>
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtChooseCount" runat="server" Width="39px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSaveUpdateSurveyOption" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                                    OnClick="btnSaveUpdateSurveyOption_Click" />
                        <asp:Button ID="btnEditCancelSurveyOption" runat="server" CssClass="SaveCancelBtn"
                                    OnClick="btnEditCancelSurveyOption_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlSurveyEdit" runat="server" Visible="False" DefaultButton="btnSurveySaveUpdate">
            <table class="rightcontenttable">
                <tr>
                    <td style="width: 100px;">
                        <%= AdminResource.lbSurveyQuestion %>
                    </td>
                    <td style="width: 10px;">
                        :
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuestion" runat="server" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rf" runat="server" ControlToValidate="txtQuestion"
                                                    ForeColor="Red" ValidationGroup="vldGroup1">(!)</asp:RequiredFieldValidator>
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
                        <asp:CheckBox ID="chkState" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSurveySaveUpdate" runat="server" CssClass="SaveCancelBtn" ValidationGroup="vldGroup1"
                                    OnClick="btnSurveySaveUpdateClick" />
                        <asp:Button ID="btnSurveyEditCancel" runat="server" CssClass="SaveCancelBtn" OnClick="btnSurveyEditCancelClick" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vldGroup1" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlList" runat="server">
            <table class="rightcontenttable">
                <tr>
                    <td>
                        <asp:Button ID="BtnNew" runat="server" CssClass="NewBtn" OnClick="btnNewSurvey" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gVSurveyList" runat="server" AutoGenerateColumns="False" CssClass="GridViewStyle"
                                      PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" SortedAscendingHeaderStyle-CssClass="sortasc-header"
                                      SortedDescendingHeaderStyle-CssClass="sortdesc-header" SortedAscendingCellStyle-CssClass="sortasc"
                                      SortedDescendingCellStyle-CssClass="sortdesc" EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty"
                                      SelectedRowStyle="selected" DataKeyNames="surveyId" DataSourceID="EntityDataSource1"
                                      CellPadding="4" OnRowDataBound="GridView1_RowDataBound" Width="100%" PageSize="15"
                                      ForeColor="#333333" GridLines="None" AllowSorting="True">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/Admin/images/icon/edit.png"
                                                                     OnClick="imgBtnSurveyEdit_Click" CommandArgument='<%#Bind
                                                                                                   ("surveyId") %>' />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="~/Admin/images/icon/cop.png"
                                                                     CommandArgument='<%#Bind("surveyId") %>' OnClick="imgBtnSurveyDelete_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="question" SortExpression="question" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="state" SortExpression="state" HeaderStyle-HorizontalAlign="Left"
                                                   ItemStyle-HorizontalAlign="Justify" ItemStyle-Width="50">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Justify" Width="50px" />
                                </asp:CheckBoxField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="left">
                                                    <asp:Button ID="imgBtnInnerNew" runat="server" CssClass="NewBtn" CommandArgument='<%#Bind("surveyId") %>' OnClick="btnNewSurveyOption"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gVSurveyOption" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                                  CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                                  SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                                                                  SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                                                                  EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                                                                  DataKeyNames="surveyOptionId" OnRowDataBound="grdSecenek_RowDataBound" PageSize="8"
                                                                  Width="100%" ForeColor="#333333" GridLines="None">
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="75" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgBtnInnerEdit" runat="server"
                                                                                                 OnClick="imgBtnInerEditSurveyOption_Click"
                                                                                                 CommandArgument='<%#Bind("surveyOptionId") %>' 
                                                                                                 ImageUrl="~/Admin/images/icon/edit.png" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgBtnInerDelete" runat="server" CommandArgument='<%#Bind("surveyOptionId") %>'
                                                                                                 ImageUrl="~/Admin/images/icon/cop.png" OnClick="imgBtnInerDelete_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="surveyOption" SortExpression="surveyOption" />
                                                            <asp:BoundField DataField="chooseCount" SortExpression="chooseCount" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=Entities"
                                  DefaultContainerName="Entities" EntitySetName="Survey">
            </asp:EntityDataSource>
            <asp:EntityDataSource ID="EntityDataSource2" runat="server" ConnectionString="name=Entities"
                                  DefaultContainerName="Entities" EntitySetName="Survey_Option">
            </asp:EntityDataSource>
        </asp:Panel>
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
<asp:HiddenField ID="hfSurveyId" runat="server" />
<asp:HiddenField ID="hfSurveyOptionId" runat="server" />