
using API.Exceptions;
using Application.Features.Employees.Command;
using Application.Features.Employees.Queries;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(command.Name))
                    throw new ApiException("Name is required", 400, "Validation failed", null);

                var id = await _mediator.Send(command);
                return Ok(new { id });
            }
            catch (Exception ex)
            {

                throw new ApiException("Failed to create employee", 500, ex.Message, null, ex);
            }

        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var employees = await _mediator.Send(new GetEmployeesListQuery());
                return Ok(employees);
            }
            catch (Exception ex)
            {
                throw new ApiException("Failed to fetch employee list", 500, ex.Message, null, ex);
            }
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
                if (employee == null)
                    throw new ApiException($"Employee with ID {id} not found.", 404, "Not Found", null);

                return Ok(employee);
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error fetching employee with ID {id}", 500, ex.Message, null, ex);
            }
        }

        [HttpPut("{id}")] 
        public async Task<IActionResult> Update(int id, UpdateEmployeeCommand cmd)
        {
            try
            {
                if (id != cmd.Id && cmd.Id != 0)
                    throw new ApiException("Mismatched employee ID in route and body.", 400, "Bad Request", null);

                cmd.Id = id;

                var updatedId = await _mediator.Send(cmd);
                return Ok(new { id = updatedId });
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error updating employee with ID {id}", 500, ex.Message, null, ex);
            }
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedId = await _mediator.Send(new DeleteEmployeeCommand(id));
                return Ok(new { id = deletedId });
            }
            catch (Exception ex)
            {
                throw new ApiException($"Error deleting employee with ID {id}", 500, ex.Message, null, ex);
            }
        }
    }
}
