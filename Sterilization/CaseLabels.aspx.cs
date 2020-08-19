using Seagull.BarTender.Print;
using Sterilization.DLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class CaseLabels : System.Web.UI.Page
    {
        private string _sortDirection;
        GPLS_DLL st_dll = new GPLS_DLL();
        UsersDLL us_dll = new UsersDLL();

        public int categorycode = 0;
        public string BarTenderStatus = ConfigurationManager.AppSettings["BarTendarPrinter"];
        public string SortDireaction
        {
            get
            {
                if (ViewState["SortDireaction"] == null)
                    return string.Empty;
                else
                    return ViewState["SortDireaction"].ToString();
            }
            set
            {
                ViewState["SortDireaction"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                if (!IsPostBack)
                {
                    CheckUserGroup();
                    GetCaseBoxLabelDetails();
                    GetCaseDetails();
                    GetVoidDetails();
                    GetRejectdDetails();
                }

                DataTable chk_Permission = us_dll.CheckUserAccessLevel(Convert.ToInt32(Session["UserID"]), 2);
                if (chk_Permission.Rows.Count > 0)
                {
                    if (chk_Permission.Rows[0]["ACCESSLEVEL"].ToString() == "FA")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "permission", "permission('FA');", true);
                    }
                    else {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "permission", "permission('RO');", true);
                    }
                }
                if (CheckUserGroup() == 4)
                {

                    // btnAddNewLable.Visible = true;
                    btnVoid.Visible = true;
                    btnPrint.Visible = true;
                    btnRead.Visible = true;
                    btnAddCaseLabels.Visible = true;
                    btnTakeout.Visible = true;
                    btnReturn.Visible = true;


                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "DisableButtons", "disableButtons('QA');", true);
                }
                else {
                    //btnAddNewLable.Visible = false;
                    btnVoid.Visible = false;
                    btnPrint.Visible = true;
                    btnRead.Visible = true;
                    btnAddCaseLabels.Visible = true;
                    btnTakeout.Visible = true;
                    //btnReturn.Visible = false;

                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "DisableButtons", "disableButtons('US');", true);
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }
        private void GetCaseDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = st_dll.GetDataTable("dbo.spGetCaseLabels");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ddlCaseLables.DataSource = dt;
                ddlCaseLables.DataTextField = "PRODUCTDESC";
                ddlCaseLables.DataValueField = "TOTALCOUNT";
                ddlCaseLables.DataBind();
                ddlCaseLables.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        public void GetCaseBoxLabelDetails()
        {
            try
            {
                DataTable dt = st_dll.GetDataTable("dbo.spGetCaseBoxLabels");
                if (dt.Rows.Count > 0)
                {
                    ViewState["InsertLabelingDetails"] = dt;
                    grvCaseLabeling.DataSource = dt;
                    grvCaseLabeling.DataBind();
                }
                else {
                    ShowEmptyGrid();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }

        }
        public int CheckUserGroup()
        {
            try
            {
                int result = st_dll.CheckUserGroup(Convert.ToInt32(Session["UserID"]));
                ViewState["UserGroup"] = result;
                return result;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        private void GetRejectdDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {

                    dt = st_dll.GetDataTable("dbo.spGetRejectdReasons");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //ViewState["EmployeeDetails"] = dt;
                ddlRejectedReasons.DataSource = dt;
                ddlRejectedReasons.DataTextField = "ReasonDesc";
                ddlRejectedReasons.DataValueField = "ReasonID";
                ddlRejectedReasons.DataBind();
                ddlRejectedReasons.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        private void GetVoidDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {

                    dt = st_dll.GetDataTable("dbo.spGetVoidReasons");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //ViewState["EmployeeDetails"] = dt;
                ddlvoidreasons.DataSource = dt;
                ddlvoidreasons.DataTextField = "ReasonDesc";
                ddlvoidreasons.DataValueField = "ReasonID";
                ddlvoidreasons.DataBind();
                ddlvoidreasons.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        private void ShowEmptyGrid()
        {
            grvCaseLabeling.DataSource = new List<String>();
            grvCaseLabeling.DataBind();
        }
        private void ErrorMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", "ErrorMessage('" + msg + "');", true);
        }
        private void SucessMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", "SuccessMessage('" + msg + "');", true);
        }
        protected void grvCaseLabeling_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnStatus = (Button)e.Row.FindControl("btnStatus");
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("ChkSelect");
                string catid = e.Row.Cells[11].Text;
                btnStatus.Enabled = false;
                btnStatus.CssClass = "btn btn-warning pendingclass";
                int toprint = Convert.ToInt32(catid.Split('-')[4]);
                int takenout = Convert.ToInt32(catid.Split('-')[8]);
                int totallabelcount = Convert.ToInt32(catid.Split('-')[3]);
                int totallabelvoided = Convert.ToInt32(catid.Split('-')[7]);
                if (totallabelcount == takenout)
                {
                    btnStatus.Text = "Completed";
                    btnStatus.CssClass = "btn btn-primary";
                    // chkSelect.Enabled = false;
                    if (Convert.ToInt32(ViewState["UserGroup"]) == 4)
                    {
                        chkSelect.Enabled = true;
                    }
                    else {
                        chkSelect.Enabled = false;
                    }
                }
                if (totallabelcount == 0)
                {
                    btnStatus.CssClass = "btn btn-danger";
                    btnStatus.Text = "Voided";
                    chkSelect.Enabled = false;
                    e.Row.Cells[5].Text = totallabelvoided.ToString(); ;
                }

            }
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                string fieldName = ddFilter.SelectedItem.Value;
                switch (fieldName)
                {
                    case "LotNo":
                        pe.LotNo = txtfilter.Text;
                        break;
                    case "ProductDesc":
                        pe.ProductDesc = txtfilter.Text;
                        break;
                    case "LabelCount":
                        pe.labelCount = Convert.ToInt32(txtfilter.Text);
                        break;
                    case "ManufacturingDate":
                        pe.ManufacturingDate = Convert.ToDateTime(txtfilter.Text);
                        break;
                    case "ExpirationDate":
                        pe.ExpirationDate = Convert.ToDateTime(txtfilter.Text);
                        break;
                    default:
                        break;

                }
                DataTable dt = st_dll.ProductSearch("dbo.spCaseLabelSearch", pe);
                if (dt.Rows.Count > 0)
                {
                    grvCaseLabeling.DataSource = dt;
                    grvCaseLabeling.DataBind();
                }
                else {
                    ErrorMessage("No records found.Please search with different criteria.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = (DataTable)ViewState["ProductLabelingDetails"];
                //grvProductLabeling.DataSource = dt;
                //grvProductLabeling.DataBind();
                GetCaseBoxLabelDetails();
                txtfilter.Text = "";
                ddFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        protected void btnAddLabels_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdntxtlabelsToAdd.Value != "0")
                {

                    //GenerateLables(controlId, Convert.ToInt32(hdntxtlabelsToAdd.Value));
                    GenerateCaseLabels(Convert.ToInt32(hdntxtlabelsToAdd.Value), Convert.ToInt32(hdnCheckedBatchID.Value));
                    GetCaseBoxLabelDetails();

                }
                else {
                    ErrorMessage("Dont have option to add lables !");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage("Adding label error " + ex.Message);
            }

        }

        protected void btnvoid_click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("voiding.aspx?batchid=" + hdnCheckedBatchID.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString(), false);
            }
            catch (Exception ex)
            {

                ErrorMessage("Voiding error " + ex.Message);
            }
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                //Response.Redirect("Return.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString(), false);
                Response.Redirect("Return.aspx", false);

            }
            catch (Exception ex)
            {

                ErrorMessage("Return error " + ex.Message);
            }
        }
        protected void btnSplit_Click(object sender, EventArgs e)
        {
            try
            {
                //Response.Redirect("Return.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString(), false);
                Response.Redirect("SplitAndCombineLabels.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&batchid=" + hdnCheckedBatchID.Value.ToString(), false);

            }
            catch (Exception ex)
            {

                ErrorMessage("Return error " + ex.Message);
            }
        }
        protected void btnRejected_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlRejectedReasons.SelectedValue != "0" || ddlRejectedReasons.SelectedValue != "")
                {
                    int reasonid = Convert.ToInt32(ddlRejectedReasons.SelectedValue);
                    int controlid = Convert.ToInt32(hdnCheckedControlid.Value);
                    int categorycode = Convert.ToInt32(hdnCheckedCategoryCode.Value);

                    int result = st_dll.UpdateAllRejected(reasonid, controlid, categorycode, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(hdnCheckedBatchID.Value));
                    if (result == 1)
                    {
                        SucessMessage("Case labels and Insert labels are rejected successfully. Takeout the correct number of Product/Components again from Master Labels.");
                        GetCaseBoxLabelDetails();
                    }
                    else {
                        ErrorMessage("Failed to reject the labels.");
                    }
                }
                else {

                }

            }
            catch (Exception ex)
            {

                ErrorMessage("Rejected error " + ex.Message);
            }
        }
        protected void btnVoided_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlvoidreasons.SelectedValue != "0" || ddlvoidreasons.SelectedValue != "")
                {
                    int reasonid = Convert.ToInt32(ddlvoidreasons.SelectedValue);
                    int controlid = Convert.ToInt32(hdnCheckedControlid.Value);
                    int categorycode = Convert.ToInt32(hdnCheckedCategoryCode.Value);

                    int result = st_dll.UpdateAllVoided(reasonid, controlid, categorycode, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(hdnCheckedBatchID.Value));
                    if (result == 1)
                    {
                        SucessMessage("Labels are voided successfully.");
                        GetCaseBoxLabelDetails();
                    }
                    else {
                        ErrorMessage("Failed to void the labels.");
                    }
                }
                else {

                }

            }
            catch (Exception ex)
            {

                ErrorMessage("Voided error " + ex.Message);
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                GenerateCaseLabels(Convert.ToInt32(hdntxtTotalCount.Value), Convert.ToInt32(ddlCaseLables.SelectedItem.Text.Split('-')[2]));
            }
            catch (Exception ex)
            {

                ErrorMessage("Error in Generation " + ex.Message);
            }
        }
        protected void btnTakeout_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("CaseTakeout.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString() + "&batchid=" + hdnCheckedBatchID.Value.ToString(), false);
            }
            catch (Exception ex)
            {

                ErrorMessage("Takeout error " + ex.Message);
            }
        }
        protected void btnPrintLabels_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.formatname = hdnLabelFormatName.Value.ToString();
                pe.ControlID = Convert.ToInt32(hdnCheckedControlid.Value);
                pe.categorycode = Convert.ToInt32(hdnCheckedCategoryCode.Value);
                pe.labelCount = Convert.ToInt32(txtTotalLabelsremaningtoprint.Text);
                pe.batchid = Convert.ToInt32(hdnCheckedBatchID.Value);
                pe.PrintedByID = Convert.ToInt32(Session["UserID"]);
                pe.PONo = txtPoNo.Text != "" ? txtPoNo.Text : "N/A";
                int po_result = st_dll.UpdatePONo("dbo.spUpdateCasePONo", pe);
                if (po_result != 1)
                {
                    ErrorMessage("Error in Updating PoNo.");
                }

                if (pe.formatname != "")
                {
                    //int result = 1;// BarTenderPrintingLable(pe.ControlID, pe.labelCount, pe.categorycode, pe.formatname,pe.batchid);
                    int result = 0;

                    if (BarTenderStatus == "1")
                    {
                        result = BarTenderPrintingLable(pe.ControlID, pe.labelCount, pe.categorycode, pe.formatname, pe.batchid, 0);
                    }
                    else {
                        result = 1;
                    }

                    if (result == 1)
                    {
                        int updateprint_result = st_dll.UpdatePrinting("dbo.spUpdateCasePrinting", pe);
                        if (updateprint_result == 1)
                        {
                            SucessMessage("The " + pe.labelCount + " labels are printed successfully.");
                            GetCaseBoxLabelDetails();
                        }
                        else {
                            ErrorMessage("Error in updating print status.");
                        }

                        //pe.PONo = txtPoNo.Text != "" ? txtPoNo.Text : "N/A";
                        //int po_result = st_dll.UpdatePONo("dbo.spUpdateCasePONo", pe);
                        //if (po_result != 1)
                        //{
                        //    ErrorMessage("Error in Updating PoNo.");
                        //}
                    }
                    else {
                        ErrorMessage("Error in Bartender.");
                    }
                }
                else {
                    ErrorMessage("Unable to print.Error in getting filename.");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage("Printing error " + ex.Message);
            }
        }
        public int BarTenderPrintingLable(int controlId, int labelcount, int categorycode, string filename, int batchid, int testprint)
        {
            try
            {

                string btFileName = "";
                LogFile lf = new LogFile();
                using (Engine btEngine = new Engine())
                {
                    lf.LogMessge("S1 :" + "Inside Service");
                    btEngine.Start();
                    lf.LogMessge("S2 :" + "Service Started");
                    btEngine.Window.Visible = true;

                    // btFileName = @"\\psapp01\IT Files\PLS\BarTenderFilesTes1\" + filename + ".btw";
                    btFileName = @"\\psapp01\IT Files\PLS\BarTenderFiles\" + filename + ".btw";
                    lf.LogMessge("S4 :" + "File :" + btFileName);
                    LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
                    lf.LogMessge("S3 :" + "File Path");
                    lf.LogMessge("S4 :" + "File :" + btFileName);

                    Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
                    lf.LogMessge("S5 :" + "Go the File");
                    queryprompts["controlid"].Value = controlId.ToString();
                    queryprompts["categorycode"].Value = categorycode.ToString();
                    queryprompts["toprint"].Value = labelcount.ToString();
                    queryprompts["batchid"].Value = batchid.ToString();
                    queryprompts["testprint"].Value = testprint.ToString();
                    lf.LogMessge("S6 :" + "Query Promted");
                    //btFormat.PrintSetup.IdenticalCopiesOfLabel = labelcount;
                    //btFormat.PrintSetup.NumberOfSerializedLabels = 4
                    Result result = btFormat.Print(btFileName);
                    lf.LogMessge("S7 :" + "Result: " + result);
                    //Result result = btFormat.Print();
                    if (result == Result.Failure)
                    {
                        lf.LogMessge("S8 :" + "Failure ");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 0;

                    }
                    else {
                        lf.LogMessge("S9 :" + "Sucess ");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile lf1 = new LogFile();
                lf1.LogMessge("S10 :" + "Exception Date :" + DateTime.Now.ToLongTimeString() + "\nException  Message:" + ex.Message);
                return 0;
            }
        }
        protected void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("readlabel.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString() + "&batchid=" + hdnCheckedBatchID.Value.ToString(), false);
            }
            catch (Exception ex)
            {


                ErrorMessage("Reading error " + ex.Message);
            }
        }
        public void GenerateCaseLabels(int labelcount, int batchid)
        {
            try
            {
                int totallabelcount = 0;
                //Getting UnitSize if exist.
                //OLD CODE WITH UNIT SIZE FROM PRODUCT MASTER FILE              
                ////DataTable dt = st_dll.GetCaseSize(batchid);
                ////if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["CASESIZE"]) > 0)
                ////{
                ////    totallabelcount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(labelcount) / Convert.ToDouble(dt.Rows[0]["CASESIZE"])));

                ////}
                ////else {
                ////    totallabelcount = labelcount;
                ////}
                if (txtCaseBoxSize.Text != "")
                {
                    if (Convert.ToInt32(txtCaseBoxSize.Text) > 0)
                    {
                        totallabelcount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(labelcount) / Convert.ToInt32(txtCaseBoxSize.Text)));

                    }
                }
                else {
                    totallabelcount = labelcount;
                }

                string result;
                if (hdnCheckedControlid.Value != "")
                {
                    result = st_dll.GenerateCaseLabels(labelcount, batchid, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(hdnCheckedControlid.Value), Convert.ToInt32(hdnCheckedCategoryCode.Value));
                }
                else {
                    result = st_dll.GenerateCaseLabels(totallabelcount, batchid, Convert.ToInt32(Session["UserID"]));
                }

                if (result != "" || result != null)
                {
                    if (hdnCheckedControlid.Value != "")
                    {
                        SucessMessage("Total " + labelcount + " labels are generated for " + CategoryDesc(4));
                        //st_dll.UpdateUnitSizeInCaseLabelSizeWhenVoided(batchid);
                    }
                    else {
                        SucessMessage("Total " + totallabelcount + " labels are generated for " + CategoryDesc(4));

                    }
                    DataTable dt = st_dll.GetInsertLabelSize(Convert.ToInt32(result.Split('-')[1]), 4, batchid);

                    if (dt.Rows.Count > 0)
                    {
                        //INSERTING VARIABLE INSERBOX SIZE IN LABELSIZE TABLE
                        st_dll.InsertLabelSizes(Convert.ToInt32(result.Split('-')[1]), 4, batchid, Convert.ToInt32(dt.Rows[0]["SIZE"]), Convert.ToInt32(Session["UserID"]));

                        st_dll.InsertCaseSize(batchid, Convert.ToInt32(dt.Rows[0]["SIZE"]));
                        st_dll.UpdateUnitSizeInCaseLabelSize(batchid, Convert.ToInt32(dt.Rows[0]["SIZE"]));
                    }
                    else {
                        // if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["CASESIZE"]) > 0)
                        if (Convert.ToInt32(txtCaseBoxSize.Text) >= 1)
                        {
                            //INSERTING VARIABLE INSERBOX SIZE IN LABELSIZE TABLE
                            st_dll.InsertLabelSizes(Convert.ToInt32(result.Split('-')[1]), 4, batchid, Convert.ToInt32(txtCaseBoxSize.Text), Convert.ToInt32(Session["UserID"]));

                            st_dll.InsertCaseSize(batchid, Convert.ToInt32(txtCaseBoxSize.Text));
                            st_dll.UpdateUnitSizeInCaseLabelSize(batchid, Convert.ToInt32(txtCaseBoxSize.Text));

                        }

                    }

                    GetCaseBoxLabelDetails();
                    GetCaseDetails();
                }
                else {
                    ErrorMessage("Error in generating labels");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string CategoryDesc(int code)
        {

            switch (code)
            {
                case 4:
                    return "Case";
                default:
                    return "";
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Controlid"] = Convert.ToInt32(hdnCheckedControlid.Value);
                Session["BatchID"] = Convert.ToInt32(hdnCheckedBatchID.Value);
                Page.ClientScript.RegisterStartupScript(
            this.GetType(), "OpenWindow", String.Format("<script>window.open('Reportpage.aspx?catid={0}','_newtab');</script>", Convert.ToInt32(hdnCheckedCategoryCode.Value)));

            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        protected void SetSortDirection(string sortDirection)
        {
            if (sortDirection == "ASC")
            {
                _sortDirection = "DESC";
                //sortImage.ImageUrl = "view_sort_ascending.png";

            }
            else
            {
                _sortDirection = "ASC";
                //sortImage.ImageUrl = "view_sort_descending.png";
            }
        }
        protected void grvCaseLabeling_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["ProductLabelingDetails"];
                SetSortDirection(SortDireaction);
                if (dt != null)
                {
                    //Sort the data.
                    dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                    grvCaseLabeling.DataSource = dt;
                    grvCaseLabeling.DataBind();
                    SortDireaction = _sortDirection;
                    int columnIndex = 0;
                    foreach (DataControlFieldHeaderCell headerCell in grvCaseLabeling.HeaderRow.Cells)
                    {
                        if (headerCell.ContainingField.SortExpression == e.SortExpression)
                        {
                            columnIndex = grvCaseLabeling.HeaderRow.Cells.GetCellIndex(headerCell);
                        }
                    }
                    //grvProducts.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        protected void btnTestPrintLabels_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.formatname = hdnLabelFormatName.Value.ToString();
                pe.ControlID = Convert.ToInt32(hdnCheckedControlid.Value);
                pe.categorycode = Convert.ToInt32(hdnCheckedCategoryCode.Value);
                pe.labelCount = 1;
                pe.batchid = Convert.ToInt32(hdnCheckedBatchID.Value);
                pe.PrintedByID = Convert.ToInt32(Session["UserID"]);
                pe.PONo = txtPoNo.Text != "" ? txtPoNo.Text : "N/A";
                //int po_result = st_dll.UpdatePONo("dbo.spUpdateCasePONo", pe);
                //if (po_result != 1)
                //{
                //    ErrorMessage("Error in Updating PoNo.");
                //}

                if (pe.formatname != "")
                {
                    //int result = 1;// BarTenderPrintingLable(pe.ControlID, pe.labelCount, pe.categorycode, pe.formatname,pe.batchid);
                    int result = 0;

                    if (BarTenderStatus == "1")
                    {
                        result = BarTenderPrintingLable(pe.ControlID, pe.labelCount, pe.categorycode, pe.formatname, pe.batchid, 1);
                    }
                    else {
                        result = 1;
                    }


                }
                else {
                    ErrorMessage("Unable to print.Error in getting filename.");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage("Printing error " + ex.Message);
            }
        }
    }
}