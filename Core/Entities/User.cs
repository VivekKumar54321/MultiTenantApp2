using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
