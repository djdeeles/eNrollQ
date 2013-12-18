using System;
using Enroll.BaseObjects;
using Enroll.Managers;

public partial class M_UserControls_base_MenuContent : MenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            try
            {
                MenuControlBase oMenuControlBase;
                try
                {
                    oMenuControlBase = (MenuControlBase) Parent.Parent;
                }
                catch
                {
                    oMenuControlBase = (MenuControlBase) Parent;
                }

                OMenuObject = oMenuControlBase.OMenuObject;

                //dinamik listelere sahipise
                if (!Convert.ToBoolean(OMenuObject.isShowDyna))
                    MenuDynamicList_M.Visible = false;
                else
                    MenuDynamicList_M.EnrollInit();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }
}