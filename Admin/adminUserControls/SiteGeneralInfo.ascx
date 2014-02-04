<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_SiteGeneralInfo" 
            CodeBehind="SiteGeneralInfo.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<%@ Register Src="Rtb.ascx" TagName="Rtb" TagPrefix="uc1" %>
<style type="text/css">
    .tdLeft { width: 115px; }

    .tdCenter { width: 10px; }
</style>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <asp:Panel runat="server" DefaultButton="btnSave">
            <div class="rightcontenttable">
                <asp:Menu ID="Menu1" runat="server" OnMenuItemClick="Menu1_MenuItemClick" Orientation="Horizontal"
                          RenderingMode="Table" StaticEnableDefaultPopOutImage="False" CssClass="generalmenu">
                    <Items>
                        <asp:MenuItem Value="vBaslik"></asp:MenuItem>
                        <asp:MenuItem Value="vTepe1"></asp:MenuItem>
                    </Items>
                </asp:Menu>
                <div id="ortasag">
                    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vBaslik" runat="server">
                            <div class="rightcontenttable">
                                <div class="rightcontenttable" style="font-size: 14px;">
                                    <b>
                                        <%= AdminResource.lbGeneralSettings %></b>
                                </div>
                                <table class="rightcontenttable">
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbTitle %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBaslik" runat="server" Width="620px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbDesc %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAciklama" runat="server" Width="620px" TabIndex="1" MaxLength="200"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbKeyWords %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtKeywords" runat="server" Width="620px" TabIndex="2" MaxLength="300"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft" style="vertical-align: top;">
                                            <%= AdminResource.lbFooterText %>
                                        </td>
                                        <td class="tdCenter" style="vertical-align: top;">
                                            :
                                        </td>
                                        <td style="vertical-align: top;">
                                            <uc1:Rtb ID="Rtb1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft" style="vertical-align: top;">
                                            <%= AdminResource.lbScriptCode %>
                                        </td>
                                        <td class="tdCenter" style="vertical-align: top;">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox  runat="server" content="text/javascript; charset=utf-8" ID="analyticsCode"
                                                          TextMode="MultiLine" Height="200px" Width="620px"></asp:TextBox> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbSiteDefaultLanguage %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cbChooseLang" DataValueField="languageId" DataTextField="name"
                                                              runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbSiteMaintenanceMode %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbSiteMaintenanceMode" runat="server" />
                                        </td>
                                    </tr>
                                </table> 
                            </div>
                        </asp:View>
                        <asp:View ID="vTepe1" runat="server">
                            <table class="rightcontenttable">
                                <tr>
                                    <td colspan="4" style="font-size: 14px;">
                                        <%= AdminResource.lbHeadImage %>&nbsp;1
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbFileType %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTepeDosyaTipi1" runat="server" Height="22px" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbImagePath %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtTepeDosya1" runat="server" Width="500px" MaxLength="150" />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Button ID="btnImageSelect" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog(); return false;" />
                                        <script type="text/javascript">
                                            function OpenFileExplorerDialog() {
                                                selectedFile = $find("<%= txtTepeDosya1.ClientID %>");
                                                var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                wnd.show();
                                            }
                                        </script>
                                        <telerik:RadWindow runat="server" Width="600px" Height="600px" VisibleStatusbar="false"
                                                           ShowContentDuringLoad="false" NavigateUrl="../FileSelector.aspx" ID="ExplorerWindow"
                                                           Modal="true" Behaviors="Close,Move,Resize,Maximize">
                                        </telerik:RadWindow>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbHeight %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTepeYukseklik1" runat="server" Width="50px" MaxLength="50"></asp:TextBox>&nbsp;px
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbWidth %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTepeGenislik1" runat="server" Width="50px" MaxLength="50"></asp:TextBox>&nbsp;px
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft" style="font-size: 14px;">
                                        <%= AdminResource.lbHeadImage %>&nbsp;2
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbFileType %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTepe1DosyaTipi" OnSelectedIndexChanged="ddlDosyaTipi_Chaged"
                                                          runat="server" AutoPostBack="True" Height="22px" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel runat="server" ID="pnlImageFlashHeight">
                                <table>
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbImagePath %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtTepeDosya2" runat="server" Width="500px" MaxLength="150" />
                                        </td>
                                        <td style="width: 100px;">
                                            <asp:Button ID="imgBtnImageSelect0" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog0(); return false;" />
                                            <script type="text/javascript">
                                                function OpenFileExplorerDialog0() {
                                                    selectedFile = $find("<%= txtTepeDosya2.ClientID %>");
                                                    var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                    wnd.show();
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbHeight %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHeadYukseklik2" runat="server" Width="50px" MaxLength="50"></asp:TextBox>&nbsp;px
                                        </td>
                                        <td style="width: 100px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLeft">
                                            <%= AdminResource.lbWidth %>
                                        </td>
                                        <td class="tdCenter">
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHeadGenislik2" runat="server" Width="50px" MaxLength="50"></asp:TextBox>&nbsp;px
                                        </td>
                                        <td style="width: 100px;">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table class="rightcontenttable" id="slideShow" runat="server">
                                <tr>
                                    <td colspan="4" style="font-size: 12px;">
                                        <b>
                                            <%= AdminResource.lbSlideSettings %></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <%= AdminResource.lbSlide %>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbImage %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSsImg1" runat="server" Width="500px" MaxLength="150" />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Button ID="imgBtnImageSelect2" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog2(); return false;" />
                                        <script type="text/javascript">
                                            function OpenFileExplorerDialog2() {
                                                selectedFile = $find("<%= txtSsImg1.ClientID %>");
                                                var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                wnd.show();
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbLink %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbUrl1" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbDesc %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSsAciklama1" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <%= AdminResource.lbSlide %>&nbsp;2
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbImage %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSsImg2" runat="server" Width="500px" MaxLength="150" />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Button ID="imgBtnImageSelect3" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog3(); return false;" />
                                        <script type="text/javascript">
                                            function OpenFileExplorerDialog3() {
                                                selectedFile = $find("<%= txtSsImg2.ClientID %>");
                                                var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                wnd.show();
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbLink %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbUrl2" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbDesc %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSsAciklama2" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <%= AdminResource.lbSlide %>&nbsp;3
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbImage %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSsImg3" runat="server" Width="500px" MaxLength="150" />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Button ID="imgBtnImageSelect4" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog4(); return false;" />
                                        <script type="text/javascript">
                                            function OpenFileExplorerDialog4() {
                                                selectedFile = $find("<%= txtSsImg3.ClientID %>");
                                                var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                wnd.show();
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbLink %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbUrl3" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbDesc %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSsAciklama3" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <%= AdminResource.lbSlide %>&nbsp;4
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbImage %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSsImg4" runat="server" Width="500px" MaxLength="150" />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Button ID="imgBtnImageSelect5" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog5(); return false;" />
                                        <script type="text/javascript">
                                            function OpenFileExplorerDialog5() {
                                                selectedFile = $find("<%= txtSsImg4.ClientID %>");
                                                var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                wnd.show();
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbLink %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbUrl4" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbDesc %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSsAciklama4" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <%= AdminResource.lbSlide %>&nbsp;5
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbImage %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtSsImg5" runat="server" Width="500px" MaxLength="150" />
                                    </td>
                                    <td style="width: 100px;">
                                        <asp:Button ID="imgBtnImageSelect6" runat="server" CssClass="ImageSelectBtn" OnClientClick="OpenFileExplorerDialog6(); return false;" />
                                        <script type="text/javascript">
                                            function OpenFileExplorerDialog6() {
                                                selectedFile = $find("<%= txtSsImg5.ClientID %>");
                                                var wnd = $find("<%= ExplorerWindow.ClientID %>");
                                                wnd.show();
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbLink %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbUrl5" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbDesc %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSsAciklama5" runat="server" Width="500px" MaxLength="150"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbHeight %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSlideShowYukseklik" runat="server" Width="50px" MaxLength="50"></asp:TextBox>&nbsp;px
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbWidth %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSlideShowGenislik" runat="server" Width="50px" MaxLength="50"></asp:TextBox>&nbsp;px
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbTiming %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSsTimer" runat="server" Width="50px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLeft">
                                        <%= AdminResource.lbEfect %>
                                    </td>
                                    <td class="tdCenter">
                                        :
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSlideShowEfekt" runat="server" Width="150px">
                                            <asp:ListItem Value="zipper">Zipper</asp:ListItem>
                                            <asp:ListItem Value="wave">Wave</asp:ListItem>
                                            <asp:ListItem Value="curtain">Curtain</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 100px;">
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                    <div class="rightcontenttable">
                        <table class="rightcontenttable">
                            <tr>
                                <td class="tdLeft">
                                </td>
                                <td style="text-align: left; width: 475px;" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="btnSave_Click" />
                                </td>
                                <td style="width: 100px;">
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
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