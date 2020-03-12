using System;
using System.Collections.Generic;
using System.Data.Entity;
using HwHelpDesk.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HwHelpDesk.Shared.DomainEntity;

namespace HwHelpDesk.Data
{
    class LiveDBContext : DbContext
    {
        public LiveDBContext() : base("LocalContext") { }
        //public DbSet<UserLogin> User_login { get; set; }
    }
}
