using SongShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongShop.DataAccess.Repository.IRepository
{
	public interface IAuditRepository
	{
		void InsertAuditLogs(AuditModel objauditmodel);
	}
}
