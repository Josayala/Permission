using MediatR;
using Microsoft.AspNetCore.Mvc;
using Permission.Application.CommandService.Commands;
using Permission.Application.QueryService.Queries;
using Permission.Infrastructure.Dtos;
using Serilog;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Permission.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator mediator;
        public PermissionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all the Permissions
        /// </summary>
        /// <returns>Returns new all Permissions </returns>
        [HttpGet()]
        [ProducesResponseType(typeof(List<PermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPermissions()
        {
            Log.Information("Getting all permissions", nameof(GetPermissionsQuery));
            var query = new GetPermissionsQuery();
            var result = await mediator.Send(query);
            return Ok(result);
        }

   
         /// <summary>
        /// Creates a new permission.
        /// </summary>
        /// <param name="createPermissionCommand">The command to create the permission.</param>
        /// <returns>Returns the created permission.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionCommand createPermissionCommand)
        {
            Log.Information("Creating new permission", nameof(CreatePermissionCommand));
            var result = await mediator.Send(createPermissionCommand);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePermission(int id,[FromBody] UpdatePermissionCommand updatePermissionCommand)
        {
            Log.Information("Updating permission", nameof(UpdatePermissionCommand));
            if (id != updatePermissionCommand.Id)
            {
                return BadRequest();
            }
            var result = await mediator.Send(updatePermissionCommand);
            return Ok(result);
        }
    }
}
