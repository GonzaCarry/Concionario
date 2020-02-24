using System;
using System.Collections.Generic;
using System.Text;

namespace Concesionario.Models
{
    public enum MenuItemType
    {
        Home,
        Users,
        Info
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
