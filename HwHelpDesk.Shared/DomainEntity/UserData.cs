using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class userData
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string roleID { get; set; }
        public string roleName { get; set; }
        public string lastLogin { get; set; }
        public string dailerID { get; set; }
        public string companyID { get; set; }
    }
}
