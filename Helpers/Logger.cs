using System;
using System.Linq;
using System.Web;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

namespace eNroll.Helpers
{
    public class LogInfo
    {
        public string ModulContentUrl = null;
        public string ModulName = null;
        public string Options = string.Empty;
    }

    public class Logger
    {
        /*
         *  ekleme 1
            silme 2
            düzenleme 3
         */

        public static void Add(int moduleId, int moduleSubId, int moduleContentId, int operation)
        {
            var entities = new Entities();
            var log = new System_Logs();
            log.userId = GetUserId();
            log.moduleId = moduleId;
            log.moduleSubId = moduleSubId;
            log.operation = operation;
            log.moduleContentId = moduleContentId;
            log.Ip = ExceptionManager.GetUserIp();
            log.CreatedTime = DateTime.Now;
            entities.AddToSystem_Logs(log);
            entities.SaveChanges();
        }

        private static int GetUserId()
        {
            var entities = new Entities();
            var user = entities.Users.FirstOrDefault(p => p.EMail == HttpContext.Current.User.Identity.Name);
            if (user != null) return user.Id;

            return -1;
        }

        public static string GetUserEmail(int id)
        {
            var entities = new Entities();
            var user = entities.Users.FirstOrDefault(p => p.Id == id);
            if (user != null) return user.EMail;

            return string.Empty;
        }

        public static string GetOperation(int operation)
        {
            string operationText = string.Empty;
            switch (operation)
            {
                case 1:
                    operationText = AdminResource.msgSaved;
                    break;
                case 2:
                    operationText = AdminResource.msgDeleted;
                    break;
                case 3:
                    operationText = AdminResource.msgUpdated;
                    break;
            }
            return operationText;
        }

        public static LogInfo GetModul(int modul, int moduleContentId, object modulSub)
        {
            var entities = new Entities();
            var logInfo = new LogInfo();
            int modulSubId = 0;
            if (modulSub != null) modulSubId = Convert.ToInt32(modulSub);
            string url = "#";
            string name = string.Empty;
            switch (modul)
            {
                case 1:
                    name = AdminResource.lbUserManagement;
                    if (modulSubId == 1)
                    {
                        name += " - " + AdminResource.lbRoles;
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbUsers;
                    }
                    logInfo.ModulName = name;
                    break;
                case 2:
                    var systemMenu = entities.System_menu.FirstOrDefault(p => p.menuId == moduleContentId);
                    if (systemMenu != null)
                        logInfo.ModulContentUrl = "/sayfa-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(systemMenu.name);
                    logInfo.ModulName = AdminResource.lbMenuContentManagement;
                    break;
                case 3:
                    var news = entities.News.FirstOrDefault(p => p.newsId == moduleContentId);
                    if (news != null)
                        logInfo.ModulContentUrl = "/haberdetay-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(news.header);
                    logInfo.ModulName = AdminResource.lbNewsManagement;
                    break;
                case 4:
                    var notices = entities.Notices.FirstOrDefault(p => p.noticeId == moduleContentId);
                    if (notices != null)
                        logInfo.ModulContentUrl = "/duyurudetay-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(notices.header);
                    logInfo.ModulName = AdminResource.lbNoticeManagement;
                    break;
                case 5:
                    name += AdminResource.lbPhotoGalleryManagement;
                    if (modulSubId == 1)
                    {
                        url = "/albumler-" + moduleContentId + "-1";
                        name += " - " + AdminResource.lbCategory;
                    }
                    else if (modulSubId == 2)
                    {
                        url = "/albumdetay-" + moduleContentId + "-1";
                        name += " - " + AdminResource.lbAlbum;
                    }
                    else if (modulSubId == 3)
                    {
                        url = "/albumdetay-" + moduleContentId + "-1";
                        name += " - " + AdminResource.lbPhoto;
                    }
                    logInfo.ModulContentUrl = url;
                    logInfo.ModulName = name;
                    break;
                case 6:
                    name += AdminResource.lbVideoGalleryManagement;
                    if (modulSubId == 1)
                    {
                        url = "/albumvideolari-" + moduleContentId + "-1";
                        name += " - " + AdminResource.lbCategory;
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbVideo;
                        url = "/albumvideolari-" + moduleContentId + "-1";
                    }
                    logInfo.ModulContentUrl = url;
                    logInfo.ModulName = name;
                    break;
                case 7:
                    logInfo.ModulName = AdminResource.lbSliderImageManagement;
                    break;
                case 8:
                    name += AdminResource.lbProductManagement;
                    if (modulSubId == 1)
                    {
                        name += " - " + AdminResource.lbCategory;
                        url = "/urunler-" + moduleContentId + "-1";
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbProduct;
                        var product = entities.Products.FirstOrDefault(p => p.ProductId == moduleContentId);
                        if (product != null)
                            url = "/urundetay-" + moduleContentId + "-" + UrlMapping.cevir(product.Name);
                    }
                    logInfo.ModulContentUrl = url;
                    logInfo.ModulName = name;
                    break;
                case 9:
                    // UserRoles sayfasındaki rolleri CheckBox a Bind ederken isimlerinin gelmesi için gerekli
                    name = AdminResource.lbBannerManagement;
                    if (modulSubId == 1)
                    {
                        name = AdminResource.lbBannerManagement;
                    }
                    else if (modulSubId == 2)
                    {
                        name = AdminResource.lbBannerFieldManagement;
                    }
                    logInfo.ModulName = name;
                    break;
                case 10:
                    var events = entities.Events.FirstOrDefault(p => p.id == moduleContentId);
                    if (events != null)
                        logInfo.ModulContentUrl = "/etkinlik-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(events.Name);
                    logInfo.ModulName = AdminResource.lbEventManagement;
                    break;
                case 11:
                    logInfo.ModulName = AdminResource.lbFileManager;
                    break;
                case 12:
                    // UserRoles sayfasındaki rolleri CheckBox a Bind ederken isimlerinin gelmesi için gerekli
                    name = AdminResource.lbFAQManagement;
                    if (modulSubId == 1)
                    {
                        name = AdminResource.lbFAQCategoryManagement;
                    }
                    else if (modulSubId == 2)
                    {
                        name = AdminResource.lbFAQManagement;
                    }
                    logInfo.ModulName = name;
                    break;
                case 13:
                    logInfo.ModulName = AdminResource.lbError;
                    break;
                case 14:
                    logInfo.ModulName = AdminResource.lbInterfaceSettings;
                    break;
                case 15:
                    logInfo.ModulName = AdminResource.lbGeneralSettings;
                    break;
                case 16:
                    name += AdminResource.lbDynamicFields;
                    if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbDynamicFieldGroup;
                    }
                    else if (modulSubId == 3)
                    {
                        name += " - " + AdminResource.lbDynamicFieldItem;
                        var dynamicItem = entities.Customer_Dynamic.FirstOrDefault(p => p.dynamicId == moduleContentId);
                        if (dynamicItem != null)
                            logInfo.ModulContentUrl = "/dinamik-" + moduleContentId.ToString() + "-" +
                                                      UrlMapping.cevir(dynamicItem.name);
                    }
                    else if (modulSubId == 4)
                    {
                        name += " - " + AdminResource.lbDynamicFieldReferance;
                        var dynamicMenu = entities.System_menu.FirstOrDefault(p => p.menuId == moduleContentId);
                        if (dynamicMenu != null)
                            logInfo.ModulContentUrl = "/sayfa-" + moduleContentId.ToString() + "-" +
                                                      UrlMapping.cevir(dynamicMenu.name);
                    }
                    logInfo.ModulName = name;
                    break;
                case 17:
                    name += AdminResource.lbSurveyManagement;
                    if (modulSubId == 1)
                    {
                        name += " - " + AdminResource.lbSurveyQuestion;
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbSurveyAnsver;
                    }
                    logInfo.ModulName = name;
                    break;
                case 18:
                    logInfo.ModulName = AdminResource.lbTemplateManagement;
                    break;
                case 19:
                    logInfo.ModulName = AdminResource.lbNewsGroupMember;
                    break;
                case 20:
                    logInfo.ModulName = AdminResource.lbRssManagement;
                    break;
                case 21:
                    logInfo.ModulName = AdminResource.lbLogs;
                    break;
                case 22:
                    logInfo.ModulName = AdminResource.lbLatestUpdatesManagement;
                    break;
                case 23:
                    name += AdminResource.lbBackupRestore;
                    if (modulSubId == 1)
                    {
                        name += " - " + AdminResource.lbRestore;
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbDbBackUp;
                    }
                    else if (modulSubId == 3)
                    {
                        name += " - " + AdminResource.lbFileBackUp;
                    }
                    logInfo.ModulName = name;
                    break;
                case 24:
                    name += AdminResource.lbFormManagement;
                    if (modulSubId == 1)
                    {
                        name += " - " + AdminResource.lbForm;
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbFormContent;
                    }
                    else if (modulSubId == 3)
                    {
                        name += " - " + AdminResource.lbFormContentOption;
                    }
                    logInfo.ModulName = name;
                    break;
                case 25:
                    name += AdminResource.lbListsManagement;
                    if (modulSubId == 1)
                    {
                        name += " - " + AdminResource.lbList;
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbListContent;
                    }
                    logInfo.ModulName = name;
                    break;
                case 26:
                    logInfo.ModulName = AdminResource.lbIpFilterManagement;
                    break;
                case 27:
                    logInfo.ModulName = AdminResource.lbMemberSearch;
                    break;
                case 28:
                    logInfo.ModulName = AdminResource.lbMailTemplateManagement;
                    break;
                case 29:
                    logInfo.ModulName = AdminResource.lbNewMember;
                    break;
                case 30:
                    logInfo.ModulName = AdminResource.lbMemberActivation;
                    break;
                case 31:
                    logInfo.ModulName = AdminResource.lbScheduledJobs;
                    break;
                case 32:
                    logInfo.ModulName = AdminResource.lbDefinitions;
                    break;
                case 33:
                    name += AdminResource.lbFinanceManagement;
                    if (modulSubId == 1)
                    {
                        name += " - " + AdminResource.lbDebiting;
                    }
                    else if (modulSubId == 2)
                    {
                        name += " - " + AdminResource.lbPayment;
                    }
                    else if (modulSubId == 3)
                    {
                        name += " - " + AdminResource.lbInvoicing;
                    }
                    else if (modulSubId == 4)
                    {
                        name += " - " + AdminResource.lbCancelInvoice;
                    }
                    else if (modulSubId == 5)
                    {
                        name += " - " + AdminResource.lbDeleteDebiting;
                    }
                    else if (modulSubId == 6)
                    {
                        name += " - " + AdminResource.lbDeletePayment;
                    }
                    logInfo.ModulName = name;
                    break;
                case 34:
                    logInfo.ModulName = AdminResource.lbSmsManagement;
                    break;
                case 35:
                    logInfo.ModulName = AdminResource.lbEMailManagement;
                    break;
                case 36:
                    logInfo.ModulName = AdminResource.lbSupport;
                    break;
                case 37:
                    logInfo.ModulName = AdminResource.lbRandomFields;
                    break;
                case 38:
                    logInfo.ModulName = AdminResource.lbStaticFieldManagement;
                    break;
                default:
                    break;
            }
            return logInfo;
        }

        public static LogInfo GetLatestContentInfo(int modul, int moduleContentId, object modulSub, int languageId)
        {
            var entities = new Entities();
            var logInfo = new LogInfo();
            int modulSubId = 0;
            if (modulSub != null) modulSubId = Convert.ToInt32(modulSub);
            switch (modul)
            {
                case 2:
                    var systemMenu =
                        entities.System_menu.FirstOrDefault(
                            p => p.menuId == moduleContentId && p.state == true && p.languageId == languageId);
                    if (systemMenu != null)
                    {
                        logInfo.ModulContentUrl = "/sayfa-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(systemMenu.name);
                        logInfo.ModulName = systemMenu.name + " (" + Resource.lbPage + ")";
                    }
                    break;
                case 3:
                    var news =
                        entities.News.FirstOrDefault(
                            p => p.newsId == moduleContentId && p.state == true && p.languageId == languageId);
                    if (news != null)
                    {
                        logInfo.ModulContentUrl = "/haberdetay-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(news.header);
                        logInfo.ModulName = news.header + " (" + Resource.lbNews + ")";
                    }
                    break;
                case 4:
                    var notices =
                        entities.Notices.FirstOrDefault(
                            p => p.noticeId == moduleContentId && p.state == true && p.languageId == languageId);
                    if (notices != null)
                    {
                        logInfo.ModulContentUrl = "/duyurudetay-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(notices.header);
                        logInfo.ModulName = notices.header + "( " + Resource.lbNotice + ")";
                    }
                    break;
                case 5:
                    if (modulSubId == 1)
                    {
                        var photoGalleryManagement = entities.Def_photoAlbumCategory.FirstOrDefault(
                            p =>
                            p.photoAlbumCategoryId == moduleContentId && p.state == true && p.languageId == languageId);
                        if (photoGalleryManagement != null)
                        {
                            logInfo.ModulContentUrl = "/albumler-" + moduleContentId + "-1";
                            logInfo.ModulName = photoGalleryManagement.categoryName + " (" +
                                                Resource.lbPhotoAlbumCategory + ")";
                        }
                    }
                    else if (modulSubId == 2)
                    {
                        var photoAlbumManagement = entities.Def_photoAlbum.FirstOrDefault(
                            p => p.photoAlbumId == moduleContentId && p.state == true && p.languageId == languageId);
                        if (photoAlbumManagement != null)
                        {
                            logInfo.ModulContentUrl = "/albumler-" + moduleContentId + "-1";
                            logInfo.ModulName = photoAlbumManagement.albumName + " (" + Resource.lbPhotoAlbum + ")";
                        }
                    }
                    else if (modulSubId == 3)
                    {
                        var photo = entities.PhotoAlbum.FirstOrDefault(
                            p => p.photoId == moduleContentId && p.State == true);
                        if (photo != null)
                        {
                            logInfo.ModulContentUrl = "/" + photo.photoPath.Replace("~/", "");
                            logInfo.Options = " rel='prettyPhoto' ";
                            logInfo.ModulName = photo.photoName + " (" + Resource.lbPhoto + ")";
                        }
                    }
                    break;
                case 6:
                    if (modulSubId == 1)
                    {
                        var videoCategories = entities.VideoCategories.FirstOrDefault(
                            p => p.id == moduleContentId && p.State == true && p.languageId == languageId);
                        if (videoCategories != null)
                        {
                            logInfo.ModulContentUrl = "/albumvideolari-" + moduleContentId + "-1";
                            logInfo.ModulName = videoCategories.name + " (" + Resource.lbVideoCategory + ")";
                        }
                    }
                    else if (modulSubId == 2)
                    {
                        var video = entities.Videos.FirstOrDefault(
                            p => p.id == moduleContentId && p.State == true && p.languageId == languageId);
                        if (video != null)
                        {
                            logInfo.ModulName = video.Name + " (" + Resource.lbVideo + ")";
                            logInfo.ModulContentUrl = video.videoURL;
                            logInfo.Options = " rel='prettyPhoto' ";
                        }
                    }
                    break;
                case 8:
                    if (modulSubId == 1)
                    {
                        var productCat = entities.Def_ProductCategories.FirstOrDefault(
                            p => p.ProductCategoryId == moduleContentId && p.State == true && p.languageId == languageId);
                        if (productCat != null)
                        {
                            logInfo.ModulName = productCat.Name + " (" + Resource.lbProductCategory + ")";
                            logInfo.ModulContentUrl = "/urunler-" + moduleContentId + "-1";
                        }
                    }
                    else if (modulSubId == 2)
                    {
                        var product =
                            entities.Products.FirstOrDefault(
                                p => p.ProductId == moduleContentId && p.State == true && p.languageId == languageId);
                        if (product != null)
                        {
                            logInfo.ModulName = product.Name + " (" + Resource.lbProduct + ")";
                            logInfo.ModulContentUrl = "/urundetay-" + moduleContentId + "-" +
                                                      UrlMapping.cevir(product.Name);
                        }
                    }
                    break;
                case 10:
                    var events =
                        entities.Events.FirstOrDefault(
                            p => p.id == moduleContentId && p.State == true && p.languageId == languageId);
                    if (events != null)
                    {
                        logInfo.ModulContentUrl = "/etkinlik-" + moduleContentId.ToString() + "-" +
                                                  UrlMapping.cevir(events.Name);
                        logInfo.ModulName = events.Name + " (" + Resource.lbEvent + ")";
                    }
                    break;
            }
            return logInfo;
        }
    }
}