using Sterilization.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class Return : System.Web.UI.Page
    {
        GPLS_DLL st_dll = new GPLS_DLL();
        int controlId = 0;
        int categorycode = 0;
        int batchid = 0;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] != null)
            {
                if (Request.QueryString["controlid"] != null)
                {
                    controlId = Convert.ToInt32(Request.QueryString["controlid"]);
                    //hdnControlid.Value = controlId.ToString();
                }
                if (Request.QueryString["categorycode"] != null)
                {
                    categorycode = Convert.ToInt32(Request.QueryString["categorycode"]);
                    //hdncategorycode.Value = categorycode.ToString();
                }
                if (Request.QueryString["batchid"] != null)
                {
                    batchid = Convert.ToInt32(Request.QueryString["batchid"]);
                    //hdnbatchid.Value = batchid.ToString();
                }
                if (!IsPostBack)
                {
                    //GetProductDetails();
                    //GetDetailsOfLabels();
                   

                }
                //GetDetailsOfLabelsFromViewState();
               
                txtTakeoutLabel.Focus();
            }
            else {
                Response.Redirect("Login.aspx");
            }

        }
        //public void GetProductDetails() {
        //    try
        //    {
        //        DataTable dt = st_dll.GetDataTable("dbo.spGetCompletedProducts");
        //        ddlProducts.DataSource = dt;
        //        ddlProducts.DataTextField = "ProductDesc";
        //        ddlProducts.DataValueField = "CONTROLID";
        //        ddlProducts.DataBind();
        //        ddlProducts.Items.Insert(0, new ListItem("--Select--", "0"));
        //    }
        //    catch (Exception ex)
        //    {

        //        ErrorMessage("Error in Getting Products "+ ex.Message);
        //    }
        //}
        public int Num
        {
            get
            {
                if (ViewState["num"] != null)
                    return Convert.ToInt32(ViewState["num"]);
                else
                    return 0;
            }
            set { ViewState["num"] = value; }
        }
       
        protected void txtTakeoutLabel_TextChanged(object sender, EventArgs e)
        {
            string labelno = txtTakeoutLabel.Text;
            if (labelno != "")
            {
                string controlid = labelno.Split('-')[0];
                string labellno = labelno.Split('-')[2].TrimStart('0');
                string categorycode = labelno.Split('-')[1];
                               
                int labelexist = st_dll.CheckLabelReturn(Convert.ToInt32(controlid), Convert.ToInt32(categorycode), Convert.ToInt32(labellno));
                //if (controlid == ddlProducts.SelectedValue)
                //{
                    if (labelexist == 1)
                    {
                        ReadReturnLabel(Convert.ToInt32(controlid), Convert.ToInt32(categorycode), Convert.ToInt32(labellno));
                        txtTakeoutLabel.Text = "";
                        txtTakeoutLabel.Focus();

                    }
                    else {
                       
                        ErrorMessage("You cannot return the label which is already return or not taken out.");
                        txtTakeoutLabel.Text = "";
                    }
                //}
                //else {
                //    ErrorMessage("Cannot read a different label.");
                //    txtTakeoutLabel.Text = "";

                //}               
            }
            else {
                ErrorMessage("Cannot read the label!");
                txtTakeoutLabel.Text = "";
            }
        }
        public void ReadReturnLabel(int controlid, int categorycode, int labelno)
        {
            try
            {
                int result = st_dll.ReadReturnLabel(controlid, categorycode, labelno);
                if (result == 1)
                {
                    Num++;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessReadMessage", "SuccessStatus();", true);
                }               
                else {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ReadErrorMessage", "ReadErrorMessage('Failed to Return the label.');", true);
                }
                lblTotalScanStatus.Text = "Total Scanned: " + Num.ToString();
            }
            catch (Exception ex)
            {

                ErrorMessage("Error in Return " + ex.Message);
            }
        }
       
        private void GetDetailsOfLabelsFromViewState()
        {
            try
            {

                DataTable dt = (DataTable)ViewState["ProductDetails"];

                if (dt.Rows.Count > 0)
                {
                    //lblDescription.Text = dt.Rows[0]["PRODUCTDESC"].ToString();
                    //lblManufacturingdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ManufacturingDate"]);
                    //lblExpirationdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ExpirationDate"]);
                    //lblSku.Text = dt.Rows[0]["SKUNO"].ToString();
                    //lblBatchID.Text= ViewState["batchid"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("ERROR:1 " + ex.Message);
            }
        }
        private void GetDetailsOfLabels()
        {
            try
            {
               

                int c_controlid = 0;
                if (categorycode == 4)
                {
                    c_controlid = st_dll.GetControlIDByBatch(batchid, 4);
                }
                else
                {
                    c_controlid = controlId;
                }

                DataTable dt = st_dll.GetProductByControlID(c_controlid.ToString());
                ViewState["ProductDetails"] = dt;
                if (dt.Rows.Count > 0)
                {
                    
                    //lblManufacturingdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ManufacturingDate"]);
                    //lblExpirationdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ExpirationDate"]);
                    //lblSku.Text = dt.Rows[0]["SKUNO"].ToString();
                    //lbllotno.Text = dt.Rows[0]["LotNo"].ToString();
                    //lblBatchID.Text = batchid.ToString();

                }
            }
            catch (Exception ex)
            {
                ErrorMessage("ERROR:1 " + ex.Message);
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

        //protected void ddlProducts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlProducts.SelectedValue != "0")
        //    {
        //        string controlid = ddlProducts.SelectedValue;
        //        DataTable dt = st_dll.GetProductByControlID(controlid);
        //        if (dt.Rows.Count > 0)
        //        {
        //            //lbllotno.Text = dt.Rows[0]["Lotno"].ToString();
        //            //lblSku.Text = dt.Rows[0]["SKUNO"].ToString();
        //            //lblManufacturingdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ManufacturingDate"]);
        //            //lblExpirationdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ExpirationDate"]);
        //            txtTakeoutLabel.Focus();
        //            //Page.ClientScript.RegisterStartupScript(this.GetType(), "EnableDetails", "EnableDetails(1);", true);
        //        }
        //        else {
        //            ErrorMessage("Error in slecting the product");
        //        }
        //    }
        //    else {
        //        //Page.ClientScript.RegisterStartupScript(this.GetType(), "EnableDetails", "EnableDetails(0);", true);
        //        ErrorMessage("Please select the product.");
        //    }
            
            
        //}
    }

}