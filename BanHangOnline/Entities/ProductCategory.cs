using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities;

[Table("tb_ProductCategory")]
public class ProductCategory : CommonAbstract
{
    public ProductCategory()
    {
        Products = new List<Product>();
    }
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [StringLength(150)]
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Icon {  get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }
    public string? SeoKeywords { get; set; }

    public ICollection<Product>? Products { get; set; }
}
