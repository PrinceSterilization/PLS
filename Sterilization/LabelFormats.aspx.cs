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
    public partial class LabelFormats : System.Web.UI.Page
    {
        GPLS_DLL st_dll = new GPLS_DLL();
        private string _sortDirection;
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
                    BindGrid();
                    BindProductDropdowns();
                }
            }
            else {
                Response.Redirect("Login.aspx");

            }
        }
        private void ShowEmptyGrid()
        {
            grvLabelFormats.DataSource = new List<String>();
            grvLabelFormats.DataBind();
        }
        public void BindProductDropdowns()
        {
            try
            {

                DataTable dt = st_dll.GetDataTable("dbo.spGetProducts");
                ddlAddProducts.DataSource = dt;
                ddlAddProducts.DataTextField = "ProductDesc";
                ddlAddProducts.DataValueField = "ProductID";
                ddlAddProducts.DataBind();
                ddlAddProducts.Items.Insert(0, new ListItem("--Select--", "0"));


                ddlCopyProducts.DataSource = dt;
                ddlCopyProducts.DataTextField = "ProductDesc";
                ddlCopyProducts.DataValueField = "ProductID";
                ddlCopyProducts.DataBind();
                ddlCopyProducts.Items.Insert(0, new ListItem("--Select--", "0"));

                ddlUpProducts.DataSource = dt;
                ddlUpProducts.DataTextField = "ProductDesc";
                ddlUpProducts.DataValueField = "ProductID";
                ddlUpProducts.DataBind();
                ddlUpProducts.Items.Insert(0, new ListItem("--Select--", "0"));


            }
            catch (Exception ex)
            {

                ErrorMessage("Dropdown binding error " + ex.Message);
            }

        }
        public void BindGrid()
        {
            try
            {
                DataTable dt = st_dll.GetLabelFormats();

                if (dt.Rows.Count > 0)
                {
                    ViewState["LabelData"] = dt;
                    grvLabelFormats.DataSource = dt;
                    grvLabelFormats.DataBind();
                }
                else {
                    ShowEmptyGrid();
                }

            }
            catch (Exception ex)
            {

                ErrorMessage("Grid binding error:1 " + ex.Message);
            }

        }
        protected void btnUp_Update_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.productid = Convert.ToInt32(ddlUpProducts.SelectedValue);
                pe.categorycode = Convert.ToInt32(ddlUpCategory.SelectedValue);
                pe.formatno = Convert.ToInt32(ddlUpFormats.SelectedValue);
                pe.LastUserID = Convert.ToInt32(Session["UserID"]);
                pe.copyproduct = hdnUpdate.Value.ToString();
                int result = st_dll.UpdateLabelFormat(pe);
                if (result == 1)
                {
                    SucessMessage("Format label updated successfully.");
                    BindGrid();
                }
                else {
                    ErrorMessage("Error while updating.");
                }

            }
            catch (Exception ex)
            {

                ErrorMessage("Update error " + ex.Message);
            }
        }
            
        protected void btnCopy_Save_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                string item = hdncopyitem.Value.ToString();
                if (item != "") {
                    pe.productid = Convert.ToInt32(ddlCopyProducts.SelectedValue);
                    //pe.categorycode = Convert.ToInt32(item.Split('-')[1]);
                    //pe.formatno = Convert.ToInt32(item.Split('-')[2]);
                    pe.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    pe.copyproduct = item;
                    int result = st_dll.CopyLabelFormat(pe);
                    if(result == 1)
                    {
                        SucessMessage("Format label copied successfully.");
                        BindGrid();
                    }
                    else{
                        ErrorMessage("Error while copying.");
                    }
                }
                else{
                    ErrorMessage("Error while copying.");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("Copy error " + ex.Message);
            }

        }
            
        protected void btnAddSave_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsEntity pe = new ProductsEntity();
                pe.productid = Convert.ToInt32(ddlAddProducts.SelectedValue);
                pe.categorycode = Convert.ToInt32(ddlAddCategory.SelectedValue);
                pe.formatno = Convert.ToInt32(ddlAddFormats.SelectedValue);
                pe.CreatedBy = Convert.ToInt32(Session["UserID"]);
                int result = st_dll.InsertLabelFormat(pe);

                if (result == 1)
                {
                    SucessMessage("Format label added successfully.");
                    BindGrid();
                   // AddBartenderFileToFolder(pe);
                }
                else {

                    ErrorMessage("Error in adding the format label.");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage("Adding error " + ex.Message);
            }
        }
        public void AddBartenderFileToFolder(ProductsEntity pe)
        {
            try
            {
                string filename = "SKU" + pe.productid.ToString() + "_" + pe.categorycode.ToString() + "_" + pe.formatno.ToString();

                string folderName = @"\\glpdc01\corp\IT Files\GPLS\BarTenderFiles\";
                string fileName = filename + ".btw";

                string pathString = System.IO.Path.Combine(folderName, fileName);

                if (!System.IO.File.Exists(pathString))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(pathString)) {

                    }
                }                
            }
            catch (Exception ex)
            {

                ErrorMessage("Error in adding file to folder " + ex.Message);
            }
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {

            if (ViewState["LabelData"] != null)
            {

                DataTable dt = (DataTable)ViewState["LabelData"];
                DataView view = new DataView();
                string fieldName = ddFilter.SelectedItem.Value;

                if (fieldName == "GroupCode" || fieldName == "GroupName")
                {
                    var query = from t in dt.AsEnumerable()
                                where t.Field<string>(fieldName).Contains(txtfilter.Text)
                                select t;
                    view = query.AsDataView();
                    grvLabelFormats.DataSource = view;
                    grvLabelFormats.DataBind();

                }

                else if (fieldName == "0")
                {
                    grvLabelFormats.DataSource = dt;
                    grvLabelFormats.DataBind();

                }
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
                txtfilter.Text = "";
                ddFilter.SelectedIndex = 0;
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
        protected void grvLabelFormats_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grvLabelFormats_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grvLabelFormats_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}