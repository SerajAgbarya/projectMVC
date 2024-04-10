using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace projectMVC.Models.Ref
{
    public class RefDataModel
    {
        [Key, Column(Order = 1)]
        public string RefName { get; set; }
        [Key, Column(Order = 2)]
        public string RefValue { get; set; }
    }
}
