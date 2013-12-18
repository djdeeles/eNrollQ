using System;
using System.Configuration;
using Enroll.BaseObjects;
using Enroll.Managers;

public partial class UserControls_MainMenu : DropMenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            MainMenuVertical.Items.Clear();
            InitMenu(1, ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
            GenerateMenu(0, MainMenuVertical.Items);
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

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Page.Request.ServerVariables["http_user_agent"].ToLower().Contains("safari"))
        {
            Page.ClientTarget = "uplevel";
        }
    }
}