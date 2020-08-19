//systempages.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Adding system page 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class systempages : System.Web.UI.Page
    {
        UsersDLL user_dll;
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
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }
        private void ShowEmptyGrid()
        {
            grvPages.DataSource = new List<String>();
            grvPages.DataBind();
        }
        public void BindGrid()
        {
            try
            {


                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetSystemPages();

                if (dt.Rows.Count > 0)
                {
                    ViewState["SystemPages"] = dt;
                    grvPages.DataSource = dt;
                    grvPages.DataBind();
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

        private void ErrorMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", "ErrorMessage('" + msg + "');", true);

        }
        private void SucessMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", "SuccessMessage('" + msg + "');", true);
        }

        protected void grvUserGroups_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dt = (DataTable)ViewState["SystemPages"];

            SetSortDirection(SortDireaction);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                grvPages.DataSource = dt;
                grvPages.DataBind();
                SortDireaction = _sortDirection;
                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in grvPages.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = grvPages.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }

                //grvProducts.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }

        protected void grvUserGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        public int AddSystemPages()
        {
            try
            {
                Users ug = new Users();
                ug.PageDesc = txtPagedesc.Text;
                // ug.GroupId = Convert.ToInt32(txtGroupID.Text);
                ug.PageName = txtPageName.Text;
                ug.CreatedByID = Convert.ToInt32(Session["UserID"]);
                ug.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.AddSystemPage(ug);

            }
            catch (Exception ex)
            {

                ErrorMessage("Adding System pages Error:2 " + ex.Message);
                return 0;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (AddSystemPages() == 0)
                {
                    ErrorMessage("Unable to add the system pages !");
                }
                else {
                    BindGrid();
                    SucessMessage("Page details are added successfully.");
                }
            }
            else {
                ErrorMessage("Please fill the required field details!");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            grvPages.SelectedIndex = -1;
            //btnUpdate.Visible = false;
            //btnSave.Enabled = true;
            txtPageName.Text = "";
            // txtGroupID.Text = "";
            txtPagedesc.Text = "";

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (UpdateSystemPage() == 0)
            {
                ErrorMessage("Unable to updated the page details !");
            }
            else {
                BindGrid();
                SucessMessage("Page details are updated successfully.");
            }

        }
        private int UpdateSystemPage()
        {
            try
            {


                Users ug = new Users();
                ug.PageID = Convert.ToInt32(hdnpageid.Value);
                ug.PageDesc = txtPagedesc.Text;
                ug.PageName = txtPageName.Text;
                ug.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.UpdateSystemPage(ug);

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
            }

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {

            if (ViewState["SystemPages"] != null)
            {

                DataTable dt = (DataTable)ViewState["SystemPages"];
                DataView view = new DataView();
                string fieldName = ddFilter.SelectedItem.Value;

                if (fieldName == "PageName" || fieldName == "PageDesc")
                {
                    var query = from t in dt.AsEnumerable()
                                where t.Field<string>(fieldName).Contains(txtfilter.Text)
                                select t;
                    view = query.AsDataView();
                    grvPages.DataSource = view;
                    grvPages.DataBind();

                }

                else if (fieldName == "0")
                {
                    grvPages.DataSource = dt;
                    grvPages.DataBind();

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
    }
}