<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
         CodeBehind="NewMember.aspx.cs" Inherits="eNroll.NewMember" %>
<%@ Import Namespace="Resources" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"
             ViewStateMode="Enabled">
    <script src="App_Themes/mainTheme/js/memberProfile.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#" + '<%= tbParola.ClientID %>').keyup(function() {
                $('#result').html(checkStrength($("#" + '<%= tbParola.ClientID %>').val()));
            });
        });
    </script>
    <script type="text/javascript">

        function onlyNumber(event) {
            var keyCode = event.keyCode;
            if ((keyCode < 46 || keyCode > 57) && keyCode != 8 && keyCode != 9 && keyCode != 0 && keyCode != 47 && (keyCode < 96 || keyCode > 105)) {
                return false;
            }
        }

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
        fieldset {
            border: 1px #ccc solid;
            margin: 0 0 10px 0;
            padding: 10px;
        }

        legend { font-weight: bold; }

        .hideaboutgender { display: none; }
    </style>
    <h1>
        <%= Resources.Resource.lbNewMembership %>
    </h1>

    <asp:MultiView ID="mvCreateMember" runat="server">
        <asp:View ID="vPersonalDetail" runat="server">
            <fieldset>
                <legend>
                    <%= Resources.Resource.lbPersonalInfo %></legend>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <%= AdminResource.lbName %>
                        </td>
                        <td style="width: 10px;">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbName" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbName"
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
                            <asp:TextBox ID="tbSurname" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbSurname"
                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= AdminResource.lbEmail %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEPosta" runat="server" Width="200px" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbEPosta"
                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbEPosta"
                                                            Display="Dynamic" ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbPassword %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbParola" runat="server" Width="200px"></asp:TextBox>
                            <input id="iGeneratePassword" type="button" value="<%= AdminResource.lbGeneratePassword %>"
                                   class="button" onclick="generatePassword();return false; " />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="tbParola"
                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g1" Display="Dynamic" />
                            <label id="result"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbTC %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="tbTC" runat="server" onkeydown="return onlyNumber(event);" MaxLength="11"
                                         Width="200px"></asp:TextBox>
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
                            <asp:DropDownList ID="ddlGender" runat="server" onchange="showHideAboutGender();" />
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
                            <asp:DropDownList runat="server" ID="ddlBloodType" />
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
                            <asp:DropDownList ID="ddlMaritalStatus" runat="server" onchange="showHideAboutGender();" />
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
                            <asp:TextBox ID="tbMaidenName" runat="server" Width="200px"></asp:TextBox>
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
                            <telerik:RadDatePicker ID="dpMarriageDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                                                   runat="server" ZIndex="30001">
                            </telerik:RadDatePicker>
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
                            <telerik:RadDatePicker ID="dpBirthDate" runat="server" MaxDate="01-01-2200" MinDate="01-01-1900"
                                                   ZIndex="30001">
                            </telerik:RadDatePicker>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                                        ControlToValidate="dpBirthDate" ValidationGroup="g1" ErrorMessage="!">
                            </asp:RequiredFieldValidator>
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
                            <asp:TextBox ID="tbBirthPlace" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:TextBox runat="server" ID="tbMemberFoundation" TextMode="MultiLine" />
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
                            <asp:TextBox ID="tbGsmNo" onkeydown="return onlyNumber(event);" MaxLength="11" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:TextBox ID="tbLastSchool" runat="server" Width="200px"></asp:TextBox>
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
                            <telerik:RadMonthYearPicker ID="dpLastSchoolGraduateDate" runat="server" MaxDate="01-2200"
                                                        MinDate="01-1900">
                                <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                </DateInput>
                            </telerik:RadMonthYearPicker>
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <asp:Button ID="btnSavePersonalInfo" runat="server" CssClass="button" OnClick="BtnSavePersonalInfoClick"
                                        ValidationGroup="g1" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lbError" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:View>
        <asp:View ID="vMemberInfo" runat="server">
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
                            <asp:TextBox ID="tbMembershipNumber" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:DropDownList runat="server" ID="ddlMembershipRelType" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlMembershipRelType"
                                                        ErrorMessage="!" ForeColor="Red" ValidationGroup="g4" Display="Dynamic" />
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
                            <telerik:RadDatePicker ID="dpMembershipDate" MaxDate="01-01-2200" MinDate="01-01-1900"
                                                   runat="server" ZIndex="30001">
                            </telerik:RadDatePicker>
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
                            <asp:TextBox ID="tbSpecialNumber" runat="server" Width="200px"></asp:TextBox>
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
                            <telerik:RadMonthYearPicker ID="dpTerm" runat="server" MaxDate="01-2200" MinDate="01-1900">
                                <DateInput DateFormat="yyyy" DisplayDateFormat="yyyy">
                                </DateInput>
                            </telerik:RadMonthYearPicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%= Resources.Resource.lbTermLeader %>
                        </td>
                        <td>
                            :
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbIsTermLider" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <asp:Button ID="btnMemberInfoGoBack" runat="server" CssClass="button" OnClick="BtnMemberInfoGoBackClick" />
                            <asp:Button ID="btnSaveMemberInfo" runat="server" CssClass="button" OnClick="BtnSaveMemberInfoClick"
                                        ValidationGroup="g4" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label3" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:View>
        <asp:View ID="vHomeDetail" runat="server">
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
                            <asp:DropDownList runat="server" ID="ddlHomeCountry" AutoPostBack="true" OnSelectedIndexChanged="ddlHomeCountry_OnSelectedIndexChanged" />
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
                            <asp:DropDownList runat="server" ID="ddlHomeCity" AutoPostBack="true" OnSelectedIndexChanged="ddlHomeCity_OnSelectedIndexChanged" />
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
                            <asp:DropDownList runat="server" ID="ddlHomeTown" />
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
                            <asp:TextBox ID="tbHomeAddress" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:TextBox ID="tbHomeZipCode" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:TextBox ID="tbHomePhone" onkeydown="return onlyNumber(event);" runat="server"
                                         Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <asp:Button ID="btnHomeInfoGoBack" runat="server" CssClass="button" OnClick="BtnHomeInfoGoBackClick" />
                            <asp:Button ID="btnSaveHomeInfo" runat="server" CssClass="button" OnClick="BtnSaveHomeInfoClick"
                                        ValidationGroup="g2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label1" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:View>
        <asp:View ID="vJobDetail" runat="server">
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
                            <asp:DropDownList runat="server" ID="ddlWorkCountry" AutoPostBack="true" OnSelectedIndexChanged="ddlWorkCountry_OnSelectedIndexChanged" />
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
                            <asp:DropDownList runat="server" ID="ddlWorkCity" AutoPostBack="true" OnSelectedIndexChanged="ddlWorkCity_OnSelectedIndexChanged" />
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
                            <asp:DropDownList runat="server" ID="ddlWorkTown" />
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
                            <asp:TextBox ID="tbWorkAddress" TextMode="MultiLine" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:TextBox ID="tbWorkZipCode" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:TextBox ID="tbWorkPhone" onkeydown="return onlyNumber(event);" runat="server"
                                         Width="200px"></asp:TextBox>
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
                            <asp:TextBox ID="tbWorkFax" onkeydown="return onlyNumber(event);" runat="server"
                                         Width="200px"></asp:TextBox>
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
                            <asp:DropDownList runat="server" ID="ddlJobSectors" />
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
                            <asp:DropDownList runat="server" ID="ddlJobs" />
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
                            <asp:TextBox ID="tbWorkTitle" runat="server" Width="200px"></asp:TextBox>
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
                            <asp:TextBox ID="tbWorkCorparation" runat="server" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table cellpadding="3">
                    <tr>
                        <td>
                            <asp:Button ID="btnJobInfoGoBack" runat="server" CssClass="button" OnClick="BtnJobInfoGoBackClick" />
                            <asp:Button ID="btnSaveJobInfo" runat="server" CssClass="button" OnClick="BtnSaveJobInfoClick"
                                        ValidationGroup="g3" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="Label2" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:View>
        <asp:View ID="vCreateMemberResult" runat="server">
            <asp:Literal ID="ltNewMemberResult" runat="server"></asp:Literal>
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField runat="server" ID="hfHomeCountry" />
    <asp:HiddenField runat="server" ID="hfHomeCity" />
    <asp:HiddenField runat="server" ID="hfHomeTown" />
    <asp:HiddenField runat="server" ID="hfWorkCountry" />
    <asp:HiddenField runat="server" ID="hfWorkCity" />
    <asp:HiddenField runat="server" ID="hfWorkTown" />
</asp:Content>