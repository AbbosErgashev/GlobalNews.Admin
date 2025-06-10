using System.ComponentModel.DataAnnotations;

namespace News.Admin.DTO.CategoryDto;

public class CategoryCreateDto
{
    [MaxLength(100)]
    public required string Name { get; set; }
}
