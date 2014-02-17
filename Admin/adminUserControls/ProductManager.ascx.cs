using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using Telerik.Web.UI;
using eNroll.App_Data;
using eNroll.Helpers;
using Image = System.Drawing.Image;

public partial class admin_adminUserControls_ProductManager : UserControl
{
    private readonly Entities ent = new Entities();
    public List<Def_ProductCategories> productCategoryList;

    protected void Page_Load(object sender, EventArgs e)
    {
        productCategoryList =
            ent.Def_ProductCategories.Where(p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId).
                ToList();

        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 8))
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
                BaslangicEdit();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        if (Request.QueryString.Count != 0)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                pnlEditProduct.Visible = true;
                TemizleEdit();
                int ProductId = Convert.ToInt32(Request.QueryString["id"]);
                UrunVer(ProductId);
            }
        }

        btnAddProduct.Text = AdminResource.lbNew + " " + AdminResource.lbProduct;

        ImageButton1Insert.Text = AdminResource.lbImageSelect;
        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;

        cbActive.Text = AdminResource.lbActive;
        cbActiveInsert.Text = AdminResource.lbActive;
        CheckBoxActiveEdit.Text = AdminResource.lbActive;

        btnUpdate.Text = AdminResource.lbSave;
        btnUpdateCancel.Text = AdminResource.lbCancel;

        grvProducts.Columns[0].HeaderText = AdminResource.lbActions;
        grvProducts.Columns[1].HeaderText = AdminResource.lbName;

        btnUpdate.OnClientClick = "return priceInputsValidation(" + TextBoxPriceEdit.ClientID + "," +
                                  ddlPriceCurrencyEdit.ClientID + ");";
        btnSave.OnClientClick = "return priceInputsValidation(" + txtPriceInsert.ClientID + "," +
                                ddlPriceCurrency.ClientID + ");";

        btnNewFolder.Text = AdminResource.lbAdd;
        btnAddNewCategoryCancel.Text = AdminResource.lbCancel;
        btnButtonEditFolder.Text = AdminResource.lbSave;
        btnAddEditCategoryCancel.Text = AdminResource.lbCancel;
        btnDelete.Text = AdminResource.lbDelete;
        btnDelete.OnClientClick = "return confirm ('" + AdminResource.lbDeletingQuestion + "')";

        btNewCategory.Text = AdminResource.lbNewCategory;
        btEditCategory.Text = AdminResource.lbEdit;

        pnlAddCategory.Visible = false;
        btNewCategory.Visible = true;

        btnNewImage.Text = AdminResource.lbImageSelect;
        btnSave.Text = AdminResource.lbSave;
        btnCancel.Text = AdminResource.lbCancel;

        btnAddImage.Text = AdminResource.lbSave;

        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
            btnAddProduct.Visible = true;
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbProductManagement;
    }

    private void BaslangicEdit()
    {
        var enroll = new Entities();

        mvKat.SetActiveView(vTree);

        if (edsUrunler.WhereParameters.Contains(new Parameter("languageId")))
            edsUrunler.WhereParameters.Remove(new Parameter("languageId"));
        edsUrunler.WhereParameters.Add("languageId", DbType.Int32,
                                       EnrollAdminContext.Current.DataLanguage.LanguageId.ToString());
        LoadTree(TreeView2);
        LoadTree(TreeView1Edit);

        ddlPriceCurrency.DataSource = enroll.Currency.ToList();
        ddlPriceCurrency.DataTextField = "Symbol";
        ddlPriceCurrency.DataValueField = "Id";
        ddlPriceCurrency.DataBind();

        ddlPriceCurrency.Items.Insert(0, new ListItem(AdminResource.lbChoose, "0"));


        ddlPriceCurrencyEdit.DataSource = enroll.Currency.ToList();
        ddlPriceCurrencyEdit.DataTextField = "Symbol";
        ddlPriceCurrencyEdit.DataValueField = "Id";
        ddlPriceCurrencyEdit.DataBind();

        ddlPriceCurrencyEdit.Items.Insert(0, new ListItem(AdminResource.lbChoose, "0"));
    }

    public void LoadTree(RadTreeView view)
    {
        try
        {
            view.Nodes.Clear();
            var categories =
                ent.Def_ProductCategories.Where(
                    p =>
                    p.ParentCategoryId == null && p.State == true &&
                    p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId);
            foreach (Def_ProductCategories cat in categories)
            {
                var node = new RadTreeNode();
                node.Text = cat.Name + " ( " +
                            ent.Products.Where(p => p.ProductCategoryId == cat.ProductCategoryId).Count().ToString() +
                            " )";
                node.Value = cat.ProductCategoryId.ToString();
                GenerateTreeNode(cat.ProductCategoryId, node.Nodes);
                view.Nodes.Add(node);
            }
            //view.ExpandAllNodes();
            if (view == TreeView2)
            {
                view.Nodes.Insert(0, new RadTreeNode(AdminResource.lbRoot, "0"));
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    public void GenerateTreeNode(Int32 parent, RadTreeNodeCollection nodes)
    {
        try
        {
            var cat = GetChildNodes(parent);
            foreach (Def_ProductCategories category in cat)
            {
                try
                {
                    var node = new RadTreeNode();
                    node.Text = string.Format("{0} ( {1} )", category.Name,
                                              ent.Products.Count(p => p.ProductCategoryId == category.ProductCategoryId)
                                                  .ToString());
                    node.Value = category.ProductCategoryId.ToString();
                    nodes.Add(node);
                    GenerateTreeNode(Convert.ToInt32(category.ProductCategoryId), node.Nodes);
                }
                catch (Exception exception)
                {
                    ExceptionManager.ManageException(exception);
                }
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private List<Def_ProductCategories> GetChildNodes(Int32 ParentId)
    {
        List<Def_ProductCategories> cat = null;
        try
        {
            cat =
                ent.Def_ProductCategories.Where("it.ParentCategoryId=" + ParentId.ToString() + " and it.State=True")
                    .ToList();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        return cat;
    }

    protected void TreeView1Edit_SelectedNodeChanged(object sender, EventArgs e)
    {
        HiddenField1ProductCategoryId.Value = TreeView1Edit.SelectedValue;
        LabelKategoriEdit.Text = TreeView1Edit.SelectedNode.Text;
        mvKat.SetActiveView(vLabel);
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            pnlSelecetedCategory.Visible = true;
            var sb = new StringBuilder();
            string categoryId = TreeView2.SelectedValue;
            int catId = Convert.ToInt32(categoryId);
            edsUrunler.WhereParameters.Clear();
            edsUrunler.WhereParameters.Add("catId", DbType.Int32, categoryId);
            edsUrunler.WhereParameters.Add("languageId", DbType.Int32,
                                           EnrollAdminContext.Current.DataLanguage.LanguageId.ToString());
            grvProducts.DataBind();

            pnlAddCategory.Visible = false;
            btNewCategory.Visible = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        if (!String.IsNullOrEmpty(TreeView2.SelectedNode.Value) && TreeView2.SelectedNode.Value != "0")
        {
            HiddenField1ProductCategoryIdInsert.Value = TreeView2.SelectedNode.Value;
            HiddenField1.Value = TreeView2.SelectedNode.Value;
            LabelKategoriInsert.Text = TreeView2.SelectedNode.Text;
            MultiView2.SetActiveView(View5);
            int id = Convert.ToInt32(TreeView2.SelectedValue);
            var cat = new Entities().Def_ProductCategories.First(p => p.ProductCategoryId == id);
            txtEdit.Text = cat.Name;
            if (cat.State != null) cbEditActive.Checked = cat.State.Value;
            Label1.Text = TreeView2.SelectedNode.Text;
        }
        else
        {
            HiddenField1.Value = "";
            pnlSelecetedCategory.Visible = false;
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Guncelle")
            {
                pnlEditProduct.Visible = true;
                TemizleEdit();
                int productId = Convert.ToInt32(e.CommandArgument);

                UrunVer(productId);
            }
            if (e.CommandName == "Sil")
            {
                string id = e.CommandArgument.ToString();
                var p = ent.Products.Where("it.productId=" + id).First();
                var catId = p.ProductCategoryId.ToString();
                var img = ent.Product_Images.Where("it.Products.productId=" + id);
                foreach (var item in img)
                {
                    var thumbnail = item.Thumbnail;
                    ent.DeleteObject(item);
                    if (!String.IsNullOrEmpty(thumbnail))
                    {
                        ImageHelper.DeleteImage(Server.MapPath("../" + thumbnail.Replace("~/", "")));
                    }
                }

                Logger.Add(8, 2, p.ProductId, 2);

                ent.DeleteObject(p);
                ent.SaveChanges();
                MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
                edsUrunler.WhereParameters.Clear();
                edsUrunler.WhereParameters.Add("catId", DbType.Int32, catId);
                edsUrunler.WhereParameters.Add("languageId", DbType.Int32,
                                               EnrollAdminContext.Current.DataLanguage.LanguageId.ToString());
                grvProducts.DataBind();
                LoadTree(TreeView2);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void TemizleEdit()
    {
        try
        {
            TextBoxNameEdit.Text = string.Empty;
            TextBoxPriceEdit.Text = string.Empty;
            LoadTree(TreeView1Edit);
            LabelKategoriEdit.Text = string.Empty;

            DataList1.DataSource = null;
            DataList1.DataBind();

            var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
            radEditor.Content = string.Empty;
            CheckBoxActiveEdit.Checked = false;
            CheckBoxVitrinEdit.Checked = false;
            HiddenField1ProductIdEdit.Value = string.Empty;
            HiddenField1ProductCategoryId.Value = string.Empty;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    private void TemizleInsert()
    {
        txtNameInsert.Text = string.Empty;
        LoadTree(TreeView3Insert);
        MultiView2.SetActiveView(View4);
        var radEditor = ((RadEditor)Rtb2.FindControl("RadEditor1"));
        radEditor.Content = string.Empty;
        cbActiveInsert.Checked = false;
        cbVitrinInsert.Checked = false;
        txtPriceInsert.Text = string.Empty;
        tbImageText.Text = string.Empty;
        HiddenField1ProductCategoryIdInsert.Value = string.Empty;
    }

    private void UrunVer(int ProductId)
    {
        try
        {
            Products p = ent.Products.First(x => x.ProductId == ProductId);
            TextBoxNameEdit.Text = p.Name;
            var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
            radEditor.Content = p.Description;
            mvKat.SetActiveView(vLabel);
            LabelKategoriEdit.Text = p.Def_ProductCategories.Name;
            HiddenField1ProductIdEdit.Value = p.ProductId.ToString();
            HiddenField1ProductCategoryId.Value = p.ProductCategoryId.ToString();


            LabelKategoriInsert.Text = p.Def_ProductCategories.Name;
            HiddenField1ProductCategoryIdInsert.Value = p.ProductId.ToString();
            if (!string.IsNullOrEmpty(p.Price.ToString()))
            {
                TextBoxPriceEdit.Text = p.Price.Value.ToString("N");
            }
            CheckBoxActiveEdit.Checked = (Boolean)p.State;
            CheckBoxVitrinEdit.Checked = p.Vitrin.Value;
            var rst = from x in ent.Products
                      join i in ent.Product_Images
                          on x.ProductId equals i.ProductId
                      where x.ProductId == ProductId
                      orderby i.MainImage.Value descending
                      select new
                                 {
                                     x.Name,
                                     x.ProductId,
                                     i.MainImage,
                                     Thumbnail =
                          i.Thumbnail ?? "../../App_Themes/mainTheme/images/noimage.png"
                                 };
            ////DataList1Gorseller.DataSource = rst;
            ////DataList1Gorseller.DataBind();

            BindProductImageDataList(ProductId);
            //try
            //{   
            //    var query = from x in ent.Product_Images
            //                where x.Products.ProductId == ProductId
            //                select new { x.ProductImageId, x.Thumbnail, x.MainImage };
            //    DataList1.DataSource = query;
            //    DataList1.DataBind();
            //}
            //catch (Exception exception)
            //{
            //    ExceptionManager.ManageException(exception);
            //}


            try
            {
                var query = from x in ent.Product_Images
                            where x.Products.ProductId == ProductId
                            select new { x.ProductImageId, x.Thumbnail, x.MainImage };
                DataList1.DataSource = query;
                DataList1.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }

            var entity = new Entities();
            var currencies = entity.Currency.ToList();
            var selectedCurrency = 0;
            if (p.Currency != null)
                foreach (var currency in currencies)
                {
                    if (p.Currency == currency.Id)
                    {
                        ddlPriceCurrencyEdit.SelectedIndex = currency.Id;
                        selectedCurrency++;
                        break;
                    }
                    selectedCurrency++;
                }
            ddlPriceCurrencyEdit.SelectedIndex = selectedCurrency;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void LinkButtonShowTreeEdit_Click(object sender, EventArgs e)
    {
        mvKat.SetActiveView(vTree);
    }

    protected void btnUpdate_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            int productId = Convert.ToInt32(HiddenField1ProductIdEdit.Value);
            int productCategoryId = Convert.ToInt32(HiddenField1ProductCategoryId.Value);
            Products product = ent.Products.First(p => p.ProductId == productId);
            product.languageId = Convert.ToInt32(EnrollAdminContext.Current.DataLanguage.LanguageId);
            product.Name = TextBoxNameEdit.Text;
            var radEditor = ((RadEditor)Rtb1.FindControl("RadEditor1"));
            product.Description = radEditor.Content;
            product.ProductCategoryId = productCategoryId;
            if (!string.IsNullOrEmpty(TextBoxPriceEdit.Text) && ddlPriceCurrencyEdit.SelectedIndex != 0)
            {
                product.Price = Convert.ToDecimal(TextBoxPriceEdit.Text.Replace(".", ","));
                product.Currency = Convert.ToInt32(ddlPriceCurrencyEdit.SelectedValue);
            }
            else
            {
                product.Price = null;
                product.Currency = null;
            }
            product.State = CheckBoxActiveEdit.Checked;

            product.Vitrin = CheckBoxVitrinEdit.Checked;
            product.UpdatedTime = DateTime.Now;
            ent.SaveChanges();

            Logger.Add(8, 2, product.ProductId, 3);
            //MultiView1.SetActiveView(View1);
            edsUrunler.WhereParameters.Clear();
            edsUrunler.WhereParameters.Add("catId", DbType.Int32, productCategoryId.ToString());
            edsUrunler.WhereParameters.Add("languageId", DbType.Int32,
                                           EnrollAdminContext.Current.DataLanguage.LanguageId.ToString());
            grvProducts.DataBind();
            MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        grvProducts.DataBind();

        btNewCategory.Visible = true;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }

        pnlEditDeleteCategory.Visible = false;
        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;
    }

    protected void btnUpdateCancel_Click(object sender, EventArgs eventArgs)
    {
        btNewCategory.Visible = true;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }
        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditDeleteCategory.Visible = false;
        pnlEditProduct.Visible = false;
        HiddenField1ProductIdEdit.Value = null;
    }

    protected void btnAddProduct_Click(object sender, EventArgs eventArgs)
    {
        if (TreeView2.Nodes.Count != 0)
        {
            btNewCategory.Visible = true;
            btnAddProduct.Visible = false;
            pnlAddCategory.Visible = false;
            pnlAddProduct.Visible = true;
            pnlEditDeleteCategory.Visible = false;
            pnlEditProduct.Visible = false;
            TemizleInsert();
        }
        else
        {
            MessageBox.Show(MessageType.Success, AdminResource.lbNoCategoryForProduct);
        }
        if (TreeView2.SelectedNode != null && TreeView2.SelectedValue != "0")
        {
            LabelKategoriInsert.Text = TreeView2.SelectedNode.Text;
            HiddenField1ProductCategoryIdInsert.Value = TreeView2.SelectedValue;
            MultiView2.SetActiveView(View5);
        }
        else
        {
            MultiView2.SetActiveView(View4);
        }
    }

    protected void btnSave_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            if (!string.IsNullOrEmpty(HiddenField1ProductCategoryIdInsert.Value))
            {
                int ProductCategoryId = Convert.ToInt32(HiddenField1ProductCategoryIdInsert.Value);
                var product = new Products();
                var mainPic = new Product_Images();
                product.Name = txtNameInsert.Text;
                var radEditor = ((RadEditor)Rtb2.FindControl("RadEditor1"));
                product.Description = radEditor.Content;
                product.languageId = Convert.ToInt32(EnrollAdminContext.Current.DataLanguage.LanguageId);
                product.ProductCategoryId = ProductCategoryId;
                if (txtPriceInsert.Text != "" && ddlPriceCurrency.SelectedIndex != 0)
                {
                    product.Price = Convert.ToDecimal(txtPriceInsert.Text.Replace(".", ","));
                    product.Currency = Convert.ToInt32(ddlPriceCurrency.SelectedValue);
                }
                else
                {
                    product.Price = null;
                    product.Currency = null;
                }
                if (cbActiveInsert.Checked)
                    product.State = true;
                if (!cbActiveInsert.Checked)
                    product.State = false;
                if (cbVitrinInsert.Checked)
                    product.Vitrin = true;
                if (!cbVitrinInsert.Checked)
                    product.Vitrin = false;
                product.CreatedTime = DateTime.Now;
                product.UpdatedTime = DateTime.Now;
                ent.AddToProducts(product);

                if (!string.IsNullOrEmpty(tbImageText.Text))
                {
                    var orj = new Bitmap(Server.MapPath(tbImageText.Text.Replace("~", "..")));
                    Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                    string thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                    string dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                    ImageHelper.SaveJpeg(dest, (Bitmap)i, 75);
                    mainPic.ProductImage = tbImageText.Text;
                    mainPic.Thumbnail = "~/FileManager/thumbnails/" + thumbName;
                    mainPic.ProductId = product.ProductId;
                    mainPic.MainImage = true;
                    ent.AddToProduct_Images(mainPic);
                }

                ent.SaveChanges();

                Logger.Add(8, 2, product.ProductId, 1);

                LoadTree(TreeView2);


                edsUrunler.WhereParameters.Clear();
                edsUrunler.WhereParameters.Add("catId", DbType.Int32, ProductCategoryId.ToString());
                edsUrunler.WhereParameters.Add("languageId", DbType.Int32,
                                               EnrollAdminContext.Current.DataLanguage.LanguageId.ToString());
                grvProducts.DataBind();
                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);

                btNewCategory.Visible = true;
                if (productCategoryList.Count == 0)
                    btnAddProduct.Visible = false;
                else
                {
                    btnAddProduct.Visible = true;
                }
                pnlEditDeleteCategory.Visible = false;
                pnlAddCategory.Visible = false;
                pnlAddProduct.Visible = false;
                pnlEditProduct.Visible = false;
            }
            else
            {
                MessageBox.Show(MessageType.Success, AdminResource.msgSelectCategory);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void TreeView3Insert_SelectedNodeChanged(object sender, EventArgs e)
    {
        HiddenField1ProductCategoryIdInsert.Value = TreeView3Insert.SelectedValue;
        LabelKategoriInsert.Text = TreeView3Insert.SelectedNode.Text;
        MultiView2.SetActiveView(View5);
    }

    protected void lbShowTree_Click(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(View4);
    }

    protected void btnCancel_Click(object sender, EventArgs eventArgs)
    {
        btNewCategory.Visible = true;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }

        pnlEditDeleteCategory.Visible = false;
        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;
    }

    protected void grvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var myMastDelete = (ImageButton)e.Row.FindControl("btnDeleteProduct");
                myMastDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnNewFolder_Click(object sender, EventArgs e)
    {
        try
        {
            var cat = new Def_ProductCategories();
            cat.Name = txtNew.Text;
            if (HiddenField1.Value != "")
            {
                cat.ParentCategoryId = Convert.ToInt32(HiddenField1.Value);
            }
            if (HiddenField1.Value == "")
            {
                cat.ParentCategoryId = null;
            }
            cat.languageId = Convert.ToInt32(EnrollAdminContext.Current.DataLanguage.LanguageId);
            cat.State = cbActive.Checked;
            cat.CreatedTime = DateTime.Now;
            cat.UpdatedTime = DateTime.Now;
            ent.AddToDef_ProductCategories(cat);
            ent.SaveChanges();
            Logger.Add(8, 1, cat.ProductCategoryId, 1);
            MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            LoadTree(TreeView2);
            btnAddProduct.Visible = true;
            txtNew.Text = string.Empty;
            cbActive.Checked = true;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        btNewCategory.Visible = true;

        pnlEditDeleteCategory.Visible = false;
        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;
    }

    protected void btnButtonEditFolder_Click(object sender, EventArgs e)
    {
        int id = 0;
        try
        {
            id = Convert.ToInt32(HiddenField1.Value);
            var cat = ent.Def_ProductCategories.FirstOrDefault(p => p.ProductCategoryId == id);
            if (cat != null)
            {
                cat.Name = txtEdit.Text;
                cat.State = cbEditActive.Checked;
                cat.UpdatedTime = DateTime.Now;
                ent.SaveChanges();

                Logger.Add(8, 1, id, 3);
                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            LoadTree(TreeView2);
            txtEdit.Text = string.Empty;
            cbEditActive.Checked = false;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        btNewCategory.Visible = true;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }

        pnlEditDeleteCategory.Visible = false;
        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(HiddenField1.Value);
            kategoriTemizle(id);
            var cat = ent.Def_ProductCategories.First(p => p.ProductCategoryId == id);
            var urunler = ent.Products.Where(p => p.ProductCategoryId == cat.ProductCategoryId).ToList();
            foreach (Products urun in urunler)
            {
                //ürün görsellerini temizle
                var pimg = ent.Product_Images.Where(x => x.ProductId == urun.ProductId).ToList();
                foreach (var img in pimg)
                {
                    var thumbnail = img.Thumbnail;
                    ent.DeleteObject(img);
                    if (!string.IsNullOrWhiteSpace(thumbnail))
                        ImageHelper.DeleteImage(Server.MapPath(thumbnail.Replace("~", "..")));

                }
                ent.DeleteObject(urun);
            }
            ent.DeleteObject(cat);
            ent.SaveChanges();

            productCategoryList =
                ent.Def_ProductCategories.Where(p => p.languageId == EnrollAdminContext.Current.DataLanguage.LanguageId)
                    .ToList();
            if (productCategoryList.Count == 0)
                btnAddProduct.Visible = false;
            else
            {
                btnAddProduct.Visible = true;
            }

            Logger.Add(8, 1, id, 2);

            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
            LoadTree(TreeView2);
            txtEdit.Text = string.Empty;
            cbEditActive.Checked = false;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        btNewCategory.Visible = true;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }

        pnlEditDeleteCategory.Visible = false;
        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;

        HiddenField1.Value = null;
        pnlSelecetedCategory.Visible = false;
    }

    private void kategoriTemizle(int catId)
    {
        try
        {
            var altKategoriler = ent.Def_ProductCategories.Where(p => p.ParentCategoryId == catId).ToList();
            foreach (var i in altKategoriler)
            {
                //ürünleri temizle
                var urunler = ent.Products.Where(p => p.ProductCategoryId == i.ProductCategoryId).ToList();
                foreach (var urun in urunler)
                {
                    //ürün görsellerini temizle
                    var pimg = ent.Product_Images.Where(x => x.ProductId == urun.ProductId).ToList();
                    foreach (var img in pimg)
                    {
                        var thumbnail = img.Thumbnail;
                        ent.DeleteObject(img);
                        if (!string.IsNullOrWhiteSpace(thumbnail))
                            ImageHelper.DeleteImage(Server.MapPath(thumbnail.Replace("~", ".."))); 
                    }
                    ent.DeleteObject(urun);
                }
                kategoriTemizle(i.ProductCategoryId);
                ent.DeleteObject(i);
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btShowPnlAddCategory(object sender, EventArgs e)
    {
        btNewCategory.Visible = false;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }

        pnlEditDeleteCategory.Visible = false;
        pnlAddCategory.Visible = true;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;
    }

    protected void btShowPnlEditDeleteCategory(object sender, EventArgs e)
    {
        btNewCategory.Visible = false;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }

        pnlEditDeleteCategory.Visible = true;

        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;
    }

    protected void BtnAddEditCategoryCancelClick(object sender, EventArgs e)
    {
        btNewCategory.Visible = true;
        if (productCategoryList.Count == 0)
            btnAddProduct.Visible = false;
        else
        {
            btnAddProduct.Visible = true;
        }

        pnlEditDeleteCategory.Visible = false;
        pnlAddCategory.Visible = false;
        pnlAddProduct.Visible = false;
        pnlEditProduct.Visible = false;

        HiddenField1ProductCategoryId.Value = null;
    }

    protected void btnAddImageClick(object sender, EventArgs eventArgs)
    {
        if (!String.IsNullOrEmpty(TextBox1.Text))
        {
            try
            {
                var img = new Product_Images();
                int productId = Convert.ToInt32(HiddenField1ProductIdEdit.Value);
                Products p = ent.Products.First(x => x.ProductId == productId);

                var orj = new Bitmap(Server.MapPath(TextBox1.Text.Replace("~", "..")));
                Image i = ImageHelper.ResizeImage(orj, new Size(150, 150));
                string thumbName = Guid.NewGuid().ToString("N").Substring(1, 8) + ".jpg";
                string dest = Server.MapPath("../FileManager/thumbnails/" + thumbName);
                ImageHelper.SaveJpeg(dest, (Bitmap)i, 75);

                img.Thumbnail = "~/FileManager/thumbnails/" + thumbName;
                img.ProductId = p.ProductId;
                img.ProductImage = TextBox1.Text;
                img.MainImage = false;
                ent.AddToProduct_Images(img);
                ent.SaveChanges();

                TextBox1.Text = string.Empty;
                BindProductImageDataList(productId);
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
    }

    protected void ImgBtnDeleteImage_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                var lblid = (ImageButton)sender;
                int id = Convert.ToInt32(lblid.CommandArgument);
                Product_Images p = ent.Product_Images.Where(x => x.ProductImageId == id).First();
                string thumbToDel = "";
                thumbToDel = p.Thumbnail;
                ent.DeleteObject(p);
                ent.SaveChanges();
                if (!String.IsNullOrEmpty(thumbToDel))
                {
                    ImageHelper.DeleteImage(Server.MapPath("../" + thumbToDel.Replace("~/", "")));
                }

                int productId = Convert.ToInt32(HiddenField1ProductIdEdit.Value);
                BindProductImageDataList(productId);
                grvProducts.DataBind();
            }
            catch (Exception exception)
            {
                ExceptionManager.ManageException(exception);
            }
        }
        catch (Exception)
        {
        }
    }

    protected void lbGorselSec_Click(object sender, EventArgs e)
    {
        try
        {
            var lb = (LinkButton)sender;
            int productId = Convert.ToInt32(HiddenField1ProductIdEdit.Value);
            int id = Convert.ToInt32(lb.CommandArgument);
            var clearMainImage = ent.Product_Images.Where(p => p.ProductId == productId).ToList();
            for (int i = 0; i < clearMainImage.Count; i++)
            {
                clearMainImage[i].MainImage = false;
            }
            Product_Images pimg = ent.Product_Images.First(p => p.ProductImageId == id);
            pimg.MainImage = true;
            ent.SaveChanges();

            BindProductImageDataList(productId);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            var deleteButton = e.Item.FindControl("ImgBtnDeleteImage") as ImageButton;
            if (deleteButton != null)
            {
                deleteButton.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "')";
                deleteButton.ToolTip = AdminResource.lbDelete;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    public void BindProductImageDataList(int productId)
    {
        try
        {
            var query = from x in ent.Product_Images
                        where x.Products.ProductId == productId
                        select new { x.ProductImageId, x.Thumbnail, x.MainImage };
            if (query.Any())
            {
                var haveMainImage = query.Where(p => p.MainImage == true).ToList();
                if (haveMainImage.Count == 0)
                {
                    var image = ent.Product_Images.First(p => p.ProductId == productId);
                    image.MainImage = true;
                    ent.SaveChanges();
                }
            }

            DataList1.DataSource = query;
            DataList1.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }
}