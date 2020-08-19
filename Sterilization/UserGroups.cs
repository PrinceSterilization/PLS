using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sterilization
{
    public class UserGroups
    {
        public int GroupId { get; set; }
        public string Groupcode { get; set; }
        public string GroupName { get; set; }        
        public int CreatedBy { get; set; }
        public int LastUserID { get; set; }

        public string Action { get; set; }
    }
}