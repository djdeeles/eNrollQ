using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using eNroll.App_Data;

public partial class M_UserControls_DynamicListDetail : UserControl
{
    Entities _entities = new Entities();
    public int ListDataId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        var ent = new Entities();
        string header = string.Empty;
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ListDataId = Convert.ToInt32(Request.QueryString["id"]);
                ListData lData = ent.ListData.First(p => p.Id == ListDataId);
                if (lData.State != true)
                    Response.Redirect("~/404.aspx");
                header = lData.Title;
                lblBaslik.Text = header;
                lblYazi.Text = lData.Detail;
                if (!String.IsNullOrEmpty(lData.Image))
                {
                    Image1.ImageUrl = lData.Image;
                }
                else
                {
                    Image1.Visible = false;
                }
            }

            GenerateDownloadAttachmentsHtml();

            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) Page.Title = site.title + " - " + header;
            MetaGenerate.SetMetaTags(site, Page);
        }
    }


    private void GenerateDownloadAttachmentsHtml()
    {
        var stringBuilder = new StringBuilder();
        var listDataAttachmentsImages = _entities.ListDataAttachments.Where(p => p.ListDataId == ListDataId &&
            (p.FileType == "jpeg" || p.FileType == "jpg" || p.FileType == "bmp" || p.FileType == "png")).ToList();
        if (listDataAttachmentsImages.Count > 0)
        {
            stringBuilder.AppendFormat("<div class='title'>{0}</div><div class='items'>",
                                       Resources.Resource.lbAttachments);
            foreach (var attachment in listDataAttachmentsImages)
            {
                var fileType = attachment.FileType.Replace(" ", "").ToLower();

                var attachmntDownloadLink = string.Format("<a href='{0}' data-ajax='false' title='{1}' download='{1}' >" +
                    "<div class='item'><img alt='{1}' src='{2}' /></div></a>",
                    attachment.Attachment.Replace("~", ""),
                    attachment.Title + "." + fileType, attachment.Thumbnail.Replace("~", ""));

                stringBuilder.Append(attachmntDownloadLink);
            }
        }

        var listDataAttachmentsOther = _entities.ListDataAttachments.Where(p => p.ListDataId == ListDataId &&
            p.FileType != "jpeg" && p.FileType != "jpg" && p.FileType != "bmp" && p.FileType != "png").ToList();
        if (listDataAttachmentsOther.Count > 0)
        {
            if (listDataAttachmentsImages.Count == 0)
            {
                stringBuilder.AppendFormat("<div class='title'>{0}</div><div class='items'>",
                                       Resources.Resource.lbAttachments);
            }

            foreach (var attachment in listDataAttachmentsOther)
            {
                var fileType = attachment.FileType.Replace(" ", "").ToLower();

                var attachmntDownloadLink = string.Format("<a href='{0}' data-ajax='false' title='{1}' download='{1}' >" +
                                                          "<div class='item'><img alt='{1}' src='{2}' /></div>" +
                                                          "</a>",
                                                          attachment.Attachment.Replace("~", ""),
                                                          attachment.Title + "." + fileType, "{0}");

                if (fileType == "zip" || fileType == "rar")
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "/App_Themes/mainTheme/images/downloadicons/zip.png");
                }
                else if (fileType == "doc" || fileType == "docx" || fileType == "xls" ||
                    fileType == "xlsx" || fileType == "ppt" || fileType == "pptx" ||
                    fileType == "txt" || fileType == "rtf")
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "/App_Themes/mainTheme/images/downloadicons/doc.png");
                }
                else if (fileType == "pdf")
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "/App_Themes/mainTheme/images/downloadicons/pdf.png");
                }
                else
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "/App_Themes/mainTheme/images/downloadicons/none.png");
                }

                stringBuilder.Append(attachmntDownloadLink);
            }
        }

        if (listDataAttachmentsImages.Count != 0 || listDataAttachmentsOther.Count != 0)
        {
            stringBuilder.Append("</div><br/>");
            ltAttachments.Text = stringBuilder.ToString();
        }
    }

}