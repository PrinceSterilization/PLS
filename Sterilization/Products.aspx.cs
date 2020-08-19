//Products.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Adding Product page

using Sterilization.DLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class Products : System.Web.UI.Page
    {

        Image sortImage = new Image();
        private string _sortDirection;
        GPLS_DLL st_dll = new GPLS_DLL();
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
                    GetProductDetails();
                }
               // ddlCategory.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            else {
                Response.Redirect("Login.aspx");
            }
            DataTable chk_Permission = us_dll.CheckUserAccessLevel(Convert.ToInt32(Session["UserID"]), 1);
           

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
            // AddProductDetails();
        }


        private int AddProductDetails()
        {
            try
            {

                ProductsEntity pe = new ProductsEntity();
                pe.ProductDesc = txtProductDesc.Text;
                pe.SKUNo = txtSKUNo.Text;
                pe.StorageCondition = txtStoragecond.Text;
                pe.ItemDesc = txtItemDesc.Text;
                pe.MasterFormat = Convert.ToInt32(ddlMasterLabels.SelectedValue);
                pe.InsertFormat = Convert.ToInt32(ddlInsertLabelFormat.SelectedValue);
                pe.CaseFormat = Convert.ToInt32(ddlCaseLabelFormat.SelectedValue);

                //.Replace("\r\n", "<br/>"); ;
                //if (txtUnitsize.Text != "") {
                //    pe.Unitsize = Convert.ToInt32(txtUnitsize.Text);
                //}
                //pe.Units = Convert.ToInt32(txtUnits.Text);
                if (txtUnits.Text != "")
                {
                    pe.Units = Convert.ToInt32(txtUnits.Text);
                }
                if (rbtUnits.Checked)
                {
                    pe.UnitType = 1;
                }
                else if (rbtBags.Checked)
                {
                    pe.UnitType = 2;
                }
                //else {
                //    pe.UnitType = Convert.ToInt32(rdUnit.SelectedItem.Value);
                //}
              
                pe.categorycode = Convert.ToInt32(ddlCategory.SelectedItem.Value);
                //pe.casesize = Convert.ToInt32(txtCasesize.Text);
                pe.CreatedBy = Convert.ToInt32(Session["UserID"]);
                pe.LastUserID = Convert.ToInt32(Session["UserID"]);
                return st_dll.InsertProduct(pe);
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
            }

        }

        public void GetProductDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = st_dll.GetDBProductData();
                ViewState["ProductDetails"] = dt;
                grvProducts.DataSource = dt;
                grvProducts.DataBind();
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }

        protected void grvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvProducts.PageIndex = e.NewPageIndex;
            grvProducts.DataSource = ViewState["ProductDetails"];
            grvProducts.DataBind();
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
        protected void grvProducts_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {


                DataTable dt = (DataTable)ViewState["ProductDetails"];

                SetSortDirection(SortDireaction);
                if (dt != null)
                {
                    //Sort the data.
                    dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                    grvProducts.DataSource = dt;
                    grvProducts.DataBind();
                    SortDireaction = _sortDirection;
                    int columnIndex = 0;
                    foreach (DataControlFieldHeaderCell headerCell in grvProducts.HeaderRow.Cells)
                    {
                        if (headerCell.ContainingField.SortExpression == e.SortExpression)
                        {
                            columnIndex = grvProducts.HeaderRow.Cells.GetCellIndex(headerCell);
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

        //protected void grvProducts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = grvProducts.SelectedRow;
        //    txtProductDesc.Text = row.Cells[1].Text;
        //    ViewState["ProductID"] = row.Cells[0].Text;
        //    txtSKUNo.Text = row.Cells[2].Text;
        //    btnUpdate.Visible = true;
        //    btnSave.Enabled = false;
        //    btnCancel.Visible = true;
        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenDailog", "OpenDailog();", true);
        //    //string pName = GridView1.SelectedRow.Cells[1].Text;
        //    //msg.Text = "<b>Publisher Name  &nbsp;:&nbsp;&nbsp;   " + pName + "</b>";
        //}

        protected void grvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grvProducts, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
                e.Row.Cells[2].Width = 250;
                e.Row.Cells[1].Width = 200;

                //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                //e.Row.Cells[4].Width = 140;
                //e.Row.Cells[3].Width = 100;
                //e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
                //e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
                //e.Row.ToolTip = "Click last column for selecting this row.";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (AddProductDetails() == 0)
                {
                    ErrorMessage("Unable to add the product details !");
                }
                else {
                    GetProductDetails();
                    SucessMessage("Product is added successfully.");
                }
            }
            else {
                ErrorMessage("Please fill the required field details!");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            grvProducts.SelectedIndex = -1;
            //btnUpdate.Visible = false;
            //btnSave.Enabled = true;
            txtProductDesc.Text = "";
            txtSKUNo.Text = "";
            txtUnits.Text = "";
            txtStoragecond.Text = "";

        }
        private void LoadGriddata()
        {

            grvProducts.DataSource = ViewState["ProductDetails"];
            grvProducts.DataBind();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (UpdateProductDetails() == 0)
                {
                    ErrorMessage("Unable to updated the product details !");
                }
                else {
                    GetProductDetails();
                    this.Page_Load(sender, e);
                    SucessMessage("Product is updated successfully.");
                }
            }
            else {
                ErrorMessage("Please fill the required field details!");
            }
        }
        private int UpdateProductDetails()
        {
            try
            {

                //SqlCommand cmd = new SqlCommand("dbo.stCRUDProductDetails", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ViewState["ProductID"]));
                //cmd.Parameters.AddWithValue("@ProductDesc", txtProductDesc.Text);
                //cmd.Parameters.AddWithValue("@Type", "U");
                //cmd.Parameters.AddWithValue("@SKUNo", txtSKUNo.Text);
                //cmd.Parameters.AddWithValue("@LastUserID", username);
                //cmd.ExecuteNonQuery();

                ProductsEntity pe = new ProductsEntity();
                pe.productid = Convert.ToInt32(hdnproductid.Value);
                pe.ProductDesc = txtProductDesc.Text;
                pe.SKUNo = txtSKUNo.Text;
                pe.ItemDesc = txtItemDesc.Text;  
                pe.StorageCondition = txtStoragecond.Text;
                pe.MasterFormat = Convert.ToInt32(ddlMasterLabels.SelectedValue);
                pe.InsertFormat = Convert.ToInt32(ddlInsertLabelFormat.SelectedValue);
                pe.CaseFormat = Convert.ToInt32(ddlCaseLabelFormat.SelectedValue);
                //if (txtUnitsize.Text != "") {
                //    pe.Unitsize = Convert.ToInt32(txtUnitsize.Text);
                //}   
                if (txtUnits.Text != "")
                {
                    pe.Units = Convert.ToInt32(txtUnits.Text);
                }
                if (rbtUnits.Checked)
                {
                    pe.UnitType = 1;
                }
                else if (rbtBags.Checked)
                {
                    pe.UnitType = 2;
                }
                //pe.UnitType = Convert.ToInt32(rdUnit.SelectedItem.Value);
                pe.categorycode = Convert.ToInt32(ddlCategory.SelectedItem.Value);
                //pe.casesize = Convert.ToInt32(txtCasesize.Text);
                pe.LastUserID = Convert.ToInt32(Session["UserID"]);
                return st_dll.UpdateProduct(pe);

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
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
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {


                if (ViewState["ProductDetails"] != null)
                {

                    DataTable dt = (DataTable)ViewState["ProductDetails"];
                    DataView view = new DataView();
                    string fieldName = ddFilter.SelectedItem.Value;

                    if (fieldName == "ProductDesc" || fieldName == "SKUNo" )
                    {
                        var query = from t in dt.AsEnumerable()
                                    where t.Field<string>(fieldName).Contains(txtfilter.Text)
                                    select t;
                        view = query.AsDataView();
                        grvProducts.DataSource = view;
                        grvProducts.DataBind();

                    }

                    else if (fieldName == "0")
                    {
                        grvProducts.DataSource = dt;
                        grvProducts.DataBind();

                    }
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
                DataTable dt = (DataTable)ViewState["ProductDetails"];
                grvProducts.DataSource = dt;
                grvProducts.DataBind();
                txtfilter.Text = "";
                ddFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }

        }
        protected void btnTestPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("testprint.aspx");
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        

    }

}