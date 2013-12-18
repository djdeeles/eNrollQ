using System;
using System.Linq;
using System.Web.UI;
using Resources;
using eNroll.App_Data;

public partial class UserControls_EmailAdd : UserControl
{
    private readonly Entities oEntities = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MultiView1.ActiveViewIndex = 0;
            tbName.Text = Resource.lbName;
            tbSurname.Text = Resource.lbSurname;
            TextBoxEmail.Text = Resource.lbEmail;
        }
        RequiredFieldValidator1.ErrorMessage = Resource.errEmailRequired;
        RegularExpressionValidator1.ErrorMessage = Resource.errEmail;
        ImageButtonEkle.ImageUrl = Resource.phEmailAdd;

        hfNameResource.Value = Resource.lbName;
        hfSurnameResource.Value = Resource.lbSurname;
        hfEmailResource.Value = Resource.lbEmail;
    }

    protected void ImageButtonEkle_Click(object sender, ImageClickEventArgs e)
    {
        var email = TextBoxEmail.Text.Trim(' ');
        var checkEmail = oEntities.EmailList.Where(p => p.email == email).ToList();
        if (checkEmail.Count == 0)
        {
            var Email = new EmailList();
            Email.email = email;
            Email.Name = tbName.Text;
            Email.Surname = tbSurname.Text;
            Email.CreatedTime = DateTime.Now;
            Email.UpdatedTime = DateTime.Now;
            Email.guid = Guid.NewGuid();
            oEntities.AddToEmailList(Email);
            oEntities.SaveChanges();
            MultiView1.ActiveViewIndex = 1;
        }
    }
}