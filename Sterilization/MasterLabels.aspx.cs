using Seagull.BarTender.Print;
using Sterilization.DLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class MasterLabels : System.Web.UI.Page
    {
        private string _sortDirection;
        GPLS_DLL st_dll = new GPLS_DLL();
        UsersDLL us_dll = new UsersDLL();

        public int categorycode = 0;
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
        public string path = ConfigurationManager.AppSettings["SendingEmail"];
        public string BarTenderStatus = ConfigurationManager.AppSettings["BarTendarPrinter"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                if (!IsPostBack)
                {
                    CheckUserType();
                    GetProductsDropDown();
                    GetProductLabelDetails();
                    GetQaDetails();
                    GetRejectedDetails();
                    GetVoidDetails();
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

                if (CheckUserType() == 1)
                {
                    btnQAEsign.Visible = true;
                    btnAddNewLable.Visible = true;
                    btnVoid.Visible = true;
                    btnaddNewProduct.Visible = false;
                    btnGenerateLabels.Visible = false;
                    btnPrint.Visible = false;
                    btnRead.Visible = false;
                    btnTakeout.Visible = false;
                    btnReturn.Visible = false;

                    // For CheckBox Enable to QA


                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "DisableButtons", "disableButtons('QA');", true);
                }
                else {

                    btnQAEsign.Visible = false;
                    btnAddNewLable.Visible = false;
                    btnVoid.Visible = false;
                    btnaddNewProduct.Visible = true;
                    btnGenerateLabels.Visible = true;
                    btnPrint.Visible = true;
                    btnRead.Visible = true;
                    btnTakeout.Visible = true;

                    btnReturn.Visible = true;
                    // Page.ClientScript.RegisterStartupScript(this.GetType(), "DisableButtons", "disableButtons('US');", true);
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
            // txtfilter.Attributes.Add("onkeypress", "javascript:var evnt = window.event;if (evnt.keyCode == 13) {document.getElementById('" + btnFilter.ClientID + "').click();}else { }; ");
        }
        public void GetProductsDropDown()
        {
            try
            {
                DataTable dt = st_dll.GetDataTable("dbo.spGetProductsDropDown");
                if (dt.Rows.Count > 0)
                {
                    ViewState["ProductDetails"] = dt;
                    ddproducts.DataSource = dt;
                    ddproducts.DataTextField = "ProductDesc";
                    ddproducts.DataValueField = "ProductID";
                    ddproducts.DataBind();
                    ddproducts.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                grvProductLabeling.DataSource = null;
                if (AddProductLabelDetails() == 0)
                {
                    ErrorMessage("Unable to add the product label details !");
                }
                else {
                    GetProductLabelDetails();
                    EmptyControls();


                    if (path == "1")
                    {
                        SendEmailtoQA();
                    }
                    SucessMessage("Product label is added successfully.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        protected void SendRejectedEmailtoUser()
        {
            try
            {
                var categories = new Dictionary<int, string>();
                categories.Add(1, "Product Label");
                categories.Add(4, "Component Label");
                categories.Add(2, "Insert Box");
                categories.Add(3, "Case Box");
                DataTable dt_details = st_dll.GetProductByControlID(hdnCheckedControlid.Value.ToString());
                //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");

                string strbody = "";
                strbody = "Hi Everyone , " + "<br /><br />";
                strbody = strbody + "The following label has been rejected for printing for " + categories[Convert.ToInt32(hdnCheckedCategoryCode.Value)].ToString() + "<br /><br />";
                strbody = strbody + "Control#: " + dt_details.Rows[0]["ControlID"] + "<br />";
                strbody = strbody + "Product Description: " + dt_details.Rows[0]["productdesc"] + "<br />";
                strbody = strbody + "Lot#: " + dt_details.Rows[0]["LotNo"] + "<br /><br />";
                strbody = strbody + "Please follow the link to login to Sterlization Labeling System";
                strbody = strbody + "<br/ ><br />";
                strbody = strbody + "<a href='http://pssql01:81/'>Prince Sterlization Login</a>" + "<br /><br />";
                strbody = strbody + "Thank you." + "<br /><br />";
                strbody = strbody + " PLS Alert System" + "<br />";
                strbody = strbody + " (System generated e-mail)" + "<br />";
                MailMessage mail = new MailMessage();

                mail.To.Add("rvarughese@princesterilization.com");
                mail.To.Add("alerro@princesterilization.com");
                mail.To.Add("apurisima@princesterilization.com");
                mail.To.Add("nbrackett@princesterilization.com");
                mail.To.Add("zkonneh@princesterilization.com");
                mail.To.Add("kredman@princesterilization.com");
                mail.To.Add("czapata@princesterilization.com");

                if (Session["EMail"] != null)
                {
                    mail.CC.Add(Session["EMail"].ToString());
                }

                mail.From = new MailAddress("pls@princesterilization.com");
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
        protected void SendEmailtoUser()
        {
            try
            {
                var categories = new Dictionary<int, string>();
                categories.Add(2, "Product Label");
                categories.Add(1, "Component Label");

                DataTable dt_details = st_dll.GetProductByControlID(hdnCheckedControlid.Value.ToString());
                //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");

                string strbody = "";
                strbody = "Hi Everyone , " + "<br /><br />";
                strbody = strbody + "The following label has been approved for printing for " + categories[Convert.ToInt32(hdnCheckedCategoryCode.Value)].ToString() + "<br /><br />";
                strbody = strbody + "Control#: " + dt_details.Rows[0]["ControlID"] + "<br />";
                strbody = strbody + "Product Description: " + dt_details.Rows[0]["productdesc"] + "<br />";
                strbody = strbody + "Lot#: " + dt_details.Rows[0]["LotNo"] + "<br /><br />";
                strbody = strbody + "Please follow the link to login to Sterlization Labeling System";
                strbody = strbody + "<br/ ><br />";
                strbody = strbody + "<a href='http://pssql01:81/'>Prince Sterlization Login</a>" + "<br /><br />";
                strbody = strbody + "Thank you." + "<br /><br />";
                strbody = strbody + " PLS Alert System" + "<br />";
                strbody = strbody + " (System generated e-mail)" + "<br />";
                MailMessage mail = new MailMessage();

                //for (int j = 0; j < dt.Rows.Count; j++)
                //{
                //    mail.To.Add(dt.Rows[j]["COMPANYEMAIL"].ToString());
                //}
                mail.To.Add("rvarughese@princesterilization.com");
                mail.To.Add("alerro@princesterilization.com");
                mail.To.Add("apurisima@princesterilization.com");
                mail.To.Add("nbrackett@princesterilization.com");
                mail.To.Add("kmartinez@princesterilization.com");
                mail.To.Add("kredman@princesterilization.com");
                mail.To.Add("egeorge@princesterilization.com");
                mail.To.Add("rvasquez@princesterilization.com");
                mail.To.Add("czapata@princesterilization.com");

                if (Session["EMail"] != null)
                {
                    mail.CC.Add(Session["EMail"].ToString());
                }

                mail.From = new MailAddress("pls@princesterilization.com");
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
        protected void SendEmailtoQA()
        {
            try
            {

                DataTable dt = st_dll.GetDataTable("dbo.spGetLatestMasterLabel");
                if (dt.Rows.Count > 0)
                {
                    string strbody = "";
                    strbody = "Dear Quality Analyst , " + "<br /><br />";
                    strbody = strbody + "You have " + dt.Rows[0]["CATEGORY"] + " Label for E-Signature." + "<br /><br />";
                    strbody = strbody + "Control#: " + dt.Rows[0]["ControlID"] + "<br />";
                    strbody = strbody + "Product Description: " + dt.Rows[0]["PRODUCTDESC"] + "<br />";
                    strbody = strbody + "Lot#: " + dt.Rows[0]["LotNo"] + "<br /><br />";
                    strbody = strbody + "Please follow the link to login to Sterlization Labeling System";
                    strbody = strbody + "<br/ ><br />";
                    strbody = strbody + "<a href='http://pssql01:81/'>Prince Sterlization Login</a>" + "<br /><br />";
                    strbody = strbody + "Thank you." + "<br /><br />";
                    strbody = strbody + " PLS Alert System" + "<br />";
                    strbody = strbody + " (System generated e-mail)" + "<br />";
                    MailMessage mail = new MailMessage();

                    //for (int j = 0; j < dt.Rows.Count; j++)
                    //{
                    //    mail.To.Add(dt.Rows[j]["COMPANYEMAIL"].ToString());
                    //}


                    mail.To.Add("MPannullo@princesterilization.com");
                    mail.To.Add("czapata@princesterilization.com");
                    //mail.To.Add("rpuar@princesterilization.com");


                    mail.From = new MailAddress("pls@princesterilization.com");
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
        private void GetQaDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {

                    dt = st_dll.GetDataTable("dbo.spGetQADetails");
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
        protected void btnesign_Click(object sender, EventArgs e)
        {
            if (txtsignature.Text != "" && ddqanames.SelectedValue != "")
            {
                int result = SaveQAsign();
                if (result == 1)
                {
                    //btnQAEsign.Enabled = false;
                    SucessMessage("The following labels are approved for printing!");
                    //SendEmailtoUser(Convert.ToInt32(hdnCheckedControlid.Value));
                    if (path == "1")
                    {
                        SendEmailtoUser();
                    }

                    GetProductLabelDetails();
                    btnPreview_Click(sender, e);
                }
                else if (result == 2)
                {

                    ErrorMessage("The following labels are rejected for printing!");
                    if (path == "1")
                    {
                        SendRejectedEmailtoUser();
                    }
                    //SendRejectedEmailtoUser(Convert.ToInt32(hdnCheckedControlid.Value));
                    GetProductLabelDetails();
                    btnPreview_Click(sender, e);
                    //btnQAEsign.Enabled = false;
                    //btnPreview.Enabled = false;
                }
                else {
                    ErrorMessage("E-Sign failed ! Please check user credentials");
                }
                // BindGrid(1, 100);
            }
        }
        public int CheckUserType()
        {
            try
            {
                int result = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));
                ViewState["UserType"] = result;
                return result;
            }
            catch (Exception)
            {

                return 0;
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {

                int user = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));
                if (user == 1)
                {
                    int previedstate = st_dll.InsertQApreviewed(Convert.ToInt32(hdnCheckedControlid.Value));
                }
                Session["Controlid"] = Convert.ToInt32(hdnCheckedControlid.Value);
                Page.ClientScript.RegisterStartupScript(
            this.GetType(), "OpenWindow", String.Format("<script>window.open('Reportpage.aspx?catid={0}','_newtab');</script>", Convert.ToInt32(hdnCheckedCategoryCode.Value)));

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
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
                        pe.ControlID = Convert.ToInt32(hdnCheckedControlid.Value);
                        //pe.categorycode = categorycode;

                        int result_status = 0;
                        result_status = st_dll.InsertQASign(pe);
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
        protected int VerifyEsign(int id, string pwd)
        {

            DataTable dt = st_dll.GetDataTable("dbo.spGetQADetails");
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


        private void GetRejectedDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {

                    dt = st_dll.GetDataTable("dbo.spGetRejectedReasons");
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
        public void GetProductLabelDetails()
        {
            try
            {
                DataTable dt = st_dll.GetDataTable("dbo.spGetMasterLabels");
                if (dt.Rows.Count > 0)
                {
                    ViewState["ProductLabelingDetails"] = dt;
                    grvProductLabeling.DataSource = dt;
                    grvProductLabeling.DataBind();
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
            grvProductLabeling.DataSource = new List<String>();
            grvProductLabeling.DataBind();
        }

        private int AddProductLabelDetails()
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.ManufacturingDate = Convert.ToDateTime(txtManufacturingDate.Text + " " + DateTime.Now.ToString("HH:mm:ss tt"));
                pe.ExpirationDate = Convert.ToDateTime(txtExpirationDate.Text + " " + DateTime.Now.ToString("HH:mm:ss tt"));
                pe.productid = Convert.ToInt32(ddproducts.SelectedItem.Value);
                pe.LotNo = txtlotno.Text;
                pe.labelCount = Convert.ToInt32(txtNoLables.Text);
                pe.CreatedBy = Convert.ToInt32(Session["UserID"]);
                pe.PONo = hdnPONo.Value.ToString();
                return st_dll.InsertMasterLabels(pe);
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
                return 0;
            }
        }
        private int UpdateProductLabel()
        {
            try
            {
                if (Page.IsValid)
                {
                    try
                    {
                        ProductsEntity pe = new ProductsEntity();
                        if (hdnControlID.Value != null)
                        {
                            pe.ManufacturingDate = Convert.ToDateTime(txtManufacturingDate.Text + " " + DateTime.Now.ToString("HH:mm:ss tt"));
                            pe.ExpirationDate = Convert.ToDateTime(txtExpirationDate.Text + " " + DateTime.Now.ToString("HH:mm:ss tt"));
                            pe.productid = Convert.ToInt32(ddproducts.SelectedItem.Value);
                            pe.LotNo = txtlotno.Text;
                            pe.labelCount = Convert.ToInt32(txtNoLables.Text);
                            pe.LastUserID = Convert.ToInt32(Session["UserID"]);
                            pe.ControlID = Convert.ToInt32(hdnControlID.Value);
                            pe.PONo = hdnPONo.Value.ToString();

                        }
                        return st_dll.UpdateMasterLabels(pe);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else {
                    ErrorMessage("Please fill the required fields details!");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
                return 0;
            }
        }


        protected void btnTakeout_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("shipment.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString(), false);
            }
            catch (Exception ex)
            {

                ErrorMessage("Takeout error " + ex.Message);
            }
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Return.aspx", false);
            }
            catch (Exception ex)
            {

                ErrorMessage("Takeout error " + ex.Message);
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

                    int result = st_dll.UpdateAllVoided(reasonid, controlid, categorycode, Convert.ToInt32(Session["UserID"]));
                    if (result == 1)
                    {
                        SucessMessage("Labels are voided successfully.");
                        GetProductLabelDetails();
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
        protected void btnAddLabels_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdntxtlabelsToAdd.Value != "0")
                {

                    //GenerateLables(controlId, Convert.ToInt32(hdntxtlabelsToAdd.Value));
                    GenerateLabels(Convert.ToInt32(hdnCheckedCategoryCode.Value), Convert.ToInt32(hdnCheckedControlid.Value), Convert.ToInt32(hdntxtlabelsToAdd.Value));

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

        //protected void btnReturn_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Redirect("Return.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString(), false);

        //    }
        //    catch (Exception ex)
        //    {

        //        ErrorMessage("Return error " + ex.Message);
        //    }
        //}
        protected void btnvoid_click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("voiding.aspx?categorycode=" + hdnCheckedCategoryCode.Value.ToString(), false);
            }
            catch (Exception ex)
            {

                ErrorMessage("Voiding error " + ex.Message);
            }
        }
        protected void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("readlabel.aspx?controlid=" + hdnCheckedControlid.Value.ToString() + "&categorycode=" + hdnCheckedCategoryCode.Value.ToString(), false);
            }
            catch (Exception ex)
            {


                ErrorMessage("Reading error " + ex.Message);
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
                pe.PrintedByID = Convert.ToInt32(Session["UserID"]);


                if (pe.formatname != "")
                {
                    int result = 0;

                    if (BarTenderStatus == "1")
                    {
                        result = BarTenderPrintingLable(pe.ControlID, pe.labelCount, pe.categorycode, pe.formatname, 0);
                    }
                    else {
                        result = 1;
                    }
                    if (result == 1)
                    {
                        int updateprint_result = st_dll.UpdatePrinting("dbo.spUpdatePrinting", pe);
                        if (updateprint_result == 1)
                        {
                            SucessMessage("The " + pe.labelCount + " labels are printed successfully.");
                            GetProductLabelDetails();
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
        protected void btnTestPrintLabels_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.formatname = hdnLabelFormatName.Value.ToString();
                pe.ControlID = Convert.ToInt32(hdnCheckedControlid.Value);
                pe.categorycode = Convert.ToInt32(hdnCheckedCategoryCode.Value);
                pe.labelCount = 1;
                pe.PrintedByID = Convert.ToInt32(Session["UserID"]);


                if (pe.formatname != "")
                {
                    int result = 0;

                    if (BarTenderStatus == "1")
                    {
                        result = BarTenderPrintingLable(pe.ControlID, pe.labelCount, pe.categorycode, pe.formatname, 1);
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
        public int BarTenderPrintingLable(int controlId, int labelcount, int categorycode, string filename, int testprint)
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


                    btFileName = @"\\psapp01\IT Files\PLS\BarTenderFiles\" + filename + ".btw";
                    //btFileName = @"\\psapp01\IT Files\PLS\BarTenderFilesTes1\" + filename + ".btw";


                    lf.LogMessge("S3 :" + "File Path");
                    lf.LogMessge("S4 :" + "File :" + btFileName);
                    LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
                    lf.LogMessge("S5 :" + "Go the File");
                    Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
                    queryprompts["controlid"].Value = controlId.ToString();
                    queryprompts["categorycode"].Value = categorycode.ToString();
                    queryprompts["toprint"].Value = labelcount.ToString();
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
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "log", "log('" + ex.Message + "');", true);
                return 0;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            EmptyControls();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateProductLabel() == 0)
                {
                    ErrorMessage("Unable to update the product label details !");
                }
                else {
                    GetProductLabelDetails();
                    SucessMessage("Product label is updated successfully.");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }

        }

        protected void grvProductLabeling_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkSelect = (CheckBox)e.Row.FindControl("ChkSelect");
                Button btnUpdate = (Button)e.Row.FindControl("btnUpdate");
                string reasonid = e.Row.Cells[11].Text;
                string status = e.Row.Cells[12].Text;
                string dateapprovedbyid = e.Row.Cells[13].Text;
                if (status == "1")
                {
                    if (Convert.ToInt32(ViewState["UserType"]) == 1)
                    {
                        chkSelect.Enabled = true;
                    }
                    else {
                        chkSelect.Enabled = false;
                    }
                    btnUpdate.Enabled = false;
                    btnUpdate.CssClass = "btn btn-primary";
                    btnUpdate.Text = "Completed";
                }
                else if (status == "2")
                {
                    btnUpdate.Enabled = false;
                    btnUpdate.CssClass = "btn btn-warning pendingclass";
                    btnUpdate.Text = "Pending";

                }
                else if (status == "3")
                {
                    chkSelect.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnUpdate.CssClass = "btn btn-danger";
                    btnUpdate.Text = "Rejected";
                }
                else if (status == "4")
                {
                    chkSelect.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnUpdate.CssClass = "btn btn-danger";
                    btnUpdate.Text = "Voided";
                }
                else {
                    chkSelect.Enabled = true;
                    btnUpdate.Enabled = true;
                }





                //if (status == "1")
                //{
                //    chkSelect.Enabled = false;
                //    btnUpdate.Enabled = false;
                //    btnUpdate.CssClass = "btn btn-danger";
                //    if (reasonid != "&nbsp;")
                //    {
                //        btnUpdate.Text = "Rejected";
                //    }
                //    else {
                //        btnUpdate.Text = "Completed";
                //    }
                //}
                //else if (status == "2") {
                //    if (dateapprovedbyid != "&nbsp;") {
                //        btnUpdate.Enabled = false;
                //        btnUpdate.CssClass = "btn btn-danger";
                //        btnUpdate.Text = "Pending";
                //    }                    
                //}
                //else {
                //    chkSelect.Enabled = true;
                //    btnUpdate.Enabled = true;

                //}           
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
        protected void grvProductLabeling_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["ProductLabelingDetails"];
                SetSortDirection(SortDireaction);
                if (dt != null)
                {
                    //Sort the data.
                    dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                    grvProductLabeling.DataSource = dt;
                    grvProductLabeling.DataBind();
                    SortDireaction = _sortDirection;
                    int columnIndex = 0;
                    foreach (DataControlFieldHeaderCell headerCell in grvProductLabeling.HeaderRow.Cells)
                    {
                        if (headerCell.ContainingField.SortExpression == e.SortExpression)
                        {
                            columnIndex = grvProductLabeling.HeaderRow.Cells.GetCellIndex(headerCell);
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
        private void EmptyControls()
        {
            grvProductLabeling.SelectedIndex = -1;
            //btnUpdate.Visible = false;
            //btnSave.Enabled = true;
            txtManufacturingDate.Text = "";
            ddproducts.SelectedIndex = 0;
            txtExpirationDate.Text = "";
            txtlotno.Text = "";
            txtNoLables.Text = "";
            txtskuno.Text = "";
        }
        private void ErrorMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", "ErrorMessage('" + msg + "');", true);
        }
        private void SucessMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", "SuccessMessage('" + msg + "');", true);
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
                DataTable dt = st_dll.ProductSearch("dbo.spMasterLabelSearch", pe);
                if (dt.Rows.Count > 0)
                {
                    grvProductLabeling.DataSource = dt;
                    grvProductLabeling.DataBind();
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
                GetProductLabelDetails();
                txtfilter.Text = "";
                ddFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
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
                DataTable dt_details = st_dll.GetProductByControlID(hdnCheckedControlid.Value.ToString());
                //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");

                string strbody = "";
                strbody = "Hi Everyone , " + "<br /><br />";
                strbody = strbody + "The following label has been rejected for printing for " + categories[category].ToString() + "<br /><br />";
                strbody = strbody + "Control#: " + dt_details.Rows[0]["ControlID"] + "<br />";
                strbody = strbody + "Product Description: " + dt_details.Rows[0]["productdesc"] + "<br />";
                strbody = strbody + "Lot#: " + dt_details.Rows[0]["LotNo"] + "<br /><br />";
                strbody = strbody + "Please follow the link to login to Sterlization Labeling System";
                strbody = strbody + "<br/ ><br />";
                strbody = strbody + "<a href='http://pssql01:81'>Prince Sterilization Services Login</a>" + "<br /><br />";
                strbody = strbody + "Thank you." + "<br /><br />";
                //strbody = strbody + " GPLS Alert System" + "<br />";
                strbody = strbody + " PSS Alert System" + "<br />";
                strbody = strbody + " (System generated e-mail)" + "<br />";
                MailMessage mail = new MailMessage();
                mail.To.Add("gyemets@princesterilization.com");
                //mail.To.Add("rvarughese@gibraltarlabsinc.com");
                //mail.To.Add("alerro@gibraltarlabsinc.com");
                //mail.To.Add("apurisima @gibraltarlabsinc.com");
                //mail.To.Add("nbrackett @gibraltarlabsinc.com");
                //mail.To.Add("zkonneh@gibraltarlabsinc.com");
                //mail.To.Add("kredman @gibraltarlabsinc.com");
                //mail.To.Add("czapata@gibraltarlabsinc.com");

                if (Session["EMail"] != null)
                {
                    mail.CC.Add(Session["EMail"].ToString());
                }
                mail.From = new MailAddress("pss@princesterilization.com");
                //mail.From = new MailAddress("gpls@gibraltarlabsinc.com");
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
                DataTable dt_details = st_dll.GetProductByControlID(hdnCheckedControlid.Value.ToString());
                //DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");

                string strbody = "";
                strbody = "Hi Everyone , " + "<br /><br />";
                strbody = strbody + "The following label has been approved for printing for " + categories[category].ToString() + "<br /><br />";
                strbody = strbody + "Control#: " + dt_details.Rows[0]["ControlID"] + "<br />";
                strbody = strbody + "Product Description: " + dt_details.Rows[0]["productdesc"] + "<br />";
                strbody = strbody + "Lot#: " + dt_details.Rows[0]["LotNo"] + "<br /><br />";
                strbody = strbody + "Please follow the link to login to Sterlization Labeling System";
                strbody = strbody + "<br/ ><br />";
                //strbody = strbody + "<a href='http://gblnj4:85'>Gibraltar Laboratories Sterlization Login</a>" + "<br /><br />";
                strbody = strbody + "<a href='http://pssql01:81'>Prince Sterilization Services Login</a>" + "<br /><br />";
                strbody = strbody + "Thank you." + "<br /><br />";
                strbody = strbody + " PSS Alert System" + "<br />";
                strbody = strbody + " (System generated e-mail)" + "<br />";
                MailMessage mail = new MailMessage();

                //for (int j = 0; j < dt.Rows.Count; j++)
                //{
                //    mail.To.Add(dt.Rows[j]["COMPANYEMAIL"].ToString());
                //}
                mail.To.Add("gyemets@princesterilization.com");
                //mail.To.Add("rvarughese@gibraltarlabsinc.com");
                //mail.To.Add("alerro@gibraltarlabsinc.com");
                //mail.To.Add("apurisima @gibraltarlabsinc.com");
                //mail.To.Add("nbrackett @gibraltarlabsinc.com");
                //mail.To.Add("zkonneh@gibraltarlabsinc.com");
                //mail.To.Add("kredman @gibraltarlabsinc.com");
                //mail.To.Add("czapata@gibraltarlabsinc.com");

                if (Session["EMail"] != null)
                {
                    mail.CC.Add(Session["EMail"].ToString());
                }
                mail.From = new MailAddress("pss@princesterilization.com");
                //mail.From = new MailAddress("gpls@gibraltarlabsinc.com");
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


        protected void grvProductLabeling_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void btnGenerateLabels_Click(object sender, EventArgs e)
        {
            GenerateLabels(Convert.ToInt32(hdnCheckedCategoryCode.Value), Convert.ToInt32(hdnCheckedControlid.Value), Convert.ToInt32(hdnCheckedLabelCount.Value));
        }
        public void GenerateLabels(int categorycode, int controlid, int labelcount)
        {
            try
            {
                int result = st_dll.GenerateComponentAndProductLabels(controlid, categorycode, labelcount, Convert.ToInt32(Session["UserID"]));
                if (result == 1)
                {
                    SucessMessage("Total " + labelcount + " labels are generated for " + CategoryDesc(categorycode));
                    GetProductLabelDetails();
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
                case 1:
                    return "Component";
                case 2:
                    return "Product";
                default:
                    return "";
            }
        }

        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.Status = 1;
                DataTable dt = st_dll.ProductSearch("dbo.spMasterLabelSearch", pe);
                if (dt.Rows.Count > 0)
                {
                    grvProductLabeling.DataSource = dt;
                    grvProductLabeling.DataBind();
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
                DataTable dt = st_dll.ProductSearch("dbo.spMasterLabelSearch", pe);
                if (dt.Rows.Count > 0)
                {
                    grvProductLabeling.DataSource = dt;
                    grvProductLabeling.DataBind();
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

    }

}