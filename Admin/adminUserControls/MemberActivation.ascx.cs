using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

namespace eNroll.Admin.adminUserControls
{
    public partial class MemberActivation : UserControl
    {
        private const string SqlSelect = "Select *,u.Id as uId From Users as u " +
                                         "Inner Join UserFoundation f on u.Id=f.UserId " +
                                         "Inner Join UserEmails e on (u.Id=e.UserId and e.MainAddress=1) ";

        private static MemberInfo Member = new MemberInfo();
        private readonly Entities _entities = new Entities();
        public string SqlWhere = string.Empty;
        private DataTable _memberSearchDataTable = new DataTable();

        public SqlConnection _oConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ToString());

        private EntityDataSource edsMember = new EntityDataSource();


        protected override void OnInit(EventArgs e)
        {
            Session["currentPath"] = AdminResource.lbMemberActivation;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 30))
            {
                mvAuthoriztn.SetActiveView(vAuth);

                gVMembers.Columns[0].HeaderText = AdminResource.lbActions;
                gVMembers.Columns[1].HeaderText = AdminResource.lbName;
                gVMembers.Columns[2].HeaderText = AdminResource.lbSurname;
                gVMembers.Columns[3].HeaderText = AdminResource.lbEmail;
                gVMembers.Columns[4].HeaderText = AdminResource.lbMemberRelType;
                gVMembers.Columns[5].HeaderText = AdminResource.lbState;

                btSendActivationCode.Text = AdminResource.lbSendActivationCode;
                btSendActivationCode.Visible = false;
                BtnDeleteAll.Text = AdminResource.lbDeleteAll;
                BtnDeleteAll.Visible = false;

                //gVMembers.DataBind();
                if (!IsPostBack)
                {
                    mvMembers.SetActiveView(vGridMembers);
                    EnrollMembershipHelper.BindDdlMemberStatesFilter(ddlMemberStatesFilter);
                    BindMembers();
                }
                ddlMemberStatesFilter.Items[1].Attributes.Add("title", AdminResource.lbWaitingForApprovalToolTip);
                ddlMemberStatesFilter.Items[2].Attributes.Add("title", AdminResource.lbActiveToolTip);
                ddlMemberStatesFilter.Items[3].Attributes.Add("title", AdminResource.lbPassiveToolTip);
                ddlMemberStatesFilter.Items[4].Attributes.Add("title", AdminResource.lbInvalidMembersToolTip);
                //mvMembers.SetActiveView(vGridMembers);
                //EnrollMembershipHelper.BindDdlMemberStatesFilter(ddlMemberStatesFilter);
                //BindMembers();
            }
            else
            {
                mvAuthoriztn.SetActiveView(vNoAuth);
            }
        }

        protected void ImgBtnMemberEditClick(object sender, ImageClickEventArgs e)
        {
            var btnMemberEdit = sender as ImageButton;
            if (btnMemberEdit != null)
            {
                try
                {
                    hfUserId.Value = btnMemberEdit.CommandArgument;
                    int userId = Convert.ToInt32(hfUserId.Value);
                    cMemberAddEdit.BindForEditMember(userId);
                    Member = EnrollMembershipHelper.BindUser(Member, userId);
                    if (Member != null)
                    {
                        var btnConfirmMemberActive = ((Button) cMemberAddEdit.FindControl("BtnConfirmMemberActive"));
                        btnConfirmMemberActive.Visible =
                            EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                            EnrollMembershipHelper.MembershipType.OnayBekleyen;

                        var btnSetPassive = ((Button) cMemberAddEdit.FindControl("BtnSetPassive"));
                        btnSetPassive.Visible =
                            EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                            EnrollMembershipHelper.MembershipType.Aktif;

                        var btnSetActive = ((Button) cMemberAddEdit.FindControl("BtnSetActive"));
                        btnSetActive.Visible =
                            EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                            EnrollMembershipHelper.MembershipType.Pasif;

                        var btnResendActivationCode = ((Button) cMemberAddEdit.FindControl("BtnResendActivationCode"));
                        btnResendActivationCode.Visible =
                            EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                            EnrollMembershipHelper.MembershipType.Hatali;


                        BtnDeleteAll.Visible = EnrollMembershipHelper.UserMembershipState(userId.ToString()) ==
                                               EnrollMembershipHelper.MembershipType.Hatali;

                        cMemberAddEdit.BindPersonalInfo();
                        cMemberAddEdit.BindMemberInfo();
                        cMemberAddEdit.BindHomeInfo();
                        cMemberAddEdit.BindWorkInfo();
                        cMemberAddEdit.BindEmailInfo();
                    }
                    mvMembers.SetActiveView(vEditMembers);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindMembers();
        }

        protected void ImgBtnMemberDeleteClick(object sender, ImageClickEventArgs e)
        {
            var ent = new Entities();
            var btnMemberDelete = sender as ImageButton;
            if (btnMemberDelete != null)
            {
                try
                {
                    hfUserId.Value = btnMemberDelete.CommandArgument;
                    var userId = Convert.ToInt32(hfUserId.Value);
                    var user = ent.Users.FirstOrDefault(p => p.Id == userId);
                    if (user != null && KullanicininBilgileriniSil(user.Id, ent))
                    {
                        ent.Users.DeleteObject(user);
                        ent.SaveChanges();
                        {
                            ent.SaveChanges();
                        }
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindMembers();
        }

        protected void BtnSetActiveClick(object sender, EventArgs eventArgs)
        {
            var btnMemberEdit = sender as Button;
            if (btnMemberEdit != null)
            {
                try
                {
                    hfUserId.Value = btnMemberEdit.CommandArgument;
                    var userId = Convert.ToInt32(hfUserId.Value);
                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    if (user != null)
                    {
                        user.State = true;
                        _entities.SaveChanges();
                        gVMembers.DataBind();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindMembers();
        }

        protected void BtnSetPassiveClick(object sender, EventArgs eventArgs)
        {
            var btnMemberEdit = sender as Button;
            if (btnMemberEdit != null)
            {
                try
                {
                    hfUserId.Value = btnMemberEdit.CommandArgument;
                    var userId = Convert.ToInt32(hfUserId.Value);
                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    if (user != null)
                    {
                        user.State = false;
                        _entities.SaveChanges();
                        gVMembers.DataBind();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindMembers();
        }

        protected void BtnConfirmMemberActiveClick(object sender, EventArgs e)
        {
            var btnMemberConfirm = sender as Button;
            if (btnMemberConfirm != null)
            {
                try
                {
                    hfUserId.Value = btnMemberConfirm.CommandArgument;
                    var userId = Convert.ToInt32(hfUserId.Value);

                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == userId);

                    if (user != null && userFoundation != null)
                    {
                        user.State = true;
                        userFoundation.MemberState = true;
                        _entities.SaveChanges();
                        gVMembers.DataBind();
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
            BindMembers();
        }

        protected void BtnResendActivationCodeClick(object sender, EventArgs e)
        {
            var btnResendActivationCode = sender as Button;
            if (btnResendActivationCode != null)
            {
                try
                {
                    hfUserId.Value = btnResendActivationCode.CommandArgument;
                    var userId = Convert.ToInt32(hfUserId.Value);

                    var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                    if (user != null)
                    {
                        var userEmail = _entities.UserEmails.FirstOrDefault(p => p.UserId == userId && p.MainAddress);
                        if (userEmail != null)
                        {
                            EnrollMembershipHelper.SendEmailActivationMail(user.Id, userEmail.Id,
                                                                           "../App_Themes/mainTheme/mailtemplates/MemberActivationMailContent.htm");
                        }
                    }
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                    MessageBox.Show(MessageType.Warning, AdminResource.lbErrorOccurred);
                }
            }
        }

        protected void gVMembers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            var imgBtnDelete = e.Row.FindControl("imgBtnDelete") as ImageButton;
            var imgBtnEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
            var imgBtnSetActive = e.Row.FindControl("BtnConfirmMemberActive") as Button;
            var imgBtnSetPassive = e.Row.FindControl("BtnSetPassive") as Button;
            var btnSetActive = e.Row.FindControl("BtnSetActive") as Button;
            var btnResendActivationCode = e.Row.FindControl("BtnResendActivationCode") as Button;

            if (imgBtnEdit != null)
            {
                var userId = Convert.ToInt32(imgBtnEdit.CommandArgument);
                if (HttpContext.Current.User.Identity.IsAuthenticated &&
                    !EnrollMembershipHelper.IsAuthForThisProcess(userId,
                                                                 EnrollMembershipHelper.GetUserIdFromEmail(
                                                                     HttpContext.Current.User.Identity.Name)))
                {
                    imgBtnEdit.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;",
                                                             AdminResource.msgNoAuth);
                }
            }
            if (imgBtnDelete != null)
            {
                var userId = Convert.ToInt32(imgBtnDelete.CommandArgument);
                if (
                    !EnrollMembershipHelper.IsAuthForThisProcess(userId,
                                                                 EnrollMembershipHelper.GetUserIdFromEmail(
                                                                     HttpContext.Current.User.Identity.Name)) ||
                    EnrollMembershipHelper.AreYouActiveUser(userId))
                {
                    imgBtnDelete.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;",
                                                               AdminResource.msgNoAuth);
                }
                else
                {
                    imgBtnDelete.OnClientClick = " return confirm('" + AdminResource.lbConfirmMsgDeleteMember + "'); ";
                }
            }

            if (imgBtnSetActive != null)
            {
                imgBtnSetActive.Text = AdminResource.lbConfirmMember;
                var userId = Convert.ToInt32(imgBtnSetActive.CommandArgument);
                if (
                    !EnrollMembershipHelper.IsAuthForThisProcess(userId,
                                                                 EnrollMembershipHelper.GetUserIdFromEmail(
                                                                     HttpContext.Current.User.Identity.Name)))
                {
                    imgBtnSetActive.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;",
                                                                  AdminResource.msgNoAuth);
                }
                else
                {
                    imgBtnSetActive.OnClientClick = " return confirm('" + AdminResource.lbConfirmActivateMember + "'); ";
                }
            }


            if (imgBtnSetPassive != null)
            {
                imgBtnSetPassive.Text = AdminResource.lbSetPassive;
                var userId = Convert.ToInt32(imgBtnSetPassive.CommandArgument);
                if (
                    !EnrollMembershipHelper.IsAuthForThisProcess(userId,
                                                                 EnrollMembershipHelper.GetUserIdFromEmail(
                                                                     HttpContext.Current.User.Identity.Name)))
                {
                    imgBtnSetPassive.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;",
                                                                   AdminResource.msgNoAuth);
                }
                else
                {
                    imgBtnSetPassive.OnClientClick = " return confirm('" + AdminResource.lbConfirmPassiveMember + "'); ";
                }
            }


            if (btnSetActive != null)
            {
                btnSetActive.Text = AdminResource.lbSetActive;
                var userId = Convert.ToInt32(btnSetActive.CommandArgument);
                if (
                    !EnrollMembershipHelper.IsAuthForThisProcess(userId,
                                                                 EnrollMembershipHelper.GetUserIdFromEmail(
                                                                     HttpContext.Current.User.Identity.Name)))
                {
                    btnSetActive.OnClientClick = string.Format("javascript:showWarningToast('{0}');return false;",
                                                               AdminResource.msgNoAuth);
                }
                else
                {
                    btnSetActive.OnClientClick = " return confirm('" + AdminResource.lbConfirmActivateMember + "'); ";
                }
            }

            if (btnResendActivationCode != null) btnResendActivationCode.Text = AdminResource.lbSendActivationCode;
        }

        public string GetMembershipType(string userIdStr)
        {
            var membershipType = string.Empty;
            if (!string.IsNullOrEmpty(userIdStr))
            {
                var userId = Convert.ToInt32(userIdStr);
                if (userId > 0)
                {
                    var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == userId);
                    if (userFoundation != null)
                    {
                        var foundationRelType =
                            _entities.FoundationRelType.FirstOrDefault(p => p.Id == userFoundation.MemberRelType);
                        if (foundationRelType != null)
                            membershipType = foundationRelType.Name;
                    }
                }
            }
            return membershipType;
        }

        public bool GetMembershipState(string userIdStr)
        {
            var membershipType = false;
            if (!string.IsNullOrEmpty(userIdStr))
            {
                var userId = Convert.ToInt32(userIdStr);
                if (userId > 0)
                {
                    var userFoundation = _entities.UserFoundation.FirstOrDefault(p => p.UserId == userId);
                    if (userFoundation != null)
                    {
                        membershipType = userFoundation.MemberState;
                    }
                }
            }
            return membershipType;
        }

        public bool GetEmailActivation(string userIdStr)
        {
            var emailActivation = false;
            if (!string.IsNullOrEmpty(userIdStr))
            {
                var userId = Convert.ToInt32(userIdStr);
                if (userId > 0)
                {
                    var userEmails = _entities.UserEmails.FirstOrDefault(p => p.UserId == userId && p.MainAddress);
                    if (userEmails != null)
                    {
                        emailActivation = userEmails.Activated;
                    }
                }
            }
            return emailActivation;
        }

        protected void ddlMemberStatesFilter_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedFilter = Crypto.Decrypt(ddlMemberStatesFilter.SelectedItem.Value);

            hfMemberEmailActivationFilter.Value = string.Empty;
            hfMemberStateFilter.Value = string.Empty;

            if (!string.IsNullOrEmpty(selectedFilter))
            {
                var data = selectedFilter.Split('|');
                if (data.Length > 2)
                {
                    hfUserStateFilter.Value = (!string.IsNullOrWhiteSpace(data[0]) ? Crypto.Encrypt(data[0]) : "");
                    hfMemberStateFilter.Value = (!string.IsNullOrWhiteSpace(data[1]) ? Crypto.Encrypt(data[1]) : "");
                    hfMemberEmailActivationFilter.Value = (!string.IsNullOrWhiteSpace(data[2])
                                                               ? Crypto.Encrypt(data[2])
                                                               : "");
                }
            }

            SqlWhere = CreateWhereParamMemberState();
            BindMembers();
        }

        public string CreateWhereParamMemberState()
        {
            var where = string.Empty;
            if (!string.IsNullOrWhiteSpace(hfMemberStateFilter.Value))
            {
                //üye kontrolü => [User.State|UserFoundation.MemberState|UserEmails.Activated]
                var filterUserState = Crypto.Decrypt(hfUserStateFilter.Value);
                var filterMembersState = Crypto.Decrypt(hfMemberStateFilter.Value);
                var filterMailActivation = Crypto.Decrypt(hfMemberEmailActivationFilter.Value);

                //lbUnverified; [0|0|0]
                if (filterUserState == "0" && filterMembersState == "0" && filterMailActivation == "0")
                {
                    btSendActivationCode.Visible = true;
                    BtnDeleteAll.Visible = true;
                    where = "Where u.State=0 AND  f.MemberState=0 AND e.Activated=0";
                }
                    //lbWaitingForApproval; [0|0|1]
                else if (filterUserState == "0" && filterMembersState == "0" && filterMailActivation == "1")
                {
                    where = "Where u.State=0 AND  f.MemberState=0 AND e.Activated=1";
                }
                    //lbActive; [1|1|1]
                else if (filterUserState == "1" && filterMembersState == "1" && filterMailActivation == "1")
                {
                    where = "Where u.State=1 AND  f.MemberState=1 AND e.Activated=1";
                }
                    //üyelik pasif edilmiş;  [0|1|1]
                else if (filterUserState == "0" && filterMembersState == "1" && filterMailActivation == "1")
                {
                    where = "Where u.State=0 AND  f.MemberState=1 AND e.Activated=1";
                }
                else
                {
                    where = string.Empty;
                }
            }
            else
            {
                where = string.Empty;
            }


            return where;
        }

        public void BindMembers()
        {
            try
            {
                SqlWhere = CreateWhereParamMemberState();

                var cmdSearchResault = new SqlCommand
                                           {
                                               Connection = _oConnection,
                                               CommandText = SqlSelect + SqlWhere
                                           };

                if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                _memberSearchDataTable = new DataTable();
                var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                oAdaptor.Fill(_memberSearchDataTable);
                if (btSendActivationCode.Visible && _memberSearchDataTable.Rows.Count == 0)
                {
                    btSendActivationCode.Visible = false;
                    BtnDeleteAll.Visible = false;
                }
                gVMembers.DataSource = _memberSearchDataTable;
                gVMembers.DataBind();

                tbTestSqlSource.Text = SqlSelect + SqlWhere;
            }
            catch (Exception exception)
            {
                if (_oConnection.State == ConnectionState.Open) _oConnection.Close();
                ExceptionManager.ManageException(exception);
            }
            finally
            {
                if (_oConnection.State == ConnectionState.Open) _oConnection.Close();
            }
        }

        protected void BtSendActivationCode_OnClick(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    SqlWhere = CreateWhereParamMemberState();
                    var cmdSearchResault = new SqlCommand
                                               {
                                                   Connection = _oConnection,
                                                   CommandText = SqlSelect + SqlWhere
                                               };

                    if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                    _memberSearchDataTable = new DataTable();
                    var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                    oAdaptor.Fill(_memberSearchDataTable);

                    if (_memberSearchDataTable.Rows.Count > 0)
                    {
                        var mailCount = 0;
                        foreach (DataRow row in _memberSearchDataTable.Rows)
                        {
                            #region sendMail

                            var userId = Convert.ToInt32(row["uId"].ToString());
                            var user = _entities.Users.FirstOrDefault(p => p.Id == userId);
                            if (user != null)
                            {
                                var userEmail =
                                    _entities.UserEmails.FirstOrDefault(p => p.UserId == userId && p.MainAddress);

                                if (userEmail != null)
                                {
                                    EnrollMembershipHelper.SendEmailActivationMail(userId, userEmail.Id,
                                                                                   "../App_Themes/mainTheme/mailtemplates/MemberActivationMailContent.htm");
                                    mailCount++;
                                }
                            }

                            #endregion
                        }
                        if (mailCount > 0)
                        {
                            MessageBox.Show(MessageType.Notice,
                                            string.Format(AdminResource.msgVerificationCodeSentMember,
                                                          mailCount.ToString()));
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageType.Warning, AdminResource.msgNoMemberFound);
                    }
                }
                catch (Exception exception)
                {
                    if (_oConnection.State == ConnectionState.Open) _oConnection.Close();
                    ExceptionManager.ManageException(exception);
                }
                finally
                {
                    if (_oConnection.State == ConnectionState.Open) _oConnection.Close();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        protected void BtnDeleteAll_OnClick(object sender, EventArgs e)
        {
            try
            {
                SqlWhere = CreateWhereParamMemberState();

                var cmdSearchResault = new SqlCommand
                                           {
                                               Connection = _oConnection,
                                               CommandText = SqlSelect + SqlWhere
                                           };

                if (_oConnection.State == ConnectionState.Closed) _oConnection.Open();
                _memberSearchDataTable = new DataTable();
                var oAdaptor = new SqlDataAdapter(cmdSearchResault);
                oAdaptor.Fill(_memberSearchDataTable);
                foreach (DataRow dataRow in _memberSearchDataTable.Rows)
                {
                    var userId = Convert.ToInt32(dataRow["uId"]);
                    var userEmails = _entities.UserEmails.Where(p => p.UserId == userId).ToList();
                    var userFoundation = _entities.UserFoundation.Where(p => p.UserId == userId).ToList();
                    var userRoles = _entities.UserRole.Where(p => p.UserId == userId).ToList();
                    var user = _entities.Users.Where(p => p.Id == userId).ToList();

                    _entities.DeleteObject(userEmails);
                    _entities.DeleteObject(userFoundation);
                    _entities.DeleteObject(userRoles);
                    _entities.DeleteObject(user);

                    _entities.SaveChanges();
                }
                MessageBox.Show(MessageType.Success, "");
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
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
    }
}