﻿using System.ComponentModel.DataAnnotations;

namespace News.Admin.Models;

public class NewsItem
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? MediaUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
