using HwHelpDesk.Data.IManager;
using HwHelpDesk.Data.Manager;
using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HwHelpDesk.WebUI.Controllers
{
    [RoutePrefix("api/Route")]
    public class RouteController : ApiController
    {
        private IRouteRequestManage _IRouteRequestManage;
        public RouteController()
        {
            if(_IRouteRequestManage==null)
            {
                _IRouteRequestManage = new RouteRequestManage();
            }
        }
        [Route("requestFor")]
        [HttpGet]
        public List<DropDownDto> GetRequestFor()
        {
            List<DropDownDto> obj = new List<DropDownDto>();
            obj = _IRouteRequestManage.GetRequestFor();
            return obj;
        }

        [Route("defaultRoute")]
        [HttpGet]
        public List<defaultRouteRequest> GetDefaultRoute()
        {
            List<defaultRouteRequest> obj = new List<defaultRouteRequest>();
            obj = _IRouteRequestManage.GetDefaultRoute();
            return obj;
        }



        [Route("RouteRequest")]
        [HttpPost]
        public IHttpActionResult INSERT(getData obj)
        {
            try
            {
                List<APIResponse> objResponse = new List<APIResponse>();
                if (obj.dataObj != null)
                {
                    for (int i = 0; i < obj.dataObj.Count; i++)
                    {
                        _IRouteRequestManage.InsertRouteData(obj.dataObj[i],obj.Requesttype,obj.userID);
                    }
                }
                objResponse = _IRouteRequestManage.InsertRouteRequest(obj.Requesttype, obj.slotID, obj.userID);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("RecentRouteRequest")]
        [HttpGet]
        public List<Route> GetRecentRequest(int status)
        {
            List<Route> obj = new List<Route>();
            obj = _IRouteRequestManage.GetRecentRequest(status);
            return obj;
        }

        [Route("RecentRouteRequestDetails")]
        [HttpGet]
        public List<RecentRouteRequestDetails> GetRecentRequestDetails(int rrID)
        {
            List<RecentRouteRequestDetails> obj = new List<RecentRouteRequestDetails>();
            obj = _IRouteRequestManage.GetRecentRequestDetails(rrID);
            return obj;
        }

        [Route("Approve")]
        [HttpPost]
        public IHttpActionResult ApproveRouteRequest(ApproveRouteData obj)
        {
            try
            {
                List<APIResponse> objResponse = new List<APIResponse>();
                if (obj.dataObj != null)
                {
                    for (int i = 0; i < obj.dataObj.Count; i++)
                    {
                        _IRouteRequestManage.InsertApproveRouteData(obj.dataObj[i], obj.rrID);
                    }
                }
                objResponse = _IRouteRequestManage.ApproveRouteRequest(obj.rrID,obj.approvedBy,obj.status);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
