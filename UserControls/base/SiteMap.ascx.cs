using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using eNroll.App_Data;

public partial class UserControls_base_SiteMap : UserControl
{
    private readonly SqlConnection con = new SqlConnection(
        ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ConnectionString);

    private readonly DataTable dt = new DataTable();
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        TreeView1.Nodes[0].Text =
            ent.SiteGeneralInfo.Where(p => p.languageId == EnrollContext.Current.WorkingLanguage.LanguageId)
                .FirstOrDefault()
                .title;
        TreeView1.Nodes[0].NavigateUrl = "../../#";
        GetSiteMapByLanguageId(EnrollContext.Current.WorkingLanguage.LanguageId);

        int lang = EnrollContext.Current.WorkingLanguage.LanguageId;
        SiteGeneralInfo site = ent.SiteGeneralInfo.FirstOrDefault(p => p.languageId == lang);
        if (site != null) Page.Title = site.title;
        MetaGenerate.SetMetaTags(site, Page);
    }

    private void GetSiteMapByLanguageId(int DilId)
    {
        var da = new SqlDataAdapter(
            "select * from System_menu where languageId=" + DilId + " and location!=2 and state=1 order by location",
            con);
        da.Fill(dt);
        kategori(TreeView1.Nodes[0], getrows("0"));
    }

    private DataRowCollection getrows(string id)
    {
        var dw = new DataView(dt);
        dw.RowFilter = "MasterId=" + id;
        return dw.ToTable().Rows;
    }

    private void kategori(TreeNode node, DataRowCollection col)
    {
        foreach (DataRow row in col)
        {
            var tn = new TreeNode(row[1].ToString());
            string type = row["type"].ToString();
            switch (type)
            {
                case "0": //yanlızca başlık
                    tn.NavigateUrl = "../../#";
                    break;
                case "1": //url
                    if (row["url"].ToString().StartsWith("http"))
                    {
                        tn.NavigateUrl = row["url"].ToString();
                    }
                    else if (row["url"].ToString().StartsWith("www"))
                    {
                        tn.NavigateUrl = "http://" + row["url"];
                    }
                    else
                    {
                        tn.NavigateUrl = "/" + row["url"];
                    }
                    break;
                case "2": //sayfa
                    tn.NavigateUrl = "../../sayfa-" + row["MenuId"] + "-" + UrlMapping.cevir(row["name"].ToString());
                    break;
                case "3": //yeni sekmede url
                    if (row["url"].ToString().StartsWith("http"))
                    {
                        tn.NavigateUrl = row["url"].ToString();
                        tn.Target = "_blank";
                    }
                    else if (row["url"].ToString().StartsWith("www"))
                    {
                        tn.NavigateUrl = "http://" + row["url"];
                        tn.Target = "_blank";
                    }
                    else
                    {
                        tn.NavigateUrl = "/" + row["url"];
                        tn.Target = "_blank";
                    }
                    break;
            }
            node.ChildNodes.Add(tn);
            kategori(tn, getrows(row[0].ToString()));
        }
    }
}