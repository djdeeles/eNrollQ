using System;
using System.Configuration;
using Enroll.BaseObjects;
using Enroll.Managers;

public partial class UserControls_MainMenuBottom : DropMenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            MainMenu.Items.Clear();
            InitMenu(2, ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
            GenerateMenu(0, MainMenu.Items);
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