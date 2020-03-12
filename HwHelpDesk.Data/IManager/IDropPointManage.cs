using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IDropPointManage
    {
        List<DropDownDto> GetDropPoint(int dropPointID);
    }
}
