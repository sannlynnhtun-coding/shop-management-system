using System;
using System.Collections.Generic;

namespace ShopManagementSystem.Database.Models;

public partial class Shop
{
    public Shop()
    {
        StaffShopLinks = new HashSet<StaffShopLink>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int? LocationId { get; set; }

    public virtual Location Location { get; set; }
    public virtual ICollection<StaffShopLink> StaffShopLinks { get; set; }
}

