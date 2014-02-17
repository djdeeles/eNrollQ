using System;
using System.Linq;
using System.Text;
using eNroll.App_Data;

namespace eNroll.UserControls.dynamicLists
{
    public partial class DynamicListMainPage : System.Web.UI.UserControl
    {
        private readonly Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            var list = _entities.Lists.Where(p=> p.State && p.LanguageId== EnrollContext.Current.WorkingLanguage.LanguageId)
                .ToList().Take(1).OrderByDescending(p => p.UpdatedTime).FirstOrDefault();
            if(list!=null)
            {
                var listData = _entities.ListData.Where(p => p.State.Value && p.ListId == list.Id &&
                p.LanguageId == EnrollContext.Current.WorkingLanguage.LanguageId).ToList().Take(5).OrderByDescending(p => p.Date);
                if (listData.Any())
                {
                    var builder = new StringBuilder();
                    foreach (var data in listData)
                    {
                        builder.AppendFormat("<a class='mainlistitem' href='listedetay-{0}-{1}' >" +
                                             "<img src='App_Themes/mainTheme/images/ok.png' style='vertical-align:middle; margin:0px 3px 3px 0px;' />" +
                                             "{2}</a>", data.Id, UrlMapping.cevir(data.Title), data.Title);
                    }
                    ltDynamicList.Text = builder.ToString();
                }   
            }
        }
    }
}