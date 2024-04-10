using projectMVC.DataTypes;
using projectMVC.Models;

namespace projectMVC.DataTypes
{
    public class ProductDTO
    {
        public int? Id { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public int? StockQuantity { get; set; }
        public decimal? Price { get; set; }
        public string? StockLocation { get; set; }
        
        public string? brand { get; set; }

        public int? requiredQuntity { get; set; }

        public decimal? discountPrice { get; set; }
        public Dictionary<string, string>? AttributesDict { get; set; } 

        

    }
}
