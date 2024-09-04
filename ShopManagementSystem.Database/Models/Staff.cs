using System;
using System.Collections.Generic;

namespace ShopManagementSystem.Database.Models;

public partial class Staff
{
    public Staff()
    {
        StaffShopLinks = new HashSet<StaffShopLink>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasGlobalAccess { get; set; }

    public virtual ICollection<StaffShopLink> StaffShopLinks { get; set; }
}


