using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class AdminIntroLightBox : UserControl
{
    private readonly Entities _entities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbIntroLightBox;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var listItemImage = new ListItem(AdminResource.lbImage, "0");
            var listItemPage = new ListItem(AdminResource.lbPage, "1");
            ddlType.Items.Clear();
            ddlType.Items.Insert(0, listItemImage);
            ddlType.Items.Insert(1, listItemPage);

            if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 15))
            {
                MultiView2.SetActiveView(vLightBox);
                MultiView1.SetActiveView(vNewBtn);
                BindType(0);
            }
            else
            {
                MultiView2.SetActiveView(vNoAuth);
            }
        }

        btNewLightBox.Text = AdminResource.lbNew;
        btnKaydet.Text = AdminResource.lbSave;
        btnIptal.Text = AdminResource.lbCancel;

        gvIntroLightBoxes.Columns[0].HeaderText = AdminResource.lbActions;
        gvIntroLightBoxes.Columns[1].HeaderText = AdminResource.lbTitle;
        gvIntroLightBoxes.Columns[2].HeaderText = AdminResource.lbContent;
        gvIntroLightBoxes.Columns[3].HeaderText = AdminResource.lbType;
        gvIntroLightBoxes.Columns[4].HeaderText = AdminResource.lbState;

        imgBtnImageSelect.Text = AdminResource.lbImageSelect;
    }

    protected void YeniEkleClick(object sender, EventArgs eventArgs)
    {
        Temizle();
        MultiView1.SetActiveView(vNew);
    }

    protected void KaydetClick(object sender, EventArgs eventArgs)
    {
        try
        {
            if (hfLightBoxId.Value != string.Empty)
            {
                // düzenleme
                var lightboxId = Convert.ToInt32(hfLightBoxId.Value);

                var lightBox = _entities.IntroLightBox.First(p => p.Id == lightboxId);
                lightBox.Title = tbTitle.Text;
                lightBox.Type = Convert.ToInt32(ddlType.SelectedValue);
                lightBox.UpdatedTime = DateTime.Now;
                lightBox.CookieExpireTime = Convert.ToInt32(tbRepeatDisplayTimeout.Text);
                lightBox.State = cbState.Checked;
                if (cbState.Checked)
                    SetPassiveActiveLightBox(); //aktif olarak düzenlenirse diğer aktif olan pasif yapılır
                if (ddlType.SelectedValue == "0") //görsel 
                {
                    lightBox.Width = null;
                    lightBox.Height = null;
                    lightBox.Link = txtImage.Text;
                }
                else
                {
                    //sayfa 
                    lightBox.Link = tbLink.Text;
                    lightBox.Width = Convert.ToInt32(tbWidth.Text);
                    lightBox.Height = Convert.ToInt32(tbHeight.Text);
                }

                _entities.SaveChanges(); 
                Temizle();
                gvIntroLightBoxes.DataBind();
                MultiView1.SetActiveView(vNewBtn);
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            else
            {
                // yeni ekleme
                var lightBox = new IntroLightBox();
                lightBox.Title = tbTitle.Text;
                lightBox.CookieExpireTime = Convert.ToInt32(tbRepeatDisplayTimeout.Text);
                lightBox.Type = Convert.ToInt32(ddlType.SelectedValue);
                lightBox.CreatedTime = DateTime.Now;
                lightBox.UpdatedTime = DateTime.Now;
                lightBox.State = cbState.Checked;
                if (cbState.Checked) SetPassiveActiveLightBox(); //aktif olarak eklenirse diğer aktif olan pasif yapılır
                if (ddlType.SelectedValue == "0") //görsel
                {
                    lightBox.Link = txtImage.Text; 
                }
                else {
                    lightBox.Link = tbLink.Text; //sayfa   
                    lightBox.Width = Convert.ToInt32(tbWidth.Text);
                    lightBox.Height = Convert.ToInt32(tbHeight.Text);
                }

                _entities.AddToIntroLightBox(lightBox);
                _entities.SaveChanges();
                
                Temizle();
                gvIntroLightBoxes.DataBind();
                MultiView1.SetActiveView(vNewBtn);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
        }
        catch (Exception hata)
        {
            ExceptionManager.ManageException(hata);
        }
    }

    protected void IptalClick(object sender, EventArgs eventArgs)
    {
        MultiView1.SetActiveView(vNewBtn);
        Temizle();
    }

    protected void GvIntroLightBoxes_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        var btnDelete = e.Row.FindControl("imgBtnDelete") as ImageButton;
        if (btnDelete != null)
        {
            btnDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
        }
    }

    protected void GvIntroLightBoxesRowCommand(object sender, GridViewCommandEventArgs e)
    {
        var entities = new Entities();
        if (e.CommandName == "Guncelle")
        {
            try
            {
                var id = Convert.ToInt32(e.CommandArgument);
                Temizle();
                hfLightBoxId.Value = id.ToString(); 
                var lightBox = _entities.IntroLightBox.First(p => p.Id == id);
                LightBoxBindForEdit(lightBox);
                MultiView1.SetActiveView(vNew);
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
                Temizle();
                hfLightBoxId.Value = id.ToString();
                 
                var lightBox = entities.IntroLightBox.FirstOrDefault(p => p.Id == id);
                if (lightBox != null)
                {
                    entities.IntroLightBox.DeleteObject(lightBox);
                    entities.SaveChanges(); 
                    MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                    gvIntroLightBoxes.DataBind();
                }
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindType(ddlType.SelectedIndex);
    }

    private void LightBoxBindForEdit(IntroLightBox lightBox)
    {
        tbTitle.Text = lightBox.Title;
        tbRepeatDisplayTimeout.Text = lightBox.CookieExpireTime.ToString();
        if (lightBox.Type == 0) // görsel
        {
            txtImage.Text = lightBox.Link;
        }
        else if (lightBox.Width != null && lightBox.Height != null) //sayfa
        {
            tbWidth.Text = lightBox.Width.Value.ToString();
            tbHeight.Text = lightBox.Height.Value.ToString();
            tbLink.Text = lightBox.Link;
        }

        cbState.Checked = lightBox.State;
        ddlType.SelectedIndex = lightBox.Type;
        BindType(lightBox.Type);
    }

    private void BindType(int type)
    {
        if (type == 0) //görsel
        {
            dvSayfa.Visible = false;
            tbLink.Visible = false;

            dvGorsel.Visible = true;
            imgBtnImageSelect.Visible = true;
            txtImage.Visible = true;

            trWidth.Visible = false;
            trHeight.Visible = false;
            tbWidth.Visible = false;
            tbHeight.Visible = false;
        }
        else //link
        {
            dvSayfa.Visible = true;
            tbLink.Visible = true;

            dvGorsel.Visible = false;
            txtImage.Visible = false;
            imgBtnImageSelect.Visible = false;


            trWidth.Visible = true;
            trHeight.Visible = true;
            tbWidth.Visible = true;
            tbHeight.Visible = true;
        }
    }

    private void Temizle()
    {
        hfLightBoxId.Value = null;

        tbTitle.Text = string.Empty;
        tbLink.Text = string.Empty;
        txtImage.Text = string.Empty;
        tbRepeatDisplayTimeout.Text = string.Empty;
        tbWidth.Text = string.Empty;
        tbHeight.Text = string.Empty;
        cbState.Checked = false;
        ddlType.SelectedIndex = 0;
        BindType(0);
    }

    private void SetPassiveActiveLightBox()
    {
        var e = new Entities();
        var activeLightBox = e.IntroLightBox.FirstOrDefault(p => p.State);
        if (activeLightBox != null)
        {
            activeLightBox.State = false;
            e.SaveChanges();
        }
    }
}