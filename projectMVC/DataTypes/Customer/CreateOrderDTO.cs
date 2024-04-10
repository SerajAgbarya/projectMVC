using projectMVC.DataTypes.Shared;

namespace projectMVC.DataTypes.Customer
{
    public class CreateOrderDTO : NavBarDTO
    {
        public CartDTO? cartDTO { get; set; }
        public PaymentDTO payment {  get; set; }

        public ShippingAddressDTO? shippingAddress { get; set; }
        public bool isPickupOrder { get; set; }
    }
}
