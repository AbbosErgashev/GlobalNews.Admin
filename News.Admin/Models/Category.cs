﻿using System.ComponentModel.DataAnnotations;

namespace News.Admin.Models;

public class Category
{
    public int Id { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
}
