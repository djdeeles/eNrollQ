using System;
using System.Linq;
using System.Web.UI;
using eNroll.App_Data;

public partial class UserControls_base_Sitemappath : UserControl
{
    private string text = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        int id;
        if (!String.IsNullOrEmpty(Request.QueryString["id"]))
        {
            var ent = new Entities();
            id = Convert.ToInt32(Request.QueryString["id"]);
            System_menu current = ent.System_menu.Where(p => p.menuId == id).First();
            string asd = getParent(current.MasterId.Value);
            string[] pathList = asd.Split('|');
            string txt = string.Empty;
            foreach (string path in pathList.Reverse())
            {
                txt += path + " » ";
            }
            txt += "<span class='sitemappathactive'>" + current.name + "</span> ";

            Literal1.Text = string.Format("<div class='sitemappath'>{0}</div>", txt);
        }
    }

    public string getParent(int masterId)
    {
        var ent = new Entities();
        System_menu prnt = ent.System_menu.Where(p => p.menuId == masterId).FirstOrDefault();
        if (prnt != null)
        {
            if (prnt.MasterId != 0)
            {
                if (prnt.type == "0") text += "<a>" + prnt.name + "</a>|";
                else
                    text += "<a href='/sayfa-" + prnt.menuId + '-' + UrlMapping.cevir(prnt.name) + "'>" + prnt.name +
                            "</a>|";
                getParent(prnt.MasterId.Value);
            }
            else
            {
                if (prnt.type == "0") text += "<a>" + prnt.name + "</a>";
                else
                    text += "<a href='/sayfa-" + prnt.menuId + '-' + UrlMapping.cevir(prnt.name) + "'>" + prnt.name +
                            "</a>";
            }
        }
        return text;
    }
}