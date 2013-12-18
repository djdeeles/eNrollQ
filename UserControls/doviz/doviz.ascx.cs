using System;
using System.Data;
using System.Web.UI;
using System.Xml;

public partial class doviz : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        KurlariGetir();
    }

    private void KurlariGetir()
    {
        var dt = new DataTable();
        dt.Columns.Add("isim");
        dt.Columns.Add("Birimi");
        dt.Columns.Add("Alis");
        dt.Columns.Add("Satis");

        var xmlReader = new XmlTextReader("http://www.tcmb.gov.tr/kurlar/today.xml");
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(xmlReader);
        XmlNode topNode = xmlDocument.DocumentElement;
        XmlNode xnDolarIsim = xmlDocument.SelectSingleNode("//Tarih_Date//Currency[Kod='USD']|//Isim");
        XmlNode xnDolarAlis = xmlDocument.SelectSingleNode("//Tarih_Date//Currency[Kod='USD']|//ForexBuying");

        XmlNode xnDolarSatis = xmlDocument.SelectSingleNode("//Tarih_Date//Currency[Kod='USD']|//ForexSelling");
        XmlNode euro = topNode.SelectSingleNode("Currency[CurrencyName='EURO']");

        string xnEuroIsim = euro.ChildNodes[1].InnerText;
        string xnEuroAlis = euro.ChildNodes[3].InnerText;
        string xnEuroSatis = euro.ChildNodes[4].InnerText;

//DataRow dr = dt.NewRow();
//dr[0] = xnDolarIsim.InnerText;
//dr[1] = "USD";
//dr[2] = xnDolarAlis.InnerText;
//dr[3] = xnDolarSatis.InnerText;
//dt.Rows.Add(dr);
        Label1.Text = xnDolarAlis.InnerText;
        Label2.Text = xnDolarSatis.InnerText;
        Label3.Text = xnEuroAlis;
        Label4.Text = xnEuroSatis;

//DataRow dr2 = dt.NewRow();
//dr2[0] = xnEuroIsim;
//dr2[1] = "EUR";
//dr2[2] = xnEuroAlis;
//dr2[3] = xnEuroSatis;
//dt.Rows.Add(dr2);
//GridView1.DataSource = dt;
//GridView1.DataBind(); 
    }
}