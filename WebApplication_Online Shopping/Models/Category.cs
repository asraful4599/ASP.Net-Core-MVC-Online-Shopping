using System.ComponentModel.DataAnnotations;

namespace WebApplication_Online_Shopping.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]  
        public string CategoryName { get; set; }
    }
}
