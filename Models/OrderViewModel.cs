using System.ComponentModel.DataAnnotations;

namespace Thesis.Models
{
	public class OrderViewModel
	{
		[Required(ErrorMessage = "Không được để trống mục này")]
		public string CustomerName { get; set; }

		[Required(ErrorMessage = "Không được để trống mục này")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Không được để trống mục này")]
		public string Address { get; set; }

		public string? Email { get; set; }

		public string? Message { get; set; }

		public int TypePayment { get; set; }
	}
}
