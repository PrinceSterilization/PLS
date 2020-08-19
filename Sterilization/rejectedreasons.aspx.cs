using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class rejectedreasons : System.Web.UI.Page
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
            grvRejectedReasons.DataSource = new List<String>();
            grvRejectedReasons.DataBind();
        }
        public void BindGrid()
        {
            try
            {
                user_dll = new UsersDLL();
                DataTable dt = user_dll.GetRejectedReasons();
                if (dt.Rows.Count > 0)
                {
                    ViewState["RejectedReasons"] = dt;
                    grvRejectedReasons.DataSource = dt;
                    grvRejectedReasons.DataBind();
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

        protected void grvRejectedReasons_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }
        protected void grvRejectedReasons_Sorting(object sender, GridViewSortEventArgs e)
        {

            DataTable dt = (DataTable)ViewState["RejectedReasons"];

            SetSortDirection(SortDireaction);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                grvRejectedReasons.DataSource = dt;
                grvRejectedReasons.DataBind();
                SortDireaction = _sortDirection;
                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in grvRejectedReasons.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = grvRejectedReasons.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }

                //grvProducts.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }
        public int AddRejectedReason()
        {
            try
            {

                user_dll = new UsersDLL();
                return user_dll.AddRejectedReason(txtReasonDescription.Text.ToString(), Convert.ToInt32(Session["UserID"]));

            }
            catch (Exception ex)
            {

                ErrorMessage("Adding User Error:3 " + ex.Message);
                return 0;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (AddRejectedReason() == 0)
            {
                ErrorMessage("Unable to add the rejected reason !");
            }
            else {
                BindGrid();
                SucessMessage("Rejected reason added successfully.");
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            grvRejectedReasons.SelectedIndex = -1;
            txtReasonDescription.Text = "";

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (UpdateRejectedReason() == 0)
            {
                ErrorMessage("Unable to updated the rejected reason!");
            }
            else {
                BindGrid();
                SucessMessage("Rejected reason is updated successfully.");
            }

        }
        private int UpdateRejectedReason()
        {
            try
            {
                user_dll = new UsersDLL();
                return user_dll.UpdateRejectedReason(txtReasonDescription.Text.ToString(), Convert.ToInt32(Session["UserID"]), Convert.ToInt32(hdnreasonid.Value));

            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
                return 0;
            }

        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {

            if (ViewState["RejectedReasons"] != null)
            {

                DataTable dt = (DataTable)ViewState["RejectedReasons"];
                DataView view = new DataView();
                string fieldName = ddFilter.SelectedItem.Value;

                if (fieldName == "ReasonDesc")
                {
                    var query = from t in dt.AsEnumerable()
                                where t.Field<string>(fieldName).Contains(txtfilter.Text)
                                select t;
                    view = query.AsDataView();
                    grvRejectedReasons.DataSource = view;
                    grvRejectedReasons.DataBind();

                }

                else if (fieldName == "0")
                {
                    grvRejectedReasons.DataSource = dt;
                    grvRejectedReasons.DataBind();

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