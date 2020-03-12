using HwHelpDesk.Data.IManager;
using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Data.Manager
{
    public class CustomerOrderManage:ICustomerOrder
    {
        LiveDBContext _dbContext;
        public CustomerOrderManage()
        {
            if(_dbContext == null)
            {
                _dbContext = new LiveDBContext();
            }
        }

        public List<IVRScheduling> GetIVRCallTracking(string date)
        {
            List<IVRScheduling> objData = new List<IVRScheduling>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT COUNT(DISTINCT CASE WHEN ISNULL(co.o_id,'')<>'' THEN co.o_ID END ) TotalOrder,0 PendingAttempt,
                            COUNT(DISTINCT CASE WHEN ISNULL(coim.o_id,'')<>'' THEN coim.o_id END ) Attempt,0 Verified,0 NoAnswer,
                            0 CallToCustomerCare,0 CallByIVR FROM customer_order co 
                            LEFT OUTER JOIN Customer_Order_IVR_Master coim ON co.o_id=coim.o_id WHERE co.o_date='" + date + "'");
            objData = _dbContext.Database.SqlQuery<IVRScheduling>(strBld.ToString()).ToList();
            return objData;

        }

        public List<IVROrderDetails> GetIVROrderDetails(string date)
        {
            List<IVROrderDetails> objData = new List<IVROrderDetails>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT co.o_id OrderID,co.o_number OrderNo,ca.ca_fname+' '+ca.ca_middle_name+' '+ca.ca_sname CustomerName ,
                            co.o_net_payable Price,[dbo].[findproductname] (co.o_id) as Product,0 IvrStatus
                            FROM customer_order co 
                            LEFT OUTER JOIN Customer_Order_IVR_Master coim ON co.o_id=coim.o_id
                            INNER JOIN customer_account ca ON co.ca_id=ca.ca_id
                            WHERE co.o_date='" + date + "'");
            objData = _dbContext.Database.SqlQuery<IVROrderDetails>(strBld.ToString()).ToList();
            return objData;

        }

        public List<IVROrderDetails> GetIVRPendingAttempt(int Status,string OrderNo,string Name,int type)
        {
            List<IVROrderDetails> objData = new List<IVROrderDetails>();
            StringBuilder strBld = new StringBuilder();
            strBld.Append(@"SELECT co.o_id OrderID,co.o_number OrderNo,ca.ca_fname+' '+ca.ca_middle_name+' '+ca.ca_sname CustomerName ,
                            co.o_net_payable Price,[dbo].[findproductname] (co.o_id) as Product,0 IvrStatus
                            FROM customer_order co 
                            LEFT OUTER JOIN Customer_Order_IVR_Master coim ON co.o_id=coim.o_id
                            INNER JOIN customer_account ca ON co.ca_id=ca.ca_id
                            WHERE  ivr_status=" + Status + "");
            objData = _dbContext.Database.SqlQuery<IVROrderDetails>(strBld.ToString()).ToList();
            return objData;

        }
    }
}
