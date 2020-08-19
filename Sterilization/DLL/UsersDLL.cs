//UsersDLL.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: User database Access  page
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Sterilization
{
    public class UsersDLL
    {
        static string connectionstring = ConfigurationManager.ConnectionStrings["sterilization"].ConnectionString;
        SqlConnection conn = new SqlConnection(connectionstring);

        #region UserGroup
        public DataTable GetUserGroups()
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetAllUserGroups", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@GROUPID", ug.GroupId);
                //cmd.Parameters.AddWithValue("@GROUPCODE", ug.Groupcode);
                //cmd.Parameters.AddWithValue("@GROUPNAME", ug.GroupName);
                //cmd.Parameters.AddWithValue("@CREATEDBYID", ug.CreatedBy);
                //cmd.Parameters.AddWithValue("@LASTUSERID", ug.LastUserID);
                //cmd.Parameters.AddWithValue("@ACTION", ug.Action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public int AddUserGroup(UserGroups ug)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertUserGroup", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@GROUPID", ug.GroupId);
                cmd.Parameters.AddWithValue("@GROUPCODE", ug.Groupcode);
                cmd.Parameters.AddWithValue("@GROUPNAME", ug.GroupName);
                cmd.Parameters.AddWithValue("@CREATEDBYID", ug.CreatedBy);
                cmd.Parameters.AddWithValue("@LASTUSERID", ug.LastUserID);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateUserGroup(UserGroups ug)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateUserGroup", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GROUPID", ug.GroupId);
                cmd.Parameters.AddWithValue("@GROUPCODE", ug.Groupcode);
                cmd.Parameters.AddWithValue("@GROUPNAME", ug.GroupName);
                cmd.Parameters.AddWithValue("@LASTUSERID", ug.LastUserID);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        

        #endregion

        #region Users
        public DataTable GetUserPages(int userid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spUserPages", conn);
                cmd.Parameters.AddWithValue("@USERID", userid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable CheckMultipleSystemsLogin(int userid,string computername)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckMultipleSystemsLogin", conn);
                cmd.Parameters.AddWithValue("@USERID", userid);
                cmd.Parameters.AddWithValue("@COMPUTERNAME", userid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetGISUsers()
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetPTSUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetUsers()
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetAllUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetAllUsersAccess()
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetAllUserAccess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public int AddUser(Users u)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", u.UserID);
                cmd.Parameters.AddWithValue("@USERTYPE", u.UserType);
                cmd.Parameters.AddWithValue("@GROUPID", u.GroupID);
                cmd.Parameters.AddWithValue("@LEVELCODE", u.LevelCode);
                cmd.Parameters.AddWithValue("@LASTUSERID", u.LastUserID);
                cmd.Parameters.AddWithValue("@CREATEDBYID", u.CreatedByID);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateUser(Users u)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", u.UserID);
                cmd.Parameters.AddWithValue("@USERTYPE", u.UserType);
                cmd.Parameters.AddWithValue("@GROUPID", u.GroupID);
                cmd.Parameters.AddWithValue("@LEVELCODE", u.LevelCode);
                cmd.Parameters.AddWithValue("@LASTUSERID", u.LastUserID);                
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Permissions

        public DataTable CheckUser(int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckUserLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", userid);
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
        public int UpdateUserLog(int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateUserLog", conn);
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
        public int RegisterUserLog(int userid,string computername)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spRegisterUserLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", userid);
                cmd.Parameters.AddWithValue("@COMPUTERNAME", computername);
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
        public DataTable CheckUserLog(int userid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckUserLog", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", userid);
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
        public DataTable CheckUserAccessLevel(int userid, int pageid)
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckAccessLevel", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", userid);
                cmd.Parameters.AddWithValue("@PAGEID", pageid);
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
        public DataTable GetSystemPages()
        {
            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetSystemPages", conn);
                cmd.CommandType = CommandType.StoredProcedure;           
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
        public int AddSystemPage(Users u)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertSystemPage", conn);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@PAGENAME", u.PageName);
                cmd.Parameters.AddWithValue("@PAGEDESC", u.PageDesc);
                cmd.Parameters.AddWithValue("@LASTUSERID", u.LastUserID);
                cmd.Parameters.AddWithValue("@CREATEDBYID", u.CreatedByID);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int AddUserAccess(Users u)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertUserAccess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", u.UserID);
                cmd.Parameters.AddWithValue("@PAGEID", u.PageID);
                cmd.Parameters.AddWithValue("@ACCESSLEVEL", u.AccessLevel);
                cmd.Parameters.AddWithValue("@CREATEDBYID", u.CreatedByID);
                cmd.Parameters.AddWithValue("@LASTUSERID", u.CreatedByID);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateUserAccess(Users u)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateUserAccess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@USERID", u.UserID);
                cmd.Parameters.AddWithValue("@PAGEID", u.PageID);
                cmd.Parameters.AddWithValue("@ACCESSLEVEL", u.AccessLevel);                
                cmd.Parameters.AddWithValue("@LASTUSERID", u.CreatedByID);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateSystemPage(Users u)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateSystemPage", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PAGENAME", u.PageName);
                cmd.Parameters.AddWithValue("@PAGEID", u.PageID);
                cmd.Parameters.AddWithValue("@PAGEDESC", u.PageDesc);
                cmd.Parameters.AddWithValue("@LASTUSERID", u.LastUserID);                
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Reports
        public DataTable GetProductDetails()
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetProducts", conn);                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetProductReportData(int productid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spProductLabelReport", conn);
                cmd.Parameters.AddWithValue("@PRODUCTID", productid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetInventoryReportData(int productid,string type)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spInventoryReport", conn);
                cmd.Parameters.AddWithValue("@PRODUCTID", productid);
                cmd.Parameters.AddWithValue("@TYPE", type);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetReportData(string pn)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(pn, conn);              
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetListOfVoidedLabels(int productid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spListOfVoidedLabelsReport", conn);
                cmd.Parameters.AddWithValue("@PRODUCTID", productid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetListOfAddedLabels(int productid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spListOfAddedLabelsReport", conn);
                cmd.Parameters.AddWithValue("@PRODUCTID", productid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public DataTable GetListOfRejectedLabels(int productid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spListOfRejectedLabelsReport", conn);
                cmd.Parameters.AddWithValue("@PRODUCTID", productid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        
        #endregion

        #region Others
        //GetPoNumber
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
        public string CheckLabels(int controlid)
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spCheckLabelGenerated", conn);
                cmd.Parameters.AddWithValue("@CONTROLID", controlid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt.Rows[0]["countlabel"].ToString();
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
        public DataTable GetVoidingReasons()
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetVoidingReasons", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt); 
                return dt;
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
        public DataTable GetRejectedReasons()
        {

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("dbo.spGetRejectedReasons", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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
        public int AddVoidingReason(string reasonDesc,int userid)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertVoidingReason", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@REASONDESC", reasonDesc);
                cmd.Parameters.AddWithValue("@USERID", userid);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int AddRejectedReason(string reasonDesc, int userid)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spInsertRejectedReason", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@REASONDESC", reasonDesc);
                cmd.Parameters.AddWithValue("@USERID", userid);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateVoidingReason(string reasonDesc, int userid,int reasonid)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateVoidingReason", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@REASONDESC", reasonDesc);
                cmd.Parameters.AddWithValue("@USERID", userid);
                cmd.Parameters.AddWithValue("@REASONID", reasonid);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int UpdateRejectedReason(string reasonDesc, int userid, int reasonid)
        {

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("dbo.spUpdateRejectedReason", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@REASONDESC", reasonDesc);
                cmd.Parameters.AddWithValue("@USERID", userid);
                cmd.Parameters.AddWithValue("@REASONID", reasonid);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
    }
}