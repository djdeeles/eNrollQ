using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using Telerik.Web.Zip;
using eNroll.App_Data;

public partial class UserControls_DynamicListDetail : UserControl
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
                hfListDataId.Value = ListDataId.ToString();
                ListData lData = ent.ListData.First(p => p.Id == ListDataId);
                hfListId.Value = lData.ListId.ToString();
                if (lData.State != true)
                    Response.Redirect("~/404.aspx");
                header = lData.Title;
                lblBaslik.Text = header;
                lbDate.Text = lData.Date.ToShortDateString();
                lblYazi.Text = lData.Detail;
                if (!String.IsNullOrEmpty(lData.Image))
                {
                    Image1.ImageUrl = lData.Image;
                }
                else
                {
                    Image1.Visible = false;
                }
                 
                GenerateDownloadAttachmentsHtml();

            }

            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            if (site != null) Page.Title = site.title + " - " + header;
            MetaGenerate.SetMetaTags(site, Page);
        }

        if (Session["listItemPageIndex"] != null && Session["listItemPageIndex"].ToString() != "")
        {
            hfPageIndex.Value = Session["listItemPageIndex"].ToString();
        }
        else hfPageIndex.Value = "1";
    }

    private void GenerateDownloadAttachmentsHtml()
    {
        var stringBuilder = new StringBuilder();
        var listDataAttachmentsImages = _entities.ListDataAttachments.Where(p => p.ListDataId == ListDataId &&
            (p.FileType == "jpeg" || p.FileType == "jpg" || p.FileType == "bmp" || p.FileType == "png")).ToList();
        if (listDataAttachmentsImages.Count > 0)
        {
            stringBuilder.AppendFormat("<span class='title'>{0}</span><span class='items'>",
                                       Resources.Resource.lbAttachments);
            lbDownloadAll.Text = Resources.Resource.lbDownloadAll;
            foreach (var attachment in listDataAttachmentsImages)
            {
                var fileType = attachment.FileType.Replace(" ", "").ToLower();

                var attachmntDownloadLink = string.Format("<a href='{0}' title='{1}' download='{1}' >" +
                    "<span class='item'><img alt='{1}' src='{2}' /></span></a>",
                    attachment.Attachment.Replace("~/", ""),
                    attachment.Title + "." + fileType, attachment.Thumbnail.Replace("~/", ""));

                stringBuilder.Append(attachmntDownloadLink);
            }
        }

        var listDataAttachmentsOther = _entities.ListDataAttachments.Where(p => p.ListDataId == ListDataId &&
            p.FileType != "jpeg" && p.FileType != "jpg" && p.FileType != "bmp" && p.FileType != "png").ToList();
        if (listDataAttachmentsOther.Count > 0)
        {
            if (listDataAttachmentsImages.Count == 0)
            {
                stringBuilder.AppendFormat("<span class='title'>{0}</span><span class='items'>",
                                       Resources.Resource.lbAttachments);
            }

            lbDownloadAll.Text = Resources.Resource.lbDownloadAll;
            foreach (var attachment in listDataAttachmentsOther)
            {
                var fileType = attachment.FileType.Replace(" ", "").ToLower();

                var attachmntDownloadLink = string.Format("<a href='{0}' title='{1}' download='{1}' >" +
                                                          "<span class='item'><img alt='{1}' src='{2}' /></span>" +
                                                          "</a>",
                                                          attachment.Attachment.Replace("~/", ""),
                                                          attachment.Title + "." + fileType, "{0}");

                if (fileType == "zip" || fileType == "rar")
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "App_Themes/mainTheme/images/downloadicons/zip.png");
                }
                else if (fileType == "doc" || fileType == "docx" || fileType == "xls" ||
                    fileType == "xlsx" || fileType == "ppt" || fileType == "pptx" ||
                    fileType == "txt" || fileType == "rtf")
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "App_Themes/mainTheme/images/downloadicons/doc.png");
                }
                else if (fileType == "pdf")
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "App_Themes/mainTheme/images/downloadicons/pdf.png");
                }
                else
                {
                    attachmntDownloadLink = string.Format(attachmntDownloadLink, "App_Themes/mainTheme/images/downloadicons/none.png");
                }

                stringBuilder.Append(attachmntDownloadLink);
            }
        }

        if (listDataAttachmentsImages.Count != 0 || listDataAttachmentsOther.Count != 0)
        {
            stringBuilder.Append("</span><br/>");
            ltAttachments.Text = stringBuilder.ToString();
        }
        else
        {
            lbDownloadAll.Visible = false;
        }
    }

    protected void btnDownloadAll_OnClick(object sender, EventArgs e)
    {
        if (hfListDataId.Value != string.Empty)
        {
            var listDataId = Convert.ToInt32(hfListDataId.Value);
            var listData = _entities.ListData.FirstOrDefault(p => p.Id == listDataId);
            if (listData != null)
            {
                var title = UrlMapping.cevir(listData.Title);
                var zipLibrary = new ZipLibrary(title);
                zipLibrary.Files = new List<ZipFile>();

                var listDataAttachments = _entities.ListDataAttachments.Where(p => p.ListDataId == listDataId).ToList();
                foreach (var attachment in listDataAttachments)
                {
                    var fileType = attachment.FileType.Replace(" ", "").ToLower();
                    var file = new ZipFile();
                    file.Path = Server.MapPath(attachment.Attachment.Replace("~/", ""));
                    file.Name = attachment.Title + "." + fileType;
                    zipLibrary.Files.Add(file);
                }

                zipLibrary.ZipFile();
            }
        }
    }
}