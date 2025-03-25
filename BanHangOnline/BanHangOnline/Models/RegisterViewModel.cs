using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BanHangOnline.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name can't be blank")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already is use")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Phone can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain nubers only")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password is not match")]
        public string? ConfirmPassword { get; set; }
        public string? Role { get; set; }
    }
}
