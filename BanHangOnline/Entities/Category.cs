using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

[Table("tb_Category")]
public class Category : CommonAbstract
{
    public Category()
    {
        this.News = new List<News>();
        this.Posts = new List<Posts>();
    }
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set;}
    public string? SeoKeywords { get; set; }
    public int Position { get; set; }

    public ICollection<News>? News { get; set; }
    public ICollection<Posts>? Posts { get; set; }
}
