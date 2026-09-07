using Back.Common.Students.Commands.DeleteStudent;
using Back.Common.Students.Commands.RegisterStudent;
using Back.Common.Students.Commands.UpdateStudent;
using Back.Common.Students.Queries.GetAllStudents;
using Back.Common.Students.Queries.GetClassmates;
using Back.DTOs;

using MediatR;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ISender _mediator;

        public StudentsController(ISender mediator) => _mediator = mediator;



        [HttpGet]
        [ProducesResponseType(typeof(List<StudentDetailDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllStudentsQuery());
            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterStudentCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetClassmates), new { studentId = id }, new { Id = id });
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la petición.");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteStudentCommand(id));
            return NoContent();
        }


        [HttpGet("{studentId}/classmates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClassmates(Guid studentId)
        {
            var query = new GetClassmatesQuery(studentId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
