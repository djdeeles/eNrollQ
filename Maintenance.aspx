<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs"
    Inherits="Maintenance" %>

<%@ Register TagPrefix="uc1" TagName="Analytics_" Src="~/UserControls/base/Analytics.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="App_Themes/mainTheme/css/mainTheme.css" type="text/css" />
    <link rel="stylesheet" href="App_Themes/mainTheme/css/layout.css" type="text/css" />
    <title>Site Bakımda!</title>
    <uc1:Analytics_ runat="server" />
</head>
<body>
    <div class="hata">
        <p class="bakimtitle">
            Site Bakımda!</p>
        <p class="bakimmsg">
            Sitede bakım çalışması vardır. Lütfen daha sonra tekrar deneyiniz.</p>
    </div>
</body>
</html>
