using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;

public class MessageBox
{
    public static string messageType;
    protected static Hashtable handlerPages = new Hashtable();

    private MessageBox()
    {
    }

    public static void Show(string Type, string Message)
    {
        messageType = Type;
        if (!(handlerPages.Contains(HttpContext.Current.Handler)))
        {
            var currentPage = (Page) HttpContext.Current.Handler;
            if (!((currentPage == null)))
            {
                var messageQueue = new Queue();
                messageQueue.Enqueue(Message);
                handlerPages.Add(HttpContext.Current.Handler, messageQueue);
                currentPage.Unload += CurrentPageUnload;
            }
        }
        else
        {
            var queue = ((Queue) (handlerPages[HttpContext.Current.Handler]));
            queue.Enqueue(Message);
        }
    }

    private static void CurrentPageUnload(object sender, EventArgs e)
    {
        var queue = ((Queue) (handlerPages[HttpContext.Current.Handler]));
        if (queue != null)
        {
            var builder = new StringBuilder();
            int iMsgCount = queue.Count;
            builder.Append("<script language='javascript'>");
            string sMsg;
            while ((iMsgCount > 0))
            {
                iMsgCount = iMsgCount - 1;
                sMsg = Convert.ToString(queue.Dequeue());
                sMsg = sMsg.Replace("\"", "'");
                if(messageType == "jAlert")
                    builder.AppendFormat("javascript:alert('{0}');",sMsg);
                else
                    builder.AppendFormat("javascript:{0}('{1}');", messageType, sMsg);
            }
            builder.Append("</script>");
            handlerPages.Remove(HttpContext.Current.Handler);
            HttpContext.Current.Response.Write(builder.ToString());
            
        }
    }
}

public class MessageType
{
    public static string jAlert = "jAlert";
    public static string Notice = "showNoticeToast";
    public static string Warning = "showWarningToast";
    public static string Success = "showSuccessToast";
    public static string Error = "showErrorToast";
}