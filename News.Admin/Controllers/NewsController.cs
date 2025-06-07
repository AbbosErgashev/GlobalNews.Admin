using Microsoft.AspNetCore.Mvc;
using News.Admin.DTO;
using News.Admin.IService;

namespace News.Admin.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _service;

    public NewsController(INewsService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string searchText, int page = 1)
    {
        var pageSize = 3;
        var paginationDto = new PaginationDto { Page = page, PageSize = pageSize, SearchText = searchText };
        var entity = await _service.GetPaginationAsync(paginationDto);
        if (entity == null) return NoContent();
        return View(entity);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsItemCreateDto item)
    {
        if (!ModelState.IsValid) return View(item);

        await _service.CreateAsync(item);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id is null) return NotFound();

        var entity = await _service.GetByIdAsync(id.Value);
        if (entity is null) return NotFound();

        var dto = new NewsItemEditDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            UpdatedAt = DateTime.UtcNow
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NewsItemEditDto dto)
    {
        if (id != dto.Id) return NotFound();
        if (!ModelState.IsValid) return View(dto);

        await _service.UpdateAsync(id, dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var entity = await _service.GetByIdAsync(id.Value);
        if (entity is null) return NotFound();

        return View(entity);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(Guid? id)
    {
        if (id is null) return NotFound();

        var entity = await _service.GetByIdAsync(id);
        if (entity == null) return NotFound();

        return View(entity);
    }
}
