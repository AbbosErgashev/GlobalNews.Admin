using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using News.Admin.DTO;
using News.Admin.DTO.NewsDto;
using News.Admin.IService;

namespace News.Admin.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _service;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<NewsController> _logger;

    public NewsController(INewsService service, ICategoryService categoryService, ILogger<NewsController> logger)
    {
        _service = service;
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int? page, int? pageSize, string sortOrder, int? categoryId)
    {
        try
        {
            _logger.LogInformation("Index action called.");

            int currentPage = page ?? 1;
            int currentPageSize = pageSize ?? 3;
            string currentSortOrder = string.IsNullOrEmpty(sortOrder) ? "desc" : sortOrder;

            var paginationDto = new PaginationDto
            {
                Page = currentPage,
                PageSize = currentPageSize,
                SortOrder = currentSortOrder,
                CategoryId = categoryId
            };

            var entity = await _service.GetPaginationAsync(paginationDto);
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = categoryId.HasValue && c.Id == categoryId.Value
            }).ToList();

            return View(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Index action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// CREATE New
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Create()
    {
        try
        {
            _logger.LogInformation("Create GET action called.");
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Create GET action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// POST New
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsItemCreateDto item)
    {
        try
        {
            _logger.LogInformation("Create POST action called.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed in Create POST action.");
                ModelState.AddModelError("CategoryId", "Please, check the category.");
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                return View(item);
            }

            await _service.CreateAsync(item);
            _logger.LogInformation("News item created successfully.");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Create POST action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// GET New
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id)
    {
        try
        {
            _logger.LogInformation("Edit GET action called with Id: {Id}", id);

            if (id is null) return NotFound();

            var entity = await _service.GetByIdAsync(id.Value);
            if (entity is null) return NotFound();

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var dto = new NewsItemEditDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                UpdatedAt = DateTime.UtcNow,
                ExistingMediaUrl = entity.MediaUrl,
                CategoryId = entity.CategoryId
            };

            return View(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Edit GET action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// POST New
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NewsItemEditDto dto)
    {
        try
        {
            _logger.LogInformation("Edit POST action called with Id: {Id}", id);

            if (id != dto.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed in Edit POST action.");
                ModelState.AddModelError("CategoryId", "Please, check the category.");
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                return View(dto);
            }

            await _service.UpdateAsync(id, dto);
            _logger.LogInformation("News item updated successfully.");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Edit POST action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// DELETE New
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        try
        {
            _logger.LogInformation("Delete GET action called with Id: {Id}", id);

            if (id == null) return NotFound();

            var entity = await _service.GetByIdAsync(id.Value);
            if (entity is null) return NotFound();

            return View(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Delete GET action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// POST New for confirming
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            _logger.LogInformation("Delete POST action called with Id: {Id}", id);

            await _service.DeleteAsync(id);
            _logger.LogInformation("News item deleted successfully.");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Delete POST action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// GET New
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        try
        {
            _logger.LogInformation("Details action called with Id: {Id}", id);

            if (id is null) return NotFound();

            var entity = await _service.GetByIdAsync(id);
            if (entity == null) return NotFound();

            return View(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in Details action.");
            return RedirectToAction("Error", "Home");
        }
    }

    /// <summary>
    /// Search by all of the types
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<IActionResult> SearchPartial(string searchText, int page = 1)
    {
        try
        {
            _logger.LogInformation("SearchPartial action called with searchText: {SearchText}, page: {Page}", searchText, page);

            var pageSize = 3;
            var paginationDto = new PaginationDto
            {
                Page = page,
                PageSize = pageSize,
                SearchText = searchText
            };

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var entity = await _service.GetPaginationAsync(paginationDto);
            return PartialView("_NewsListPartial", entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in SearchPartial action.");
            return PartialView("_NewsListPartial", new List<NewsItemDto>());
        }
    }
}
