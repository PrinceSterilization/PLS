//Inventory.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Inventory page

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
    public partial class Inventory : System.Web.UI.Page
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
        protected void BindProductDroDown()
        {
            try
            {
                DataTable dt = st_dll.GetDataTable("dbo.spComponents");
                if (dt.Rows.Count > 0)
                {
                    ddlcomponent.DataSource = dt;
                    ddlcomponent.DataTextField = "ProductDesc";
                    ddlcomponent.DataValueField = "ProductID";
                    ddlcomponent.DataBind();
                    ddlcomponent.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }
        }
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (ddlType.SelectedValue != "0")
            {
                string reportId = ddlType.SelectedValue;

                switch (reportId)
                {
                    //case "1":
                    //    GenerateReport(1,"STOCK CLEAN ROOM");
                    //    break;
                    //case "2":
                    //    GenerateReport(2,"TAKEOUT FROM CLEAN ROOM");
                    //    break;
                    //case "3":
                    //    GenerateReport(3,"STOCK IN WAREHOUSE");
                    //    break;
                    //case "4":
                    //    GenerateReport(4,"TAKEOUT FROM WAREHOUSE");
                    //    break;
                    case "1":
                        GenerateCaseBoxReport(1, "WAREHOUSE INVENTORY");
                        break;
                    case "2":
                        GenerateQuarantineBoxReport(2, "QUARANTINE INVENTORY");
                        break;
                    default:
                        break;
                }
            }
            else {
                ErrorMessage("Please select the report.");
            }
        }
        protected void GenerateQuarantineBoxReport(int rptId, string name)
        {
            try
            {

                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                DataTable dt = us_dll.GetReportData("dbo.spProductInvty");
                if (dt.Rows.Count > 0)
                {
                    rptDoc.Load(Server.MapPath("~/Reports/ProductInvty.rpt"));
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "GPLS");
                    rptDoc.DataSourceConnections[0].SetConnection("GBLNJ4", "GPLS", "sa", "Pass2012");
                    //CrystalReportViewer1.Page.Title = name;
                    rptDoc.SummaryInfo.ReportTitle = name;
                    // rptDoc.DataDefinition.FormulaFields["cTitle"].Text = "'" + name + "'";
                    CrystalReportViewer1.ReportSource = rptDoc;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                 Response, true, name);
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
        protected void GenerateCaseBoxReport(int rptId, string name)
        {
            try
            {
                
                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                DataTable dt = us_dll.GetReportData("dbo.spCaseBoxInvty");
                if (dt.Rows.Count > 0)
                {
                    rptDoc.Load(Server.MapPath("~/Reports/CaseBoxInvty.rpt"));                
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "GPLS");
                    rptDoc.DataSourceConnections[0].SetConnection("GBLNJ4", "GPLS", "sa", "Pass2012");
                    //CrystalReportViewer1.Page.Title = name;
                    rptDoc.SummaryInfo.ReportTitle = name;
                   // rptDoc.DataDefinition.FormulaFields["cTitle"].Text = "'" + name + "'";
                    CrystalReportViewer1.ReportSource = rptDoc;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                 Response, true, name);
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
        protected void GenerateReport(int rptId,string name)
        {
            try
            {
                int productid = Convert.ToInt32(ddlType.SelectedValue);
                DataSet rptDS = new DataSet();
                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                string type="";
                if (rptId == 1)
                {
                    type= "IN_CLEAN_ROOM";
                }
                else if (rptId == 2)
                {
                    type = "OUT_CLEAN_ROOM";
                }
                else if (rptId == 3)
                {
                    type = "IN_WAREHOUSE";
                }
                else if (rptId == 4)
                {
                    type = "OUT_WAREHOUSE";
                }                
                DataTable dt = us_dll.GetInventoryReportData(productid, type);
                if (dt.Rows.Count > 0)
                {
                    rptDoc.Load(Server.MapPath("~/Reports/rptInventory.rpt"));                   
                    rptDoc.SetParameterValue("@TYPE", type);                  
                    rptDoc.SetParameterValue("@PRODUCTID", productid);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "GPLS");
                    rptDoc.DataSourceConnections[0].SetConnection("GBLNJ4", "GPLS", "sa", "Pass2012");
                    CrystalReportViewer1.Page.Title = name;
                    rptDoc.SummaryInfo.ReportTitle = name;
                    rptDoc.DataDefinition.FormulaFields["cTitle"].Text = "'"+ name + "'";                    
                    CrystalReportViewer1.ReportSource = rptDoc;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = "application/pdf";
                    rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                 Response, true, name);
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