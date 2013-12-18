using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

public partial class Admin_adminUserControls_SiteLayout : UserControl
{
    private Entities ent;

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbInterfaceSettings;
        btnSave.Text = AdminResource.lbSave;
    }

    private void getChilds(Panel pnlParent, EnrollHtmlPanels panel, List<EnrollHtmlPanels> childPanels)
    {
        foreach (EnrollHtmlPanels childPanel in childPanels)
        {
            try
            {
                var pnlChild = new Panel();
                pnlChild.ID = childPanel.divId;
                pnlChild.Controls.Add(new LiteralControl(childPanel.EnrollModules.name));
                var lbSil = new LinkButton();
                lbSil.Text = " " + AdminResource.lbDelete + " | ";
                lbSil.Font.Size = FontUnit.XSmall;
                lbSil.CommandArgument = childPanel.id.ToString();
                lbSil.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "');";
                lbSil.Click += lbSil_Click;
                var lbDuzenle = new LinkButton();
                lbDuzenle.Font.Size = FontUnit.XSmall;
                lbDuzenle.Text = AdminResource.lbEdit + " | ";
                lbDuzenle.OnClientClick = "window.open('EditContent.aspx?content=LayoutEditor&id=" + childPanel.id +
                                          "&div=" + childPanel.divId +
                                          "&loc=" + childPanel.location.ToString() + "&parent=" +
                                          childPanel.parentId.ToString() +
                                          "','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=yes');";
                var lbAddChildControl = new LinkButton();
                lbAddChildControl.Text = AdminResource.lbNew;
                lbAddChildControl.OnClientClick = "window.open('EditContent.aspx?content=LayoutEditor&div=" + newKey() +
                                                  "&loc=" +
                                                  panel.location + "&parent=" + panel.id +
                                                  "','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=yes');";
                lbAddChildControl.Font.Size = FontUnit.XSmall;
                lbAddChildControl.Attributes.Add("margin-top", "8px");
                pnlChild.Controls.Add(lbSil);
                pnlChild.Controls.Add(lbDuzenle);
                pnlChild.Controls.Add(lbAddChildControl);
                List<EnrollHtmlPanelValues> pvals = ent.EnrollHtmlPanelValues.Where(p => p.panelId == panel.id).ToList();
                foreach (EnrollHtmlPanelValues v in pvals)
                {
                    v.EnrollCssAttributesReference.Load();
                    if (v.EnrollCssAttributes.cssValueTypeId == 1)
                    {
                        string type = "";
                        string value = "";
                        if (v.cssAttributeValue.Contains("px"))
                        {
                            type = "px";
                            value = v.cssAttributeValue.Replace("px", "");
                        }
                        else
                        {
                            type = "%";
                            value = v.cssAttributeValue.Replace("%", "");
                        }
                        int length = 0;
                        if (!String.IsNullOrEmpty(value))
                        {
                            try
                            {
                                length = Convert.ToInt32(value);
                            }
                            catch (Exception)
                            {
                                length = 0;
                            }
                        }
                        length = Convert.ToInt32(length/8*6);
                        pnlChild.Attributes.CssStyle.Add(v.EnrollCssAttributes.cssAttribute, length.ToString() + type);
                    }
                    else pnlChild.Attributes.CssStyle.Add(v.EnrollCssAttributes.cssAttribute, v.cssAttributeValue);
                }
                pnlChild.Attributes.CssStyle.Add("background-color", "#dfdfdf");
                pnlParent.Controls.Add(pnlChild);
                pnlChild.CssClass = childPanel.divId;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    private void onizle(int location, PlaceHolder ph)
    {
        List<EnrollHtmlPanels> paneller = null;
        try
        {
            paneller = ent.EnrollHtmlPanels.Where(p => p.location == location && p.parentId == null).
                OrderBy(p => p.divOrder).ToList();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        if (paneller != null)
            foreach (EnrollHtmlPanels panel in paneller)
            {
                try
                {
                    var pnlDynamic = new Panel();
                    pnlDynamic.ID = panel.divId;
                    pnlDynamic.Controls.Add(new LiteralControl(panel.EnrollModules.name + " "));
                    var lbSil = new LinkButton();
                    lbSil.Text = AdminResource.lbDelete + " | ";
                    lbSil.Font.Size = FontUnit.XSmall;
                    lbSil.CommandArgument = panel.id.ToString();
                    lbSil.OnClientClick = "return confirm(' " + AdminResource.lbDeletingQuestion + "');";
                    lbSil.Click += lbSil_Click;
                    var lbDuzenle = new LinkButton();
                    lbDuzenle.Font.Size = FontUnit.XSmall;
                    lbDuzenle.Text = AdminResource.lbEdit + " | ";
                    lbDuzenle.OnClientClick = "window.open('EditContent.aspx?content=LayoutEditor&id=" + panel.id +
                                              "&div=" + panel.divId + "&loc=" +
                                              panel.location.ToString() +
                                              "','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=no');";
                    var lbAddChildControl = new LinkButton();
                    lbAddChildControl.Text = AdminResource.lbNew;
                    lbAddChildControl.OnClientClick = "window.open('EditContent.aspx?content=LayoutEditor&div=" +
                                                      newKey() + "&loc=" +
                                                      panel.location + "&parent=" + panel.id +
                                                      "','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=no');";
                    lbAddChildControl.Font.Size = FontUnit.XSmall;
                    pnlDynamic.Controls.Add(lbSil);
                    pnlDynamic.Controls.Add(lbDuzenle);
                    pnlDynamic.Controls.Add(lbAddChildControl);
                    List<EnrollHtmlPanelValues> pvals =
                        ent.EnrollHtmlPanelValues.Where(p => p.panelId == panel.id).ToList();
                    foreach (EnrollHtmlPanelValues v in pvals)
                    {
                        v.EnrollCssAttributesReference.Load();
                        if (v.EnrollCssAttributes.cssValueTypeId == 1)
                        {
                            string type = "";
                            string value = "";
                            if (v.cssAttributeValue.Contains("px"))
                            {
                                type = "px";
                                value = v.cssAttributeValue.Replace("px", "");
                            }
                            else
                            {
                                type = "%";
                                value = v.cssAttributeValue.Replace("%", "");
                            }
                            int length = 0;
                            if (!String.IsNullOrEmpty(value))
                            {
                                try
                                {
                                    length = Convert.ToInt32(value);
                                }
                                catch (Exception)
                                {
                                    length = 0;
                                }
                            }
                            length = Convert.ToInt32(length/8*6);
                            pnlDynamic.Attributes.CssStyle.Add(v.EnrollCssAttributes.cssAttribute,
                                                               length.ToString() + type);
                        }
                        else
                            pnlDynamic.Attributes.CssStyle.Add(v.EnrollCssAttributes.cssAttribute, v.cssAttributeValue);
                    }
                    pnlDynamic.Attributes.CssStyle.Add("background-color", "#dfdfdf");
                    ph.Controls.Add(pnlDynamic);
                    pnlDynamic.CssClass = panel.divId;
                    List<EnrollHtmlPanels> childPanels =
                        ent.EnrollHtmlPanels.Where(x => x.parentId == panel.id).ToList();
                    if (childPanels.Count > 0)
                    {
                        getChilds(pnlDynamic, panel, childPanels);
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
    }

    private string newKey()
    {
        string nKey = string.Empty;
        string a = ("abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ");
        var pw = new char[8];
        var r = new Random();
        try
        {
            for (int i = 0; i < 8; i++)
            {
                pw[i] = a[r.Next(0, a.Count() - 1)];
            }
            nKey = new string(pw);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return nKey;
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
        ent = new Entities();
        if (!IsPostBack)
        {
            try
            {
                ddlHtmlTag.DataSource = ent.EnrollHtmlTags.ToList();
                ddlHtmlTag.DataValueField = "id";
                ddlHtmlTag.DataTextField = "Tag";
                ddlHtmlTag.DataBind();
                ddlHtmlTag.Items[0].Selected = true;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        try
        {
            string selected = Request.Form.Get("ctl00$ContentPlaceHolder1$ctl00$ddlHtmlTag");

            if (selected == null) selected = "1";
            ibtnTepe.OnClientClick = "window.open('EditContent.aspx?content=LayoutEditor&div=" + newKey() +
                                     "&loc=0&htmlTag=no','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=yes')";
            ibtnGovde.OnClientClick = "window.open('EditContent.aspx?content=LayoutEditor&div=" + newKey() +
                                      "&loc=1&htmlTag=no','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=yes')";
            ibtnAlt.OnClientClick = "window.open('EditContent.aspx?content=LayoutEditor&div=" + newKey() +
                                    "&loc=2&htmlTag=no','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=yes')";
            ibtnHtmlTagValues.OnClientClick = "window.open('EditContent.aspx?content=TagEditor&htmlTag=" + selected +
                                              "','','width=800, height=500,scrollbars=yes,menubar=no,titlebar=no,resizable=yes')";

            onizle(0, phTepe);
            onizle(1, phGovde);
            onizle(2, phAlt);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void lbSil_Click(object sender, EventArgs e)
    {
        try
        {
            ent = new Entities();
            var lbSil = sender as LinkButton;
            int panelId = Convert.ToInt32(lbSil.CommandArgument);
            var panel = ent.EnrollHtmlPanels.First(p => p.id == panelId);
            var panelValues =
                ent.EnrollHtmlPanelValues.Where(p => p.panelId == panelId).ToList();
            var fi = new System.IO.FileInfo(Server.MapPath("../App_Themes/mainTheme/css/" + panel.divId + ".css"));
            if (fi.Exists) fi.Delete();
            foreach (var pval in panelValues)
            {
                ent.DeleteObject(pval);
            }
            ent.DeleteObject(panel);
            ent.SaveChanges();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        Response.Redirect(Request.RawUrl);
    }

    protected void btnSave_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            ent = new Entities();
            var paneller = ent.EnrollHtmlPanels.ToList();
            var tags = ent.EnrollHtmlTags.ToList();
            var tw = new StreamWriter(Server.MapPath("../App_Themes/mainTheme/css/layout.css"));
            var htw = new HtmlTextWriter(tw);
            foreach (var t in tags)
            {
                tw.WriteLine(t.Value + " {");
                var values = ent.EnrollHtmlTagValues.Where(p => p.htmlTagId == t.id).ToList();
                foreach (var v in values)
                {
                    tw.WriteLine(v.EnrollCssAttributes.cssAttribute + ":" + v.value + ";");
                }
                tw.WriteLine("}");
            }
            foreach (EnrollHtmlPanels p in paneller)
            {
                tw.WriteLine("#" + p.divId + " {");
                List<EnrollHtmlPanelValues> values = ent.EnrollHtmlPanelValues.Where(x => x.panelId == p.id).ToList();
                foreach (EnrollHtmlPanelValues v in values)
                {
                    tw.WriteLine(v.EnrollCssAttributes.cssAttribute + ":" + v.cssAttributeValue + ";");
                }
                tw.WriteLine("}");
            }
            tw.Close();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}