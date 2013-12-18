using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_ErrorList : Page
{
    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbErrorsAndNotes;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 13))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        GridView1.Columns[0].HeaderText = AdminResource.lbNo;
        GridView1.Columns[1].HeaderText = AdminResource.lbError;
        GridView1.Columns[2].HeaderText = AdminResource.lbErrorSummary;
        GridView1.Columns[3].HeaderText = AdminResource.lbUrl;
        GridView1.Columns[4].HeaderText = AdminResource.lbErrorDate;
        GridView1.Columns[5].HeaderText = AdminResource.lbIpAdress;

        btDeleteErrors.Text = AdminResource.lbDeleteAll;

        GridView1.EmptyDataText = AdminResource.lbNoRecord;
    }

    protected void btDeleteErrors_Click(object sender, EventArgs e)
    {
        var entities = new Entities();
        var errors = entities.System_Errors.ToList();
        foreach (var systemErrors in errors)
        {
            entities.DeleteObject(systemErrors);
        }
        entities.SaveChanges();

        Logger.Add(13, 0, 0, 2);

        GridView1.DataBind();
    }

    protected void ImageButton2Ara_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var aranan = txtAra.Text;

            EntityDataSource1.Where = " it.ErrorMessage like '%" + aranan + "%'";
            EntityDataSource1.DataBind();
            GridView1.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "ErrorList.aspx:ImageButton2Ara_Click()");
        }
    }
}