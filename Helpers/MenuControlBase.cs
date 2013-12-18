using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

namespace Enroll.BaseObjects
{
    public class MenuControlBase : UserControl
    {
        private static readonly Entities _entities = new Entities();
        public String[] ContentList;
        public string CurrentMenuContent;
        public System_menu OMenuObject;

        public void LoadWebControl(PlaceHolder PlaceHolder1)
        {
            for (int i = 0; i < ContentList.Length; i++)
            {
                string[] menuData = ContentList[i].Split('-');
                switch (menuData[0])
                {
                    case "MenuContent":
                        Control tempControl = LoadControl(CurrentMenuContent + ".ascx");
                        PlaceHolder1.Controls.Add(tempControl);
                        break;
                    case "SiteMapPath":
                        Control tempSiteMapPath = LoadControl("~/UserControls/base/Sitemappath.ascx");
                        PlaceHolder1.Controls.Add(tempSiteMapPath);
                        break;
                    case "ShareSocial":
                        Control tempShareSocial = LoadControl("~/UserControls/base/ShareSocial.ascx");
                        PlaceHolder1.Controls.Add(tempShareSocial);
                        break;
                    case "NoticeScroller":
                        Control tempNoticeScroller = LoadControl("~/UserControls/duyuru/NoticeScroller.ascx");
                        PlaceHolder1.Controls.Add(tempNoticeScroller);
                        break;
                    case "NoticeList":
                        Control tempNoticeList = LoadControl("~/UserControls/duyuru/NoticeList.ascx");
                        PlaceHolder1.Controls.Add(tempNoticeList);
                        break;
                    case "MansetScroller":
                        Control tempMansetScroller = LoadControl("~/UserControls/haber/MansetScroller.ascx");
                        PlaceHolder1.Controls.Add(tempMansetScroller);
                        break;
                    case "NewsList":
                        var controlNewsList =
                            (UserControls_haber_NewsList) LoadControl("~/UserControls/haber/NewsList.ascx");
                        if (menuData.Length > 1)
                        {
                            controlNewsList.Manset = true;
                        }
                        PlaceHolder1.Controls.Add(controlNewsList);
                        break;
                    case "EmailAdd":
                        Control tempEmailAdd = LoadControl("~/UserControls/ebultenAbonelik/EmailAdd.ascx");
                        PlaceHolder1.Controls.Add(tempEmailAdd);
                        break;
                    case "PhotoAlbum":
                        Control tempPhotoAlbum = LoadControl("~/UserControls/galeri/PhotoGalleryMain.ascx");
                        PlaceHolder1.Controls.Add(tempPhotoAlbum);
                        break;
                    case "pAlbum":
                        var pAlbum =
                            (uye_userControls_photoAlbum)LoadControl("~/UserControls/galeri/AlbumPhotos.ascx");
                        if (menuData.Length > 1)
                        {
                            pAlbum.AlbumId = Convert.ToInt32(menuData[1]);
                        }
                        PlaceHolder1.Controls.Add(pAlbum);
                        break;
                    case "pCategory":
                        var pCategory =
                            (UserControls_Albums)LoadControl("~/UserControls/galeri/Albums.ascx");
                        if (menuData.Length > 1)
                        {
                            pCategory.CategoryId = Convert.ToInt32(menuData[1]);
                        }
                        PlaceHolder1.Controls.Add(pCategory);
                        break;
                    case "VideoGallery":
                        Control tempVideoGallery = LoadControl("~/UserControls/galeri/VideoGalleryMain.ascx");
                        PlaceHolder1.Controls.Add(tempVideoGallery);
                        break;
                    case "vCategory":
                        var vCategory =
                            (userControls_AlbumVideos)LoadControl("~/UserControls/galeri/AlbumVideos.ascx");
                        if (menuData.Length > 1)
                        {
                            vCategory.CategoryId = Convert.ToInt32(menuData[1]);
                        }
                        PlaceHolder1.Controls.Add(vCategory);
                        break;
                    case "Products":
                        Control tempProducts = LoadControl("~/UserControls/urun/ProductCategoryMain.ascx");
                        PlaceHolder1.Controls.Add(tempProducts);
                        break;
                    case "ProductScroller":
                        Control tempProductScroller = LoadControl("~/UserControls/urun/ProductScroller.ascx");
                        PlaceHolder1.Controls.Add(tempProductScroller);
                        break;
                    case "SSS":
                        Control tempSSS = LoadControl("~/UserControls/sss/SSS.ascx");
                        PlaceHolder1.Controls.Add(tempSSS);
                        break;
                    case "Calender":
                        Control tempCalender = LoadControl("~/UserControls/etkinlik/Calendar.ascx");
                        PlaceHolder1.Controls.Add(tempCalender);
                        break;
                    case "EventList":
                        Control tempEventList = LoadControl("~/UserControls/etkinlik/EventList.ascx");
                        PlaceHolder1.Controls.Add(tempEventList);
                        break;
                    case "Survey":
                        Control tempSurvey = LoadControl("~/UserControls/anket/Survey.ascx");
                        PlaceHolder1.Controls.Add(tempSurvey);
                        break;
                    case "RefLogos":
                        Control tempRefLogos = LoadControl("~/UserControls/referans/RefSlider.ascx");
                        PlaceHolder1.Controls.Add(tempRefLogos);
                        break;
                    case "Doviz":
                        Control tempDoviz = LoadControl("~/UserControls/doviz/doviz.ascx");
                        PlaceHolder1.Controls.Add(tempDoviz);
                        break;
                    case "MainPageSpecial1":
                        Control tempMainPageSpecial1 = LoadControl("~/UserControls/dinamikAlanlar/MainPageSpecial1.ascx");
                        PlaceHolder1.Controls.Add(tempMainPageSpecial1);
                        break;
                    case "MainPageSpecial2":
                        Control tempMainPageSpecial2 = LoadControl("~/UserControls/dinamikAlanlar/MainPageSpecial2.ascx");
                        PlaceHolder1.Controls.Add(tempMainPageSpecial2);
                        break;
                    case "MainPageSpecial3":
                        Control tempMainPageSpecial3 = LoadControl("~/UserControls/dinamikAlanlar/MainPageSpecial3.ascx");
                        PlaceHolder1.Controls.Add(tempMainPageSpecial3);
                        break;
		    case "BannerField1":
                        Control tempBannerField1 = LoadControl("~/UserControls/banner/Banner.ascx");
                        PlaceHolder1.Controls.Add(tempBannerField1);
                        break;
                    case "BannerField2":
                        Control tempBannerField2 = LoadControl("~/UserControls/banner/Banner2.ascx");
                        PlaceHolder1.Controls.Add(tempBannerField2);
                        break;
                    case "BannerField3":
                        Control tempBannerField3 = LoadControl("~/UserControls/banner/Banner3.ascx");
                        PlaceHolder1.Controls.Add(tempBannerField3);
                        break;
                    case "RandomField":
                        Control cRandomField = LoadControl("~/UserControls/dinamikAlanlar/RandomField.ascx");
                        PlaceHolder1.Controls.Add(cRandomField);
                        break;
                    case "Rss":
                        var rss = (UserControls_Rss) LoadControl("~/UserControls/rss/Rss.ascx");
                        if (menuData.Length > 1)
                        {
                            rss.RssId = Convert.ToInt32(menuData[1]);
                        }
                        PlaceHolder1.Controls.Add(rss);
                        break;
                    case "LatestUpdates":
                        Control tempLatestUpdates = LoadControl("~/UserControls/sonGuncellenenler/LatestUpdates.ascx");
                        PlaceHolder1.Controls.Add(tempLatestUpdates);
                        break;
                    case "DynamicForm":
                        var dynamicForm = (UserControls_DynamicForm) LoadControl("~/UserControls/base/DynamicForm.ascx");
                        if (menuData.Length > 1)
                        {
                            dynamicForm.FormId = Convert.ToInt32(menuData[1]);
                        }
                        PlaceHolder1.Controls.Add(dynamicForm);
                        break;
                    case "List":
                        var dynamicList =
                            (UserControls_DynamicList) LoadControl("~/UserControls/dynamicLists/DynamicList.ascx");
                        if (menuData.Length > 1)
                        {
                            dynamicList.ListId = Convert.ToInt32(menuData[1]);
                        }
                        PlaceHolder1.Controls.Add(dynamicList);
                        break;
                    default:
                        var dynaLabe = new Label();
                        dynaLabe.Text = ContentList[i];
                        PlaceHolder1.Controls.Add(dynaLabe);
                        break;
                }
            }
        }

        public void LoadMobilControl(PlaceHolder PlaceHolder1)
        {
            for (int i = 0; i < ContentList.Length; i++)
            {
                string[] menuPlace = ContentList[i].Split('-');
                switch (menuPlace[0])
                {
                    case "MenuContent":
                        Control tempControl = LoadControl(CurrentMenuContent + ".ascx");
                        PlaceHolder1.Controls.Add(tempControl);
                        break;
                    case "SiteMapPath":
                        Control tempSiteMapPath = LoadControl("~/m/UserControls/base/Sitemappath.ascx");
                        PlaceHolder1.Controls.Add(tempSiteMapPath);
                        break;
                    case "ShareSocial":
                        Control tempShareSocial = LoadControl("~/m/UserControls/base/ShareSocial.ascx");
                        PlaceHolder1.Controls.Add(tempShareSocial);
                        break;
                    case "NoticeScroller":
			            Control tempNoticeList = LoadControl("~/m/UserControls/duyuru/NoticeList.ascx");
                        PlaceHolder1.Controls.Add(tempNoticeList);
                        break;
                    case "NoticeList":
                        Control tempNoticeList2 = LoadControl("~/m/UserControls/duyuru/NoticeList.ascx");
                        PlaceHolder1.Controls.Add(tempNoticeList2);
                        break;
                    case "MansetScroller":
                        Control tempMansetNewsList = LoadControl("~/m/UserControls/haber/NewsList.ascx");
                        PlaceHolder1.Controls.Add(tempMansetNewsList);
                        break;
                    case "MansetNewsList":
                        Control tempMansetNewsList2 = LoadControl("~/m/UserControls/haber/NewsList.ascx");
                        PlaceHolder1.Controls.Add(tempMansetNewsList2);
                        break;
                    case "NewsList":
                        Control tempNewsList = LoadControl("~/m/UserControls/haber/NewsList.ascx");
                        PlaceHolder1.Controls.Add(tempNewsList);
                        break;
                    case "EmailAdd":
                        break;
                    case "PhotoAlbum":
                        Control tempPhotoAlbum = LoadControl("~/m/UserControls/galeri/PhotoGalleryMain.ascx");
                        PlaceHolder1.Controls.Add(tempPhotoAlbum);
                        break;
                    case "pCategory":
                        var pCategory =
                            (M_Albums)LoadControl("~/m/UserControls/galeri/Albums.ascx");
                        if (menuPlace.Length > 1)
                        {
                            pCategory.CategoryId = Convert.ToInt32(menuPlace[1]);
                        }
                        PlaceHolder1.Controls.Add(pCategory);
                        break;
                    case "pAlbum":
                        var pAlbum =
                            (M_AlbumPhotos)LoadControl("~/m/UserControls/galeri/AlbumPhotos.ascx");
                        if (menuPlace.Length > 1)
                        {
                            pAlbum.AlbumId = Convert.ToInt32(menuPlace[1]);
                        }
                        PlaceHolder1.Controls.Add(pAlbum);
                        break;
                    case "VideoGallery":
                        Control tempVideoGallery = LoadControl("~/m/UserControls/galeri/VideoGalleryMain.ascx");
                        PlaceHolder1.Controls.Add(tempVideoGallery);
                        break;
                    case "vCategory":
                        var vCategory =
                            (M_AlbumVideos)LoadControl("~/m/UserControls/galeri/AlbumVideos.ascx");
                        if (menuPlace.Length > 1)
                        {
                            vCategory.CategoryId = Convert.ToInt32(menuPlace[1]);
                        }
                        PlaceHolder1.Controls.Add(vCategory);
                        break;
                    case "Products":
                        Control tempProducts = LoadControl("~/m/UserControls/urun/ProductCategoryMain.ascx");
                        PlaceHolder1.Controls.Add(tempProducts);
                        break;
                    case "ProductScroller":
                        break;
                    case "SSS":
                        Control tempSSS = LoadControl("~/m/UserControls/sss/SSS.ascx");
                        PlaceHolder1.Controls.Add(tempSSS);
                        break;
                    case "Calender":
                        break;
                    case "EventList":
                        Control tempEventList = LoadControl("~/m/UserControls/etkinlik/EventList.ascx");
                        PlaceHolder1.Controls.Add(tempEventList);
                        break;
                    case "Survey":
                        break;
                    case "RefLogos":
                        break;
                    case "Doviz":
                        break;
                    case "MainPageSpecial1":
                        break;
                    case "MainPageSpecial2":
                        break;
                    case "MainPageSpecial3":
                        break;
		    case "BannerField1":
                        break;
                    case "BannerField2":
                        break;
                    case "BannerField3":
                        break;
                    case "RandomField":
                        Control cRandomField = LoadControl("~/m/UserControls/dinamikAlanlar/RandomField.ascx");
                        PlaceHolder1.Controls.Add(cRandomField);
                        break;
                    case "Rss":
                        var rss = (M_UserControls_Rss) LoadControl("~/m/UserControls/rss/Rss.ascx");
                        if (menuPlace.Length > 1)
                        {
                            rss.RssId = Convert.ToInt32(menuPlace[1]);
                        }
                        PlaceHolder1.Controls.Add(rss);
                        break;
                    case "DynamicForm":
                        var dynamicForm = (UserControls_DynamicForm) LoadControl("~/UserControls/base/DynamicForm.ascx");
                        if (menuPlace.Length > 1)
                        {
                            dynamicForm.FormId = Convert.ToInt32(menuPlace[1]);
                        }
                        PlaceHolder1.Controls.Add(dynamicForm);
                        break;
                    case "List":
                        var dynamicList =
                            (M_UserControls_DynamicList) LoadControl("~/m/UserControls/dynamicLists/DynamicList.ascx");
                        if (menuPlace.Length > 1)
                        {
                            dynamicList.ListId = Convert.ToInt32(menuPlace[1]);
                        }
                        PlaceHolder1.Controls.Add(dynamicList);
                        break;
                    case "LatestUpdates":
                        break;
                    default:
                        var dynaLabe = new Label();
                        dynaLabe.Text = ContentList[i];
                        PlaceHolder1.Controls.Add(dynaLabe);
                        break;
                }
            }
        }

        #region language control

        public static void GetProductCategoryLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var productCategory = _entities.Def_ProductCategories.FirstOrDefault(x => x.ProductCategoryId == id);
            if (productCategory != null)
            {
                var lang = productCategory.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetProductLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var product = _entities.Products.FirstOrDefault(x => x.ProductId == id);
            if (product != null)
            {
                var lang = product.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetGalleryLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var photoAlbumCategory = _entities.Def_photoAlbumCategory.FirstOrDefault(x => x.photoAlbumCategoryId == id);
            if (photoAlbumCategory != null)
            {
                var lang = photoAlbumCategory.languageId;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetAlbumPhotosLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var album = _entities.Def_photoAlbum.FirstOrDefault(x => x.photoAlbumId == id);
            if (album != null)
            {
                var lang = album.languageId;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetAlbumVideosLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var videoCategories = _entities.Videos.FirstOrDefault(x => x.categoryId == id);
            if (videoCategories != null)
            {
                var lang = videoCategories.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetEventLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var events = _entities.Events.FirstOrDefault(x => x.id == id);
            if (events != null)
            {
                var lang = events.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetDynamicLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var menu = _entities.Customer_Dynamic_Group.FirstOrDefault(x => x.groupId == id);
            if (menu != null)
            {
                var lang = menu.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetPageLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var menu = _entities.System_menu.FirstOrDefault(x => x.menuId == id);
            if (menu != null)
            {
                var lang = menu.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetNewsLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var news = _entities.News.FirstOrDefault(x => x.newsId == id);
            if (news != null)
            {
                var lang = news.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetNoticeLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var notice = _entities.Notices.FirstOrDefault(x => x.noticeId == id);
            if (notice != null)
            {
                var lang = notice.languageId.Value;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetDynamicListDataLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var listData = _entities.ListData.FirstOrDefault(x => x.Id == id);
            if (listData != null)
            {
                var lang = listData.LanguageId;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        public static void GetRssLanguage(string param2)
        {
            int id = Convert.ToInt32(param2);
            var rss = _entities.Rss.FirstOrDefault(x => x.Id == id);
            if (rss != null)
            {
                var lang = (int) rss.Language;
                if (EnrollContext.Current.WorkingLanguage.LanguageId != lang)
                {
                    EnrollContext.Current.WorkingLanguage.LanguageId = lang;
                    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.RawUrl);
                }
            }
        }

        #endregion
    }
}