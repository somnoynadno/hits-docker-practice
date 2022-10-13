using System.ComponentModel.DataAnnotations;

namespace Mockups.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email обязателен для заполнения")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна для заполнения")]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "ФИО обязательно для заполнения")]
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Пароль обязателен для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

}
