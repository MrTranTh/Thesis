using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
    [Table("tbl_Order")]
    public class Order : CommonEntity
    {
        public Order() 
        {
            this.OrderDetail = new HashSet<OrderDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public string CustomerName {  get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public string Address { get; set; }

		public string? Email { get; set; }

        public string? Message { get; set; }

		public int TypePayment { get; set; }

		[Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Không được để trống mục này")]
        public int Quantity { get; set; }

        

        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
