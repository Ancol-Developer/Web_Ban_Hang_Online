using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

[Table("tb_Order")]
public class Order : CommonAbstract
{
    public Order()
    {
        this.OrderDetail = new List<OrderDetail>();
    }
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string? Code { get; set; }
    [Required]
    public string? CustomerName { get; set; }
    [Required]
    public string? Phone { get; set; }
    [Required]
    public string? Address { get; set; }
    public decimal TotalAmount { get; set; }
    public int Quantity { get; set; }

    public ICollection<OrderDetail>? OrderDetail { get; set; }
}
