using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "product name can't exceed 50 characters")]
        public string Name { get; set; } = "";

        [Required]
       
        public decimal Price { get; set; }

        public string Description { get; set; } = "";

        public string? ImageFileName { get; set; }




    }
}
