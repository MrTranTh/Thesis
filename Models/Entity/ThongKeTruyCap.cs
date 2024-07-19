using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models.Entity
{
	[Table("tbl_ThongKeTruyCap")]
	public class ThongKeTruyCap
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTime ThoiGian {  get; set; }

		public long SoLuotTruyCap { get; set; }


	}
}
