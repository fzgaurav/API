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
    public class PincodeManage : IPincodeManage
    {
        LiveDBContext _dbContext;
        public PincodeManage()
        {
            if(_dbContext==null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<PincodeData> GetPincode()
        {
            List<PincodeData> objPincodeList = new List<PincodeData>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT id,pincode,City,District,state,ISNULL(cm.CircleName,'N/A') CircleName,Status_Info  FROM Pincode_Master pm
                            LEFT OUTER JOIN state s ON pm.State=s.s_name
                            LEFT OUTER JOIN Circle_Master cm ON s.circleID=cm.CircleId WHERE pm.Status_Info=1
                            ORDER BY Pincode ASC");
            objPincodeList = _dbContext.Database.SqlQuery<PincodeData>(strBld.ToString()).ToList();
            return objPincodeList;
        }

        public List<APIResponse> InsertPincode(Pincode obj)
        {
            List <APIResponse> responseList= new List<APIResponse>();
            APIResponse responseObj = new APIResponse();
            //First input parameter
            var cityID = new SqlParameter
            {
                ParameterName = "@cityID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = obj.city_id
            };
           
            //Second input parameter
            var pincode = new SqlParameter
            {
                ParameterName = "@pincode",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Input,
                Value = obj.pincode,
                Size = 500
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
            string SQLString = "EXEC [dbo].[USP_AddPincode] @pincode, @cityID, @responseMsg OUT, @responseCode OUT";
            var result = _dbContext.Database.ExecuteSqlCommand(SQLString, pincode, cityID, responseMsg, responseCode);

            responseObj.responseMsg = (string)responseMsg.Value;
            responseObj.responseCode = (int)responseCode.Value;
            responseList.Add(responseObj);
            return responseList;
        }
    }
}
