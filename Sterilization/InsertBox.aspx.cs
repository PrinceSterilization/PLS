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
    public partial class InsertBox : System.Web.UI.Page
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
                    GetInsertBoxLabelDetails();
                    GetInsertDetails();
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
                    btnAddInsertLabels.Visible = true;


                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "DisableButtons", "disableButtons('QA');", true);
                }
                else {
                    //btnAddNewLable.Visible = false;
                    btnVoid.Visible = false;
                    btnPrint.Visible = true;
                    btnRead.Visible = true;
                    btnAddInsertLabels.Visible = true;

                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "DisableButtons", "disableButtons('US');", true);
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }
        private void GetInsertDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {
                    dt = st_dll.GetDataTable("dbo.spGetInsertLabels");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ddlInsertLables.DataSource = dt;
                ddlInsertLables.DataTextField = "PRODUCTDESC";
                ddlInsertLables.DataValueField = "TOTALCOUNT";
                ddlInsertLables.DataBind();
                ddlInsertLables.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        public void GetInsertBoxLabelDetails()
        {
            try
            {
                DataTable dt = st_dll.GetDataTable("dbo.spGetInsertBoxLabels");
                if (dt.Rows.Count > 0)
                {
                    ViewState["InsertLabelingDetails"] = dt;
                    grvInsertLabeling.DataSource = dt;
                    grvInsertLabeling.DataBind();
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
            grvInsertLabeling.DataSource = new List<String>();
            grvInsertLabeling.DataBind();
        }
        private void ErrorMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", "ErrorMessage('" + msg + "');", true);
        }
        private void SucessMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", "SuccessMessage('" + msg + "');", true);
        }
        protected void grvInsertLabeling_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnStatus = (Button)e.Row.FindControl("btnStatus");
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("ChkSelect");
                string catid = e.Row.Cells[11].Text;
                btnStatus.Enabled = false;
                btnStatus.CssClass = "btn btn-warning pendingclass";
                int toprint = Convert.ToInt32(catid.Split('-')[4]);
                int toread = Convert.ToInt32(catid.Split('-')[5]);
                int totallabelcount = Convert.ToInt32(catid.Split('-')[3]);
                int totallabelvoided = Convert.ToInt32(catid.Split('-')[7]);
                if (toprint == 0 && toread == 0)
                {
                    btnStatus.Text = "Completed";
                    btnStatus.CssClass = "btn btn-primary";
                    // Giving the Full options to QA Void,Add the labels-09/27/2017- w.r.t damage labels discussion.
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
                    e.Row.Cells[5].Text = totallabelvoided.ToString();
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
                DataTable dt = st_dll.ProductSearch("dbo.spInsertLabelSearch", pe);
                if (dt.Rows.Count > 0)
                {
                    grvInsertLabeling.DataSource = dt;
                    grvInsertLabeling.DataBind();
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
                GetInsertBoxLabelDetails();
                txtfilter.Text = "";
                ddFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
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
                        SucessMessage("Labels are rejected successfully.");
                        GetInsertBoxLabelDetails();
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
        protected void btnAddLabels_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdntxtlabelsToAdd.Value != "0")
                {

                    //GenerateLables(controlId, Convert.ToInt32(hdntxtlabelsToAdd.Value));
                    GenerateInsertLabels(Convert.ToInt32(hdntxtlabelsToAdd.Value), Convert.ToInt32(hdnCheckedBatchID.Value));
                    GetInsertBoxLabelDetails();

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
                        GetInsertBoxLabelDetails();
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
                GenerateInsertLabels(Convert.ToInt32(hdntxtTotalCount.Value), Convert.ToInt32(ddlInsertLables.SelectedItem.Text.Split('-')[2]));
            }
            catch (Exception ex)
            {

                ErrorMessage("Error in Generation " + ex.Message);
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


                if (pe.formatname != "")
                {
                    ///int result = 1;// BarTenderPrintingLable(pe.ControlID, pe.labelCount, pe.categorycode, pe.formatname, pe.batchid);
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
                        int updateprint_result = st_dll.UpdatePrinting("dbo.spUpdateInsertBoxPrinting", pe);
                        if (updateprint_result == 1)
                        {
                            SucessMessage("The " + pe.labelCount + " labels are printed successfully.");
                            GetInsertBoxLabelDetails();
                        }
                        else {
                            ErrorMessage("Error in updating print status.");
                        }

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
                //LogFile lf = new LogFile();
                using (Engine btEngine = new Engine())
                {
                    // lf.LogMessge("S1 :" + "Inside Service");
                    btEngine.Start();
                    //lf.LogMessge("S2 :" + "Service Started");
                    btEngine.Window.Visible = true;


                    //btFileName = @"\\psapp01\IT Files\PLS\BarTenderFilesTes1\" + filename + ".btw";
                    btFileName = @"\\psapp01\IT Files\PLS\BarTenderFiles\" + filename + ".btw";

                    // lf.LogMessge("S3 :" + "File Path");
                    // lf.LogMessge("S4 :" + "File :"+ btFileName);
                    LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
                    // lf.LogMessge("S5 :" + "Go the File");
                    Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
                    queryprompts["controlid"].Value = controlId.ToString();
                    queryprompts["categorycode"].Value = categorycode.ToString();
                    queryprompts["toprint"].Value = labelcount.ToString();
                    queryprompts["batchid"].Value = batchid.ToString();
                    queryprompts["testprint"].Value = testprint.ToString();
                    // lf.LogMessge("S6 :" + "Query Promted");
                    //btFormat.PrintSetup.IdenticalCopiesOfLabel = labelcount;
                    //btFormat.PrintSetup.NumberOfSerializedLabels = 4
                    Result result = btFormat.Print(btFileName);
                    //lf.LogMessge("S7 :" + "Result: "+ result);
                    //Result result = btFormat.Print();
                    if (result == Result.Failure)
                    {
                        // lf.LogMessge("S8 :" + "Failure ");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 0;

                    }
                    else {
                        // lf.LogMessge("S9 :" + "Sucess ");
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
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "log", "log('" + ex.Message + "');", true);
                return 0;
            }
            //try
            //{

            //    string btFileName = "";
            //    using (Engine btEngine = new Engine())
            //    {
            //        btEngine.Start();
            //        btEngine.Window.Visible = true;
            //        btFileName = @"\\glpdc01\corp\IT Files\GPLS\BarTenderFiles\" + filename + ".btw";

            //        LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
            //        Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
            //        queryprompts["controlid"].Value = controlId.ToString();
            //        queryprompts["categorycode"].Value = categorycode.ToString();
            //        queryprompts["toprint"].Value = labelcount.ToString();
            //        queryprompts["batchid"].Value = batchid.ToString();

            //        //btFormat.PrintSetup.IdenticalCopiesOfLabel = labelcount;
            //        //btFormat.PrintSetup.NumberOfSerializedLabels = 4
            //        Result result = btFormat.Print(btFileName);
            //        //Result result = btFormat.Print();
            //        if (result == Result.Failure)
            //        {
            //            btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
            //            btEngine.Stop();
            //            btEngine.Dispose();
            //            return 0;

            //        }
            //        else {
            //            btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
            //            btEngine.Stop();
            //            btEngine.Dispose();
            //            return 1;
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    return 0;
            //}
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
        public void GenerateInsertLabels(int labelcount, int batchid)
        {
            try
            {
                int totallabelcount = 0;
                //Getting UnitSize if exist.
                //OLD CODE WITH UNIT SIZE FROM PRODUCT MASTER FILE                
                ////DataTable dt = st_dll.GetUnitSize(batchid);
                ////if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["UNITSIZE"]) > 0)
                ////{
                ////    totallabelcount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(labelcount) / Convert.ToDouble(dt.Rows[0]["UNITSIZE"])));

                ////}
                ////else {
                ////    totallabelcount = labelcount;
                ////}
                //int checkcontrol = Convert.ToInt32(hdnCheckedControlid.Value);

                if (txtInsertBoxSize.Text != "")
                {
                    if (Convert.ToInt32(txtInsertBoxSize.Text) > 0)
                    {
                        totallabelcount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(labelcount) / Convert.ToInt32(txtInsertBoxSize.Text)));

                    }
                }
                else {
                    totallabelcount = labelcount;
                }
                //Generation of Insert Labels.                                                
                string result;
                if (hdnCheckedControlid.Value != "")
                {
                    result = st_dll.GenerateInsertLabels(labelcount, batchid, Convert.ToInt32(Session["UserID"]), 0, Convert.ToInt32(hdnCheckedControlid.Value), Convert.ToInt32(hdnCheckedCategoryCode.Value));
                }
                else {
                    result = st_dll.GenerateInsertLabels(totallabelcount, batchid, Convert.ToInt32(Session["UserID"]), Convert.ToInt32(txtInsertBoxSize.Text));
                }

                if (result != "" || result != null)
                {
                    if (hdnCheckedControlid.Value != "")
                    {
                        SucessMessage("Total " + labelcount + " labels are generated for " + CategoryDesc(3));
                    }
                    else {
                        SucessMessage("Total " + totallabelcount + " labels are generated for " + CategoryDesc(3));
                    }
                    DataTable dt = st_dll.GetInsertLabelSize(Convert.ToInt32(result.Split('-')[1]), 3, batchid);

                    if (dt.Rows.Count > 0)
                    {
                        //INSERTING VARIABLE INSERBOX SIZE IN LABELSIZE TABLE
                        st_dll.InsertLabelSizes(Convert.ToInt32(result.Split('-')[1]), 3, batchid, Convert.ToInt32(dt.Rows[0]["SIZE"]), Convert.ToInt32(Session["UserID"]));

                        //CALCULATING NO. OF LABELS TO GENERATE BASED ON VARIABLE UNIT SIZE
                        st_dll.InsertIntoCaseSize(batchid, Convert.ToInt32(dt.Rows[0]["SIZE"]));
                    }
                    else {
                        if (Convert.ToInt32(txtInsertBoxSize.Text) > 1)
                        {
                            //INSERTING VARIABLE INSERBOX SIZE IN LABELSIZE TABLE
                            st_dll.InsertLabelSizes(Convert.ToInt32(result.Split('-')[1]), 3, batchid, Convert.ToInt32(txtInsertBoxSize.Text), Convert.ToInt32(Session["UserID"]));

                            //CALCULATING NO. OF LABELS TO GENERATE BASED ON VARIABLE UNIT SIZE
                            st_dll.InsertIntoCaseSize(batchid, Convert.ToInt32(txtInsertBoxSize.Text));
                        }

                    }
                    //if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["UNITSIZE"]) > 0)

                    GetInsertBoxLabelDetails();
                    GetInsertDetails();
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
                case 3:
                    return "Insert Box";
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
        protected void grvInsertLabeling_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["ProductLabelingDetails"];
                SetSortDirection(SortDireaction);
                if (dt != null)
                {
                    //Sort the data.
                    dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                    grvInsertLabeling.DataSource = dt;
                    grvInsertLabeling.DataBind();
                    SortDireaction = _sortDirection;
                    int columnIndex = 0;
                    foreach (DataControlFieldHeaderCell headerCell in grvInsertLabeling.HeaderRow.Cells)
                    {
                        if (headerCell.ContainingField.SortExpression == e.SortExpression)
                        {
                            columnIndex = grvInsertLabeling.HeaderRow.Cells.GetCellIndex(headerCell);
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

        protected void btnAvoidInserts_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnCheckedBatchID.Value != null && hdnControlID.Value != null)
                {
                    //int size = Convert.ToInt32(txtAvoidSize.Text);
                    int result = st_dll.AvoidInsertLabels(Convert.ToInt32(hdnCheckedControlid.Value), Convert.ToInt32(hdnCheckedBatchID.Value), Convert.ToInt32(Session["UserID"]));
                    if (result == 1)
                    {
                        SucessMessage("Insert labels are avoided successfully.");
                        GetInsertBoxLabelDetails();
                    }
                    else {
                        ErrorMessage("Problem in avoiding insert labels.");
                    }
                }
                else {
                    ErrorMessage("Please select  insert label.");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.Status = 1;
                DataTable dt = st_dll.ProductSearch("dbo.spInsertLabelSearch", pe);
                if (dt.Rows.Count > 0)
                {
                    grvInsertLabeling.DataSource = dt;
                    grvInsertLabeling.DataBind();
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

        protected void btnPending_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.Status = 2;
                DataTable dt = st_dll.ProductSearch("dbo.spInsertLabelSearch", pe);
                if (dt.Rows.Count > 0)
                {
                    grvInsertLabeling.DataSource = dt;
                    grvInsertLabeling.DataBind();
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


                if (pe.formatname != "")
                {

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
                    ErrorMessage("Unable to print test file.Error in getting filename.");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage("Printing error " + ex.Message);
            }
        }
    }
}