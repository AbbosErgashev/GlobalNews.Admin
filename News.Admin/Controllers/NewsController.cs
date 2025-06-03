using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Admin.Data;
using News.Admin.DTO;
using News.Admin.Models;
using News.Admin.NewsMapping;

namespace News.Admin.Controllers;

public class NewsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public NewsController(AppDbContext context, IWebHostEnvironment env)
    {
        _env = env;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var getAll = await _context.NewsItems.OrderByDescending(x => x.CreatedAt).ToListAsync();

        var entity = getAll.Select(NewsMapper.ToDto).ToList();
        return View(entity);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsItemCreateDto item)
    {
        if (!ModelState.IsValid) return View(item);

        if (item.MediaFile == null || item.MediaFile.Length == 0)
        {
            ModelState.AddModelError("MediaFile", "File did not check.");
            return View(item);
        }

        var mediaUrl = await SaveMediaFileAsync(item.MediaFile);

        var entity = new NewsItem
        {
            Id = Guid.NewGuid(),
            Title = item.Title,
            Description = item.Description,
            MediaUrl = mediaUrl!,
            CreatedAt = DateTime.UtcNow
        };

        _context.NewsItems.Add(entity);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        var entity = await _context.NewsItems.FindAsync(id);
        if (entity is null) return NotFound();

        var dto = new NewsItemEditDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ExistingMediaUrl = entity.MediaUrl
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NewsItemEditDto dto)
    {
        if (id != dto.Id) return NotFound();
        if (!ModelState.IsValid) return View(dto);

        var entity = await _context.NewsItems.FindAsync(id);

        if (entity is null) return NotFound();

        if (dto.NewMediaFile != null)
        {
            var newMediaUrl = await SaveMediaFileAsync(dto.NewMediaFile);
            entity.MediaUrl = newMediaUrl;
        }

        _context.Update(entity);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var news = await _context.NewsItems.FindAsync(id);
        if (news == null) return NotFound();

        var dto = NewsMapper.ToDto(news);
        return View(dto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var items = await _context.NewsItems.FindAsync(id);
        if (items == null) return NotFound();

        _context.NewsItems.Remove(items);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet, ActionName("Get")]
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id is null) return NotFound();
        var entity = await _context.NewsItems.FindAsync(id);

        if (entity == null) return NotFound();

        var dto = NewsMapper.ToDto(entity);
        return View(dto);
    }

    private async Task<string?> SaveMediaFileAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0) return null;

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
}
