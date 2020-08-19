
//users.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: User  page 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class users : System.Web.UI.Page
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
                    BindUserGroupDropDown();
                    BindUserDropDown();
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }
        private void ShowEmptyGrid()
        {
            grvUser.DataSource = new List<String>();
            grvUser.DataBind();
        }
        public void BindGrid()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetUsers();
                if (dt.Rows.Count > 0)
                {
                    ViewState["UserData"] = dt;
                    grvUser.DataSource = dt;
                    grvUser.DataBind();
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
        public void BindUserGroupDropDown()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetUserGroups();
                ddlUserGroup.DataSource = dt;
                ddlUserGroup.DataTextField = "GroupName";
                ddlUserGroup.DataValueField = "GroupID";
                ddlUserGroup.DataBind();
                ddlUserGroup.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {

                ErrorMessage("User group dropdown binding error:1 " + ex.Message);
            }

        }
        public void BindUserDropDown()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetGISUsers();
                ddlUsers.DataSource = dt;
                ddlUsers.DataTextField = "UserName";
                ddlUsers.DataValueField = "UserID";
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {

                ErrorMessage("User dropdown binding error:2 " + ex.Message);
            }

        }
        protected void grvUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }
        protected void grvUser_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dt = (DataTable)ViewState["UsersData"];

            SetSortDirection(SortDireaction);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                grvUser.DataSource = dt;
                grvUser.DataBind();
                SortDireaction = _sortDirection;
                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in grvUser.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = grvUser.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }

                //grvProducts.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }
        public int AddUserDetails()
        {
            try
            {
                Users u = new Users();
                u.UserID = Convert.ToInt32(ddlUsers.SelectedItem.Value);
                u.GroupID = Convert.ToInt32(ddlUserGroup.SelectedItem.Value);
                u.LevelCode = Convert.ToInt32(ddlLevelCode.SelectedItem.Value);
                u.UserType = Convert.ToInt32(ddlUserType.SelectedItem.Value);
                u.CreatedByID = Convert.ToInt32(Session["UserID"]);
                u.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.AddUser(u);

            }
            catch (Exception ex)
            {

                ErrorMessage("Adding User Error:3 " + ex.Message);
                return 0;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (AddUserDetails() == 0)
                {
                    ErrorMessage("Unable to add the user details !");
                }
                else {
                    BindGrid();
                    SucessMessage("User is added successfully.");
                }
            }
            else {
                ErrorMessage("Please fill the required field details!");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            grvUser.SelectedIndex = -1;
            ddlLevelCode.SelectedIndex = -1;
            ddlUserGroup.SelectedIndex = -1;
            ddlUsers.SelectedIndex = -1;
            ddlUserType.SelectedIndex = -1;
            //btnUpdate.Visible = false;
            //btnSave.Enabled = true;
            //txtGroupCode.Text = "";
            // txtGroupID.Text = "";
           // txtGroupName.Text = "";

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (UpdateUserDetails() == 0)
            {
                ErrorMessage("Unable to updated the user!");
            }
            else {
                BindGrid();
                SucessMessage("User is updated successfully.");
            }

        }
        private int UpdateUserDetails()
        {
            try
            {


                Users u = new Users();
                u.UserID = Convert.ToInt32(hdnuserid.Value);
                u.GroupID = Convert.ToInt32(ddlUserGroup.SelectedItem.Value);
                u.LevelCode = Convert.ToInt32(ddlLevelCode.SelectedItem.Value);
                u.UserType = Convert.ToInt32(ddlUserType.SelectedItem.Value);
                u.CreatedByID = Convert.ToInt32(Session["UserID"]);
                u.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.UpdateUser(u);

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
            }

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {

            if (ViewState["UserData"] != null)
            {

                DataTable dt = (DataTable)ViewState["UserData"];
                DataView view = new DataView();
                string fieldName = ddFilter.SelectedItem.Value;

                if (fieldName == "GroupCode" || fieldName == "GroupName")
                {
                    var query = from t in dt.AsEnumerable()
                                where t.Field<string>(fieldName).Contains(txtfilter.Text)
                                select t;
                    view = query.AsDataView();
                    grvUser.DataSource = view;
                    grvUser.DataBind();

                }

                else if (fieldName == "0")
                {
                    grvUser.DataSource = dt;
                    grvUser.DataBind();

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

    }
}