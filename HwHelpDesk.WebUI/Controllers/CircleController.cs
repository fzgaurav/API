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
    [RoutePrefix("api/Circle")]
    public class CircleController : ApiController
    {
        private ICircleManage _circle;
        public CircleController()
        {
            if (_circle == null)
            {
                _circle = new CircleManage();
            }
        }
        [Route("getcircle")]
        [HttpGet]
        public IHttpActionResult GetCircle()
        {
            try
            {
                List<Circle> objCircle = new List<Circle>();
                objCircle = _circle.GetMany();
                return Ok(objCircle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("getCircleByID")]
        [HttpGet]
        public IHttpActionResult GetCircle(int circleID)
        {
            try
            {
                List<Circle> objCircle = new List<Circle>();
                objCircle = _circle.GetMany(circleID);
                return Ok(objCircle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
