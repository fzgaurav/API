using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IPhleboManage
    {
        List<PhleboData> GetPhleboList(int phleboID,int isActive);

        List<DropDownDto> GetPhlebo(int phleboID);

        List<APIResponse> CreatePhlebo(List<PhleboData> objData);

    }
}
