//Reportpage.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Printing reports 
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using KeepAutomation.Barcode.Bean;

namespace Sterilization
{
    public partial class Reportpage : System.Web.UI.Page
    {
        static string connectionstring = ConfigurationManager.ConnectionStrings["sterilization"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionstring);
        private int _catid;
        ReportDocument rptDoc;
        protected void Page_Init(object sender, EventArgs e)
        {

            if (Request.QueryString["catid"].ToString() != null) {
                _catid = Convert.ToInt32(Request.QueryString["catid"]);
            }
          
            
            if (Convert.ToInt16(Session["Controlid"]) != 0 )
            {
                OpenPreviewReport(Convert.ToInt16(Session["Controlid"]));
            }
          
            //
        }
        private void OpenPreviewReport(int id)
        {
            try
            {
                
                DataSet rptDS = new DataSet();
                rptDoc = new ReportDocument();
                DataTable dt = new DataTable();
                crPreview.HasToggleGroupTreeButton = false;
                crPreview.BestFitPage = false;
                crPreview.Width = 920;
                conn.Open();
                SqlCommand cmd;
             
                if (Session["BatchID"] != null )
                {
                    cmd = new SqlCommand("dbo.spInsertReportPreview", conn);
                    cmd.Parameters.AddWithValue("@BATCHID", Convert.ToInt32(Session["BatchID"]));
                }               
                else
                {
                    cmd = new SqlCommand("dbo.spComponentPreview", conn);

                    //BarCode qrcode = new BarCode();
                    ////Barcode settings
                    //qrcode.Symbology = KeepAutomation.Barcode.Symbology.QRCode;
                    //qrcode.X = 6;
                    //qrcode.Y = 6;
                    //qrcode.LeftMargin = 24;
                    //qrcode.RightMargin = 24;
                    //qrcode.TopMargin = 24;
                    //qrcode.BottomMargin = 24;
                    //qrcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;




                    //foreach (System.Data.DataRow dr in dt.Rows)
                    //{

                    //    qrcode.CodeToEncode = "Product Description: " + dr["ProductDesc"].ToString() + " \n"
                    //                            + "SKU Number: " + dr["SKUNo"].ToString() + " \n"
                    //                            + "Manufacturing Date: " + Convert.ToDateTime(dr["ManufacturingDate"]).ToString("MM/dd/yyyy") + " \n"
                    //                            + "Expiration Date:  " + Convert.ToDateTime(dr["ExpirationDate"]).ToString("MM/dd/yyyy") + " \n"
                    //                            + "Lot Number: " + dr["LotNo"].ToString();
                    //    byte[] imageData = qrcode.generateBarcodeToByteArray();
                    //    dr["BarcodeImage"] = imageData;
                    //}
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", id);
                cmd.Parameters.AddWithValue("@Categoryid", _catid);
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //Int32 intRows=dt.Rows.Count;
                Int32 intColumns = dt.Columns.Count;
                //In the associated Stored Procedures Last three Parameters are passing Label Formats                
                Int32 intMasterFormatID = dt.Rows[0].Field<Int32>(intColumns - 3);
                Int32 intInsertFormatID = dt.Rows[0].Field<Int32>(intColumns - 2);
                Int32 intCaseFormatID = dt.Rows[0].Field<Int32>(intColumns-1);
                conn.Close();
                
                if (Session["BatchID"] != null && _catid == 3)
                {
                    if (intInsertFormatID != 7)
                    {
                        rptDoc.Load(Server.MapPath("~/Reports/rptInsertBoxPreview.rpt"));
                    }
                    else
                    {
                        rptDoc.Load(Server.MapPath("~/Reports/rptInsertBoxPreview_Frmt7.rpt"));
                    }
                    
                    rptDoc.SetParameterValue("@BATCHID", Convert.ToInt32(Session["BatchID"]));
                }
                else if (Session["BatchID"] != null && _catid == 4)
                {
                    if (intCaseFormatID != 7)
                    {
                        rptDoc.Load(Server.MapPath("~/Reports/rptCaseBoxPreview.rpt"));
                    }
                    else
                    {
                        rptDoc.Load(Server.MapPath("~/Reports/rptCaseBoxPreview _Frmt7.rpt"));
                    }
                    
                }
                else {
                    if (intMasterFormatID != 7)
                    {
                        rptDoc.Load(Server.MapPath("~/Reports/rptPreview.rpt"));
                    }
                    else
                    {
                        rptDoc.Load(Server.MapPath("~/Reports/rptPreview_Frmt7.rpt"));
                    }


                    
                }
               
                rptDoc.SetParameterValue("@ControlID", id);
                rptDoc.SetParameterValue("@Categoryid", _catid);
                rptDoc.SetDataSource(dt);
                //rptDoc.SetDatabaseLogon("sa", "Pass2012", "GBLNJ4", "GPLS");
                rptDoc.SetDatabaseLogon("sa", "Pass2018", "pssql01", "PLS");
                crPreview.Page.Title = "LABELING REPORT";
                
                //rptDoc.DataDefinition.FormulaFields["cStatus"].Text = "'"+ _rejectedStatus + "'";
                //crPreview.EnableParameterPrompt = false;
                crPreview.ReportSource = rptDoc;



                if (CheckQAEsignIsSuccessOnPreview() == 1)
                {
                    crPreview.HasPrintButton = true;
                }
                else {
                    crPreview.HasPrintButton = false;

                }
                //crPreview.HasPrintButton = false;
                if (_catid == 3 || _catid == 2)
                {
                    crPreview.HasPrintButton = true;
                }


                crPreview.SeparatePages = true;
                crPreview.DataBind();
                crPreview.HasExportButton = false;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private int CheckQAEsignIsSuccessOnPreview() {
            try
            {
                conn.Open();
                SqlCommand cmd_chekPreview = new SqlCommand("dbo.spCheckQAEsignIsSuccessOnPreview", conn);
                cmd_chekPreview.CommandType = CommandType.StoredProcedure;
                cmd_chekPreview.Parameters.AddWithValue("@ControlID", Convert.ToInt16(Session["Controlid"]));
                cmd_chekPreview.Parameters.AddWithValue("@Categoryid", _catid);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd_chekPreview.Parameters.Add(code);
                cmd_chekPreview.ExecuteNonQuery();

                return (int)cmd_chekPreview.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
            finally {

                conn.Close();
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
    }
}