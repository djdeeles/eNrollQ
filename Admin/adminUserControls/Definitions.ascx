<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Definitions.ascx.cs"
            Inherits="eNroll.Admin.adminUserControls.Definitions" %>
<%@ Import Namespace="Resources" %>

<%@ Register Src="~/Admin/adminUserControls/DefCorporationRelType.ascx" TagName="DefCorpRelType1"
             TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/DefCountryCityTown.ascx" TagName="DefCountryCityTown1"
             TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/DefSectorJob.ascx" TagName="DefSectorJob1"
             TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/DefBloodType.ascx" TagName="DefBloodType1"
             TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/DefCurrency.ascx" TagName="DefCurrency1"
             TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/DefDues.ascx" TagName="DefDues1" TagPrefix="uc1" %>
<%@ Register Src="~/Admin/adminUserControls/DefTax.ascx" TagName="DefTax1" TagPrefix="uc1" %>
<asp:MultiView runat="server" ID="mvAuthoriztn">
    <asp:View runat="server" ID="vAuth" ViewStateMode="Enabled">
        <asp:Button ID="btnCorporationRelType" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCorporationRelType_OnClick" />
        <asp:Button ID="btnCountryCityTown" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCountryCityTown_OnClick" />
        <asp:Button ID="btnSectorJob" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSectorJob_OnClick" />
        <asp:Button ID="btnBloodType" runat="server" CssClass="SaveCancelBtn" OnClick="BtnBloodType_OnClick" />
        <asp:Button ID="btnCurrency" runat="server" CssClass="SaveCancelBtn" OnClick="BtnCurrency_OnClick" />
        <asp:Button ID="btnDuesType" runat="server" CssClass="SaveCancelBtn" OnClick="BtnDuesType_OnClick" /> 
        <asp:Button ID="btnTaxType" runat="server" CssClass="SaveCancelBtn" OnClick="BtnTaxType_OnClick" />
        <asp:MultiView runat="server" ID="mvDefinations" ActiveViewIndex="0" ViewStateMode="Enabled">
            <asp:View runat="server" ID="vDefinition">
                <p>
                    <%= AdminResource.lbDefinitionIntro %>
                </p>
            </asp:View>
            <asp:View runat="server" ID="vDefCorporationRelType">
                <div>
                    <uc1:DefCorpRelType1 ID="cDefCorpRelType" runat="server" />
                </div>
            </asp:View>
            <asp:View runat="server" ID="vDefCountryCityTown">
                <div>
                    <uc1:DefCountryCityTown1 ID="cDefCountryCityTown" runat="server" />
                </div>
            </asp:View>
            <asp:View runat="server" ID="vDefSectorJob">
                <div>
                    <uc1:DefSectorJob1 ID="cDefSectorJob" runat="server" />
                </div>
            </asp:View>
            <asp:View runat="server" ID="vDefBloodType">
                <div>
                    <uc1:DefBloodType1 ID="cDefBloodType" runat="server" />
                </div>
            </asp:View>
            <asp:View runat="server" ID="vDefCurrency">
                <div>
                    <uc1:DefCurrency1 ID="cDefCurrency" runat="server" />
                </div>
            </asp:View>
            <asp:View runat="server" ID="vDefDues">
                <div>
                    <uc1:DefDues1 ID="cDefDues" runat="server" />
                </div>
            </asp:View> 
            <asp:View runat="server" ID="vDefTax">
                <div>
                    <uc1:DefTax1 ID="cDefTax" runat="server" />
                </div>
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