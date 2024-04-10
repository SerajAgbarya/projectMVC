namespace projectMVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProductCategoresModel
    {
        [Key]
        [StringLength(255)]
        public string CategoryName { get; set; }
    }

}
