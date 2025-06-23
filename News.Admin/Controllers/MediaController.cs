using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace News.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
    [HttpGet("{filename}")]
    public IActionResult GetFile(string filename)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "E:\\Developer Files\\.NET\\C# Projects\\News\\News.Admin\\News.Admin\\wwwroot\\uploads\\", filename);
        if (!System.IO.File.Exists(path))
            return NotFound();

        var contentType = "application/octet-stream";
        new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);
        var bytes = System.IO.File.ReadAllBytes(path);
        return File(bytes, contentType);
    }
}
