using System.ComponentModel.DataAnnotations;

namespace BanHangOnline.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng không để trống")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không để trống")]
        public string? Address { get; set; }
        public int TypePayment {  get; set; }
    }
}
