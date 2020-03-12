using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class Pincode
    {
        public string pincode {get;set;}
        public string cityName { get; set; }
        public int city_id { get; set; }
        public int isActive { get; set; }
    }

    public class PincodeData
    {
        public int id { get; set; }
        public int pincode { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string state { get; set; }
        public string CircleName { get; set; }
        public int Status_Info { get; set; }
    }
}
