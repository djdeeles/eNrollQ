using System;
using System.Linq;
using System.Text;
using eNroll.App_Data;

namespace M_eNroll.UserControls.dynamicLists
{
    public partial class DynamicListMainPage : System.Web.UI.UserControl
    {
        private readonly Entities _entities = new Entities();
        protected void Page_Load(object sender, EventArgs e)
        {
            var list = _entities.Lists.Where(p=>p.State && p.LanguageId== EnrollContext.Current.WorkingLanguage.LanguageId)
                .ToList().Take(1).OrderByDescending(p => p.UpdatedTime).FirstOrDefault();
            if(list!=null)
            {
                var listData = _entities.ListData.Where(p => p.ListId == list.Id && p.State.Value &&
                p.LanguageId == EnrollContext.Current.WorkingLanguage.LanguageId).ToList().Take(5).OrderByDescending(p => p.Date);
                if (listData.Any())
                {
                    var builder = new StringBuilder();
                    foreach (var data in listData)
                    {
                        builder.AppendFormat("<a href='listedetay-{0}-{1}' data-role='button' data-corners='false' >{2}</a>", 
                            data.Id, UrlMapping.cevir(data.Title), data.Title);
                    }
                    ltDynamicList.Text = builder.ToString();
                }   
            }
        }
    }
}