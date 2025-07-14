using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Employee 
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Department must be under 100 characters.")]
        public string Department { get; set; }

        [Required]
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
