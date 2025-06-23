using Microsoft.AspNetCore.Mvc;
using News.Admin.DTO;
using News.Admin.IService;

namespace News.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsApiController : ControllerBase
{
    private readonly INewsService _service;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<NewsController> _logger;

    public NewsApiController(INewsService service, ICategoryService categoryService, ILogger<NewsController> logger)
    {
        _service = service;
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// endpoint for user
    /// </summary>
    /// <param name="paginationDto"></param>
    /// <returns></returns>
    [HttpGet("pagination")]
    public async Task<IActionResult> GetPagination([FromQuery] PaginationDto paginationDto)
    {
        try
        {
            _logger.LogInformation("API Pagination endpoint called.");
            var paginatedNews = await _service.GetPaginationAsync(paginationDto);
            return Ok(paginatedNews);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in API Pagination endpoint.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> Details(Guid? id)
    {
        try
        {
            _logger.LogInformation("Details action called with Id: {Id}", id);

            if (id is null) return NotFound();

            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            return Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Details action.");
            return StatusCode(500, "Internal server error");
        }
    }
}
