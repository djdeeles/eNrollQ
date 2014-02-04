using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_SSS : UserControl
{
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 12))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        if (!IsPostBack)
        {
            try
            {
                mvFaqCategories.SetActiveView(vNewBtnFaqCat);
                hfLanguageId.Value = EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
                mvFaqs.Visible = false;
                GridViewSSS.Visible = false;
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }

        btnNewCategory.Text = AdminResource.lbNewSssCategory;
        BtnAddNewFaq.Text = AdminResource.lbNewSss;

        CheckBoxDurum.Text = AdminResource.lbActive;
        cbFaqDurum.Text = AdminResource.lbActive;

        btnSaveCat.Text = AdminResource.lbSave;
        btnCancelCat.Text = AdminResource.lbCancel;

        BtnSaveQuest.Text = AdminResource.lbSave;
        BtnCancelQuest.Text = AdminResource.lbCancel;


        GridViewSSSKategoriler.Columns[0].HeaderText = AdminResource.lbActions;
        GridViewSSSKategoriler.Columns[1].HeaderText = AdminResource.lbCategoryName;
        GridViewSSSKategoriler.Columns[2].HeaderText = AdminResource.lbIndex;
        GridViewSSSKategoriler.Columns[3].HeaderText = AdminResource.lbState;

        GridViewSSS.Columns[0].HeaderText = AdminResource.lbActions;
        GridViewSSS.Columns[1].HeaderText = AdminResource.lbQuestion;
        GridViewSSS.Columns[2].HeaderText = AdminResource.lbAnsver;
        GridViewSSS.Columns[3].HeaderText = AdminResource.lbIndex;
        GridViewSSS.Columns[4].HeaderText = AdminResource.lbState;

        ClearInputs();
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbFAQManagement;
    }

    protected void BtnAddNewFaq_Click(object sender, EventArgs eventArgs)
    {
        hfFaqId.Value = null;
        KategorileriVer(ddlAddNew);
        mvFaqs.SetActiveView(vAddEditFaq);
    }

    protected void ImageButton3_Click(object sender, EventArgs eventArgs)
    {
        hfCategoryId.Value = null;
        mvFaqCategories.SetActiveView(vNewBtnFaqCat);
    }

    protected void GridViewSSSKategoriler_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            FaqCategories SSSKategori;
            int faqCategoryId = 0;
            if (e.CommandName == "Guncelle")
            {
                hfCategoryId.Value = e.CommandArgument.ToString();
                faqCategoryId = Convert.ToInt32(e.CommandArgument);
                SSSKategori = ent.FaqCategories.Where(p => p.faqCategoryId == faqCategoryId).First();
                SSSKategoriVer(SSSKategori);

                mvFaqCategories.SetActiveView(vAddEditFaqCat);
            }
            else if (e.CommandName == "Sil")
            {
                faqCategoryId = Convert.ToInt32(e.CommandArgument);
                SSSKategori = ent.FaqCategories.Where(p => p.faqCategoryId == faqCategoryId).First();

                //sıralamayı düzenle
                int index = Convert.ToInt32(SSSKategori.orderId);
                var cat = ent.FaqCategories.Where(p => p.orderId > index).ToList();
                foreach (var item in cat)
                {
                    item.orderId--;
                }

                ent.DeleteObject(SSSKategori);
                ent.SaveChanges();

                Logger.Add(12, 1, faqCategoryId, 2);

                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                GridViewSSSKategoriler.DataBind();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void SSSKategoriVer(FaqCategories SSSKategori)
    {
        try
        {
            TextBoxKategoriAdi.Text = SSSKategori.faqCategory;
            if (SSSKategori.orderId != null) TextBoxSiraNo.Text = SSSKategori.orderId.Value.ToString();
            if (SSSKategori.active != null && SSSKategori.active.Value)
            {
                CheckBoxDurum.Checked = true;
            }
            else
            {
                CheckBoxDurum.Checked = false;
            }
            hfCategoryId.Value = SSSKategori.faqCategoryId.ToString();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnSaveCat_Click(object sender, EventArgs e)
    {
        try
        {
            var faqCategories = new FaqCategories();

            if (!string.IsNullOrWhiteSpace(hfCategoryId.Value))
            {
                int faqCatId = Convert.ToInt32(hfCategoryId.Value);
                faqCategories = ent.FaqCategories.First(p => p.faqCategoryId == faqCatId);
            }
            faqCategories.faqCategory = TextBoxKategoriAdi.Text;
            if (CheckBoxDurum.Checked)
            {
                faqCategories.active = true;
            }
            else
            {
                faqCategories.active = false;
            }
            int index = Convert.ToInt32(TextBoxSiraNo.Text);

            var faqUpdate = ent.FaqCategories.Where(p => p.orderId == index).ToList();
            if (faqUpdate.Count > 0)
            {
                var faqList = ent.FaqCategories.Where(p => p.orderId >= index).ToList();
                foreach (FaqCategories faq in faqList)
                {
                    faq.orderId++;
                }
            }

            faqCategories.orderId = Convert.ToInt32(TextBoxSiraNo.Text);
            faqCategories.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
            faqCategories.UpdatedTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(hfCategoryId.Value))
            {
                faqCategories.CreatedTime = DateTime.Now;
                ent.AddToFaqCategories(faqCategories);
                ent.SaveChanges();
                Logger.Add(12, 1, faqCategories.faqCategoryId, 1);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            else
            {
                ent.SaveChanges();
                Logger.Add(12, 1, faqCategories.faqCategoryId, 3);
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }

            mvFaqCategories.SetActiveView(vNewBtnFaqCat);
            GridViewSSSKategoriler.DataBind();

            ClearInputs();
            hfCategoryId.Value = null;
            hfFaqId.Value = null;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    public void ClearInputs()
    {
        TextBoxKategoriAdi.Text = "";
        TextBoxSoru.Text = "";

        tbIndex.Text = "";
        TextBoxSiraNo.Text = "";
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        radEditor.Content = "";

        CheckBoxDurum.Checked = false;
        cbFaqDurum.Checked = false;
    }

    protected void GridViewSSSKategoriler_onDataRowBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var myMastDelete = (ImageButton) e.Row.FindControl("BtnDeleteCategory");
                myMastDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void lbCatSec_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["catId"] != null) ViewState.Remove("catId");
            var lbCatSec = (LinkButton) sender;
            int categoryId = Convert.ToInt32(lbCatSec.CommandArgument);
            hfCategoryId.Value = lbCatSec.CommandArgument;
            ViewState.Add("catId", categoryId);
            EntityDataSourceSSS.WhereParameters.Clear();
            EntityDataSourceSSS.WhereParameters.Add("catId", DbType.Int32, categoryId.ToString());

            GridViewSSS.DataBind();
            GridViewSSS.Visible = true;

            mvFaqs.SetActiveView(vNewBtnFaq);
            mvFaqs.Visible = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnSaveQuest_Click(object sender, EventArgs e)
    {
        int catId = 0;
        try
        {
            var sss = new Faq();
            if (!string.IsNullOrWhiteSpace(hfFaqId.Value))
            {
                int faqId = Convert.ToInt32(hfFaqId.Value);
                sss = ent.Faq.First(p => p.faqId == faqId);
            }
            catId = Convert.ToInt32(ddlAddNew.SelectedValue);
            sss.faqCategoryId = catId;
            sss.faqQuestion = TextBoxSoru.Text;
            var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
            sss.faqAnswer = radEditor.Content;
            sss.active = cbFaqDurum.Checked;

            int orderNo = Convert.ToInt32(tbIndex.Text);
            sss.faqOrderId = orderNo;
            var faqUpdate = ent.Faq.Where(p => p.faqCategoryId == catId && p.faqOrderId == orderNo).ToList();

            if (faqUpdate.Count > 0 && faqUpdate[0].faqId != sss.faqId)
            {
                var faqList = ent.Faq.Where(p => p.faqCategoryId == catId && p.faqOrderId >= orderNo).ToList();
                foreach (Faq faq in faqList)
                {
                    faq.faqOrderId++;
                }
            }

            if (!string.IsNullOrWhiteSpace(hfFaqId.Value))
            {
                sss.UpdatedTime = DateTime.Now;
                ent.SaveChanges();

                Logger.Add(12, 2, sss.faqId, 3);
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            else
            {
                sss.CreatedTime = DateTime.Now;
                ent.AddToFaq(sss);
                ent.SaveChanges();

                Logger.Add(12, 2, sss.faqId, 1);
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        GridViewSSSKategoriler.DataBind();
        EntityDataSourceSSS.WhereParameters.Clear();
        EntityDataSourceSSS.WhereParameters.Add("catId", DbType.Int32, catId.ToString());
        EntityDataSourceSSS.DataBind();
        GridViewSSS.DataBind();
        mvFaqs.SetActiveView(vNewBtnFaq);

        ClearInputs();
        hfFaqId.Value = null;
    }


    private void KategorileriVer(DropDownList dropDownListKategori)
    {
        ddlAddNew.Items.Clear();
        int itemCount = ddlAddNew.Items.Count;
        if (itemCount > 0)
            for (int i = 0; i < itemCount; i++)
            {
                ddlAddNew.Items.RemoveAt(0);
            }
        try
        {
            int dilId = EnrollAdminContext.Current.DataLanguage.LanguageId;
            var kategoriler = (from p in ent.FaqCategories
                               where p.languageId == dilId && p.active == true
                               orderby p.orderId
                               select new
                                          {
                                              KategoriAdi = p.faqCategory,
                                              KategoriId = p.faqCategoryId
                                          }).ToList();
            if (kategoriler.Count() != 0)
            {
                foreach (var item in kategoriler)
                {
                    var i = new ListItem();
                    i.Text = item.KategoriAdi;
                    i.Value = item.KategoriId.ToString();
                    dropDownListKategori.Items.Add(i);
                }
            }
            dropDownListKategori.Items.Insert(0, new ListItem(AdminResource.lbChoose, ""));
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void BtnCancelQuest_Click(object sender, EventArgs e)
    {
        ClearInputs();
        hfFaqId.Value = null;
        mvFaqs.SetActiveView(vNewBtnFaq);
    }

    protected void GridViewSSS_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var myButInnerDelete = (ImageButton) e.Row.FindControl("LinkButtonSil");
                myButInnerDelete.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void GridViewSSS_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Faq sss;
            int faqId = 0;
            if (e.CommandName == "Guncelle")
            {
                faqId = Convert.ToInt32(e.CommandArgument);
                sss = ent.Faq.First(p => p.faqId == faqId);
                KategorileriVer(ddlAddNew);
                SssGuncelle(sss);
                int selectedIndex = 0;
                foreach (ListItem item in ddlAddNew.Items)
                {
                    if (item.Value == sss.faqCategoryId.ToString())
                    {
                        ddlAddNew.SelectedIndex = selectedIndex;
                        break;
                    }
                    else
                    {
                        selectedIndex++;
                    }
                }
                mvFaqs.SetActiveView(vAddEditFaq);
            }
            else if (e.CommandName == "Sil")
            {
                faqId = Convert.ToInt32(e.CommandArgument);
                sss = ent.Faq.First(p => p.faqId == faqId);

                var faqList =
                    ent.Faq.Where(p => p.faqCategoryId == sss.faqCategoryId && p.faqOrderId >= sss.faqOrderId).ToList();
                foreach (Faq faq in faqList)
                {
                    faq.faqOrderId--;
                }

                ent.DeleteObject(sss);
                ent.SaveChanges();

                Logger.Add(12, 2, faqId, 2);

                GridViewSSS.DataBind();
                mvFaqs.SetActiveView(vNewBtnFaq);
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void SssGuncelle(Faq SSS)
    {
        ClearInputs();
        TextBoxSoru.Text = SSS.faqQuestion;
        var radEditor = ((RadEditor) Rtb1.FindControl("RadEditor1"));
        radEditor.Content = SSS.faqAnswer;

        if (SSS.active != null) CheckBoxDurum.Checked = SSS.active.Value;

        if (SSS.faqOrderId != null) tbIndex.Text = SSS.faqOrderId.Value.ToString();
        hfFaqId.Value = SSS.faqId.ToString();
    }

    protected void btnNewCategory_Click(object sender, EventArgs e)
    {
        hfCategoryId.Value = null;
        mvFaqCategories.SetActiveView(vAddEditFaqCat);
    }
}