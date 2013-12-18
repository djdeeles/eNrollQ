<%@ Control Language="C#" AutoEventWireup="true"
            Inherits="M_UserControls_SearchControl" Codebehind="SearchControl.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<script src="App_Themes/mainTheme/js/mobileSearch.js" type="text/javascript"></script>
<script type="text/javascript">
    function yaz(obj) {
        if (obj.value == "") {
            obj.value = '<%= Resource.lbSearchInSite %>';

            $("#searchText").addClass("searchbox");
        }
    }

    function temizle(obj) {
        if (obj.valueOf != "") {
            obj.value = "";
            $("#searchText").removeClass("searchbox");
        }
    }

</script>
<input type="search" id="searchText" onkeypress="Search_m(event); "
       onclick=" temizle(this); " value='<%= Resource.lbSearchInSite %>' class="searchbox" onmouseout=" yaz(this); " />