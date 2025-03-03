using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TENBookingSystem.Application.Commands.Uploads;
using TENBookingSystem.Application.Core;
using TENBookingSystem.DTO.Uploads;

namespace TENBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : BaseApiController
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload/members")]        
        public async Task<ActionResult<Result<UplaodFileResult>>> UploadMembers([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var command = new UploadMemberCommand(file);
            return await _mediator.Send(command);
        }

        [HttpPost("upload/inventory")]
        public async Task<ActionResult<Result<UplaodFileResult>>> UploadInventory([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var command = new UploadInventoryCommand(file);
            return await _mediator.Send(command);
        }

    }
}
