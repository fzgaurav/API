using HwHelpDesk.Models;
using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HwHelpDesk.WebUI.Controllers
{
   
    public class LoginController : ApiController
    {
        Utility obj = new Utility();
        #region Booking API
        [Route("api/Login")]
        [HttpPost]
        public IHttpActionResult Login(userLogin Login)
        {
            try
            {
                LoginResponse response = new LoginResponse();
                if (Login != null)
                {
                    try
                    {
                        /*Check Login Credential*/
                        response = obj.UserLogin(Login.userName,Login.password,Login.tokenID,Login.ipAddress);
                        if (response.MsgCode > 0)
                        {
                            return Ok(response);
                        }
                        else
                        {
                            return BadRequest(response.Msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

       

    }
}
