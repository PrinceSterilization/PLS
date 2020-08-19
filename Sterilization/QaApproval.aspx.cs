//QaApproval.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Label Printing and QA Approval page

using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Sterilization
{
    public partial class QaApproval : System.Web.UI.Page
    {

        private string _sortDirection;
        SterilizationDLL st_dll = new SterilizationDLL();
        public string _qauser;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
               
                if (!IsPostBack)
                {                   
                    GetQaDetails();
                    GridColumns();
                    BindQAGrid();                   
                }
                
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }
        
        private void BindQAGrid()
        {
            try
            {


                DataTable dt = new DataTable();
                //SqlCommand cmd = new SqlCommand("dbo.stProductsLabeling", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Type", "QA");
                //SqlDataReader dr = cmd.ExecuteReader();
                //dt.Load(dr);

                dt = st_dll.GetDBData("dbo.stProductsLabeling", "QA");

                if (dt.Rows.Count > 0)
                {
                    ViewState["QADetails"] = dt;
                    grvQaApproval.DataSource = dt;
                    grvQaApproval.DataBind();
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
        private void ShowEmptyGrid()
        {
            grvQaApproval.DataSource = new List<String>();
            grvQaApproval.DataBind();
        }

        protected void grvQaApproval_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["QADetails"];

            SetSortDirection(SortDireaction);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                grvQaApproval.DataSource = dt;
                grvQaApproval.DataBind();
                SortDireaction = _sortDirection;
                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in grvQaApproval.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = grvQaApproval.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }

                //grvProducts.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }


        private void GetQaDetails()
        {

            //SqlCommand cmd = new SqlCommand("dbo.stProductsLabeling", conn);
            //cmd.CommandType = CommandType.StoredProcedure;
            //SqlParameter type = new SqlParameter("@Type", "QD");
            //cmd.Parameters.Add(type);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                DataTable dt = new DataTable();
                try
                {

                    dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewState["EmployeeDetails"] = dt;
                ddqanames.DataSource = dt;
                ddqanames.DataTextField = "EmployeeName";
                ddqanames.DataValueField = "EmployeeID";
                ddqanames.DataBind();
                ddqanames.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }

        protected void btnesign_Click(object sender, EventArgs e)
        {
            if (txtsignature.Text != "" && ddqanames.SelectedValue != "")
            {

                int result = SaveQAsign();
                if (result != 0)
                {
                    
                    SucessMessage("E-Sign is successfull !");
                }
                else {
                    ErrorMessage("E-Sign failed ! Please check user credentials");
                }
                BindQAGrid();

            }
        }
        private int SaveQAsign()
        {
            try
            {
                //SqlCommand cmd = new SqlCommand("dbo.stProductsLabeling", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Type", "QS");
                //cmd.Parameters.AddWithValue("@ApprovedByID", ddqanames.SelectedValue);
                //cmd.Parameters.AddWithValue("@ControlID", hdnControlid.Value);
                //cmd.ExecuteNonQuery();

                if (ddqanames.SelectedValue != null)
                {
                    int result = VerifyEsign(Convert.ToInt32(ddqanames.SelectedValue), txtsignature.Text);
                    if (result != 0)
                    {
                        ProductsEntity pe = new ProductsEntity();
                        pe.ApprovedByID = Convert.ToInt32(ddqanames.SelectedValue);
                        pe.ControlID = Convert.ToInt32(hdnControlid.Value);
                        return st_dll.InsertQASign("dbo.stProductsLabeling", "QS", pe);
                    }
                    else {
                        return 0;
                    }

                }
                else {
                    return 0;

                }

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
            }
        }

        protected void grvQaApproval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Preview")
            {

                //int controlid = Convert.ToInt16(e.CommandArgument);
                //Session["Controlid"] = controlid;
                ////Response.Redirect("Reportpage.aspx");
                ////Server.Transfer("");

                //Page.ClientScript.RegisterStartupScript(
                //this.GetType(), "OpenWindow", "window.open('Reportpage.aspx','_newtab');", true);
            }
            else if (e.CommandName == "Print")
            {
                GridColumns();
                ProductsEntity pe = new ProductsEntity();
                int controlid = Convert.ToInt32(hdnControlid.Value);
                int labelcount = Convert.ToInt32(hdnlabelcount.Value);
                pe.ControlID = controlid;
                pe.categorycode = 1;               
                DataTable dt = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
                if (dt.Rows.Count == 0)
                {
                    GenerateLables(controlid, labelcount, 1);
                    //SendEmailtoQA("Product");
                    
                }
                //Session["ControlID"] = controlid.ToString();
                //Session["labelcount"] = labelcount.ToString();
                //Session["categorycode"] = "1";
                // BarTenderPrintingLable(controlid);
                //Session["Controlid"] = controlid;

                //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenWindow('" + controlid + "','" + labelcount + "','" + 1 + "');", true);

                //Response.Redirect("Printing.aspx",false);
                Response.Redirect("Printing.aspx?ctrid=" + controlid.ToString() + "&lc=" + labelcount.ToString() + "&cc=1", false);

            }
            else if (e.CommandName == "Component")
            {
                GridColumns();
                ProductsEntity pe = new ProductsEntity();
                int controlid = Convert.ToInt32(hdnControlid.Value);
                int labelcount = Convert.ToInt32(hdnlabelcount.Value);
                pe.ControlID = controlid;
                pe.categorycode = 4;
                DataTable dt = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
                if (dt.Rows.Count == 0)
                {
                    GenerateLables(controlid, labelcount, 4);
                    //SendEmailtoQA("Component");

                }
                //Session["ControlID"] = controlid.ToString();
                //Session["labelcount"] = labelcount.ToString();
                //Session["categorycode"] = "4";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenWindow('" + controlid + "','" + labelcount + "','" + 4 + "');", true);
                //Response.Redirect("Printing.aspx", false);
                Response.Redirect("Printing.aspx?ctrid="+ controlid.ToString() + "&lc="+ labelcount.ToString() +"&cc=4", false);
                
            }
            else if (e.CommandName == "Insert")
            {
                //GridColumns();
                //ProductsEntity pe = new ProductsEntity();               
                //int controlid = Convert.ToInt32(hdnControlid.Value);               
                //int labelcount = st_dll.GetLabelCount(controlid);
                //pe.ControlID = controlid;
                //pe.categorycode = 2;
                //DataTable dt = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
                //if (dt.Rows.Count == 0)
                //{
                //    GenerateLables(controlid, labelcount, 2);
                //    int unitsize = st_dll.GetUnitSize(controlid);
                //    if (unitsize != 0) {
                //        st_dll.InsertIntoCaseSize(controlid, 2);
                //    }
                       
                //    GenerateCaseLabels(labelcount);
                   
                                   
                //}
                
                //Response.Redirect("Printing.aspx?ctrid=" + controlid.ToString() + "&lc=" + labelcount.ToString() + "&cc=2", false);
            }
            else if (e.CommandName == "Case")
            {
                GridColumns();
                int controlid = Convert.ToInt32(hdnControlid.Value);
                DataTable dt = st_dll.GetCaseOfData(controlid);
                int labelcount = Convert.ToInt32(Math.Round(Convert.ToDouble(hdnlabelcount.Value) / Convert.ToDouble(dt.Rows[0]["CASESIZE"])));
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenCaseWindow", "OpenCaseModal('" + controlid + "','" + labelcount + "','" + 3 + "');", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenWindow('" + controlid + "','" + labelcount + "','" + 3 + "');", true);
            }
        }
        protected void btnInsertGenerate_Click(object sender, EventArgs e)
        {
            GridColumns();
            ProductsEntity pe = new ProductsEntity();
            int controlid = Convert.ToInt32(hdnControlid.Value);
            //int labelcount = st_dll.GetLabelCount(controlid);
            //Save the No.Of Products in ProdcutCount Table
            st_dll.SaveProductCount(controlid,2, Convert.ToInt32(txtInsertProduct.Text));
            int labelcount = Convert.ToInt32(txtInsertProduct.Text);
            pe.ControlID = controlid;
            pe.categorycode = 2;
            DataTable dt = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
            if (dt.Rows.Count == 0)
            {
                GenerateLables(controlid, labelcount, 2);
                int unitsize = st_dll.GetUnitSize(controlid);
                if (unitsize != 0)
                {
                    st_dll.InsertIntoCaseSize(controlid, 2);
                }

                GenerateCaseLabels(labelcount);


            }

            Response.Redirect("Printing.aspx?ctrid=" + controlid.ToString() + "&lc=" + labelcount.ToString() + "&cc=2", false);

        }
            
        protected void btnCase_Click(object sender, EventArgs e)
        {
            GridColumns();
           // GenerateCaseLabels();
            //SendEmailtoQA("Case");
            
        }

        public void GenerateCaseLabels(int casecount)
        {
           
            ProductsEntity pe = new ProductsEntity();
            int controlid = Convert.ToInt32(hdnControlid.Value);
            DataTable dt1 = st_dll.GetCaseOfData(controlid);            
            int labelcount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(casecount) / Convert.ToDouble(dt1.Rows[0]["CASESIZE"])));
            pe.ControlID = controlid;
            pe.categorycode = 3;
            //st_dll.SaveCaseOfData(txtcase.Text, controlid);
            DataTable dt = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
            if (dt.Rows.Count == 0)
            {
                GenerateLables(controlid, labelcount, 3);
                st_dll.InsertIntoCaseSize(controlid, 3);
            }
           // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenWindow('" + controlid + "','" + labelcount + "','" + 3 + "');", true);
        }
        private void GenerateLables(int controlId, int labelCount, int categorycode)
        {
            if (controlId != 0 && labelCount != 0)
            {
                int CreatedBy = Convert.ToInt32(Session["UserID"]);
                int result = st_dll.GenerateLables("dbo.spGenerateLabels", controlId, labelCount, CreatedBy, categorycode, "IN");
                if (result == 0)
                {
                    ErrorMessage("Failed to load the data !");
                }
            }
            else {
                ErrorMessage("Failed to load the data !");
            }
        }
        public void BarTenderPrintingLable(int id)
        {
            try
            {
                DataTable dt = new DataTable();
                //SqlCommand cmd = new SqlCommand("dbo.spLabelingReportPreview", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ControlID", id);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                dt = st_dll.ReportData("dbo.spLabelingReportPreview", id,4);
                DataRow row = dt.Rows[0];
                using (Engine btEngine = new Engine())
                {
                    btEngine.Start();
                    //btEngine.Window.Visible = true;
                    string btFileName = "";
                    btFileName = @"\\GLWS18\Users\samancha\Documents\BarTender\BarTenderDocuments\Product_Label.btw";
                    //btFileName = @"C:\Users\samancha\Desktop\Product_Label.btw";
                    LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
                    Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
                    queryprompts["controlid"].Value = id.ToString();
                    btFormat.PrintSetup.IdenticalCopiesOfLabel = Convert.ToInt32(dt.Rows[0]["LabelCount"]);

                    Result result = btFormat.Print(btFileName);
                    //Result result = btFormat.Print();

                    if (result == Result.Failure)
                    {
                        //string Message = "Label is printed Failure";
                        //string wsScript = "<script type=\"text/javascript\">document.getElementById('#divfailure').style.display='block';document.getElementById('#divfailure').InnerHtml ='" + Message + "'</script>";
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "validation", wsScript, true);

                        SucessMessage("Label is printed successfully !");

                    }

                    else {
                        //string Message = "Label is printed Successfully";
                        //string wsScript = "<script type=\"text/javascript\">document.getElementById('#divAdded').style.display='block';document.getElementById('#divAdded').InnerHtml ='" + Message + "'</script>";
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "validation", wsScript, true);
                        ErrorMessage("Failed to print the label !");
                    }

                    btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                    btEngine.Stop();
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        private void ErrorMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", "ErrorMessage('" + msg + "');", true);

        }
        private void SucessMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", "SuccessMessage('" + msg + "');", true);
        }
        protected void grvQaApproval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ////6P 7I 8C 2lblc
                    //string assignedby = e.Row.Cells[4].Text;
                    //int product_count = Convert.ToInt32(e.Row.Cells[4].Text);
                    //int insertbox_count = Convert.ToInt32(e.Row.Cells[5].Text);
                    //int case_count = Convert.ToInt32(e.Row.Cells[6].Text);
                    //int totallabel_count = Convert.ToInt32(e.Row.Cells[2].Text);

                    ////generated count labels
                    //// 9p 10I 11c
                    //int p_count = Convert.ToInt32(e.Row.Cells[7].Text);
                    //int i_count = Convert.ToInt32(e.Row.Cells[8].Text);
                    //int c_count = Convert.ToInt32(e.Row.Cells[9].Text);

                    //int product_type = Convert.ToInt32(e.Row.Cells[10].Text);
                    //int component_count = Convert.ToInt32(e.Row.Cells[11].Text);
                    //int cm_count = Convert.ToInt32(e.Row.Cells[12].Text);
                    Button btninsert = (Button)e.Row.FindControl("btnInserBox");
                    Button btnProduct = (Button)e.Row.FindControl("btnGenerate");
                    Button btnComponents = (Button)e.Row.FindControl("btnComponents");

                    int product_type = Convert.ToInt32(e.Row.Cells[17].Text);
                    int label_count = Convert.ToInt32(e.Row.Cells[4].Text);

                    string status = e.Row.Cells[1].Text;

                    if (status == "1")
                    {
                        e.Row.Cells[1].Text = "<span class='label1'>Completed</span>";
                    }
                    else if (status == "2")
                    {
                        e.Row.Cells[1].Text = "<span class='label2'>Pending</span>";

                    }
                    else if (status == "3")
                    {
                        e.Row.Cells[1].Text = "<span class='label3'>Rejected</span>";


                    }

                    if (product_type != 1)
                    {
                        btnComponents.Visible = false;
                        
                    }
                    else {
                        btnComponents.Visible = true;
                        btnProduct.Visible = false;
                    }
                    //Product
                    int product_printed = Convert.ToInt32(e.Row.Cells[5].Text);


                    if (product_printed == label_count)
                    {
                        btnProduct.CssClass = "btn btn-danger";
                        btnProduct.Enabled = false;
                    }

                    //Component

                    int component_printed = Convert.ToInt32(e.Row.Cells[8].Text);


                    if (component_printed == label_count)
                    {
                        btnComponents.CssClass = "btn btn-danger";
                        btnComponents.Enabled = false;
                    }

                    //Insert and Case
                    int insert_printed = Convert.ToInt32(e.Row.Cells[11].Text);
                    int case_printed = Convert.ToInt32(e.Row.Cells[14].Text);
                    int case_total = Convert.ToInt32(e.Row.Cells[18].Text);
                    int _unitsize = Convert.ToInt32(e.Row.Cells[19].Text);
                    //int total_case= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(label_count) / Convert.ToDouble());
                    if (_unitsize != 0)
                    {
                        int i_count = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(label_count) / Convert.ToDouble(_unitsize)));
                        if (insert_printed == i_count && case_printed == case_total)
                        {
                            btninsert.CssClass = "btn btn-danger";
                            btninsert.Enabled = false;
                        }
                    }
                    else {

                        if (insert_printed == label_count && case_printed == case_total)
                        {
                            btninsert.CssClass = "btn btn-danger";
                            btninsert.Enabled = false;
                        }

                    }


                    if (status == "3") {

                        btninsert.CssClass = "btn btn-danger";
                        btninsert.Enabled = false;
                        btnComponents.CssClass = "btn btn-danger";
                        btnComponents.Enabled = false;
                        btnProduct.CssClass = "btn btn-danger";
                        btnProduct.Enabled = false;
                    }






                    //if (_qauser == "Yes")
                    //{
                    //    btnComponents.CssClass = "btn btn-success";
                    //    btnComponents.Enabled = true;

                        //    if (cm_count > 0)
                        //    {
                        //        btnComponents.CssClass = "btn btn-success";
                        //        btnComponents.Enabled = true;

                        //    }
                        //    else {
                        //        btnComponents.CssClass = "btn btn-success";
                        //        btnComponents.Enabled = true;
                        //        btnGenerate.CssClass = "btn btn-danger";
                        //        btnGenerate.Enabled = false;
                        //        btninsert.CssClass = "btn btn-danger";
                        //        btninsert.Enabled = false;
                        //       // btncase.CssClass = "btn btn-danger";
                        //       // btncase.Enabled = false;
                        //    }

                        //    if (product_type != 1)
                        //    {
                        //        btnGenerate.CssClass = "btn btn-success";
                        //        btnGenerate.Enabled = true;
                        //    }
                        //    if (p_count > 0)
                        //    {
                        //        //Button btnsign = (Button)e.Row.FindControl("btnqasign");
                        //        //btnsign.CssClass = "btn btn-primary";
                        //        //btnsign.Enabled = false;
                        //        btnGenerate.CssClass = "btn btn-success";
                        //        btnGenerate.Enabled = true;
                        //        btnComponents.CssClass = "btn btn-danger";
                        //         btnComponents.Enabled = false;
                        //    }
                        //    else {
                        //       // btnComponents.CssClass = "btn btn-danger";
                        //       // btnComponents.Enabled = false;
                        //        btnGenerate.CssClass = "btn btn-danger";
                        //        btnGenerate.Enabled = false;
                        //        btninsert.CssClass = "btn btn-danger";
                        //        btninsert.Enabled = false;
                        //       // btncase.CssClass = "btn btn-danger";
                        //       // btncase.Enabled = false;
                        //    }
                        //    if (i_count > 0 ) 
                        //    {
                        //        if (totallabel_count == insertbox_count )
                        //        {
                        //            btnComponents.CssClass = "btn btn-danger";
                        //            btnComponents.Enabled = false;
                        //            btnGenerate.CssClass = "btn btn-danger";
                        //            btnGenerate.Enabled = false;
                        //            btninsert.CssClass = "btn btn-danger";
                        //            btninsert.Enabled = false;
                        //        }
                        //        else {
                        //            btnComponents.CssClass = "btn btn-danger";
                        //            btnComponents.Enabled = false;
                        //            btnGenerate.CssClass = "btn btn-danger";
                        //            btnGenerate.Enabled = false;
                        //            btninsert.CssClass = "btn btn-success";
                        //            btninsert.Enabled = true;
                        //        }
                        //    }
                        //    else {
                        //        btninsert.CssClass = "btn btn-danger";
                        //        btninsert.Enabled = false;
                        //    }

                        //    //if (c_count > 0)
                        //    //{
                        //    //    if (totallabel_count == insertbox_count)
                        //    //    {
                        //    //        btnComponents.CssClass = "btn btn-danger";
                        //    //        btnComponents.Enabled = false;
                        //    //        btnGenerate.CssClass = "btn btn-danger";
                        //    //        btnGenerate.Enabled = false;
                        //    //        btninsert.CssClass = "btn btn-danger";
                        //    //        btninsert.Enabled = false;
                        //    //        //  btncase.CssClass = "btn btn-success";
                        //    //        //btncase.Enabled = true;
                        //    //    }
                        //    //    else
                        //    //    {
                        //    //        // btncase.CssClass = "btn btn-danger";
                        //    //        // btncase.Enabled = false;

                        //    //    }
                        //    //}
                        //    //else {
                        //    //    // btncase.CssClass = "btn btn-danger";
                        //    //    // btncase.Enabled = false;

                        //    //}
                        //    //if (c_count > 0)
                        //    //{
                        //    //    if (case_count != 0)
                        //    //    {
                        //    //        btninsert.CssClass = "btn btn-danger";
                        //    //        btninsert.Enabled = false;
                        //    //    }
                        //    //}
                        //    //else {
                        //    //    btninsert.CssClass = "btn btn-danger";
                        //    //    btninsert.Enabled = false;
                        //    //}
                        //}
                        //else {


                        //    btnComponents.CssClass = "btn btn-success";
                        //    btnComponents.Enabled = true;

                        //    if (totallabel_count == component_count)
                        //    {
                        //        btnGenerate.CssClass = "btn btn-success";
                        //        btnGenerate.Enabled = true;
                        //        btnComponents.CssClass = "btn btn-danger";
                        //        btnComponents.Enabled = false;

                        //    }
                        //    else {
                        //        btnGenerate.CssClass = "btn btn-danger";
                        //        btnGenerate.Enabled = false;

                        //    }

                        //    if (product_type != 1)
                        //    {
                        //        btnGenerate.CssClass = "btn btn-success";
                        //        btnGenerate.Enabled = true;
                        //    }


                        //    if (totallabel_count == product_count)
                        //    {
                        //        btnGenerate.CssClass = "btn btn-danger";
                        //        btnGenerate.Enabled = false;
                        //        btninsert.CssClass = "btn btn-success";
                        //        btninsert.Enabled = true;
                        //    }
                        //    else {
                        //        btninsert.CssClass = "btn btn-danger";
                        //        btninsert.Enabled = false;
                        //    }
                        //    if (totallabel_count == insertbox_count)
                        //    {
                        //        btnGenerate.CssClass = "btn btn-danger";
                        //        btnGenerate.Enabled = false;
                        //        btninsert.CssClass = "btn btn-danger";
                        //        btninsert.Enabled = false;
                        //        // btncase.CssClass = "btn btn-success";
                        //        // btncase.Enabled = true;
                        //    }
                        //    else
                        //    {
                        //        //btninsert.CssClass = "btn btn-success";
                        //        //btninsert.Enabled = true;


                        //    }


                        //    if (i_count > 0)
                        //    {
                        //        if (case_count != 0)
                        //        {
                        //            btninsert.CssClass = "btn btn-danger";
                        //            btninsert.Enabled = false;
                        //        }
                        //        else {
                        //            btninsert.CssClass = "btn btn-success";
                        //            btninsert.Enabled = true;
                        //        }
                        //    }                      
                        //}
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        protected void btnFilter_Click(object sender, EventArgs e)
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
            DataTable dt = st_dll.ProductSearch("dbo.spProductSearch", pe);

            if (dt.Rows.Count > 0)
            {
                grvQaApproval.DataSource = dt;
                grvQaApproval.DataBind();

            }
            else {
                ErrorMessage("No records found.Please search with different criteria.");
            }


            //if (ViewState["QADetails"] != null)
            //{

            //    DataTable dt = (DataTable)ViewState["QADetails"];
            //    DataView view = new DataView();
            //    string fieldName = ddFilter.SelectedItem.Value;

            //    if (fieldName == "ProductDesc" || fieldName == "LotNo")
            //    {
            //        var query = from t in dt.AsEnumerable()
            //                    where t.Field<string>(fieldName).Contains(txtfilter.Text.ToString())
            //                    select t;
            //        view = query.AsDataView();
            //        grvQaApproval.DataSource = view;
            //        grvQaApproval.DataBind();

            //    }

            //    else if (fieldName == "0")
            //    {
            //        grvQaApproval.DataSource = dt;
            //        grvQaApproval.DataBind();

            //    }
            //}
        }
        protected void btnPrintBatch_Click(object sender, EventArgs e)
        {
            try
            {


                ProductsEntity pe = new ProductsEntity();
                pe.ControlID = Convert.ToInt32(hdnControlid.Value);
                //dbo.spProductLabelPrintBatchDetai\s
                DataTable dt = st_dll.GetDBData("dbo.spProductLabelPrintBatchDetails", "S", pe);

                //XElement xelm = (XElement)dt.Rows[0]["PrintBatches"];
                string xmlelm = dt.Rows[0]["PrintBatches"].ToString();
                string approvedby = dt.Rows[0]["ApprovedByID"].ToString();
                if (approvedby != "")
                {
                    if (xmlelm == "")
                    {
                        XElement Batches =
                        new XElement("Batches",
                            new XElement("Batch",
                                new XElement("PrintCount", txtPrintNumber.Text),
                                new XElement("PrintedByID", Session["UserID"]),
                                new XElement("PrintDate", DateTime.Now.ToString("MM-dd-yyyy h:m:s"))
                                )
                            );
                        pe.PrintBatches = Batches;
                        int result = st_dll.PrintBatchDetails("dbo.spProductLabelPrintBatchDetails", "IN", pe);

                    }
                    else {


                        XElement Batch =
                            new XElement("Batch",
                                    new XElement("PrintCount", txtPrintNumber.Text),
                                    new XElement("PrintedByID", Session["UserID"]),
                                    new XElement("PrintDate", DateTime.Now.ToString("MM-dd-yyyy h:m:s"))
                                    );
                        pe.PrintBatches = Batch;
                        int result = st_dll.PrintBatchDetails("dbo.spProductLabelPrintBatchDetails", "U", pe);

                    }
                    txtPrintNumber.Text = "";
                    BindQAGrid();
                }
                else {
                    ErrorMessage("QA Approval is pending!");
                }
                //Printing to BarTender Machine
               // BarTenderPrintingLable(pe.ControlID);
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
                
                BindQAGrid();
                txtfilter.Text = "";
                ddFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }

        protected void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.ControlID = Convert.ToInt32(hdnControlid.Value);
                pe.AppliedByID = Convert.ToInt32(Session["UserID"]);
                int result = st_dll.PrintBatchDetails("dbo.spProductLabelPrintBatchDetails", "RU", pe);
                if (result == 0)
                {
                    ErrorMessage("Reconcilation failed !");
                }
                else {
                    SucessMessage("Reconcilation is successfull !");
                    BindQAGrid();
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }

        }


        protected void GridColumns()
        {
            try
            {
                if (ViewState["EmployeeDetails"] != null)
                {
                    //DataTable dt = (DataTable)ViewState["EmployeeDetails"];
                    //var query = (from t in dt.AsEnumerable()
                    //             where t.Field<int>("EmployeeID") == Convert.ToInt32(Session["UserID"])
                    //             select t).ToList();                  
                    //if (query.Count > 0)
                    //{
                    //    Session["QAUser"] = "Yes";
                    //    _qauser = Session["QAUser"].ToString();
                    //    string title = "Quality Analyst Approval";
                    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "HeaderTextChage", "HeaderTextChange('" + title + "');", true);
                    //}
                    //else {
                    //    Session["QAUser"] = "";
                    //    _qauser = Session["QAUser"].ToString();
                    //    string title = "Label Printing and Reading";
                    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "HeaderTextChage", "HeaderTextChange('" + title + "');", true);                        
                    //}
                    int result = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));

                    if (result == 1)
                    {
                        Session["QAUser"] = "Yes";
                        _qauser = Session["QAUser"].ToString();
                        string title = "Quality Analyst Approval";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "HeaderTextChage", "HeaderTextChange('" + title + "');", true);
                    }
                    else {
                        Session["QAUser"] = "";
                        _qauser = Session["QAUser"].ToString();
                        string title = "Label Printing";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "HeaderTextChage", "HeaderTextChange('" + title + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }

        protected void grvQaApproval_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell Cell_Header = new TableCell();


                Cell_Header = new TableCell();
                Cell_Header.Text = "";
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 5 ;

                HeaderRow.Cells.Add(Cell_Header);

                Cell_Header = new TableCell();
                if (_qauser == "Yes") {
                    Cell_Header.Text = "Label Approval";
                } else {
                    Cell_Header.Text = "Generate Labels Numbers";
                }
                    
                Cell_Header.HorizontalAlign = HorizontalAlign.Center;
                Cell_Header.ColumnSpan = 6;
                HeaderRow.Cells.Add(Cell_Header);

                grvQaApproval.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }

        protected void SendEmailtoQA(string category)
        {
            try
            {
                DataTable dt_details = st_dll.GetDetails("dbo.spProductDetails", Convert.ToInt32(hdnControlid.Value));
                DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");
                if (dt.Rows.Count > 0)
                {
                    string strbody = "";
                    strbody = "Dear Quality Analyst , " + "<br /><br />";
                    strbody = strbody + "You have "+ category + " Label for E-Signature." + "<br /><br />";
                    strbody = strbody + "Control#: " + dt_details.Rows[0]["ControlID"] + "<br />";
                    strbody = strbody + "Product Description: " + dt_details.Rows[0]["productdesc"] + "<br />";
                    strbody = strbody + "Lot#: " + dt_details.Rows[0]["LotNo"] + "<br /><br />";
                    strbody = strbody + "Please follow the link to login to Sterlization Labeling System";
                    strbody = strbody + "<br/ ><br />";
                    strbody = strbody + "<a href='http://gblnj4:85'>Gibraltar Laboratories Sterlization Login</a>" + "<br /><br />";
                    strbody = strbody + "Thank you." + "<br /><br />";
                    strbody = strbody + " GPLS Alert System" + "<br />";
                    strbody = strbody + " (System generated e-mail)" + "<br />";
                    MailMessage mail = new MailMessage();

                    //for (int j = 0; j < dt.Rows.Count; j++)
                    //{
                    //    mail.To.Add(dt.Rows[j]["COMPANYEMAIL"].ToString());
                    //}

                    //mail.To.Add("cweibel @gibraltarlabsinc.com");
                    //mail.To.Add("shechter@gibraltarlabsinc.com");
                    //mail.To.Add("htarcan @gibraltarlabsinc.com");
                    mail.To.Add("MPannullo@gibraltarlabsinc.com");
                    mail.To.Add("rpuar@gibraltarlabsinc.com");
                    //mail.To.Add("slongo@gibraltarlabsinc.com");
                    //mail.CC.Add("samancha@gibraltarlabsinc.com");
                    //mail.To.Add("samancha@gibraltarlabsinc.com");
                                   
                    mail.From = new MailAddress("gpls@gibraltarlabsinc.com");
                    mail.Subject = "Request to review and approve printing of labels";
                    mail.Body = strbody;
                    mail.IsBodyHtml = true;
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }

        }
       
        protected int VerifyEsign(int id, string pwd)
        {

            //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");
            //var query = from t in dt.AsEnumerable()
            //            where t.Field<string>("esignpassword").Contains(pwd)
            //            && t.Field<int>("EmployeeID") == id
            //            select t;
            //DataView view = new DataView();
            //view = query.AsDataView();
            //if (view.Count > 0)
            int result = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));

            if (result == 1)
            {
                return 1;
            }
            else {
                return 0;

            }
        }
    }
}