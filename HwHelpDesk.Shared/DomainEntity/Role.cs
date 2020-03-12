using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class Role
    {
        // public roleID int {get; set;};
        public int r_id { get; set; }
        public string r_role_name { get; set; }
        public string PREFIX { get; set; }
        public int r_active { get; set; }
    }
}
