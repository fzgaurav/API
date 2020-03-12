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
    
    [RoutePrefix("api/District")]
    public class DistrictController : ApiController
    {
        private IDistrictManage _district;
        public DistrictController()
        {
            if(_district==null)
            {
                _district = new DistrictManage();
            }
        }        
        
        [Route("Insert")]
        [HttpPost]
        public IHttpActionResult InsertDistrict(City objData)
        {
            try
            {
                _district.InsertDistrict(objData);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [Route("getdistrict")]
        [HttpGet]
        public IHttpActionResult GetDistrict()
        {
            try
            {
                List<City> objDistrict = new List<City>();
                objDistrict = _district.GetDistrict();
                return Ok(objDistrict);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("getActivedistrict")]
        [HttpGet]
        public IHttpActionResult GetActiveDistrict()
        {
            try
            {
                List<City> objDistrict = new List<City>();
                objDistrict = _district.GetActiveDistrict();
                return Ok(objDistrict);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("getdistrictbystateID")]
        [HttpGet]
        public IHttpActionResult GetDistrictStateWise(int stateID)
        {
            try
            {
                List<City> objDistrict = new List<City>();
                objDistrict = _district.GetDistrictByStateID(stateID);
                return Ok(objDistrict);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("SetDefaultRoute")]
        [HttpPut]
        public IHttpActionResult UpdateDefaultRoute(City objData)
        {
            try
            {
                List<APIResponse> obj=new List<APIResponse>();
                obj = _district.UpdateDefaultRoute(objData);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
