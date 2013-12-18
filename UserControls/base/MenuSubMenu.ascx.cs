using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Enroll.BaseObjects;
using Resources;
using eNroll.App_Data;

public partial class UserControls_MenuSubMenu : MenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var oMenuControlBase = (MenuControlBase) Parent;
        OMenuObject = oMenuControlBase.OMenuObject;
        var oentity = new Entities();
        List<System_menu> oList =
            oentity.System_menu.Where("it.MasterId=" + OMenuObject.menuId + " and it.languageId=" +
                                      EnrollContext.Current.WorkingLanguage.LanguageId.ToString() +
                                      " and it.state=true and it.location=" + OMenuObject.location.ToString() +
                                      " order by it.menuIndex").ToList();
        DataList1.DataSource = oList;
        DataList1.DataBind();
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        var myHidId = (HiddenField) e.Item.FindControl("hdnId");
        var myLinkBtn = (HyperLink) e.Item.FindControl("HyperLink1");
        var mylblBrief = (Label) e.Item.FindControl("lblMenuBrief");
        var lblMenuName = (Label) e.Item.FindControl("lblMenuName");
        if (mylblBrief.Text.Length < 500)
        {
            myLinkBtn.NavigateUrl = UrlMapping.LinkOlustur(myHidId.Value, lblMenuName.Text, "sayfa");
            myLinkBtn.Text = Resource.details;
        }
        else
        {
            myLinkBtn.Visible = false;
        }
    }

    protected void Literal1_DataBinding(object sender, EventArgs e)
    {
        var myLiteral = (Literal) sender;
        if (!string.IsNullOrEmpty(myLiteral.Text))
        {
            String strType = myLiteral.Text.Substring(myLiteral.Text.Length - 3, 3);
            switch (strType)
            {
                case "swf":
                    myLiteral.Text = CreateObjectTag("248", "81", myLiteral.Text.Replace("~/", ".."));
                    break;
                default:
                    myLiteral.Text = CreateImageTag(myLiteral.Text);
                    break;
            }
        }
    }

    private String CreateObjectTag(String w, String h, String src)
    {
        String strResult = string.Empty;
        strResult = "<object width=\"" + w + "\" height=\"" + h + "\">";
        strResult += "<param name=\"movie\" value=\"" + src + "\">";
        strResult += "<embed src=\"" + src + "\" width=\"" + w + "\" height=\"" + h + "\">";
        strResult += "</embed></object>";
        return strResult;
    }

    private String CreateImageTag(String src)
    {
        String strResult = string.Empty;
        strResult = "<img src=\"" + src.Replace("~/", "") + "\" />";
        return strResult;
    }

    protected void lblMenuBrief_DataBinding(object sender, EventArgs e)
    {
        var myLabel = (Label) sender;
        myLabel.Text = Server.HtmlDecode(myLabel.Text);
    }
}