using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_IpFilter : UserControl
{
    Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 26))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        if (!IsPostBack)
        {
            MultiView1.SetActiveView(View1);
            BindDdlFilterType(ddlIpFilterType);
        }

        btnNewIpAddress.Text = AdminResource.lbNewIp;
        BtnSaveUpdateIp.Text = AdminResource.lbSave;
        BtnCancelIp.Text = AdminResource.lbCancel;
        cbState.Text = AdminResource.lbActive;
        lbCopy.Text = AdminResource.lbCopyMyIpAddress;

        gvIpFilterList.Columns[0].HeaderText = AdminResource.lbActions;
        gvIpFilterList.Columns[1].HeaderText = AdminResource.lbName;
        gvIpFilterList.Columns[2].HeaderText = AdminResource.lbIpAdress;
        gvIpFilterList.Columns[3].HeaderText = AdminResource.lbDate;
        gvIpFilterList.Columns[4].HeaderText = AdminResource.lbState;

        BindRbIpFilterOnOff();
    }

    private void BindRbIpFilterOnOff()
    {
        rbIpFilterOnOff_Off.Text = AdminResource.lbOff;
        rbIpFilterOnOff_Off.Attributes["value"]= "0";

        rbIpFilterOnOff_BlackList.Text = AdminResource.lbBlackList;
        rbIpFilterOnOff_BlackList.Attributes["value"] = "1";

        rbIpFilterOnOff_WhiteList.Text = AdminResource.lbWhiteList;
        rbIpFilterOnOff_WhiteList.Attributes["value"] = "2";

        var info = ent.SiteGeneralInfo.First();
        if (info.IpFilter == 0)
            rbIpFilterOnOff_Off.Checked = true;
        
        else if (info.IpFilter == 1)
            rbIpFilterOnOff_BlackList.Checked = true;
        
        else if (info.IpFilter == 2)
            rbIpFilterOnOff_WhiteList.Checked = true;
        
    }

    private void BindDdlFilterType(DropDownList ddlFilterType)
    {
        ClearInputs();
        try
        {
            var wList = new ListItem();
            wList.Text = AdminResource.lbBlackList;
            wList.Value = "True";
            ddlFilterType.Items.Add(wList);
            var bList = new ListItem();
            bList.Text = AdminResource.lbWhiteList;
            bList.Value = "False";
            ddlFilterType.Items.Add(bList);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void ClearInputs()
    {
        ddlIpFilterType_New.DataSource = string.Empty;
        ddlIpFilterType_New.DataBind();
        cbState.Checked = false;
        txtIpAddress.Text = string.Empty;
        txtTitle.Text = string.Empty;
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbIpFilterManagement;// değişecek
    }

    protected void BtnNewIpAddressClick(object sender, EventArgs eventArgs)
    {
        BindDdlFilterType(ddlIpFilterType_New);
        MultiView1.SetActiveView(View2);
    }

    protected void DdlIpFilterTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlIpFilterType.SelectedItem.Value != "")
            {
                
            }
            else
            {
                gvIpFilterList.DataSource = null;
                gvIpFilterList.DataBind();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
    
    public bool CheckIpNumbers()
    {
        string[] data = new string[] { };
        try
        {
            data = txtIpAddress.Text.Split('.');

            // '*' karakter kontrolü
            if (data[0] == "*" )
            {
                return false;
            }
            else if (data[1] == "*" && (data[2] != "*" || data[3]!="*"))
            {
                return false;
            }
            else if (data[2] == "*" && data[3] != "*")
            {
                return false;
            }
            
            // '0..255' aralığı kontrolü
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i] == "*")
                    data[i] = "0";
            }
            int a = Convert.ToInt32(data[0]);
            int b = Convert.ToInt32(data[1]);
            int c = Convert.ToInt32(data[2]);
            int d = Convert.ToInt32(data[3]);

            if (data.Count() != 4
                    || ((a < 0 || a > 255))
                    || ((b < 0 || b > 255))
                    || ((c < 0 || c > 255))
                    || ((d < 0 || d > 255)))
            {
                
                return false;
            }

        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }


    protected void BtnSaveUpdateIp_Click(object sender, EventArgs eventArgs)
    {
        
        if(!CheckIpNumbers())
        {
            MessageBox.Show(MessageType.Warning, AdminResource.msgWrongIpAddress); 
            return;
        }

        try
        {
            SaveUpdateIpFilter();
            MultiView1.SetActiveView(View1);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnCancelIp_Click(object sender, EventArgs eventArgs)
    {
        ClearInputs();
        hfIpAddress.Value = null;
        MultiView1.SetActiveView(View1);
    }

    protected void gvIpFilterList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int ipId = 0;
            if (e.CommandName == "Guncelle")
            {
                BindDdlFilterType(ddlIpFilterType_New);
                ipId = Convert.ToInt32(e.CommandArgument);
                var ipFilter = ent.IpFilterList.First(p => p.Id == ipId);
                txtIpAddress.Text = ipFilter.IpAddress;
                txtTitle.Text = ipFilter.Title;
                cbState.Checked = ipFilter.State;
                if (ipFilter.BlackList)
                    ddlIpFilterType_New.SelectedIndex = 0;
                else
                    ddlIpFilterType_New.SelectedIndex = 1;

                hfIpAddress.Value = ipFilter.Id.ToString();

                MultiView1.SetActiveView(View2);

            }
            else if (e.CommandName == "Sil")
            {
                ipId = Convert.ToInt32(e.CommandArgument);
                var ipFilterList = ent.IpFilterList.First(p => p.Id == ipId);
                ent.DeleteObject(ipFilterList);
                ent.SaveChanges();

                Logger.Add(26, 0, ipFilterList.Id, 2);
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);

                gvIpFilterList.DataBind();
                MultiView1.SetActiveView(View1);
            }


        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void gvIpFilterList_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var myButInnerDelete = (ImageButton)e.Row.FindControl("LinkButtonSil");
                myButInnerDelete.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    public void SaveUpdateIpFilter()
    {
        var ipFilter = new IpFilterList();
        try
        {
            //update
            if (!string.IsNullOrWhiteSpace(hfIpAddress.Value))
            {
                int ipId = Convert.ToInt32(hfIpAddress.Value);
                ipFilter = ent.IpFilterList.First(p => p.Id == ipId);
                ipFilter.UpdatedTime = DateTime.Now;
                ipFilter.Title = txtTitle.Text;
                ipFilter.State = cbState.Checked;
                ipFilter.IpAddress= txtIpAddress.Text;
                if (ddlIpFilterType_New.SelectedIndex == 0)
                    ipFilter.BlackList = true;
                else
                    ipFilter.BlackList = false;

                ent.SaveChanges();
                Logger.Add(26, 0, ipFilter.Id, 3);
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }

            else // insert
            {
                ipFilter.Title = txtTitle.Text;
                ipFilter.IpAddress = txtIpAddress.Text;
                ipFilter.CreatedTime = DateTime.Now;
                ipFilter.UpdatedTime = DateTime.Now;
                ipFilter.State = cbState.Checked;
                if (ddlIpFilterType_New.SelectedIndex == 0)
                    ipFilter.BlackList = true;
                else
                    ipFilter.BlackList = false;

                ent.AddToIpFilterList(ipFilter);
                ent.SaveChanges();
                Logger.Add(26, 0, ipFilter.Id, 1);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }    
        }
        catch(Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        ClearInputs();
        hfIpAddress.Value = null;

        gvIpFilterList.DataBind();
    }

    protected void lbCopyClick(object sender, EventArgs e)
    {
        txtIpAddress.Text = ExceptionManager.GetUserIp();
    }

    protected void rbIpFilterOnOffChanged(object sender, EventArgs e)
    {
        try
        {
            var rb = sender as RadioButton;
            var siteGeneralInfo = ent.SiteGeneralInfo.ToList();
            foreach (var generalInfo in siteGeneralInfo)
            {
                generalInfo.IpFilter = Convert.ToInt32(rb.Attributes["value"]);
            }
            ent.SaveChanges();
            Logger.Add(26, 0, 0, 3);
            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}