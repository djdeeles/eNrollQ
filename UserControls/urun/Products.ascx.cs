using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class UserControls_Products : UserControl
{
    private const string NoImgUrl = "App_Themes/mainTheme/images/noimage.png";
    private readonly Entities _ent = new Entities();

    private readonly Localizations _localizations = new Localizations();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string qs = Request.QueryString["cat"];
            if (!string.IsNullOrEmpty(qs))
            {
                try
                {
                    int categoryId = Convert.ToInt32(qs);
                    hfCategoryId.Value = categoryId.ToString();
                    UrunleriGoster(categoryId);

                    AltKategorileriListele(categoryId);
                }
                catch (Exception)
                {
                }
            }
        }
    }

    private void AltKategorileriListele(int catId)
    {
        var entities = new Entities();
        var subCategories =
            entities.Def_ProductCategories.Where(
                p =>
                (p.ParentCategoryId == catId && p.State.Value &&
                 p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId)).ToList();
        ltSubCategories.Text = subCategories[0].Name;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("<ul style='margin:0 15px 0 0;padding:0;list-style:none;'>");
        foreach (var categories in subCategories)
        {
            stringBuilder.AppendFormat("<li><a href='urunler-{0}-1'>" +
                                       "<img src='App_Themes/mainTheme/images/ok.png' style='vertical-align:middle;' />{1}</a>" +
                                       "</li>", categories.ProductCategoryId, categories.Name);
        }
        stringBuilder.AppendLine("</ul>");

        if (subCategories.Count() > 0)
        {
            ltSubCategories.Text = stringBuilder.ToString();
        }
    }

    protected void DataPager1_Init(object sender, EventArgs e)
    {
        _localizations.ChangeDataPager((DataPager) sender);
    }

    private void UrunleriGoster(int categoryId)
    {
        Def_ProductCategories cat = categoryId > 0
                                        ? _ent.Def_ProductCategories.FirstOrDefault(
                                            p => p.ProductCategoryId == categoryId)
                                        : _ent.Def_ProductCategories.FirstOrDefault();
        if (cat != null && cat.State != true)
            Response.Redirect("~/404.aspx");
        if (cat != null) Label1.Text = cat.Name;
        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = _ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) if (cat != null) Page.Title = site.title + " - " + cat.Name;
        MetaGenerate.SetMetaTags(site, Page);

        ListView1.DataSource = _ent.Products.Where(
            p => p.ProductCategoryId == cat.ProductCategoryId && p.languageId == lang && p.State == true)
            .OrderByDescending(p => p.Vitrin)
            .ToList();
        ListView1.DataBind();
    }

    public string GetThumbnailPath(string productId)
    {
        string thumbnailPath;
        int id = Convert.ToInt32(productId);
        Product_Images productImages = _ent.Product_Images.FirstOrDefault(p => p.ProductId == id && p.MainImage == true);
        if (productImages != null && productImages.Thumbnail != null)
        {
            thumbnailPath = productImages.Thumbnail;
        }
        else
        {
            thumbnailPath = NoImgUrl;
        }
        return thumbnailPath.Replace("~/", "");
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        DataPager1.PreRender += DataPager1_PreRender;
    }

    private void DataPager1_PreRender(object sender, EventArgs e)
    {
        foreach (Control control in DataPager1.Controls)
        {
            foreach (Control c in control.Controls)
            {
                if (c is HyperLink)
                {
                    var currentLink = (HyperLink) c;
                    if ((!string.IsNullOrEmpty(Request.Url.AbsolutePath)) && (!string.IsNullOrEmpty(Request.Url.Query)))
                    {
                        if (Request.Url.AbsolutePath != "/Products.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../urunler-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../urunler-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/Products.aspx?",
                                                                                      "../../urunler-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("&page=", "-");
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("cat=", "");
                    }
                }
            }
        }
    }


    protected object GetPrice(int productId)
    {
        string result = string.Empty;
        var products = _ent.Products.FirstOrDefault(p => p.ProductId == productId);
        if (products != null && products.Price != null)
        {
            var currency = _ent.Currency.FirstOrDefault(p => p.Id == products.Currency);
            if (currency != null)
            {
                var builder = new StringBuilder();
                var price = decimal.Round(Convert.ToDecimal(products.Price), 2).ToString();
                var symbol = currency.Symbol;

                if (currency.Position.Trim(' ') == "R")
                {
                    result = builder.AppendFormat("<br/><b>{0}</b> {1} {2}", Resource.lbPrice, price, symbol).ToString();
                }
                else
                {
                    result = builder.AppendFormat("<br/><b>{0}</b> {2} {1}", Resource.lbPrice, price, symbol).ToString();
                }
            }
        }
        return result;
    }
}