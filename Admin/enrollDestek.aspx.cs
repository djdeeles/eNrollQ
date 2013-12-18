using System;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using eNroll.App_Data;

public partial class admin_enrollDestek : Page
{
    private readonly Entities ent = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        ImageButton1.Text = AdminResource.lbSend;
        try
        {
            var itemCount = DropDownList1.Items.Count;
            for (int i = (itemCount - 1); i >= 0; i--)
            {
                DropDownList1.Items.RemoveAt(i);
            }
        }
        catch (Exception)
        {
        }
        var item = new ListItem(AdminResource.lbNormal, "1");
        DropDownList1.Items.Add(item);
        item = new ListItem(AdminResource.lbLow, "0");
        DropDownList1.Items.Add(item);
        item = new ListItem(AdminResource.lbHigh, "2");
        DropDownList1.Items.Add(item);
    }

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbSupport;
    }

    protected void ImageButton1_Click(object sender, EventArgs eventArgs)
    {
        try
        {
            var yanit = new MailMessage();
            yanit.Subject = "Destek Talebi";
            yanit.Body = AdminResource.msgReply;
            var from = new MailAddress("destek@sayajans.com");
            var to = new MailAddress(KullaniciVer());
            var oSmtp = new SmtpClient("localhost", 25);
            yanit.From = from;
            yanit.To.Add(to);
            yanit.IsBodyHtml = true;
            oSmtp.Send(yanit);

            var destek = new MailMessage();
            destek.Subject = "Destek Talebi Alındı";
            destek.Body = ent.Users.First().EMail +
                          " adresinden 1 adet destek talebi alındı. <br/><br/><br/> Önem Derecesi: " +
                          DropDownList1.SelectedValue + "<br/><br/> Konu : &nbsp;&nbsp;" + TextBox1.Text +
                          "<br/><br/> Mesaj :  &nbsp;&nbsp;" + TextBox2.Text;
            ;
            switch (DropDownList1.SelectedValue)
            {
                case "2":
                    destek.Priority = MailPriority.High;
                    break;
                case "1":
                    destek.Priority = MailPriority.Normal;
                    break;
                case "0":
                    destek.Priority = MailPriority.Low;
                    break;
            }
            destek.IsBodyHtml = true;
            destek.From = to;
            destek.To.Add(from);
            oSmtp.Send(destek);
            Label5.Text = AdminResource.msgSendSucces;
            TextBox2.Text = "";
            TextBox1.Text = "";
            DropDownList1.SelectedIndex = 0;
        }
        catch
        {
            Label5.Text = AdminResource.msgSendErr;
        }
    }

    private string KullaniciVer()
    {
        string Kullanici = string.Empty;
        if (HttpContext.Current.User != null)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    string username = HttpContext.Current.User.Identity.Name;
                    Users cust = ent.Users.First(p => p.EMail == username);
                    Kullanici = cust.EMail;
                }
            }
        }
        return Kullanici;
    }
}