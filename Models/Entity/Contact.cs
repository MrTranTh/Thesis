using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
    [Table("tbl_Contact")]
    public class Contact: CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        [StringLength(100, ErrorMessage = "Không được vượt quá 100 ký tự")]
        public string YourName { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        [StringLength(100, ErrorMessage = "Không được vượt quá 100 ký tự")]
        public string YourEmail { get; set; }

        [StringLength(2000, ErrorMessage = "Không được vượt quá 2000 ký tự")]
        public string? Subject { get; set; }

        [StringLength(3000, ErrorMessage = "Không được vượt quá 3000 ký tự")]
        public string? Message { get; set; }

        public bool isRead { get; set; }
    }
}
