using System.Web.UI.WebControls;
using Resources;

/// <summary>
///   Summary description for Localizations
/// </summary>
public class Localizations
{
    public void ChangeDataPager(DataPager dataPager)
    {
        var field1 = (NextPreviousPagerField) dataPager.Fields[0];
        field1.FirstPageText = Resource.first;
        field1.LastPageText = Resource.last;
        field1.NextPageText = Resource.next;
        field1.PreviousPageText = Resource.prev;
        field1.ButtonCssClass = "button";

        var fieldNum = (NumericPagerField) dataPager.Fields[1];
        fieldNum.NumericButtonCssClass = "button";
        fieldNum.CurrentPageLabelCssClass = "buttonactive";
        fieldNum.NextPreviousButtonCssClass = "button";
        fieldNum.NextPageText = "»";
        fieldNum.PreviousPageText = "«";


        var field2 = (NextPreviousPagerField) dataPager.Fields[2];
        field2.FirstPageText = Resource.first;
        field2.LastPageText = Resource.last;
        field2.NextPageText = Resource.next;
        field2.PreviousPageText = Resource.prev;
        field2.ButtonCssClass = "button";
    }
}