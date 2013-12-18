using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Enroll.BaseObjects;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;

public partial class UserControls_MenuDynamicList : MenuControlBase
{
    //protected List<Customer_Dynamic> dynamicGroupList;
    private readonly SqlConnection con = new SqlConnection(
        ConfigurationManager.ConnectionStrings["eNrollConnectionString"].ConnectionString);

    protected string Title = string.Empty;
    private Entities _entities = new Entities();

    protected void Page_Load(object sender, EventArgs e)
    {
        //dynamicGroupList = _entities.Customer_Dynamic.ToList();
    }

    public void EnrollInit()
    {
        try
        {
            var oMenuControlBase = (MenuControlBase) Parent;
            OMenuObject = oMenuControlBase.OMenuObject;
            MultiView1.ActiveViewIndex = 0;
            String selectCommand =
                "select Customer_Dynamic.dynamicId as Id,Customer_Dynamic.[name] as [Name] From " +
                "Customer_Dynamic_Reference inner join Customer_Dynamic on " +
                "Customer_Dynamic_Reference.DynamicId=Customer_Dynamic.dynamicId " +
                "where Customer_Dynamic_Reference.MenuId=" + OMenuObject.menuId + " order by [Name]";


            var oAdaptor = new SqlDataAdapter(selectCommand, con);
            var oDataTable = new DataTable();

            con.Open();
            oAdaptor.Fill(oDataTable);
            con.Close();


            Title = OMenuObject.dynaDisplayText;


            switch (OMenuObject.dynaDisplayType.ToString())
            {
                case "1": //combo
                    MultiView1.ActiveViewIndex = 0;
                    foreach (DataRow row in oDataTable.Rows)
                    {
                        ddlDynamicList.Items.Insert(0,
                                                    new ListItem(Resource.lbChoose,
                                                                 Resource.lbChoose));
                        var item = new ListItem(
                            row["Name"].ToString(), row["Id"] + "-" + UrlMapping.cevir(row["Name"].ToString()));
                        ddlDynamicList.Items.Add(item);
                    }
                    break;
                case "2": //grid
                    MultiView1.ActiveViewIndex = 1;
                    DataListMenuDynamic.DataSource = oDataTable;
                    DataListMenuDynamic.DataBind();
                    break;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }


    protected void ddlDynamicList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDynamicList.SelectedValue != Resource.lbChoose)
        {
            Response.Redirect("dinamik-" + ddlDynamicList.SelectedValue, false);
        }
    }

    protected void DataListMenuDynamic_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        var myLink = (LinkButton) e.Item.FindControl("LinkButton1");
        myLink.PostBackUrl = "dinamik-" + myLink.CommandArgument + "-" + UrlMapping.cevir(myLink.Text);
    }
}