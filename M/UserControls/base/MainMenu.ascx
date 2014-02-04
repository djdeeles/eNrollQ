<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_base_MainMenu"
            CodeBehind="MainMenu.ascx.cs" %>
<%@ Import Namespace="Enroll.BaseObjects" %>
<%@ Import Namespace="Resources" %>
<script type="text/javascript">

    function go(control) {
        try {
            var data = control.value.split("|");
            ;
            var type = data[0];
            var url = data[1];

            if (type == '3') {
                window.open(url, '_newtab');
            } else {
                window.location = url;
            }
        } catch(err) {

        }
    }

</script>
<select name="Menu" class="ui-btn" id="Menu" data-corners="false" onchange=" go(this.options[this.selectedIndex]) ">
    <option>
        <%= Resource.lbMenu %></option>
    <% if (MenuList0.Count > 0)
       {%>
        <optgroup label="-">
            <% foreach (var menu in MenuList0)
               {%>
                <option value="<%= menu.type + '|' + DropMenuControlBase.DetectMobileMenuType(menu) %>">
                    <%= menu.name %></option>
            <% } %>
        </optgroup>
    <% } %>
    <% if (MenuList1.Count > 0)
       {%>
        <optgroup label="-">
            <% foreach (var menu in MenuList1)
               {%>
                <option value="<%= menu.type + '|' + DropMenuControlBase.DetectMobileMenuType(menu) %>">
                    <%= menu.name %></option>
            <% } %>
        </optgroup>
    <% } %>
    <% if (MenuList3.Count > 0)
       {%>
        <optgroup label="-">
            <% foreach (var menu in MenuList3)
               {%>
                <option value="<%= menu.type + '|' + DropMenuControlBase.DetectMobileMenuType(menu) %>">
                    <%= menu.name %></option>
            <% } %>
        </optgroup>
    <% } %>
</select>