using News.Admin.DTO.CategoryDto;
using News.Admin.Models;

namespace News.Admin.NewsMapping;

public static class CategoryMapper
{
    public static CategoryDto ToDto(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };

    public static Category ToEntity(CategoryDto categoryDto) => new()
    {
        Id = categoryDto.Id,
        Name = categoryDto.Name
    };
}
