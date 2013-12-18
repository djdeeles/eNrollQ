using System;
using System.Configuration;
using Enroll.BaseObjects;

public partial class UserControls_SubMenu : DropMenuControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MainMenu.Items.Clear();
        InitMenu(2, ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());
        GenerateMenu(0, MainMenu.Items);
    }
}