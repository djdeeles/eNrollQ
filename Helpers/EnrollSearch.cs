using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Resources;
using eNroll.App_Data;

namespace Enroll.WebParts
{
    public class EnrollSearch
    {
        public static List<Search> SearchList { get; set; }
        public static List<Search> ResultList { get; set; }

        public static string QueryStringeCevir(string AranacakKelime)
        {
            AranacakKelime = AranacakKelime.ToLower();
            AranacakKelime = AranacakKelime.Replace(" ", "-");
            AranacakKelime = AranacakKelime.Replace("ı", "i_");
            AranacakKelime = AranacakKelime.Replace("ğ", "g_");
            AranacakKelime = AranacakKelime.Replace("ü", "u_");
            AranacakKelime = AranacakKelime.Replace("ş", "s_");
            AranacakKelime = AranacakKelime.Replace("ö", "o_");
            AranacakKelime = AranacakKelime.Replace("ç", "c_");
            AranacakKelime = AranacakKelime.Replace("<", "");
            AranacakKelime = AranacakKelime.Replace(">", "");
            AranacakKelime = AranacakKelime.Replace(";", "");
            AranacakKelime = AranacakKelime.Replace(",", "");
            AranacakKelime = AranacakKelime.Replace("`", "");
            AranacakKelime = AranacakKelime.Replace("'", "");
            AranacakKelime = AranacakKelime.Replace("!", "");
            AranacakKelime = AranacakKelime.Replace("/", "");
            AranacakKelime = AranacakKelime.Replace("\\", "");
            AranacakKelime = AranacakKelime.Replace("%", "");
            AranacakKelime = AranacakKelime.Replace("^", "");
            AranacakKelime = AranacakKelime.Replace("\"", "");
            return AranacakKelime;
        }

        public static string qStringDuzelt(string s)
        {
            s = s.ToLower();
            s = s.Replace("-", " ");
            s = s.Replace("i_", "ı");
            s = s.Replace("g_", "ğ");
            s = s.Replace("u_", "ü");
            s = s.Replace("s_", "ş");
            s = s.Replace("o_", "ö");
            s = s.Replace("c_", "ç");
            return s;
        }

        public static List<Search> GetSearchList(string ArananKelime)
        {
            var ent = new Entities();
            SearchList = new List<Search>(); 
            var dilId = EnrollContext.Current.WorkingLanguage.LanguageId;
            var menuResult = ent.SP_System_menuSearch(dilId, ArananKelime).ToList();
            var newsResult = ent.SP_NewsSearch(dilId, ArananKelime).ToList();
            var noticesResult = ent.SP_NoticesSearch(dilId, ArananKelime).ToList();
            var eventResult = ent.SP_EventsSearch(dilId, ArananKelime).ToList();
            var productResult = ent.SP_ProductsSearch(dilId, ArananKelime).ToList();
            var dynamicListResult = ent.SP_ListDataSearch(dilId, ArananKelime).ToList();
            var customerRandomResult = ent.SP_Customer_Random(dilId, ArananKelime).ToList();
            foreach (System_menu m in menuResult)
            {
                string details = HtmlRemoval.StripTagsRegexCompiled(m.Details);
                var s = new Search(Resource.lbSearchResultMenu, "sayfa-" + m.menuId + "-" + UrlMapping.cevir(m.name),
                                   m.brief, details, m.name);
                SearchList.Add(s);
            }
            foreach (News ne in newsResult)
            {
                string details = HtmlRemoval.StripTagsRegexCompiled(ne.details);
                var s = new Search(Resource.lbSearchResultNews,
                                   "haberdetay-" + ne.newsId + "-" + UrlMapping.cevir(ne.header), ne.brief, details,
                                   ne.header);
                SearchList.Add(s);
            }
            foreach (Notices no in noticesResult)
            {
                string details = HtmlRemoval.StripTagsRegexCompiled(no.details);
                var s = new Search(Resource.lbSearchResultNotices,
                                   "duyurudetay-" + no.noticeId + "-" + UrlMapping.cevir(no.header), no.description,
                                   details,
                                   no.header);
                SearchList.Add(s);
            }
            foreach (Events no in eventResult)
            {
                string details = HtmlRemoval.StripTagsRegexCompiled(no.Details);
                var s = new Search(Resource.lbSearchResultEvents, "etkinlik-" + no.id + "-" + UrlMapping.cevir(no.Name),
                                   no.Description, details,
                                   no.Name);
                SearchList.Add(s);
            }
            foreach (Products no in productResult)
            {
                string description = HtmlRemoval.StripTagsRegexCompiled(no.Description);
                var s = new Search(Resource.lbSearchResultProducts,
                                   "urundetay-" + no.ProductId.ToString() + "-" + UrlMapping.cevir(no.Name), "",
                                   description,
                                   no.Name);
                SearchList.Add(s);
            }
            foreach (ListData no in dynamicListResult)
            {
                string detail = HtmlRemoval.StripTagsRegexCompiled(no.Detail);
                var s = new Search(no.Lists.Name,
                                   "listedetay-" + no.Id.ToString() + "-" + UrlMapping.cevir(no.Title),
                                   no.Description, detail, no.Title);
                SearchList.Add(s);
            }
            foreach (Customer_Random cRandom in customerRandomResult)
            {
                string summary = HtmlRemoval.StripTagsRegexCompiled(cRandom.Summary);
                string text = HtmlRemoval.StripTagsRegexCompiled(cRandom.Text);
                var s = new Search(Resource.lbRandomField,
                                   "sayfa-r-" + cRandom.Id + "-" + UrlMapping.cevir(cRandom.Title),
                                   summary, text, cRandom.Title);
                SearchList.Add(s);
            }

            return SearchList;
        }

        public static string writeResults(string arananKelime)
        {
            if (arananKelime.Length < 3)
            {
                MessageBox.Show(MessageType.Success, Resource.lbSearchWordMinLength);
                return null;
            }

            ResultList = GetSearchList(arananKelime);
            string Liste = string.Empty;
            if (ResultList.Count != 0)
            {
                Liste += "<strong><u>" + "\"" + arananKelime + "\"</u> " + Resource.lbSearchTotal + " " +
                         ResultList.Count().ToString() +
                         " " + Resource.lbSearchResult + "</strong></br>";
                Liste += "<ul class='search'>";

                var resultGroupName = string.Empty;
                foreach (var group in ResultList)
                {
                    if (resultGroupName == string.Empty || resultGroupName != group.type)
                    {
                        resultGroupName = group.type;
                        Liste += "<p><b>" + resultGroupName + "</b></p>";
                    }
                    Liste += "<li class='serachresults'><a href=" + group.link + "><strong>" +
                             group.name + "</strong></br>" + group.brief + "</a></li>";
                }
                Liste += "</ul>";
            }
            else if (ResultList.Count == 0)
            {
                Liste += "<strong><u>" + "\"" + arananKelime + "\"</u> " + Resource.lbSearchNoResult + "</strong>";
            }
            return Liste;
        }
    }

    public class Search
    {
        public Search(String _type, String _link, String _brief, String _content, String _name)
        {
            type = _type;
            link = _link;
            brief = _brief;
            content = _content;
            name = _name;
        }

        public String type { get; set; }
        public String link { get; set; }
        public String brief { get; set; }
        public String content { get; set; }
        public String searchKey { get; set; }
        public String name { get; set; }
    }

    public static class HtmlRemoval
    {
        private static readonly Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        public static string StripTagsRegexCompiled(string source)
        {
            if (!String.IsNullOrEmpty(source))
            {
                string result = _htmlRegex.Replace(source, "");
                return result;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}