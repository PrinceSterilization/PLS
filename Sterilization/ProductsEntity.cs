using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Sterilization
{
    public class ProductsEntity
    {
        public int productid { get; set; }
        public int batchid { get; set; }

        public int Status { get; set; }
        public int Units { get; set; }
        public int UnitType { get; set; }

        public int labelCount { get; set; }
        public string ProductDesc { get; set; }
        public string SKUNo { get; set; }
        public string LotNo { get; set; }
        public DateTime? DateCreated { get; set; }

        public int CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }

        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int LastUserID { get; set; }

        public int ApprovedByID { get; set; }
        public int AppliedByID { get; set; }
        public int ControlID { get; set; }
        public string formatname { get; set; }
        public int formatno { get; set; }
        
        public XElement PrintBatches { get; set; }

        public string StorageCondition { get; set; }
        public string copyproduct { get; set; }

        public string ItemDesc { get; set; }
        public int? Unitsize { get; set; }

        public int Pageno { get; set; }
        public int NoOfRecords { get; set; }

        #region Printing

        public int LabelNo { get; set; }
        public int LabelStatus { get; set; }

        public int StatusReason { get; set; }

        public int  Printed { get; set; }

        
        public int PrintedByID { get; set; }

        public int Voided { get; set; }

        public int VoidedByID { get; set; }
        public int categorycode { get; set; }
        public string CLabelNo { get; set; }

        public int toPrint { get; set; }
        public int casesize { get; set; }
        public string PONo { get; set; }
        public int RejectedByID { get; set; }


        public int MasterFormat { get; set; }
        public int InsertFormat { get; set; }
        public int CaseFormat { get; set; }

        #endregion

    }
}