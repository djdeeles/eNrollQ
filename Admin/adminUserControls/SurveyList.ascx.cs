using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enroll.Managers;
using Resources;
using eNroll.App_Data;
using eNroll.Helpers;

public partial class Admin_adminUserControls_SurveyList : UserControl
{
    private readonly Entities oEntities = new Entities();

    protected override void OnInit(EventArgs e)
    {
        Session["currentPath"] = AdminResource.lbSurveyManagement;
        EntityDataSource1.Where = " it.languageId=" + EnrollAdminContext.Current.DataLanguage.LanguageId.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (RoleControl.YetkiAlaniKontrol(HttpContext.Current.User.Identity.Name, 17))
        {
            mvAuth.ActiveViewIndex = 0;
        }
        else
        {
            mvAuth.ActiveViewIndex = 1;
        }

        BtnNew.Text = AdminResource.lbNewSurvey;
        gVSurveyList.Columns[0].HeaderText = AdminResource.lbActions;
        gVSurveyList.Columns[1].HeaderText = AdminResource.lbSurvey;
        gVSurveyList.Columns[2].HeaderText = AdminResource.lbState;
        gVSurveyList.Columns[3].HeaderText = AdminResource.lbOptions;

        btnSurveySaveUpdate.Text = AdminResource.lbSave;
        btnSurveyEditCancel.Text = AdminResource.lbCancel;
        chkState.Text = AdminResource.lbActive;

        btnSaveUpdateSurveyOption.Text = AdminResource.lbSave;
        btnEditCancelSurveyOption.Text = AdminResource.lbCancel;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gr;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var myMastDelete = (ImageButton) e.Row.FindControl("imgBtnDelete");
                myMastDelete.OnClientClick = " return confirm('" + AdminResource.lbDeletingQuestion + "'); ";

                var myButton = (Button) e.Row.FindControl("imgBtnInnerNew");
                myButton.Text = AdminResource.lbNewSurveyOption;


                gr = ((GridView) e.Row.FindControl("gVSurveyOption"));

                gr.Columns[0].HeaderText = AdminResource.lbActions;
                gr.Columns[1].HeaderText = AdminResource.lbOptions;
                gr.Columns[2].HeaderText = AdminResource.lbResult;

                EntityDataSource2.Where = "it.surveyId=" + myButton.CommandArgument;
                gr.DataSource = EntityDataSource2;
                gr.DataBind();
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void DeletingProccess(string id)
    {
        try
        {
            var oEntities = new Entities();
            Survey oSurvey = oEntities.Survey.Where("it.surveyId=" + id).FirstOrDefault();
            foreach (Survey_Option option in oEntities.Survey_Option.Where("it.surveyId=" + id))
            {
                oEntities.DeleteObject(option);
            }
            oEntities.DeleteObject(oSurvey);
            Logger.Add(17, 1, oSurvey.surveyId, 2);
            oEntities.SaveChanges();

            gVSurveyList.DataBind();
            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void imgBtnSurveyDelete_Click(object sender, ImageClickEventArgs e)
    {
        var myButton = (ImageButton) sender;
        DeletingProccess(myButton.CommandArgument);
    }

    protected void grdSecenek_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var myButInnerDelete = (ImageButton) e.Row.FindControl("imgBtnInerDelete");
                myButInnerDelete.OnClientClick = "return confirm('" + AdminResource.lbDeletingQuestion + "'); ";
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void imgBtnInerDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var myImage = (ImageButton) sender;
            var oEntities = new Entities();
            var optionId = Convert.ToInt32(myImage.CommandArgument);
            Survey_Option oOption =
                oEntities.Survey_Option.Where("it.surveyOptionId=" + optionId).FirstOrDefault();
            oEntities.DeleteObject(oOption);
            if (oOption != null) Logger.Add(17, 2, oOption.surveyOptionId, 2);
            oEntities.SaveChanges();
            gVSurveyList.DataBind();

            MessageBox.Show(MessageType.Success, AdminResource.msgDeleted);
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }

    protected void btnSurveySaveUpdateClick(object sender, EventArgs e)
    {
        Survey oSurvey = null;
        try
        {
            if (string.IsNullOrWhiteSpace(hfSurveyId.Value))
            {
                oSurvey = new Survey();
                oSurvey.CreatedTime = DateTime.Now;
                SetDataProccess(oSurvey);
                oSurvey.System_language =
                    oEntities.System_language.Where("it.languageId=" +
                                                    EnrollAdminContext.Current.DataLanguage.LanguageId)
                        .FirstOrDefault();
                oEntities.SaveChanges();
                Logger.Add(17, 1, oSurvey.surveyId, 1);

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
            }
            else
            {
                int surveyId = Convert.ToInt32(hfSurveyId.Value);
                oSurvey = oEntities.Survey.First(p => p.surveyId == surveyId);
                SetDataProccess(oSurvey);
                oEntities.SaveChanges();
                Logger.Add(17, 1, oSurvey.surveyId, 3);

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }

            gVSurveyList.DataBind();

            pnlList.Visible = true;
            pnlSurveyEdit.Visible = false;
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
        ClearFormInputs();
    }

    protected void btnSurveyEditCancelClick(object sender, EventArgs e)
    {
        pnlSurveyEdit.Visible = false;
        pnlList.Visible = true;

        ClearFormInputs();
    }

    protected void imgBtnSurveyEdit_Click(object sender, ImageClickEventArgs e)
    {
        pnlList.Visible = false;
        pnlSurveyEdit.Visible = true;

        var btnSurveyEdit = sender as ImageButton;
        if (btnSurveyEdit != null)
        {
            hfSurveyId.Value = btnSurveyEdit.CommandArgument;
            int surveyId = Convert.ToInt32(btnSurveyEdit.CommandArgument);
            Survey oSurvey = oEntities.Survey.First(p => p.surveyId == surveyId);
            GetDataProccess(oSurvey);
        }
    }

    private void GetDataProccess(Survey survey)
    {
        txtQuestion.Text = survey.question;
        chkState.Checked = Convert.ToBoolean(survey.state);
    }

    private void SetDataProccess(Survey survey)
    {
        survey.question = txtQuestion.Text;
        survey.state = chkState.Checked;
        survey.languageId = EnrollAdminContext.Current.DataLanguage.LanguageId;
        survey.UpdatedTime = DateTime.Now;
    }

    public void ClearFormInputs()
    {
        txtQuestion.Text = string.Empty;
        chkState.Checked = false;

        txtOption.Text = string.Empty;
        txtChooseCount.Text = string.Empty;

        hfSurveyId.Value = null;
        hfSurveyOptionId.Value = null;
    }

    protected void btnNewSurvey(object sender, EventArgs e)
    {
        pnlSurveyEdit.Visible = true;
        pnlList.Visible = false;
    }

    protected void btnSaveUpdateSurveyOption_Click(object sender, EventArgs e)
    {
        try
        {
            Survey_Option oOption = null;
            if (!string.IsNullOrWhiteSpace(hfSurveyOptionId.Value))
            {
                int serveyOptionId = Convert.ToInt32(hfSurveyOptionId.Value);
                oOption = oEntities.Survey_Option.First(p => p.surveyOptionId == serveyOptionId);
                SetDataProccess_SurveyOption(oOption);
                oEntities.SaveChanges();

                Logger.Add(17, 2, oOption.surveyOptionId, 3);

                MessageBox.Show(MessageType.Success, AdminResource.msgUpdated);
            }
            else
            {
                oOption = new Survey_Option();
                oOption.CreatedTime = DateTime.Now;
                SetDataProccess_SurveyOption(oOption);
                oEntities.AddToSurvey_Option(oOption);
                oEntities.SaveChanges();

                int surveyId = Convert.ToInt32(hfSurveyId.Value);
                var oSurvey = oEntities.Survey.First(p => p.surveyId == surveyId);

                oOption.Survey = oSurvey;
                oEntities.SaveChanges();

                MessageBox.Show(MessageType.Success, AdminResource.msgSaved);
                Logger.Add(17, 2, oOption.surveyOptionId, 1);
            }

            gVSurveyList.DataBind();
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }

        pnlSurveyOptionEdit.Visible = false;
        pnlList.Visible = true;

        ClearFormInputs();
    }

    protected void btnEditCancelSurveyOption_Click(object sender, EventArgs e)
    {
        pnlSurveyOptionEdit.Visible = false;
        pnlList.Visible = true;

        ClearFormInputs();
    }

    protected void imgBtnInerEditSurveyOption_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            var btnEditSurveyOption = sender as ImageButton;
            if (btnEditSurveyOption != null)
            {
                hfSurveyOptionId.Value = btnEditSurveyOption.CommandArgument;
                int optionId = Convert.ToInt32(btnEditSurveyOption.CommandArgument);

                Survey_Option surveyOption = oEntities.Survey_Option.First(p => p.surveyOptionId == optionId);
                GetDataProccess_SurveyOption(surveyOption);

                pnlList.Visible = false;
                pnlSurveyOptionEdit.Visible = true;
            }
        }
        catch (Exception exception)
        {
            ExceptionManager.ManageException(exception);
        }
    }


    private void GetDataProccess_SurveyOption(Survey_Option option)
    {
        txtOption.Text = option.surveyOption;
        if (option.chooseCount.ToString() == string.Empty)
        {
            txtChooseCount.Text = "0";
        }
        else
        {
            txtChooseCount.Text = option.chooseCount.ToString();
        }
    }

    private void SetDataProccess_SurveyOption(Survey_Option option)
    {
        option.surveyOption = txtOption.Text;
        if (txtChooseCount.Text == string.Empty)
            option.chooseCount = 0;
        else
            option.chooseCount = Convert.ToInt32(txtChooseCount.Text);
        option.UpdatedTime = DateTime.Now;
    }

    protected void btnNewSurveyOption(object sender, EventArgs e)
    {
        var btnNewSurvey = sender as Button;
        hfSurveyId.Value = btnNewSurvey.CommandArgument;
        pnlList.Visible = false;
        pnlSurveyOptionEdit.Visible = true;
    }
}