using Microsoft.EntityFrameworkCore;
using News.Admin.Data;
using News.Admin.DTO;
using News.Admin.DTO.NewsDto;
using News.Admin.IService;
using News.Admin.Models;
using News.Admin.NewsMapping;
using System.Linq;

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
            CreatedAt = DateTime.UtcNow,
            CategoryId = itemDto.CategoryId
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

    public async Task<NewsPaginationDto> GetPaginationAsync(PaginationDto pgnDto)
    {
        var query = _context.NewsItems
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pgnDto.SearchText))
        {
            var lowered = pgnDto.SearchText.Trim().ToLower();
            query = query.Where(n =>
                n.Title.ToLower().Contains(lowered) ||
                n.Description.ToLower().Contains(lowered) ||
                n.CreatedAt.ToString().Contains(lowered) ||
                n.Category.Name.ToLower().Contains(lowered) ||
                (n.UpdatedAt != null && n.UpdatedAt.Value.ToString().Contains(lowered))
            );
        }

        if (pgnDto.CategoryId.HasValue && pgnDto.CategoryId > 0)
        {
            query = query.Where(n => n.CategoryId == pgnDto.CategoryId);
        }

        if (pgnDto.SortOrder == "asc")
        {
            query = query.OrderBy(x => x.CreatedAt);
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pgnDto.PageSize);

        if (pgnDto.Page < 1 || pgnDto.Page > totalPages) pgnDto.Page = 1;

        var items = await query
            .Skip((pgnDto.Page - 1) * pgnDto.PageSize)
            .Take(pgnDto.PageSize)
            .ToListAsync();

        return new NewsPaginationDto
        {
            NewsItemDtos = items.Select(NewsMapper.ToDto).ToList(),
            CurrentPage = pgnDto.Page,
            TotalPage = totalPages,
            SortOrder = pgnDto.SortOrder,
        };
    }
}
