<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DynamicForm.ascx.cs"
            Inherits="UserControls_DynamicForm" %>
<script type="text/javascript" src="http://ajax.microsoft.com/ajax/jQuery.Validate/1.11.1/jQuery.Validate.min.js"></script>
<script type="text/javascript">

    function validate(evt) {
        var validator = $("#form1").validate();

        window.jQuery.extend(window.jQuery.validator.messages, {
            required: '!',
            email: '!'/*,
            remote: "..........",
            url: "Please enter a valid URL.",
            date: "Please enter a valid date.",
            dateISO: "Please enter a valid date (ISO).",
            number: "Please enter a valid number.",
            digits: "Please enter only digits.",
            creditcard: "Please enter a valid credit card number.",
            equalTo: "Please enter the same value again.",
            accept: "Please enter a value with a valid extension.",
            maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
            minlength: jQuery.validator.format("Please enter at least {0} characters."),
            rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
            range: jQuery.validator.format("Please enter a value between {0} and {1}."),
            max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
            min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")*/
        });
        if (validator.form()) { // validation perform
            return true;
        } else {
            return false;
        }
    }
</script>
<table class="dynamicform">
    <asp:Literal ID="ltForm" runat="server" />
    <tr>
        <td>
            <%= Resources.Resource.lbCapthcaMessage %>
        </td>
        <td width="10px">
            :
        </td>
        <td>
            <telerik:RadCaptcha ID="RadCaptcha1" runat="server" CssClass="Captcha" CaptchaTextBoxLabelCssClass="CaptchaTextBoxLabel"
                                CaptchaTextBoxCssClass="CaptchaTextBox" EnableRefreshImage="False" ImageStorageLocation="Cache"
                                ValidationGroup="captcaValidation">
                <CaptchaImage Width="130" Height="31" />
            </telerik:RadCaptcha>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Button ID="btFormSubmit" CssClass="FormSubmit" OnClick="BtFormSubmitClick" OnClientClick="return validate(); "
                        ValidationGroup="captcaValidation" runat="server"></asp:Button>
        </td>
    </tr>
</table>
<asp:Label ID="mailSendReport" runat="server"></asp:Label>