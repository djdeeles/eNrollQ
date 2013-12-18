<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberAddEdit.ascx.cs"
    Inherits="eNroll.Admin.adminUserControls.MemberAddEdit" %>
<%@ Import Namespace="Resources" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#" + '<%=tbEditParola.ClientID %>').keyup(function () {
            $('#result').html(checkStrength($("#" + '<%=tbEditParola.ClientID %>').val()));
        });
    });
</script>
<script type="text/javascript">

    function showUserInfo(control) {
        $("#results").empty();
        $("#dPersonalDetail").css("display", "none");
        $("#dHomeDetail").css("display", "none");
        $("#dWorkDetail").css("display", "none");
        $("#dEmailAddress").css("display", "none");
        $("#dMemberDetail").css("display", "none");

        $("#" + control).css("display", "block");

    }

    function viewEmailInfo() {
        $("#results").empty();
        $(".viewEmailInfo").css("display", "none");
        $(".editEmailInfo").css("display", "table-row");
        $("#<%=btnSaveEmailInfo.ClientID %>").css("display", "inline-block");
        return false;
    }
</script>
<script type="text/javascript">

    function generatePassword() {
        $.ajax({
            type: "POST",
            url: "/NewMember.aspx/GeneratePassword",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                document.getElementById("<%=tbEditParola.ClientID %>").value = msg.d;
                $('#result').html(checkStrength($("#" + '<%=tbEditParola.ClientID %>').val()));
            }
        });
        return "";
    }
    function checkStrength(password) {

        //initial strength
        var strength = 0;

        //if the password length is less than 6, return message.
        if (password.length < 5) {
            $('#result').removeClass();
            $('#result').addClass('short');
            return '<%=Resources.Resource.msgPasswordTooShort %>';
        }

        //length is ok, lets continue.

        //if length is 8 characters or more, increase strength value
        if (password.length > 7) strength += 1;

        //if password contains both lower and uppercase characters, increase strength value
        if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1;

        //if it has numbers and characters, increase strength value
        if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1;

        //if it has one special character, increase strength value
        if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1;

        //if it has two special characters, increase strength value
        if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1;

        //now we have calculated strength value, we can return messages

        //if value is less than 2
        if (strength < 2) {
            $('#result').removeClass();
            $('#result').addClass('weak');
            return '<%=Resources.Resource.msgPasswordWeak%>';
        } else if (strength == 2) {
            $('#result').removeClass();
            $('#result').addClass('good');
            return '<%=Resources.Resource.msgPasswordGood %>';
        } else {
            $('#result').removeClass();
            $('#result').addClass('strong');
            return '<%=Resources.Resource.msgPasswordStrong %>';
        }
    }
    function setChecked(control) {
        document.getElementById(control).checked = true;
    }

    //g[0] seçilmemiş, g[1] bay,  g[2] bayan
    //m[0] seçilmemiş, m[1] evli, m[2] bekar
    function showHideAboutGender() {
        var i = document.getElementById("<%=ddlEditGender.ClientID %>").selectedIndex;
        var g = document.getElementById("<%=ddlEditGender.ClientID %>").options;

        var j = document.getElementById("<%=ddlEditMaritalStatus.ClientID %>").selectedIndex;
        var m = document.getElementById("<%=ddlEditMaritalStatus.ClientID %>").options;

        if (m[j].index == "0") { // seçilmemiş
            $("#<%=trMaidenName.ClientID %>").addClass("hideaboutgender");
            $("#<%=trMarriageDate.ClientID %>").addClass("hideaboutgender");
        } else if (m[j].index == "1") { // evli
            if (g[i].index == "0") { //seçilmemiş
                $("#<%=trMaidenName.ClientID %>").addClass("hideaboutgender");
                $("#<%=trMarriageDate.ClientID %>").removeClass("hideaboutgender");
            } else if (g[i].index == "1") { // bay
                $("#<%=trMaidenName.ClientID %>").addClass("hideaboutgender");
                $("#<%=trMarriageDate.ClientID %>").removeClass("hideaboutgender");
            } else if (g[i].index == "2") { // bayan
                $("#<%=trMaidenName.ClientID %>").removeClass("hideaboutgender");
                $("#<%=trMarriageDate.ClientID %>").removeClass("hideaboutgender");
            }
        } else if (m[j].index == "2") { // bekar
            $("#<%=trMaidenName.ClientID %>").addClass("hideaboutgender");
            $("#<%=trMarriageDate.ClientID %>").addClass("hideaboutgender");
        }
    }

    function onlyNumber(event) {
        var keyCode = event.keyCode;
        if ((keyCode < 46 || keyCode > 57) && keyCode != 8 && keyCode != 9 &&
            keyCode != 0 && keyCode != 47 && (keyCode < 96 || keyCode > 105)) return false;
        return true;
    }

</script>
<style type="text/css">
    .hideaboutgender { display: none; }
    .viewEmailInfo { display: none; }
    .editEmailInfo { display: block; }
</style>
<script type="text/javascript">

    function deleteEmail(userId, emailId) {
        $("#results").empty();
        var r = confirm("Email adresini silmek istiyor musunuz?");
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "/MemberProfile.aspx/DeleteEmailAdmn",
                data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d != "") {
                        if (msg.d == "-1") {
                            alert("Aktif mail adresi olarak kullanıldığı için silinemiyor. " +
                                "Silmek için aktif mailinizi değiştirmeniz gerekmektedir.");
                            return;
                        }
                        $("#dEmailProcessResult").empty();
                        $("#dEmailProcessResult").append(msg.d);
                        javascript: showSuccessToast('<%=AdminResource.msgDeleted%>');
                        showUserInfo('dEmailAddress');
                    }
                }
            });
        }
        else {
            return false;
        }
    }
    function changeMailing(userId, emailId, islem) {
        $("#results").empty();
        $.ajax({
            type: "POST",
            url: "/MemberProfile.aspx/ChangeMailingAdmn",
            data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "','islem': '" + islem + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d != "") {
                    $("#dEmailProcessResult").empty();
                    $("#dEmailProcessResult").append(msg.d);
                    javascript: showSuccessToast('<%=AdminResource.msgUpdated%>');
                    showUserInfo('dEmailAddress');
                }
            }
        });
    }

    function onayMailiGonder(userId, emailId) {
        $("#results").empty();
        $.ajax({
            type: "POST",
            url: "/MemberProfile.aspx/ActivateMailAddress",
            data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                javascript: showSuccessToast(msg.d);
                showUserInfo('dEmailAddress');
            }
        });
    }
    function changeActiveEmail(userId, emailId, control) {
        $("#results").empty();
        var r = confirm("<%=AdminResource.msgQuestionChangeEmailComfirm %>");
        if (r == true) {
            $.ajax({
                type: "POST",
                url: "/MemberProfile.aspx/ChangeActiveEmailAdmn",
                data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d != "") {
                        $("#dEmailProcessResult").empty();
                        $("#dEmailProcessResult").append(msg.d);
                        javascript: showSuccessToast('<%=AdminResource.msgUpdated%>');
                        showUserInfo('dEmailAddress');
                    }
                }
            });
        }
        else {
            document.getElementById(control).checked = false;
            return false;
        }
    }
</script>
<asp:MultiView runat="server" ID="mvMember" ActiveViewIndex="0" ViewStateMode="Enabled">
    <asp:View ID="vEditMember" runat="server">
        <div style="width: 100%; float: left;">
            <table cellpadding="3" align="left">
                <tr>
                    <td>
                        <asp:Image runat="server" ID="lUserPhoto" />
                    </td>
                    <td valign="top">
                        <div style="margin-left: 5px;">
                            <span style="font-size: 15px; font-weight: bold;">
                                <asp:Label runat="server" ID="lUserNameSurname" /></span>
                            <br />
                            <asp:Label runat="server" ID="lUserEmail" /></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <p style="margin: 0 0 3px 0;">
                            <asp:Button ID="BtnConfirmMemberActive" CssClass="SaveCancelBtn" runat="server" OnClick="BtnConfirmMemberActiveClick" />
                            <asp:Button ID="BtnSetPassive" CssClass="SaveCancelBtn" runat="server" OnClick="BtnSetPassiveClick" />
                            <asp:Button ID="BtnSetActive" CssClass="SaveCancelBtn" runat="server" OnClick="BtnSetActiveClick" />
                            <asp:Button ID="BtnResendActivationCode" CssClass="SaveCancelBtn" runat="server" OnClick="BtnResendActivationCodeClick"/>
                        </p>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <table align="right" width="150px">
                <tr>
                    <td>
                        <p style="margin: 0 0 3px 0;">
                            <input id="bPersonalDetail" onclick="showUserInfo('dPersonalDetail');" style="width: 100%;
                                text-align: left;" type="button" class="SaveCancelBtn" value='<%=AdminResource.lbPersonalInfo %>' /></p>
                        <p style="margin: 0 0 3px 0;">
                            <input id="Button2" onclick="showUserInfo('dMemberDetail');" style="width: 100%;
                                text-align: left;" type="button" class="SaveCancelBtn" value='<%=AdminResource.lbMembershipInfo%>' /></p>
                        <p style="margin: 0 0 3px 0;">
                            <input id="bHomeDetail" onclick="showUserInfo('dHomeDetail');" style="width: 100%;
                                text-align: left;" type="button" class="SaveCancelBtn" value='<%=AdminResource.lbHomeInfo%>' /></p>
                        <p style="margin: 0 0 3px 0;">
                            <input id="bJobDetail" onclick="showUserInfo('dWorkDetail');" style="width: 100%;
                                text-align: left;" type="button" class="SaveCancelBtn" value='<%=AdminResource.lbWorkInfo%>' /></p>
                        <p style="margin: 0 0 3px 0;">
                            <input id="Button1" onclick="showUserInfo('dEmailAddress');" style="width: 100%;
                                text-align: left;" type="button" class="SaveCancelBtn" value='<%=AdminResource.lbEmailInfo%>' /></p>
                    </td>
                </tr>
            </table>
        </div>
        <div id="dPersonalDetail" style="display: block; float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%=Resources.Resource.lbPersonalInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%=AdminResource.lbTC %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditTC" runat="server" onkeydown="return onlyNumber(event);" MaxLength="11"
                                Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbName %>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditName" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbEditName"
                                ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbSurname %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditSurname" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbEditSurname"
                                ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbPassword %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditParola" runat="server" Width="200px"></asp:TextBox>
                            <input id="iGeneratePassword" value="<%=Resources.AdminResource.lbGeneratePassword%>"
                                type="button" class="SaveCancelBtn" onclick="generatePassword();return false;" />
                            <label id="result" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbGender%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEditGender" runat="server" onchange="showHideAboutGender();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbBloodType%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditBloodType" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbFatherName%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditFatherName" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbMotherName%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditMotherName" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbMartialStatus%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEditMaritalStatus" runat="server" onchange="showHideAboutGender();">
                                <asp:ListItem Text="Bekar" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Evli" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trMaidenName" runat="server">
                        <td>
                            <asp:Label ID="lbMaidenName" runat="server" />
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditMaidenName" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trMarriageDate" runat="server">
                        <td>
                            <asp:Label ID="lbMarriageDate" runat="server" />
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="dpEditMarriageDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                                runat="server" ZIndex="30001">
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbBirthDate%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="dpEditBirthDate" runat="server" MaxDate="01-01-2200" MinDate="01-01-1900"
                                ZIndex="30001">
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                ControlToValidate="dpEditBirthDate" ValidationGroup="g1" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbBirthPlace%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditBirthPlace" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbPhotoUrl%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <p>
                                <input type="file" id="iPhotoUrl" name="iPhotoUrl" multiple="false" accept="image/*" /></p>
                            <asp:HiddenField runat="server" ID="hdnActiveDirectory" />
                            <asp:HiddenField runat="server" ID="hdnUserProfilPhotoUrl" Value="hdnUserProfilPhotoUrl" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbHobby%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditHobbies" TextMode="MultiLine" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbMemberFoundation%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox runat="server" TextMode="MultiLine" ID="tbEditMemberFoundation" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbWeb%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditWeb" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbGsmNo%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditGsmNo" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbLastSchool%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditLastSchool" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbLastSchoolGraduateDate%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadMonthYearPicker ID="dpEditLastSchoolGraduateDate" runat="server" MaxDate="01-2200"
                                MinDate="01-1900">
                                <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                </DateInput>
                            </telerik:RadMonthYearPicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbDecease%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbMemberDecease" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbDeceaseDate%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="dpMemberDeceaseDate" runat="server" MaxDate="01-2200"
                                MinDate="01-1900" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btnUpdatePersonalInfo" CssClass="SaveCancelBtn" OnClick="BtnUpdatePersonalInfoClick" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dMemberDetail" style="display: none; float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%=Resources.Resource.lbMembershipInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= AdminResource.lbMemberNumber%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbEditMembershipNumber"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbRelationType%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditMemberRelationType" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                ControlToValidate="ddlEditMemberRelationType" ValidationGroup="vgUpdateMember" InitialValue="" ErrorMessage="!" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbEnterDate%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadDatePicker ID="dpEditMembershipDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                                runat="server" ZIndex="30001">
                            </telerik:RadDatePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbSpecialNumber%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbEditSpecialNumber"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbTerm%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <telerik:RadMonthYearPicker ID="dpEditTerm" runat="server" MaxDate="01-2200" MinDate="01-1900">
                                <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy" />
                            </telerik:RadMonthYearPicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbAutoPaymentOrder%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbEditAutoPaymentOrder"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbAdminNote%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbAdminNote" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="btnUpdateMemberInfo" CssClass="SaveCancelBtn" ValidationGroup="vgUpdateMember" OnClick="BtnUpdateMemberInfoClick" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dHomeDetail" style="display: none; float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%=Resources.Resource.lbHomeInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= AdminResource.lbCountry%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditHomeCountry" CssClass="editHomeInfo"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlEditHomeCountry_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbCity%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditHomeCity" CssClass="editHomeInfo" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlEditHomeCity_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbTown%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditHomeTown" CssClass="editHomeInfo" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbAddress%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditHomeAddress" runat="server" TextMode="MultiLine" CssClass="editHomeInfo"
                                Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbZipCode%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditHomeZipCode" runat="server" CssClass="editHomeInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbPhone%>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditHomePhone" runat="server" CssClass="editHomeInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbHidePersonalInfo%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbEditHidePersonalInfo" CssClass="editHomeInfo" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="BtnUpdateHomeInfo" CssClass="SaveCancelBtn" OnClick="BtnUpdateHomeInfoClick" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dWorkDetail" style="display: none; float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%=Resources.Resource.lbWorkInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= AdminResource.lbCountry%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditWorkCountry" AutoPostBack="true" OnSelectedIndexChanged="ddlEditWorkCountry_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbCity%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditWorkCity" AutoPostBack="true" OnSelectedIndexChanged="ddlEditWorkCity_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbTown%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditWorkTown" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbAddress%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditWorkAddress" TextMode="MultiLine" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbZipCode%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditWorkZipCode" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbPhone%>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditWorkPhone" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbFax%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditWorkFax" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbJobSector%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditJobSectors" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbJob%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEditJobs" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbWorkTitle%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditWorkTitle" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbWorkCorporation%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEditWorkCorparation" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=AdminResource.lbHideJobInfo%>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbEditHideJobInfo" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="server" ID="BtnUpdateWorkInfo" CssClass="SaveCancelBtn" OnClick="BtnUpdateWorkInfoClick" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dEmailAddress" style="display: none; float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%=Resources.Resource.lbEmailInfo %></legend>
                <div id="dEmailProcessResult">
                    <asp:Literal runat="server" ID="ltUserEMailAddress" />
                </div>
                <table cellpadding="3">
                    <tr class="editEmailInfo">
                        <td>
                            <label>
                                <%=AdminResource.lbNewEmail%></label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbNewEmailAddress" runat="server" />
                            &nbsp;
                            <asp:Button ID="btnSaveEmailInfo" runat="server" CssClass="SaveCancelBtn" OnClick="BtnSaveEmailInfoClick"
                                ValidationGroup="vgNewEmail" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="tbNewEmailAddress"
                                ErrorMessage="!" ForeColor="Red" ValidationGroup="vgNewEmail" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ForeColor="Red"
                                ControlToValidate="tbNewEmailAddress" Display="Dynamic" ValidationGroup="vgNewEmail"
                                ErrorMessage="!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <div id="results">
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </asp:View>
    <asp:View ID="vGridMember" runat="server">
    </asp:View>
</asp:MultiView>
<asp:HiddenField runat="server" ID="hfUserId" />
<asp:HiddenField runat="server" ID="hfEditHomeCountry" />
<asp:HiddenField runat="server" ID="hfEditHomeCity" />
<asp:HiddenField runat="server" ID="hfEditHomeTown" />
<asp:HiddenField runat="server" ID="hfEditWorkCountry" />
<asp:HiddenField runat="server" ID="hfEditWorkCity" />
<asp:HiddenField runat="server" ID="hfEditWorkTown" />
<br style="clear: both;"/><br/>