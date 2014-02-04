using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

public partial class UserControls_DynamicForm : UserControl
{
    private readonly Entities _entities = new Entities();
    private int _formid = -1;

    public int FormId
    {
        get { return _formid; }
        set { _formid = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RadCaptcha1.Style.Clear();
        RadCaptcha1.Enabled = true;
        RadCaptcha1.Visible = true;
        RadCaptcha1.TextBoxDecoration.CssClass = "CaptchaTextBox";
        RadCaptcha1.EnableEmbeddedBaseStylesheet = false;
        RadCaptcha1.CaptchaTextBoxLabel = Resource.lbCapthcaMessage;
        RadCaptcha1.ErrorMessage = Resource.lbCapthcaErrorMessage;

        btFormSubmit.Text = Resource.btSend;
        ltForm.Text = GetFormContents(_formid);
    }

    public string GetFormContents(int formId)
    {
        List<FormContentOptions> options;
        var builder = new StringBuilder();
        var form = _entities.Forms.First(p => p.Id == formId);
        var formContents = _entities.FormContents.Where(p => p.FormId == formId).OrderBy(p => p.OrderId).ToList();
        //builder.AppendFormat("<table width='100%'>");
        foreach (var content in formContents)
        {
            builder.AppendFormat("<tr><td valign='top'>{0}</td><td width='10px'>:</td><td>", content.FieldName);
            int type = content.FieldType.Value;
            int contentId = content.Id;
            var required = content.Required ? "class='required'" : "";
            var wrongEmailType = content.Required ? "class='required email'" : "";
            switch (content.FieldType)
            {
                case 1:
                    builder.AppendFormat("<input type='text' {2} style='width:90%;' name='form_{0}_{1}' />", type,
                                         contentId, required);
                    break;
                case 2:
                    builder.AppendFormat("<textarea name='form_{0}_{1}' {2} style='width:90%;' ></textarea>", type,
                                         contentId, required);
                    break;
                case 3:
                    builder.AppendFormat("<input type='file' {2} name='form_{0}_{1}' runat='server'/>", type, contentId,
                                         required);
                    break;
                case 4:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    builder.AppendFormat("<select name='form_{0}_{1}' {2} >", type, contentId, required);
                    foreach (var option in options)
                    {
                        builder.AppendFormat("<option value='{0}'>{0}</option>", option.Value);
                    }
                    builder.AppendLine("</select>");
                    break;
                case 5:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    foreach (var option in options)
                    {
                        builder.AppendFormat("<input type='radio' name='form_{1}_{2}' {3} value='{0}'>{0}<br>",
                                             option.Value, type, contentId, required);
                    }
                    break;
                case 6:
                    options = _entities.FormContentOptions.Where(p => p.FormContentId == contentId).ToList();
                    foreach (var option in options)
                    {
                        builder.AppendFormat("<input type='checkbox' name='form_{1}_{2}' {3} value='{0}'>{0}<br>",
                                             option.Value, type, contentId, required);
                    }
                    break;
                case 7:
                    builder.AppendFormat("<input type='text' {2} name='form_{0}_{1}' style='width:90%;' />", type,
                                         contentId, wrongEmailType);
                    break;
                default:
                    break;
            }
            builder.AppendFormat("</td></tr>");
        }
        //builder.AppendFormat("</table>");

        return builder.ToString();
    }

    protected void BtFormSubmitClick(object sender, EventArgs e)
    {
        var attachmentList = new List<Attachment>();

        if (RadCaptcha1.Enabled)
        {
            RadCaptcha1.Validate();
            if (RadCaptcha1.IsValid)
            {
                try
                {
                    var stringBuilder = new StringBuilder();
                    var collection = Request.Form;
                    var form = _entities.Forms.Single(p => p.Id == _formid);
                    var formContents = _entities.FormContents.Where(p => p.FormId == _formid).ToList();
                    var site = _entities.SiteGeneralInfo.First(
                        p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId);

                    //create mail 
                    var md = new MailDefinition();
                    md.IsBodyHtml = true;
                    md.BodyFileName = "../../App_Themes/mainTheme/mailtemplates/dynamicForm.htm";
                    md.Subject = site.title + " - " + form.Name;
                    md.From = "noreply@" + HttpContext.Current.Request.Url.Host;
                    var to = form.EmailAddress;

                    stringBuilder.AppendFormat("<table>");
                    foreach (var content in formContents)
                    {
                        var elementName = string.Format("form_{0}_{1}", content.FieldType, content.Id);
                        if (content.FieldType != 3)
                        {
                            stringBuilder.AppendFormat("<tr><td>{0}</td><td>:</td><td>{1}</td></tr>", content.FieldName,
                                                       collection[elementName]);
                        }
                        else
                        {
                            HttpPostedFile file = Request.Files[elementName];

                            if (file != null && file.ContentLength > 0)
                            {
                                var extension = Path.GetExtension(file.FileName);
                                if (extension != null && (
                                                             extension.ToLower() == ".js" ||
                                                             extension.ToLower() == ".exe"
                                                             || extension.ToLower() == ".com" ||
                                                             extension.ToLower() == ".bat"
                                                             || extension.ToLower() == ".pif" ||
                                                             extension.ToLower() == ".scr"))
                                {
                                    MessageBox.Show(MessageType.jAlert, Resource.lbUploadErrorFileType);
                                    return;
                                }
                                else
                                {
                                    var attach = new Attachment(file.InputStream, file.FileName);
                                    attachmentList.Add(attach);
                                }
                            }
                        }
                    }
                    stringBuilder.AppendFormat("<tr><td>{0}</td><td>:</td><td>{1}</td></tr>",
                                               Resource.lbIpAdress, ExceptionManager.GetUserIp());
                    stringBuilder.AppendFormat("</table>");

                    var replacements = new ListDictionary();
                    replacements.Add("%%mailContent%%", stringBuilder.ToString());

                    var msg = md.CreateMailMessage(to, replacements, this);
                    msg.BodyEncoding = Encoding.Default;
                    msg.SubjectEncoding = Encoding.Default;
                    msg.Priority = MailPriority.Normal;

                    var section = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
                    var host = section.Network.Host;
                    var port = section.Network.Port;
                    var smtp = new SmtpClient(host, port);

                    foreach (var attachment in attachmentList)
                    {
                        msg.Attachments.Add(attachment);
                    }

                    smtp.Send(msg);
                    MessageBox.Show(MessageType.jAlert, Resource.lbMessageSend);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    MessageBox.Show(MessageType.jAlert, Resource.lbMessageFail);
                }
            }
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //simulate longer page load
        Thread.Sleep(2000);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
    }
}