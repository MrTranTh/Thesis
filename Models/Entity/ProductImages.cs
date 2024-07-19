using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
    //Quản lý các hình ảnh sản phẩm
    [Table("tbl_ProductImages")]
    public class ProductImages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[ForeignKey("Id")]
		public int ProductId { get; set; }

        public string? Image { get; set; }

        public bool isDefault { get; set; }

        public virtual Product? Product { get; set; }
    }
}
