﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities;

[Table("tb_Product")]
public class Product : CommonAbstract
{
    public Product()
    {
        this.ProductImage = new HashSet<ProductImage>();
        this.OrderDetail = new HashSet<OrderDetail>();
    }
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [StringLength(250)]
    public string? Title { get; set; }
	[StringLength(250)]
    public string? Alias { get; set; }
	public string? Description { get; set; }
	[StringLength(50)]
    public string? ProductCode { get; set; }
    public decimal OriginalPrice { get; set; }
	public string? Detail { get; set; }
	[StringLength(250)]
    public string? Image { get; set; }
	public decimal Price { get; set; }
    public decimal? PriceSale { get; set; }
    public int Quantity { get; set; }

    public int ViewCount { get; set; }
    public bool IsHome { get; set; }
    public bool IsSale { get; set; }
    public bool IsHot { get; set; }
	public bool IsActive { get; set; }
	public bool IsFeature {  get; set; }
    public int ProductCategoryId { get; set; }
	[StringLength(250)]
    public string? SeoTitle { get; set; }
	[StringLength(500)]
	public string? SeoDescription { get; set; }
	[StringLength(250)]
	public string? SeoKeywords { get; set; }

    [ForeignKey(nameof(ProductCategoryId))]
    public virtual ProductCategory? ProductCategory { get; set; }
    public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    public virtual ICollection<ProductImage> ProductImage { get; set; }
}
