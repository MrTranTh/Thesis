using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Thesis.Models.Entity
{
    [Table("tbl_Posts")]
    public class Posts : CommonEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(150, ErrorMessage = "Không được vượt quá 150 ký tự")]
        [Required(ErrorMessage = "Không được để trống mục này")]
        public string Title { get; set; }

		[StringLength(150)]
		public string? Alias { get; set; }

		public string? Description { get; set; }

        [AllowHtml]
        public string? Detail { get; set; }

        public string? Image { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự")]
        public string? SeoTitle { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự")]
        public string? SeoDescription { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự")]
        public string? SeoKeywords { get; set; }

        public bool isActive { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
		[ForeignKey("Id")]
		public int CategoriesId { get; set; }

        public virtual Categories? Categories { get; set; }
    }
}
