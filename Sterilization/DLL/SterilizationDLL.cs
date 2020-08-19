//SterilizationDLL.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Database Access  page 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Sterilization
{
    public class SterilizationDLL
    {
        static string connectionstring = ConfigurationManager.ConnectionStrings["sterilization"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionstring);



        private static bool DBConnectionStatus()
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(connectionstring))
                {
                    conn.Open();
                    return (conn.State == ConnectionState.Open);
                }
            }
            catch (SqlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region ProductLabeling


        public int GetLabelCount(int controlid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spGetLabelCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", controlid);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable GetDBData(string ProcedureName, string Type, [Optional] ProductsEntity pe)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                if (ProcedureName != "" && Type != "")
                {
                    SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", Type);
                    if (pe != null)
                    {
                        cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                        if (pe.categorycode != 0)
                        {
                            cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);

                        }
                    }

                    SqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    conn.Close();
                }

                else {
                    return null;
                }
                return dt;


            }
            catch (Exception)
            {
                return null;
            }

        }

        public int AddProductLabelDetails(string ProcedureName, string Type, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ManufacturingDate", pe.ManufacturingDate);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@ExpirationDate", pe.ExpirationDate);
                cmd.Parameters.AddWithValue("@ProductID", pe.productid);
                cmd.Parameters.AddWithValue("@LotNo", pe.LotNo);
                cmd.Parameters.AddWithValue("@LabelCount", pe.labelCount);
                cmd.Parameters.AddWithValue("@CreatedByID", pe.CreatedBy);
                cmd.Parameters.AddWithValue("@LastUserID", pe.LastUserID);
                cmd.Parameters.AddWithValue("@PONO", pe.PONo);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }

        }

        public int GetLabelCountToPrint(string ProcedureName, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROID", pe.ControlID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["totalcount"]);               
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable GetTotalLabelcount(int controlid ,int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetTotalLabelsByCategory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public int UpdateProductLabelDetails(string ProcedureName, string Type, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ManufacturingDate", pe.ManufacturingDate);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@ExpirationDate", pe.ExpirationDate);
                cmd.Parameters.AddWithValue("@ProductID", pe.productid);
                cmd.Parameters.AddWithValue("@LotNo", pe.LotNo);
                cmd.Parameters.AddWithValue("@LabelCount", pe.labelCount);
                cmd.Parameters.AddWithValue("@LastUserID", pe.LastUserID);
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@PONO", pe.PONo);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }

        }

        public DataTable GetPagingData(string storeprocedure, ProductsEntity pe, out int totalcount)
        {

            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(storeprocedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageNo", pe.Pageno);
                cmd.Parameters.AddWithValue("@NoOfRecord", pe.NoOfRecords);
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);

                SqlParameter code = new SqlParameter("@TotalRecord", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                totalcount = (int)cmd.Parameters["@TotalRecord"].Value;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable GetCaseOfData(int controlid)
        {

            try
            {
                //conn.Open();
                //SqlCommand cmd = new SqlCommand("dbo.spGenerateLabels", conn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@CASESIZE", casecount);
                //cmd.Parameters.AddWithValue("@ControlID", controlid);
                //cmd.Parameters.AddWithValue("@Type", "CS");
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                //return dt;

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGenerateLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@Type", "CS");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable GetRejectedReasons()
        {

            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetRejectedReasons", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ControlID", controlid);
                //cmd.Parameters.AddWithValue("@Type", "CS");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region Products
        public DataTable GetDBProductData(string ProcedureName)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                if (ProcedureName != "")
                {
                    SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    conn.Close();
                }
                else {
                    return null;
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int InsertUpdateProductDetails(string ProcedureName, string Type, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductDesc", pe.ProductDesc);
                cmd.Parameters.AddWithValue("@StorageCondition", pe.StorageCondition);
                cmd.Parameters.AddWithValue("@casesize", pe.casesize);
                cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);
                cmd.Parameters.AddWithValue("@itemdesc", pe.ItemDesc);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@SKUNo", pe.SKUNo);
                cmd.Parameters.AddWithValue("@unitsize", pe.Unitsize);
                if (Type == "IN")
                {
                    cmd.Parameters.AddWithValue("@CreatedBy", pe.CreatedBy);
                }
                else {
                    cmd.Parameters.AddWithValue("@ProductID", pe.productid);
                }
                cmd.Parameters.AddWithValue("@LastUserID", pe.LastUserID);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region QaApproval
        public int InsertQASign(string ProcedureName, string Type, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@ApprovedByID", pe.ApprovedByID);
                cmd.Parameters.AddWithValue("@RejectedByID", pe.RejectedByID);
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);
                cmd.Parameters.AddWithValue("@StatusReason", pe.StatusReason);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable ReportData(string ProcedureName, int controlid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@Categoryid", categorycode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region LogIn Details

        public DataTable GetUserDetails(string username)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                string Query = "SELECT UserID,GroupID,SystemAccess,Password,AccessLevel,UserName FROM GIS.dbo.Users WHERE LogonID='" + username + "'";
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region Printing

        public int PrintBatchDetails(string ProcedureName, string Type, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@PrintBatches", pe.PrintBatches);

                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                if (pe.AppliedByID != 0)
                {
                    cmd.Parameters.AddWithValue("@AppliedBy", pe.AppliedByID);
                }
                else {
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@PrintBatches", Value = pe.PrintBatches.ToString(), SqlDbType = System.Data.SqlDbType.Xml });
                }
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        public int GenerateLables(string ProcedureName, int controlid, int labelcount, int CreatedBy, int categorycode, string Type)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@LabelCount", labelcount);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@CREATEDBYID", CreatedBy);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string var = ex.Message;
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }


        public int SaveVoideddata(string ProcedureName, string Type, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                cmd.Parameters.AddWithValue("@STATUSREASON", pe.StatusReason);
                cmd.Parameters.AddWithValue("@PRINTEDBYID", pe.PrintedByID);
                cmd.Parameters.AddWithValue("@VOIDEDBYID", pe.VoidedByID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@TOPRINT", pe.toPrint);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdatePrinting(string ProcedureName, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                //cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                //cmd.Parameters.AddWithValue("@STATUSREASON", pe.StatusReason);
                cmd.Parameters.AddWithValue("@PRINTEDBYID", pe.PrintedByID);
                //cmd.Parameters.AddWithValue("@VOIDEDBYID", pe.VoidedByID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@TOPRINT", pe.toPrint);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetLabelCountData(string ProcedureName, string Type, int controlid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable GetLabelFormatsByProduct(string ProcedureName, int controlid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;               
                cmd.Parameters.AddWithValue("@controlid", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public string GetCaseFileName(string ProcedureName, int controlid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt.Rows[0]["FileName"].ToString();
            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        #region ReadingLabel
        public int ReadLabel(string ProcedureName, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                cmd.Parameters.AddWithValue("@APPLIEDBYID", pe.AppliedByID);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public int ChecklabelPrinted(string ProcedureName, int controlid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                //cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                //cmd.Parameters.AddWithValue("@APPLIEDBYID", pe.AppliedByID);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public int GetNumberOfProductsRemainingtoPrint(string data)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spGetNumberOfProductsRemainingtoPrint", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", Convert.ToInt32(data.Split(',')[0]));
                cmd.Parameters.AddWithValue("@categorycode", Convert.ToInt32(data.Split(',')[1]));
                //cmd.Parameters.AddWithValue("@APPLIEDBYID", pe.AppliedByID);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public int CheckPermission(string ProcedureName, int controlid, int userid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@userid", userid);
                //cmd.Parameters.AddWithValue("@APPLIEDBYID", pe.AppliedByID);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int ReadshipingLabel(string ProcedureName, ProductsEntity pe, string type)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                cmd.Parameters.AddWithValue("@APPLIEDBYID", pe.AppliedByID);
                cmd.Parameters.AddWithValue("@TYPE", type);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetDetails(string ProcedureName, int controlid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;

            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        public DataTable GetShipoutLabelDetails(string ProcedureName, string type, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                cmd.Parameters.AddWithValue("@TYPE", type);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;

            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        public DataTable GnerateLabels(string ProcedureName, string Type, [Optional] ProductsEntity pe)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                if (ProcedureName != "" && Type != "")
                {
                    SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", Type);
                    if (pe != null)
                    {
                        cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);

                    }
                    SqlDataReader dr = cmd.ExecuteReader();
                    dt.Load(dr);
                    conn.Close();
                }

                else {
                    return null;
                }
                return dt;


            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion

        #region Voiding


        public DataTable GetVoidLabelDetails(int controlid, int categorycode, int labelno)
        {

            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spLabelVoiding", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@LABELNO", labelno);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region CaseSize
        public void InsertIntoCaseSize(int controlid, int categorycode)
        {

            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertCaseSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYID", categorycode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public void SaveProductCount(int controlid, int categorycode,int productcount)
        {

            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertProductCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYID", categorycode);
                cmd.Parameters.AddWithValue("@productcount", categorycode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        public string GetItemDesc(int controlid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetItemDesc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                //cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                //if (Convert.ToInt32(dt.Rows[0]["UNITSIZE"]) != 0 && dt.Rows[0]["UNITSIZE"].ToString() != null)
                //{
                //    return 1;
                //}
                //else {
                //    return 0;
                //}
                return dt.Rows[0]["itemsdesc"].ToString();
            }
            catch (Exception)
            {

                return "";
            }
            finally
            {
                conn.Close();
            }
        }
        public int GetUnitSize(int controlid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetUnitSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                //cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                //if (Convert.ToInt32(dt.Rows[0]["UNITSIZE"]) != 0 && dt.Rows[0]["UNITSIZE"].ToString() != null)
                //{
                //    return 1;
                //}
                //else {
                //    return 0;
                //}
                return Convert.ToInt32(dt.Rows[0]["UNITSIZE"]);
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddIntoCaseSize(int controlid, int categorycode)
        {

            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spAddNewCaseSizeLabel", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYID", categorycode);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion


        #region Others


        public int GetLabelStatus(string ProcedureName, string type, int controlid, int categoryid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TYPE", type);
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYID", categoryid);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public int InsertPreview(string ProcedureName, string type, int controlid, int categoryid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(ProcedureName, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TYPE", type);
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYID", categoryid);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable GetProductType(string procedurename, int controlid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetComponents(string procedurename)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@ControlID", controlid);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public void SaveCompletedStatus(string procedurename, int controlid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                //return 1;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetData(string procedurename, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int CheckUser(int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckUserType", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", userid);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public int CheckAddNewLabel(int controlid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spAddNewLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYID", categorycode);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable ProductSearch(string procedurename, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (pe.LotNo != null)
                {
                    cmd.Parameters.AddWithValue("@LOTNO", pe.LotNo);
                }
                if (pe.labelCount != 0)
                {
                    cmd.Parameters.AddWithValue("@LABELCOUNT", pe.labelCount);
                }
                if (pe.ProductDesc != null)
                {
                    cmd.Parameters.AddWithValue("@PRODUCTDESC", pe.ProductDesc);
                }
                if (pe.ManufacturingDate != null)
                {
                    cmd.Parameters.AddWithValue("@MANUFACTURINGDATE", pe.ManufacturingDate);
                }
                if (pe.ExpirationDate != null)
                {
                    cmd.Parameters.AddWithValue("@EXPIRATIONDATE", pe.ExpirationDate);
                }
                if (pe.ControlID != 0)
                {
                    cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public int ChekProcudtExpired(int controlid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spCheckExpirationDate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion


        #region Audit Trail

        public DataTable ProductsGPLS(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spProductsGPLS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProdID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable AuProducts(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spAuProducts", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProdID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable ProductLabelGPLS(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spProductLabelsGPLS", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProdID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable LabelNosGPLS(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spAuLabelNos", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProdID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable AuProductLabel(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spAuProductLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProdID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public string UserFullName(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spUserFullName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LogID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt.Rows[0]["UserName"].ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public string GetProductDescription(string procedurename, int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt.Rows[0]["PRODUCTDESC"].ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion


        #region LabelFormts
        public DataTable GetLabelFormats()
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetLabelFormats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ControlID", controlid);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateLabelFormat(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateLabelFormats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PRODUCTID", pe.productid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@FORMATNO", pe.formatno);
                //cmd.Parameters.AddWithValue("@C_PRODUCTID", pe.copyproduct.Split('-')[0]);
                cmd.Parameters.AddWithValue("@C_CATEGORYCODE", pe.copyproduct.Split('-')[1]);
                cmd.Parameters.AddWithValue("@C_FORMATNO", pe.copyproduct.Split('-')[2]);
                cmd.Parameters.AddWithValue("@LASTUSERID", pe.LastUserID);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public int InsertLabelFormat(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertLabelFormats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PRODUCTID", pe.productid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@FORMATNO", pe.formatno);
                cmd.Parameters.AddWithValue("@CREATEDBYID", pe.CreatedBy);                
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public int CopyLabelFormat(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spCopyLabelFormats", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PRODUCTID", pe.productid);
                cmd.Parameters.AddWithValue("@C_CATEGORYCODE", pe.copyproduct.Split('-')[1]);
                cmd.Parameters.AddWithValue("@C_FORMATNO", pe.copyproduct.Split('-')[2]);
                cmd.Parameters.AddWithValue("@C_PRODUCTID", pe.copyproduct.Split('-')[0]);
                //cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                //cmd.Parameters.AddWithValue("@FORMATNO", pe.formatno);
                cmd.Parameters.AddWithValue("@CREATEDBYID", pe.CreatedBy);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
            }
            catch (Exception)
            {

                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
    }
}