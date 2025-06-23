using Microsoft.AspNetCore.Mvc;
using News.Admin.IService;

namespace News.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryApiController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryApiController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories); // bu yerda CategoryDto bo‘lishi kerak
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories.");
            return StatusCode(500, "Internal server error");
        }
    }
}
