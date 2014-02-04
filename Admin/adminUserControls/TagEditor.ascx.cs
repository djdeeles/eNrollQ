using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

public partial class Admin_adminUserControls_TagEditor : UserControl
{
    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbInterfaceSettings;

        gvTagValues.Columns[0].HeaderText = AdminResource.lbActions;
        gvTagValues.Columns[1].HeaderText = AdminResource.lbProperty;
        gvTagValues.Columns[2].HeaderText = AdminResource.lbValue;

        ibtnGozat.Text = AdminResource.lbImageSelect;
        btnAddProperty.Text = AdminResource.lbAdd;
        btnClose.Text = AdminResource.lbClose;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 14))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        if (!IsPostBack)
        {
            ColorPicker.Visible = false;
            ddlCssAttValues.Visible = false;
            ddlUzunluk.Visible = false;
            ibtnGozat.Visible = false;
            txtAttValue.Visible = false;
        }
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["htmlTag"]) && !IsPostBack)
            {
                var ent = new Entities();
                int htmlTag = Convert.ToInt32(Request.QueryString["htmlTag"]);

                ddlAttList.DataSource = GetCssAttributes();
                ddlAttList.DataTextField = "cssDescription";
                ddlAttList.DataValueField = "cssAttributeId";
                ddlAttList.DataBind();

                lblHtmlTag.Text = ent.EnrollHtmlTags.First(p => p.id == htmlTag).Tag;
                edsTagValues.WhereParameters.Add("htmlTagId", DbType.Int32, htmlTag.ToString());
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private List<EnrollCssAttributes> GetCssAttributes()
    {
        var ent = new Entities();
        var cssAttList = ent.EnrollCssAttributes.OrderBy(p => p.cssDescription).ToList();
        return cssAttList;
    }

    protected void btnAddProperty_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            int tagId = Convert.ToInt32(Request.QueryString["htmlTag"]);
            var ent = new Entities();
            var tv = new EnrollHtmlTagValues();
            tv.htmlTagId = tagId;
            tv.cssAttributeId = Convert.ToInt32(ddlAttList.SelectedValue);
            EnrollCssAttributes cssAttribute =
                ent.EnrollCssAttributes.First(p => p.cssAttributeId == tv.cssAttributeId);
            switch (cssAttribute.cssValueTypeId)
            {
                case 1: //uzunluk
                    if (ddlUzunluk.SelectedValue == "px")
                    {
                        tv.value = txtAttValue.Text + "px";
                    }
                    else
                    {
                        tv.value = txtAttValue.Text + "%";
                    }
                    break;
                case 2: //dropdown'dan seçilecek
                    tv.value = ddlCssAttValues.SelectedItem.Text;
                    break;
                case 3: //renk
                    tv.value = ColorTranslator.ToHtml(ColorPicker.SelectedColor);
                    break;
                case 4: //dosya
                    tv.value = "url('" + txtAttValue.Text.Replace("~", "") + "')";
                    break;
                case null: //custom
                    tv.value = txtAttValue.Text;
                    break;
            }
            tv.cssAttDesc = cssAttribute.cssDescription;
            ent.AddToEnrollHtmlTagValues(tv);
            ent.SaveChanges();
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
            var ent = new Entities();
            int attId = Convert.ToInt32(ddlAttList.SelectedValue);
            EnrollCssAttributes cssAttribute = ent.EnrollCssAttributes.First(p => p.cssAttributeId == attId);

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
                    var attVal =
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

    protected void gvTagValues_RowDataBound(object sender, GridViewRowEventArgs e)
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