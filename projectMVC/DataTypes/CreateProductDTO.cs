using projectMVC.DataTypes.Shared;

namespace projectMVC.DataTypes
{
    public class CreateProductDTO: NavBarDTO
    {
        public int Id { get; } 
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string StockLocation { get; set; }
        public string brand { get; set; }

        public List<ProductAttributeDTO> Attributes { get; set; }

        public Dictionary<string, List<string>>? categoryAttributesDict { get; set; }
    }
}
