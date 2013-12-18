using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using Resources;
using eNroll.App_Data;

public partial class M_UserControls_ProductDetails : UserControl
{
    private readonly Entities ent = new Entities();

    protected void imgCliclk(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PanelResimDiv.Visible = true;
        UrunDetayVer();
    }

    private void UrunDetayVer()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            Products products = ent.Products.FirstOrDefault(x => x.ProductId == id);

            if (products != null && products.State != true)
                Response.Redirect("~/404.aspx");

            int resimKontrol = ent.Product_Images.Count(x => x.ProductId == id);

            if (products != null)
            {
                ltName.Text = products.Name;
                ltDesc.Text = products.Description;

                if (products.Price != null)
                {
                    var currency = ent.Currency.FirstOrDefault(p => p.Id == products.Currency);
                    if (currency != null)
                    {
                        var builder = new StringBuilder();
                        var price = decimal.Round(Convert.ToDecimal(products.Price), 2).ToString();
                        var symbol = currency.Symbol;

                        if (currency.Position.Trim(' ') == "R")
                        {
                            ltPrice.Text =
                                builder.AppendFormat("<br/><b>{0}</b> {1} {2}", Resource.lbPrice, price, symbol).
                                    ToString();
                        }
                        else
                        {
                            ltPrice.Text =
                                builder.AppendFormat("<br/><b>{0}</b> {2} {1}", Resource.lbPrice, price, symbol).
                                    ToString();
                        }
                    }
                }
                else
                {
                    ltPrice.Text = string.Empty;
                }
            }

            if (resimKontrol > 0)
            {
                Product_Images pi = ent.Product_Images.FirstOrDefault(x => x.ProductId == id && x.MainImage == true);
                if (pi != null) Image1.ImageUrl = pi.ProductImage;
            }
            else
            {
                Image1.ImageUrl = "/App_Themes/mainTheme/images/noimage.png";
                DataList1.Visible = false;
            }

            if (DataList1.Visible && resimKontrol > 1)
            {
                IQueryable<Product_Images> Resimler = from q in ent.Product_Images
                                                      where q.ProductId == id
                                                      select q;
                if (Resimler.Count() != 0)
                {
                    DataList1.DataSource = Resimler;
                    DataList1.DataBind();
                }
                else
                {
                    DataList1.DataSource = null;
                    DataList1.DataBind();
                }
            }

            if (products != null)
            {
                Label1.Text = "<b>" + Resource.lbCategoryOtherProducts + "</b>";
                var matchList = from x in ent.Products
                                where x.ProductCategoryId == products.ProductCategoryId && x.State == true
                                orderby x.Vitrin descending
                                select new
                                           {
                                               x.Name,
                                               x.ProductId,
                                               x.ProductCategoryId
                                           };
                const string digerUrunler = "<ul style='list-style:none; margin:0; padding:0; '>{0}</ul>";
                var urunler = new StringBuilder();
                var urun = new StringBuilder();
                foreach (var item in matchList)
                {
                    urun.AppendFormat(
                        "<li><img src='../../../App_Themes/mainTheme/images/ok.png' style='vertical-align:middle;' /> <a href='{0}'>{1}</a></li>",
                        "urundetay-" + item.ProductId + "-" + UrlMapping.cevir(item.Name),
                        item.Name);
                }

                DigerUrunLink.Text = urunler.AppendFormat(digerUrunler, urun).ToString();
            }

            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
            string pageTitle = site.title;
            MetaGenerate.SetMetaTags(site, Page);

            if (products != null) pageTitle += " - " + products.Name;
            Page.Title = pageTitle;
        }
        else
        {
            Response.Redirect("/m/Products.aspx");
        }
    }
}