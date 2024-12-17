using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

[Table("tb_Contact")]
public class Contact : CommonAbstract
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be blank")]
    [StringLength(150, ErrorMessage ="Length cant be higher than 150")]
    public string? Name { get; set; }

    [StringLength(150, ErrorMessage = "Length cant be higher than 150")]
    public string? Email { get; set; }
    public string? Website { get; set; }

    [StringLength(4000)]
    public string? Message { get; set; }
    public bool IsRead { get; set; }
}
