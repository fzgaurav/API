using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class IVRScheduling
    {
        public int TotalOrder { get; set; }
        public int PendingAttempt { get; set; }
        public int Attempt { get; set; }
        public int Verified { get; set; }
        public int NoAnswer { get; set; }
        public int CallToCustomerCare { get; set; }
        public int CallByIVR { get; set; }
    }

    public class IVROrderDetails
    {
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }
        public int IvrStatus { get; set; }
    }
}
