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
    [Required(ErrorMessage ="Tên khách hàng không để trống")]
    public string? CustomerName { get; set; }
    [Required(ErrorMessage = "Số điện thoại không để trống")]
    public string? Phone { get; set; }
    [Required(ErrorMessage = "Địa chỉ không để trống")]
    public string? Address { get; set; }
    public decimal TotalAmount { get; set; }
    public int Quantity { get; set; }
    public string? Email { get; set; }
    public int TypePayment { get; set; }

    public ICollection<OrderDetail>? OrderDetail { get; set; }
}
