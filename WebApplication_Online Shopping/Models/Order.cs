using System.ComponentModel.DataAnnotations;

namespace WebApplication_Online_Shopping.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
       
        [Required]
        public string EmailId { get; set; }
        [Required]  
        public string FirstName { get; set; }
        [Required]  
        public string LastName { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public int PinNo { get; set; }  
        [Required]  
        public string PhoneNo { get; set; }
        [Required]
        public string ProductNames { get; set; }
        [Required]
        public float TotalAmount {  get; set; }   

        [Required]  
        public DateTime OrderDate { get; set; }
    }
}
