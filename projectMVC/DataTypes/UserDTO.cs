using projectMVC.enums;

namespace projectMVC.DataTypes
{
    public class UserDTO
    {
        public int Id { get; }

       
        public string UserName { get; set; }
        public string PasWord { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserAddress { get; set; }
        public string? PermisonType { get; set; }
    }
}
