using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Mockups.Models.Account
{
    public class EditUserDataViewModel
    {

        [Required(ErrorMessage = "Дата рождения обязательна для заполнения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "ФИО обязательно для заполнения")]
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }
    }
}
