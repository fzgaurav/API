using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class MenuManage:IMenuManage
    {
        LiveDBContext _dbContext;
        public MenuManage()
        {
            if(_dbContext==null)
            {
                _dbContext = new LiveDBContext();
            }
        }

        public List<MenuManager> GetAllMenu()
        {
            List<MenuManager> objList = new List<MenuManager>();
            var param = new SqlParameter("@roleID", 0);
            objList = _dbContext.Database.SqlQuery<MenuManager>("EXEC dbo.USP_menuManager @roleID", param).ToList();
            return objList;
        }

        public List<MenuManager> GetAllMenuRoleWise(int roleID)
        {
            List<MenuManager> objList = new List<MenuManager>();
            var param = new SqlParameter("@roleID", roleID);
            objList = _dbContext.Database.SqlQuery<MenuManager>("EXEC dbo.USP_menuManager @roleID", param).ToList();
            return objList;
        }

        //public List<MenuManager> GetDynamicMenuArray(int roleID)
        //{
        //    List<MenuManager> objList = new List<MenuManager>();
        //    var param = new SqlParameter("@roleID", roleID);
        //    objList = _dbContext.Database.SqlQuery<MenuManager>("EXEC dbo.USP_menuManager @roleID", param).ToList();
        //    return objList;
        //}


        public List<MenuData> GetDynamicMenuArray(int roleID)
        {
            List<MenuManager> objList = new List<MenuManager>();
            List<MenuData> objD = new List<MenuData>();
            var param = new SqlParameter("@roleID", roleID);
            objList = _dbContext.Database.SqlQuery<MenuManager>("EXEC dbo.USP_menuManager @roleID", param).ToList();
            if (objList != null)
            {
                var newobj = objList.Select(x => new { x.ModuleID, x.ModuleName }).Distinct().ToList();
                MenuData objData;
                foreach (var dto in newobj)
                {
                    objData = new MenuData();
                    objData.ModuleID = dto.ModuleID;
                    objData.ModuleName = dto.ModuleName;
                    objData.Menus = new List<MenuManager>();
                    var dd = objList.Where(d => d.ModuleID == dto.ModuleID).ToList();
                    objData.Menus.AddRange(dd);
                    objD.Add(objData);
                }
            }
            return objD;
        }
        public List<APIResponse> InsertMenuPermission(MenuManager objData)
        {
            List<APIResponse> objResponseList = new List<APIResponse>();
            APIResponse objResponse = new APIResponse();
            var roleID = new SqlParameter
            {
                ParameterName = "@roleID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = objData.RoleID
            };
            var pageID = new SqlParameter
            {
                ParameterName = "@pageID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = objData.PageID
            };
            var isView = new SqlParameter
            {
                ParameterName = "@isView",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = objData.IsView,
                Size = 500
            };
            var Isenabledisable = new SqlParameter
            {
                ParameterName = "@Isenabledisable",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = objData.IsenableDisable,
                Size = 500
            };
            var isEdit = new SqlParameter
            {
                ParameterName = "@isEdit",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = objData.isActive,
                Size = 500
            };
            var isActive = new SqlParameter
            {
                ParameterName = "@isActive",
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Input,
                Value = objData.isActive,
                Size = 500
            };
            var responseMsg = new SqlParameter
            {
                ParameterName = "@responseMsg",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Output,
                Size = 500
            };
            var responseCode = new SqlParameter
            {
                ParameterName = "@responseCode",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };
            string SQLString = "EXEC [dbo].[USP_AssignMenu] @roleID, @pageID, @isView,@Isenabledisable,@isEdit,@isActive, @responseMsg OUT, @responseCode OUT";
            var result = _dbContext.Database.ExecuteSqlCommand(SQLString, roleID, pageID, isView,Isenabledisable,isEdit, isActive, responseMsg, responseCode);

            objResponse.responseMsg = (string)responseMsg.Value;
            objResponse.responseCode = (int)responseCode.Value;
            objResponseList.Add(objResponse);
            return objResponseList;
        }
    }
}
