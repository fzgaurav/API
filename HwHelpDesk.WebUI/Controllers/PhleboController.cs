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
    [RoutePrefix("api/phlebo")]
    public class PhleboController : ApiController
    {
        private IPhleboManage _phlebo;
        public PhleboController()
        {
            if(_phlebo==null)
            {
                _phlebo = new PhleboManage();
            }
        }

        public IHttpActionResult GetPhlebo(int phlebo)
        {
            try
            {
                List<DropDownDto> objPhlebo = new List<DropDownDto>();
                objPhlebo = _phlebo.GetPhlebo(phlebo);
                return Ok(objPhlebo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetPhleboList(int phlebo,int isActive,int dropPointID,int phleboType,int helpDeskID)
        {
            try
            {
                List<PhleboData> objPhlebo = new List<PhleboData>();
                objPhlebo = _phlebo.GetPhleboList(phlebo, isActive);
                return Ok(objPhlebo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult CreatePhlebo(List<PhleboData> objData)
        {
            try
            {
                List<APIResponse> objPhlebo = new List<APIResponse>();
                objPhlebo = _phlebo.CreatePhlebo(objData);
                return Ok(objPhlebo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
