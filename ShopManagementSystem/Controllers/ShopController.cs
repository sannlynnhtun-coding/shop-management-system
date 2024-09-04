using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagementSystem.Services;

namespace ShopManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController : ControllerBase
{
    private readonly ShopService _shopService;

    public ShopController(ShopService shopService)
    {
        _shopService = shopService;
    }

    [HttpGet("staff/{staffId}")]
    public async Task<IActionResult> GetShopsForStaff(int staffId)
    {
        try
        {
            var shops = await _shopService.GetShopsForStaffAsync(staffId);
            return Ok(shops);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("staff/{staffId}/shop/{shopId}")]
    public async Task<IActionResult> AddShopAccessForStaff(int staffId, int shopId)
    {
        try
        {
            await _shopService.AddShopAccessForStaffAsync(staffId, shopId);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
