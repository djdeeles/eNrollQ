<%@ Control Language="C#" AutoEventWireup="true" Inherits="Admin_adminUserControls_SiteLayout"
            CodeBehind="SiteLayout.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<style type="text/css">
    .field {
        border: 1px dashed #c0c0c0;
        float: left;
        height: 100%;
        min-height: 100px;
        overflow: visible;
        width: 735px;
    }
</style>
<asp:MultiView runat="server" ID="mvAuth">
    <asp:View runat="server">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td width="25px">
                    <asp:ImageButton ID="ibtnHtmlTagValues" runat="server" ImageUrl="~/Admin/images/icon/arti.png"
                                     Width="16px" />
                </td>
                <td >
                    <asp:DropDownList ID="ddlHtmlTag" runat="server" Width="200px" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Button ID="btnSave" runat="server" CssClass="SaveCancelBtn" OnClick="btnSave_Click" />
                </td>
            </tr>
            <tr><td collspan="3">&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnTepe" runat="server" ImageUrl="~/Admin/images/icon/arti.png"
                                     Width="16px" CssClass="ibtn" />
                </td>
                <td colspan="2">
                    <div id="tepe" runat="server" class="field">
                        <asp:PlaceHolder ID="phTepe" runat="server"></asp:PlaceHolder>
                    </div>
                </td>
            </tr>
            <tr><td collspan="3">&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnGovde" runat="server" ImageUrl="~/Admin/images/icon/arti.png"
                                     Width="16px" CssClass="ibtn" />
                </td>
                <td colspan="2">
                    <div id="govde" runat="server" class="field">
                        <asp:PlaceHolder ID="phGovde" runat="server"></asp:PlaceHolder>
                    </div>
                </td>
            </tr>
            <tr><td collspan="3">&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnAlt" runat="server" ImageUrl="~/Admin/images/icon/arti.png"
                                     Width="16px" CssClass="ibtn" />
                </td>
                <td colspan="2">
                    <div id="alt" runat="server" class="field">
                        <asp:PlaceHolder ID="phAlt" runat="server"></asp:PlaceHolder>
                    </div>
                </td>
            </tr>
        </table>
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