using System;
using System.Web.UI;

public partial class Banner_Bottom : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new SiteBanner().BannerGetir(Panel1, 1);
    }
}