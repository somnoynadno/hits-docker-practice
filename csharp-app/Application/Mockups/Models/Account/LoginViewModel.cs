using System.ComponentModel.DataAnnotations;

namespace Mockups.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

}
