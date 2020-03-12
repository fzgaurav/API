using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IRouteRequestManage
    {
        List<DropDownDto> GetRequestFor();

        List<defaultRouteRequest> GetDefaultRoute();

        int InsertRouteData(defaultRouteRequest obj,string requestType,string userID);        

        List<APIResponse> InsertRouteRequest(string requestType, string slotID, string userID);

        List<Route> GetRecentRequest(int status);

        List<RecentRouteRequestDetails> GetRecentRequestDetails(int rrID);

        int InsertApproveRouteData(RecentRouteRequestDetails obj, int rrID);

        List<APIResponse> ApproveRouteRequest(int rrID, int approvedBy, int requestStatus);
    }
}
