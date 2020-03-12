using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IStateManage
    {
        List<State> GetMany();

        List<State> GetStateByCircleID(int circleID);

        List<State> GetStateByStateID(int stateID);
    }
}
