<%@ Control Language="C#" AutoEventWireup="true" Inherits="M_UserControls_SSS" Codebehind="SSS.ascx.cs" %>
<script type="text/javascript">
    $(document).ready(function() {
        /*
			if ($("#faqcat")) {
                $("#faqcat dd").hide();
                $("#faqcat dt b").click(function () {
                    if (this.className.indexOf("clicked") != -1) {
                        $(this).parent().next().slideUp(100);
                        $(this).removeClass("clicked");
                    } else {
                        $("#faqcat dt b").removeClass();
                        $(this).addClass("clicked");
                        $("#faqcat dd:visible").slideUp(100);
                        $(this).parent().next().slideDown(300);
                    }
                    return false;
                });
            }
			*/
        if ($("#faq")) {
            $("#faq dd").hide();
            $("#faq dt b").click(function() {
                if (this.className.indexOf("clicked") != -1) {
                    $(this).parent().next().slideUp(100);
                    $(this).removeClass("clicked");
                } else {
                    $("#faq dt b").removeClass();
                    $(this).addClass("clicked");
                    $("#faq dd:visible").slideUp(100);
                    $(this).parent().next().slideDown(300);
                }
                return false;
            });
        }
    });
</script>
<style type="text/css">
    #faqcat {
        margin: 0;
        padding: 0;
    }

    #faqcat dt b, #nav dt a {
        cursor: pointer;
        display: inline;
        height: 23px;
        line-height: 23px;
    }

    #faqcat dt { }

    #faqcat dd {
        margin: 0;
        padding: 0;
    }

    #faqcat dd ul {
        list-style: none;
        margin: 0;
        padding: 0;
    }

    #faqcat dd ul li {
        margin-left: 18px;
        padding-left: 10px;
    }

    #faq {
        margin: 0;
        padding: 0;
    }

    #faq dt b, #nav dt a {
        cursor: pointer;
        display: inline;
        height: 23px;
        line-height: 23px;
    }

    #faq dd {
        margin: 2px 0px 0px 10px;
        padding: 0;
    }
</style>
<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>