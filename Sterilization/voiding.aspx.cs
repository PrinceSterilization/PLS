//voiding.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: voiding the labels
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
    public partial class voiding : System.Web.UI.Page
    {
        GPLS_DLL st_dll = new GPLS_DLL();
        UsersDLL ud = new UsersDLL();
        int batchid = 0;
        int catid = 0;
        public List<VoidDetails> _voideddetails = new List<VoidDetails>();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] != null)
            {
                if (!IsPostBack)
                {

                    EmptyGrid();
                    Session["myList"] = null;
                }
                txtLabel.Focus();

                if (Request.QueryString["batchid"] != null)
                {
                    batchid = Convert.ToInt32(Request.QueryString["batchid"]);
                }
                if (Request.QueryString["categorycode"] != null)
                {
                    catid = Convert.ToInt32(Request.QueryString["categorycode"]);
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
            string parameter = Request["__EVENTARGUMENT"];
            if (parameter == "void")
            {
                voidLabels();
            }
        }
        //private void BindVoidReasons() {
        //    try
        //    {
        //        DataTable dt = ud.GetVoidingReasons();
        //        if (dt.Rows.Count > 0){
        //            ddlStatus.DataSource = dt;
        //            ddlStatus.DataTextField = "ReasonDesc";
        //            ddlStatus.DataValueField = "ReasonID";
        //            ddlStatus.DataBind();
        //           // ddlStatus.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }
        //        else {
        //            ErrorMessage("Failed to retrive void reasons");
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        ErrorMessage("Failed to retrive void reasons");
        //    }
        //}
        private void BindGrid()
        {
            try
            {
                if (Session["myList"] != null)
                {
                    List<VoidDetails> myList = (List<VoidDetails>)Session["myList"];
                    if (myList.Count > 0)
                    {
                        grvVoidLabels.DataSource = myList;
                        grvVoidLabels.DataBind();
                    }
                    else {
                        EmptyGrid();
                        ErrorMessage("Failed to retrive user details");
                    }
                }
                else {
                    EmptyGrid();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public class VoidDetails
        {
            public string productdesc { get; set; }
            public string clabelno { get; set; }
            public string status { get; set; }

            public VoidDetails()
            {
            }
            public VoidDetails(string p_productdesc, string p_clabelno, string p_status)
            {
                productdesc = p_productdesc;
                clabelno = p_clabelno;
                status = p_status;
            }
        }
        protected void EmptyGrid()
        {
            grvVoidLabels.DataSource = new List<String>();
            grvVoidLabels.DataBind();
        }
        private void ErrorMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", "ErrorMessage('" + msg + "');", true);

        }
        private void SucessMessage(string msg)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", "SuccessMessage('" + msg + "');", true);
        }
        public void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;
            string labelno = (string)list.SelectedValue;
            GridViewRow row = (GridViewRow)list.Parent.Parent;
            int idx = row.RowIndex;
            string lblno = grvVoidLabels.Rows[idx].Cells[0].Text.ToString();
            List<VoidDetails> myList = (List<VoidDetails>)Session["myList"];
            var obj = myList.FirstOrDefault(x => x.clabelno == lblno);
            if (obj != null) obj.status = labelno;
            BindGrid();
        }

        public void txtLabel_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string label = txtLabel.Text;
                int controlid = Convert.ToInt32(label.Split('-')[0]);
                int categorycode = Convert.ToInt32(label.Split('-')[1]);
                int labelno = Convert.ToInt32(label.Split('-')[2].TrimStart('0'));
                if (catid == categorycode)
                {


                    bool result = FindLoginInSession(label);
                    if (!result)
                    {
                        DataTable dt = st_dll.GetVoidLabelDetails(controlid, categorycode, labelno, batchid);

                        if (dt.Rows.Count > 0)
                        {
                            //_voideddetails.Add( new VoidDetails(dt.Rows[0]["PRODUCTDESC"].ToString(), dt.Rows[0]["PRODUCTDESC"].ToString(),dt.Rows[0]["PRODUCTDESC"].ToString()));

                            if (Session["myList"] == null)
                                Session["myList"] = new List<VoidDetails>();

                            List<VoidDetails> myList = (List<VoidDetails>)Session["myList"];
                            myList.Add(new VoidDetails(dt.Rows[0]["PRODUCTDESC"].ToString(), dt.Rows[0]["CLABELNO"].ToString(), dt.Rows[0]["STATUS"].ToString()));

                            Session["myList"] = myList;
                            txtLabel.Text = "";
                        }
                        else {
                            txtLabel.Text = "";
                            ErrorMessage("Label is not printed or already voided ");
                        }
                    }
                    else {
                        txtLabel.Text = "";
                        ErrorMessage("You cannot scan the diffrent label.");
                    }
                    BindGrid();
                }
                else {
                    txtLabel.Text = "";
                    ErrorMessage("You cannot scan the login multiple times.");
                }
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        public bool FindLoginInSession(string label)
        {
            List<VoidDetails> myList = (List<VoidDetails>)Session["myList"];
            if (myList != null)
            {
                var obj = myList.FirstOrDefault(x => x.clabelno == label);
                if (obj != null)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }


        }
        protected void grvVoidLabels_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DropDownList dd = e.Row.FindControl("ddlStatus") as DropDownList;
                string ssfid = (e.Row.FindControl("lblStatus") as Label).Text;
                dd.Items.FindByValue(ssfid).Selected = true;

            }
        }

        public void voidLabels()
        {
            List<VoidDetails> myList = (List<VoidDetails>)Session["myList"];

            try
            {
                if (myList.Count > 0)
                {
                    ProductsEntity pe;
                    foreach (VoidDetails lst in myList)
                    {
                        pe = new ProductsEntity();
                        pe.ControlID = Convert.ToInt32(lst.clabelno.Split('-')[0]);
                        pe.LabelNo = Convert.ToInt32(lst.clabelno.Split('-')[2].TrimStart('0'));
                        pe.StatusReason = Convert.ToInt32(lst.status);
                        pe.VoidedByID = Convert.ToInt32(Context.Session["UserID"]);
                        pe.categorycode = Convert.ToInt32(lst.clabelno.Split('-')[1]);
                        pe.batchid = batchid;
                        int result = st_dll.SaveVoideddata(pe);
                    }
                    Session["myList"] = null;
                    BindGrid();
                    SucessMessage("Total " + myList.Count + " labels are voided successfully.");
                }
                else {
                    ErrorMessage("Please scan the lables to void.");
                }
            }
            catch (Exception)
            {
                ErrorMessage("Error in voiding the label.");
            }
        }
    }
}