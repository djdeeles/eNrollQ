using System;
using System.Configuration;
using Enroll.BaseObjects;
using Enroll.Managers;

public partial class UserControls_MainMenuTop : DropMenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            MainMenuTop.Items.Clear();
            //0 üst menü
            InitMenu(3, ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
            GenerateMenu(0, MainMenuTop.Items);
            if (Request.UserAgent.Contains("AppleWebKit"))
            {
                Request.Browser.Adapters.Clear();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}