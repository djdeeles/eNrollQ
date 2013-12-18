<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_Calendar" Codebehind="Calendar.ascx.cs" %>
<%@ Import Namespace="Resources" %>
<div style="font-size: 14px; font-weight: bold; padding: 5px; text-align: center; width: 190px;"><%= Resource.lbCalenderTitle %></div>
<div style="width: 200px;">
    <div style="float: none; padding: 5px 15px;">
        <asp:Calendar ID="Calendar1" runat="server" CellPadding="4" Font-Bold="False" Font-Size="8pt" SelectionMode="None" Width="170px" OnDayRender="Calendar1_DayRender" BackColor="White" DayNameFormat="Shortest">
            <DayHeaderStyle BorderWidth="0px" BackColor="#fbfbfb" Font-Bold="False" Font-Size="7pt" />
            <DayStyle BorderWidth="1px" BorderColor="#bfbfbf" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#bbbbbb" />
            <SelectedDayStyle BackColor="#dfdfdf" Font-Bold="True" />
            <SelectorStyle BackColor="#dddddd" />
            <TitleStyle BackColor="#f0f0f0" BorderWidth="1px" BorderColor="#bfbfbf" Font-Bold="True" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="#333333" />
            <WeekendDayStyle BackColor="#efefef" />
        </asp:Calendar>
    </div>
    <div style="float: none; text-align: center">
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"><%= Resource.lbViewAll %></asp:LinkButton>
    </div>
</div>