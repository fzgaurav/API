using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class City
    {
        public int city_id { get; set; }
        public string city_name { get; set; }
        public string city_s_name { get; set; }
        public int state_s_id { get; set; }
        public bool IsActive { get; set; }
        public int status_info1 { get; set; }
        public int CircleId { get; set; }
        public string s_name { get; set; }
        public string CircleName { get; set; }
        public int defaultRoute { get; set; }
    }

  
}
