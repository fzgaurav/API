using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IPincodeManage
    {
        List<APIResponse> InsertPincode(Pincode objData);
        List<PincodeData> GetPincode();
    }
}
