//Audittrail.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Audit trail page
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using Sterilization.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sterilization
{
    public partial class Audittrail : System.Web.UI.Page
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
                DataTable dt = st_dll.GetDataTable("dbo.spGetProducts");
                if (dt.Rows.Count > 0)
                {
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
            if (ddlpage.SelectedValue != "0")
            {
                string reportId = ddlpage.SelectedValue;

                switch (reportId)
                {
                    case "1":
                        ProductReport();
                        break;
                    case "2":
                        ProductLabelReport();
                        break;
                    case "3":
                        Labelnumbers();
                        break;
                    default:
                        break;
                }
            }
            else {
                ErrorMessage("Please select the page.");
            }
        }


        public void Labelnumbers() {
            if (ddlproduct.SelectedValue != "0")
            {

                int productid = Convert.ToInt32(ddlproduct.SelectedValue);
               // string strPattern = "([a-z?])[_ ]?([A-Z])";

                DataTable dtAudit = new DataTable();
                dtAudit.Columns.Add("KeyDataName", typeof(string));
                dtAudit.Columns.Add("KeyDataValue", typeof(string));
                dtAudit.Columns.Add("KeyDataDesc", typeof(string));
                dtAudit.Columns.Add("DataName", typeof(string));
                dtAudit.Columns.Add("DataType", typeof(string));
                dtAudit.Columns.Add("ActionTaken", typeof(string));
                dtAudit.Columns.Add("OldValue", typeof(string));
                dtAudit.Columns.Add("NewValue", typeof(string));
                dtAudit.Columns.Add("DateDone", typeof(string));
                dtAudit.Columns.Add("DoneBy", typeof(string));

                int nProdID = productid;//Product id
                                        //Get All Label No. Activities
                DataTable dtAuLogGPLS = st_dll.ProductLabelGPLS(nProdID);
                DataTable dtAuLabelNos = st_dll.LabelNosGPLS(nProdID);
                if (dtAuLabelNos != null && dtAuLabelNos.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                    {
                        if (dtAuLabelNos.Rows[i]["DatePrinted"] != DBNull.Value)
                        {
                            string strType = dtAuLabelNos.Columns["DatePrinted"].DataType.ToString();
                            DataRow dR = dtAudit.NewRow();
                            dR["KeyDataName"] = "PRODUCT ID";
                            dR["KeyDataValue"] = nProdID.ToString();
                            dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                            dR["DataType"] = strType;
                            dR["DataName"] = "Date Printed (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                            dR["OldValue"] = DBNull.Value;
                            dR["NewValue"] = dtAuLabelNos.Rows[i]["DatePrinted"].ToString();
                            dR["ActionTaken"] = "2";
                            dR["Datedone"] = dtAuLabelNos.Rows[i]["DatePrinted"].ToString();
                            dR["DoneBy"] = st_dll.UserFullName(Convert.ToInt16(dtAuLabelNos.Rows[i]["PrintedByID"]));
                            dtAudit.Rows.Add(dR);
                        }
                    }
                    //Voided
                    for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                    {
                        if (dtAuLabelNos.Rows[i]["DateVoided"] != DBNull.Value)
                        {
                            string strType = dtAuLabelNos.Columns["DateVoided"].DataType.ToString();
                            DataRow dR = dtAudit.NewRow();
                            dR["KeyDataName"] = "PRODUCT ID";
                            dR["KeyDataValue"] = nProdID.ToString();
                            dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                            dR["DataType"] = strType;
                            dR["DataName"] = "DateVoided (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                            dR["OldValue"] = DBNull.Value;
                            dR["NewValue"] = dtAuLabelNos.Rows[i]["DateVoided"].ToString();
                            dR["ActionTaken"] = "2";
                            dR["Datedone"] = dtAuLabelNos.Rows[i]["DateVoided"].ToString();
                            dR["DoneBy"] = st_dll.UserFullName(Convert.ToInt16(dtAuLabelNos.Rows[i]["VoidedByID"]));
                            dtAudit.Rows.Add(dR);
                        }
                    }
                    //Approved
                    for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                    {
                        if (dtAuLabelNos.Rows[i]["DateApproved"] != DBNull.Value)
                        {
                            string strType = dtAuLabelNos.Columns["DateApproved"].DataType.ToString();
                            DataRow dR = dtAudit.NewRow();
                            dR["KeyDataName"] = "PRODUCT ID";
                            dR["KeyDataValue"] = nProdID.ToString();
                            dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                            dR["DataType"] = strType;
                            dR["DataName"] = "DateApproved (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                            dR["OldValue"] = DBNull.Value;
                            dR["NewValue"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                            dR["ActionTaken"] = "2";
                            dR["Datedone"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                            dR["DoneBy"] = st_dll.UserFullName(Convert.ToInt16(dtAuLabelNos.Rows[i]["ApprovedByID"]));
                            dtAudit.Rows.Add(dR);
                        }
                    }
                    //Added
                    for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                    {
                        if (dtAuLabelNos.Rows[i]["LabelStatus"].ToString() == "2")
                        {
                            string strType = dtAuLabelNos.Columns["DateApproved"].DataType.ToString();
                            DataRow dR = dtAudit.NewRow();
                            dR["KeyDataName"] = "PRODUCT ID";
                            dR["KeyDataValue"] = nProdID.ToString();
                            dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                            dR["DataType"] = strType;
                            dR["DataName"] = "DateApproved (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                            dR["OldValue"] = DBNull.Value;
                            dR["NewValue"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                            dR["ActionTaken"] = "1";
                            dR["Datedone"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                            dR["DoneBy"] = st_dll.UserFullName(Convert.ToInt16(dtAuLabelNos.Rows[i]["ApprovedByID"]));
                            dtAudit.Rows.Add(dR);
                        }
                    }
                    //Rejected
                    for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                    {
                        if (dtAuLabelNos.Rows[i]["DateRejected"] != DBNull.Value)
                        {
                            string strType = dtAuLabelNos.Columns["DateRejected"].DataType.ToString();
                            DataRow dR = dtAudit.NewRow();
                            dR["KeyDataName"] = "PRODUCT ID";
                            dR["KeyDataValue"] = nProdID.ToString();
                            dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                            dR["DataType"] = strType;
                            dR["DataName"] = "DateRejected (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                            dR["OldValue"] = DBNull.Value;
                            dR["NewValue"] = dtAuLabelNos.Rows[i]["DateRejected"].ToString();
                            dR["ActionTaken"] = "2";
                            dR["Datedone"] = dtAuLabelNos.Rows[i]["DateRejected"].ToString();
                            dR["DoneBy"] = st_dll.UserFullName(Convert.ToInt16(dtAuLabelNos.Rows[i]["RejectedByID"]));
                            dtAudit.Rows.Add(dR);
                        }
                    }
                }
                if (dtAudit == null || dtAudit.Rows.Count == 0)
                {
                    ErrorMessage("No audit records found.");
                    return;
                }

                //AuditRpt rpt = new AuditRpt();
                //rpt.rptName = "Audit Products";
                //rpt.dt = dtAudit;
                //rpt.WindowState = FormWindowState.Maximized;
                //rpt.Show();

                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                rptDoc.Load(Server.MapPath("~/Reports/AuditProducts.rpt"));
                //rptDoc.SetParameterValue("@PRODUCTID", productid);
                rptDoc.SetDataSource(dtAudit);
                rptDoc.SetDatabaseLogon("sa", "Pass2018", "172.16.4.12", "PLSAudit");
                rptDoc.DataSourceConnections[0].SetConnection("172.16.4.12", "PLSAudit", "sa", "Pass2018");
                CrystalReportViewer1.Page.Title = "Audit Label Numbers";
                rptDoc.SummaryInfo.ReportTitle = "Audit Label Numbers";
                rptDoc.DataDefinition.FormulaFields["cTitle"].Text = "'Audit Trail : Label Numbers'";
                CrystalReportViewer1.ReportSource = rptDoc;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
             Response, true, "Audit Label Numbers");
                CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                CrystalReportViewer1.SeparatePages = true;
                CrystalReportViewer1.DataBind();
                CrystalReportViewer1.HasExportButton = false;
                Response.Flush();
                Response.Close();
            }
            else {
                ErrorMessage("Please select the Product/Component.");
            }
        }

        public void ProductReport() {


            if (ddlproduct.SelectedValue != "0")
            {

                int productid = Convert.ToInt32(ddlproduct.SelectedValue);
                string strPattern = "([a-z?])[_ ]?([A-Z])";

                DataTable dtAudit = new DataTable();
                dtAudit.Columns.Add("KeyDataName", typeof(string));
                dtAudit.Columns.Add("KeyDataValue", typeof(string));
                dtAudit.Columns.Add("KeyDataDesc", typeof(string));
                dtAudit.Columns.Add("DataName", typeof(string));
                dtAudit.Columns.Add("DataType", typeof(string));
                dtAudit.Columns.Add("ActionTaken", typeof(string));
                dtAudit.Columns.Add("OldValue", typeof(string));
                dtAudit.Columns.Add("NewValue", typeof(string));
                dtAudit.Columns.Add("DateDone", typeof(string));
                dtAudit.Columns.Add("DoneBy", typeof(string));

                int nProdID = productid;//Product id

                //Products Master Table
                // DataTable dtAuLogGPLS = GISClass.AuditReport.ProductsGPLS(nProdID);

                // DataTable dtAuLog = GISClass.AuditReport.AuProducts(nProdID);

                DataTable dtAuLogGPLS = st_dll.ProductsGPLS(nProdID);
                DataTable dtAuLog = st_dll.AuProducts(nProdID);

                if (dtAuLog != null && dtAuLog.Rows.Count > 0)
                {
                    for (int j = 0; j < dtAuLog.Columns.Count; j++)
                    {
                        if (j < dtAuLog.Columns.Count - 3)
                        {
                            if (dtAuLogGPLS.Rows[0][j].ToString() != dtAuLog.Rows[0][j].ToString())
                            {
                                string strType = dtAuLog.Columns[j].DataType.ToString();
                                DataRow dR = dtAudit.NewRow();
                                dR["KeyDataName"] = "Product ID";
                                dR["KeyDataValue"] = dtAuLog.Rows[0]["ProductID"].ToString();
                                dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                dR["DataType"] = strType;
                                dR["OldValue"] = dtAuLog.Rows[0][j];
                                dR["NewValue"] = dtAuLogGPLS.Rows[0][j];
                                dR["ActionTaken"] = dtAuLog.Rows[0]["FileMaintCode"].ToString();
                                dR["Datedone"] = dtAuLog.Rows[0]["FileMaintDate"].ToString();
                                dR["DoneBy"] = dtAuLog.Rows[0]["FileMaintByID"].GetType() == typeof(string) ? st_dll.GetUserFullName(dtAuLog.Rows[0]["FileMaintByID"].ToString()) : st_dll.UserFullName(Convert.ToInt16(dtAuLog.Rows[0]["FileMaintByID"]));
                                dtAudit.Rows.Add(dR);
                            }
                        }
                    }

                    int k = 1;
                    for (int i = 0; i < dtAuLog.Rows.Count; i++)
                    {
                        if (k >= dtAuLog.Rows.Count)
                            break;
                        for (int j = 0; j < dtAuLog.Columns.Count; j++)
                        {
                            if (j < dtAuLog.Columns.Count - 3)
                            {
                                if (dtAuLog.Rows[i][j].ToString() != dtAuLog.Rows[k][j].ToString())
                                {
                                    string strType = dtAuLog.Columns[j].DataType.ToString();

                                    DataRow dR = dtAudit.NewRow();
                                    dR["KeyDataName"] = "Product ID";
                                    dR["KeyDataValue"] = dtAuLog.Rows[i]["ProductID"].ToString();
                                    dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                    dR["DataType"] = strType;
                                    dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                    dR["OldValue"] = dtAuLog.Rows[k][j];
                                    dR["NewValue"] = dtAuLog.Rows[i][j];
                                    dR["ActionTaken"] = dtAuLog.Rows[k]["FileMaintCode"].ToString();
                                    dR["Datedone"] = dtAuLog.Rows[k]["FileMaintDate"].ToString();
                                    //dR["DoneBy"] = st_dll.UserFullName(Convert.ToInt16(dtAuLog.Rows[k]["FileMaintByID"]));
                                    dR["DoneBy"] = dtAuLog.Rows[k]["FileMaintByID"].GetType() == typeof(string) ? st_dll.GetUserFullName(dtAuLog.Rows[k]["FileMaintByID"].ToString()) : st_dll.UserFullName(Convert.ToInt16(dtAuLog.Rows[k]["FileMaintByID"]));
                                    dtAudit.Rows.Add(dR);
                                }
                            }
                        }
                        k++;
                    }
                }
                if (dtAudit == null || dtAudit.Rows.Count == 0)
                {
                    ErrorMessage("No audit records found.");
                    return;
                }
                //AuditRpt rpt = new AuditRpt();
                //rpt.rptName = "Audit Products";
                //rpt.dt = dtAudit;
                //rpt.WindowState = FormWindowState.Maximized;
                //rpt.Show();

                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                rptDoc.Load(Server.MapPath("~/Reports/AuditProducts.rpt"));
                //rptDoc.SetParameterValue("@PRODUCTID", productid);
                rptDoc.SetDataSource(dtAudit);
                rptDoc.SetDatabaseLogon("sa", "Pass2018", "172.16.4.12", "PLSAudit");
                rptDoc.DataSourceConnections[0].SetConnection("172.16.4.12", "PLSAudit", "sa", "Pass2018");
                CrystalReportViewer1.Page.Title = "Audit Products";
                rptDoc.SummaryInfo.ReportTitle = "Audit Products";
                rptDoc.DataDefinition.FormulaFields["cTitle"].Text = "'Audit Trail : Product Master File'";
                CrystalReportViewer1.ReportSource = rptDoc;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
             Response, true, "Audit Products");
                CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                CrystalReportViewer1.SeparatePages = true;
                CrystalReportViewer1.DataBind();
                CrystalReportViewer1.HasExportButton = false;
                Response.Flush();
                Response.Close();

            }
            else {
                ErrorMessage("Please select the Product/Component.");
            }
        }

        public void ProductLabelReport()
        {


            if (ddlproduct.SelectedValue != "0")
            {

                int productid = Convert.ToInt32(ddlproduct.SelectedValue);
                string strPattern = "([a-z?])[_ ]?([A-Z])";

                DataTable dtAudit = new DataTable();
                dtAudit.Columns.Add("KeyDataName", typeof(string));
                dtAudit.Columns.Add("KeyDataValue", typeof(string));
                dtAudit.Columns.Add("KeyDataDesc", typeof(string));
                dtAudit.Columns.Add("DataName", typeof(string));
                dtAudit.Columns.Add("DataType", typeof(string));
                dtAudit.Columns.Add("ActionTaken", typeof(string));
                dtAudit.Columns.Add("OldValue", typeof(string));
                dtAudit.Columns.Add("NewValue", typeof(string));
                dtAudit.Columns.Add("DateDone", typeof(string));
                dtAudit.Columns.Add("DoneBy", typeof(string));

                int nProdID = productid;//Product id

                //Products Master Table
                // DataTable dtAuLogGPLS = GISClass.AuditReport.ProductsGPLS(nProdID);

                // DataTable dtAuLog = GISClass.AuditReport.AuProducts(nProdID);

                DataTable dtAuLogGPLS = st_dll.ProductLabelGPLS(nProdID);
                DataTable dtAuLog = st_dll.AuProductLabel(nProdID);

                if (dtAuLog != null && dtAuLog.Rows.Count > 0)
                {
                    for (int j = 0; j < dtAuLog.Columns.Count; j++)
                    {
                        if (j < dtAuLog.Columns.Count - 3)
                        {
                            if (dtAuLogGPLS.Rows[0][j].ToString() != dtAuLog.Rows[0][j].ToString())
                            {
                                string strType = dtAuLog.Columns[j].DataType.ToString();
                                DataRow dR = dtAudit.NewRow();
                                dR["KeyDataName"] = "Product ID";
                                dR["KeyDataValue"] = dtAuLog.Rows[0]["ProductID"].ToString();
                                dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                dR["DataType"] = strType;
                                dR["OldValue"] = dtAuLog.Rows[0][j];
                                dR["NewValue"] = dtAuLogGPLS.Rows[0][j];
                                dR["ActionTaken"] = dtAuLog.Rows[0]["FileMaintCode"].ToString();
                                dR["Datedone"] = dtAuLog.Rows[0]["FileMaintDate"].ToString();
                                dR["DoneBy"] = dtAuLog.Rows[0]["FileMaintByID"].GetType() == typeof(string) ? st_dll.GetUserFullName(dtAuLog.Rows[0]["FileMaintByID"].ToString()) : st_dll.UserFullName(Convert.ToInt16(dtAuLog.Rows[0]["FileMaintByID"]));
                                dtAudit.Rows.Add(dR);
                            }
                        }
                    }

                    int k = 1;
                    for (int i = 0; i < dtAuLog.Rows.Count; i++)
                    {
                        if (k >= dtAuLog.Rows.Count)
                            break;
                        for (int j = 0; j < dtAuLog.Columns.Count; j++)
                        {
                            if (j < dtAuLog.Columns.Count - 3)
                            {
                                if (dtAuLog.Rows[i][j].ToString() != dtAuLog.Rows[k][j].ToString())
                                {
                                    string strType = dtAuLog.Columns[j].DataType.ToString();

                                    DataRow dR = dtAudit.NewRow();
                                    dR["KeyDataName"] = "Product ID";
                                    dR["KeyDataValue"] = dtAuLog.Rows[i]["ProductID"].ToString();
                                    dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                    dR["DataType"] = strType;
                                    dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                    dR["OldValue"] = dtAuLog.Rows[k][j];
                                    dR["NewValue"] = dtAuLog.Rows[i][j];
                                    dR["ActionTaken"] = dtAuLog.Rows[k]["FileMaintCode"].ToString();
                                    dR["Datedone"] = dtAuLog.Rows[k]["FileMaintDate"].ToString();
                                    dR["DoneBy"] = dtAuLog.Rows[k]["FileMaintByID"].GetType() == typeof(string) ? st_dll.GetUserFullName(dtAuLog.Rows[k]["FileMaintByID"].ToString()) : st_dll.UserFullName(Convert.ToInt16(dtAuLog.Rows[k]["FileMaintByID"]));
                                    dtAudit.Rows.Add(dR);
                                }
                            }
                        }
                        k++;
                    }
                }

          

                if (dtAudit == null || dtAudit.Rows.Count == 0)
                {
                    ErrorMessage("No audit records found.");
                    return;
                }
                //AuditRpt rpt = new AuditRpt();
                //rpt.rptName = "Audit Products";
                //rpt.dt = dtAudit;
                //rpt.WindowState = FormWindowState.Maximized;
                //rpt.Show();

                rptDoc = new ReportDocument();
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.BestFitPage = false;
                CrystalReportViewer1.Width = 920;
                rptDoc.Load(Server.MapPath("~/Reports/AuditProducts.rpt"));
                //rptDoc.SetParameterValue("@PRODUCTID", productid);
                rptDoc.SetDataSource(dtAudit);
                rptDoc.SetDatabaseLogon("sa", "Pass2018", "172.16.4.12", "PLSAudit");
                rptDoc.DataSourceConnections[0].SetConnection("172.16.4.12", "PLSAudit", "sa", "Pass2018");
                CrystalReportViewer1.Page.Title = "Audit ProductLabels";
                rptDoc.SummaryInfo.ReportTitle = "Audit ProductLabels";
                rptDoc.DataDefinition.FormulaFields["cTitle"].Text = "'Audit Trail : ProductLabels Master File'";
                CrystalReportViewer1.ReportSource = rptDoc;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                rptDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
             Response, true, "Audit ProductLabels");
                CrystalReportViewer1.ToolPanelView = ToolPanelViewType.None;
                CrystalReportViewer1.SeparatePages = true;
                CrystalReportViewer1.DataBind();
                CrystalReportViewer1.HasExportButton = false;
                Response.Flush();
                Response.Close();

            }
            else {
                ErrorMessage("Please select the Product/Component.");
            }
        }
        protected void GenerateReport()
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
                    rptDoc.SetDatabaseLogon("sa", "Pass2018", "172.16.4.12", "PLSAudit");
                    rptDoc.DataSourceConnections[0].SetConnection("172.16.4.12", "PLSAudit", "sa", "Pass2018");
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