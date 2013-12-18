using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Enroll.WebParts;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_MenuManager : UserControl
{
    Entities _entities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        switch (Request.QueryString["location"])
        {
            case "1":
                Session["currentPath"] = AdminResource.lbMainMenuManagement;
                break;
            case "2":
                Session["currentPath"] = AdminResource.lbSubMenuManagement;
                break;
            case "3":
                Session["currentPath"] = AdminResource.lbTopMenuManagement;
                break;
            case "0":
                Session["currentPath"] = AdminResource.lbLeftMenuManagement;
                break;
        }
        RadTreeViewMenuler.ToolTip = AdminResource.lbChangeMenuOrderToolTip;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 2))
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
                hdnLocationId.Value = Request.QueryString["location"];
                pnlEdit.Visible = false;
                //geçici
                LoadTree();
                hdnActiveMenuId.Value = "0";
                MenuleriVer(RadTreeViewMenuler);
                var panel = (Panel)Rtb1.FindControl("ChooseTemplate");
                panel.Visible = true;


                cbState.Text = AdminResource.lbActive;


                try
                {
                    int itemCount = ddlSubMenuType.Items.Count;
                    for (int i = itemCount - 1; i >= 0; i--)
                    {
                        ddlSubMenuType.Items.RemoveAt(i);
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    int itemCount = ddlType.Items.Count;
                    for (int i = itemCount - 1; i >= 0; i--)
                    {
                        ddlType.Items.RemoveAt(i);
                    }
                }
                catch (Exception)
                {
                }

                var item = new ListItem(AdminResource.lbShowSubMenuInDropdownMenu, "0");
                ddlSubMenuType.Items.Add(item);
                item = new ListItem(AdminResource.lbShowSubMenuInPage, "1");
                ddlSubMenuType.Items.Add(item);
                item = new ListItem(AdminResource.lbShowSubMenuInLeft, "2");
                ddlSubMenuType.Items.Add(item);

                item = new ListItem(AdminResource.lbChoose, "");
                ddlType.Items.Add(item);
                item = new ListItem(AdminResource.lbOnlyTitle, "0");
                ddlType.Items.Add(item);
                item = new ListItem(AdminResource.lbUrl, "1");
                ddlType.Items.Add(item);
                item = new ListItem(AdminResource.lbNewPageUrl, "3");
                ddlType.Items.Add(item);
                item = new ListItem(AdminResource.lbPage, "2");
                ddlType.Items.Add(item);

                chkSetStartPage.Text = AdminResource.lbStartPage;
                chkisHideName.Text = AdminResource.lbHideMenuTitle;
                chkisHideToMenu.Text = AdminResource.lbHideInMenu;

                BtnUpdateMenu.Text = AdminResource.lbSave;
                BtnCancelUpdateMenu.Text = AdminResource.lbCancel;

                BtnAddNewItem.Text = AdminResource.lbNewPage;
                BtnDeleteItem.Text = AdminResource.lbDelete;

                imgBtnMenuImageSelect.Text = AdminResource.lbImageSelect;
                imgBtnMenuImageHover.Text = AdminResource.lbImageSelect;
                imgBtnImageSelect.Text = AdminResource.lbImageSelect;

                
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        
        
        ddlThemas.Items.Clear();
        ddlThemas.DataBind();
        ddlThemas.Items.Insert(0, new ListItem(AdminResource.lbDefault, ""));
        
        BtnDeleteItem.OnClientClick = "return confirm('" + AdminResource.comfirmDeleteMenu + "');";

        btnShowDynamicFieldManager.Text = AdminResource.lbDynamicFields;

        btnAdd.Text = AdminResource.lbAdd;
        btnRemove.Text = AdminResource.lbDelete;
        btSave.Text = AdminResource.lbSave;
        btCancel.Text = AdminResource.lbCancel;
    }

    private void MenuleriVer(RadTreeView radTreeView)
    {
        try
        {
            var locationId = Convert.ToInt32(Request.QueryString["location"]);

            var menuler = (from p in _entities.System_menu
                           where
                               p.location == locationId &&
                               p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId
                           orderby p.menuIndex ascending
                           select new
                                      {
                                          p.name,
                                          p.menuId,
                                          MasterId = p.MasterId == 0 ? null : p.MasterId
                                      }).ToList();

            radTreeView.DataTextField = "name";
            radTreeView.DataFieldParentID = "MasterId";
            radTreeView.DataFieldID = "menuId";
            radTreeView.DataValueField = "menuId";
            radTreeView.DataSource = menuler;
            radTreeView.DataBind();

            radTreeView.Nodes.Insert(0, new RadTreeNode(AdminResource.lbRoot, "0"));

            radTreeView.CollapseAllNodes();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void RadTreeViewMenulerNodeClick(object sender, RadTreeNodeEventArgs e)
    {
        try
        {
            pnlEdit.Visible = true;
            if (Convert.ToInt32(RadTreeViewMenuler.SelectedNode.Value) != 0)
            {
                hdnActiveMenuId.Value = RadTreeViewMenuler.SelectedNode.Value;
                if (RadTreeViewMenuler.SelectedNode.Parent != null)
                {
                    if (RadTreeViewMenuler.SelectedNode.Parent.ToString() != "Enroll_RootMenuItem")
                        hdnParentId.Value = RadTreeViewMenuler.SelectedNode.Parent.ToString();
                    else
                        hdnParentId.Value = "0";
                }
                else
                {
                    hdnParentId.Value = "0";
                }
                UpdateMode();
            }
            else
            {
                hdnActiveMenuId.Value = "0";
                ResetEditPanel();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void RadTreeViewMenulerNodeDrop(object sender, RadTreeNodeDragDropEventArgs e)
    {
        try
        {
            foreach (var node in e.DraggedNodes)
            {
                var menuKaynakId = Convert.ToInt32(node.Value);
                var menuHedefId = Convert.ToInt32(e.DestDragNode.Value);
                System_menu menuHedef = (menuHedefId != 0 ? _entities.System_menu.First(p => p.menuId == menuHedefId) : null);
                System_menu menuKaynak = (menuKaynakId != 0 ? _entities.System_menu.First(p => p.menuId == menuKaynakId) : null);

                if (menuKaynak != null)
                {
                    var menuHedefUstMenuId = (menuHedef != null ? menuHedef.MasterId : 0);
                    var menuKaynakUstMenuId = menuKaynak.MasterId;
                    RadTreeViewDropPosition dropPosition = e.DropPosition;

                    if (menuHedef != null)
                    {
                        #region aynı dizin

                        if (menuHedefUstMenuId == menuKaynakUstMenuId)
                        {
                            if (dropPosition == RadTreeViewDropPosition.Over)
                            {
                                UpdateInMasterNode(0, menuKaynak, menuHedef);
                            }
                            else if (menuHedef.menuIndex > menuKaynak.menuIndex && dropPosition == RadTreeViewDropPosition.Above)
                            {
                                menuHedefId = Convert.ToInt32(RadTreeViewMenuler.Nodes[e.DestDragNode.Index - 1].Value);
                                menuHedef = (menuHedefId != 0 ? _entities.System_menu.First(p => p.menuId == menuHedefId) : null);
                                UpdateInMasterNode(1, menuKaynak, menuHedef);
                            }
                            else if (menuHedef.menuIndex > menuKaynak.menuIndex && dropPosition == RadTreeViewDropPosition.Below)
                            {
                                UpdateInMasterNode(1, menuKaynak, menuHedef);
                            }
                            else if (menuHedef.menuIndex < menuKaynak.menuIndex && dropPosition == RadTreeViewDropPosition.Above)
                            {
                                UpdateInMasterNode(1, menuKaynak, menuHedef);
                            }
                            else if (menuHedef.menuIndex < menuKaynak.menuIndex && dropPosition == RadTreeViewDropPosition.Below)
                            {
                                menuHedefId = Convert.ToInt32(RadTreeViewMenuler.Nodes[e.DestDragNode.Index + 1].Value);
                                menuHedef = (menuHedefId != 0 ? _entities.System_menu.First(p => p.menuId == menuHedefId) : null);
                                UpdateInMasterNode(1, menuKaynak, menuHedef);
                            }
                        }
                        #endregion
                        #region farklı dizin

                        else
                        {
                            if (dropPosition == RadTreeViewDropPosition.Over)
                            {
                                UpdateOutMasterNode(0, menuKaynak, menuHedef);
                            }
                            else if (dropPosition == RadTreeViewDropPosition.Above)
                            {
                                UpdateOutMasterNode(1, menuKaynak, menuHedef);
                            }
                            else if (dropPosition == RadTreeViewDropPosition.Below)
                            {
                                UpdateOutMasterNode(2, menuKaynak, menuHedef);
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region root dizin
                        UpdateOverFirstNode(menuKaynak);
                        #endregion
                    }

                    MenuleriVer(RadTreeViewMenuler);
                }

            }
            MenuleriVer(RadTreeViewMenuler);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    //  aynı dizin
    public void UpdateInMasterNode(int type, System_menu sourceMenu, System_menu destMenu)
    {
        var itemIndexChanged = false;
        var languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
        var lokasyonId = Convert.ToInt32(Request.QueryString["location"]);

        var menuHedefUstMenuId = (destMenu != null ? destMenu.MasterId : 0);
        var menuHedefSiraNo = (destMenu != null ? destMenu.menuIndex : 0);
        var menuKaynakUstMenuId = sourceMenu.MasterId;
        var menuKaynakSiraNo = sourceMenu.menuIndex;

        List<System_menu> menuList;

        switch (type)
        {
            case 0: // over

                #region eski menü elemanların sıralarını düzenle
                menuList = _entities.System_menu.Where(p =>
                    p.MasterId == menuKaynakUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex > menuKaynakSiraNo).ToList();
                foreach (System_menu item in menuList)
                {
                    item.menuIndex -= 1;
                    _entities.SaveChanges();
                }
                #endregion
                var count = _entities.System_menu.Count(p => p.MasterId == destMenu.menuId &&
                                                                 p.languageId == languageId &&
                                                                 p.location == lokasyonId);
                if (destMenu != null) sourceMenu.MasterId = destMenu.menuId;
                sourceMenu.menuIndex = (count + 1);
                _entities.SaveChanges();
                break;
            case 1: // above or below

                if (menuHedefSiraNo > menuKaynakSiraNo)
                {
                    menuList = _entities.System_menu.Where(p => p.MasterId == menuHedefUstMenuId && p.languageId == languageId &&
                        p.location == lokasyonId && p.menuIndex > menuKaynakSiraNo && p.menuIndex <= menuHedefSiraNo).ToList();

                    foreach (var item in menuList)
                    {
                        item.menuIndex -= 1;
                        _entities.SaveChanges();
                    }
                    itemIndexChanged = true;
                }
                else if (menuHedefSiraNo < menuKaynakSiraNo)
                {
                    menuList = _entities.System_menu.Where(p =>
                    p.MasterId == menuHedefUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex < menuKaynakSiraNo &&
                    p.menuIndex >= menuHedefSiraNo
                    ).ToList();

                    foreach (System_menu item in menuList)
                    {
                        item.menuIndex += 1;
                        _entities.SaveChanges();
                    }
                    itemIndexChanged = true;
                }
                if (itemIndexChanged)
                {
                    sourceMenu.menuIndex = menuHedefSiraNo;
                    _entities.SaveChanges();
                }
                break;
        }
    }

    // farklı dizin
    public void UpdateOutMasterNode(int type, System_menu sourceMenu, System_menu destMenu)
    {
        var languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
        var lokasyonId = Convert.ToInt32(Request.QueryString["location"]);

        var menuHedefUstMenuId = (destMenu != null ? destMenu.MasterId : 0);
        var menuHedefSiraNo = (destMenu != null ? destMenu.menuIndex : 0);
        var menuKaynakUstMenuId = sourceMenu.MasterId;
        var menuKaynakSiraNo = sourceMenu.menuIndex;

        List<System_menu> menuList;

        switch (type)
        {
            case 0: // over

                #region diğer elemanların sıralarını düzenle
                menuList = _entities.System_menu.Where(p =>
                    p.MasterId == menuKaynakUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex > menuKaynakSiraNo).ToList();
                foreach (System_menu item in menuList)
                {
                    item.menuIndex -= 1;
                    _entities.SaveChanges();
                }
                #endregion
                int itemCount = _entities.System_menu.Count(p => p.MasterId == destMenu.menuId &&
                                                                 p.languageId == languageId &&
                                                                 p.location == lokasyonId);
                if (destMenu != null)
                {
                    sourceMenu.MasterId = destMenu.menuId;
                    sourceMenu.menuIndex = (itemCount + 1);
                    _entities.SaveChanges();
                }

                break;
            case 1: // above  
                #region eski menüdeki diğer elemanların sıralarını düzenle
                menuList = _entities.System_menu.Where(p =>
                    p.MasterId == menuKaynakUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex > menuKaynakSiraNo).ToList();
                foreach (System_menu item in menuList)
                {
                    item.menuIndex -= 1;
                    _entities.SaveChanges();
                }
                #endregion
                #region yeni menüdeki diğer elemanların sıralarını düzenle
                menuList = _entities.System_menu.Where(p =>
                    p.MasterId == menuHedefUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex >= menuHedefSiraNo).ToList();
                foreach (System_menu item in menuList)
                {
                    item.menuIndex += 1;
                    _entities.SaveChanges();
                }
                #endregion
                var count = _entities.System_menu.Count(p => p.MasterId == menuHedefUstMenuId &&
                                                             p.languageId == languageId &&
                                                             p.location == lokasyonId &&
                                                             p.menuIndex < menuHedefSiraNo);
                if (destMenu != null)
                {
                    sourceMenu.MasterId = destMenu.MasterId;
                    sourceMenu.menuIndex = (count + 1);
                    _entities.SaveChanges();
                }

                break;
            case 2: // belowk 
                #region eski menüdeki diğer elemanların sıralarını düzenle
                menuList = _entities.System_menu.Where(p =>
                    p.MasterId == menuKaynakUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex > menuKaynakSiraNo).ToList();
                foreach (System_menu item in menuList)
                {
                    item.menuIndex -= 1;
                    _entities.SaveChanges();
                }
                #region yeni menüdeki diğer elemanların sıralarını düzenle
                count = _entities.System_menu.Where(p =>
                    p.MasterId == menuHedefUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex <= menuHedefSiraNo).Count();
                if (destMenu != null)
                {
                    sourceMenu.MasterId = destMenu.MasterId;
                    sourceMenu.menuIndex = (count + 1);
                    _entities.SaveChanges();
                }
                #endregion
                #endregion
                #region yeni menüdeki diğer elemanların sıralarını düzenle
                menuList = _entities.System_menu.Where(p =>
                    p.MasterId == menuHedefUstMenuId &&
                    p.languageId == languageId &&
                    p.location == lokasyonId &&
                    p.menuIndex > menuHedefSiraNo).ToList();
                foreach (System_menu item in menuList)
                {
                    item.menuIndex += 1;
                    _entities.SaveChanges();
                }
                #endregion
                count = _entities.System_menu.Count(p => p.MasterId == menuHedefUstMenuId &&
                                                         p.languageId == languageId &&
                                                         p.location == lokasyonId &&
                                                         p.menuIndex <= menuHedefSiraNo);
                if (destMenu != null)
                {
                    sourceMenu.MasterId = destMenu.MasterId;
                    sourceMenu.menuIndex = (count + 1);
                    _entities.SaveChanges();
                }
                break;
        }
    }

    // root dizin
    public void UpdateOverFirstNode(System_menu sourceMenu)
    {
        var languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
        var lokasyonId = Convert.ToInt32(Request.QueryString["location"]);

        var menuKaynakUstMenuId = sourceMenu.MasterId;
        var menuKaynakSiraNo = sourceMenu.menuIndex;

        List<System_menu> menuList;
        #region eski menüdeki diğer elemanların sıralarını düzenle
        menuList = _entities.System_menu.Where(p =>
            p.MasterId == menuKaynakUstMenuId &&
            p.languageId == languageId &&
            p.location == lokasyonId &&
            p.menuIndex > menuKaynakSiraNo).ToList();
        foreach (System_menu item in menuList)
        {
            item.menuIndex -= 1;
            _entities.SaveChanges();
        }
        #endregion

        var count = _entities.System_menu.Count(p => p.MasterId == 0 && p.languageId == languageId && p.location == lokasyonId && p.menuId != sourceMenu.menuId);

        sourceMenu.menuIndex = count + 1;
        sourceMenu.MasterId = 0;
        _entities.SaveChanges();
    }

    private void LoadTree()
    { 
        var node = new TreeNode("Kök Menü", "Enroll_RootMenuItem"); 
        GenerateTreenode(0, node.ChildNodes);
        hdnParentId.Value = "0"; 
    }

    private void ResetEditPanel()
    {
        var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
        txtName.Text = "";
        txtKeys.Text = "";
        txtUrl.Text = "";
        radEditor.Content = "";
        ddlType.SelectedIndex = 0;
        txtHeader.Text = "";
        MultiView1.ActiveViewIndex = -1;
        pnlEdit.Visible = false;
        txtImage.Text = "";
        chkSetStartPage.Checked = false;
        txtKeys.Text = "";
        txtImage.Text = "";
        BtnDeleteItem.Visible = false;
    }

    private void GenerateTreenode(Int32 masterId, TreeNodeCollection nodes)
    {
        try
        {
            List<System_menu> oList;
            oList = GetChildNodeList(masterId);
            foreach (System_menu menu in oList)
            {
                var oNode = new ImageTreeViewNode();
                oNode.Text = menu.name;
                oNode.Value = menu.menuId.ToString();
                nodes.Add(oNode);
                GenerateTreenode(Convert.ToInt32(menu.menuId), oNode.ChildNodes);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(MessageType.Success, ex.Message + " - " + ex.Source + " - " + ex.StackTrace);
        }
    }

    private List<System_menu> GetChildNodeList(Int32 masterId)
    {
        List<System_menu> oList =
            _entities.System_menu.Where("it.MasterId=" + masterId.ToString() + " and it.languageId=" +
                                      EnrollAdminContext.Current.DataLanguage.LanguageId.ToString() +
                                      " and it.location=" +
                                      hdnLocationId.Value + " order by it.menuIndex").ToList();
        return oList;
    }

    private void UpdateMode()
    {
        try
        {
            ResetEditPanel();
            hdnEditMode.Value = "U";
            pnlEdit.Visible = true;
            BtnDeleteItem.Visible = true;
            var oEntities = new Entities();
            var oMenu = oEntities.System_menu.Where("it.menuId=" + hdnActiveMenuId.Value).FirstOrDefault();
            GetDataProccess(oMenu, oEntities);
            var oDataLanguage =
                oEntities.System_language.Where("it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId)
                    .FirstOrDefault();
            if (oDataLanguage.startupMenuId == oMenu.menuId)
            {
                chkSetStartPage.Checked = true;
            }
            ddlThemas.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(oMenu.thema.ToString()))
            {
                
                if (ddlThemas.Items.Count > 0)
                {
                    var i = 0;
                    foreach (ListItem item in ddlThemas.Items)
                    {
                        if (oMenu.thema!=null && item.Value == oMenu.thema.ToString())
                        {
                            ddlThemas.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                }
            }
            btnShowDynamicFieldManager.Visible = true;

        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void InsertMode()
    {
        ResetEditPanel();
        hdnEditMode.Value = "I";
        pnlEdit.Visible = true;
        btnShowDynamicFieldManager.Visible = false;
    }

    #region AddNew or Delete item

    protected void BtnAddNewItemClick(object sender, EventArgs eventArgs)
    {
        InsertMode();
        txtMenuImage.Text = "";
        txtMenuImageHover.Text = "";
    }
    protected void BtnDeleteItemClick(object sender, EventArgs eventArgs)
    {
        try
        {
            if (hdnActiveMenuId.Value != "0")
            {
                System_menu oMenu = _entities.System_menu.Where("it.menuId=" + hdnActiveMenuId.Value).First();

                // menülere bağlı dynamic alanların referanslarını siliyoruz
                var dynamicReferenceList = _entities.Customer_Dynamic_Reference.Where(p => p.MenuId == oMenu.menuId).ToList();
                foreach (var customerDynamicReference in dynamicReferenceList)
                {
                    _entities.DeleteObject(customerDynamicReference);
                    _entities.SaveChanges();
                }

                int control = _entities.System_menu.Where("it.MasterId=" + hdnActiveMenuId.Value).Count();
                if (control > 0)
                {
                    List<System_menu> altmenu =
                        _entities.System_menu.Where("it.MasterId=" + hdnActiveMenuId.Value).ToList();
                    foreach (System_menu i in altmenu)
                    {
                        // alt menülere bağlı dynamic alanların referanslarını siliyoruz
                        var subMenuDynamicReferenceList = _entities.Customer_Dynamic_Reference.Where(p => p.MenuId == i.menuId).ToList();
                        foreach (var subMenuDynamicReferance in subMenuDynamicReferenceList)
                        {
                            _entities.DeleteObject(subMenuDynamicReferance);
                            _entities.SaveChanges();
                        }

                        _entities.DeleteObject(i);
                    }
                }
                _entities.DeleteObject(oMenu);
                _entities.SaveChanges();

                Logger.Add(2, 0, oMenu.menuId, 2);

                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);

                //geçici
                LoadTree();
                hdnActiveMenuId.Value = "0";
                MenuleriVer(RadTreeViewMenuler);

                ResetEditPanel();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    #endregion

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlType.SelectedValue)
        {
            case "0":
                MultiView1.ActiveViewIndex = -1;
                BtnUpdateMenu.Enabled = true;
                break;
            case "1":
                MultiView1.SetActiveView(View1);
                BtnUpdateMenu.Enabled = true;
                break;
            case "2":
                MultiView1.SetActiveView(View2);
                BtnUpdateMenu.Enabled = true;
                break;
            case "Seçiniz":
                MultiView1.ActiveViewIndex = -1;
                btnShowDynamicFieldManager.Visible = true;
                //BtnUpdateMenu.Enabled = false;
                break;
            case "3":
                MultiView1.SetActiveView(View1);
                BtnUpdateMenu.Enabled = true;
                break;
        }
    }

    private void GetDataProccess(System_menu menu, Entities entities)
    {
        try
        {
            txtName.Text = menu.name;
            ddlSubMenuType.SelectedValue = menu.subMenuShowType.ToString();
            cbState.Checked = menu.state.Value;
            txtMenuImage.Text = menu.menuImage;
            txtMenuImageHover.Text = menu.menuImageHover;
            switch (menu.type)
            {
                case "0":
                    ddlType.SelectedValue = "0";
                    break;
                case "1":
                    ddlType.SelectedValue = "1";
                    txtUrl.Text = menu.url;
                    MultiView1.ActiveViewIndex = 0;
                    break;
                case "2":
                    chkisHideToMenu.Checked = Convert.ToBoolean(menu.isHideToMenu);
                    chkisHideName.Checked = Convert.ToBoolean(menu.isHideMenuName);
                    ddlType.SelectedValue = "2";
                    MultiView1.ActiveViewIndex = 1;
                    txtHeader.Text = menu.brief;
                    var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
                    radEditor.Content = menu.Details;
                    txtKeys.Text = menu.keywords;
                    txtImage.Text = menu.masterImage;
                    hdnMenuThema.Value = menu.thema.ToString();
                    ddlThemas.SelectedIndex = 0;
                    if (ddlThemas.Items.Count > 0)
                    {
                        var i = 0;
                        foreach (ListItem item in ddlThemas.Items)
                        {
                            if (menu.thema != null && item.Value == menu.thema.ToString())
                            {
                                ddlThemas.SelectedIndex = i;
                                break;
                            }
                            i++;
                        }
                    }
                    break;
                case "3":
                    ddlType.SelectedValue = "3";
                    txtUrl.Text = menu.url;
                    MultiView1.ActiveViewIndex = 0;
                    break;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void SetDataProccess(System_menu menu)
    {
        try
        {
            menu.name = txtName.Text;
            menu.subMenuShowType = Convert.ToInt32(ddlSubMenuType.SelectedValue);
            menu.isHideToMenu = false;
            menu.isHideMenuName = false;
            menu.menuImage = txtMenuImage.Text;
            menu.menuImageHover = txtMenuImageHover.Text;
            menu.state = cbState.Checked;
            menu.UpdatedTime = DateTime.Now;
            switch (ddlType.SelectedValue)
            {
                case "0": //yanlızca başlık
                    menu.type = "0";
                    break;
                case "1": //url
                    menu.type = "1";
                    menu.url = txtUrl.Text;
                    break;
                case "2": //sayfa
                    var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
                    menu.type = "2";
                    menu.brief = txtHeader.Text;
                    menu.Details = radEditor.Content;
                    menu.keywords = txtKeys.Text;
                    menu.masterImage = txtImage.Text;
                    if (ddlThemas.SelectedIndex > 0) menu.thema = Convert.ToInt32(ddlThemas.SelectedValue);
                    menu.isHideToMenu = chkisHideToMenu.Checked;
                    menu.isHideMenuName = chkisHideName.Checked; 
                    break;
                case "3": // site dışı url
                    menu.type = "3";
                    menu.url = txtUrl.Text;
                    break;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private Int32 GetIndex(Entities entities)
    {
        String masterId;
        try
        {
            if (hdnParentId.Value == "Enroll_RootMenuItem")
                masterId = "0";
            else
                masterId = hdnActiveMenuId.Value;

            var omenu =
                entities.System_menu.Where("it.masterId=" + masterId + "and it.location=" + hdnLocationId.Value +
                                           "and it.languageId=" +
                                           EnrollAdminContext.Current.DataLanguage.LanguageId.ToString() +
                                           " order by it.menuIndex desc").FirstOrDefault();

            if (omenu != null)
                return Convert.ToInt32(omenu.menuIndex + 1);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return 1;
    }

    #region dynamic group management

    protected void btnShowDynamicFieldManagerClick(object sender, EventArgs e)
    {
        GetDynamicFieldDatas();
        pnlDynamicField.Visible = true;
        pnlMenu.Visible = false;

    }

    protected void btEditCancelDynamicMenu_Click(object sender, EventArgs e)
    {
        pnlDynamicField.Visible = false;
        pnlMenu.Visible = true;

    }

    private void GetDynamicFieldDatas()
    {
        int itemsCount = ListBoxRef.Items.Count;
        for (int i = 0; i < itemsCount; i++)
        {
            ListBoxRef.Items.RemoveAt(0);
        }

        int menuId = Convert.ToInt32(hdnActiveMenuId.Value);
        var entities = new Entities();
        var oMenu = entities.System_menu.FirstOrDefault(p => p.menuId == menuId);

        if (oMenu != null)
        {
            chkDyna.Checked = Convert.ToBoolean(oMenu.isShowDyna);

            txtDynaDisplay.Text = oMenu.dynaDisplayText;

            if (oMenu.dynaDisplayType != 0 & oMenu.dynaDisplayType != null)
                ddlDynaDisType.SelectedValue = oMenu.dynaDisplayType.ToString();

            foreach (var refe in entities.Customer_Dynamic_Reference.Where(p => p.MenuId == oMenu.menuId))
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
                        var oDynamic = _entities.Customer_Dynamic.FirstOrDefault(
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
            if (ddlDynamicGroup.SelectedValue != AdminResource.lbChoose)
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
            var oList = (DropDownList)sender;
            oList.Items.Insert(0, new ListItem(AdminResource.lbChoose));
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btSaveDynamicMenu_Click(object sender, EventArgs e)
    {
        try
        {
            int menuId = Convert.ToInt32(hdnActiveMenuId.Value);
            var oMenu = _entities.System_menu.FirstOrDefault(p => p.menuId == menuId);
            if (oMenu != null)
            {
                oMenu.isShowDyna = chkDyna.Checked;
                oMenu.dynaDisplayText = txtDynaDisplay.Text;
                oMenu.dynaDisplayType = Convert.ToInt32(ddlDynaDisType.SelectedValue);
                UpdateRefData(oMenu, _entities);
            }
            Logger.Add(16, 4, menuId, 3);
            _entities.SaveChanges();

            pnlDynamicField.Visible = false;
            pnlMenu.Visible = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }


    }

    #endregion

    #region SaveUpdate, CancelUpdate Menu

    protected void BtnUpdateMenuClick(object sender, EventArgs eventArgs)
    {
        try
        {
            System_menu oMenu;
            if (hdnEditMode.Value == "I")
            {
                oMenu = new System_menu();
                oMenu.MasterId = Convert.ToInt32(hdnActiveMenuId.Value);
                oMenu.menuIndex = Convert.ToInt32(GetIndex(_entities));
                oMenu.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
                oMenu.location = Convert.ToInt32(hdnLocationId.Value);
                oMenu.CreatedTime = DateTime.Now;
                oMenu.UpdatedTime = DateTime.Now;
                 
                _entities.AddToSystem_menu(oMenu);  

                SetDataProccess(oMenu);
                Logger.Add(2, 0, oMenu.menuId, 1);
            }
            else
            {
                oMenu = _entities.System_menu.Where("it.menuId=" + hdnActiveMenuId.Value).FirstOrDefault();
                SetDataProccess(oMenu);
                _entities.SaveChanges();
                if (oMenu != null) Logger.Add(2, 0, oMenu.menuId, 3);
            } 
            _entities.SaveChanges();
             
            //başlangıç sayfası ayarları
            if (ddlType.SelectedValue == "2")
            {
                System_language oSystemLanguage =
                    _entities.System_language.Where("it.languageId=" +
                                                    EnrollAdminContext.Current.DataLanguage.LanguageId)
                        .FirstOrDefault();

                if (oMenu != null && oSystemLanguage != null && chkSetStartPage.Checked)
                {
                    oSystemLanguage.startupMenuId = Convert.ToInt32(oMenu.menuId);
                }
                else if (oSystemLanguage != null && (oMenu != null && oMenu.menuId == Convert.ToDecimal(oSystemLanguage.startupMenuId)))
                {
                    oSystemLanguage.startupMenuId = 0; //hiç bir menu set edilmez.
                }
            }
            _entities.SaveChanges();
            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            LoadTree();
            btnShowDynamicFieldManager.Visible = true;
            ResetEditPanel();
            MenuleriVer(RadTreeViewMenuler);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnCancelUpdateMenuClick(object sender, EventArgs eventArgs)
    {
        ResetEditPanel();
    }

    #endregion
     
}