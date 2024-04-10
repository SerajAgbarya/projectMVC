namespace projectMVC.DataTypes.Customer
{


    public class PaymentDTO
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public double Amount { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
