using System;
using System.Security.Claims;
using DemoAudit.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SongShop.DataAccess.Repository;
using SongShop.DataAccess.Repository.IRepository;
using SongShop.Models;

namespace DemoAudit.Filters
{
	public class AuditFilterAttribute : ActionFilterAttribute
	{
		private readonly IAuditRepository _auditRepository;
		private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _contextAccessor;
		public AuditFilterAttribute(IAuditRepository auditRepository, IUserRepository userRepository, IHttpContextAccessor contextAccessor)
		{
			_auditRepository = auditRepository;
			_userRepository = userRepository;
			_contextAccessor = contextAccessor;
		}
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var objaudit = new AuditModel(); 
			var controllerName = ((ControllerBase)filterContext.Controller)
				.ControllerContext.ActionDescriptor.ControllerName;
			var actionName = ((ControllerBase)filterContext.Controller)
				.ControllerContext.ActionDescriptor.ActionName;

			string userEmail = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
			if(userEmail == null)
			{
				objaudit.UserId = 0;
			}
			else
			{
				User userFromDb = _userRepository.Get(userEmail);
				objaudit.UserId = userFromDb.Id;
			}

			objaudit.ControllerName = controllerName;
			objaudit.ActionName = actionName;

			objaudit.CurrentDatetime = DateTime.Now;

			_auditRepository.InsertAuditLogs(objaudit);
		}
	}
}