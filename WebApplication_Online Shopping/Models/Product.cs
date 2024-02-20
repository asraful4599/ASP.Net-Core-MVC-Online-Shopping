using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication_Online_Shopping.Models
{
    public class Product
    {
        [Key] 
        public int ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name ="Description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name ="Price")]
        public float ProductPrice { get; set;}
        [Required]  
        public int CategoryId { get; set; }
        [Required]
        [Display(Name ="Picture")]
        public string ProductPic {  get; set; }
        [NotMapped]
        public IFormFile Picture {  get; set; } 

        [NotMapped]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set;}
    }
}
