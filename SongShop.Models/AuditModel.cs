using System.ComponentModel.DataAnnotations;

namespace SongShop.Models
{
	public class AuditModel
	{
		[Key]
		public int AuditId { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
		public int UserId { get; set; }
		public DateTime CurrentDatetime { get; set; }
	}
}
