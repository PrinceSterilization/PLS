//Printing.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Label Printing page


using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class Printing : System.Web.UI.Page
    {
        SterilizationDLL st_dll = new SterilizationDLL();
        int controlId = 0;
        int labelCount = 0;
        int categorycode = 0;
        public string _qauser;

        //categorycode = 1 - Product Label
        //categorycode = 2 - Insert Box Labels
        //categorycode = 3 - Case Labels
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {

                if (Request.QueryString["ctrid"] != null)
                {
                    controlId = Convert.ToInt32(Request.QueryString["ctrid"]);
                    hdnControlid.Value = controlId.ToString();
                }
                if (Request.QueryString["lc"] != null)
                {
                    labelCount = Convert.ToInt32(Request.QueryString["lc"]);
                    hdnlabelcount.Value = labelCount.ToString();
                }
                if (Request.QueryString["cc"] != null)
                {
                    categorycode = Convert.ToInt32(Request.QueryString["cc"]);
                    hdncategorycode.Value = categorycode.ToString();
                }


                // Getting QA Users                
                GetQaDetails();
                //Getting the Rejected Reasons 
                GetRejectedDetails();
                //Disabling the buttons based on QA or User
                DisableButtonsOnUser();

                if (!IsPostBack)
                {

                    BindGrid(1, 100);
                    BindLabelFormats();
                }
                AddpagingButton();
                //Label data Binding to Grid

                // Getting the Header Text based on QA or User    
                ChangeName(categorycode);

                CheckQAEsign(controlId, categorycode);
                //Enabeling the QA-Esign Button based on Preview-Intial Load
                //CheckPreview(controlId, categorycode);

                if (categorycode == 1 || categorycode == 4)
                {
                    btnCase.Enabled = false;
                }
                if (categorycode == 3)
                {
                    btnCase.Visible = false;
                    // GenerateCaseLabels(controlId, categorycode);
                }

                //Enabling Case Labels button when labels are printed and read
                if (categorycode == 2)
                {
                    CheckAtleastOneLabelIsPrinted(controlId, categorycode);
                    btnQAEsign.Enabled = false;
                }
                //DiablePrintTotext(controlId, categorycode);

                //DisableAddNewLabel(controlId, categorycode);

                CheckLabelPrint(controlId, categorycode);

                // CheckLabelRead(controlId, categorycode);
            }
            else {
                Response.Redirect("Login.aspx");
            }
            //string parameter = Request["__EVENTARGUMENT"];
            //if (parameter == "ReadLabel")
            //{
            //    //Response.Redirect("readlabel.aspx?cid" + controlId + "&lbc=" + labelCount + "&cat=" + categorycode, false);
            //}
        }

        public void BindLabelFormats() {
            try
            {
               
                DataTable dt = st_dll.GetLabelFormatsByProduct("dbo.spGetLabelFormatsByProduct",controlId, categorycode);
                ddlLabelFormats.DataSource = dt;
                ddlLabelFormats.DataTextField = "FormatNo";
                ddlLabelFormats.DataValueField = "FileName";
                ddlLabelFormats.DataBind();
                ddlLabelFormats.Items.Insert(0, new ListItem("--Select Label Format--", "0"));
            }
            catch (Exception ex)
            {
                ErrorMessage("Label Format dropdown binding error:1 " + ex.Message);
            }

        }
        public void CheckAddNewLabel(int controlId, int categorycode)
        {
            int dt = st_dll.CheckAddNewLabel(controlId, categorycode);
            if (dt == 0)
            {
                btnaddnew.Enabled = true;
            }
            else {
                btnaddnew.Enabled = false;
            }
        }
        public void DisableAddNewLabel(int controlId, int categorycode)
        {
            DataTable dt = st_dll.GetProductType("dbo.spCheckVoidedlabels", controlId);

            if (dt.Rows.Count > 0)
            {

                btnaddnew.Enabled = true;
            }
            else {
                btnaddnew.Enabled = false;
            }
        }
        //public void DiablePrintTotext(int controlId, int categorycode) {
        //    DataTable Product_dt = st_dll.GetProductType("dbo.spGetProductType", controlId);

        //    int producttype = Convert.ToInt32(Product_dt.Rows[0]["PRODUCTTYPE"]);

        //    if (producttype == 1)
        //    {
        //        if (categorycode == 1 || categorycode == 2 || categorycode == 3)
        //        {
        //            txtTotalLabelsremaningtoprint.ReadOnly = true;
        //        }
        //        else {
        //            txtTotalLabelsremaningtoprint.ReadOnly = false;
        //        }
        //    }
        //    else {

        //        if (categorycode == 2 || categorycode == 3)
        //        {
        //            txtTotalLabelsremaningtoprint.ReadOnly = true;
        //        }
        //        else {
        //            txtTotalLabelsremaningtoprint.ReadOnly = false;
        //        }
        //    }
        //}
        public void GenerateCaseLabels(int controlId, int categorycode)
        {

            ProductsEntity pe = new ProductsEntity();

            DataTable dt1 = st_dll.GetCaseOfData(controlId);
            pe.ControlID = controlId;
            pe.categorycode = 3;

            DataTable dt = st_dll.GetData("dbo.spGetLabelToPrintForCase", pe);
            int labelcount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dt.Rows[0]["TOTAL_LABELS_TO_PRINT"]) / Convert.ToDouble(dt1.Rows[0]["CASESIZE"])));

            //st_dll.SaveCaseOfData(txtcase.Text, controlid);
            //DataTable dt2 = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
            if (Convert.ToInt32(dt.Rows[0]["TOTAL_LABELS_TO_PRINT"]) > 0)
            {
                GenerateLables(controlId, labelcount, 3);
            }
            BindGrid(1, 100);
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
        public void CheckAtleastOneLabelIsPrinted(int controlId, int categorycode)
        {

            int dt = st_dll.GetLabelStatus("dbo.spCheckLabel", "ILP", controlId, categorycode); ;
            if (dt == 0)
            {
                btnCase.Enabled = false;
            }
            else {
                btnCase.Enabled = true;
            }
        }
        public void CheckLabelPrint(int controlId, int categorycode)
        {

            int dt = st_dll.GetLabelStatus("dbo.spCheckLabel", "CP", controlId, categorycode); ;
            if (dt == 0)
            {
                btnReading.Enabled = false;
            }
            else {
                btnReading.Enabled = true;
                //btnPrintRequired.Enabled = false;
            }
        }
        public int CheckQAEsign(int controlId, int categorycode)
        {

            int dt = st_dll.GetLabelStatus("dbo.spCheckLabel", "SC", controlId, categorycode); ;
            if (dt == 0)
            {
                btnQAEsign.Enabled = false;
                // btnCase.Enabled = false;
                return dt;
            }
            else {
                //if (_qauser == "Yes") {
                //    SucessMessage("QA E-Sign is Successful !");
                //}                
                btnQAEsign.Enabled = true;
                // btnCase.Enabled = true;
                return dt;
            }

        }
        //public void CheckPreview(int controlId, int categorycode)
        //{

        //    int dt = st_dll.GetLabelStatus("dbo.spCheckLabel", "GP", controlId, categorycode);

        //    if (categorycode == 2 || categorycode == 3)
        //    {
        //        if (dt == 0)
        //        {
        //            //btnPrintRequired.Enabled = false;
        //            btnReading.Enabled = false;
        //            btnCase.Enabled = false;
        //        }
        //        else {
        //           // btnPrintRequired.Enabled = true;

        //        }
        //    }

        //    else {
        //        int esign = CheckQAEsign(controlId, categorycode);
        //        if (dt == 0)
        //        {

        //            if (esign == 0) {
        //                btnQAEsign.Enabled = false;
        //            }
        //            else {
        //                btnQAEsign.Enabled = true;
        //            }

        //            // btnCase.Enabled = false;
        //        }
        //        else {
        //            if (esign == 0)
        //            {
        //                btnQAEsign.Enabled = true;
        //            }
        //            else {
        //                btnQAEsign.Enabled = false;
        //            }
        //            //btnCase.Enabled = true;
        //        }
        //    }

        //}
        private void ChangeName(int categorycode)
        {
            try
            {
                string productdesc = st_dll.GetProductDescription("dbo.spGetProductDescription", controlId);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "changename", "changename('" + categorycode + "','" + productdesc + "');", true);
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }

        }
        private void DisableButtonsOnUser()
        {

            try
            {
                //int count = st_dll.GetStatus("dbo.spCheckLabel ", "SC");
                //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");
                //var query = (from t in dt.AsEnumerable()
                //             where t.Field<int>("EmployeeID") == Convert.ToInt32(Session["UserID"])
                //             select t).ToList();
                //var query = 0;

                int result = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));

                if (result == 1)
                {    //qa logins  
                    _qauser = "Yes";
                    btnQAEsign.Enabled = true;
                    // btnPrint.Enabled = false;
                    // btnaddnew.Enabled = false;
                    btnPrintRequired.Enabled = false;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Disablebutton", "Disablebutton('" + 1 + "');", true);
                    // btnPrint.Visible = false;
                    btnPrintRequired.Visible = false;
                    btnReading.Visible = false;

                    //Enable or Disable Add New Label Button
                    CheckAddNewLabel(controlId, categorycode);

                }
                else {
                    _qauser = "";
                    btnQAEsign.Visible = false;
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Disablebutton", "DisableQAButton('" + 0 + "');", true);
                    // btnadd.Visible = false;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Disablebutton", "Disablebutton('" + 0 + "');", true);
                    // btnPrintRequired.Visible = true;
                    btnReading.Visible = true;
                    if (categorycode != 2 && categorycode != 3)
                    {
                        int toprint_result = st_dll.GetLabelStatus("dbo.spCheckLabel", "QA-ES", controlId, categorycode);
                        if (toprint_result != 0)
                        {
                            //btnPrint.Enabled = true;
                            btnPrintRequired.Enabled = true;
                            btnReading.Enabled = true;
                            btnCase.Enabled = true;

                        }
                        else {
                            //   btnPrintRequired.Enabled = false;
                            btnReading.Enabled = false;
                            btnCase.Enabled = false;
                            btnPreview.Visible = false;
                            // btnPrint.Enabled = false;
                            btnPrintRequired.Enabled = false;
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "DisablePreview", "DisablePreview('" + 0 + "');", true);
                            ErrorMessage("Labels have not been approved for printing!");
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }



        }
        private void BindGrid(int pageno, int noOfRecords)
        {
            ProductsEntity pe = new ProductsEntity();
            pe.ControlID = controlId;
            pe.categorycode = categorycode;
            pe.Pageno = pageno;
            pe.NoOfRecords = noOfRecords;
            int total = 0;
            //DataTable dt = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
            DataTable dt = st_dll.GetPagingData("dbo.GetLabelPaging", pe, out total);
            ViewState["LabelsData"] = dt;
            grvPrinting.DataSource = dt;
            grvPrinting.DataBind();



            ViewState["TotalRecord"] = total;
            ViewState["NoOfRecord"] = noOfRecords;
            //GetLabelCountToAdd();
        }
        private void AddpagingButton()
        {
            // this method for generate custom button for Custom paging in Gridview
            int totalRecord = 0;
            int noofRecord = 0;
            totalRecord = ViewState["TotalRecord"] != null ? (int)ViewState["TotalRecord"] : 0;
            noofRecord = ViewState["NoOfRecord"] != null ? (int)ViewState["NoOfRecord"] : 0;
            int pages = 0;
            if (totalRecord > 0 && noofRecord > 0)
            {
                //Text
                Button t = new Button();
                t.Text = "Paging";
                t.CssClass = "btn btn-primary";
                Panel1.Controls.Add(t);
                // Count no of pages 
                pages = (totalRecord / noofRecord) + ((totalRecord % noofRecord) > 0 ? 1 : 0);
                for (int i = 0; i < pages; i++)
                {
                    Button b = new Button();
                    b.Text = (i + 1).ToString();
                    b.CommandArgument = (i + 1).ToString();
                    b.ID = "Button_" + (i + 1).ToString();
                    b.CssClass = "btn btn-primary";
                    b.Click += new EventHandler(this.b_click);
                    Panel1.Controls.Add(b);
                }
            }

        }
        protected void b_click(object sender, EventArgs e)
        {
            // this is for Get data from Database on button (paging button) click
            string pageNo = ((Button)sender).CommandArgument;
            BindGrid(Convert.ToInt32(pageNo), 100);
        }
        private void GenerateLables(int controlId, int labelCount)
        {


            if (controlId != 0 && labelCount != 0)
            {
                int CreatedBy = Convert.ToInt32(Session["UserID"]);
                int result = st_dll.GenerateLables("dbo.spGenerateLabels", controlId, labelCount, CreatedBy, categorycode, "AN");
                if (result == 0)
                {
                    ErrorMessage("Failed to load the data !");
                }
            }
            else {
                ErrorMessage("Failed to load the data !");
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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                int controlid = controlId;
                Session["Controlid"] = controlid;
                int user = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));
                if (user == 1)
                {
                    int previedstate = st_dll.InsertPreview("dbo.spCheckLabel", "IP", controlId, categorycode);
                }

                CheckQAEsign(controlId, categorycode);
                if (categorycode == 2)
                {
                    btnQAEsign.Enabled = false;
                }
                Page.ClientScript.RegisterStartupScript(
            this.GetType(), "OpenWindow", String.Format("<script>window.open('Reportpage.aspx?catid={0}','_newtab');</script>", categorycode));
                //if (categorycode == 2 || categorycode == 3) {
                //    //btnPrintRequired.Enabled = true;
                //}

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        protected int VerifyEsign(int id, string pwd)
        {

            DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");
            var query = from t in dt.AsEnumerable()
                        where t.Field<string>("esignpassword").Contains(pwd)
                        && t.Field<int>("EmployeeID") == id
                        select t;
            DataView view = new DataView();
            view = query.AsDataView();
            if (view.Count > 0)
            //int result = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));

            //if (result == 1)
            {
                return 1;
            }
            else {
                return 0;

            }
        }
        private int SaveQAsign()
        {
            try
            {
                if (ddqanames.SelectedValue != null)
                {
                    int result = VerifyEsign(Convert.ToInt32(hdnqanames.Value), txtsignature.Text);
                    if (result != 0)
                    {
                        ProductsEntity pe = new ProductsEntity();

                        if (rbtApproval.Checked)
                        {
                            pe.ApprovedByID = Convert.ToInt32(hdnqanames.Value);
                        }
                        else {
                            pe.RejectedByID = Convert.ToInt32(hdnqanames.Value);
                            pe.StatusReason = Convert.ToInt32(hdnRejectedReasons.Value);
                        }
                        pe.ControlID = Convert.ToInt32(hdnControlid.Value);
                        pe.categorycode = categorycode;

                        int result_status = 0;
                        result_status = st_dll.InsertQASign("dbo.stProductsLabeling", "QS", pe);
                        if (rbtApproval.Checked)
                        {
                            result_status = 1;
                        }
                        else {
                            result_status = 2;
                        }
                        return result_status;
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
        protected void btnesign_Click(object sender, EventArgs e)
        {
            if (txtsignature.Text != "" && ddqanames.SelectedValue != "")
            {
                int result = SaveQAsign();
                if (result == 1)
                {
                    btnQAEsign.Enabled = false;
                    SucessMessage("The following labels are approved for printing!");
                    //SendEmailtoUser(categorycode);

                    btnPreview_Click(sender, e);
                }
                else if (result == 2)
                {

                    ErrorMessage("The following labels are rejected for printing!");
                    //SendRejectedEmailtoUser(categorycode);

                    btnPreview_Click(sender, e);
                    btnQAEsign.Enabled = false;
                    btnPreview.Enabled = false;
                }
                else {
                    ErrorMessage("E-Sign failed ! Please check user credentials");
                }
                BindGrid(1, 100);
            }
        }

        private void GetQaDetails()
        {
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
                //ViewState["EmployeeDetails"] = dt;
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

        private void GetRejectedDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {

                    dt = st_dll.GetRejectedReasons();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //ViewState["RejectedDetails"] = dt;
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


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (hdntxtlabels.Value != "0")
            {

                GenerateLables(controlId, Convert.ToInt32(hdntxtlabels.Value));

                int unitsize = st_dll.GetUnitSize(controlId);
                if (categorycode == 3 || (categorycode == 2 && (unitsize != 0)))
                {
                    st_dll.AddIntoCaseSize(controlId, categorycode);
                }


                CheckAddNewLabel(controlId, categorycode);
                BindGrid(1, 100);
            }
            else {
                ErrorMessage("Dont have option to add lables !");
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
           // btnPrint.Enabled = false;
            DataTable dt = st_dll.GetLabelCountData("dbo.spGenerateLabels", "C", controlId, categorycode);
            if (Convert.ToInt32(dt.Rows[0]["LabelCount"]) > 0)
            {
                ProductsEntity pe = new ProductsEntity();
                pe.ControlID = controlId;
                pe.PrintedByID = Convert.ToInt32(Session["UserID"]);
                pe.categorycode = categorycode;
                

                if (txtTotalLabelsremaningtoprint.Text != "")
                {
                    pe.toPrint = Convert.ToInt32(txtTotalLabelsremaningtoprint.Text);
                    int print_result = 1;// BarTenderPrintingLable(controlId, Convert.ToInt32(txtTotalLabelsremaningtoprint.Text), categorycode);
                    if (print_result != 0)
                    {
                        int result = st_dll.UpdatePrinting("dbo.spUpdatePrinting", pe);

                        if (result != 0)
                        {
                            if (categorycode == 2)
                            {
                                ddlLabelFormats.SelectedValue = "0";
                                ProductsEntity pe1 = new ProductsEntity();
                                pe1.ControlID = controlId;
                                pe1.PrintedByID = Convert.ToInt32(Session["UserID"]);
                                pe1.categorycode = 3;
                                
                                DataTable dt2 = st_dll.GetLabelCountData("dbo.spGenerateLabels", "C", controlId, 3);
                                pe1.toPrint = Convert.ToInt32(dt2.Rows[0]["LabelCount"]);
                                System.Threading.Thread.Sleep(2000);
                                int case_result = 1; BarTenderPrintingLable(controlId, Convert.ToInt32(dt2.Rows[0]["LabelCount"]), 3);
                                //DelayMethod(controlId, Convert.ToInt32(dt2.Rows[0]["LabelCount"]), categorycode);
                                if (case_result != 0)
                                {
                                    st_dll.UpdatePrinting("dbo.spUpdatePrinting", pe1);
                                }
                            }
                            BindGrid(1, 100);
                            CheckLabelPrint(controlId, categorycode);
                            SucessMessage("The labels are printed successfully!");
                        }
                        else {
                            ErrorMessage("Failed to Save the Print Data!");
                        }
                    }
                    else {

                        ErrorMessage("Failed to Print the Label in BarTender!");
                    }
                }
                else {
                    ErrorMessage("Please select how many labels you want to print!");
                }
            }
            else {
                ErrorMessage("All labels are printed!");
            }
           // btnPrint.Enabled = true;
        }

        //protected void btnPrint_Click(object sender, EventArgs e)
        //{
        //    btnPrint.Enabled = false;
        //    DataTable dt = st_dll.GetLabelCountData("dbo.spGenerateLabels", "C", controlId, categorycode);
        //    if (Convert.ToInt32(dt.Rows[0]["LabelCount"]) > 0)
        //    {
        //        ProductsEntity pe = new ProductsEntity();
        //        pe.ControlID = controlId;
        //        pe.PrintedByID = Convert.ToInt32(Session["UserID"]);
        //        pe.categorycode = categorycode;


        //        int print_result = BarTenderPrintingLable(controlId, Convert.ToInt32(dt.Rows[0]["LabelCount"]), categorycode);
        //        if (print_result != 0)
        //        {
        //            int result = st_dll.UpdatePrinting("dbo.spUpdatePrinting", pe);

        //            if (result != 0)
        //            {
        //                if (categorycode == 2)
        //                {
        //                    ProductsEntity pe1 = new ProductsEntity();
        //                    pe1.ControlID = controlId;
        //                    pe1.PrintedByID = Convert.ToInt32(Session["UserID"]);
        //                    pe1.categorycode = 3;
        //                    DataTable dt2 = st_dll.GetLabelCountData("dbo.spGenerateLabels", "C", controlId, 3);
        //                    System.Threading.Thread.Sleep(2000);
        //                    int case_result = BarTenderPrintingLable(controlId, Convert.ToInt32(dt2.Rows[0]["LabelCount"]), 3);
        //                    //DelayMethod(controlId, Convert.ToInt32(dt2.Rows[0]["LabelCount"]), categorycode);
        //                    if (case_result != 0) {
        //                        st_dll.UpdatePrinting("dbo.spUpdatePrinting", pe1);
        //                    }                            
        //                }
        //                BindGrid(1,100);
        //                CheckLabelPrint(controlId, categorycode);                        
        //                SucessMessage("The labels are printed successfully!");
        //            }
        //            else {
        //                ErrorMessage("Failed to Save the Print Data!");
        //            }
        //        }
        //        else {

        //            ErrorMessage("Failed to Print the Label in BarTender!");
        //        }
        //    }
        //    else {
        //        ErrorMessage("All labels are printed!");
        //    }
        //    btnPrint.Enabled = true;
        //}

        //public void DelayMethod(int ControlID,int labelcount,int categorycode)
        //{
        //    System.Threading.Timer timer = null;
        //    timer = new System.Threading.Timer((obj) =>
        //    {
        //        BarTenderPrintingLable(ControlID, labelcount, categorycode);
        //        timer.Dispose();
        //    },
        //                null, 1000, System.Threading.Timeout.Infinite);
        //}

        public int BarTenderPrintingLable(int controlId, int labelcount, int categorycode)
        {
            try
            {
                //DataTable dt = new DataTable();
                //dt = st_dll.ReportData("dbo.spLabelingReportPreview", controlId, categorycode);
                //DataRow row = dt.Rows[0];
                string btFileName = "";
                string fileName = "";                
                using (Engine btEngine = new Engine())
                {
                    btEngine.Start();
                    btEngine.Window.Visible = true;
                    if (categorycode == 3)
                    {
                        if (ddlLabelFormats.SelectedValue == "0")
                        {
                            fileName = st_dll.GetCaseFileName("dbo.spGetCaseFileName", controlId, categorycode);
                            btFileName = @"\\glpdc01\corp\IT Files\GPLS\BarTenderFiles\" + fileName + ".btw";
                        }
                        else {
                            //fileName = ddlLabelFormats.SelectedItem.Text;
                            fileName = ddlLabelFormats.SelectedValue.ToString();
                            btFileName = @"\\glpdc01\corp\IT Files\GPLS\BarTenderFiles\" + fileName + ".btw";
                        }

                    }
                    else {
                        //fileName = ddlLabelFormats.SelectedItem.Text;
                        fileName = ddlLabelFormats.SelectedValue.ToString();
                        // btFileName = @"\\GLWS18\Users\samancha\Documents\BarTender\BarTenderDocuments\" + fileName + ".btw";
                        btFileName = @"\\glpdc01\corp\IT Files\GPLS\BarTenderFiles\" + fileName + ".btw";
                    }

                    //if (categorycode == 1 || categorycode == 4)
                    //{ //Product Label
                    //    int unitsize = st_dll.GetUnitSize(controlId);
                    //    string itemdesc = st_dll.GetItemDesc(controlId);
                    //    if (unitsize != 0)
                    //    {
                    //        btFileName = @"\\GLWS18\Users\samancha\Documents\BarTender\BarTenderDocuments\ProductLabel_New.btw";
                    //    }

                    //    else {
                    //        btFileName = @"\\GLWS18\Users\samancha\Documents\BarTender\BarTenderDocuments\ProductLabel_New_ItemDesc.btw";
                    //    }

                    //}
                    //else if (categorycode == 2)
                    //{ //Insert Box Label
                    //    btFileName = @"\\GLWS18\Users\samancha\Documents\BarTender\BarTenderDocuments\InsertLabel_New.btw";
                    //}
                    //else if (categorycode == 3)
                    //{ //Case Label
                    //    btFileName = @"\\GLWS18\Users\samancha\Documents\BarTender\BarTenderDocuments\CaseLabel_New.btw";
                    //}

                    // btFileName = @"C:\Users\samancha\Desktop\Product_Label.btw";
                    LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
                    Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
                    queryprompts["controlid"].Value = controlId.ToString();
                    queryprompts["categorycode"].Value = categorycode.ToString();
                    queryprompts["toprint"].Value = labelcount.ToString();
                    //btFormat.PrintSetup.IdenticalCopiesOfLabel = labelcount;
                    //btFormat.PrintSetup.NumberOfSerializedLabels = 4
                    Result result = btFormat.Print(btFileName) ;
                    //Result result = btFormat.Print();

                    if (result == Result.Failure)
                    {
                        //string Message = "Label is printed Failure";
                        //string wsScript = "<script type=\"text/javascript\">document.getElementById('#divfailure').style.display='block';document.getElementById('#divfailure').InnerHtml ='" + Message + "'</script>";
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "validation", wsScript, true);

                        //SucessMessage("Label is printed Successfully !");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 0;

                    }

                    else {
                        //string Message = "Label is printed Successfully";
                        //string wsScript = "<script type=\"text/javascript\">document.getElementById('#divAdded').style.display='block';document.getElementById('#divAdded').InnerHtml ='" + Message + "'</script>";
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "validation", wsScript, true);
                        //ErrorMessage("Failed to Print the Label !");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 1;
                    }
                }

            }
            catch (Exception)
            {
                return 0;
            }
        }
        protected void grvPrinting_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    string printed = e.Row.Cells[8].Text;
                    //string voided = e.Row.Cells[10].Text;
                    HtmlInputCheckBox chkprint = (HtmlInputCheckBox)e.Row.FindControl("chkprinted");
                    // HtmlInputCheckBox chkvoided = (HtmlInputCheckBox)e.Row.FindControl("chkvoided");
                    if (printed == "True")
                    {
                        chkprint.Checked = true;
                        //chkvoided.Disabled = false;
                    }
                    else {
                        chkprint.Checked = false;
                        //chkvoided.Disabled = true;
                    }
                    //if (voided == "True")
                    //{
                    //    chkvoided.Checked = true;
                    //    chkvoided.Disabled = true;
                    //}
                    //else {
                    //    chkvoided.Checked = false;
                    //}
                    //if (_qauser == "")
                    //{
                    //    chkvoided.Disabled = true;
                    //}

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void SendRejectedEmailtoUser(int category)
        {
            try
            {
                var categories = new Dictionary<int, string>();
                categories.Add(1, "Product Label");
                categories.Add(4, "Component Label");
                categories.Add(2, "Insert Box");
                categories.Add(3, "Case Box");
                DataTable dt_details = st_dll.GetDetails("dbo.spProductDetails", Convert.ToInt32(hdnControlid.Value));
                //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");

                string strbody = "";
                strbody = "Hi Everyone , " + "<br /><br />";
                strbody = strbody + "The following label has been rejected for printing for " + categories[category].ToString() + "<br /><br />";
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

                mail.To.Add("rvarughese@gibraltarlabsinc.com");
                mail.To.Add("alerro@gibraltarlabsinc.com");
                mail.To.Add("apurisima @gibraltarlabsinc.com");
                mail.To.Add("nbrackett @gibraltarlabsinc.com");
                mail.To.Add("zkonneh@gibraltarlabsinc.com");
                mail.To.Add("kredman @gibraltarlabsinc.com");
                mail.To.Add("czapata@gibraltarlabsinc.com");

                if (Session["EMail"] != null)
                {
                    mail.CC.Add(Session["EMail"].ToString());
                }

                mail.From = new MailAddress("gpls@gibraltarlabsinc.com");
                mail.Subject = "Rejected Labels";
                mail.Body = strbody;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }

        }

        protected void SendEmailtoUser(int category)
        {
            try
            {
                var categories = new Dictionary<int, string>();
                categories.Add(1, "Product Label");
                categories.Add(4, "Component Label");
                categories.Add(2, "Insert Box");
                categories.Add(3, "Case Box");
                DataTable dt_details = st_dll.GetDetails("dbo.spProductDetails", Convert.ToInt32(hdnControlid.Value));
                //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");

                string strbody = "";
                strbody = "Hi Everyone , " + "<br /><br />";
                strbody = strbody + "The following label has been approved for printing for " + categories[category].ToString() + "<br /><br />";
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
                mail.To.Add("rvarughese@gibraltarlabsinc.com");
                mail.To.Add("alerro@gibraltarlabsinc.com");
                mail.To.Add("apurisima @gibraltarlabsinc.com");
                mail.To.Add("nbrackett @gibraltarlabsinc.com");
                mail.To.Add("zkonneh@gibraltarlabsinc.com");
                mail.To.Add("kredman @gibraltarlabsinc.com");
                mail.To.Add("czapata@gibraltarlabsinc.com");

                if (Session["EMail"] != null)
                {
                    mail.CC.Add(Session["EMail"].ToString());
                }

                mail.From = new MailAddress("gpls@gibraltarlabsinc.com");
                mail.Subject = "Labels Approved for Printing";
                mail.Body = strbody;
                mail.IsBodyHtml = true;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }

        }
        protected void btnCase_Click(object sender, EventArgs e)
        {
            int controlid = controlId;
            DataTable dt = st_dll.GetCaseOfData(controlid);
            //Session["categorycode"] = 3;
            //int labelcount1 = Convert.ToInt32(Math.Round(Convert.ToDouble(hdnlabelcount.Value) / Convert.ToDouble(dt.Rows[0]["CASESIZE"])));
            int labelcount1 = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(hdnlabelcount.Value) / Convert.ToDouble(dt.Rows[0]["CASESIZE"])));
            //Session["labelcount"] = null;
            //Session["labelcount"] = labelcount1.ToString();
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenCaseWindow", "OpenCaseModal('" + controlid + "','" + labelcount + "','" + 3 + "');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "OpenWindow('" + controlid + "','" + labelcount1 + "','" + 3 + "');", true);
            Response.Redirect("Printing.aspx?ctrid=" + controlid.ToString() + "&lc=" + labelcount1.ToString() + "&cc=3", false);
        }

        protected void btnReading_Click(object sender, EventArgs e)
        {
            //Response.Redirect("readlabel.aspx", false);
            Response.Redirect("readlabel.aspx?ctrid=" + controlId.ToString() + "&cc=" + categorycode.ToString(), false);
        }


    }
}