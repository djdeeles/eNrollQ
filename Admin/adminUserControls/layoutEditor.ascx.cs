using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

// Oluşturulan paneller daha sıralanmıyor.


public partial class Admin_adminUserControls_layoutEditor : UserControl
{
    Entities ent = new Entities();
    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbInterfaceSettings;

        btnCancel.Text = AdminResource.lbClose;
        btnSave.Text = AdminResource.lbSave;

        btnSaveProperty.Text = AdminResource.lbSave;
        btnCancelAddproperty.Text = AdminResource.lbCancel;
        ibtnGozat.Text = AdminResource.lbImageSelect;

        gvAttValues.Columns[0].HeaderText = AdminResource.lbActions;
        gvAttValues.Columns[1].HeaderText = AdminResource.lbProperty;
        gvAttValues.Columns[2].HeaderText = AdminResource.lbValue;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 14))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        } 
        if (!IsPostBack && !string.IsNullOrEmpty(Request.QueryString["loc"]))
        {
            ColorPicker.Visible = false;
            ddlCssAttValues.Visible = false;
            ddlUzunluk.Visible = false;
            ibtnGozat.Visible = false;
            txtAttValue.Visible = false;
            try
            {
                var location = Convert.ToInt32(Request.QueryString["loc"]);
                switch (location)
                {
                    case 0:
                        lblLocation.Text = "Tepe";
                        break;
                    case 1:
                        lblLocation.Text = "İçerik";
                        break;
                    case 2:
                        lblLocation.Text = "Gövde";
                        break;
                    case 3:
                        lblLocation.Text = "Alt";
                        break;
                }
                var moduleList = ent.EnrollModules.Where(p => p.location == location).ToList();
                var checkControl = ent.EnrollHtmlPanels.Where(p => p.location == location).ToList();
                foreach (var pan in checkControl)
                {
                    var module = ent.EnrollModules.First(p => p.moduleId == pan.moduleId);
                    moduleList.Remove(module);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["div"]))
                {
                    string divID = Request.QueryString["div"];
                    var panel = ent.EnrollHtmlPanels.FirstOrDefault(p => p.divId == divID);
                    var cssAttList = GetCssAttributes();
                    if (panel != null)
                    {
                        var panelValues =
                            ent.EnrollHtmlPanelValues.Where(p => p.panelId == panel.id).ToList();
                        foreach (var pv in panelValues)
                        {
                            var attributes =
                                ent.EnrollCssAttributes.First(p => p.cssAttributeId == pv.cssAttributeId);
                            cssAttList.Remove(attributes);
                        }
                        var module = ent.EnrollModules.First(p => p.moduleId == panel.moduleId);
                        moduleList.Add(module);
                        ddlKontroller.SelectedValue = module.moduleId.ToString();

                        ibtnArti.Enabled = true;
                        if (!IsPostBack)
                        {
                            txtName.Text = panel.name;
                        }
                    }
                    else
                    {
                        edsEnrollPanelValues = null;
                        gvAttValues.Visible = false;
                    }
                    ddlAttList.DataSource = cssAttList;
                    ddlAttList.DataTextField = "cssDescription";
                    ddlAttList.DataValueField = "cssAttributeId";
                    ddlAttList.DataBind();
                }
                ddlKontroller.DataSource = moduleList.Where(p => p.location == location);
                ddlKontroller.DataTextField = "name";
                ddlKontroller.DataValueField = "moduleId";
                ddlKontroller.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void btnArti_Click(object sender, ImageClickEventArgs e)
    {
        Panel1.Visible = true;
        ibtnArti.Visible = false;
    }

    protected void ImageButton1_Click(object sender, EventArgs eventArgs)
    {
        bool saved = false;
        int id_ = 0;
        string divId_ = string.Empty;
        int location_ = 0;
        try
        {
            ibtnArti.Visible = true;
            ibtnArti.Enabled = true;
            btnCancelAddproperty.Visible = false;
            if (!String.IsNullOrEmpty(Request.QueryString["div"]) && !String.IsNullOrEmpty(Request.QueryString["loc"]))
            { 
                string divId = Request.QueryString["div"];
                int location = Convert.ToInt32(Request.QueryString["loc"]);
                int kontrol = ent.EnrollHtmlPanels.Count(p => p.divId == divId);

                EnrollHtmlPanels panel;
                if (kontrol == 0) panel = new EnrollHtmlPanels();
                else panel = ent.EnrollHtmlPanels.First(p => p.divId == divId);
                if (!String.IsNullOrEmpty(Request.QueryString["parent"]))
                {
                    int parentId = Convert.ToInt32(Request.QueryString["parent"]);
                    panel.parentId = parentId;
                }
                else panel.parentId = null;

                panel.divId = divId;
                divId_ = divId;

                panel.location = location;
                location_ = location;

                panel.name = txtName.Text;

                panel.moduleId = Convert.ToInt32(ddlKontroller.SelectedValue);
                panel.divOrder = Convert.ToInt32(Session[divId + "_order"]);
                if (kontrol == 0) ent.AddToEnrollHtmlPanels(panel);
                ent.SaveChanges();
                id_ = panel.id;
                saved = true;
                Session.Remove(divId + "_order");
                ent.SaveChanges();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        if (saved)
        {
            Response.Redirect("EditContent.aspx?content=LayoutEditor&id=" + id_ + "&" +
                              "div=" + divId_ + "&loc=" + location_);
        }
    }

    protected void btnCancelAddproperty_Click(object sender, EventArgs eventArgs)
    {
        Panel1.Visible = false;
        ibtnArti.Visible = true;
    }

    private List<EnrollCssAttributes> GetCssAttributes()
    { 
        return ent.EnrollCssAttributes.OrderBy(p => p.cssDescription).ToList();
    }

    protected void btnSaveProperty_Click(object sender, EventArgs eventArgs)
    {
        try
        { 
            var pval = new EnrollHtmlPanelValues();
            string divId = Request.QueryString["div"];
            var panel = ent.EnrollHtmlPanels.First(p => p.divId == divId);
            pval.panelId = panel.id;

            pval.cssAttributeId = Convert.ToInt32(ddlAttList.SelectedValue);
            pval.cssAttributeValue = ddlAttList.SelectedItem.Text;
            var cssAttribute =
                ent.EnrollCssAttributes.First(p => p.cssAttributeId == pval.cssAttributeId);
            pval.cssAttDesc = cssAttribute.cssDescription;
            switch (cssAttribute.cssValueTypeId)
            {
                case 1: //uzunluk
                    if (ddlUzunluk.SelectedValue == "px")
                    {
                        pval.cssAttributeValue = txtAttValue.Text + "px";
                    }
                    else
                    {
                        pval.cssAttributeValue = txtAttValue.Text + "%";
                    }
                    break;
                case 2: //dropdown'dan seçilecek
                    pval.cssAttributeValue = ddlCssAttValues.SelectedItem.Text;
                    break;
                case 3: //renk
                    pval.cssAttributeValue = ColorTranslator.ToHtml(ColorPicker.SelectedColor);
                    break;
                case 4: //dosya
                    pval.cssAttributeValue = "url('" + txtAttValue.Text.Replace("~", "") + "')";
                    break;
                case null:
                    pval.cssAttributeValue = txtAttValue.Text;
                    break;
            }
            ent.AddToEnrollHtmlPanelValues(pval);
            ent.SaveChanges();
            ddlAttList.SelectedValue = null;
            txtAttValue.Text = "";
            Panel1.Visible = false;
            ibtnArti.Visible = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        Response.Redirect(Request.RawUrl);
    }

    protected void ddlAttList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        { 
            int attId = Convert.ToInt32(ddlAttList.SelectedValue);
            var cssAttribute = ent.EnrollCssAttributes.First(p => p.cssAttributeId == attId);

            ColorPicker.Visible = false;
            ddlCssAttValues.Visible = false;
            ddlUzunluk.Visible = false;
            ibtnGozat.Visible = false;
            txtAttValue.Visible = false;

            switch (cssAttribute.cssValueTypeId)
            {
                case null: //text
                    txtAttValue.Visible = true;
                    break;
                case 1: //uzunluk
                    //txtAttValue_ColorPickerExtender.Enabled = false;
                    txtAttValue.Visible = true;
                    ddlUzunluk.Visible = true;
                    rwTxtAttValue.Enabled = true;
                    if (ddlUzunluk.SelectedValue == "px")
                    {
                        rwTxtAttValue.MaximumValue = "1260";
                        rwTxtAttValue.MinimumValue = "-9999";
                        rwTxtAttValue.ErrorMessage = AdminResource.lbMsgInvalidInput;
                    }
                    else
                    {
                        rwTxtAttValue.MaximumValue = "100";
                        rwTxtAttValue.MinimumValue = "0";
                        rwTxtAttValue.ErrorMessage = AdminResource.lbMsgInvalidInput;
                    }
                    break;
                case 2: //ddl'den seçilecek

                    ddlCssAttValues.Visible = true;
                    int cssAttId = Convert.ToInt32(ddlAttList.SelectedValue);
                    List<EnrollCssAttributeValues> attVal =
                        ent.EnrollCssAttributeValues.Where(p => p.cssAttributeId == cssAttId).ToList();
                    ddlCssAttValues.DataSource = attVal;
                    ddlCssAttValues.DataValueField = "cssValue";
                    ddlCssAttValues.DataTextField = "cssValue";
                    ddlCssAttValues.DataBind();
                    ddlCssAttValues.Items.Insert(0, AdminResource.lbChoose);
                    break;
                case 3: //renk
                    ColorPicker.Visible = true;
                    break;
                case 4: //dosya
                    txtAttValue.Visible = true;
                    ibtnGozat.Visible = true;
                    break;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void gvAttValues_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            var deleteButton = e.Row.FindControl("btnDelete") as ImageButton;
            deleteButton.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "')";
        }
        catch (Exception)
        {
        }
    }
}