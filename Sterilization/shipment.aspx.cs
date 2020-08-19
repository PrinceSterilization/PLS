//shipment.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Takeout from inventory  page
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
    public partial class shipment : System.Web.UI.Page
    {
        GPLS_DLL st_dll = new GPLS_DLL();
        private int controlId = 0;
        private int categorycode = 0;
        private int batchid = 0;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] != null)
            {
                if (Request.QueryString["controlid"] != null)
                {
                    controlId = Convert.ToInt32(Request.QueryString["controlid"]);
                    hdnControlId.Value = controlId.ToString();
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
                    GetDetailsOfLabels();
                    GetBatches();

                }
                GetDetailsOfLabelsFromViewState();
                GetRemainingLabels(controlId, categorycode);
                txtTakeoutLabel.Focus();
            }
            else {
                Response.Redirect("Login.aspx");
            }
            
        }
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
        public void GetRemainingLabels(int controlid, int categorycode) {
            try
            {
                int lblExpiredStatus = st_dll.CheckRemainingLabels(controlid,categorycode);
                lblRemaningLabel.Text = "Remaining labels to scan are: " + lblExpiredStatus.ToString();

                if (lblExpiredStatus == 0) {
                    st_dll.UpdateCompletedStatusForComponentsAndProduct(controlId);
                }
            }
            catch (Exception ex )
            {
                ErrorMessage("Error in GetRemaningLabels" + ex.Message);

            }
        }
        protected void txtTakeoutLabel_TextChanged(object sender, EventArgs e)
        {
            string labelno = txtTakeoutLabel.Text;
            if (Convert.ToInt32(labelno.Split('-')[1]) == categorycode && Convert.ToInt32(labelno.Split('-')[0]) == controlId)
            {
                if (labelno != "")
                {

                    string controlid = labelno.Split('-')[0];
                    string labellno = labelno.Split('-')[2].TrimStart('0');
                    string categorycode = labelno.Split('-')[1];

                    int lblExpiredStatus = st_dll.ChekProcudtExpired(Convert.ToInt32(controlid));
                    GetUsage();

                    int labelexist = st_dll.CheckLabel(Convert.ToInt32(controlid), Convert.ToInt32(categorycode), Convert.ToInt32(labellno));

                    if (labelexist > 0)
                    {
                        //Check product Expired or not 
                        //    1- Expired
                        //    0- Not-Expired
                        if (lblExpiredStatus == 0)
                        {
                            txtTakeoutLabel.Text = "";
                            txtTakeoutLabel.Focus();
                            // string categorycode = labelno.Split('-')[1];
                            //GetDetailsOfLabels(controlid, labellno);
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "ReadShipinglabel", "ReadShipinglabel('" + controlid + "','" + labellno + "','" + ViewState["Usage"].ToString() + "','" + ViewState["batchid"].ToString() + "');", true);                    
                            ReadTakeoutLabel(Convert.ToInt32(controlid), Convert.ToInt32(categorycode), Convert.ToInt32(labellno), Convert.ToInt32(ViewState["Usage"]), Convert.ToInt32(ddlBatches.SelectedValue));
                            GetRemainingLabels(controlId, Convert.ToInt32(categorycode));
                        }
                        else {
                            txtTakeoutLabel.Text = "";
                            ErrorMessage("Product has expired.");
                        }

                    }
                    else {
                        txtTakeoutLabel.Text = "";
                        ErrorMessage("The label is already scanned or no more labels to read.");
                    }
                }
                else {
                    txtTakeoutLabel.Text = "";
                    ErrorMessage("Cannot read the label!");
                }
            }
            else {
                txtTakeoutLabel.Text = "";
                ErrorMessage("Cannot read the label!");
            }
               
        }
        public void ReadTakeoutLabel(int controlid,int categorycode, int labelno, int usage, int batchid) {
            try
            {
                // This is only used for replacement for damage
                int damage = 0;
                if (chkDamage.Checked)
                {
                    damage = 1;
                }
                else {
                    damage = 0;
                }
                int result = st_dll.ReadTakeoutLabel(controlid, categorycode, labelno, usage, batchid, Convert.ToInt32(Session["UserID"]), damage);
                if (result == 1)
                {
                    Num++;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessReadMessage", "SuccessStatus();", true);
                }
                else if (result == 2)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessReadMessage", "ReadErrorMessage('Label already scanned or Voided.');", true);

                }
                else {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessReadMessage", "ReadErrorMessage('Failed to takeout the label.');", true);
                }
                lblTotalScanStatus.Text = "Total Scanned: " + Num.ToString();
            }
            catch (Exception ex)
            {

                ErrorMessage("Error ReadTakeout "+ ex.Message);
            }
        }
        public void GetUsage() {    
            if (ViewState["Usage"].ToString() == "1")
            {
                rbtSample.Checked = true;
            }
            else if (ViewState["Usage"].ToString() == "2")
            {
                rbtDirectShipment.Checked = true;
            }
            else if (ViewState["Usage"].ToString() == "3")
            {
                rbtProductKit.Checked = true;
            }
            else {
                rbtDirectShipment.Checked = true;
            }
        }
        private void GetBatches()
        {
            try
            {
                DataTable dt = new DataTable();
                try
                {

                    dt = st_dll.GetBatches(controlId,categorycode);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //ViewState["RejectedDetails"] = dt;
                ddlBatches.DataSource = dt;
                ddlBatches.DataTextField = "BATCHID";
                ddlBatches.DataValueField = "BAT";
                ddlBatches.DataBind();
               
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        private void GetDetailsOfLabelsFromViewState()
        {
            try
            {

                DataTable dt = (DataTable)ViewState["ProductDetails"];

                if (dt.Rows.Count > 0)
                {
                    lblDescription.Text = dt.Rows[0]["PRODUCTDESC"].ToString();
                    lblManufacturingdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ManufacturingDate"]);
                    lblExpirationdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ExpirationDate"]);
                    lblSku.Text = dt.Rows[0]["SKUNO"].ToString();
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
                //int batchid = st_dll.GetLatestBatchID(controlId, categorycode);

                //int c_controlid = 0;
                //if (categorycode == 4)
                //{
                //    c_controlid = st_dll.GetControlIDByBatch(batchid, 4);
                //}
                //else 
                //{
                //    c_controlid = controlId;
                //}

                DataTable dt = st_dll.GetProductByControlID(controlId.ToString());
                ViewState["ProductDetails"] = dt;
                if (dt.Rows.Count > 0)
                {
                    lblDescription.Text = dt.Rows[0]["PRODUCTDESC"].ToString();
                    lblManufacturingdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ManufacturingDate"]);
                    lblExpirationdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ExpirationDate"]);
                    lblSku.Text = dt.Rows[0]["SKUNO"].ToString();
                    //lblBatchID.Text = batchid.ToString();
                    ViewState["batchid"] = ddlBatches.SelectedValue;
                    ViewState["Usage"] = "2";
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

        protected void rbtSample_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["Usage"] = "1";
            txtTakeoutLabel.Focus();
        }

        protected void rbtDirectShipment_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["Usage"] = "2";
            txtTakeoutLabel.Focus();
        }

        protected void rbtProductKit_CheckedChanged(object sender, EventArgs e)
        {
            ViewState["Usage"] = "3";
            txtTakeoutLabel.Focus();
        }


        //protected void btnMergeLabels_Click(object sender, EventArgs e)
        //{
            
        //    int controlid = Convert.ToInt32(hdnControlId.Value);
        //    int frombatchid = Convert.ToInt32(hdnfrombatchid.Value);
        //    int tobatchid = Convert.ToInt32(hdntobatchid.Value);

        //    int result = st_dll.MergeBatchids(controlid, frombatchid, tobatchid, Convert.ToInt32(Session["UserID"]));

        //    if (result == 1)
        //    {
        //        SucessMessage("Labels are merged successfull.");
               
        //    }
        //    else {
        //        ErrorMessage("Failed to merge.");
        //    }
        //}
    }
}