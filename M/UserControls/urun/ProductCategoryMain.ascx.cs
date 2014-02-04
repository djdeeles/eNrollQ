using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class M_UserControls_ProductCategoryMain : UserControl
{
    private const string NoImgUrl = "/App_Themes/mainTheme/images/noimage.png";
    private readonly Entities _ent = new Entities();

    private readonly Localizations _localizations = new Localizations();

    protected void Page_Load(object sender, EventArgs e)
    {
        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = _ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title + " - " + Resource.titleProductCategoryMain;
        MetaGenerate.SetMetaTags(site, Page);

        if (!IsPostBack)
        {
            UrunleriGoster(lang);
        }
    }

    protected void DataPager1_Init(object sender, EventArgs e)
    {
        _localizations.ChangeDataPager((DataPager) sender);
    }

    private void UrunleriGoster(int lang)
    {
        ListView1.DataSource =
            _ent.Def_ProductCategories.Where(p => p.languageId == lang && p.State == true && p.ParentCategoryId == null)
                .OrderBy(p => p.Name).ToList();
        ListView1.DataBind();
    }

    public string GetThumbnailPath(string productCategoryId)
    {
        var entities = new Entities();

        string thumbnailPath = NoImgUrl;
        int id = Convert.ToInt32(productCategoryId);
        var urunler = entities.Products.Where(x => x.ProductCategoryId == id).ToList();
        if (urunler.Count > 0)
        {
            int urunId = Convert.ToInt32(urunler.Take(1).First().ProductId);
            Product_Images productImages =
                entities.Product_Images.FirstOrDefault(p => p.ProductId == urunId && p.MainImage == true);
            if (productImages != null && productImages.Thumbnail != null)
            {
                thumbnailPath = productImages.Thumbnail;
            }
        }
        return thumbnailPath.Replace("~/", "../");
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
                        if (Request.Url.AbsolutePath != "/m/ProductCategoryMain.aspx")
                        {
                            if (Request.Url.PathAndQuery.IndexOf("&") != -1)
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(
                                    Request.Url.PathAndQuery + "&", "../../m/urunkategorileri-");
                            }
                            else
                            {
                                currentLink.NavigateUrl = currentLink.NavigateUrl.Replace(Request.Url.PathAndQuery,
                                                                                          "../../urunkategorileri-");
                            }
                        }
                        else
                        {
                            currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("/m/ProductCategoryMain.aspx?",
                                                                                      "/m/urunkategorileri-");
                        }
                        currentLink.NavigateUrl = currentLink.NavigateUrl.Replace("page=", "");
                    }
                }
            }
        }
    }
}