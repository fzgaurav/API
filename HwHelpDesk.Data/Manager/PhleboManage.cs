using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DataTransferObject;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class PhleboManage:IPhleboManage
    {
        LiveDBContext _dbContext;
        public PhleboManage()
        {
            if(_dbContext==null)
            {
                _dbContext = new LiveDBContext();
            }
        }
        public List<PhleboData> GetPhleboList(int phleboID,int isActive)
        {
            List<PhleboData> objList = new List<PhleboData>();
            var ID = new SqlParameter("@phleboID", phleboID);
            var status = new SqlParameter("@isActive", isActive);
            objList = _dbContext.Database.SqlQuery<PhleboData>("EXEC dbo.USP_getPhlebo_fullfillment @phleboID,@isActive", ID, status).ToList();
            return objList;
        }

        public List<DropDownDto> GetPhlebo(int phleboID)
        {
            List<DropDownDto> objList = new List<DropDownDto>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT ui.userid AS phleboID, u_title+' '+ u_name+' ('+ul.username+')' AS phleboName
                            FROM user_informations ui
                            INNER JOIN user_login ul ON ui.userid = ul.user_id
                            INNER JOIN user_role ur ON ul.user_role_r_id = ur.r_id
                            WHERE ISNULL(ul.U_enable, 0)=1 AND r_role_name = 'Phlebos' ORDER BY u_name ASC;");
            objList = _dbContext.Database.SqlQuery<DropDownDto>(strBld.ToString()).ToList();
            return objList;
        }

        public List<APIResponse> CreatePhlebo(List<PhleboData> objData)
        {
            List<APIResponse> responseList = new List<APIResponse>();
            // todo
            return responseList;
        }
    }
}
