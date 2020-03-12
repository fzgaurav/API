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
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {
        private IRoleManage _role;
        public RoleController()
        {
            if (_role == null)
            {
                _role = new RoleManage();
            }
        }
        [Route("getActiveRoles")]
        [HttpGet]
        public IHttpActionResult GetActiveRoles()
        {
            try
            {
                List<Role> objList = new List<Role>();
                objList = _role.GetActiveRoles();
                return Ok(objList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("getAllRoles")]
        [HttpGet]
        public IHttpActionResult GetAllRoles()
        {
            try
            {
                List<Role> objList = new List<Role>();
                objList = _role.GetAllRoles();
                return Ok(objList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
