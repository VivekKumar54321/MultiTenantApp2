using Application.DTOs;
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
    public class GetEmployeesListHandler : IRequestHandler<GetEmployeesListQuery, List<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetEmployeesListHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<List<EmployeeDto>> Handle(GetEmployeesListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Employees
                .Where(e => e.TenantId == _currentUser.TenantId)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Department = e.Department
                })
                .ToListAsync(cancellationToken);
        }
    }
}
