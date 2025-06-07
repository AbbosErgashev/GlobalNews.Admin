using Microsoft.EntityFrameworkCore;
using News.Admin.Data;
using News.Admin.DTO;
using News.Admin.IService;
using News.Admin.Models;
using News.Admin.NewsMapping;

namespace News.Admin.Service;

public class NewsService : INewsService
{
    public readonly AppDbContext _context;
    public readonly IWebHostEnvironment _env;

    public NewsService(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task CreateAsync(NewsItemCreateDto itemDto)
    {
        var mediaUrl = await SaveMediaFileAsync(itemDto.MediaFile);

        var entity = new NewsItem
        {
            Id = Guid.NewGuid(),
            Title = itemDto.Title,
            Description = itemDto.Description,
            MediaUrl = mediaUrl!,
            CreatedAt = DateTime.UtcNow
        };

        _context.NewsItems.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.NewsItems.FindAsync(id);
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.NewsItems.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<NewsItemDto>> GetAllAsync()
    {
        var entities = await _context.NewsItems.OrderByDescending(x => x.CreatedAt).ToListAsync();
        if (entities.Count == 0)
            throw new ArgumentNullException(nameof(entities));

        var dto = entities.Select(NewsMapper.ToDto).ToList();
        return dto;
    }

    public async Task<NewsItemDto?> GetByIdAsync(Guid? id)
    {
        var entity = await _context.NewsItems.FindAsync(id);

        return entity is null ? null : NewsMapper.ToDto(entity);
    }

    public async Task UpdateAsync(Guid id, NewsItemEditDto itemDto)
    {
        var entity = await _context.NewsItems.FindAsync(id);
        if (entity is null)
            throw new Exception("Not Found");

        if (itemDto.NewMediaFile != null && itemDto.NewMediaFile.Length > 0)
        {
            var newMediaUrl = await SaveMediaFileAsync(itemDto.NewMediaFile);
            entity.MediaUrl = newMediaUrl;
        }

        entity.Title = itemDto.Title;
        entity.Description = itemDto.Description;
        entity.UpdatedAt = DateTime.UtcNow;

        _context.NewsItems.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<string?> SaveMediaFileAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return null;

        string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{fileName}";
    }

    public async Task<NewsPaginationDto> GetPaginationAsync(int page, int pageSize)
    {
        var totalCount = await _context.NewsItems.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        if (page < 1 || page > totalPages) page = 1;

        var items = await _context.NewsItems
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new NewsPaginationDto
        {
            NewsItemDtos = items.Select(NewsMapper.ToDto).ToList(),
            CurrentPage = page,
            TotalPage = totalPages
        };
    }
}
