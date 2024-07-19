using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
    [Table("tbl_Categories")]
    public class Categories : CommonEntity
    {
        public Categories() 
        {
            this.News = new HashSet<News>();
            this.Posts = new HashSet<Posts>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Không được để trống mục này")]
        [StringLength(150)]
        public string Title { get; set; }

		[StringLength(150)]
		public string? Alias { get; set; }

		[StringLength(3000, ErrorMessage = "Không được vượt quá 3000 ký tự")]
        public string? Description { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự")]
        public string? SeoTitle { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự")]
        public string? SeoDescription { get; set; }

        [StringLength(250, ErrorMessage = "Không được vượt quá 250 ký tự")]
        public string? SeoKeywords { get; set; }

        public int? Position { get; set; }

        public bool isActive { get; set; }

        public ICollection<News> News { get; set; }

        public ICollection<Posts> Posts { get; set; }
    }
}
