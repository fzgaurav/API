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
    [RoutePrefix("api/droppoint")]
    public class DropPointController : ApiController
    {
        private IDropPointManage _dropPoint;
        public DropPointController()
        {
            if(_dropPoint==null)
            {
                _dropPoint = new DropPointManage();
            }
        }

        public IHttpActionResult GetDropPoint(int dropPointID)
        {
            try
            {
                List<DropDownDto> objDropPoint = new List<DropDownDto>();
                objDropPoint = _dropPoint.GetDropPoint(dropPointID);
                return Ok(objDropPoint);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
