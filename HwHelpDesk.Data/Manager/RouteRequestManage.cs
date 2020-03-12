using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class RouteRequestManage:IRouteRequestManage
    {
        LiveDBContext _dbContext;
        public RouteRequestManage()
        {
            if(_dbContext==null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<DropDownDto> GetRequestFor()
        {
            List<DropDownDto> ddObj = new List<DropDownDto>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT TOP 3 CONVERT(VARCHAR(2),dayFROM,106)+' '+CONVERT(char(3),GETDATE(),0)+' to '+ CONVERT(VARCHAR(2),dayTO,106)+' '+CONVERT(char(3),GETDATE(),0) AS VALUE,rsmID AS ID FROM routeSlotMaster_fullfillment
                            WHERE isActive=1 AND DATEPART(dd,lastDateOfRequest)>DATEPART(dd,GETDATE()) ORDER BY rsmID ASC");
            ddObj = _dbContext.Database.SqlQuery<DropDownDto>(strBld.ToString()).ToList();
            return ddObj;
        }

        public List<defaultRouteRequest> GetDefaultRoute()
        {
            List<defaultRouteRequest> ddObj = new List<defaultRouteRequest>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT city_name,city_id,defaultRoute,0 requestedRoute,0 lastCycleRequestedRoute,0 lastCycleApprovedRoute FROM cities WHERE isActive=1 ORDER BY city_name ASC");
            ddObj = _dbContext.Database.SqlQuery<defaultRouteRequest>(strBld.ToString()).ToList();
            return ddObj;
        }

        public List<Route> GetRecentRequest(int status)
        {
            List<Route> ddObj = new List<Route>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT rrID,rsmID,requestNo,CONVERT(varchar(10),requestDate,103) requestDate,CASE WHEN requestType='N' THEN 'NORMAL' ELSE 'TATKAL' END requestType,ul.username userName,requestBY,requestStatus,totalRouteRequested FROM routeRequest_fullfillment rrf
                            INNER JOIN user_login ul ON rrf.requestBy=ul.user_id WHERE requestStatus="+status+" ORDER BY rrID DESC");
            ddObj = _dbContext.Database.SqlQuery<Route>(strBld.ToString()).ToList();
            return ddObj;
        }

        public List<RecentRouteRequestDetails> GetRecentRequestDetails(int rrID)
        {
            List<RecentRouteRequestDetails> ddObj = new List<RecentRouteRequestDetails>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT rrdID,rrID,districtID,city_Name districtName,c.defaultRoute DefaultRoute ,0 LastApprovedRoute,RequestedRoute,CASE WHEN ISNULL(ApprovedRoute,0)=0 THEN RequestedRoute ELSE ISNULL(ApprovedRoute,0) END ApprovedRoute FROM routeRequestDetails_fullfillment rdf
                            INNER JOIN cities c ON rdf.districtID=c.city_id WHERE rrID=" + rrID + " ORDER BY rrdID ASC");
            ddObj = _dbContext.Database.SqlQuery<RecentRouteRequestDetails>(strBld.ToString()).ToList();
            return ddObj;
        }

        public int InsertRouteData(defaultRouteRequest routeRequest,string requestType,string userID)
        {
            //List<APIResponse> obj = new List<APIResponse>();
            //APIResponse objResponse = new APIResponse();
            var districtID = new SqlParameter
            {
                ParameterName = "@districtID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.city_id)
            };
            var requestedRoute = new SqlParameter
            {
                ParameterName = "@requestedRoute",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.requestedRoute)
            };
            var defaultRoute = new SqlParameter
            {
                ParameterName = "@defaultRoute",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.defaultRoute)
            };
            var user = new SqlParameter
            {
                ParameterName = "@userID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(userID)
            };
            var requestT = new SqlParameter
            {
                ParameterName = "@requestType",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = requestType,
                Size = 5
            };
            string SQLString = "EXEC [dbo].[USP_InsertRequestRouteDate_fullfillment]  @userID,@districtID,@defaultRoute, @requestType, @requestedRoute";
             _dbContext.Database.ExecuteSqlCommand(SQLString, user,districtID, defaultRoute, requestT, requestedRoute);
            int result = 1;
            return result;
        }

        public List<APIResponse> InsertRouteRequest(string requestType,string slotID, string userID)
        {
            List<APIResponse> obj = new List<APIResponse>();
            APIResponse objResponse = new APIResponse();
            var slot = new SqlParameter
            {
                ParameterName = "@requestFor",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(slotID)
            };
    
            var user = new SqlParameter
            {
                ParameterName = "@userID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(userID)
            };
            var requestT = new SqlParameter
            {
                ParameterName = "@requestType",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = requestType,
                Size = 5
            };
            string SQLString = "EXEC [dbo].[USP_CreateDefaultRouteRequest_fullfillment]  @userID,@requestFor,@requestType";
            _dbContext.Database.ExecuteSqlCommand(SQLString, user, slot, requestT);
            objResponse.responseCode = 200;
            objResponse.responseMsg = "Request Created Successfully !!";
            obj.Add(objResponse);
            return obj;
        }


        public int InsertApproveRouteData(RecentRouteRequestDetails routeRequest, int rrID)
        {
            var districtID = new SqlParameter
            {
                ParameterName = "@districtID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.DistrictID)
            };
            var approvedRoute = new SqlParameter
            {
                ParameterName = "@approvedRoute",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.RequestedRoute)
            };
            var defaultRoute = new SqlParameter
            {
                ParameterName = "@defaultRoute",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.DefaultRoute)
            };
            var ID = new SqlParameter
            {
                ParameterName = "@rrID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.RrID)
            };
            var rID = new SqlParameter
            {
                ParameterName = "@rrdID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(routeRequest.RrdID)
            };

            string SQLString = "EXEC [dbo].[USP_InsertApprovedRouteData]  @districtID,@approvedRoute,@defaultRoute, @rrID, @rrdID";
            _dbContext.Database.ExecuteSqlCommand(SQLString, districtID, approvedRoute, defaultRoute, ID, rID);
            int result = 1;
            return result;
        }

        public List<APIResponse> ApproveRouteRequest(int rrID, int approvedBy, int requestStatus)
        {
            List<APIResponse> obj = new List<APIResponse>();
            APIResponse objResponse = new APIResponse();
            var rID = new SqlParameter
            {
                ParameterName = "@rrID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(rrID)
            };

            var userID = new SqlParameter
            {
                ParameterName = "@approvedBy",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(approvedBy)
            };
            var Status = new SqlParameter
            {
                ParameterName = "@requestStatus",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = Convert.ToInt32(requestStatus)
                
            };
            var responseMsg = new SqlParameter
            {
                ParameterName = "@responseMsg",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Output,
                Size = 500
            };
            var responseCode = new SqlParameter
            {
                ParameterName = "@responseCode",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            string SQLString = "EXEC [dbo].[USP_ApprovedRouteRequest] @rrID,@approvedBy,@requestStatus,@responseMsg OUT,@responseCode OUT";
            _dbContext.Database.ExecuteSqlCommand(SQLString, rID, userID, Status, responseMsg, responseCode);
            objResponse.responseCode = (int)responseCode.Value;
            objResponse.responseMsg = (string)responseMsg.Value;
            obj.Add(objResponse);
            return obj;
        }
    }
}
