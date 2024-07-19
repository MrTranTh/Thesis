using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
    [Table("tbl_Advertisement")]
    public class Advertisement : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Không được để trống mục này")]
        [StringLength(150, ErrorMessage ="Không được vượt quá 150 ký tự")]
        public string Title { get; set; }

		[StringLength(150)]
		public string? Alias { get; set; }

		[StringLength(3000, ErrorMessage = "Không được vượt quá 30000 ký tự")]
        public string? Description { get; set; }

        public string? Image {  get; set; }

        public string? Link { get; set; }

        public int? Type { get; set; }

    }
}
