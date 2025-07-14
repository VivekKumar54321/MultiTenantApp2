using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employees.Command
{
    public class UpdateEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Department { get; set; }
    }
}
