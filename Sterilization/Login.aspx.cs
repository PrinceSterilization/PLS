using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.IO;
using Sterilization.DLL;

namespace Sterilization
{
    public partial class Login1 : System.Web.UI.Page
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool LockWorkStation();
        [DllImport("ADVAPI32.dll", EntryPoint =
"LogonUserW", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string lpszUsername,
                                            string lpszDomain, 
                                            string lpszPassword, 
                                            int dwLogonType,   
                                            int dwLogonProvider, 
                                            ref IntPtr phToken);

        GPLS_DLL st_dll = new GPLS_DLL();
        UsersDLL us_dll = new UsersDLL();
        public static int chkLock;
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("Expires", "0");


            if (Request.QueryString["username"] != null)
            {
                txtUserName.Text = Request.QueryString["username"].ToString();
            }

            if (Request.QueryString["pwd"] != null)
            {
                txtPassword.Text = Request.QueryString["pwd"].ToString();
            }

            if (Request.QueryString["username"] != null && Request.QueryString["pwd"] != null)
            {

                btnLogin_Click(sender, e);
            }

            string status = Request.QueryString["param"];
            if (status == "LG")
            {
                us_dll.UpdateUserLog(Convert.ToInt32(Session["UserID"]));
                Session["UserName"] = null;
                Session["UserID"] = null;
                Session["EMail"] = null;
                //Session["Level"] = null;
                //Application["SessionUserID"] = null;
            }

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = GetUserName(txtUserName.Text);
            IntPtr token = IntPtr.Zero;

            try
            {
                if (userName != "" || txtPassword.Text != "")
                {
                    bool result = LogonUser(userName, "", txtPassword.Text, 3, 1, ref token);
                    if (result)
                    {
                        DataTable dt = new DataTable();
                        DataTable dt_user = new DataTable();
                        DataTable dt_useraccess = new DataTable();
                        dt = st_dll.GetUserDetails(userName);
                        //Check if User as any "User Access" Pages
                        dt_useraccess = st_dll.GetUserAccessPermissions(Convert.ToInt32(dt.Rows[0]["UserID"]));

                        if (dt_useraccess.Rows.Count > 0)
                        {
                            
                            dt_user = us_dll.CheckUser(Convert.ToInt32(dt.Rows[0]["UserID"]));
                            string computername = System.Net.Dns.GetHostEntry("").HostName;
                            DataTable dt_multipleLogin = us_dll.CheckMultipleSystemsLogin(Convert.ToInt32(dt.Rows[0]["UserID"]), computername);
                            if (CheckMultipleSystemLogins(Convert.ToInt32(dt.Rows[0]["UserID"])))
                            {
                                chkLock = 0;
                                if (UserLog(Convert.ToInt32(dt.Rows[0]["UserID"])))
                                {
                                    if (dt_user.Rows.Count > 0)
                                    {
                                        Session["UserName"] = dt_user.Rows[0]["UserName"];
                                        Session["UserID"] = dt_user.Rows[0]["UserID"];
                                        Session["EMail"] = dt_user.Rows[0]["CompanyEMail"];
                                        //Session["Level"] = dt.Rows[0]["AccessLevel"];
                                        //Application["SessionUserID"] = dt.Rows[0]["UserID"];
                                        //Login.MessageBox("Hello dsfgsg");
                                        int dtCheckUser = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));

                                        if (dtCheckUser == 1)
                                        {
                                            Response.Redirect("MasterLabels.aspx");
                                        }
                                        else {

                                            Response.Redirect("Products.aspx");
                                        }

                                    }
                                    else {
                                        ErrorMessage("No login permission to this user.Please contact IT department");
                                    }
                                }
                            }
                            else {
                                ErrorMessage("Your currently logged in to " + dt_multipleLogin.Rows[0]["COMPUTERNAME"].ToString() + ".");
                            }
                        }
                        else {
                            ErrorMessage("User access is denied.Please contact IT department");

                        }

                    }
                    else {

                        chkLock++;
                        if (chkLock == 3)
                        {
                            ErrorMessage("Your account has been locked out.Please contact IT Department!");
                            //chkLock = 0;
                            LockWorkStation();
                        }
                        else {

                            ErrorMessage("Please enter a valid user !");
                        }
                    }
                }
                else {

                    ErrorMessage("Please enter all the fields !");
                }
            }
            catch (Exception ex)
            {

                //ErrorMessage("Please Enter a valid User !");
                ErrorMessage(ex.Message);
            }

        }
        public bool CheckMultipleSystemLogins(int userid)
        {
            //string computername = System.Net.Dns.GetHostEntry("").HostName;
            //string computername = System.Environment.MachineName;
            string computername = GetComputerName(Request.UserHostAddress);
            DataTable dt_multipleLogin = us_dll.CheckMultipleSystemsLogin(userid, computername);

            if (dt_multipleLogin == null || dt_multipleLogin.Rows.Count <= 0)
            {
                return true;
            }
            if (dt_multipleLogin.Rows[0]["COMPUTERNAME"].ToString() == computername)
            {
                return true;
            }
            else {
                return false;
            }
        }
        public string GetComputerName(string clientIP)
        {
            try
            {
                var hostEntry = System.Net.Dns.GetHostEntry(clientIP);
                return hostEntry.HostName;
            }
            catch
            {
                return string.Empty;
            }
        }


        public bool UserLog(int userid)
        {
            try
            {
                DataTable dt = us_dll.CheckUserLog(userid);
                if (dt.Rows.Count > 0)
                {
                    ErrorMessage("You did not log out properly!");
                    us_dll.UpdateUserLog(userid);
                    return false;
                }
                else {
                    RegisterUserLog(userid);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void RegisterUserLog(int userid)
        {
            try
            {
                //string computername = System.Net.Dns.GetHostEntry("").HostName;
                // string computername = System.Environment.MachineName;
                string computername = GetComputerName(Request.UserHostAddress);

                int result = us_dll.RegisterUserLog(userid, computername);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string GetUserName(string username)
        {

            if (username.Contains("\\"))
            {
                int index = username.IndexOf("\\");
                return username.Substring(index + 1);
            }
            else if (username.Contains("@"))
            {
                int index = username.IndexOf("@");
                return username.Substring(0, index);
            }
            else
            {
                return username;
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

