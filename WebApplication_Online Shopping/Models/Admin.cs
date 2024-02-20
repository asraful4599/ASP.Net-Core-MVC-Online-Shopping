using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WebApplication_Online_Shopping.Models
{
    public class Admin
    {
        [Key]
        public string UserName { get; set; }
        [Required]  
        public string Password { get; set; }
    }
}
