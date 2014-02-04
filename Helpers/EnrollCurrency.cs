using System;
using System.Linq;
using System.Xml;
using eNroll.App_Data;

/// <summary>
/// 	Summary description for GetCurrency
/// </summary>
public class EnrollCurrency
{
    private static readonly Entities _entities = new Entities();
    private readonly double avroalis;
    private readonly double avrosatis;
    private readonly double caprazkur;
    private readonly String date = string.Empty;
    private readonly double usdalis;
    private readonly double usdsatis;

    public EnrollCurrency()
    {
        var oDoc = new XmlDocument();

        oDoc.Load("http://www.tcmb.gov.tr/kurlar/today.xml");

        XmlNode oDateNode = oDoc.FirstChild.NextSibling.NextSibling;

        date = oDateNode.Attributes["Tarih"].Value;
        usdalis =
            Convert.ToDouble(
                oDateNode.SelectSingleNode("Currency[@Kod='USD']")
                    .SelectSingleNode("ForexBuying")
                    .InnerText.Replace(".", ","));
        usdsatis =
            Convert.ToDouble(
                oDateNode.SelectSingleNode("Currency[@Kod='USD']")
                    .SelectSingleNode("ForexSelling")
                    .InnerText.Replace(".", ","));
        avroalis =
            Convert.ToDouble(
                oDateNode.SelectSingleNode("Currency[@Kod='EUR']")
                    .SelectSingleNode("ForexBuying")
                    .InnerText.Replace(".", ","));
        avrosatis =
            Convert.ToDouble(
                oDateNode.SelectSingleNode("Currency[@Kod='EUR']")
                    .SelectSingleNode("ForexSelling")
                    .InnerText.Replace(".", ","));
        caprazkur =
            Convert.ToDouble(
                oDateNode.SelectSingleNode("Currency[@Kod='EUR']")
                    .SelectSingleNode("CrossRateOther")
                    .InnerText.Replace(".", ","));
    }

    public Double UsdAlis
    {
        get { return usdalis; }
    }

    public Double UsdSatis
    {
        get { return usdsatis; }
    }

    public Double AvroAlis
    {
        get { return avroalis; }
    }

    public Double AvroSatis
    {
        get { return avrosatis; }
    }

    public Double CaprazKur
    {
        get { return caprazkur; }
    }

    public String Date
    {
        get { return date; }
    }

    public static EnrollCurrency GetCurrency()
    {
        return new EnrollCurrency();
    }

    public static Currency SiteDefaultCurrency()
    {
        var currency = _entities.Currency.FirstOrDefault(p => p.Active);
        return currency ?? _entities.Currency.FirstOrDefault();
    }

    public static string SiteDefaultCurrencyUnit()
    {
        var currency = _entities.Currency.FirstOrDefault(p => p.Active);
        if (currency != null) return currency.Symbol;
        return string.Empty;
    }
}