using News.Admin.DTO;

namespace News.Admin.IService;

public interface INewsService
{
    public Task<NewsItemDto?> GetByIdAsync(Guid? id);
    public Task CreateAsync(NewsItemCreateDto itemDto);
    public Task UpdateAsync(Guid id, NewsItemEditDto itemDto);
    public Task DeleteAsync(Guid id);
    public Task<string?> SaveMediaFileAsync(IFormFile? file);
    public Task<NewsPaginationDto> GetPaginationAsync(PaginationDto pgnDto);
}
