using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Uye_UserControls_EventManager : UserControl
{
    Entities ent = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbEventManagement;
        btnAddNew.Text = AdminResource.lbAdd;

        gvEventList.Columns[0].HeaderText = AdminResource.lbActions;
        gvEventList.Columns[1].HeaderText = AdminResource.lbTitle;
        gvEventList.Columns[2].HeaderText = AdminResource.lbStartDate;
        gvEventList.Columns[3].HeaderText = AdminResource.lbEndDate;
        gvEventList.Columns[4].HeaderText = AdminResource.lbState;

        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;

        btnSave2.Text = AdminResource.lbSave;
        btnCancel2.Text = AdminResource.lbCancel;

        cbActive.Text = AdminResource.lbActive;
        CheckBox1Durum.Text = AdminResource.lbActive;

        btnAddNew.Text = AdminResource.lbNewEvent;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 10))
        {
            MultiView2.ActiveViewIndex = 0;
        }
        else
        {
            MultiView2.ActiveViewIndex = 1;
        }

        hfLanguageId.Value = EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
        if (!IsPostBack)
        {
            MultiView1.SetActiveView(View1);
        }
    }

    protected void ImageButton2_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            var etkinlik = new Events();

            if (dpStartDate.SelectedDate != null)
            {
                if (tpStartTime.SelectedDate != null)
                {
                    etkinlik.StartDate = Convert.ToDateTime(dpStartDate.SelectedDate.Value.ToShortDateString() + " " +
                                                            tpStartTime.SelectedDate.Value.ToShortTimeString());

                    try
                    {
                        if (dpEndDate.SelectedDate != null)
                        {
                            if (tpEndTime.SelectedDate != null)
                            {
                                etkinlik.EndDate = Convert.ToDateTime(dpEndDate.SelectedDate.Value.ToShortDateString() + " " +
                                                                      tpEndTime.SelectedDate.Value.ToShortTimeString());
                            }
                            else
                            {
                                etkinlik.EndDate = Convert.ToDateTime(dpEndDate.SelectedDate.Value.ToString());
                            }

                            if (etkinlik.EndDate < etkinlik.StartDate)
                            {
                                MessageBox.Show(MessageType.Error, AdminResource.lbEndDateError);
                                return;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            etkinlik.Name = txtName.Text;
            etkinlik.Description = txtDesc.Text;
            var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
            etkinlik.Details = radEditor.Content;

            etkinlik.State = cbActive.Checked;
            etkinlik.CreatedTime = DateTime.Now;
            etkinlik.UpdatedTime = DateTime.Now;
            etkinlik.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
            ent.AddToEvents(etkinlik);
            ent.SaveChanges();

            Logger.Add(10, 0, etkinlik.id, 1);

            MessageBox.Show(MessageType.Success, AdminResource.msgEventSaved);
            gvEventList.DataBind();
            Temizle();
            MultiView1.SetActiveView(View1);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ImageButton3_Click(object sender, EventArgs eventArgs)
    {
        MultiView1.SetActiveView(View1);
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Events etkinlik;
            int etkinlikId = 0;
            if (e.CommandName == "Guncelle")
            {
                etkinlikId = Convert.ToInt32(e.CommandArgument);
                etkinlik = ent.Events.First(p => p.id == etkinlikId);
                EtkinlikGuncelle(etkinlik);
                MultiView1.SetActiveView(View3);
            }
            else if (e.CommandName == "Sil")
            {
                etkinlikId = Convert.ToInt32(e.CommandArgument);
                etkinlik = ent.Events.First(p => p.id == etkinlikId);
                ent.DeleteObject(etkinlik);
                ent.SaveChanges();
                Logger.Add(10, 0, etkinlikId, 2);
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                gvEventList.DataBind();
                MultiView1.SetActiveView(View1);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void EtkinlikGuncelle(Events Etkinlik)
    {
        Temizle();
        TextBox1Baslik.Text = Etkinlik.Name;
        TextBox2Ozet.Text = Etkinlik.Description;
        var radEditor = ((RadEditor)Rtb2.FindControl("RadEditor1"));
        radEditor.Content = Etkinlik.Details;
        editDpStartDate.SelectedDate = Etkinlik.StartDate;
        editTpStartTime.SelectedDate = Etkinlik.StartDate;
        editDpEndDate.SelectedDate = Etkinlik.EndDate;
        editTpEndTime.SelectedDate = Etkinlik.EndDate;
        CheckBox1Durum.Checked = Convert.ToBoolean(Etkinlik.State);
        HiddenFieldEtkinlikId.Value = Etkinlik.id.ToString();
    }

    private void Temizle()
    {
        txtName.Text = string.Empty;
        TextBox1Baslik.Text = string.Empty;

        txtDesc.Text = string.Empty;
        TextBox2Ozet.Text = string.Empty;

        var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
        radEditor.Content = string.Empty;

        var radEditor2 = ((RadEditor)Rtb2.FindControl("RadEditor1"));
        radEditor2.Content = string.Empty;

        cbActive.Checked = true;
        CheckBox1Durum.Checked = true;

        dpStartDate.SelectedDate = null;
        tpStartTime.SelectedDate = null;
        dpEndDate.SelectedDate = null;
        tpEndTime.SelectedDate = null;

        editDpStartDate.SelectedDate = null;
        editTpStartTime.SelectedDate = null;
        editDpEndDate.SelectedDate = null;
        editTpEndTime.SelectedDate = null;
        cbActive.Checked = false;
    }

    protected void BtnSaveClick(object sender, EventArgs eventArgs)
    {
        try
        {
            int etkinlikId = Convert.ToInt32(HiddenFieldEtkinlikId.Value);
            var etkinlik = ent.Events.First(p => p.id == etkinlikId);
            etkinlik.Name = TextBox1Baslik.Text;
            etkinlik.Description = TextBox2Ozet.Text;
            var radEditor = ((RadEditor)Rtb2.FindControl("RadEditor1"));
            etkinlik.Details = radEditor.Content;
            if (editDpStartDate.SelectedDate != null)
            {
                if (editTpStartTime.SelectedDate != null)
                    etkinlik.StartDate =
                        Convert.ToDateTime(editDpStartDate.SelectedDate.Value.ToShortDateString() + " " +
                                           editTpStartTime.SelectedDate.Value.ToShortTimeString());
            }
            if (editDpEndDate.SelectedDate != null)
            {
                if (editTpEndTime.SelectedDate != null)
                    etkinlik.EndDate = Convert.ToDateTime(editDpEndDate.SelectedDate.Value.ToShortDateString() + " " +
                                                          editTpEndTime.SelectedDate.Value.ToShortTimeString());
                else
                {
                    etkinlik.EndDate = Convert.ToDateTime(editDpEndDate.SelectedDate.Value.ToString());
                }
            }

            // bitiş tarihinin başlagıç tarihinden büyük olma kontrolü
            if (etkinlik.EndDate < etkinlik.StartDate)
            {
                MessageBox.Show(MessageType.Success, AdminResource.lbEndDateError);
                MultiView1.SetActiveView(View3);
                return;
            }

            etkinlik.State = CheckBox1Durum.Checked;
            etkinlik.UpdatedTime = DateTime.Now;

            ent.SaveChanges();

            Logger.Add(10, 0, etkinlik.id, 3);

            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            gvEventList.DataBind();
            Temizle();
            MultiView1.SetActiveView(View1);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnCancel_Click(object sender, EventArgs eventArgs)
    {
        MultiView1.SetActiveView(View1);
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        MultiView1.SetActiveView(View2);
    }


    protected void gvEventList_onRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var deleteButton = (ImageButton)e.Row.FindControl("btnDelete");
                deleteButton.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "')";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void ImageButton2Ara_Click(object sender, ImageClickEventArgs e)
    {

        try
        {
            var aranan = txtAra.Text;

            dsEvents.Where = "it.languageId=" +
                                      EnrollAdminContext.Current.DataLanguage.LanguageId.ToString() +
                                      " and (it.Name like '%" + aranan + "%' OR it.Description like '%" + aranan + "%')";
            dsEvents.DataBind();
            gvEventList.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}