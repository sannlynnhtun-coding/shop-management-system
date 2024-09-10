using System;
using System.Collections.Generic;

namespace ShopManagementSystem.Database.Models;

public partial class StaffShopLink
{
    public int StaffId { get; set; }

    public int ShopId { get; set; }
}
