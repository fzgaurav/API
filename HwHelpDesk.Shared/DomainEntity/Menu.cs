using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwHelpDesk.Shared.DomainEntity
{
    public class Menu
    {
        public int MenuName {get; set;}
        public string MenuUrl { get; set; }
    }


    public class MenuManager
    {
        public int MpID { get; set; }
        public int ModuleID { get; set; }
        public int PageID { get; set; }
        public string ModuleName { get; set; }
        public string routeUrl { get; set; }
        public string PageName { get; set; }
        public bool IsView { get; set; }
        public bool IsEdit { get; set; }
        public bool IsenableDisable { get; set; }
        public bool isActive { get; set; }
        public int RoleID { get; set; }
    }


    public class MenuData
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public List<MenuManager> Menus { get; set; }
    }
}
