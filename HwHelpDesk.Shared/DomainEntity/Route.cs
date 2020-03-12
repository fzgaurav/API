using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class Route
    {
        public int rrID { get; set; }
        public int rsmID { get; set; }
        public string requestNo { get; set; }
        public string requestDate { get; set; }
        public string userName { get; set; }
        public int requestBY { get; set; }
        public char requestType { get; set; }
        public int requestStatus { get; set; }
        public int requestApprovedBY { get; set; }
        public string requestApprovedDate { get; set; }
        public int totalRouteRequested { get; set; }
    }
    public class defaultRouteRequest
    {
        public string city_name { get; set; } 
        public int city_id { get; set; }
        public int defaultRoute { get; set; }
        public int lastCycleRequestedRoute { get; set; }
        public int lastCycleApprovedRoute { get; set; }
        public int requestedRoute { get; set; }
    }
    public class getData
    {
        public List<defaultRouteRequest> dataObj { get; set; }
        public string Requesttype { get; set; }
        public string userID { get; set; }
        public string slotID { get; set; }
    }

    public class ApproveRouteData
    {
        public List<RecentRouteRequestDetails> dataObj { get; set; }
        public int rrID { get; set; }
        public int approvedBy { get; set; }
        public int status { get; set; }
    }

    public class RecentRouteRequestDetails
    {
        public int RrdID { get; set; }
        public int RrID { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int RequestedRoute { get; set; }
        public int DefaultRoute { get; set; }
        public int LastApprovedRoute { get; set; }
        public int ApprovedRoute { get; set; }
    }


}
