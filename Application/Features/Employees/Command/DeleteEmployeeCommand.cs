﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employees.Command
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteEmployeeCommand(int id) => Id = id;
    }
}
