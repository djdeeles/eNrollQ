using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

public partial class UserControls_Calendar : UserControl
{
    private readonly Entities ent = new Entities();
    private List<Events> etkinlikler;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
    {
        etkinlikler =
            ent.Events.Where(p => p.State == true && p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId).
                ToList();
        foreach (Events olay in etkinlikler)
        {
            if (e.Day.Date.ToShortDateString() == Convert.ToDateTime(olay.StartDate).ToShortDateString())
            {
                if (e.Day.IsSelected) Response.Redirect("/etkinlik-" + olay.id + "-" + UrlMapping.cevir(olay.Name));
                e.Day.IsSelectable = true;
                e.Cell.ToolTip = olay.Name;
                e.Cell.ForeColor = Color.OrangeRed;
            }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("/etkinlikler-1");
    }
}