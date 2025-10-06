using System.ComponentModel.DataAnnotations;

namespace Smotrilka_Web.Models
{
    public class LinkRequest
    {
        [Required(ErrorMessage = "Тип обязателен")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Тип обязателен")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название не должно быть длиннее 100 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Тип обязателен")]
        [StringLength(50, ErrorMessage = "Тип не должен быть длиннее 50 символов")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Ссылка обязательна")]
        public string Link { get; set; }
    }
}