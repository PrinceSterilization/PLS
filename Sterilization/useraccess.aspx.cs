//useraccess.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: User Access page 

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class useraccess : System.Web.UI.Page
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
                    BindUsers();
                    BindPages();
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }
        private void ShowEmptyGrid()
        {
            grvUserAccess.DataSource = new List<String>();
            grvUserAccess.DataBind();
        }
        public void BindGrid()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetAllUsersAccess();
                if (dt.Rows.Count > 0)
                {
                    ViewState["UserAccessData"] = dt;
                    grvUserAccess.DataSource = dt;
                    grvUserAccess.DataBind();
                }
                else {
                    ShowEmptyGrid();
                }

                
            }
            catch (Exception ex)
            {

                ErrorMessage("Grid Binding Error:1 " + ex.Message);
            }

        }
        public void BindUsers()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetUsers();
                ddlUsers.DataSource = dt;
                ddlUsers.DataTextField = "UserName";
                ddlUsers.DataValueField = "UserID";
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {

                ErrorMessage("Users dropdown binding error:1 " + ex.Message);
            }

        }
        public void BindPages()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetSystemPages();
                ddlPages.DataSource = dt;
                ddlPages.DataTextField = "PageName";
                ddlPages.DataValueField = "PageID";
                ddlPages.DataBind();
                ddlPages.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {

                ErrorMessage("System pages dropdown binding error:2 " + ex.Message);
            }

        }
        protected void grvUserAccess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }
        protected void grvUserAccess_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dt = (DataTable)ViewState["UserAccessData"];

            SetSortDirection(SortDireaction);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                grvUserAccess.DataSource = dt;
                grvUserAccess.DataBind();
                SortDireaction = _sortDirection;
                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in grvUserAccess.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = grvUserAccess.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }

                //grvProducts.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }
        public int AddUserAccess()
        {
            try
            {
                Users u = new Users();
                u.UserID = Convert.ToInt32(ddlUsers.SelectedItem.Value);
                u.PageID = Convert.ToInt32(ddlPages.SelectedItem.Value);
                u.AccessLevel = ddlAccessLevel.SelectedItem.Value;
                u.CreatedByID = Convert.ToInt32(Session["UserID"]);
                u.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.AddUserAccess(u);

            }
            catch (Exception ex)
            {

                ErrorMessage("Adding user access error:3 " + ex.Message);
                return 0;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (AddUserAccess() == 0)
                {
                    ErrorMessage("Unable to add the user access details !");
                }
                else {
                    BindGrid();
                    SucessMessage("User access is added successfully.");
                }
            }
            else {
                ErrorMessage("Please fill the required field details!");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            grvUserAccess.SelectedIndex = -1;
            ddlAccessLevel.SelectedIndex = -1;
            ddlPages.SelectedIndex = -1;
            ddlUsers.SelectedIndex = -1;
            //btnUpdate.Visible = false;
            //btnSave.Enabled = true;
            //txtGroupCode.Text = "";
            // txtGroupID.Text = "";
            // txtGroupName.Text = "";

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (UpdateUserAccess() == 0)
            {
                ErrorMessage("Unable to updated the user access!");
            }
            else {
                BindGrid();
                SucessMessage("User access is updated successfully.");
            }

        }
        private int UpdateUserAccess()
        {
            try
            {


                Users u = new Users();
                u.UserID = Convert.ToInt32(ddlUsers.SelectedItem.Value);
                u.AccessLevel = ddlAccessLevel.SelectedItem.Value;
                u.PageID = Convert.ToInt32(ddlPages.SelectedItem.Value);
                u.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.UpdateUserAccess(u);

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
            }

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {

            if (ViewState["UserAccessData"] != null)
            {

                DataTable dt = (DataTable)ViewState["UserAccessData"];
                DataView view = new DataView();
                string fieldName = ddFilter.SelectedItem.Value;

                if (fieldName == "PageName" || fieldName == "UserName")
                {
                    var query = from t in dt.AsEnumerable()
                                where t.Field<string>(fieldName).Contains(txtfilter.Text)
                                select t;
                    view = query.AsDataView();
                    grvUserAccess.DataSource = view;
                    grvUserAccess.DataBind();

                }

                else if (fieldName == "0")
                {
                    grvUserAccess.DataSource = dt;
                    grvUserAccess.DataBind();

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

        protected void grvUserAccess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
