using Microsoft.EntityFrameworkCore;
using News.Admin.Data;
using News.Admin.DTO.CategoryDto;
using News.Admin.IService;
using News.Admin.Models;
using News.Admin.NewsMapping;

namespace News.Admin.Service;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int? id)
    {
        var entity = await _context.Categories.FindAsync(id);

        return entity is null ? null : CategoryMapper.ToDto(entity);
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
    {
        var category = new Category { Name = dto.Name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto { Id = category.Id, Name = category.Name };
    }

    public async Task<bool> UpdateAsync(CategoryUpdateDto dto)
    {
        var category = await _context.Categories.FindAsync(dto.Id);
        if (category == null) return false;

        category.Name = dto.Name;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) 
            throw new ArgumentNullException(nameof(category));

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
