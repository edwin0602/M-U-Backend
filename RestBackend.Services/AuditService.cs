using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestBackend.Core;
using RestBackend.Core.Models.Security;
using RestBackend.Core.Services.Infrastructure;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestBackend.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogger<AuditService> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUnitOfWork _unitOfWork;

        public AuditService(
                  ILogger<AuditService> logger,
                  IHttpContextAccessor httpContextAccessor,
                  IUnitOfWork unitOfWork)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Generate Audit entry with Identity user if exist
        /// </summary>
        /// <param name="action">Action executed</param>
        /// <param name="target">Id of entity affected</param>
        /// <param name="entity">Entity instance</param>
        public async Task Add(string action, string target, object entity)
        {
            try
            {
                var userId = "Anonymous";
                var patch = _httpContextAccessor.HttpContext.Request.Path;
                if (_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated != default)
                    userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                await Add(new Audit
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    Resource = patch,
                    Action = action,
                    Target = target,
                    Entity = entity.GetType().Name
                });
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot save Audit information.");
            }
        }


        /// <summary>
        /// Add Audit entry
        /// </summary>
        /// <param name="entry"></param>
        public async Task Add(Audit entry)
        {
            try
            {
                await _unitOfWork.Audit.AddAsync(entry);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot save Audit information.");
            }
        }
    }
}
