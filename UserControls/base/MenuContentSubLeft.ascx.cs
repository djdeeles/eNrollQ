using System;
using Enroll.BaseObjects;
using Enroll.Managers;

public partial class UserControls_MenuContentSubLeft : MenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var oMenuControlBase = (MenuControlBase) Parent.Parent;
            OMenuObject = oMenuControlBase.OMenuObject;

            //masterid nin nerden alınacağına karar verilir.;
            Int32 masterId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["masterId"]))
                masterId = Convert.ToInt32(Request.QueryString["masterId"]);
            else
                masterId = Convert.ToInt32(OMenuObject.menuId);

            MenuSubMenuLeftTree1.LoadTreeview(masterId);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}