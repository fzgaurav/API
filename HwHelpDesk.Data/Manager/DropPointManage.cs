using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class DropPointManage:IDropPointManage
    {
        LiveDBContext _dbContext;
        public DropPointManage()
        {
            if(_dbContext==null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<DropDownDto> GetDropPoint(int dropPointID)
        {
            List<DropDownDto> objList = new List<DropDownDto>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append("SELECT drop_point_id AS ID,drop_point_name VALUE FROM drop_point_master WHERE status_info=1 ORDER BY drop_point_name ASC;");
            objList = _dbContext.Database.SqlQuery<DropDownDto>(strBld.ToString()).ToList();
            return objList;
        }
    }
}
