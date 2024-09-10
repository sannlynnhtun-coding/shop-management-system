using System;
using System.Collections.Generic;

namespace ShopManagementSystem.Database.Models;

public partial class Shop
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? LocationId { get; set; }
}
