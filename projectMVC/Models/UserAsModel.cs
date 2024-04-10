using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models
{
    public class UserA
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string UserAddress { get; set; }
    }
}
