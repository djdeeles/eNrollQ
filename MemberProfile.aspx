<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
         CodeBehind="MemberProfile.aspx.cs" Inherits="eNroll.MemberProfile" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="eNroll.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"
             ViewStateMode="Enabled">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
    <link href='http://fonts.googleapis.com/css?family=Vollkorn' rel='stylesheet' type='text/css'>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#" + '<%= tbParola.ClientID %>').keyup(function() {
                $('#result').html(checkStrength($("#" + '<%= tbParola.ClientID %>').val()));
            });
        });
    </script>
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
    <script type="text/javascript">
        function generatePassword() {
            $.ajax({
                type: "POST",
                url: "NewMember.aspx/GeneratePassword",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    document.getElementById("<%= tbParola.ClientID %>").value = msg.d;
                    $('#result').html(checkStrength($("#" + '<%= tbParola.ClientID %>').val()));
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
                return '<%= Resources.Resource.msgPasswordTooShort %>';
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
                return '<%= Resources.Resource.msgPasswordWeak %>';
            } else if (strength == 2) {
                $('#result').removeClass();
                $('#result').addClass('good');
                return '<%= Resources.Resource.msgPasswordGood %>';
            } else {
                $('#result').removeClass();
                $('#result').addClass('strong');
                return '<%= Resources.Resource.msgPasswordStrong %>';
            }
        }
    </script>
    <script type="text/javascript">
        function editPersonalInfoClick() {
            $("#results").empty();
            $(".viewPersonalInfo").css("display", "none");
            $(".editPersonalInfo").css("display", "table-row");
            $("#<%= btnEditPersonalInfo.ClientID %>").css("display", "none");

            $("#<%= btnViewPersonalInfo.ClientID %>").css("display", "inline-block");
            $("#<%= btnSavePersonalInfo.ClientID %>").css("display", "inline-block");
            $("#iGeneratePassword").css("display", "inline-block");
            return false;
        }

        function viewPersonalInfoClick() {
            $(".viewPersonalInfo").css("display", "table-row");
            $(".editPersonalInfo").css("display", "none");
            $("#<%= btnEditPersonalInfo.ClientID %>").css("display", "inline-block");

            $("#<%= btnViewPersonalInfo.ClientID %>").css("display", "none");
            $("#<%= btnSavePersonalInfo.ClientID %>").css("display", "none");
            $("#iGeneratePassword").css("display", "none");
            return false;
        }

        function viewMemberInfoClick() {
            $(".viewMemberInfo").css("display", "table-row");
            $(".editMemberInfo").css("display", "none");
            return false;
        }

        function editHomeInfoClick() {
            $("#results").empty();
            $(".viewHomeInfo").css("display", "none");
            $(".editHomeInfo").css("display", "table-row");
            $("#<%= btnEditHomeInfo.ClientID %>").css("display", "none");

            $("#<%= btnViewHomeInfo.ClientID %>").css("display", "inline-block");
            $("#<%= btnSaveHomeInfo.ClientID %>").css("display", "inline-block");
            return false;
        }

        function viewHomeInfoClick() {
            $(".viewHomeInfo").css("display", "table-row");
            $(".editHomeInfo").css("display", "none");
            $("#<%= btnEditHomeInfo.ClientID %>").css("display", "inline-block");

            $("#<%= btnViewHomeInfo.ClientID %>").css("display", "none");
            $("#<%= btnSaveHomeInfo.ClientID %>").css("display", "none");
            return false;
        }

        function editWorkInfoClick() {
            $("#results").empty();
            $(".viewWorkInfo").css("display", "none");
            $(".editWorkInfo").css("display", "table-row");
            $("#<%= btnEditWorkInfo.ClientID %>").css("display", "none");

            $("#<%= btnViewWorkInfo.ClientID %>").css("display", "inline-block");
            $("#<%= btnSaveWorkInfo.ClientID %>").css("display", "inline-block");
            return false;
        }

        function viewWorkInfoClick() {
            $(".viewWorkInfo").css("display", "table-row");
            $(".editWorkInfo").css("display", "none");
            $("#<%= btnEditWorkInfo.ClientID %>").css("display", "inline-block");
            $("#<%= btnViewWorkInfo.ClientID %>").css("display", "none");
            $("#<%= btnSaveWorkInfo.ClientID %>").css("display", "none");
            return false;
        }

        function editEmailInfoClick() {
            $("#results").empty();
            $(".viewEmailInfo").css("display", "none");
            $(".editEmailInfo").css("display", "table-row");
            $("#<%= btnEditEmailInfo.ClientID %>").css("display", "none");

            $("#<%= btnViewEmailInfo.ClientID %>").css("display", "inline-block");
            $("#<%= btnSaveEmailInfo.ClientID %>").css("display", "inline-block");
            return false;
        }

        function viewEmailInfoClick() {
            $(".viewEmailInfo").css("display", "table-row");
            $(".editEmailInfo").css("display", "none");
            $("#<%= btnEditEmailInfo.ClientID %>").css("display", "inline-block");
            $("#<%= btnViewEmailInfo.ClientID %>").css("display", "none");
            $("#<%= btnSaveEmailInfo.ClientID %>").css("display", "none");
            return false;
        }

        function showUserInfo(control) {
            $("#results").empty();
            $("#dPersonalDetail").css("display", "none");
            $("#dHomeDetail").css("display", "none");
            $("#dWorkDetail").css("display", "none");
            $("#dEmailAddress").css("display", "none");
            $("#dMemberDetail").css("display", "none");
            $("#dFinanceDetail").css("display", "none");
            $("#" + control).css("display", "block");
        }

        function hideAllUserInfo() {
            $("#results").empty();
            $("#dPersonalDetail").css("display", "none");
            $("#dHomeDetail").css("display", "none");
            $("#dWorkDetail").css("display", "none");
            $("#dEmailAddress").css("display", "none");
            $("#dMemberDetail").css("display", "none");
            $("#dFinanceDetail").css("display", "none");
        }
    </script>
    <script type="text/javascript">

        function deleteEmail(userId, emailId) {
            $("#results").empty();

            var r = confirm('<%= AdminResource.lbDeletingQuestion %>');
            if (r == true) {
                $.ajax({
                    type: "POST",
                    url: "MemberProfile.aspx/DeleteEmail",
                    data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        if (msg.d != "") {
                            if (msg.d == "-1") {
                                return;
                            }
                            $("#dEmailProcessResult").empty();
                            $("#dEmailProcessResult").append(msg.d);
                            viewEmailInfoClick();
                        }
                    }
                });
            } else {
                return false;
            }
        }

        function changeMailing(userId, emailId, islem) {
            $("#results").empty();
            $.ajax({
                type: "POST",
                url: "MemberProfile.aspx/ChangeMailing",
                data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "','islem': '" + islem + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    if (msg.d != "") {
                        $("#dEmailProcessResult").empty();
                        $("#dEmailProcessResult").append(msg.d);
                        viewEmailInfoClick();
                    }
                }
            });
        }

        function onayMailiGonder(userId, emailId) {
            $("#results").empty();
            $.ajax({
                type: "POST",
                url: "MemberProfile.aspx/ActivateMailAddress",
                data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {
                    if (msg.d != "") {
                        $("#results").append(msg.d);
                        viewEmailInfoClick();
                    }
                }
            });
        }

        function onlyNumber(event) {
            var keyCode = event.keyCode;
            if ((keyCode < 46 || keyCode > 57) && keyCode != 8 && keyCode != 9 &&
                keyCode != 0 && keyCode != 47 && (keyCode < 96 || keyCode > 105)) return false;
            return true;
        }

        function changeActiveEmail(userId, emailId, control) {
            $("#results").empty();
            var r = confirm("<%= Resources.Resource.msgQuestionChangeEmail %>");
            if (r == true) {
                $.ajax({
                    type: "POST",
                    url: "MemberProfile.aspx/ChangeActiveEmail",
                    data: "{'userIdStr': '" + userId + "','emailIdStr': '" + emailId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        if (msg.d != "") {
                            window.location = msg.d;
                        }
                    }
                });
            } else {
                document.getElementById(control).checked = false;
                return false;
            }


        }
    </script>
    <script type="text/javascript">

        function setChecked(control) {
            document.getElementById(control).checked = true;
        }

        //g[0] seçilmemiş, g[1] bay,  g[2] bayan
        //m[0] seçilmemiş, m[1] evli, m[2] bekar

        function showHideAboutGender() {
            var i = document.getElementById("<%= ddlGender.ClientID %>").selectedIndex;
            var g = document.getElementById("<%= ddlGender.ClientID %>").options;

            var j = document.getElementById("<%= ddlMaritalStatus.ClientID %>").selectedIndex;
            var m = document.getElementById("<%= ddlMaritalStatus.ClientID %>").options;

            if (m[j].index == "0") { // seçilmemiş
                $("#<%= trMaidenName.ClientID %>").addClass("hideaboutgender");
                $("#<%= trMarriageDate.ClientID %>").addClass("hideaboutgender");
            } else if (m[j].index == "1") { // evli
                if (g[i].index == "0") { //seçilmemiş
                    $("#<%= trMaidenName.ClientID %>").addClass("hideaboutgender");
                    $("#<%= trMarriageDate.ClientID %>").removeClass("hideaboutgender");
                } else if (g[i].index == "1") { // bay
                    $("#<%= trMaidenName.ClientID %>").addClass("hideaboutgender");
                    $("#<%= trMarriageDate.ClientID %>").removeClass("hideaboutgender");
                } else if (g[i].index == "2") { // bayan
                    $("#<%= trMaidenName.ClientID %>").removeClass("hideaboutgender");
                    $("#<%= trMarriageDate.ClientID %>").removeClass("hideaboutgender");
                }
            } else if (m[j].index == "2") { // bekar
                $("#<%= trMaidenName.ClientID %>").addClass("hideaboutgender");
                $("#<%= trMarriageDate.ClientID %>").addClass("hideaboutgender");
            }
        }

    </script>
    <style type="text/css">
        .viewPersonalInfo { display: block; }

        .editPersonalInfo { display: none; }

        .viewHomeInfo { display: block; }

        .editHomeInfo { display: none; }

        .viewWorkInfo { display: block; }

        .editWorkInfo { display: none; }

        .viewEmailInfo { display: block; }

        .editEmailInfo { display: none; }

        .viewMemberInfo { display: block; }

        .editMemberInfo { display: none; }

        .hideaboutgender { display: none; }

        .deptAmount { text-align: right; }
    </style>
    <div style="width: 100%; float: left; margin-bottom: 10px;">
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
                        <asp:Label runat="server" ID="lUserEmail" />
                        <br />
                        <%= Resources.Resource.lbCurrentDebtAmount %>:&nbsp;<asp:Literal runat="server" ID="ltCurrentDebt"/>
                    </div>
                </td>
            </tr>
        </table>
        <table align="right" width="300px">
            <tr>
                <td>
                    <p style="margin: 0 0 3px 0;">
                        <input id="bPersonalDetail" onclick=" showUserInfo('dPersonalDetail'); " style="width: 100%; text-align: left;" type="button" class="button" value='<%= Resources.Resource.lbPersonalInfo %>' /></p>
                    <p style="margin: 0 0 3px 0;">
                        <input id="Button2" onclick=" showUserInfo('dMemberDetail'); " style="width: 100%; text-align: left;" type="button" class="button" value='<%= Resources.Resource.lbMembershipInfo %>' /></p>
                    <p style="margin: 0 0 3px 0;">
                        <input id="bHomeDetail" onclick=" showUserInfo('dHomeDetail'); " style="width: 100%;
                            text-align: left;" type="button" class="button" value='<%= Resources.Resource.lbHomeInfo %>' /></p>
                </td>
                <td>
                    <p style="margin: 0 0 3px 0;">
                        <input id="bJobDetail" onclick=" showUserInfo('dWorkDetail'); " style="width: 100%;
                            text-align: left;" type="button" class="button" value='<%= Resources.Resource.lbWorkInfo %>' /></p>
                    <p style="margin: 0 0 3px 0;">
                        <input id="Button1" onclick=" showUserInfo('dEmailAddress'); " style="width: 100%;
                            text-align: left;" type="button" class="button" value='<%= Resources.Resource.lbEmailInfo %>' /></p>
                    <p style="margin: 0 0 3px 0;">
                        <input id="btnFinanceDetail" onclick=" showUserInfo('dFinanceDetail'); " style="width: 100%;
                            text-align: left;" type="button" class="button" value='<%= Resources.Resource.lbDuesInfo %>' /></p>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel runat="server">
        <div id="dPersonalDetail" style="display:none;float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%= Resources.Resource.lbPersonalInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= AdminResource.lbTC %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lTC" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbTC" runat="server" onkeydown="return onlyNumber(event);" CssClass="editPersonalInfo"
                                         MaxLength="11" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="editPersonalInfo">
                        <td>
                            <%= AdminResource.lbName %>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbName" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbName"
                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="vgPers" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="editPersonalInfo">
                        <td>
                            <%= AdminResource.lbSurname %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbSurname" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbSurname"
                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="vgPers" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr class="editPersonalInfo">
                        <td>
                            <%= AdminResource.lbPassword %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbParola" runat="server" Width="200px"></asp:TextBox>
                            <input id="iGeneratePassword" value="<%= AdminResource.lbGeneratePassword %>"
                                   type="button" class="button" onclick=" generatePassword();return false; " />
                            <label id="result" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbGender %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lGender" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="editPersonalInfo" onchange="showHideAboutGender();" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbBloodType %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lBloodType" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlBloodType" CssClass="editPersonalInfo" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbFatherName %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lFatherName" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbFatherName" runat="server" Width="200px" CssClass="editPersonalInfo"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbMotherName %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lMotherName" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbMotherName" runat="server" Width="200px" CssClass="editPersonalInfo"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbMaritalStatus %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lMaritalStatus" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:DropDownList ID="ddlMaritalStatus" CssClass="editPersonalInfo" runat="server"
                                              onchange="showHideAboutGender();">
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
                            <asp:Label runat="server" ID="lMaidenName" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbMaidenName" runat="server" CssClass="editPersonalInfo" Width="200px"></asp:TextBox>
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
                            <asp:Label runat="server" ID="lMarriageDate" CssClass="viewPersonalInfo"></asp:Label>
                            <span class="editPersonalInfo">
                                <telerik:RadDatePicker ID="dpMarriageDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                                                       runat="server" ZIndex="30001">
                                </telerik:RadDatePicker>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbBirthDate %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lBirthDate" CssClass="viewPersonalInfo" />
                            <span class="editPersonalInfo">
                                <telerik:RadDatePicker ID="dpBirthDate" CssClass="editPersonalInfo" runat="server"
                                                       MaxDate="01-01-2200" MinDate="01-01-1900" ZIndex="30001">
                                </telerik:RadDatePicker> 
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbBirthPlace %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lBirthPlace" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbBirthPlace" runat="server" CssClass="editPersonalInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="editPersonalInfo">
                        <td>
                            <%= Resources.Resource.lbPhotoUrl %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <p class="editPersonalInfo">
                                <input type="file" id="iPhotoUrl" name="iPhotoUrl" multiple="false" accept="image/*" /></p>
                            <asp:HiddenField runat="server" ID="hdnActiveDirectory" />
                            <asp:HiddenField runat="server" ID="hdnUserProfilPhotoUrl" Value="hdnUserProfilPhotoUrl" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbHobby %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lHobbies" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbHobbies" TextMode="MultiLine" CssClass="editPersonalInfo" runat="server"
                                         Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbMemberFoundation %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lMemberFoundation" CssClass="viewPersonalInfo" />
                            <asp:TextBox runat="server" CssClass="editPersonalInfo" TextMode="MultiLine" ID="tbMemberFoundation" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbWeb %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWeb" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbWeb" runat="server" CssClass="editPersonalInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbGsmNo %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lGsmNo" onkeydown="return onlyNumber(event);" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbGsmNo" runat="server" CssClass="editPersonalInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbLastSchool %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lLastSchool" CssClass="viewPersonalInfo"></asp:Label>
                            <asp:TextBox ID="tbLastSchool" runat="server" CssClass="editPersonalInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbLastSchoolGraduateDate %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lLastSchoolGraduateDate" CssClass="viewPersonalInfo"></asp:Label>
                            <span class="editPersonalInfo">
                                <telerik:RadMonthYearPicker ID="dpLastSchoolGraduateDate" CssClass="editPersonalInfo"
                                                            runat="server" MaxDate="01-2200" MinDate="01-1900">
                                    <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                    </DateInput>
                                </telerik:RadMonthYearPicker>
                            </span>
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <asp:Button ID="btnSavePersonalInfo" runat="server" CssClass="button" OnClick="BtnSavePersonalInfoClick"
                                        ValidationGroup="vgPers" />
                            <asp:Button ID="btnEditPersonalInfo" runat="server" CssClass="button" OnClientClick=" editPersonalInfoClick(); return false; " />
                            <asp:Button ID="btnViewPersonalInfo" runat="server" CssClass="button" OnClientClick=" viewPersonalInfoClick(); return false; " />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lbError" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dHomeDetail" style="display:none;float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%= Resources.Resource.lbHomeInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= Resources.Resource.lbCountry %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lHomeCountry" CssClass="viewHomeInfo"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlHomeCountry" CssClass="editHomeInfo" AutoPostBack="true"
                                              OnSelectedIndexChanged="ddlHomeCountry_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbCity %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lHomeCity" CssClass="viewHomeInfo"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlHomeCity" CssClass="editHomeInfo" AutoPostBack="true"
                                              OnSelectedIndexChanged="ddlHomeCity_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbTown %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lHomeTown" CssClass="viewHomeInfo"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlHomeTown" CssClass="editHomeInfo" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbAddress %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lHomeAddress" CssClass="viewHomeInfo" />
                            <asp:TextBox ID="tbHomeAddress" runat="server" TextMode="MultiLine" CssClass="editHomeInfo"
                                         Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbZipCode %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lHomeZipCode" CssClass="viewHomeInfo" />
                            <asp:TextBox ID="tbHomeZipCode" runat="server" CssClass="editHomeInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbPhone %>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lHomePhone" onkeydown="return onlyNumber(event);" CssClass="viewHomeInfo" />
                            <asp:TextBox ID="tbHomePhone" runat="server" CssClass="editHomeInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbHidePersonalInfo %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbViewHidePersonalInfo" CssClass="viewHomeInfo"
                                          Enabled="False" />
                            <asp:CheckBox runat="server" ID="cbHidePersonalInfo" CssClass="editHomeInfo" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveHomeInfo" runat="server" CssClass="button" OnClick="BtnSaveHomeInfoClick"
                                        ValidationGroup="g2" />
                            <asp:Button ID="btnEditHomeInfo" runat="server" CssClass="button" OnClientClick=" editHomeInfoClick(); return false; " />
                            <asp:Button ID="btnViewHomeInfo" runat="server" CssClass="button" OnClientClick=" viewHomeInfoClick(); return false; " />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label1" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dWorkDetail" style="display:none;float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%= Resources.Resource.lbWorkInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= Resources.Resource.lbCountry %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkCountry" CssClass="viewWorkInfo"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlWorkCountry" CssClass="editWorkInfo" AutoPostBack="true"
                                              OnSelectedIndexChanged="ddlWorkCountry_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbCity %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkCity" CssClass="viewWorkInfo"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlWorkCity" CssClass="editWorkInfo" AutoPostBack="true"
                                              OnSelectedIndexChanged="ddlWorkCity_OnSelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbTown %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkTown" CssClass="viewWorkInfo"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlWorkTown" CssClass="editWorkInfo" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbAddress %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkAddress" CssClass="viewWorkInfo"></asp:Label>
                            <asp:TextBox ID="tbWorkAddress" TextMode="MultiLine" CssClass="editWorkInfo" runat="server"
                                         Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbZipCode %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkZipCode" CssClass="viewWorkInfo"></asp:Label>
                            <asp:TextBox ID="tbWorkZipCode" runat="server" CssClass="editWorkInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbPhone %>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkPhone" onkeydown="return onlyNumber(event);" CssClass="viewWorkInfo"></asp:Label>
                            <asp:TextBox ID="tbWorkPhone" runat="server" CssClass="editWorkInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbFax %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkFax" onkeydown="return onlyNumber(event);" CssClass="viewWorkInfo"></asp:Label>
                            <asp:TextBox ID="tbWorkFax" runat="server" CssClass="editWorkInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbJobSector %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lJobSectors" CssClass="viewWorkInfo"></asp:Label>
                            <asp:DropDownList runat="server" CssClass="editWorkInfo" ID="ddlJobSectors" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbJob %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lJobs" CssClass="viewWorkInfo"></asp:Label>
                            <asp:DropDownList runat="server" CssClass="editWorkInfo" ID="ddlJobs" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbWorkTitle %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkTitle" CssClass="viewWorkInfo"></asp:Label>
                            <asp:TextBox ID="tbWorkTitle" runat="server" CssClass="editWorkInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbWorkCorporation %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lWorkCorparation" CssClass="viewWorkInfo"></asp:Label>
                            <asp:TextBox ID="tbWorkCorparation" runat="server" CssClass="editWorkInfo" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbHideJobInfo %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbViewHideJobInfo" CssClass="viewWorkInfo" Enabled="False" />
                            <asp:CheckBox runat="server" ID="cbHideJobInfo" CssClass="editWorkInfo" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveWorkInfo" runat="server" CssClass="button" OnClick="BtnSaveWorkInfoClick"
                                        ValidationGroup="g3" />
                            <asp:Button ID="btnEditWorkInfo" runat="server" CssClass="button" OnClientClick="editWorkInfoClick(); return false;" />
                            <asp:Button ID="btnViewWorkInfo" runat="server" CssClass="button" OnClientClick="viewWorkInfoClick(); return false;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label2" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dEmailAddress" style="display:none;float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%= Resources.Resource.lbEmailInfo %></legend>
                <div id="dEmailProcessResult">
                    <asp:Literal runat="server" ID="ltUserEMailAddress" />
                </div>
                <table cellpadding="3">
                    <tr class="editEmailInfo">
                        <td>
                            <label>
                                <%= AdminResource.lbNewEmail %></label>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbNewEmailAddress" runat="server" />
                            &nbsp;
                            <asp:Button ID="btnSaveEmailInfo" runat="server" CssClass="button" OnClick="BtnSaveEmailInfoClick"
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
                            <asp:Button ID="btnEditEmailInfo" runat="server" CssClass="button" OnClientClick="editEmailInfoClick(); return false;" />
                            <asp:Button ID="btnViewEmailInfo" runat="server" CssClass="button" OnClientClick="viewEmailInfoClick(); return false;" />
                            <div id="results">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label3" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dMemberDetail" style="display:none;float: left; width: 100%;">
            <fieldset>
                <legend>
                    <%= Resources.Resource.lbMembershipInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= Resources.Resource.lbMemberNumber %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lMembershipNumber"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbRelationType %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lMemberRelationType" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbEnterDate %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lMembershipDate"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbSpecialNumber %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lSpecialNumber"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbTerm %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lTerm"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="dFinanceDetail" style="display:none;float: left; width: 100%;">
            <fieldset>
                <table>
                    <tr>
                        <td colspan="3">
                            <asp:Literal runat="server" ID="ltAutoPaymentOrder"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbPaymentAmount %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <%= (EnrollCurrency.SiteDefaultCurrency().Position == "L"
                              ? EnrollCurrency.SiteDefaultCurrencyUnit()
                              : "") %>
                            <asp:TextBox runat="server" ID="tbSpecificAmount" Width="100px" ValidationGroup="vgPayment"
                                         CssClass="deptAmount" onkeydown="return onlyNumber(event);" />,
                            <asp:TextBox runat="server" ID="tbSpecificAmount2" Width="20px" ValidationGroup="vgPayment"
                                         onkeydown="return onlyNumber(event);" MaxLength="2" />
                            <%= (EnrollCurrency.SiteDefaultCurrency().Position == "R"
                                     ? EnrollCurrency.SiteDefaultCurrencyUnit()
                                     : "") %>
                            <asp:RequiredFieldValidator ID="rValTbSpecificAmount" runat="server" ControlToValidate="tbSpecificAmount"
                                                        ErrorMessage="!" ForeColor="Red" SetFocusOnError="True" ValidationGroup="vgPayment"
                                                        Display="Static" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbSpecificAmount2"
                                                        ForeColor="Red" SetFocusOnError="True" ErrorMessage="!" ValidationGroup="vgPayment" Display="Static" />
                            <asp:Button runat="server" CssClass="button" ValidationGroup="vgPayment" ID="btPayCertainAmount"
                                        OnClick="BtPayCertainAmount_OnClick" />
                            <asp:Button runat="server" ID="btPayAllAmount" CssClass="button" OnClientClick="return CheckValidation();" OnClick="BtPayAllAmount_OnClick" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend><span style="cursor: pointer;" onclick="hideTable($(this), '<%= gvChargesForDues.ClientID %>')"
                              title='<%= AdminResource.lbHide %>'>
                            <%= AdminResource.lbChargesForDues %>
                        </span></legend>
                <asp:GridView runat="server" ID="gvChargesForDues" AllowSorting="False" AutoGenerateColumns="False"
                              ViewStateMode="Enabled" CellPadding="4" ForeColor="#333333" GridLines="None"
                              OnPageIndexChanged="gvChargesForDues_OnPageIndexChanged" DataSourceID="edsChargesForDues"
                              CssClass="GridViewStyle" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                              SortedAscendingHeaderStyle-CssClass="sortasc-header" SortedDescendingHeaderStyle-CssClass="sortdesc-header"
                              SortedAscendingCellStyle-CssClass="sortasc" SortedDescendingCellStyle-CssClass="sortdesc"
                              EditRowStyle-CssClass="edit" EmptyDataRowStyle-CssClass="empty" SelectedRowStyle="selected"
                              PageSize="15" AllowPaging="True" Width="100%">
                    <EmptyDataTemplate>
                        <%= AdminResource.lbNoRecord %>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#                GetDuesType(Convert.ToInt32(Eval("DuesType"))) %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="75" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#                GetPaymentType(Convert.ToInt32(Eval("PaymentTypeId"))) %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="60" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%#                EnrollMembershipHelper.AmountValue(Convert.ToDecimal(Eval("Amount")), Convert.ToInt32(Eval("LogType"))) %> 
                                <%--<span style='<%#(Convert.ToInt32(Eval("LogType"))==0? "color:red;": "color:green;")%>'>
                                    <%#string.Format("{0}{1}{2}{3}",
                                    (Convert.ToInt32(Eval("LogType"))==0? "":"-"),
                                    (EnrollCurrency.SiteDefaultCurrency().Position == "L" ? EnrollCurrency.SiteDefaultCurrencyUnit() : ""),
                                    Convert.ToDecimal(Eval("Amount")).ToString(".00"),
                                    (EnrollCurrency.SiteDefaultCurrency().Position == "R" ? EnrollCurrency.SiteDefaultCurrencyUnit() : ""))
                                    %>
                                </span>--%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="60px" />
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
                                <%#                (Convert.ToInt32(Eval("LogType")) == 0 ? Resources.Resource.lbDebiting : Resources.Resource.lbPayment) %>
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
                <asp:HiddenField runat="server" ID="hfMemberId" />
            </fieldset>
        </div>
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hfHomeCountry" />
    <asp:HiddenField runat="server" ID="hfHomeCity" />
    <asp:HiddenField runat="server" ID="hfHomeTown" />
    <asp:HiddenField runat="server" ID="hfWorkCountry" />
    <asp:HiddenField runat="server" ID="hfWorkCity" />
    <asp:HiddenField runat="server" ID="hfWorkTown" /> 
</asp:Content>