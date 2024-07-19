using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
    [Table("tbl_SystemSettings")]
    public class SystemSettings
    {
        [Key]
        [StringLength(50, ErrorMessage = "Không được vượt quá 50 ký tự")]
        public string SystemKey { get; set; }

        [StringLength(2000)]
        public string? SystemValue { get; set; }

        [StringLength(3000)]
        public string? SystemDescription{ get; set; }
    }
}
