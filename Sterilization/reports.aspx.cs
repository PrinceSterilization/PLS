//reports.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: reports page

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
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
    public partial class reports : System.Web.UI.Page
    {
        UsersDLL us_dll = new UsersDLL();
        GPLS_DLL st_dll = new GPLS_DLL();
        ReportDocument rptDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                if (!IsPostBack)
                {
                    BindProductDroDown();
                }
            }
            else {
                Response.Redirect("Login.aspx");
            }
        }
        protected void BindProductDroDown() {
            try
            {
                DataTable dt = us_dll.GetProductDetails();
                if (dt.Rows.Count > 0) {
                    ddlproduct.DataSource = dt;
                    ddlproduct.DataTextField = "PRODUCTDESC";
                    ddlproduct.DataValueField = "PRODUCTID";
                    ddlproduct.DataBind();
                    ddlproduct.Items.Insert(0, new ListItem("--Select--", "0"));
                }                
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (ddlReport.SelectedValue != "0")
            {
                string reportId = ddlReport.SelectedValue;

                switch (reportId)
                {
                    case "1":
                        GenerateListOfVoidedLabels();
                        break;
                    case "2":
                        GenerateListOfAddedLabels();
                        break;
                    case "3":
                        GenerateBatchReport();
                        break;
                    case "4":
                        GenerateListOfRejectedLabels();
                        break;
                    default:
                        break;
                }               
            }
            else {
                ErrorMessage("Please select the report type.");
            }
          
        }

        protected void GenerateListOfRejectedLabels()
        {
            try
            {

                int productid = Convert.ToInt32(ddlproduct.SelectedValue);


                DataSet rptDS = new DataSet();
                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                DataTable dt = us_dll.GetListOfRejectedLabels(productid);
                if (dt.Rows.Count > 0)
                {

                    rptDoc.Load(Server.MapPath("~/Reports/rptListOfRejectedLabels.rpt"));
                    rptDoc.SetParameterValue("@PRODUCTID", productid);
                    rptDoc.SetDataSource(dt);
                    //rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "Sterilization");
                    rptDoc.SetDatabaseLogon("sa", "Pass2018", "pssql01", "PLSTEST1");
                    //rptDoc.DataSourceConnections[0].SetConnection("GBLNJ4", "Sterilization", "sa", "Pass2012");
                    rptDoc.DataSourceConnections[0].SetConnection("Pssql01", "PLS", "sa", "Pass2018");
                    CrystalReportViewer1.Page.Title = "List of Rejected Labels";
                    rptDoc.SummaryInfo.ReportTitle = "List of Rejected Labels";
                    CrystalReportViewer1.ReportSource = rptDoc;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                 Response, true, "List of Rejected Labels");
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                    CrystalReportViewer1.SeparatePages = true;
                    CrystalReportViewer1.DataBind();
                    CrystalReportViewer1.HasExportButton = false;
                    Response.Flush();
                    Response.Close();
                }
                else {
                    ErrorMessage("Current selection does not have data");

                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignore it
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        protected void GenerateBatchReport()
        {
            try
            {
               
                int productid = Convert.ToInt32(ddlproduct.SelectedValue);

               
                DataSet rptDS = new DataSet();
                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                DataTable dt = us_dll.GetProductReportData(productid);
                if (dt.Rows.Count > 0)
                {

                    rptDoc.Load(Server.MapPath("~/Reports/ProductLabelReport.rpt"));                    
                    rptDoc.SetParameterValue("@PRODUCTID", productid);                  
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "Sterilization");
                    rptDoc.DataSourceConnections[0].SetConnection("GBLNJ4", "Sterilization", "sa", "Pass2012");
                    CrystalReportViewer1.Page.Title = "Product Label Data";
                    rptDoc.SummaryInfo.ReportTitle = "Product Label Data";
                    CrystalReportViewer1.ReportSource = rptDoc;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                 Response, true, "Product Label Data");
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                    CrystalReportViewer1.SeparatePages = true;
                    CrystalReportViewer1.DataBind();
                    CrystalReportViewer1.HasExportButton = false;
                    Response.Flush();
                    Response.Close();
                }
                else {
                    ErrorMessage("Current selection does not have data");

                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignore it
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        protected void GenerateListOfAddedLabels()
        {
            try
            {
                
                int productid = Convert.ToInt32(ddlproduct.SelectedValue);


                DataSet rptDS = new DataSet();
                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                DataTable dt = us_dll.GetListOfAddedLabels(productid);
                if (dt.Rows.Count > 0)
                {

                    rptDoc.Load(Server.MapPath("~/Reports/rptListOfAddedLabels.rpt"));
                    rptDoc.SetParameterValue("@PRODUCTID", productid);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "Sterilization");
                    rptDoc.DataSourceConnections[0].SetConnection("GBLNJ4", "Sterilization", "sa", "Pass2012");
                    CrystalReportViewer1.Page.Title = "List Of Added Labels";
                    rptDoc.SummaryInfo.ReportTitle = "List Of Added Labels";
                    CrystalReportViewer1.ReportSource = rptDoc;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                 Response, true, "List Of Added Labels");
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                    CrystalReportViewer1.SeparatePages = true;
                    CrystalReportViewer1.DataBind();
                    CrystalReportViewer1.HasExportButton = false;
                    Response.Flush();
                    Response.Close();
                }
                else {
                    ErrorMessage("Current selection does not have data");

                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignore it
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }
        protected void GenerateListOfVoidedLabels()
        {
            try
            {

                int productid = Convert.ToInt32(ddlproduct.SelectedValue);


                DataSet rptDS = new DataSet();
                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                DataTable dt = us_dll.GetListOfVoidedLabels(productid);
                if (dt.Rows.Count > 0)
                {

                    rptDoc.Load(Server.MapPath("~/Reports/rptListOfVoidedLabels.rpt"));
                    rptDoc.SetParameterValue("@PRODUCTID", productid);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "Sterilization");
                    rptDoc.DataSourceConnections[0].SetConnection("GBLNJ4", "Sterilization", "sa", "Pass2012");
                    CrystalReportViewer1.Page.Title = "List Of Voided Labels";
                    rptDoc.SummaryInfo.ReportTitle = "List Of Voided Labels";
                    CrystalReportViewer1.ReportSource = rptDoc;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                 Response, true, "List Of Voided Labels");
                    CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                    CrystalReportViewer1.SeparatePages = true;
                    CrystalReportViewer1.DataBind();
                    CrystalReportViewer1.HasExportButton = false;
                    Response.Flush();
                    Response.Close();
                }
                else {
                    ErrorMessage("Current selection does not have data");

                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignore it
            }
            catch (Exception ex)
            {

                ErrorMessage(ex.Message);
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rptDoc != null)
            {
                rptDoc.Close();
                rptDoc.Dispose();
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