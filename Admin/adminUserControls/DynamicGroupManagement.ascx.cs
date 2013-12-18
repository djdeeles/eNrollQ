using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_DynamicListManagement : UserControl
{
    Entities oEntities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbDynamicFieldManagement;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 16))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }
        if (!IsPostBack)
        {
            try
            {
                hdnMenuId.Value = Request.QueryString["id"];
                GetDataProccess();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        btnAdd.Text = AdminResource.lbAdd;
        btnRemove.Text = AdminResource.lbDelete;
        btSave.Text = AdminResource.lbSave;
        btCancel.Text = AdminResource.lbCancel;
    }

    private void GetDataProccess()
    {
        int menuId = Convert.ToInt32(hdnMenuId.Value); 
        var oMenu = oEntities.System_menu.FirstOrDefault(p => p.menuId == menuId);

        if (oMenu != null)
        {
            chkDyna.Checked = Convert.ToBoolean(oMenu.isShowDyna);

            txtDynaDisplay.Text = oMenu.dynaDisplayText;

            if (oMenu.dynaDisplayType != 0 & oMenu.dynaDisplayType != null)
                ddlDynaDisType.SelectedValue = oMenu.dynaDisplayType.ToString();

            foreach (var refe in oEntities.Customer_Dynamic_Reference.Where(p => p.MenuId == oMenu.menuId))
            {
                refe.Customer_DynamicReference.Load();
                ListBoxRef.Items.Add(new ListItem(refe.Customer_Dynamic.name, refe.Customer_Dynamic.dynamicId.ToString()));
            }
        }
    }

    private void UpdateRefData(System_menu menu, Entities entities)
    {
        try
        {
            var blnChange = false;
            //önce listboxta olmayanlar silinir.
            foreach (
                Customer_Dynamic_Reference reference in
                    entities.Customer_Dynamic_Reference.Where(p => p.MenuId == menu.menuId).ToList())
            {
                reference.Customer_DynamicReference.Load();
                if (!CheckListData(reference.Customer_Dynamic.dynamicId.ToString())) //yoksa silinir.
                {
                    entities.DeleteObject(reference);
                    blnChange = true;
                }
            }

            if (blnChange)
            {
                entities.SaveChanges();
                Logger.Add(16, 4, 0, 2);
            }

            //database de olmayanlar eklenir.
            foreach (ListItem item in ListBoxRef.Items)
            {
                int menuId = Convert.ToInt32(menu.menuId);
                if (menuId > 0 && !CheckDbMenuDynamic(item.Value, entities.Customer_Dynamic_Reference.
                                                                      Where(p => p.MenuId == menuId).ToList()))
                {
                    var oRef = new Customer_Dynamic_Reference();
                    oRef.MenuId = menu.menuId;
                    var dynamicId = Convert.ToInt32(item.Value);
                    if (dynamicId > 0)
                    {
                        oRef.Customer_Dynamic = entities.Customer_Dynamic.FirstOrDefault(p => p.dynamicId == dynamicId);
                    }
                    Logger.Add(16, 4, 0, 1);
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ListBoxDynaSource.SelectedItem != null && ListBoxDynaSource.SelectedItem.Value != "")
            {
                if (!CheckListData(ListBoxDynaSource.SelectedItem.Value))
                {
                    int dynamicId = Convert.ToInt32(ListBoxDynaSource.SelectedItem.Value);
                    if (dynamicId > 0)
                    {
                        var oDynamic = oEntities.Customer_Dynamic.FirstOrDefault(
                            p => p.dynamicId == dynamicId);
                        if (oDynamic != null)
                        {
                            ListBoxRef.Items.Add(new ListItem(oDynamic.name, oDynamic.dynamicId.ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            if (ListBoxRef.SelectedItem != null)
            {
                ListBoxRef.Items.Remove(ListBoxRef.SelectedItem);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private bool CheckListData(String id)
    {
        var blnReturn = false;
        try
        {
            foreach (ListItem item in ListBoxRef.Items)
            {
                if (item.Value == id)
                    blnReturn = true;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return blnReturn;
    }

    private Boolean CheckDbMenuDynamic(String id, List<Customer_Dynamic_Reference> refList)
    {
        var blnReturn = false;

        foreach (var refe in refList)
        {
            refe.Customer_DynamicReference.Load();
            if (id == refe.Customer_Dynamic.dynamicId.ToString())
            {
                blnReturn = true;
            }
        }
        return blnReturn;
    }

    protected void ddlDynamicGroup_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            if (ddlDynamicGroup.SelectedValue != Resource.lbChoose)
            {
                EntityDynaData.Where = "it.Customer_Dynamic_Group.groupId=" + ddlDynamicGroup.SelectedValue;
                ListBoxDynaSource.DataSource = EntityDynaData;
                ListBoxDynaSource.DataTextField = "name";
                ListBoxDynaSource.DataValueField = "dynamicId";
                ListBoxDynaSource.DataBind();
            }
            else
            {
                ListBoxDynaSource.Items.Clear();
            }
            btnAdd.Enabled = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ddlDynamicGroup_DataBound(object sender, EventArgs e)
    {
        try
        {
            var oList = (DropDownList) sender;
            oList.Items.Insert(0, new ListItem(Resource.lbChoose));
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btSave_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            int menuId = Convert.ToInt32(hdnMenuId.Value);
            var oMenu = oEntities.System_menu.FirstOrDefault(p => p.menuId == menuId);
            if (oMenu != null)
            {
                oMenu.isShowDyna = chkDyna.Checked;
                oMenu.dynaDisplayText = txtDynaDisplay.Text;
                oMenu.dynaDisplayType = Convert.ToInt32(ddlDynaDisType.SelectedValue);
                UpdateRefData(oMenu, oEntities);
            }
            Logger.Add(16, 4, menuId, 3);
            oEntities.SaveChanges();

            Page.ClientScript.RegisterStartupScript(GetType(), "close", "<script>window.close();</script>");
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}