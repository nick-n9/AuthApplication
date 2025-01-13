using System.ComponentModel.DataAnnotations;

namespace AuthApplication.Models
{
    public class EditUserViewModel
    {
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
