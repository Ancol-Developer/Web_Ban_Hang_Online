using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

[Table("tb_Subscribe")]
public class Subscribe
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [EmailAddress]
    [Required]
    public string? Email { get; set; }
    public DateTime? CreateDate { get; set; }
}
