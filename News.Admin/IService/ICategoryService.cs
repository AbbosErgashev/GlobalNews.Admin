using News.Admin.DTO.CategoryDto;

namespace News.Admin.IService;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int? id);
    Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
    Task<bool> UpdateAsync(CategoryUpdateDto dto);
    Task DeleteAsync(int id);
}
