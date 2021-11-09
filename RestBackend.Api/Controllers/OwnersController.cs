using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestBackend.Api.Extensions;
using RestBackend.Api.Validators;
using RestBackend.Api.Wrappers;
using RestBackend.Core.Resources;
using RestBackend.Core.Resources.Pagination;
using RestBackend.Core.Services;
using RestBackend.Core.Services.Infrastructure;
using System.Threading.Tasks;

namespace RestBackend.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {

        private readonly IOwnerService _ownerService;
        private readonly IUriService _uriService;

        private readonly ILogger<OwnersController> _logger;

        public OwnersController(
            ILogger<OwnersController> logger,
            IOwnerService ownerService,
            IUriService uriService)
        {
            _logger = logger;
            _ownerService = ownerService;
            _uriService = uriService;
        }

        /// <summary>
        /// Create a new Owner
        /// </summary>
        /// <response code="200">Owner created</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create(CreateOwnerResource ownerResource)
        {
            var validator = new SaveOwnersResourceValidator();
            var validationResult = await validator.ValidateAsync(ownerResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var createdOwner = await _ownerService.Create(ownerResource);

            _logger.LogInformation("Owner created.");

            return Created($"{createdOwner.IdOwner}", createdOwner);
        }

        /// <summary>
        /// Update Owner info
        /// </summary>
        /// <response code="200">Owner updated</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPut("{idOwner}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int idOwner, OwnerResource ownerResource)
        {
            await _ownerService.Update(idOwner, ownerResource);
            _logger.LogInformation("Owner Updated.");

            return Ok();
        }

        /// <summary>
        /// Get an Owner by Id
        /// </summary>
        /// <param name="idOwner"></param>
        /// <response code = "200">Owner</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpGet("{idOwner}")]
        [ProducesResponseType(typeof(OwnerResource), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> FindById(int idOwner)
        {
            var owner = await _ownerService.GetById(idOwner);
            return Ok(new Response<OwnerResource>(owner));
        }

        /// <summary>
        /// Get an Owners list by filters and paginated
        /// </summary>
        /// <response code = "200">Owner paged list</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpGet()]
        [ProducesResponseType(typeof(PagedResponse<OwnerResource>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var data = await _ownerService
                .GetAll(filter);

            return Ok(data.GetPaginatedResponse(_uriService, Request.Path.Value));
        }

        /// <summary>
        /// Get a Property list by Owner Id
        /// </summary>
        /// <response code = "200">Properties paged list</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpGet("{idOwner}/properties")]
        [ProducesResponseType(typeof(PagedResponse<PropertyResource>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPropertiesByOwner(int idOwner, [FromQuery] PaginationFilter filter)
        {
            var properties = await _ownerService
                .GetPropertiesByOwner(filter, idOwner);

            return Ok(properties.GetPaginatedResponse(_uriService, Request.Path.Value));
        }


    }
}
