using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Enroll.Managers;
using eNroll.App_Data;

public partial class UserControls_base_MainMenu : UserControl
{
    private readonly Entities entities = new Entities();
    protected List<System_menu> MenuList0 = null;
    protected List<System_menu> MenuList1 = null;
    protected List<System_menu> MenuList3 = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            MenuList0 =
                entities.System_menu.Where(
                    p =>
                    p.location == 0 && p.state == true &&
                    p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId && p.MasterId == 0).ToList();


            MenuList1 =
                entities.System_menu.Where(
                    p =>
                    p.location == 1 && p.state == true &&
                    p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId && p.MasterId == 0).ToList();
            MenuList3 =
                entities.System_menu.Where(
                    p =>
                    p.location == 3 && p.state == true &&
                    p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId && p.MasterId == 0).ToList();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}