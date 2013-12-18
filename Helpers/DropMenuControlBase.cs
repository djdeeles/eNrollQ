using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Enroll.Managers;
using eNroll.App_Data;

namespace Enroll.BaseObjects
{
    public class DropMenuControlBase : UserControl
    {
        private DataTable oAllMenuList;

        public void InitMenu(Int32 location, String connectionString) //location alt üst veya ana menüyü belirtir.
        {
            try
            {
                var oConnection = new SqlConnection(connectionString);
                var oSelectCommand =
                    new SqlCommand(
                        "select menuId,url,MasterId,name,type,location,menuIndex,subMenuShowType,menuImage,menuImageHover from System_menu where isHideToMenu='False' and languageId=@paramLanguageId and location=@paramLocation and [state]='True' order by menuIndex",
                        oConnection);

                oSelectCommand.Parameters.AddWithValue("paramLanguageId",
                                                       EnrollContext.Current.WorkingLanguage.LanguageId.ToString());
                oSelectCommand.Parameters.AddWithValue("paramLocation", location);

                var oAdaptor = new SqlDataAdapter(oSelectCommand);
                oAllMenuList = new DataTable();

                oConnection.Open();
                oAdaptor.Fill(oAllMenuList);
                oConnection.Close();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        public void GenerateMenu(Int32 masterId, MenuItemCollection items)
        {
            List<System_menu> oList;
            oList = GetChildNodeList(masterId);

            foreach (System_menu menu in oList)
            {
                var oItem = new MenuItem();

                //menu image ayarlanır
                if (!string.IsNullOrEmpty(menu.menuImage))
                {
                    string onImage = menu.menuImageHover.Replace("~/", "");
                    string offImage = menu.menuImage.Replace("~/", "");
                    string src = offImage;

                    var sw = new StringWriter();
                    var htmWriter = new HtmlTextWriter(sw);
                    var image = new HtmlImage();

                    image.Src = src;
                    image.Style.Add("border-style", "none");

                    if (onImage != "" && onImage != null)
                        image.Attributes["onmouseover"] = "this.src='" + onImage + "';";
                    if (onImage != "" && onImage != null)
                        image.Attributes["onmousedown"] = "this.src='" + onImage + "';";
                    image.Attributes["onmouseout"] = "this.src='" + offImage + "';";
                    image.Attributes["alt"] = menu.name;
                    image.RenderControl(htmWriter);
                    oItem.Text = Server.HtmlDecode(sw.ToString());
                }
                else
                {
                    oItem.Text = menu.name;
                }

                switch (menu.type)
                {
                    case "0": //yanlızca başlık
                        oItem.Selectable = false;
                        break;
                    case "1": //url
                        if (menu.url.StartsWith("http"))
                        {
                            oItem.NavigateUrl = menu.url;
                        }
                        else if (menu.url.StartsWith("www"))
                        {
                            oItem.NavigateUrl = "http://" + menu.url;
                        }
                        else
                        {
                            oItem.NavigateUrl = "~/" + menu.url;
                        }

                        break;
                    case "2": //sayfa
                        oItem.NavigateUrl = UrlMapping.LinkOlustur(menu.menuId.ToString(), menu.name, "sayfa");
                        break;
                    case "3": // yeni sekmede url
                        if (menu.url.StartsWith("http"))
                        {
                            oItem.NavigateUrl = menu.url;
                            oItem.Target = "_blank";
                        }
                        else if (menu.url.StartsWith("www"))
                        {
                            oItem.NavigateUrl = "http://" + menu.url;
                            oItem.Target = "_blank";
                        }
                        else
                        {
                            oItem.NavigateUrl = "~/" + menu.url;
                        }

                        break;
                }

                items.Add(oItem);
                //alt menüleri sayfada göster değilse menüye eklesin.
                if (menu.subMenuShowType == 0)
                    GenerateMenu(Convert.ToInt32(menu.menuId), oItem.ChildItems);
            }
        }

        public static string DetectMobileMenuType(System_menu menu)
        {
            string url = string.Empty;
            switch (menu.type)
            {
                case "0": //yanlızca başlık
                    url = UrlMapping.MobilLinkOlustur(menu.menuId.ToString(), menu.name, "sayfa");
                    break;
                case "1": //url
                    if (menu.url.StartsWith("http"))
                    {
                        url = menu.url;
                    }
                    else if (menu.url.StartsWith("www"))
                    {
                        url = "http://" + menu.url;
                    }
                    else
                    {
                        url = menu.url;
                    }

                    break;
                case "2": //sayfa
                    url = UrlMapping.MobilLinkOlustur(menu.menuId.ToString(), menu.name, "sayfa");
                    break;
                case "3": // yeni sekmede url
                    if (menu.url.StartsWith("http"))
                    {
                        url = menu.url;
                    }
                    else if (menu.url.StartsWith("www"))
                    {
                        url = "http://" + menu.url;
                    }
                    else
                    {
                        url = menu.url;
                    }

                    break;
            }
            return url;
        }

        private List<System_menu> GetChildNodeList(Int32 masterId)
        {
            var oReturnList = new List<System_menu>();

            foreach (DataRow row in oAllMenuList.Rows)
            {
                if (Convert.ToInt32(row["MasterId"]) == masterId)
                {
                    var oMenu = new System_menu();
                    oMenu.MasterId = Convert.ToInt32(row["MasterId"]);
                    oMenu.location = Convert.ToInt32(row["location"]);
                    oMenu.type = row["type"].ToString();

                    if (oMenu.type == "1")
                        oMenu.url = row["url"].ToString();

                    else if (oMenu.type == "3")
                        oMenu.url = row["url"].ToString();

                    oMenu.name = row["name"].ToString();
                    oMenu.menuId = Convert.ToInt32(row["menuId"]);
                    oMenu.subMenuShowType = Convert.ToInt32(row["subMenuShowType"]);
                    oMenu.menuImage = row["menuImage"].ToString();
                    oMenu.menuImageHover = row["menuImageHover"].ToString();

                    oReturnList.Add(oMenu);
                }
            }
            return oReturnList;
        }
    }
}