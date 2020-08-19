using Newtonsoft.Json;
using Sterilization.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Sterilization
{
    /// <summary>
    /// Summary description for WebServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class WebServices : System.Web.Services.WebService
    {
        //SterilizationDLL st_dll = new SterilizationDLL();
        GPLS_DLL gpls = new GPLS_DLL();
        UsersDLL us_dll = new UsersDLL();

        [WebMethod]
        public string GetProductDetails(string value)
        {
            DataTable dt = gpls.GetProductDetails(value);
            string daresult = JsonConvert.SerializeObject(dt);
            return daresult;

        }

        [WebMethod(EnableSession = true)]
        public int ReadLabelData(string labeldata)
        {
            ProductsEntity pe = new ProductsEntity();
            pe.ControlID = Convert.ToInt32(labeldata.Split(',')[0]);
            pe.AppliedByID = Convert.ToInt32(Context.Session["UserID"]);
            pe.LabelNo = Convert.ToInt32(labeldata.Split(',')[1].TrimStart('0'));
            pe.categorycode = Convert.ToInt32(labeldata.Split(',')[2]);
            pe.LabelStatus = Convert.ToInt32(labeldata.Split(',')[3]);
            int result = gpls.ReadLabel(pe);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public int DeleteUserLog(string data)
        {
            ProductsEntity pe = new ProductsEntity();
            int userid = Convert.ToInt32(data);
            int result = us_dll.UpdateUserLog(userid);
            return result;
        }

        [WebMethod(EnableSession = true)]
        public int CheckGeneratedBatch(string data)
        {
            ProductsEntity pe = new ProductsEntity();
            int batchid = Convert.ToInt32(data);
            int result = gpls.CheckGeneratedBatch(batchid);
            return result;
        }


        [WebMethod(EnableSession = true)]
        public static int LogoutCheck()
        {
            if (HttpContext.Current.Session["UserID"] == null)
            {
                return 0;
            }
            return 1;
        }
        [WebMethod]
        public int GetLabelCountToAdd(string data)
        {
            int totallablecount = Convert.ToInt32(data.Split(',')[1]) + Convert.ToInt32(data.Split(',')[4]);
            ProductsEntity pe = new ProductsEntity();
            pe.ControlID = Convert.ToInt32(data.Split(',')[0]);
            pe.categorycode = Convert.ToInt32(data.Split(',')[2]);
            pe.batchid = Convert.ToInt32(data.Split(',')[3]);
            //DataTable dt = st_dll.GetDBData("dbo.spGenerateLabels", "S", pe);
            DataTable dt = gpls.GetLabelCountToAdd(pe);
            var not_voidedcount = (from t in dt.AsEnumerable()
                                   where t.Field<bool?>("Voided") != true
                                   select t).ToList().Count;
            int count = gpls.GetLabelCount(pe);

            //var query = (from t in dt.AsEnumerable()
            //             where t.Field<string>("LABELSTATUS") == "Added"
            //             select t).ToList().Count;
            int total;
            if (dt.Rows.Count > 0)
            {
                if (pe.categorycode == 4)//CASE
                {
                    //int labelcount1 = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / Convert.ToDouble(dt.Rows[0]["CASESIZE"])));
                    //total = Convert.ToInt32(labelcount1) - not_voidedcount;
                    total = count - not_voidedcount;
                }
                else if (pe.categorycode == 3)
                {//INSERT

                    total = count - not_voidedcount;
                }
                else {

                    //total = Convert.ToInt32(dt.Rows[0]["labelcount"]) - not_voidedcount;
                    total = count - not_voidedcount;

                }
            }
            else {
                total = 0;
            }
            return total;

        }
        [WebMethod]
        public string GetPoNumber(int value)
        {
            string pono = us_dll.GetPoNumber(value);
            return pono;
        }
        [WebMethod(EnableSession = true)]
        public int MergeBatches(string data)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            int result = gpls.MergeBatchids(Convert.ToInt32(data.Split('-')[0]), Convert.ToInt32(data.Split('-')[1]), Convert.ToInt32(data.Split('-')[2]), userid);
            return result;
        }
        [WebMethod(EnableSession = true)]
        public int SplitLabels(string data)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            int result = gpls.SplitLabels(Convert.ToInt32(data.Split('-')[0]), Convert.ToInt32(data.Split('-')[1]), data.Split('-')[2].ToString(), Convert.ToInt32(data.Split('-')[3].TrimStart('0')), userid);
            return result;
        }
        [WebMethod(EnableSession = true)]
        public string GetCaseSizeForCombine(string data)
        {

            string result = gpls.GetCaseSizeForCombine(Convert.ToInt32(data.Split('-')[0]), Convert.ToInt32(data.Split('-')[1]), Convert.ToInt32(data.Split('-')[2].TrimStart('0')), Convert.ToInt32(data.Split('-')[3]));
            return result;
        }
        [WebMethod]
        public string CheckLabels(int value)
        {
            string count = us_dll.CheckLabels(value);
            return count;
        }
        [WebMethod(EnableSession = true)]
        public string GetUserPages()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            DataTable dt = us_dll.GetUserPages(userid);
            string daresult = DataSetToJSON(dt);
            return daresult;
        }
        [WebMethod(EnableSession = true)]
        public string GetProductByControlID(string data)
        {
            DataTable dt = gpls.GetProductByControlID(data);
            string daresult = JsonConvert.SerializeObject(dt);
            return daresult;
        }
        [WebMethod(EnableSession = true)]
        public string CheckPreviewed(string data)
        {
            DataTable dt = gpls.GetProductByControlID(data);
            string daresult = JsonConvert.SerializeObject(dt);
            return daresult;
        }
        [WebMethod(EnableSession = true)]
        public string GetAllBatchesByControlID(string data)
        {
            DataTable dt = gpls.GetAllBatchesByControlID(data);
            string daresult = JsonConvert.SerializeObject(dt);
            return daresult;
        }

        [WebMethod(EnableSession = true)]
        public string GetLabelFormatsByProduct(string data)
        {
            DataTable dt = gpls.GetLabelFormatsByProduct(data);
            string daresult = JsonConvert.SerializeObject(dt);
            return daresult;
        }
        [WebMethod(EnableSession = true)]
        public string GetLabelFormat(string data)
        {
            DataTable dt = gpls.GetLabelFormat(data);
            string daresult = JsonConvert.SerializeObject(dt);
            return daresult;
        }
        [WebMethod(EnableSession = true)]
        public int CombinedLabels(string data)
        {
            string[] labels = data.Split('=');
            // List<int> result = null;
            for (int i = 0; i < labels.Length - 1; i++)
            {
                if (labels[i] != "")
                    gpls.CombinedLabels(Convert.ToInt32(labels[i].Split('-')[0]), Convert.ToInt32(labels[i].Split('-')[2].TrimStart('0')), Convert.ToInt32(Context.Session["UserID"]));

            }

            int InsertStatus = gpls.InsertCombinedLabels(Convert.ToInt32(labels[0].Split('-')[0]), Convert.ToInt32(Context.Session["UserID"]));
            //if(InsertStatus)
            return InsertStatus;



        }
        public static string DataSetToJSON(DataTable dt)
        {

            Dictionary<string, object> dict = new Dictionary<string, object>();
            object[] arr = new object[dt.Rows.Count + 1];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                arr[i] = dt.Rows[i].ItemArray;
            }
            dict.Add(dt.TableName, arr);
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(dict);
        }
    }
}
