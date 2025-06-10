using News.Admin.DTO.CategoryDto;

namespace News.Admin.IService;

public interface ICategoryService
{
    public Task<List<CategoryDto>> GetAllAsync();
    public Task<CategoryDto?> GetByIdAsync(int? id);
    public Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
    public Task<bool> UpdateAsync(CategoryUpdateDto dto);
    public Task DeleteAsync(int id);
}
