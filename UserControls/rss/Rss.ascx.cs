using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Enroll.Managers;
using eNroll.App_Data;

public partial class UserControls_Rss : UserControl
{
    private readonly Entities _entities = new Entities();
    public Localizations _localizations = new Localizations();
    protected List<RssItem> RssItems = null;
    public Rss SelectedRss = null;
    private int _rssId = -1;

    public int RssId
    {
        get { return _rssId; }
        set { _rssId = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["rssId"]))
            {
                _rssId = Convert.ToInt32(Request.QueryString["rssId"]);
            }

            RssItems = RssGetir(_rssId);

            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = _entities.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) Page.Title = site.title + " - " + SelectedRss.Name;
            MetaGenerate.SetMetaTags(site, Page);


            if (SelectedRss.IsScroll != null && (bool) SelectedRss.IsScroll)
            {
                ltRssItemsScroll.Visible = true;
                ListView1.Visible = false;
            }
            else
            {
                ltRssItemsScroll.Visible = false;

                ListView1.DataSource = RssItems;
                ListView1.DataBind();
                ListView1.Visible = true;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    public List<RssItem> RssGetir(int rssId)
    {
        List<RssItem> feeds = null;
        var ent = new Entities();
        SelectedRss =
            ent.Rss.FirstOrDefault(
                p => p.State == true && p.Language == EnrollContext.Current.WorkingLanguage.LanguageId && p.Id == rssId);
        if (SelectedRss != null)
        {
            int maxRssCount = 1000;
            if (SelectedRss.MaxItem != null) maxRssCount = SelectedRss.MaxItem.Value;
            feeds = ReadFeeds(SelectedRss.Url, maxRssCount, (bool) SelectedRss.IsScroll);
        }
        return feeds;
    }

    private List<RssItem> ReadFeeds(string rssUrl, int rssMaxCount, bool scroll)
    {
        string uri = @rssUrl;
        List<RssItem> rssFeeds = null;
        try
        {
            var client = new WebClient();
            client.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            client.Encoding = Encoding.UTF8;
            string result = client.DownloadString(new Uri(uri, UriKind.Absolute));

            var rssReader = XDocument.Parse(result);

            rssFeeds = (from rssItems in rssReader.Descendants("item")
                        select new RssItem
                                   {
                                       Title = null != rssItems.Descendants("title").FirstOrDefault()
                                                   ? rssItems.Descendants("title").First().Value
                                                   : string.Empty,
                                       Link = null != rssItems.Descendants("link").FirstOrDefault()
                                                  ? rssItems.Descendants("link").First().Value
                                                  : string.Empty,
                                       Description = null != rssItems.Descendants("description").FirstOrDefault()
                                                         ? rssItems.Descendants("description").First().Value
                                                         : string.Empty,
                                   }).ToList();

            if (!scroll) return rssFeeds;

            var builder = new StringBuilder();
            int rssCount = 0;
            foreach (var item in rssFeeds)
            {
                if (rssCount >= rssMaxCount) break;
                builder.AppendFormat("<p><a href='{0}'>{1}</a></p>", item.Link, item.Title);
                rssCount++;
            }
            if (scroll)
            {
                ltRssItemsScroll.Text =
                    string.Format(
                        "<div id='rss'><div class='rssTepe'></div><div class='rssGovde'><marquee width='100%' direction='up' scrollamount='1' " +
                        "height='100%' onmouseover=' this.stop();' onmouseout='this.start();'>{0}</marquee></div><div class='rssAlt'></div></div>",
                        builder);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        //return builder.ToString();

        return rssFeeds;
    }


    protected void DataPager1_Init(object sender, EventArgs e)
    {
        _localizations.ChangeDataPager((DataPager) sender);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataPager1.PreRender += DataPager1_PreRender;
    }

    private void DataPager1_PreRender(object sender, EventArgs e)
    {
        foreach (Control control in DataPager1.Controls)
        {
            foreach (Control c in control.Controls)
            {
                if (c is HyperLink)
                {
                    var currentLink = (HyperLink) c;
                    if ((!string.IsNullOrEmpty(Request.Url.AbsolutePath)) && (!string.IsNullOrEmpty(Request.Url.Query)))
                    {
                        if (Request.Url.AbsolutePath != "/Rss.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../rss-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../rss-" + _rssId + "-");
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("page=", "");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/Rss.aspx?", "/rss-");
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("page=", "-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("rssId=", "");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&", "");
                    }
                }
            }
        }
    }

    #region Nested type: RssFeed

    public class RssFeed
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string PubDate { get; set; }

        public string Language { get; set; }

        public ObservableCollection<RssItem> RssItems { get; set; }
    }

    #endregion

    #region Nested type: RssItem

    public class RssItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }
    }

    #endregion
}