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
    public partial class CaseTakeout : System.Web.UI.Page
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
                    GetDetailsOfLabels();
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
        public void GetRemainingLabels(int controlid, int categorycode)
        {
            try
            {
                int lblExpiredStatus = st_dll.CheckRemainingLabels(controlid, categorycode);
                lblRemaningLabel.Text = "Remaining labels to scan are: " + lblExpiredStatus.ToString();


            }
            catch (Exception ex)
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
                    int c_controlid = st_dll.GetControlIDByBatch(batchid, 4);
                    int lblExpiredStatus = st_dll.ChekProcudtExpired(Convert.ToInt32(c_controlid));

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
                            ReadTakeoutLabel(Convert.ToInt32(controlid), Convert.ToInt32(categorycode), Convert.ToInt32(labellno));
                            GetRemainingLabels(controlId, Convert.ToInt32(categorycode));
                        }
                        else {
                            txtTakeoutLabel.Text = "";
                            ErrorMessage("Product has expired.");
                        }

                    }
                    else {
                        txtTakeoutLabel.Text = "";
                        ErrorMessage("No more labels to scan.");
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
        public void ReadTakeoutLabel(int controlid, int categorycode, int labelno)
        {
            try
            {
                int result = st_dll.ReadCaseTakeoutLabel(controlid, categorycode, labelno, Convert.ToInt32(Session["UserID"]));
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

                ErrorMessage("Error ReadTakeout " + ex.Message);
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

                int c_controlid = st_dll.GetControlIDByBatch(batchid, 4);
                DataTable dt = st_dll.GetProductByControlID(c_controlid.ToString());
                ViewState["ProductDetails"] = dt;
                if (dt.Rows.Count > 0)
                {
                    lblDescription.Text = dt.Rows[0]["PRODUCTDESC"].ToString();
                    lblManufacturingdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ManufacturingDate"]);
                    lblExpirationdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ExpirationDate"]);
                    lblSku.Text = dt.Rows[0]["SKUNO"].ToString();
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


    }

}