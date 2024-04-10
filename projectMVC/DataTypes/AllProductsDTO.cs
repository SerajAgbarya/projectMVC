using projectMVC.DataTypes;
using projectMVC.DataTypes.Customer;
using projectMVC.DataTypes.Shared;
using projectMVC.Models;

namespace projectMVC.DataTypes
{
    public class AllProductsDTO : NavBarDTO
    {
        public List<ProductDTO> products { get; set; }
        public Dictionary<string, List<string>>? categoryAttributesDict { get; set; }

        

    }
}
