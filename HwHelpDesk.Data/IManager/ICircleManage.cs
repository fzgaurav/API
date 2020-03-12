using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface ICircleManage
    {
        List<Circle> GetMany();

        List<Circle> GetMany(int circleID);
    }
}
