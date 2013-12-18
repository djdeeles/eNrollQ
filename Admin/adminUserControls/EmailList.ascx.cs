using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_EmailList : UserControl
{
    Entities _entities = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 19))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }
        btnExcel.Text = AdminResource.lbExcelExport;
        GridEmails.Columns[0].HeaderText = AdminResource.lbActions;
        GridEmails.Columns[1].HeaderText = AdminResource.lbName;
        GridEmails.Columns[2].HeaderText = AdminResource.lbSurname;
        GridEmails.Columns[3].HeaderText = AdminResource.lbEmails;

        GridEmails.DataBind();
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbNewsGroupMember;
    }

    protected void DownloadExcel(object sender, EventArgs e)
    {
        var view = new GridView();
        try
        {
            var list = _entities.EmailList.ToList();
            var stringWriter = new StringWriter();
            view.DataSource = list;
            view.DataBind();

            Response.Clear();
            Response.AddHeader("content-disposition",
                               "attachment;filename=" + DateTime.Now.ToShortDateString() +
                               ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";

            var htmlTextWriter = new HtmlTextWriter(stringWriter);
            view.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        Response.End();
    }

    protected void imgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        var myButton = (ImageButton)sender;
        DeletingProccess(myButton.CommandArgument);
        GridEmails.DataBind();
    }

    protected void DeletingProccess(string id)
    {
        try
        { 
            EmailList email = _entities.EmailList.Where("it.id=" + id).FirstOrDefault();

            Logger.Add(19, 0, email.id, 2);

            _entities.DeleteObject(email);
            _entities.SaveChanges();

            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GvEmailsRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                if (btnDelete != null)
                    btnDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GvEmailsRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Update")
        {
            int id = int.Parse(e.CommandArgument.ToString());
            EmailList emailList = _entities.EmailList.First(p => p.id == id);
            emailList.UpdatedTime = DateTime.Now;
            _entities.SaveChanges();

            Logger.Add(19, 0, emailList.id, 3);

            MessageBox.Show(MessageType.Success,AdminResource.msgUpdated);
        }
    }
}