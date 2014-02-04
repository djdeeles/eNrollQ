using System;
using System.Web;
using Resources;
using eNroll.App_Data;

namespace Enroll.Managers
{
    public class ExceptionManager
    {
        public static void ManageException(Exception exception)
        {
            try
            {
                var oEntities = new Entities();
                var oErrors = new System_Errors();
                oErrors.ErrorMessage = "Message: " + exception.Message;
                oErrors.StackTrace = "StackTrace:" + exception.StackTrace + "\nBaseException: " +
                                     exception.GetBaseException()
                                     + "\nSource: " + exception.Source;
                oErrors.Date = DateTime.Now;
                oErrors.Ip = GetUserIp();
                oErrors.Url = HttpContext.Current.Request.RawUrl;
                oEntities.AddToSystem_Errors(oErrors);
                oEntities.SaveChanges();

                if (HttpContext.Current.Request.RawUrl.ToLower().Contains("/admin/"))
                {
                    MessageBox.Show(MessageType.Error, AdminResource.lbExceptionOccured);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void ManageException(Exception exception, string message)
        {
            try
            {
                var oEntities = new Entities();
                var oErrors = new System_Errors();
                oErrors.ErrorMessage = "Message: " + exception.Message + "\n" + message;
                oErrors.StackTrace = "StackTrace:" + exception.StackTrace + "\nBaseException: " +
                                     exception.GetBaseException()
                                     + "\nSource: " + exception.Source;
                oErrors.Date = DateTime.Now;
                oErrors.Ip = GetUserIp();
                oErrors.Url = HttpContext.Current.Request.RawUrl;
                oEntities.AddToSystem_Errors(oErrors);
                oEntities.SaveChanges();

                if (HttpContext.Current.Request.RawUrl.ToLower().Contains("/admin/"))
                {
                    MessageBox.Show(MessageType.Error, AdminResource.lbExceptionOccured);
                }
            }
            catch (Exception)
            {
            }
        }

        public static string GetUserIp()
        {
            try
            {
                string ipList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipList))
                {
                    return ipList.Split(',')[0];
                }
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }
    }
}