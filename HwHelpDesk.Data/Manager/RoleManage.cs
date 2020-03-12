using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class RoleManage:IRoleManage
    {
        LiveDBContext _dbContext;
        public RoleManage()
        {
            if (_dbContext == null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<Role> GetActiveRoles()
        {
            List<Role> objList = new List<Role>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append("SELECT r_id,r_role_name,PREFIX,1 r_active FROM user_role WHERE r_active=1  ORDER BY r_role_name");
            objList = _dbContext.Database.SqlQuery<Role>(strBld.ToString()).ToList();
            return objList;
        }

        public List<Role> GetAllRoles()
        {
            List<Role> objList = new List<Role>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append("SELECT r_id,r_role_name,PREFIX,1 r_active FROM user_role  ORDER BY r_role_name");
            objList = _dbContext.Database.SqlQuery<Role>(strBld.ToString()).ToList();
            return objList;
        }


    }
}
