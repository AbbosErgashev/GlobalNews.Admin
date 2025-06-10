using Microsoft.AspNetCore.Mvc;
using News.Admin.DTO.CategoryDto;
using News.Admin.IService;

namespace News.Admin.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            _logger.LogInformation("Request received for Category Index page");
            var categories = await _categoryService.GetAllAsync();

            if (categories is null)
            {
                _logger.LogWarning("No categories found when retrieving all categories");
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved {CategoryCount} categories", categories.Count);
            return View(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving categories");
            return StatusCode(500, "An error occurred while retrieving categories");
        }
    }

    /// <summary>
    /// GET Index
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        _logger.LogInformation("Request received for Category Create page");
        return View();
    }

    /// <summary>
    /// CREATE Category
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        try
        {
            _logger.LogInformation("Attempting to create new category");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for category creation");
                return View(dto);
            }

            await _categoryService.CreateAsync(dto);
            _logger.LogInformation("Successfully created new category: {CategoryName}", dto.Name);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating category: {CategoryName}", dto.Name);
            ModelState.AddModelError("", "An error occurred while creating the category. Please try again.");
            return View(dto);
        }
    }

    /// <summary>
    /// GET Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            _logger.LogInformation("Request received to edit category with ID: {CategoryId}", id);
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found for editing with ID: {CategoryId}", id);
                return NotFound();
            }

            var updateDto = new CategoryUpdateDto
            {
                Id = category.Id,
                Name = category.Name
            };

            _logger.LogInformation("Successfully retrieved category for editing: {CategoryName}", category.Name);
            return View(updateDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving category for editing with ID: {CategoryId}", id);
            return StatusCode(500, "An error occurred while retrieving the category");
        }
    }

    /// <summary>
    /// POST edited Category
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryUpdateDto dto)
    {
        try
        {
            _logger.LogInformation("Attempting to update category with ID: {CategoryId}", dto.Id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for category update");
                return View(dto);
            }

            var result = await _categoryService.UpdateAsync(dto);

            if (!result)
            {
                _logger.LogWarning("Failed to update category with ID: {CategoryId}", dto.Id);
                return NotFound();
            }

            _logger.LogInformation("Successfully updated category with ID: {CategoryId}", dto.Id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating category with ID: {CategoryId}", dto.Id);
            ModelState.AddModelError("", "An error occurred while updating the category. Please try again.");
            return View(dto);
        }
    }

    /// <summary>
    /// GET Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            _logger.LogInformation("Request received for details of category with ID: {CategoryId}", id);
            var entity = await _categoryService.GetByIdAsync(id);

            if (entity == null)
            {
                _logger.LogWarning("Category not found for details view with ID: {CategoryId}", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved details for category: {CategoryName}", entity.Name);
            return View(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving category details with ID: {CategoryId}", id);
            return StatusCode(500, "An error occurred while retrieving category details");
        }
    }

    /// <summary>
    /// GET Category for DELETE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Request received to delete category with ID: {CategoryId}", id);
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category not found for deletion with ID: {CategoryId}", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved category for deletion confirmation: {CategoryName}", category.Name);
            return View(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving category for deletion with ID: {CategoryId}", id);
            return StatusCode(500, "An error occurred while retrieving the category");
        }
    }

    /// <summary>
    /// DELETE Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            _logger.LogInformation("Attempting to delete category with ID: {CategoryId}", id);
            await _categoryService.DeleteAsync(id);

            _logger.LogInformation("Successfully deleted category with ID: {CategoryId}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting category with ID: {CategoryId}", id);
            return StatusCode(500, "An error occurred while deleting the category");
        }
    }
}
