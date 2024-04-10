using projectMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectMVC.DataTypes
{
    public class UserPerDTO
    {
            public int Id { get; set; }

        // Foreign key property
        [ForeignKey("User")]
        public int UserId { get; set; }

            public string PermissionType { get; set; }
            public UserPerDTO User { get; set; }
    }
}
