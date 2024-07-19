using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
    [Table("tbl_OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
		[ForeignKey("Id")]
		public int OrderId { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
		[ForeignKey("Id")]
		public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public int Quantity { get; set;}

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
