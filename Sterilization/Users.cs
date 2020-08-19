using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sterilization
{
    public class Users
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int UserType { get; set; }
        public int GroupID { get; set; }
        public int LevelCode { get; set; }
        public int CreatedByID { get; set; }
        public int LastUserID { get; set; }

        public int PageID { get; set; }

        public string PageName { get; set; }
        public string PageDesc { get; set; }
        public string AccessLevel { get; set; }

    }
}