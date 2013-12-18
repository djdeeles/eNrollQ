using System;
using System.Linq;
using System.Web;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin
{
    /// <summary>
    ///   Summary description for FormHandler
    /// </summary>
    public class FormHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            var s = context.Request.Form["formId"];

            var formId = Convert.ToInt32(Crypto.Decrypt(context.Request.Form["formId"]));
            var contentId = Convert.ToInt32(Crypto.Decrypt(context.Request.Form["contentId"]));
            var value = context.Request.Form["value"];
            RemoveOption(formId, contentId, value);

            context.Response.Write("success");
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }

        #endregion

        public void Init(HttpApplication app)
        {
            // register for pipeline events
            app.BeginRequest +=
                OnBeginRequest;
            app.EndRequest +=
                OnEndRequest;
        }

        public void Dispose()
        {
        }

        public void OnBeginRequest(object o,
                                   EventArgs args)
        {
            // record time when request started
        }

        public void OnEndRequest(object o,
                                 EventArgs args)
        {
            // get access to app and context
            var app =
                (HttpApplication) o;
            HttpContext ctx = app.Context;

            // add custom header to HTTP response
            ctx.Response.AppendHeader("", "");
        }

        public void RemoveOption(int formId, int contentId, string value)
        {
            var entities = new Entities();
            var option = entities.FormContentOptions.First(p => p.Value == value && p.FormContentId == contentId);
            entities.DeleteObject(option);
            entities.SaveChanges();

            Logger.Add(24, 3, option.Id, 2);
        }
    }
}