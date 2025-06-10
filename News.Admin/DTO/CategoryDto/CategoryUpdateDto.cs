using System.ComponentModel.DataAnnotations;

namespace News.Admin.DTO.CategoryDto;

public class CategoryUpdateDto
{
    public int Id { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
}
