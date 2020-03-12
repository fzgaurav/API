using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IRoleManage
    {
        //Get All Roles
        List<Role> GetAllRoles();
        //Only Active Roles
        List<Role> GetActiveRoles();

    }
}
