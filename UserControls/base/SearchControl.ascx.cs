using System;
using System.Web.UI;
using Enroll.WebParts;
using Resources;

public partial class UserControls_SearchControl : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            searchText.Text = Resource.lbSearchInSite;
        }
        hfSearchInSiteResource.Value = Resource.lbSearchInSite;
        hfSearchWordMinLength.Value = Resource.lbSearchWordMinLength;
    }

    protected void ibSearchText_Click(object sender, ImageClickEventArgs e)
    {
        if ((searchText.Text != string.Empty) ||
            (searchText.Text != Resource.lbSearchInSite || (searchText.Text != null)))
        {
            string aranacakKelime = EnrollSearch.QueryStringeCevir(searchText.Text); 
            if (aranacakKelime.Length > 2)
            {
                Response.Redirect("ara-" + aranacakKelime, false);
            }
            else
            {
                MessageBox.Show(MessageType.jAlert, Resource.lbSearchWordMinLength);
            }
        }
        else
        {
            MessageBox.Show(MessageType.jAlert, Resource.lbSearchWordMinLength);
        }
    }
}