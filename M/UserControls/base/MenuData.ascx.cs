using System;
using Enroll.BaseObjects;

public partial class M_UserControls_MenuData : MenuControlBase
{
    protected string MenuName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        var oMenuControlBase = (MenuControlBase) Parent;
        OMenuObject = oMenuControlBase.OMenuObject;

        if (!Convert.ToBoolean(OMenuObject.isHideMenuName))
            MenuName = "<h1>" + OMenuObject.name + "</h1>";
        else
            MenuName = string.Empty;


        //image
        if (!string.IsNullOrEmpty(OMenuObject.masterImage))
        {
            if (OMenuObject.masterImage.Substring(OMenuObject.masterImage.Length - 3, 3) != "swf")
            {
                imgMenuImage.ImageUrl = OMenuObject.masterImage;
            }
            else
                imgMenuImage.Visible = false;
        }
        else
        {
            imgMenuImage.Visible = false;
        }

        //details 
        if (!string.IsNullOrEmpty(OMenuObject.Details))
        {
            oMenuControlBase.ContentList = OMenuObject.Details.Split('[');
            oMenuControlBase.LoadMobilControl(PlaceHolder1);
        }
        else
        {
            MenuName = string.Empty;
            imgMenuImage.Visible = false;
        }
    }
}