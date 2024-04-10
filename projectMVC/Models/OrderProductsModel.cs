namespace projectMVC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
     

    public class OrderProductsModel
    {
        [Key, Column(Order = 1)]
        public int orderId { get; set; }

        [Key, Column(Order = 2)]
        public int productId { get; set; }
        public int? quanity { get; set; }
        public decimal? actaulPrice { get; set; }


    }

}
