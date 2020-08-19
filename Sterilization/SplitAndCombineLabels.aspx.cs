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
    public partial class SplitAndCombineLabels : System.Web.UI.Page
    {
        private string controlid; private string batchid;

        GPLS_DLL st_dll = new GPLS_DLL();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["controlid"] != null)
            {
                controlid = Request.QueryString["controlid"].ToString();
                hdnControlid.Value = controlid;
            }
            if (Request.QueryString["batchid"] != null)
            {
                batchid = Request.QueryString["batchid"].ToString();
                hdnBatchid.Value = batchid;
            }

            txtSplitLabel.Focus();
           // BindGrid();
        }

        protected void txtSplitLabel_TextChanged(object sender, EventArgs e)
        {
            if (txtSplitLabel.Text != null || txtSplitLabel.Text != "")
            {
                string label = txtSplitLabel.Text.ToString();
                DataTable dt = st_dll.GetLabelSize(Convert.ToInt32(label.Split('-')[0]), Convert.ToInt32(label.Split('-')[1]), Convert.ToInt32(label.Split('-')[2].TrimStart('0')));
                if (dt.Rows.Count > 0)
                {
                    ViewState["currentsize"] = Convert.ToInt32(dt.Rows[0]["CASESIZE"]);
                    lblTotalLabelsLeftToSplit.InnerText = "Current size: " + dt.Rows[0]["CASESIZE"].ToString();
                    txtLabels.Focus();
                }

            }
        }

        //protected void btnSplit_Click(object sender, EventArgs e)
        //{
        //    int actualsize = Convert.ToInt32(ViewState["currentsize"]);
        //    int requestedsize = Convert.ToInt32(ViewState["requestedsize"]);
        //    int totalsize = 0;

        //    List<int> items = new List<int>();

        //    for (int i = 1; i <= requestedsize; i++) {
        //        TextBox txt = (TextBox)Page.FindControl("panelrows").FindControl("txtLB" + i);;
        //        //TextBox txt =  (TextBox)this.Master.FindControl("Content2").FindControl("txtLB" + i);
        //        totalsize += Convert.ToInt32(txt.Text);
        //        items.Add(Convert.ToInt32(txt.Text));
        //        //Convert.ToInt32()
        //    }

        //    if (totalsize == actualsize)
        //    {
        //        int result = st_dll.SplitLabels(Convert.ToInt32(controlid), Convert.ToInt32(batchid), items);
        //        if (result != -1)
        //        {
        //            SucessMessage("Saved Scucessfully");
        //        }
        //        else {
        //            ErrorMessage("Problem in spliting the labels.");
        //        }                
        //    }
        //    else {
        //        ErrorMessage("Current size doesnot match with the labels which you have enter.Please adjust the label sizes!");
        //    }
        //}

        protected void txtLabels_TextChanged(object sender, EventArgs e)
        {
            if (txtLabels.Text != null || txtLabels.Text != "")
            {
                int currentsize = Convert.ToInt32(ViewState["currentsize"]);

                int num = Convert.ToInt32(txtLabels.Text);

                if (num <= currentsize)
                {
                    hdnCurrentSize.Value = currentsize.ToString();
                    hdnRequestedSize.Value = txtLabels.Text;
                    Table t = new Table();
                    t.ID = "tblrows";
                    t.CellSpacing = 10;
                    for (int i = 1; i <= num; i++)
                    {


                        TableRow tr = new TableRow();

                        TableCell tc1 = new TableCell();
                        TableCell tc2 = new TableCell();

                        Label lbl = new Label();
                        lbl.Text = "Label" + i.ToString() + " size";

                        lbl.CssClass = "labelformat";
                        tc1.Controls.Add(lbl);


                        TextBox txt = new TextBox();
                        txt.ID = "txtLB" + i.ToString();

                        txt.CssClass = "form-control";
                        tc2.Controls.Add(txt);

                        tr.Controls.Add(tc1);
                        tr.Controls.Add(tc2);

                        t.Controls.Add(tr);

                    }

                    panelrows.Controls.Add(t);
                    // btnSplit.UseSubmitBehavior = true;
                }
                else {
                    ErrorMessage("You cannot divide the labels more than current size!");
                }
            }
            else {
                ErrorMessage("Please enter the how many lables would like to dive field!");
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
        public class Labels
        {
            public string LabelNo { get; set; }
            public string Size { get; set; }
        

            public Labels()
            {
            }
            public Labels(string p_labelNo, string p_size)
            {
                LabelNo = p_labelNo;
                Size = p_size;
               
            }
        }
        //private void BindGrid()
        //{
        //    try
        //    {
        //        if (Session["myList"] != null)
        //        {
        //            List<Labels> myList = (List<Labels>)Session["myList"];
        //            if (myList.Count > 0)
        //            {
        //                grvLabels.DataSource = myList;
        //                grvLabels.DataBind();
        //            }
        //            else {
        //                EmptyGrid();
        //                ErrorMessage("Failed to retrive user details");
        //            }
        //        }
        //        else {
        //            EmptyGrid();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //}
        //protected void EmptyGrid()
        //{
        //    grvLabels.DataSource = new List<String>();
        //    grvLabels.DataBind();
        //}
        //protected void txtCombine_TextChanged(object sender, EventArgs e)
        //{
        //    string label = txtCombine.Text;
        //    //int controlid = controlid;
        //    int categorycode = 4;
        //    int labelno = Convert.ToInt32(label.Split('-')[2].TrimStart('0'));

        //    bool result = FindLoginInSession(label);
        //    if (!result)
        //    {
        //        DataTable dt = st_dll.GetCaseSizeForCombine(Convert.ToInt32(controlid), categorycode, labelno, Convert.ToInt32(batchid));

        //        if (dt.Rows.Count > 0)
        //        {
        //            if (Session["myList"] == null)
        //                Session["myList"] = new List<Labels>();
                 
        //            List<Labels> myList = (List<Labels>)Session["myList"];
        //            myList.Add(new Labels(txtCombine.Text, dt.Rows[0]["CASESIZE"].ToString()));

        //            //myList.Add(combinelabel);


        //            //string stringHTML = @"<div class='row'>
        //            //                             <div class='col-xs-4 col-md-3'>
        //            //                                " + txtCombine.Text + @"</div> 
        //            //                                    <div class='col-xs-4 col-md-3'>
        //            //                                " + dt.Rows[0]["CASESIZE"].ToString() + @"
        //            //                                </div>
        //            //                            </div>";

                    

        //            Session["myList"] = myList;
        //            txtCombine.Text = "";

        //        }

        //    }
        //    else {
        //        txtCombine.Text = "";
        //        ErrorMessage("You cannot scan the same or diffrent label.");
        //    }


        //}
        private void CreateTable() {
            if (Session["myList"] != null) {

            }
        }
        public bool FindLoginInSession(string label)
        {
            List<Labels> myList = (List<Labels>)Session["myList"];
            if (myList != null)
            {
                var obj = myList.FirstOrDefault(x => x.LabelNo == label);
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
    }

}