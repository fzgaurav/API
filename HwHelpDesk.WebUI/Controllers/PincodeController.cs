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
using System.Web.Http.Routing;

namespace HwHelpDesk.WebUI.Controllers
{
    [RoutePrefix("api/Pincode")]
    public class PincodeController : ApiController
    {
        private IPincodeManage _pincode;
        public PincodeController()
        {
            if(_pincode==null)
            {
                _pincode = new PincodeManage();
            }
        }


        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult InsertPincode(Pincode objData)
        {
            List<APIResponse> objResponse = new List<APIResponse>();
            try
            {
                objResponse = _pincode.InsertPincode(objData);
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }


        [Route("getpincode")]
        [HttpGet]
        public IHttpActionResult GetPincode()
        {
            try
            {
                List<PincodeData> objDistrict = new List<PincodeData>();
                objDistrict = _pincode.GetPincode();
                return Ok(objDistrict);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
