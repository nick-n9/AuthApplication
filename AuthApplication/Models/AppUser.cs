using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthApplication.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
