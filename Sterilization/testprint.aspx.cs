using Seagull.BarTender.Print;
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
    public partial class testprint : System.Web.UI.Page
    {
        GPLS_DLL st_dll = new GPLS_DLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetProductsDropDown();
            }
        }
        public void GetProductsDropDown()
        {
            try
            {
                DataTable dt = st_dll.GetDataTable("dbo.spGetProductsDropDown");
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
                throw ex;
            }
        }
        protected void btnTestPrintLabels_Click(object sender, EventArgs e)
        {
            try
            {
                int productid = Convert.ToInt32(ddproducts.SelectedValue);
                string formatname = ddlInsertFormats.SelectedValue.ToString();
                int ischk = chkCase.Checked == true ? 1 : 0;
                if (productid != 0 && formatname != null) {
                    InsertBarTenderPrintingLable(productid, formatname, ischk);
                }
                               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnPrintMaster_Click(object sender, EventArgs e)
        {
            try
            {
                int productid = Convert.ToInt32(ddproducts.SelectedValue);
                string formatname = ddlMasterFormats.SelectedValue.ToString();
                if (productid != 0 && formatname != null)
                {
                    BarTenderPrintingLable(productid, formatname);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnPrintInsert_Click(object sender, EventArgs e)
        {
            try
            {
                int productid = Convert.ToInt32(ddproducts.SelectedValue);
                string formatname = ddlCaseFormats.SelectedValue.ToString();

                int ischk = chkInsert.Checked == true ? 1 : 0;
                if (productid != 0 && formatname != null)
                {
                    InsertBarTenderPrintingLable(productid, formatname, ischk);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertBarTenderPrintingLable(int productid, string formatname,int chk)
        {
            try
            {
                string btFileName = "";
                LogFile lf = new LogFile();
               
                using (Engine btEngine = new Engine())
                {

                    lf.LogMessge("S1 :" + "Inside Service");
                    btEngine.Start();
                    lf.LogMessge("S2 :" + "Service Started");
                    btEngine.Window.Visible = true;

                    // btFileName = @"\\glpdc01\corp\IT Files\GPLS\BarTenderFiles\" + filename + ".btw";

                    //btFileName = @"\\psapp01\IT Files\PLS\BarTenderFiles\" + filename + ".btw";
                    btFileName = @"\\psapp01\IT Files\PLS\BarTenderFilesTest01\" + formatname + ".btw";
                    lf.LogMessge("S3 :" + "File Path");
                    lf.LogMessge("S4 :" + "File :" + btFileName);
                    LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
                    lf.LogMessge("S5 :" + "Go the File");
                    Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
                    queryprompts["productid"].Value = productid.ToString();
                    queryprompts["chk"].Value = chk.ToString();

                    lf.LogMessge("S6 :" + "Query Promted");
                    //btFormat.PrintSetup.IdenticalCopiesOfLabel = labelcount;
                    //btFormat.PrintSetup.NumberOfSerializedLabels = 4
                    Result result = btFormat.Print(btFileName);
                    lf.LogMessge("S7 :" + "Result: " + result);
                    //Result result = btFormat.Print();
                    if (result == Result.Failure)
                    {
                        lf.LogMessge("S8 :" + "Failure ");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 0;

                    }
                    else {
                        lf.LogMessge("S9 :" + "Sucess ");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile lf1 = new LogFile();
                lf1.LogMessge("S10 :" + "Exception Date :" + DateTime.Now.ToLongTimeString() + "\nException  Message:" + ex.Message);
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "log", "log('" + ex.Message + "');", true);
                return 0;
            }
        }
        public int BarTenderPrintingLable(int productid, string formatname)
        {
            try
            {
                string btFileName = ""; 
                 LogFile lf = new LogFile();
                using (Engine btEngine = new Engine())
                {

                    lf.LogMessge("S1 :" + "Inside Service");
                    btEngine.Start();
                    lf.LogMessge("S2 :" + "Service Started");
                    btEngine.Window.Visible = true;

                    // btFileName = @"\\glpdc01\corp\IT Files\GPLS\BarTenderFiles\" + filename + ".btw";

                    //btFileName = @"\\psapp01\IT Files\PLS\BarTenderFiles\" + filename + ".btw";
                    btFileName = @"\\psapp01\IT Files\PLS\BarTenderFilesTest01\" + formatname + ".btw";
                    lf.LogMessge("S3 :" + "File Path");
                    lf.LogMessge("S4 :" + "File :" + btFileName);
                    LabelFormatDocument btFormat = btEngine.Documents.Open(btFileName);
                    lf.LogMessge("S5 :" + "Go the File");
                    Seagull.BarTender.Print.Database.QueryPrompts queryprompts = btFormat.DatabaseConnections.QueryPrompts;
                    queryprompts["productid"].Value = productid.ToString();
            
                    lf.LogMessge("S6 :" + "Query Promted");
                    //btFormat.PrintSetup.IdenticalCopiesOfLabel = labelcount;
                    //btFormat.PrintSetup.NumberOfSerializedLabels = 4
                    Result result = btFormat.Print(btFileName);
                    lf.LogMessge("S7 :" + "Result: " + result);
                    //Result result = btFormat.Print();
                    if (result == Result.Failure)
                    {
                        lf.LogMessge("S8 :" + "Failure ");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 0;

                    }
                    else {
                        lf.LogMessge("S9 :" + "Sucess ");
                        btFormat.Close(Seagull.BarTender.Print.SaveOptions.DoNotSaveChanges);
                        btEngine.Stop();
                        btEngine.Dispose();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile lf1 = new LogFile();
                lf1.LogMessge("S10 :" + "Exception Date :" + DateTime.Now.ToLongTimeString() + "\nException  Message:" + ex.Message);
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "log", "log('" + ex.Message + "');", true);
                return 0;
            }
        }
    }
}