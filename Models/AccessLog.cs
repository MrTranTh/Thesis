using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis.Models
{
    [Table("tbl_AccessLog")]
    public class AccessLog
    {
        public int Id { get; set; }
        public DateTime AccessTime { get; set; }
    }
}
