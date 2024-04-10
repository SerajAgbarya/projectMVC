using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models
{
    public class ProductsModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary Key
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }

        public string brand { get; set; }
        public string StockLocation { get; set; }

    }
}
