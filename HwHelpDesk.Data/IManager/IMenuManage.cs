using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IMenuManage
    {
        List<MenuManager> GetAllMenu();

        List<MenuManager> GetAllMenuRoleWise(int roleID);

        List<MenuData> GetDynamicMenuArray(int roleID);
        List<APIResponse> InsertMenuPermission(MenuManager objData);
    }
}
