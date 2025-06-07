using Microsoft.Build.ObjectModelRemoting;
using News.Admin.DTO;
using News.Admin.Models;

namespace News.Admin.IService;

public interface INewsService
{
    public Task<List<NewsItemDto>> GetAllAsync();
    public Task<NewsItemDto?> GetByIdAsync(Guid? id);
    public Task CreateAsync(NewsItemCreateDto itemDto);
    public Task UpdateAsync(Guid id, NewsItemEditDto itemDto);
    public Task DeleteAsync(Guid id);
    public Task<string?> SaveMediaFileAsync(IFormFile? file);
}
