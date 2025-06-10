using Microsoft.AspNetCore.Mvc;
using News.Admin.DTO.CategoryDto;
using News.Admin.IService;

namespace News.Admin.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        if (categories is null) return NotFound();

        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _categoryService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        var updateDto = new CategoryUpdateDto
        {
            Id = category.Id,
            Name = category.Name
        };

        return View(updateDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _categoryService.UpdateAsync(dto);
        if (!result)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var entity = await _categoryService.GetByIdAsync(id);
        if(entity == null) return NotFound();

        return View(entity);
    }
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
            return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
