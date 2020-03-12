
using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class CircleManage:ICircleManage
    {
        LiveDBContext _dbContext;
        public CircleManage()
        {
            if (_dbContext == null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<Circle> GetMany()
        {
            List<Circle> circleList = new List<Circle>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(" SELECT CircleId,CircleName FROM Circle_Master WHERE IsActive=1 Order By CircleName ");
            circleList = _dbContext.Database.SqlQuery<Circle>(strBld.ToString()).ToList();
            //_dbContext.SaveChanges();
            return circleList;
        }
        public List<Circle> GetMany(int circleID)
        {
            List<Circle> circleList = new List<Circle>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(" SELECT CircleId,CircleName FROM Circle_Master WHERE IsActive=1 AND circleID="+ circleID + " Order By CircleName ");
            circleList = _dbContext.Database.SqlQuery<Circle>(strBld.ToString()).ToList();
            //_dbContext.SaveChanges();
            return circleList;
        }
    }
}
