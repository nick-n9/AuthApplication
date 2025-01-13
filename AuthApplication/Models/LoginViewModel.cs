using System.ComponentModel.DataAnnotations;

namespace AuthApplication.Models
{
    public class LoginViewModel
    {
        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }
    }
}
