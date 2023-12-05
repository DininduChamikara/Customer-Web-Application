using System.ComponentModel.DataAnnotations;

namespace TokenAuthDemo2.Models
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
