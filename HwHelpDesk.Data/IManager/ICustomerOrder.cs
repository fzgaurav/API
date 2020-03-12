using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface ICustomerOrder
    {
        List<IVRScheduling> GetIVRCallTracking(string date);
        List<IVROrderDetails> GetIVROrderDetails(string date);
        List<IVROrderDetails> GetIVRPendingAttempt(int status,string orderNo,string name,int type);
    }
}
