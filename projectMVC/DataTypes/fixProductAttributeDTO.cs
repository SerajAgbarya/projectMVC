namespace projectMVC.DataTypes
{
    public class fixProductAttributeDTO
    {

        public int Id { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string StockLocation { get; set; }
    }
}
