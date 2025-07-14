using Application.Features.Employees.Command;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employees.Queries
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public UpdateEmployeeHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == request.Id && e.TenantId == _currentUser.TenantId, cancellationToken);

            if (employee == null)
                return false;

            employee.Name = request.Name;
            employee.Department = request.Department;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
