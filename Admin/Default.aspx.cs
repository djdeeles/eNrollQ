using System;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;
using Resources;

public partial class Admin_Default : Page
{
    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbAdminPanel;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rssAl();
        }
    }

    private void rssAl()
    {
        try
        {
            XDocument rss = XDocument.Load("http://www.enroll.com.tr/feed");
            var rst = from x in rss.Elements("rss").Elements("channel").Elements("item")
                      select new
                                 {
                                     baslik = x.Element("title").Value,
                                     ozet = x.Element("description").Value,
                                     link = x.Element("link").Value
                                 };
            if (rst.ToList().Count != 0)
            {
                foreach (var i in rst)
                {
                    Panel1.Controls.Add(
                        new LiteralControl("<div class='haberler'><a href='" + i.link +
                                           "' target='_blank'>&nbsp;»&nbsp;" + i.baslik + "</a><br />"));
                    Panel1.Controls.Add(new LiteralControl("<i>" + i.ozet + "</i></div>"));
                }
            }
        }
        catch
        {
        }
    }
}