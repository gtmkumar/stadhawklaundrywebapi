﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StadhawkLaundry.ViewModel.ResponseModel
{
    public class NavigatorResponseModel
    {
        private List<Nav> _menu = new List<Nav>();
        List<Nav> _SubMenu = new List<Nav>();

        public List<Nav> SubMenu
        {
            get { return _SubMenu; }
            set { _SubMenu = value; }
        }
        public List<Nav> Menu
        {
            get { return _menu; }
            set { _menu = value; }
        }
    }
    public class Nav
    {
        public Nav()
        {
            SubMenu = new List<Nav>();
        }

        public int Id { get; set; }

        public string URL { get; set; }

        public string DisplayText { get; set; }

        public bool IsSubMenu { get; set; }

        public string ClassName { get; set; }

        public string Icon { get; set; }

        public int ParentId { get; set; }

        public List<Nav> SubMenu { get; set; }
        public Guid Privilege_Id { get; set; }
    }
}
