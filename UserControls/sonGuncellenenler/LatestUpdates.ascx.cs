using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.UserControls.sonGuncellenenler
{
    public partial class LatestUpdates : UserControl
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly Entities _entities = new Entities();

        protected void Page_Load(object sender, EventArgs e)
        {
            int maxCount = 0;
            var firstOrDefault = _entities.LatestUpdates.FirstOrDefault();
            if (firstOrDefault != null) maxCount = firstOrDefault.MaxCount;

            var updateLogs = (from s in _entities.System_Logs
                              join r in _entities.LatestUpdates on s.moduleId equals r.ModuleId
                              where s.operation == 1
                              orderby s.CreatedTime descending
                              select s).Take(250);

            int updatedItemCount = 0;
            foreach (var updateLog in updateLogs)
            {
                Object moduleSub = updateLog.moduleSubId;
                var log = Logger.GetLatestContentInfo(updateLog.moduleId, updateLog.moduleContentId, moduleSub,
                                                      EnrollContext.Current.WorkingLanguage.LanguageId);
                if (log.ModulName == null || log.ModulContentUrl == null) continue;
                if (updatedItemCount > maxCount) break;
                _builder.AppendFormat("<p class='LatestUpdatesItem'><a href='{0}' {1}>{2}</a></p>", log.ModulContentUrl,
                                      log.Options, log.ModulName);
                updatedItemCount++;
            }

            ltUpdates.Text =
                "<div id='LatestUpdates'><div class='LatestUpdatesTepe'></div><div class='LatestUpdatesGovde'><marquee width='100%' direction='up' scrollamount='1' " +
                "height='100%' onmouseover=' this.stop();' onmouseout='this.start();'>" + _builder +
                "</marquee></div><div class='LatestUpdatesAlt'></div></div>";
        }
    }
}