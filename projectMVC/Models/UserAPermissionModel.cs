using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models
{
    public class UserAPermission
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(50)]
        public string PermissionType { get; set; }

        [ForeignKey("UserId")]
        public virtual UserA User { get; set; }
    }
}
