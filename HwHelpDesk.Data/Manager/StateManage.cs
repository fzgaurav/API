using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class StateManage: IStateManage
    {
        LiveDBContext _dbContext;
        public StateManage()
        {
            if (_dbContext == null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<State> GetMany()
        {
            List<State> objList = new List<State>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append("SELECT s_id,s_name,s_country,circleID FROM state Order By s_name ");
            objList = _dbContext.Database.SqlQuery<State>(strBld.ToString()).ToList();
            return objList;
        }
        public List<State> GetStateByCircleID(int circleID)
        {
            List<State> objList = new List<State>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append("SELECT s_id,s_name,s_country,circleID FROM state WHERE circleID=" + circleID + " Order By s_name ");
            objList = _dbContext.Database.SqlQuery<State>(strBld.ToString()).ToList();
            return objList;
        }

        public List<State> GetStateByStateID(int stateID)
        {
            List<State> objList = new List<State>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append("SELECT s_id,s_name,s_country,circleID FROM state WHERE s_ID=" + stateID + " Order By s_name ");
            objList = _dbContext.Database.SqlQuery<State>(strBld.ToString()).ToList();
            return objList;
        }
    }
}
