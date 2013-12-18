using System;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

namespace eNroll
{
    public partial class Survey : UserControl
    {
        private readonly SqlConnection con = new SqlConnection(
            ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AnketVer();
            }
            var enroll = new Entities();
            SiteGeneralInfo info =
                enroll.SiteGeneralInfo.Where("it.System_language.languageId=" +
                                             EnrollContext.Current.WorkingLanguage.LanguageId).First();
            anketBaslik.Text = Resource.lbsurveyHeader;
        }

        private void AnketVer()
        {
            try
            {
                var com = new SqlCommand(
                    "select top 1 surveyId, question from Survey where languageId=@languageId and state=@state order by newid()",
                    con);
                com.Parameters.AddWithValue("@languageId", EnrollContext.Current.WorkingLanguage.LanguageId);
                com.Parameters.AddWithValue("@state", true);
                var dap = new SqlDataAdapter(com);
                var tablo = new DataTable();
                dap.Fill(tablo);
                if (tablo.Rows.Count != 0)
                {
                    HiddenField1.Value = tablo.Rows[0]["question"].ToString();
                    lblSoru.Text = tablo.Rows[0]["question"].ToString();
                    HiddenField2.Value = tablo.Rows[0]["surveyId"].ToString();
                    CevaplariVer(Convert.ToInt32(tablo.Rows[0]["surveyId"].ToString()));
                }
                else
                {
                    HiddenField1.Value = string.Empty;
                    HiddenField2.Value = string.Empty;
                }
            }
            catch
            {
                // hata
                HiddenField1.Value = string.Empty;
                HiddenField2.Value = string.Empty;
            }
            finally
            {
                con.Close();
            }
        }

        private void AnketVer(int AnketId)
        {
            try
            {
                var com = new SqlCommand(
                    "select surveyId, question from Survey where state=@state and surveyId=@surveyId", con);
                com.Parameters.AddWithValue("@state", true);
                com.Parameters.AddWithValue("@surveyId", AnketId);
                var dap = new SqlDataAdapter(com);
                var tablo = new DataTable();
                dap.Fill(tablo);
                if (tablo.Rows.Count != 0)
                {
                    HiddenField1.Value = tablo.Rows[0]["question"].ToString();
                    lblSoru.Text = tablo.Rows[0]["question"].ToString();
                    HiddenField2.Value = tablo.Rows[0]["surveyId"].ToString();
                    CevaplariVer(Convert.ToInt32(tablo.Rows[0]["surveyId"].ToString()));
                }
                else
                {
                    HiddenField1.Value = string.Empty;
                    HiddenField2.Value = string.Empty;
                }
            }
            catch
            {
                // hata
                HiddenField1.Value = string.Empty;
                HiddenField2.Value = string.Empty;
            }
            finally
            {
                con.Close();
            }
        }

        private void CevaplariVer(int AnketId)
        {
            try
            {
                var com = new SqlCommand(
                    "select sum(chooseCount) from Survey_Option where surveyId=@surveyId", con);
                com.Parameters.AddWithValue("@surveyId", AnketId);
                con.Open();
                hdnToplam.Value = com.ExecuteScalar().ToString();
                EntityDataSource1.Where = "it.Survey.surveyId=" + AnketId;
                GridView1.DataBind();
                HttpCookie myCookie = Request.Cookies["eNroll_anket_" + AnketId];
                if (myCookie != null)
                {
                    Panel4.Visible = true;
                    RadioButtonList1.Visible = false;
                }
                else
                {
                    Panel4.Visible = false;
                    RadioButtonList1.Visible = true;
                }
            }
            catch
            {
                // hata
                GridView1.DataSource = string.Empty;
                GridView1.DataBind();
            }
            finally
            {
                con.Close();
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var enroll = new Entities();
            if (Convert.ToInt32(RadioButtonList1.SelectedValue) > 0)
            {
                Survey_Option myOption =
                    enroll.Survey_Option.Where("it.surveyOptionId=@paramOptionId",
                                               new ObjectParameter("paramOptionId",
                                                                   Convert.ToDecimal(RadioButtonList1.SelectedValue)))
                        .First();
                myOption.chooseCount = myOption.chooseCount + 1;
                enroll.SaveChanges();
                var myCookie = new HttpCookie("eNroll_anket_" + HiddenField2.Value);
                myCookie["anketId"] = HiddenField2.Value;
                myCookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(myCookie);
                AnketVer(Convert.ToInt32(HiddenField2.Value));
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                var lblOran = (Label) row.FindControl("lblOran");
                Double oran = Convert.ToDouble(lblOran.Text);
                Int32 toplamSonuc = Convert.ToInt32(hdnToplam.Value);
                lblOran.Text = "%" + Math.Round((oran*100)/toplamSonuc).ToString();
            }
        }
    }
}