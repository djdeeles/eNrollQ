using System;
using Enroll.BaseObjects;
using Enroll.Managers;

public partial class UserControls_base_MenuContent : MenuControlBase
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

                //alt menüleri gösterilecekse
                if (OMenuObject.subMenuShowType != 1)
                    MenuSubMenu1.Visible = false;
                //dinamik listelere sahipise
                if (!Convert.ToBoolean(OMenuObject.isShowDyna))
                    MenuDynamicList1.Visible = false;
                else
                    MenuDynamicList1.EnrollInit();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }
}