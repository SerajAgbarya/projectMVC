using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectMVC.Models
{
    public class ProductCategoryAttributesModel
    {

        [Key, Column(Order = 1)]
        public string CategoryName { get; set; }

        [Key, Column(Order = 2)]
        public string AttributeName { get; set; }

        // Navigation property for the associated product category
        public ProductCategoresModel Category { get; set; }

    }
}

