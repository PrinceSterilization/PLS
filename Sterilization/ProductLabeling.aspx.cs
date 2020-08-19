//ProductLabeling.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Adding Product Label page

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Web.UI.HtmlControls;

namespace Sterilization
{
    public partial class ProductLabeling : System.Web.UI.Page
    {

        private string _sortDirection;
        SterilizationDLL st_dll = new SterilizationDLL();
        UsersDLL us_dll = new UsersDLL();
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
                    GetProducts();
                    GetProductLabelDetails();
                    //ShowEmptyGrid();
                }

            }
            else {
                Response.Redirect("Login.aspx");
            }
            DataTable chk_Permission = us_dll.CheckUserAccessLevel(Convert.ToInt32(Session["UserID"]),2);
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


            //txtManufacturingDate.Attributes.Add("onchange", "addYear(this,'365Days',365)");

        }
        public void GetProducts()
        {
            try
            {
                DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "DD");
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

        //private void GetDropDownValues()
        //{

        //    DataTable dt = (DataTable)ViewState["ProductDetails"];

        //    if (Convert.ToInt32(ddproducts.SelectedItem.Value) != 0)
        //    {
        //        var results = from c in dt.AsEnumerable()
        //                      where c.Field<int>("ProductID") == Convert.ToInt32(ddproducts.SelectedItem.Value)
        //                      select c.Field<string>("SKUNo");

        //        foreach (string skuno in results)
        //        {
        //            txtskuno.Text = skuno;
        //        }
        //    }
        //    else { txtskuno.Text = ""; }
        //}
        //protected void ddproducts_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    GetDropDownValues();
        //}

        protected void cvmandate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Regex.IsMatch(txtManufacturingDate.Text, "^(?:0?[1-9]|1[0-2])[.//-](?:[012]?[0-9]|3[01])[.//-](?:[0-9]{2}){1,2}$"))
            {
                DateTime dt;
                args.IsValid = DateTime.TryParseExact(args.Value, "MM/dd/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
                //if (args.IsValid)
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Valid Date.');", true);
                //}
            }
            else {
                args.IsValid = false;
            }
        }

        protected void cvm_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Regex.IsMatch(txtExpirationDate.Text, "^(?:0?[1-9]|1[0-2])[.//-](?:[012]?[0-9]|3[01])[.//-](?:[0-9]{2}){1,2}$"))
            {
                DateTime dt;
                args.IsValid = DateTime.TryParseExact(args.Value, "MM/dd/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
                //if (args.IsValid)
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Valid Date.');", true);
                //}
            }
            else {
                args.IsValid = false;
            }
        }

        private void ShowEmptyGrid()
        {
            grvProductLabeling.DataSource = new List<String>();
            grvProductLabeling.DataBind();
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
                       // SendEmailtoQA();
                        SucessMessage("Product label is added successfully.");
                    }


                //}
                //else {
                //    ErrorMessage("Please Fill the Required Field !");
                //}
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
                DataTable dt = st_dll.GetDBData("dbo.stProductsLabeling", "S");

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
                pe.LastUserID = Convert.ToInt32(Session["UserID"]);
                pe.PONo = hdnPONo.Value.ToString();
                return st_dll.AddProductLabelDetails("dbo.stProductsLabeling", "IN", pe);
               


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
                        //int username = 450;
                        ProductsEntity pe = new ProductsEntity();
                        if (hdnControlID.Value != null)
                        {
                            //SqlCommand cmd = new SqlCommand("dbo.stProductsLabeling", conn);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@ManufacturingDate", txtManufacturingDate.Text);
                            //cmd.Parameters.AddWithValue("@Type", "U");
                            //cmd.Parameters.AddWithValue("@ExpirationDate", txtExpirationDate.Text);
                            //cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ddproducts.SelectedItem.Value));
                            //cmd.Parameters.AddWithValue("@LotNo", txtlotno.Text);
                            //cmd.Parameters.AddWithValue("@LabelCount", txtNoLables.Text);
                            //cmd.Parameters.AddWithValue("@ControlID", ViewState["ControlID"]);

                            ////cmd.Parameters.AddWithValue("@ApprovedByID", username);
                            ////cmd.Parameters.AddWithValue("@Reconciled", username);
                            ////cmd.Parameters.AddWithValue("@LastUserID", username);


                            pe.ManufacturingDate = Convert.ToDateTime(txtManufacturingDate.Text + " " + DateTime.Now.ToString("HH:mm:ss tt"));
                            pe.ExpirationDate = Convert.ToDateTime(txtExpirationDate.Text + " " + DateTime.Now.ToString("HH:mm:ss tt"));
                            pe.productid = Convert.ToInt32(ddproducts.SelectedItem.Value);
                            pe.LotNo = txtlotno.Text;
                            pe.labelCount = Convert.ToInt32(txtNoLables.Text);
                            pe.LastUserID = Convert.ToInt32(Session["UserID"]);
                            pe.ControlID = Convert.ToInt32(hdnControlID.Value);
                            pe.PONo = hdnPONo.Value.ToString();

                        }
                        return st_dll.UpdateProductLabelDetails("dbo.stProductsLabeling", "U", pe);
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
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvProductLabeling, "Select$" + e.Row.RowIndex);
                // e.Row.Attributes["onclientclick"] = Page.ClientScript.IsStartupScriptRegistered()
                e.Row.Attributes["style"] = "cursor:pointer";

                //string status = e.Row.Cells[7].Text;
                
                //HtmlInputCheckBox chkstatus = (HtmlInputCheckBox)e.Row.FindControl("chkstatus");
                //if (status == "True")
                //{
                //    chkstatus.Checked = true;
                //}
                //else {
                //    chkstatus.Checked = false;
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

        //protected void grvProductLabeling_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = grvProductLabeling.SelectedRow;
        //    ddproducts.SelectedValue = row.Cells[5].Text;
        //    //ViewState["SelectedValue"] = Convert.ToInt32(row.Cells[5].Text);
        //    //GetDropDownValues();
        //    txtlotno.Text = row.Cells[1].Text;
        //    txtNoLables.Text = row.Cells[2].Text;
        //    txtManufacturingDate.Text = Convert.ToDateTime(row.Cells[3].Text).ToString("MM/dd/yyyy");
        //    txtExpirationDate.Text = Convert.ToDateTime(row.Cells[4].Text).ToString("MM/dd/yyyy");
        //    btnUpdate.Visible = true;
        //    btnSave.Enabled = false;
        //    btnCancel.Visible = true;

        //    ViewState["ControlID"] = Convert.ToInt32(row.Cells[6].Text);

        //}
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
        //protected void txtManufacturingDate_TextChanged(object sender, EventArgs e)
        //{
        //    DateTime dt = Convert.ToDateTime(txtManufacturingDate.Text).AddYears(1);
        //    txtExpirationDate.Text = dt.ToString("MM/dd/yyyy");
        //}

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
                DataTable dt = st_dll.ProductSearch("dbo.spProductSearch", pe);

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
                DataTable dt = (DataTable)ViewState["ProductLabelingDetails"];
                grvProductLabeling.DataSource = dt;
                grvProductLabeling.DataBind();
                txtfilter.Text = "";
                ddFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }

        

    }

}