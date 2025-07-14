using Application.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid TenantId { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
           
            var user = httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {

               var  userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? user.FindFirst(ClaimTypes.Name)?.Value;

              var   email = user.FindFirstValue(ClaimTypes.Email);

                var tenantClaim = user.FindFirst("TenantId");
                if (tenantClaim != null && Guid.TryParse(tenantClaim.Value, out var tenantId))
                {
                    TenantId = tenantId;
                }
            }
        }
    }
}
