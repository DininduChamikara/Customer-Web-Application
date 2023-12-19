using System.ComponentModel.DataAnnotations;

namespace Fidenz.Dashboard.API.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string RedirectUrl { get; set; }
    }
}
