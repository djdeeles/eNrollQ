using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Enroll.BaseObjects;
using Enroll.Managers;
using Enroll.WebParts;
using eNroll.App_Data;

public partial class MenuSubMenuLeftTree : MenuControlBase
{
    private Int32 intMasterMenuId;
    private List<System_menu> oAllMenuList;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void LoadTreeview(Int32 masterMenuId)
    {
        try
        {
            intMasterMenuId = masterMenuId;
            var oentity = new Entities();
            oAllMenuList =
                oentity.System_menu.Where(
                    p =>
                    p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId && p.state == true &&
                    p.isHideToMenu == false).OrderBy(p => p.menuIndex).ToList();

            //oAllMenuList = oentity.System_menu.Where("it.System_language.languageId=" + EnrollContext.Current.WorkingLanguage.languageId.ToString() + " and it.state=true and it.isHideToMenu=false order by it.menuIndex").ToList();

            TreeView1.Nodes.Clear();
            GenerateTreenode(masterMenuId, TreeView1.Nodes);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void GenerateTreenode(Int32 masterId, TreeNodeCollection nodes)
    {
        List<System_menu> oList;
        oList = GetChildNodeList(masterId, 1);

        foreach (System_menu menu in oList)
        {
            var oNode = new ImageTreeViewNode();
            oNode.Text = menu.name;

            switch (menu.type)
            {
                case "0": //yanlızca başlık
                    oNode.SelectAction = TreeNodeSelectAction.None;
                    break;
                case "1": //url
                    if (menu.url.StartsWith("http"))
                    {
                        oNode.NavigateUrl = menu.url;
                    }
                    else if (menu.url.StartsWith("www"))
                    {
                        oNode.NavigateUrl = "http://" + menu.url;
                    }
                    else
                    {
                        oNode.NavigateUrl = "~/" + menu.url;
                    }
                    break;
                case "2": //sayfa
                    //oNode.NavigateUrl = "../../sayfa/" + menu.menuId + "/" + EnrollSearch.cevir(menu.name);
                    oNode.NavigateUrl = UrlMapping.LinkOlustur(menu.menuId.ToString(), menu.name, "sayfa");
                    break;
                case "3": //yeni sekmede url
                    if (menu.url.StartsWith("http"))
                    {
                        oNode.NavigateUrl = menu.url;
                        oNode.Target = "_blank";
                    }
                    else if (menu.url.StartsWith("www"))
                    {
                        oNode.NavigateUrl = "http://" + menu.url;
                        oNode.Target = "_blank";
                    }
                    else
                    {
                        oNode.NavigateUrl = "~/" + menu.url;
                    }
                    break;
            }

            oNode.Value = menu.menuId.ToString();

            nodes.Add(oNode);
            GenerateTreenode(Convert.ToInt32(menu.menuId), oNode.ChildNodes);
        }
    }

    private List<System_menu> GetChildNodeList(Int32 masterId, Int32 location)
    {
        var oReturnList = new List<System_menu>();

        foreach (System_menu menu in oAllMenuList)
        {
            if (menu.MasterId == masterId)
            {
                oReturnList.Add(menu);
            }
        }

        return oReturnList;
    }
}