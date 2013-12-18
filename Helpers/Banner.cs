using System;
using System.Linq;
using System.Web.UI.WebControls;
using eNroll.App_Data;

/// <summary>
///   Summary description for Banner
/// </summary>
public class SiteBanner
{
    public void BannerGetir(Panel pnl, int locationId)
    {

        var ent = new Entities();
        BannerManagement bman =
            ent.BannerManagement.OrderBy(p => Guid.NewGuid()).FirstOrDefault(
                p =>
                p.state == true && p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId &&
                p.bannerLocationId == locationId);
        if (bman != null)
        {
            Banners banner = ent.Banners.First(p => p.bannersId == bman.bannersId);
            if (bman.Banners.bannerFileTypeId == 1)
            {
                var ltResim = new Literal();
                if (banner.bannerUrl.StartsWith("http"))
                {
                    ltResim.Text = "<a href='" + banner.bannerUrl + "' target='_blank'>" +
                                   "<img src='" + banner.bannerSource.Replace("~/", "") + "' alt='" + banner.bannerName +
                                   "'" + " /></a>";
                }
                else if (banner.bannerUrl.StartsWith("www"))
                {
                    ltResim.Text = "<a href='http://" + banner.bannerUrl + "' target='_blank'>" +
                                   "<img src='" + banner.bannerSource.Replace("~/", "") + "' alt='" + banner.bannerName +
                                   "'" + " /></a>";
                }
                else
                {
                    ltResim.Text = "<a href='/" + banner.bannerUrl + "'>" +
                                   "<img src='" + banner.bannerSource.Replace("~/", "") + "' alt='" + banner.bannerName +
                                   "'" + " /></a>";
                }
                pnl.Controls.Add(ltResim);
                if (bman.bannerCounter == null) bman.bannerCounter = 0;
                bman.bannerCounter = bman.bannerCounter + 1;
                if (bman.bannerEndDate < DateTime.Now || bman.bannerLimit <= bman.bannerCounter) bman.state = false;
                ent.SaveChanges();
            }
            if (bman.Banners.bannerFileTypeId == 2)
            {
                var lt = new Literal();
                int w = banner.bannerWidth.Value;
                int h = banner.bannerHeight.Value;
                string path = banner.bannerSource.Replace("~/", "");
                lt.Text = "<object type='application/x-shockwave-flash' data='" + path + "' width='" + w + "' height='" +
                          h + "'>" +
                          "<param name='movie' value='" + path + "'/>" +
                          "<param name='wmode' value='transparent'/>" +
                          "<param name='quality' value='high' />" +
                          "</object>";
                pnl.Controls.Add(lt);
                if (bman.bannerCounter == null) bman.bannerCounter = 0;
                bman.bannerCounter = bman.bannerCounter + 1;
                if (bman.bannerEndDate < DateTime.Now || bman.bannerLimit <= bman.bannerCounter) bman.state = false;
                ent.SaveChanges();
            }
        }
    }
}