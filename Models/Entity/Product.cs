using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Thesis.Models.Entity
{
    [Table("tbl_Product")]
    public class Product : CommonEntity
    {
        public Product() 
        {
            this.ProductImages = new HashSet<ProductImages>();
            this.OrderDetail = new HashSet<OrderDetail>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 ký tự")]
        public string Title { get; set; }

		[StringLength(150)]
		public string? Alias {  get; set; }

        [StringLength(3000, ErrorMessage = "Không được vượt quá 3000 ký tự")]
        public string? Description { get; set; }

        [AllowHtml]
        public string? Detail { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price {  get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PriceSale { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public string ProductCode {  get; set; }

        public bool isHome { get; set; }

        public bool isSale { get; set; }

        public bool isFeature { get; set; }

        public bool isHot { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự")]
        public string? SeoTitle { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự")]
        public string? SeoDescription { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự")]
        public string? SeoKeywords { get; set; }

        public bool isActive { get; set; }

        public int ViewCount {  get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        [ForeignKey("Id")]
        public int ProductCategoriesId { get; set; }

        public virtual ProductCategories? ProductCategories { get; set; }

        public virtual ICollection<ProductImages> ProductImages { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
