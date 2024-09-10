using System;
using System.Collections.Generic;

namespace ShopManagementSystem.Database.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool HasGlobalAccess { get; set; }
}
