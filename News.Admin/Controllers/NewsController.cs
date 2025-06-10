using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using News.Admin.DTO;
using News.Admin.DTO.NewsDto;
using News.Admin.IService;
using News.Admin.Models;

namespace News.Admin.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _service;
    private readonly ICategoryService _categoryService;

    public NewsController(INewsService service, ICategoryService categoryService)
    {
        _service = service;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(PaginationDto pgnDto)
    {
        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        ViewBag.SortOrder = pgnDto.SortOrder;
        var pageSize = 3;
        var paginationDto = new PaginationDto
        {
            Page = pgnDto.Page,
            PageSize = pgnDto.PageSize,
            SearchText = pgnDto.SearchText,
            SortOrder = pgnDto.SortOrder ?? "desc",
            CategoryId = pgnDto.CategoryId
        };

        var entity = await _service.GetPaginationAsync(paginationDto);
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = pgnDto.CategoryId.HasValue && c.Id == pgnDto.CategoryId
        }).ToList();

        ViewBag.SelectedCategoryId = pgnDto.CategoryId;
        return View(entity);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsItemCreateDto item)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("CategoryId", "Please, CHeck the category.");
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(item);
        }

        await _service.CreateAsync(item);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id is null) return NotFound();

        var entity = await _service.GetByIdAsync(id.Value);
        if (entity is null) return NotFound();

        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = categories;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NewsItemEditDto dto)
    {
        if (id != dto.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;
            return View(dto);
        }

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

    public async Task<IActionResult> SearchPartial(string searchText, int page = 1)
    {
        var pageSize = 3;
        var paginationDto = new PaginationDto
        {
            Page = page,
            PageSize = pageSize,
            SearchText = searchText
        };

        var entity = await _service.GetPaginationAsync(paginationDto);
        return PartialView("_NewsListPartial", entity);
    }
}
