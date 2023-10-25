using Microsoft.EntityFrameworkCore;
using SongShop.Models;

namespace SongShop.DataAccess.Data
{
	public class AuditDbContext : DbContext
	{
		public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options)
		{

		}

		public DbSet<AuditModel> AuditModels { get; set; }
	}
}
