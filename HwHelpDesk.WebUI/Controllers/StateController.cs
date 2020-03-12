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
    [RoutePrefix("api/State")]
    public class StateController : ApiController
    {
        private IStateManage _IStateManage;
        public StateController()
        {
            if(_IStateManage==null)
            {
                _IStateManage = new StateManage();
            }
        }
        [Route("getStateByCircleID")]
        [HttpGet]
        public IHttpActionResult GetStateCircleWise(int circleID)
        {
            try
            {
                List<State> objList = new List<State>();
                objList = _IStateManage.GetStateByCircleID(circleID);
                return Ok(objList);
            }
            catch(Exception Ex)
            {
                return BadRequest("Invalid Circle ID");
            }
        }
        

    }
}
