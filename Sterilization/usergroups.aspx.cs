//usergroups.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: User Groups page 
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class usergroups : System.Web.UI.Page
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
            grvUserGroups.DataSource = new List<String>();
            grvUserGroups.DataBind();
        }
        public void BindGrid()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetUserGroups();

                if (dt.Rows.Count > 0)
                {
                    ViewState["UserGroupsData"] = dt;
                    grvUserGroups.DataSource = dt;
                    grvUserGroups.DataBind();
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

            DataTable dt = (DataTable)ViewState["UserGroupsData"];

            SetSortDirection(SortDireaction);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                grvUserGroups.DataSource = dt;
                grvUserGroups.DataBind();
                SortDireaction = _sortDirection;
                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in grvUserGroups.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = grvUserGroups.HeaderRow.Cells.GetCellIndex(headerCell);
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

        public int AddUserGroupDetails()
        {
            try
            {
                UserGroups ug = new UserGroups();
                ug.Groupcode = txtGroupCode.Text;
               // ug.GroupId = Convert.ToInt32(txtGroupID.Text);
                ug.GroupName = txtGroupName.Text;
                ug.CreatedBy = Convert.ToInt32(Session["UserID"]);
                ug.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.AddUserGroup(ug);

            }
            catch (Exception ex)
            {

                ErrorMessage("Adding user group error:2 " + ex.Message);
                return 0;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (AddUserGroupDetails() == 0)
                {
                    ErrorMessage("Unable to add the user group details !");
                }
                else {
                    BindGrid();
                    SucessMessage("UserGroup is added successfully.");
                }
            }
            else {
                ErrorMessage("Please fill the required field details!");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            grvUserGroups.SelectedIndex = -1;
            //btnUpdate.Visible = false;
            //btnSave.Enabled = true;
            txtGroupCode.Text = "";
           // txtGroupID.Text = "";
            txtGroupName.Text = "";

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (UpdateUserGroupDetails() == 0)
            {
                ErrorMessage("Unable to updated the user group !");
            }
            else {
                BindGrid();
                SucessMessage("User group is updated successfully.");
            }

        }
        private int UpdateUserGroupDetails()
        {
            try
            {


                UserGroups ug = new UserGroups();
                ug.GroupId = Convert.ToInt32(hdngroupid.Value);
                ug.Groupcode = txtGroupCode.Text;
                ug.GroupName = txtGroupName.Text;
                ug.LastUserID = Convert.ToInt32(Session["UserID"]);
                user_dll = new UsersDLL();
                return user_dll.UpdateUserGroup(ug);

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
            }

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {

            if (ViewState["UserGroupsData"] != null)
            {

                DataTable dt = (DataTable)ViewState["UserGroupsData"];
                DataView view = new DataView();
                string fieldName = ddFilter.SelectedItem.Value;

                if (fieldName == "GroupCode" || fieldName == "GroupName")
                {
                    var query = from t in dt.AsEnumerable()
                                where t.Field<string>(fieldName).Contains(txtfilter.Text)
                                select t;
                    view = query.AsDataView();
                    grvUserGroups.DataSource = view;
                    grvUserGroups.DataBind();

                }

                else if (fieldName == "0")
                {
                    grvUserGroups.DataSource = dt;
                    grvUserGroups.DataBind();

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