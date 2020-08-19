//Site.aspx.cs
//AUTHOR : Alexander M.Dela Cruz- Sr.Software Developer
//         Shravan Kumar Amancha - Software Developer
//DATE   : 07-06-2017
//LOCATION: Gibraltar Laboratories,Inc
//DESCRIPTION: Master page
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
    public partial class Site : System.Web.UI.MasterPage
    {
        GPLS_DLL st_dll = new GPLS_DLL();
         
        UsersDLL us_dll = new UsersDLL();
     
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["UserID"] == null)
                Server.Transfer("Login.aspx");
            if (Session["UserName"] != null)
            {
                string username = "Welcome, " + Session["UserName"].ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showUserName", "showUserName('" + username + "');", true);
            }
            NavigationTitleChange();
            //NavigationMenu();

        }

      
        public void NavigationTitleChange()
        {

            //DataTable dt = new DataTable();
            //dt = st_dll.GetDBData("dbo.stProductsLabeling", "QD");
            //if (dt.Rows.Count > 0)
            //{
            //    var query = (from t in dt.AsEnumerable()
            //                 where t.Field<int>("EmployeeID") == Convert.ToInt32(Session["UserID"])
            //                 select t).ToList();
            //    if (query.Count > 0)
            //    {
            //        string title = "QA Approval";
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "ChageTitle", "ChageTitle('" + title + "');", true);

            //    }
            //    else {
            //        string title = "Label Printing";
            //        Page.ClientScript.RegisterStartupScript(this.GetType(), "ChageTitle", "ChageTitle('" + title + "');", true);
            //    }
            //}
           
            
            int result = st_dll.CheckUser(Convert.ToInt32(Session["UserID"]));

            if (result == 1)
            {
                //string title = "QA Approval";
                hdnTitle.Value = "QA Approval";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ChageTitle", "ChageTitle('" + hdnTitle.Value + "');", true);
            }
            else {
               // string title = "Label Printing";
                hdnTitle.Value = "Label Printing";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ChageTitle", "ChageTitle('" + hdnTitle.Value + "');", true);
            }            
        }
    }
}