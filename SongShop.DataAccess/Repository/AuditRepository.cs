using SongShop.DataAccess.Data;
using SongShop.DataAccess.Repository.IRepository;
using SongShop.Models;

namespace DemoAudit.Repository
{
	public class AuditRepository : IAuditRepository
	{
		//private readonly IConfiguration _configuration;
		private readonly AuditDbContext _db;

		public AuditRepository(AuditDbContext db)
		{
			_db = db;
			//_configuration = configuration;
		}

		public void InsertAuditLogs(AuditModel objauditmodel)
		{
			_db.AuditModels.Add(objauditmodel);
			_db.SaveChanges();
			/*using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AuditDatabaseConnection"));
			try
			{
				con.Open();
				var para = new DynamicParameters();
				para.Add("@UserID", objauditmodel.UserId);
				para.Add("@ControllerName", objauditmodel.ControllerName);
				para.Add("@ActionName", objauditmodel.ActionName);

				con.Execute("Usp_InsertAuditLogs", para, null, 0, CommandType.StoredProcedure);
			}
			catch (Exception)
			{
				throw;
			}*/
		}
	}
}