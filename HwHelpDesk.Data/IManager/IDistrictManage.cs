using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.IManager
{
    public interface IDistrictManage
    {
        List<APIResponse> InsertDistrict(City objData);
        List<City> GetDistrict();
        List<City> GetDistrictByStateID(int stateID);
        List<City> GetActiveDistrict();
        List<APIResponse> UpdateDefaultRoute(City objData);
    }
}
