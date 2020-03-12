using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class userLogin
    {
        public string userName { get; set; }        
        public string password { get; set; }
        public string ipAddress { get; set; }
        public string tokenID { get; set; }
    }
}
