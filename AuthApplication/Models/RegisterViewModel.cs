using System.ComponentModel.DataAnnotations;

namespace AuthApplication.Models
{
    public class RegisterViewModel
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }
    }
}
