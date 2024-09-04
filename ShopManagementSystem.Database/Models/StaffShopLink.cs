namespace ShopManagementSystem.Database.Models;

public partial class StaffShopLink
{
    public int StaffId { get; set; }
    public int ShopId { get; set; }

    public virtual Staff Staff { get; set; }
    public virtual Shop Shop { get; set; }
}

