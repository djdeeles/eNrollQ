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

public partial class Admin_adminUserControls_Users : UserControl
{
    private readonly Entities _entities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbUsers;
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

        btNewUser.Text = AdminResource.lbNewUser;
        CheckBoxDurum.Text = AdminResource.lbActive;
        btnKaydet.Text = AdminResource.lbSave;
        btnIptal.Text = AdminResource.lbCancel;

        GridViewKullanicilar.Columns[0].HeaderText = AdminResource.lbActions;
        GridViewKullanicilar.Columns[1].HeaderText = AdminResource.lbEmail;
        GridViewKullanicilar.Columns[2].HeaderText = AdminResource.lbName;
        GridViewKullanicilar.Columns[3].HeaderText = AdminResource.lbSurname;
        GridViewKullanicilar.Columns[4].HeaderText = AdminResource.lbState;

        lbGeneratePwd.Text = AdminResource.lbGeneratePassword;
    }

    private void Temizle()
    {
        TextBoxEPosta.Text = string.Empty;
        TextBoxParola.Text = string.Empty;
        tbName.Text = string.Empty;
        tbSurname.Text = string.Empty;
        CheckBoxDurum.Checked = false;
    }
     
    public List<int> UserActiveAuthAreaIds()
    {
        var list = new List<int>();
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
                        list.Add(roleAuthArea.AuthAreaId);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "Users:UserActiveAuthAreaIds()");
        }

        return list;
    }

    private void RolleriVer(CheckBoxList CheckBoxList)
    {
        var roles = new List<Roles>();
        var activeUserAuthAreaCount = UserActiveAuthAreaIds().Count;
        
        var roller = from p in _entities.Roles
                     orderby p.Id
                     select p;
        var i = 0;
        foreach (var item in roller)
        {
            var roleAuthAreaCount = RoleControl.RoleActiveAuthAreaIds(item.Id).Count;
            if(activeUserAuthAreaCount >= roleAuthAreaCount)
            {
                roles.Add(item);
            }
            i++;
        }

        if (roller.Count() != 0)
        { 
            CheckBoxList.DataTextField = "RoleName";
            CheckBoxList.DataValueField = "Id";
            CheckBoxList.DataSource = roles; 
            CheckBoxList.DataBind();
        }

    }
    
    protected void ImageButtonYeniEkle_Click(object sender, EventArgs eventArgs)
    {
        hfUserId.Value = null;
        MultiView1.ActiveViewIndex = 1;
        Temizle();
        RolleriVer(CheckBoxListRoller);
    }

    protected void ImageButtonKaydet_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            Users newUser;
            if (hfUserId.Value != string.Empty)
            {
                var kullaniciId = Convert.ToInt32(hfUserId.Value);

                //aynı mail adresi olan kullanıcı var mı? kontrolü. yoksa o mail adresi ile kullanıcı eklenir.
                var k = _entities.UserEmails.FirstOrDefault(p => p.UserId != kullaniciId && p.Email == TextBoxEPosta.Text);
                //var k = _entities.Users.FirstOrDefault(p => p.Id != kullaniciId && p.EMail == TextBoxEPosta.Text);

                if (k == null)
                {
                    newUser = _entities.Users.FirstOrDefault(p => p.Id == kullaniciId);
                    if (newUser != null)
                    {
                        newUser.EMail = TextBoxEPosta.Text;
                        if (TextBoxParola.Text != string.Empty)
                            newUser.Password = Crypto.Encrypt(TextBoxParola.Text);
                        newUser.State = CheckBoxDurum.Checked;
                        newUser.Name = tbName.Text;
                        newUser.Surname = tbSurname.Text;
                        newUser.UpdatedTime = DateTime.Now;
                        _entities.SaveChanges();
                        Logger.Add(1, 2, newUser.Id, 3);
                        AktifRolleriKaydet(CheckBoxListRoller, newUser);
                        Temizle();
                        RolleriVer(CheckBoxListRoller);
                        GridViewKullanicilar.DataBind();
                        MultiView1.ActiveViewIndex = 0;
                        MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                    }
                }
            }
            else
            {
                /// yeni kullanıcı ekleme
                string email = TextBoxEPosta.Text;
                var users = _entities.Users.Where(p => p.EMail == email).ToList();
                var emails = _entities.UserEmails.Where(p => p.Email == email).ToList();
                if (users.Count == 0 && emails.Count == 0 && TextBoxParola.Text != string.Empty)
                {
                    newUser = new Users();
                    newUser.EMail = TextBoxEPosta.Text;
                    newUser.State = CheckBoxDurum.Checked;
                    newUser.Name = tbName.Text;
                    newUser.Surname = tbSurname.Text;
                    newUser.Password = Crypto.Encrypt(TextBoxParola.Text);
                    newUser.Admin = true;
                    newUser.CreatedTime = DateTime.Now;
                    newUser.UpdatedTime = DateTime.Now;
                    _entities.AddToUsers(newUser);
                    _entities.SaveChanges();
                      
                    var userEmails = new UserEmails();
                    userEmails.Email = newUser.EMail;
                    userEmails.UserId = newUser.Id;
                    userEmails.MainAddress = true;
                    userEmails.Activated= true;
                    _entities.AddToUserEmails(userEmails);
                    _entities.SaveChanges();

                    var userFoundation = new UserFoundation();
                    userFoundation.MemberState = true; 
                    userFoundation.UserId = newUser.Id;  
                    _entities.AddToUserFoundation(userFoundation);
                    _entities.SaveChanges();

                    var userGeneral = new UserGeneral();
                    userGeneral.UserId = newUser.Id;
                    _entities.AddToUserGeneral(userGeneral);
                    _entities.SaveChanges();

                    var userFinance = new UserFinance();
                    userFinance.UserId = newUser.Id;
                    _entities.AddToUserFinance(userFinance);
                    _entities.SaveChanges();

                    Logger.Add(1, 2, newUser.Id, 1);
                    AktifRolleriKaydet(CheckBoxListRoller, newUser);
                    Temizle();
                    RolleriVer(CheckBoxListRoller);

                    GridViewKullanicilar.DataBind();
                    MultiView1.ActiveViewIndex = 0;
                    MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                }
                else if (TextBoxParola.Text == string.Empty)
                {
                    MessageBox.Show(MessageType.Error, AdminResource.lbError);
                }
            }
        }
        catch (Exception hata)
        {
            ExceptionManager.ManageException(hata);
        }
    }

    private void AktifRolleriKaydet(CheckBoxList roller, Users kullanici)
    {
        List<UserRole> kRoller = _entities.UserRole.Where(p => p.UserId == kullanici.Id).ToList();
        foreach (var rol in kRoller)
        {
            var krr = _entities.UserRole.First(p => p.UserId == rol.UserId);
            _entities.DeleteObject(krr);
            _entities.SaveChanges();
        }
        foreach (ListItem Li in roller.Items)
        {
            if (Li.Selected)
            {
                AktifRolKaydet(Convert.ToInt32(Li.Value), kullanici.Id);
            }
        }
    }

    private void AktifRolKaydet(int RolId, int KullaniciId)
    {
        var role = new UserRole();
        role.UserId = KullaniciId;
        role.RoleId = RolId;
        _entities.AddToUserRole(role);
        _entities.SaveChanges();
    }

    protected void ImageButtonIptal_Click(object sender, EventArgs eventArgs)
    {
        MultiView1.ActiveViewIndex = 0;
        Temizle();
    }

    public bool IsAuthForThisProcess(int userId)
    {
        var roleAuthAreaCount = UserActiveAuthAreaIds(userId);
        var activeUserRoleAuthAreaCount = UserActiveAuthAreaIds();
        if (activeUserRoleAuthAreaCount.Count < roleAuthAreaCount.Count)
        {
            return false;
        }
        return true;
    }

    // kullanıcı yetki alanı Id leri
    public List<int> UserActiveAuthAreaIds(int userId)
    {
        var list = new List<int>();
        try
        {
            var userRole = _entities.UserRole.FirstOrDefault(p => p.UserId == userId);
            if (userRole != null)
            {
                int rolId = userRole.RoleId;
                list = _entities.RoleAuthAreas.Where(p => p.RoleId == rolId).Select(p => p.AuthAreaId).ToList();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception, "Users:UserActiveAuthAreaIds(int userId)");
        }
        return list;
    }

    protected void GridViewKullanicilar_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        var entities = new Entities();
        if (e.CommandName == "Guncelle")
        {
            try
            {
                int id = Convert.ToInt32(e.CommandArgument);
                if (!EnrollMembershipHelper.IsAuthForThisProcess(id, EnrollMembershipHelper.GetUserIdFromEmail(HttpContext.Current.User.Identity.Name)))
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.msgNoAuth);
                    return;
                }
                var kullanici = _entities.Users.First(p => p.Id == id);
                KullaniciGuncelle(kullanici);
                MultiView1.ActiveViewIndex = 1;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        else if (e.CommandName == "Sil")
        {
            try
            {
                var id = Convert.ToInt32(e.CommandArgument);
                if (!EnrollMembershipHelper.IsAuthForThisProcess(id, EnrollMembershipHelper.GetUserIdFromEmail(HttpContext.Current.User.Identity.Name)) ||
                    EnrollMembershipHelper.AreYouActiveUser(id))
                {
                    MessageBox.Show(MessageType.Warning, AdminResource.msgNoAuth);
                    return;
                }

                var kullanici = entities.Users.FirstOrDefault(p => p.Id == id);
                if (kullanici!=null && KullanicininBilgileriniSil(kullanici.Id, entities))
                { 
                    entities.Users.DeleteObject(kullanici);
                    entities.SaveChanges();
                    Logger.Add(1, 2, id, 2);
                    MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);

                    GridViewKullanicilar.DataBind();
                    MultiView1.ActiveViewIndex = 0;
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            } 
            
        }
    }

    #region Delete User Related Tables
    public bool KullanicininBilgileriniSil(int userId, Entities entities)
    {
        try
        {
            //UserEmail
            var userEmails = entities.UserEmails.Where(p => p.UserId == userId).ToList();
            if (userEmails.Count > 0)
            {
                foreach (var userEmail in userEmails)
                {
                    entities.DeleteObject(userEmail);
                }
                //entities.SaveChanges();
            }

            //UserFoundation
            var userFoundations = entities.UserFoundation.Where(p => p.UserId == userId).ToList();
            if (userFoundations.Count > 0)
            {
                foreach (var userFoundation in userFoundations)
                {
                    entities.DeleteObject(userFoundation);
                }
                //entities.SaveChanges();
            }

            //UserFinance
            var userFinances = entities.UserFinance.Where(p => p.UserId == userId).ToList();
            if (userFinances.Count > 0)
            {
                foreach (var userFinance in userFinances)
                {
                    entities.DeleteObject(userFinance);
                }
                //entities.SaveChanges();
            }

            //UserGeneral
            var userGenerals = entities.UserGeneral.Where(p => p.UserId == userId).ToList();
            if (userGenerals.Count > 0)
            {
                foreach (var userGeneral in userGenerals)
                {
                    entities.DeleteObject(userGeneral);
                }
                //entities.SaveChanges();
            }

            //UserRole
            var userRoles = entities.UserRole.Where(p => p.UserId == userId).ToList();
            if (userRoles.Count > 0)
            {
                foreach (var userRole in userRoles)
                {
                    entities.DeleteObject(userRole);
                }
                //entities.SaveChanges();
            }

            //UserDuesLog
            var userDuesLogs = entities.UserDuesLog.Where(p => p.UserId == userId).ToList();
            if (userDuesLogs.Count > 0)
            {
                foreach (var userDuesLog in userDuesLogs)
                {
                    entities.DeleteObject(userDuesLog);
                }
                //entities.SaveChanges();
            }

            //System_Logs
            var systemLogs = entities.System_Logs.Where(p => p.userId == userId).ToList();
            if (systemLogs.Count > 0)
            {
                foreach (var systemLog in systemLogs)
                {
                    entities.DeleteObject(systemLog);
                }
                //entities.SaveChanges();
            }
            return true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
            return false;
        }
    }

    #endregion

    private void KullaniciGuncelle(Users user)
    {
        Temizle();
        hfUserId.Value = user.Id.ToString();
        TextBoxEPosta.Text = user.EMail;
        tbName.Text = user.Name;
        tbSurname.Text = user.Surname;
        //TextBoxParola.Text = user.Password;
        CheckBoxDurum.Checked = user.State;
        AktifRoller(user.Id);
    }

    private void AktifRoller(int Id)
    {
        RolleriVer(CheckBoxListRoller);
        var aktifRoller = from p in _entities.UserRole
                          where p.UserId == Id
                          select p;
        if (aktifRoller.Count() != 0)
        {
            foreach (var aktifRol in aktifRoller)
            {
                for (int i = 0; i <= CheckBoxListRoller.Items.Count - 1; i++)
                {
                    if (CheckBoxListRoller.Items[i].Value == aktifRol.RoleId.ToString())
                    {
                        CheckBoxListRoller.Items[i].Selected = true;
                        CheckBoxListRoller.Items[i].Attributes.Remove("onchange");
                        CheckBoxListRoller.Items[i].Attributes.Add("id", "cbRole_" + i);
                        CheckBoxListRoller.Items[i].Attributes.Add("onchange", "confirmRemoveRole('cbRole_" + i + "');");
                    }
                }
            }
        }
    }

    protected void GridViewKullanicilar_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        var btnDelete = e.Row.FindControl("imgBtnDelete") as ImageButton;
        if (btnDelete != null)
        {
            var userId = Convert.ToInt32(btnDelete.CommandArgument);
            if (!IsAuthForThisProcess(userId) || EnrollMembershipHelper.AreYouActiveUser(userId))
            {
                //btnDelete.Enabled = false;
                btnDelete.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;", AdminResource.msgNoAuth);
            }
            else
            {
                btnDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";   
            }
        }
        var btnEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
        if (btnEdit != null)
        {
            var userId = Convert.ToInt32(btnEdit.CommandArgument);
            if(!IsAuthForThisProcess(userId))
            {
                //btnDelete.Enabled = false;
                btnEdit.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;", AdminResource.msgNoAuth);
            } 
        }
    }
     
    protected void lbGeneratePwdClick(object sender, EventArgs e)
    {
        string generatedPwd = Guid.NewGuid().ToString("N");
        generatedPwd = generatedPwd.Substring(0, 8);
        var encrptedGeneratedPwd = Crypto.Encrypt(generatedPwd);
        var control = _entities.Users.Count(p => p.Password == encrptedGeneratedPwd);
        if(control==0)
        {
            TextBoxParola.Text = generatedPwd;    
        }  
    } 
}