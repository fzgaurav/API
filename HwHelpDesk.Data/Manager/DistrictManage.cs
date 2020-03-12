using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class DistrictManage: IDistrictManage
    {
        LiveDBContext _dbContext;
        public DistrictManage()
        {
            if(_dbContext==null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<APIResponse> InsertDistrict(City obj)
        {
            List<APIResponse> objResponseList = new List<APIResponse>();
              APIResponse objResponse = new APIResponse();
            var circleID = new SqlParameter
            {
                ParameterName = "@circleID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = obj.CircleId
            };

            //Second input parameter
            var stateID = new SqlParameter
            {
                ParameterName = "@stateID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = obj.state_s_id
            };

            //Second input parameter
            var districtName = new SqlParameter
            {
                ParameterName = "@districtName",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value =obj.city_name,
                Size=500
            };
            //third out parameter
            var responseMsg = new SqlParameter
            {
                ParameterName = "@responseMsg",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Output,
                Size = 500
            };
            var responseCode = new SqlParameter
            {
                ParameterName = "@responseCode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Output,
                Size = 500
            };
            string SQLString = "EXEC [dbo].[name of the Stored Proc] @circleID, @stateID, @districtName, @responseMsg OUT, @responseCode OUT";
            var result = _dbContext.Database.ExecuteSqlCommand(SQLString, circleID, stateID, districtName, responseMsg, responseCode);

            objResponse.responseMsg = (string)responseMsg.Value;
            objResponse.responseCode = (int)responseCode.Value;
            objResponseList.Add(objResponse);

            //var projectbysectorandsubsector = _context.Database.SqlQuery<ProjectsModel>("exec dbo.[GetProjectDetailsBySectorAndSubSector] @sectorId,@subSectorId", new SqlParameter("@sectorId, @subSectorId", sectorid, subsectorid)).ToList();
            return objResponseList;
        }

        public List<City> GetDistrict()
        {
            List<City> objDistrictList = new List<City>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT city_id,city_name,city_s_name,ISNULL(state_s_id,0)state_s_id,c.IsActive,
                            status_info1,ISNULL(c.CircleId,0)CircleId,ISNULL(CircleName,'N/A')CircleName,
                            ISNULL(s_name,'N/A')s_name,c.defaultRoute FROM cities c LEFT OUTER JOIN state s ON c.state_s_id = s.s_id
                            LEFT OUTER JOIN  Circle_Master cm ON c.CircleId = cm.CircleId
                            ORDER BY city_name");
            objDistrictList = _dbContext.Database.SqlQuery<City>(strBld.ToString()).ToList();
            //_dbContext.SaveChanges();
            return objDistrictList;
        }

        public List<City> GetActiveDistrict()
        {
            List<City> objDistrictList = new List<City>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT city_id,city_name,city_s_name,ISNULL(state_s_id,0)state_s_id,c.IsActive,
                            status_info1,ISNULL(c.CircleId,0)CircleId,ISNULL(CircleName,'N/A')CircleName,
                            ISNULL(s_name,'N/A')s_name FROM cities c LEFT OUTER JOIN state s ON c.state_s_id = s.s_id
                            LEFT OUTER JOIN  Circle_Master cm ON c.CircleId = cm.CircleId WHERE c.Isactive=1
                            ORDER BY city_name");
            objDistrictList = _dbContext.Database.SqlQuery<City>(strBld.ToString()).ToList();
            //_dbContext.SaveChanges();
            return objDistrictList;
        }

        public List<City> GetDistrictByStateID(int stateID)
        {
            List<City> objDistrictList = new List<City>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT city_id,city_name,city_s_name,ISNULL(state_s_id,0)state_s_id,c.IsActive,
                            status_info1,ISNULL(c.CircleId,0)CircleId,ISNULL(CircleName,'N/A')CircleName,
                            ISNULL(s_name,'N/A')s_name FROM cities c LEFT OUTER JOIN state s ON c.state_s_id = s.s_id
                            LEFT OUTER JOIN  Circle_Master cm ON c.CircleId = cm.CircleId WHERE c.state_s_id=" + stateID + " ORDER BY city_name");
            objDistrictList = _dbContext.Database.SqlQuery<City>(strBld.ToString()).ToList();
            //_dbContext.SaveChanges();
            return objDistrictList;
        }

        public List<APIResponse> UpdateDefaultRoute(City obj)
        {
            List<APIResponse> objResponseList = new List<APIResponse>();
            APIResponse objResponse = new APIResponse();
            var districtID = new SqlParameter
            {
                ParameterName = "@districtID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = obj.city_id
            };

            //Second input parameter
            var defaultRoute = new SqlParameter
            {
                ParameterName = "@defaultRoute",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = obj.defaultRoute
            };
            //third out parameter
            var responseMsg = new SqlParameter
            {
                ParameterName = "@responseMsg",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Output,
                Size = 500
            };
            var responseCode = new SqlParameter
            {
                ParameterName = "@responseCode",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            string SQLString = "EXEC [dbo].[USP_UpdateDefaultRoute] @districtID, @defaultRoute, @responseMsg OUT, @responseCode OUT";
            var result = _dbContext.Database.ExecuteSqlCommand(SQLString, districtID, defaultRoute, responseMsg, responseCode);

            objResponse.responseMsg = (string)responseMsg.Value;
            objResponse.responseCode = (int)responseCode.Value;
            objResponseList.Add(objResponse);

            //var projectbysectorandsubsector = _context.Database.SqlQuery<ProjectsModel>("exec dbo.[GetProjectDetailsBySectorAndSubSector] @sectorId,@subSectorId", new SqlParameter("@sectorId, @subSectorId", sectorid, subsectorid)).ToList();
            return objResponseList;
        }
    }
}
