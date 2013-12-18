$(document).ready(function () {
    $("a[rel^='prettyPhoto']").prettyPhoto({ social_tools: false });
    //Menu
    if ($("#nav")) {
        $("#nav dd").hide();
        $("#nav dt b").click(function () {
            if (this.className.indexOf("clicked") != -1) {
                $(this).parent().next().slideUp(100);
                $(this).removeClass("clicked");
            } else {
                $("#nav dt b").removeClass();
                $(this).addClass("clicked");
                $("#nav dd:visible").slideUp(100);
                $(this).parent().next().slideDown(300);
            }
            return false;
        });
    }
    //Menu Sonu
});

function update(returnField, returnValue) {
    document.getElementById(returnField).value = returnValue;
}

function onlyNumber(event) {
    var keyCode = event.keyCode;
    if ((keyCode < 46 || keyCode > 57) && keyCode != 8 && keyCode != 9 && keyCode != 0 && keyCode != 47 && (keyCode < 96 || keyCode > 105)) {
        return false;
    }
}

function cancelEnterKey(event, data) {

    var keyCode = event.keyCode;

    if (keyCode == 13) {
        return false;
    }
    else return true;
}

function priceInputsCharacters(event, data) {

    var keyCode = event.keyCode;
    var dataLength = data.value.split(",").length;
    if (keyCode == 188 || keyCode == 110) {
        
        if (dataLength > 1) return false;
    }

    if ((keyCode < 46 || keyCode > 57) && keyCode != 8 && keyCode != 9 && keyCode != 0 && keyCode != 47 && (keyCode < 96 || keyCode > 105) && keyCode != 188 && keyCode != 110) {
        return false;
    }
}

function priceInputsValidation(price, currency) {
    if (price.value == "" && (currency.selectedIndex != 0)) {
        alert("Fiyat bilgisi girmediniz.");
        return false;
    }
    else if (price.value != "" && (currency.selectedIndex == 0)) {
        alert("Para birimi alanını boş bıraktınız.");
        return false;
    }
    return true;
}

/*form content options delete <begin>*/
function removeSelectOption(formid, contentid, control, deleteComfirmMsg) {
        var strconfirm = confirm(deleteComfirmMsg);
        if (strconfirm == true) {
            var x = document.getElementById(control);
            var index = x.selectedIndex;
            removeOptn(formid, contentid, x.value);
            x.remove(index);
        }
    }
    
    function removeRadioOption(formid, contentid, control, deleteComfirmMsg) {
        var strconfirm = confirm(deleteComfirmMsg);
        if (strconfirm == true) {
            var item = $('input[name=' + control + ']').filter(':checked');

            var checkedValue = item.val();
            removeOptn(formid, contentid, checkedValue);

            $('input[name=' + control + ']').filter(':checked').parent().remove();
        }
    }
    
    function removeCheckOption(formid, contentid, control, deleteComfirmMsg) {
        var strconfirm = confirm(deleteComfirmMsg);
        if (strconfirm == true) {
            var item = $('input[name=' + control + ']').filter(':checked');

            var checkedValue = item.val();
            removeOptn(formid, contentid, checkedValue);

            $('input[name=' + control + ']').filter(':checked').each(function() {
                $(this).parent().remove();
            });
        }
    }

    function removeOptn(formid, contentid, value) {
        
        var url = "FormHandler.ashx";
        $.ajax({
            type: "POST",
            dataType: "json",
            url: url,
            data: { 'formId': formid, 'contentId': contentid, 'value': value },
            success: OnComplete,
            error: OnFail,
        });
    }

    function OnComplete(result) {
//        alert('Success');
//        alert(response);
//          x.remove(index);
    }
    function OnFail(result) {
//        alert('Request failed');

    }
/*form content options delete <end>*/

/*telerik file selector <begin>*/

        //<![CDATA[
//A function that will return a reference to the parent radWindow in case the page is loaded in a RadWindow object

var selectedFile; 
function getRadWindow() {
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
}

function OnClientFileOpen(sender, args) {// Called when a file is open.
    var item = args.get_item(); 
    //If file (and not a folder) is selected - call the OnFileSelected method on the parent page
    if (item.get_type() == Telerik.Web.UI.FileExplorerItemType.File) {
        // Cancel the default dialog;
        args.set_cancel(true);

        // get reference to the RadWindow
        var wnd = getRadWindow();
        
        //Get a reference to the opener parent page using RadWndow
        var openerPage = wnd.BrowserWindow;
        
        //if you need the URL for the item, use get_url() instead of get_path()
        openerPage.OnFileSelected(item.get_path()); // Call the method declared on the parent page    
        
        //Close the window which hosts this page
        wnd.close();
    }
}

//This function is called from a code declared on the Explorer.aspx page
function OnFileSelected(fileSelected) {
    var textbox = selectedFile;
    textbox.set_value("~" + fileSelected);
}
    //]]>
/*telerik file selector <end>*/

/*toast message*/
function showSuccessToast(message) {
	$().toastmessage('showSuccessToast', message);
}
function showNoticeToast(message) {
	$().toastmessage('showNoticeToast', message);
}
function showSuccessToast(message) {
	$().toastmessage('showSuccessToast', message);
}
function showErrorToast(message) {
	$().toastmessage('showErrorToast', message);
}
function showWarningToast(message) {
	$().toastmessage('showWarningToast', message);
}

function validateIpAddress(txtIpAddres) {
    var ipText = txtIpAddres.value;
    var regE = /^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}$/;
    if(ipText.match(regE) && ipText!="")
        return true;  
    else
        return false;
}