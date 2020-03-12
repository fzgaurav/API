using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class Circle
    {
        public int CircleId { get; set; }
        public string CircleName { get; set; }
        public byte? IsActive { get; set; }
    }
}
