using HwHelpDesk.Shared.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DataTransferObject
{
    public class LoginResponse
    {
        public string Msg { get; set; }
        public int MsgCode { get; set; }
        public List<userData> userData { get; set; }
    }
}
