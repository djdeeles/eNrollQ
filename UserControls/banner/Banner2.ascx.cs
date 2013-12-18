using System;
using System.Web.UI;

public partial class UserControls_banner_Banner2 : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new SiteBanner().BannerGetir(Panel1, 2);
    }
}