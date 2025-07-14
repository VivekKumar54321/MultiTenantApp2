using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100, ErrorMessage = "Tenant name must be under 100 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Identifier must be under 50 characters.")]
        public string Identifier { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
