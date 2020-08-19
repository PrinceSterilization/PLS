//readlabel.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Reading the printed labels
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
    public partial class readlabel : System.Web.UI.Page
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
                    hdnControlid.Value = controlId.ToString();
                }
                if (Request.QueryString["categorycode"] != null)
                {
                    categorycode = Convert.ToInt32(Request.QueryString["categorycode"]);
                    hdncategorycode.Value = categorycode.ToString();
                }
                if (Request.QueryString["batchid"] != null)
                {
                    batchid = Convert.ToInt32(Request.QueryString["batchid"]);
                    hdnbatchid.Value = batchid.ToString();
                }

                if (!IsPostBack)
                {
                    GetDetailsOfLabels();

                }
                GetDetailsOfLabelsFromViewState();
                txtLabel.Focus();
                lblTotalScanStatus.Text = "Total Scanned: " + GetTotalScanned();
                string parameter = Request["__EVENTARGUMENT"];
                if (parameter == "ReadLabel")
                {

                    lblTotalScanStatus.Text = "Total Scanned: " + GetTotalScanned();
                    SucessMessage("Label read sucessfully!");
                    txtLabel.Focus();

                }
                //Show "Replacement for Damage" checkbox only when it is insert  box labels
                if (categorycode == 3)
                {
                    chkDamage.Visible = true;
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }


        public string GetTotalScanned()
        {
            try
            {
                DataTable dt = st_dll.GetTotalLabelcount(controlId, categorycode); ;
                var query = from t in dt.AsEnumerable()
                            where t.Field<string>("LABELSTATUS").Contains("Yes")
                            select t;
                //if (Convert.ToInt32(query.AsDataView().Count) == Convert.ToInt32(dt.Rows[0]["TOTALLABELCOUNT"])) {
                //    st_dll.UpdateCompletedStatusForComponentsAndProduct(controlId);
                //}
                return query.AsDataView().Count.ToString() + " of " + dt.Rows[0]["TOTALLABELCOUNT"].ToString();

            }
            catch (Exception)
            {
                return null;
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
                if (categorycode == 3)
                {
                    c_controlid = st_dll.GetControlIDByBatch(batchid, 3);
                }
                else if (categorycode == 4)
                {
                    c_controlid = st_dll.GetControlIDByBatch(batchid, 4);
                }
                else {
                    c_controlid = controlId;
                }

                DataTable dt = st_dll.GetProductByControlID(c_controlid.ToString());
                ViewState["ProductDetails"] = dt;
                if (dt.Rows.Count > 0)
                {
                    lblDescription.Text = dt.Rows[0]["PRODUCTDESC"].ToString();
                    lblLotno.Text = dt.Rows[0]["LotNo"].ToString();
                    lblManufacturingdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ManufacturingDate"]);
                    lblExpirationdate.Text = String.Format("{0:MM/dd/yyyy}", dt.Rows[0]["ExpirationDate"]);
                    lblSku.Text = dt.Rows[0]["SKUNO"].ToString();

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

        protected void txtLabel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string labelno = txtLabel.Text;//1-1-001

                if (Convert.ToInt32(labelno.Split('-')[1]) == categorycode 
                    && Convert.ToInt32(labelno.Split('-')[0]) == controlId)
                {
                    if (labelno != "")
                    {
                        int damage = 0;
                        if (chkDamage.Visible)
                        {
                            if (chkDamage.Checked)
                            {
                                damage = 1;
                            }
                            else {
                                damage = 0;
                            }
                        }
                        else {
                            damage = 0;
                        }
                        string controlid = labelno.Split('-')[0];
                        string labellno = labelno.Split('-')[2];
                        string scategorycode = labelno.Split('-')[1];

                        int result = st_dll.CheckLabelRead(Convert.ToInt32(controlid), Convert.ToInt32(labellno), Convert.ToInt32(scategorycode));
                        if (result == 1)
                        {
                            txtLabel.Text = "";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "ReadingLabel", "Readlabel('" + controlid + "','" + labellno + "','" + scategorycode + "','" + damage + "');", true);
                        }
                        else {
                            txtLabel.Text = "";
                            ErrorMessage("The label has been read or voided!");
                        }
                    }
                    else {
                        txtLabel.Text = "";
                        ErrorMessage("ERROR:4 " + "Unable to read the label!");
                    }
                }
                else {
                    txtLabel.Text = "";
                    ErrorMessage("You cannot read the different label.");
                }


            }
            catch (Exception ex)
            {

                ErrorMessage("ERROR:3 " + ex.Message);
            }
        }
    }
}