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

public partial class Admin_adminUserControls_UserRoles : UserControl
{
    public List<int> UserActiveAuthAreaIds = new List<int>();
    private readonly Entities _entities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbRoles;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 1))
            {
                MultiView2.ActiveViewIndex = 0;
                MultiView1.ActiveViewIndex = 0;
                Temizle();
            }
            else
            {
                MultiView2.ActiveViewIndex = 1;
            }
        }
        btNewUserRole.Text = AdminResource.lbNewRole;
        CheckBoxDurum.Text = AdminResource.lbActive;

        ImageButtonKaydet.Text = AdminResource.lbSave;
        ImageButtonIptal.Text = AdminResource.lbCancel;

        GridViewRoller.Columns[0].HeaderText = AdminResource.lbActions;
        GridViewRoller.Columns[1].HeaderText = AdminResource.lbRoleName;
        GridViewRoller.Columns[2].HeaderText = AdminResource.lbState;
    }

    private void Temizle()
    {
        TextBoxRolAdi.Text = string.Empty;
        YetkiAlalariniVer(CheckBoxListYetkiAlanlari);
        CheckBoxDurum.Checked = false;
        HiddenFieldId.Value = string.Empty;
    }

    private void YetkiAlalariniVer(CheckBoxList CheckBoxList)
    {
        try
        {
            var user = _entities.Users.FirstOrDefault(p => p.EMail == HttpContext.Current.User.Identity.Name);
            var userRole = _entities.UserRole.FirstOrDefault(p => p.UserId == user.Id);
            if (userRole != null)
            {
                int rolId = userRole.RoleId;
                var roleAuthAreas = _entities.RoleAuthAreas.Where(p => p.RoleId == rolId).ToList();
                if (roleAuthAreas.Count > 0)
                {
                    foreach (var roleAuthArea in roleAuthAreas)
                    {
                        UserActiveAuthAreaIds.Add(roleAuthArea.AuthAreaId);
                    }
                }

                var yaList = from p in _entities.AuthAreas
                             orderby p.AuthArea
                             select p;
                CheckBoxList.Items.Clear();
                foreach (var authArease in yaList)
                {
                    if (!UserActiveAuthAreaIds.Contains(authArease.Id)) continue;
                    var item = new ListItem();
                    item.Value = authArease.Id.ToString();
                    item.Text = Logger.GetModul(authArease.Id, 0, 0).ModulName;
                    CheckBoxList.Items.Add(item);
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "UserRoles:UserActiveAuthAreaIds()");
        }
    }

    protected void ImageButtonYeniEkle_Click(object sender, EventArgs eventArgs)
    {
        Temizle();
        MultiView1.ActiveViewIndex = 1;
    }

    protected void ImageButtonKaydet_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            Roles role;
            if (HiddenFieldId.Value != string.Empty)
            {
                int Id = Convert.ToInt32(HiddenFieldId.Value);
                role = _entities.Roles.First(p => p.Id == Id);
                role.RoleName = TextBoxRolAdi.Text;
                role.State = CheckBoxDurum.Checked;
                role.UpdatedTime = DateTime.Now;
                _entities.SaveChanges();
                Logger.Add(1, 1, Id, 3);
                RolYetkiAlanlariniKaydet(CheckBoxListYetkiAlanlari, role);
                Temizle();
                GridViewRoller.DataBind();
                MultiView1.ActiveViewIndex = 0;
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            else
            {
                role = new Roles();
                role.RoleName = TextBoxRolAdi.Text;
                role.State = CheckBoxDurum.Checked;
                role.CreatedTime = DateTime.Now;
                role.UpdatedTime = DateTime.Now;
                _entities.AddToRoles(role);
                _entities.SaveChanges();
                Logger.Add(1, 1, role.Id, 1);
                RolYetkiAlanlariniKaydet(CheckBoxListYetkiAlanlari, role);
                Temizle();
                GridViewRoller.DataBind();
                MultiView1.ActiveViewIndex = 0;
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
        }
        catch (Exception hata)
        {
            ExceptionManager.ManageException(hata);
        }
    }

    private void RolYetkiAlanlariniKaydet(CheckBoxList yetkiAlanlari, Roles roller)
    {
        var roles = _entities.Roles.First(p => p.RoleName == roller.RoleName && p.State == roller.State);
        List<RoleAuthAreas> ryaList = _entities.RoleAuthAreas.Where(p => p.RoleId == roles.Id).ToList();
        foreach (RoleAuthAreas rya in ryaList)
        {
            _entities.RoleAuthAreas.DeleteObject(rya);
            _entities.SaveChanges();
        }
        foreach (ListItem Li in yetkiAlanlari.Items)
        {
            if (Li.Selected)
            {
                AktifRolKaydet(Convert.ToInt32(Li.Value), roles.Id);
            }
        }
    }

    private void AktifRolKaydet(int YetkiAlaniId, int RolId)
    {
        var rya = new RoleAuthAreas();
        rya.AuthAreaId = YetkiAlaniId;
        rya.RoleId = RolId;
        _entities.AddToRoleAuthAreas(rya);
        _entities.SaveChanges();
    }

    protected void ImageButtonIptal_Click(object sender, EventArgs eventArgs)
    {
        MultiView1.ActiveViewIndex = 0;
        Temizle();
    }

    protected void GridViewRoller_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Guncelle")
        {
            var id = Convert.ToInt32(e.CommandArgument);
            if (!EnrollMembershipHelper.IsAuthForThisProcess(id, EnrollMembershipHelper.GetUserIdFromEmail(HttpContext.Current.User.Identity.Name)))
            {
                MessageBox.Show(MessageType.Warning, AdminResource.msgNoAuth);
                return;
            }

            Roles rol = _entities.Roles.First(p => p.Id == id);
            MultiView1.ActiveViewIndex = 1;
            RolGuncelle(rol);
        }
        else if (e.CommandName == "Sil")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            var roleAuthAreaCount = RoleControl.RoleActiveAuthAreaIds(id);
            var userRoleAuthAreaCount = RoleControl.RoleActiveAuthAreaIds();
            if (!EnrollMembershipHelper.IsAuthForThisProcess(id, EnrollMembershipHelper.GetUserIdFromEmail(HttpContext.Current.User.Identity.Name)))
            {
                MessageBox.Show(MessageType.Warning, AdminResource.msgNoAuth);
                return;
            }
            Roles rol = _entities.Roles.First(p => p.Id == id);
            List<RoleAuthAreas> ryaList = _entities.RoleAuthAreas.Where(p => p.RoleId == rol.Id).ToList();
            foreach (RoleAuthAreas rya in ryaList)
            {
                _entities.RoleAuthAreas.DeleteObject(rya);
                _entities.SaveChanges();
            }
            List<UserRole> userRoleList = _entities.UserRole.Where(p => p.RoleId == rol.Id).ToList();
            foreach (UserRole role in userRoleList)
            {
                _entities.UserRole.DeleteObject(role);
                _entities.SaveChanges();
            }
            _entities.Roles.DeleteObject(rol);
            _entities.SaveChanges();
            Logger.Add(1, 1, id, 2);
            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            GridViewRoller.DataBind();
            MultiView1.ActiveViewIndex = 0;
        }
    }

    private void RolGuncelle(Roles rol)
    {
        Temizle();
        TextBoxRolAdi.Text = rol.RoleName;
        CheckBoxDurum.Checked = rol.State;
        HiddenFieldId.Value = rol.Id.ToString();
        YetkiAlalariniVer(CheckBoxListYetkiAlanlari);
        var ryaList = _entities.RoleAuthAreas.Where(p => p.RoleId == rol.Id).ToList();
         
        foreach (var rya in ryaList)
        { 
            for (var i = 0; i <= CheckBoxListYetkiAlanlari.Items.Count - 1; i++)
            {
                if (CheckBoxListYetkiAlanlari.Items[i].Value == rya.AuthAreaId.ToString())
                { 
                    CheckBoxListYetkiAlanlari.Items[i].Selected = true;
                }
            }
        }
        var index = 0;
        foreach (ListItem listItem in CheckBoxListYetkiAlanlari.Items)
        {
            if (listItem.Selected)
            {
                listItem.Attributes.Remove("onchange");
                listItem.Attributes.Add("id", "cbRole_" + index);
                listItem.Attributes.Add("onchange", "confirmRemoveAuthority('cbRole_" + index + "');");
                index++;
            }
        }
    }

    protected void GridViewRoller_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        var btnDelete = e.Row.FindControl("imgBtnDelete") as ImageButton;
        if (btnDelete != null)
            btnDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
    }
}