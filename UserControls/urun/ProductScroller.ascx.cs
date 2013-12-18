using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using Resources;
using eNroll.App_Data;

public partial class UserControls_ProductScroller : UserControl
{
    private readonly Entities ent = new Entities();
    private readonly StringBuilder stringBuilder = new StringBuilder();
    private string _noImgUrl = "App_Themes/mainTheme/images/noimage.png";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
            IQueryable<Products> products =
                ent.Products.Where(p => p.languageId == lang && p.State == true && p.Vitrin == true);
            foreach (Products product in products)
            {
                Products product1 = product;
                IQueryable<Product_Images> pictures =
                    ent.Product_Images.Where(p => p.MainImage == true && p.ProductId == product1.ProductId);

                stringBuilder.AppendLine("<li> <div class='productscrolleritem'>");
                stringBuilder.AppendFormat("<div class='urunbaslik'>{0}</div>", product.Name);
                if (pictures.Any())
                {
                    Product_Images mainPic = pictures.First();

                    stringBuilder.AppendFormat("<div class='urunIMG'>" +
                                               "<a href='urundetay-{0}-{3}'>" +
                                               "<img src='{1}' alt='{2}' width='120px' height='150px'/></a></div>"
                                               , product.ProductId, mainPic.Thumbnail.Replace("~", ""), product.Name,
                                               UrlMapping.cevir(product.Name));
                }
                else
                {
                    stringBuilder.AppendFormat("<div class='urunIMG'>" +
                                               "<a href='urundetay-{0}-{3}'>" +
                                               "<img src='{1}' alt='{2}' width='120px' height='150px'/></a></div>"
                                               , product.ProductId, _noImgUrl.Replace("~", ""), product.Name,
                                               UrlMapping.cevir(product.Name));
                }

                stringBuilder.AppendFormat("<div class='urundevam'><br/>" +
                                           "<a href='urundetay-{0}-{3}'>" +
                                           "<img src='{1}' alt='{2}'/></a></div>" +
                                           "</div></li>"
                                           , product.ProductId, Resource.imgDetails.Replace("~", ""),
                                           product.Name, UrlMapping.cevir(product.Name));
            }
            Literal1.Text = stringBuilder.ToString();
        }
        catch (Exception)
        {
        }
    }
}