namespace projectMVC.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserModel
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        public string pasWord { get; set; }


        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(55)]
        public string? Email { get; set; }

        public string? UserAddress { get; set; }

        // Navigation property
        public ICollection<UserPermissionModel> Permissions { get; set; }
    }

}
