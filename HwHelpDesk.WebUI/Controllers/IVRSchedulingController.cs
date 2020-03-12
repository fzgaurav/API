using HwHelpDesk.Data.IManager;
using HwHelpDesk.Data.Manager;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HwHelpDesk.WebUI.Controllers
{
    [RoutePrefix("api/IVR")]
    public class IVRSchedulingController : ApiController
    {
        private ICustomerOrder _IVRScheduling;
        public IVRSchedulingController()
        {
            if(_IVRScheduling == null)
            {
                _IVRScheduling = new CustomerOrderManage();
            }
        }
        [Route("IVRDetails")]
        [HttpGet]
        public List<IVRScheduling> GetIVRSchedulings(string date)
        {
            List<IVRScheduling> objData = new List<IVRScheduling>();
            objData= _IVRScheduling.GetIVRCallTracking(date);
            return objData;
        }

        [Route("IVROrder")]
        [HttpGet]
        public List<IVROrderDetails> GetIVROrderDetails(string date)
        {
            List<IVROrderDetails> objData = new List<IVROrderDetails>();
            objData = _IVRScheduling.GetIVROrderDetails(date);
            return objData;
        }

        [Route("IVRPendingAttempt")]
        [HttpGet]
        public List<IVROrderDetails> GetIVRPendingAttempt(int status,string orderNo,string name,int type)
        {
            List<IVROrderDetails> objData = new List<IVROrderDetails>();
            objData = _IVRScheduling.GetIVRPendingAttempt(status, orderNo, name, type);
            return objData;
        }
    }
}
