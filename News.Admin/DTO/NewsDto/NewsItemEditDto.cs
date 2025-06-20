﻿using System.ComponentModel.DataAnnotations;

namespace News.Admin.DTO.NewsDto;

public class NewsItemEditDto
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public required string Title { get; set; }
    public required string Description { get; set; }
    public IFormFile? NewMediaFile { get; set; }
    public string? ExistingMediaUrl { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int CategoryId { get; set; }
}
