using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models
{
    public class ProductAttributesModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

        // Navigation property for the associated product
        public ProductsModel Product { get; set; }
    }
}
