﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

[Table("tb_Posts")]
public class Posts : CommonAbstract
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [StringLength(150)]
    public string? Title { get; set; }
    public string? Alias { get; set; }
    public string? Description { get; set; }
    public string? Detail { get; set; }
    public string? Image { get; set; }
    public int CategoryId { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public virtual Category? Category { get; set; }
}
