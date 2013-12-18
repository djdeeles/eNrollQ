using System;
using System.Collections.Generic;
using System.Linq;
using Enroll.BaseObjects;
using Enroll.Managers;
using eNroll.App_Data;

public partial class M_UserControls_SubMenu : DropMenuControlBase
{
    private readonly Entities entities = new Entities();
    protected List<System_menu> MenuList = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        int masterPageId = 0;
        try
        {
            var a = Request.QueryString;
            if (Request.QueryString["id"] != null)
            {
                masterPageId = Convert.ToInt32(Request.QueryString["id"]);
            }
            MenuList =
                entities.System_menu.Where(
                    p =>
                    p.location == 1 && p.state == true &&
                    p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId && p.MasterId == masterPageId).
                    ToList();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}