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
    [RoutePrefix("api/Menu")]
    public class MenuManagerController : ApiController
    {
        private IMenuManage _menu;
        public MenuManagerController()
        {
            if (_menu == null)
            {
                _menu = new MenuManage();
            }
        }

        [Route("getMenuByRoleID")]
        [HttpGet]
        public IHttpActionResult GetMenuRoleIDWise(int roleID)
        {
            try
            {
                List<MenuManager> objMenu = new List<MenuManager>();
                objMenu = _menu.GetAllMenuRoleWise(roleID);
                return Ok(objMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("INSERT")]
        [HttpPost]
        public IHttpActionResult INSERT(List<MenuManager> Menu)
        {
            try
            {
                List<APIResponse> objResponse = new List<APIResponse>();
                if (Menu != null)
                {
                    for (int i = 0; i < Menu.Count; i++)
                    {
                        objResponse = _menu.InsertMenuPermission(Menu[i]);
                    }
                }
                return Ok(objResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("showDynamicMenu")]
        [HttpGet]
        public IHttpActionResult GetDynamicMenuArray(int roleID)
        {
            try
            {
                List<MenuData> objMenu = new List<MenuData>();
                objMenu = _menu.GetDynamicMenuArray(roleID);
                return Ok(objMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    
    public class MenuManagerData
    {
        string RoleId { get; set; }
        public List<MenuManager> MenuData { get; set; }
    }
}
