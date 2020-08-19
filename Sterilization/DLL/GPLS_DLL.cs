using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Sterilization.DLL
{
    public class GPLS_DLL
    {
        static string connectionstring = ConfigurationManager.ConnectionStrings["sterilization"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionstring);

        #region Products.
        public DataTable GetDBProductData()
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("dbo.spGetProducts", conn);
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
        public int InsertProduct(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertProducts", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductDesc", pe.ProductDesc);
                cmd.Parameters.AddWithValue("@StorageCondition", pe.StorageCondition);
                //cmd.Parameters.AddWithValue("@casesize", pe.casesize);
                cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);
                cmd.Parameters.AddWithValue("@itemdesc", pe.ItemDesc);

                cmd.Parameters.AddWithValue("@SKUNo", pe.SKUNo);
                //cmd.Parameters.AddWithValue("@unitsize", pe.Unitsize);

                cmd.Parameters.AddWithValue("@units", pe.Units);
                cmd.Parameters.AddWithValue("@unittype", pe.UnitType);

                cmd.Parameters.AddWithValue("@CreatedBy", pe.CreatedBy);
                cmd.Parameters.AddWithValue("@LastUserID", pe.LastUserID);

                cmd.Parameters.AddWithValue("@MasterFormat", pe.MasterFormat);

                cmd.Parameters.AddWithValue("@InsertFormat", pe.InsertFormat);
                cmd.Parameters.AddWithValue("@CaseFormat", pe.CaseFormat);
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
        public int UpdateProduct(ProductsEntity pe)
        {
            try
            {

                conn.Open();
                SaveContext(pe.LastUserID, conn);
                SqlCommand cmd = new SqlCommand("dbo.spUpdateProducts", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductDesc", pe.ProductDesc);
                cmd.Parameters.AddWithValue("@StorageCondition", pe.StorageCondition);
                //cmd.Parameters.AddWithValue("@casesize", pe.casesize);
                cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);
                cmd.Parameters.AddWithValue("@itemdesc", String.IsNullOrEmpty(pe.ItemDesc) ? (object)DBNull.Value : pe.ItemDesc);

                cmd.Parameters.AddWithValue("@SKUNo", pe.SKUNo);
                //cmd.Parameters.AddWithValue("@unitsize", pe.Unitsize);

                cmd.Parameters.AddWithValue("@units", pe.Units);
                cmd.Parameters.AddWithValue("@unittype", pe.UnitType);

                cmd.Parameters.AddWithValue("@ProductID", pe.productid);
                cmd.Parameters.AddWithValue("@LastUserID", pe.LastUserID);

                cmd.Parameters.AddWithValue("@MasterFormat", pe.MasterFormat);

                cmd.Parameters.AddWithValue("@InsertFormat", pe.InsertFormat);
                cmd.Parameters.AddWithValue("@CaseFormat", pe.CaseFormat);

                //Audit Trail



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
        #endregion

        #region Master Labels

        public int InsertMasterLabels(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertMasterLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ManufacturingDate", pe.ManufacturingDate);
                cmd.Parameters.AddWithValue("@ExpirationDate", pe.ExpirationDate);
                cmd.Parameters.AddWithValue("@ProductID", pe.productid);
                cmd.Parameters.AddWithValue("@LotNo", pe.LotNo);
                cmd.Parameters.AddWithValue("@LabelCount", pe.labelCount);
                cmd.Parameters.AddWithValue("@CreatedByID", pe.CreatedBy);
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
        public int UpdateMasterLabels(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SaveContext(pe.LastUserID, conn);
                SqlCommand cmd = new SqlCommand("dbo.spUpdateMasterLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ManufacturingDate", pe.ManufacturingDate);
                cmd.Parameters.AddWithValue("@ExpirationDate", pe.ExpirationDate);
                cmd.Parameters.AddWithValue("@ProductID", pe.productid);
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@LotNo", pe.LotNo);
                cmd.Parameters.AddWithValue("@LabelCount", pe.labelCount);
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
        public string GetPoNumber(int lotno)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetPoNumber", conn);
                cmd.Parameters.AddWithValue("@LOTNO", lotno);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt.Rows[0]["PONO"].ToString();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public int CheckLabel(int controlid, int categorycode, int labelno)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckLabel", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
                cmd.Parameters.AddWithValue("@Label", labelno);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt.Rows.Count;
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
        public int CheckLabelReturn(int controlid, int categorycode, int labelno)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckLabelReturn", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
                cmd.Parameters.AddWithValue("@Label", labelno);
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
        public int ReadTakeoutLabel(int controlid, int categorycode, int labelno, int usage, int batchid, int createdbyid, int damage)
        {
            try
            {
                conn.Open();
                SaveContext(createdbyid, conn);
                SqlCommand cmd = new SqlCommand("dbo.spInsertTakeout", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@LABELNO", labelno);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@USAGECODE", usage);
                cmd.Parameters.AddWithValue("@CREATEDBYID", createdbyid);
                cmd.Parameters.AddWithValue("@LABELSTATUS", damage);
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
        public int ReadReturnLabel(int controlid, int categorycode, int labelno)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertReturn", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@LABELNO", labelno);
                // cmd.Parameters.AddWithValue("@BATCHID", batchid);
                //cmd.Parameters.AddWithValue("@USAGECODE", usage);
                //cmd.Parameters.AddWithValue("@CREATEDBYID", createdbyid);
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
        public int UpdateAllVoided(int reasonid, int controlid, int categorycode, int lastuserid, [Optional] int batchid)
        {
            try
            {
                conn.Open();
                SaveContext(lastuserid, conn);
                SqlCommand cmd = new SqlCommand("dbo.spUpdateVoided", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@REASONID", reasonid);
                cmd.Parameters.AddWithValue("@LASTUSERID", lastuserid);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
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
        public int UpdateAllRejected(int reasonid, int controlid, int categorycode, int lastuserid, [Optional] int batchid)
        {
            try
            {
                conn.Open();
                SaveContext(lastuserid, conn);
                SqlCommand cmd = new SqlCommand("dbo.spRejectedProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@REASONID", reasonid);
                cmd.Parameters.AddWithValue("@LASTUSERID", lastuserid);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
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
        public int SaveVoideddata(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SaveContext(pe.VoidedByID, conn);
                SqlCommand cmd = new SqlCommand("dbo.spUpdateVoidedByLabelno", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@REASONID", pe.StatusReason);
                cmd.Parameters.AddWithValue("@VOIDEDBYID", pe.VoidedByID);
                cmd.Parameters.AddWithValue("@LABELNO", pe.LabelNo);
                cmd.Parameters.AddWithValue("@BATCHID", pe.batchid);
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
        public int GetLabelCount(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spGetLabelCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@BATCHID", pe.batchid);
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
        public DataTable GetVoidLabelDetails(int controlid, int categorycode, int labelno, int batchid)
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
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
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
        public DataTable GetLabelCountToAdd(ProductsEntity pe)
        {

            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetVoidedLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@BATCHID", pe.batchid);
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

        public DataTable GetTotalLabelcount(int controlid, int categorycode)
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


        public DataTable GetBatches(int controlid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetBatches", conn);
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
        public DataTable GetProductByControlID(string data)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetProductByControlID", conn);
                cmd.Parameters.AddWithValue("@CONTROLID", Convert.ToInt32(data));
                cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable GetAllBatchesByControlID(string data)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetBatchesByControlID", conn);
                cmd.Parameters.AddWithValue("@CONTROLID", Convert.ToInt32(data));
                cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable GetProductDetails(string data)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetProductDetails", conn);
                cmd.Parameters.AddWithValue("@PRODUCTID", Convert.ToInt32(data));
                cmd.CommandType = CommandType.StoredProcedure;
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

        public int CheckRemainingLabels(int controlid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetRemainingLabelsForTakeout", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Totalcount"]);
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
        public int InsertQASign(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SaveContext((pe.LastUserID == 0 ? pe.ApprovedByID : pe.LastUserID), conn);
                SqlCommand cmd = new SqlCommand("dbo.spInsertQaEsign", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ApprovedByID", pe.ApprovedByID);
                cmd.Parameters.AddWithValue("@RejectedByID", pe.RejectedByID);
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                //cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);
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
        public int InsertQApreviewed(int controlid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertQApreviewed", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                //cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);

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


        public int CheckLabelRead(int controlid, int labelno, int categorycode)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spCheckLabelRead", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@labelno", labelno);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
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
        public int CheckGeneratedBatch(int batchid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spCheckGeneratedBatch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batchid", batchid);
                //cmd.Parameters.AddWithValue("@labelno", labelno);
                //cmd.Parameters.AddWithValue("@categorycode", categorycode);
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
        public void UpdateCompletedStatusForComponentsAndProduct(int controlid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateCompletedComponentStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                //cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {


            }
            finally
            {
                conn.Close();
            }
        }
        public int ReadLabel(ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SaveContext(pe.AppliedByID, conn);
                SqlCommand cmd = new SqlCommand("dbo.spReadLabel", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", pe.ControlID);
                cmd.Parameters.AddWithValue("@categorycode", pe.categorycode);
                cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                cmd.Parameters.AddWithValue("@APPLIEDBYID", pe.AppliedByID);
                cmd.Parameters.AddWithValue("@LABELSTATUS", pe.LabelStatus);
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
        public int UpdatePrinting(string procedurename, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SaveContext(pe.PrintedByID, conn);
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                //cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                //cmd.Parameters.AddWithValue("@STATUSREASON", pe.StatusReason);
                cmd.Parameters.AddWithValue("@PRINTEDBYID", pe.PrintedByID);
                //cmd.Parameters.AddWithValue("@VOIDEDBYID", pe.VoidedByID);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@TOPRINT", pe.labelCount);
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
        public int UpdatePONo(string procedurename, ProductsEntity pe)
        {
            try
            {
                conn.Open();
                SaveContext(pe.PrintedByID, conn);
                SqlCommand cmd = new SqlCommand(procedurename, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@CONTROLID", pe.ControlID);
                //cmd.Parameters.AddWithValue("@LabelNo", pe.LabelNo);
                //cmd.Parameters.AddWithValue("@STATUSREASON", pe.StatusReason);
                cmd.Parameters.AddWithValue("@PRINTEDBYID", pe.PrintedByID);
                cmd.Parameters.AddWithValue("@PONO", pe.PONo);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", pe.categorycode);
                cmd.Parameters.AddWithValue("@TOPRINT", pe.labelCount);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
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
        public DataTable GetLabelFormatsByProduct(String data)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetLabelFormatsByProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", data.Split('-')[0]);
                cmd.Parameters.AddWithValue("@categorycode", data.Split('-')[1]);
                cmd.Parameters.AddWithValue("@BATCHID", data.Split('-')[2]);
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
        public DataTable GetLabelFormat(String data)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetLabelFormat", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", data.Split('-')[0]);
                cmd.Parameters.AddWithValue("@categorycode", data.Split('-')[1]);
                cmd.Parameters.AddWithValue("@BATCHID", data.Split('-')[2]);
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
        public int GenerateComponentAndProductLabels(int controlid, int categorycode, int labelcount, int createdbyid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spGenerateComponentAndProductLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@LABELCOUNT", labelcount);
                cmd.Parameters.AddWithValue("@CREATEDBYID", createdbyid);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@OUT_RESULT"].Value;
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

        #region Inser Box
        public int MergeBatchids(int controlid, int frombatchid, int tobatchid, int userid)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spMergeTakeout", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@FROMBATCHID", frombatchid);
                cmd.Parameters.AddWithValue("@TOBATCHID", tobatchid);
                cmd.Parameters.AddWithValue("@Lastuserid", userid);

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
        public int GetControlIDByBatch(int batchid, int categorycode)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetControlidByBatchid", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["CONTROLID"]);
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
        public string GenerateInsertLabels(int labelcount, int batchid, int createdbyid, [Optional]  int insertsize, [Optional]  int controlid, [Optional]  int categorycode)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spGenerateInsertLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LABELCOUNT", labelcount);
                cmd.Parameters.AddWithValue("@CREATEDBYID", createdbyid);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@C_CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@INSERTSIZE", insertsize);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                SqlParameter ctrid = new SqlParameter("@CONTROLID", SqlDbType.Int);
                ctrid.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.Parameters.Add(ctrid);
                cmd.ExecuteNonQuery();
                return cmd.Parameters["@OUT_RESULT"].Value.ToString() + "-" + cmd.Parameters["@CONTROLID"].Value.ToString();
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
        public int AvoidInsertLabels(int controlid, int batchid, int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spAvoidInserts", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                // cmd.Parameters.AddWithValue("@SIZE", size);
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
        public string GenerateCaseLabels(int labelcount, int batchid, int createdbyid, [Optional]  int controlid, [Optional]  int categorycode)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spGenerateCaseLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LABELCOUNT", labelcount);
                cmd.Parameters.AddWithValue("@CREATEDBYID", createdbyid);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@C_CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                SqlParameter ctrid = new SqlParameter("@CONTROLID", SqlDbType.Int);
                ctrid.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                cmd.Parameters.Add(ctrid);
                cmd.ExecuteNonQuery();
                return cmd.Parameters["@OUT_RESULT"].Value.ToString() + "-" + cmd.Parameters["@CONTROLID"].Value.ToString();
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
        public DataTable GetUnitSize(int batchid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetUnitSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batchid", batchid);
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
        public void InsertIntoCaseSize(int batchid, int unitsize)
        {

            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertUnitSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@UNITSIZE", unitsize);
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
        public void InsertLabelSizes(int controlid, int categorycode, int batchid, int size, int userid)
        {

            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertLabelSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@SIZE", size);
                cmd.Parameters.AddWithValue("@CREATEDBYID", userid);
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

        #region Case Labels
        public void InsertCaseSize(int batchid, int casesize)
        {

            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertCaseSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@CASESIZE", casesize);
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
        public void UpdateUnitSizeInCaseLabelSize(int batchid, int casesize)
        {

            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateUnitSizeInCaseLabelSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@CASESIZE", casesize);
                //cmd.Parameters.AddWithValue("@INSERTSIZE", insertsize);
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

        public int ReadCaseTakeoutLabel(int controlid, int categorycode, int labelno, int createdbyid)
        {
            try
            {
                conn.Open();
                SaveContext(createdbyid, conn);
                SqlCommand cmd = new SqlCommand("dbo.spInsertTakeout", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@CATEGORYCODE", categorycode);
                cmd.Parameters.AddWithValue("@LABELNO", labelno);
                cmd.Parameters.AddWithValue("@CREATEDBYID", createdbyid);
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
        public DataTable GetCaseSize(int batchid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetCaseSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@batchid", batchid);
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
        public DataTable GetLabelSize(int controlid, int categorycode, int labelno)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetLabelSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
                cmd.Parameters.AddWithValue("@labelno", labelno);
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
        public DataTable GetInsertLabelSize(int controlid, int categorycode, int batchid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetInsertLabelSize", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
                cmd.Parameters.AddWithValue("@batchid", batchid);
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

        public string GetCaseSizeForCombine(int controlid, int categorycode, int labelno, int batchid)
        {

            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetCaseSizeForLabel", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@controlid", controlid);
                cmd.Parameters.AddWithValue("@categorycode", categorycode);
                cmd.Parameters.AddWithValue("@labelno", labelno);
                cmd.Parameters.AddWithValue("@batchid", batchid);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0]["CaseSize"]) + "," + Convert.ToInt32(dt.Rows[0]["UnitSize"]);
                else
                    return "";
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
        public int SplitLabels(int controlid, int batchid, string newlabelitems, int labelno, int userid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spSplitCaseLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.Parameters.AddWithValue("@BATCHID", batchid);
                cmd.Parameters.AddWithValue("@ITEMS", newlabelitems);
                cmd.Parameters.AddWithValue("@LABELNO", labelno);
                SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                code.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(code);
                //var pList = new SqlParameter("@list", SqlDbType.Structured);
                //pList.TypeName = "dbo.StringList";
                //pList.Value = labelnos;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
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

        #endregion

        #region LogIn Details

        public DataTable GetUserDetails(string username)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                string Query = "SELECT UserID,GroupID,SystemAccess,Password,AccessLevel,UserName FROM [pts].[dbo].Users WHERE LogonID='" + username + "'";
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public DataTable GetUserAccessPermissions(int userid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetUserAccessPermissions", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);
                //cmd.Parameters.AddWithValue("@categorycode", categorycode);
                //cmd.Parameters.AddWithValue("@batchid", batchid);
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
        public int CheckUserGroup(int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckUserGroup", conn);
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
        #endregion

        #region Common Methods
        public DataTable GetDataTable(string procedure)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(procedure, conn);
                cmd.CommandType = CommandType.StoredProcedure;
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
        public void SaveContext(int userid, SqlConnection conn)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("dbo.spSetContextLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", userid);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }



        #endregion

        #region Search
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
                if (pe.Status != 0)
                {
                    cmd.Parameters.AddWithValue("@STATUS", pe.Status);
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
        #endregion

        #region Audit Trail

        public DataTable ProductLabelGPLS(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spProductLabelsPLS", conn);
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
        public string GetUserFullName(string logonid)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetUserName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@logonid", logonid);
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
        public DataTable ProductsGPLS(int id)
        {
            try
            {

                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spProductsPLS", conn);
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
        #endregion

        #region LabelFormats
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
                SaveContext(pe.LastUserID, conn);
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
        #endregion

        #region Combine Labels

        public void CombinedLabels(int controlid, int labelno, int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCombineLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
                cmd.Parameters.AddWithValue("@labelno", labelno);
                cmd.Parameters.AddWithValue("@USERID", userid);
                //SqlParameter code = new SqlParameter("@OUT_RESULT", SqlDbType.Int);
                //code.Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(code);
                cmd.ExecuteNonQuery();
                //return (int)cmd.Parameters["@OUT_RESULT"].Value;


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
        public int InsertCombinedLabels(int controlid, int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spInsertCombinedLabels", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ControlID", controlid);
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
        //InsertCombinedLabels
        #endregion
    }
}