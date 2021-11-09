using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestBackend.Api.Extensions;
using RestBackend.Api.Validators;
using RestBackend.Api.Wrappers;
using RestBackend.Core.Resources;
using RestBackend.Core.Resources.Pagination;
using RestBackend.Core.Services;
using RestBackend.Core.Services.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestBackend.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ILogger<PropertiesController> _logger;

        private readonly IPropertyService _propertyService;
        private readonly IUriService _uriService;

        public PropertiesController(
            ILogger<PropertiesController> logger,
            IPropertyService ownerService,
            IUriService uriService)
        {
            _logger = logger;
            _propertyService = ownerService;
            _uriService = uriService;
        }

        /// <summary>
        /// Create a new Property
        /// </summary>
        /// <response code="200">Property created</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create(CreatePropertyResource propertyResource)
        {
            var validator = new SavePropertyResourceValidator();
            var validationResult = await validator.ValidateAsync(propertyResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var createdProperty = await _propertyService.Create(propertyResource);
            _logger.LogInformation("Property created");

            return Created($"{createdProperty.IdProperty}", createdProperty);
        }

        /// <summary>
        /// Update Property info
        /// </summary>
        /// <response code="200">Property updated</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPut("{idProperty}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int idProperty, PropertyResource propertyResource)
        {
            await _propertyService.Update(idProperty, propertyResource);
            _logger.LogInformation($"Property {idProperty} Updated ");

            return Ok();
        }

        /// <summary>
        /// Update Property price
        /// </summary>
        /// <response code="200">Property price updated</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPut("{idProperty}/price/{value}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePrice(int idProperty, decimal value)
        {
            await _propertyService.UpdatePrice(idProperty, value);
            _logger.LogInformation($"Property {idProperty} Price Updated ");

            return Ok();
        }

        /// <summary>
        /// Sell Property to new Owner
        /// </summary>
        /// <response code="200">Property owner updated</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPut("{idProperty}/sell/{value}/owner{idBuyer}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Sell(int idProperty, decimal value, int idBuyer)
        {
            await _propertyService.SellProperty(idProperty, value, idBuyer);
            _logger.LogInformation($"Property {idProperty} sold!");

            return Ok();
        }

        /// <summary>
        /// Get Property by Id
        /// </summary>
        /// <response code="200">Property</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpGet("{idProperty}")]
        [ProducesResponseType(typeof(PropertyResource), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> FindById(int idProperty)
        {
            var property = await _propertyService.GetById(idProperty);
            return Ok(new Response<PropertyResource>(property));
        }

        /// <summary>
        /// Get a Properties list filtered and paginated
        /// </summary>
        /// <response code="200">Properties paged list</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPost("paginated")]
        [ProducesResponseType(typeof(PagedResponse<PropertyResource>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter pagination, PropertyFilterResource filter)
        {
            var data = await _propertyService
                .GetAll(pagination, filter);

            return Ok(data.GetPaginatedResponse(_uriService, Request.Path.Value));
        }

        /// <summary>
        /// Get a Trace list by Property
        /// </summary>
        /// <response code="200">Property traces list</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpGet("{idProperty}/trace")]
        [ProducesResponseType(typeof(IEnumerable<PropertyTraceResource>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTrace(int idProperty)
        {
            var traces = await _propertyService.GetTrace(idProperty);
            return Ok(new Response<IEnumerable<PropertyTraceResource>>(traces));
        }

        /// <summary>
        /// Add image to property
        /// </summary>
        /// <response code="200">Image added</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpPut("{idProperty}/image")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddImage(int idProperty, IFormFile imageResource)
        {
            await _propertyService.AddImage(idProperty, imageResource);
            _logger.LogInformation($"Image added to Property {idProperty}.");

            return Ok();
        }

        /// <summary>
        /// Get images from property
        /// </summary>
        /// <response code="200">Image's list</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpGet("{idProperty}/image")]
        [ProducesResponseType(typeof(IEnumerable<PropertyImageResource>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetImages(int idProperty)
        {
            var images = await _propertyService.GetImages(idProperty);
            return Ok(new Response<IEnumerable<PropertyImageResource>>(images));
        }

        /// <summary>
        /// Remove image from property by Id
        /// </summary>
        /// <response code="200">Image removed</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpDelete("{idProperty}/image/{idImage}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteImages(int idProperty, int idImage)
        {
            await _propertyService.RemoveImage(idProperty, idImage);
            _logger.LogInformation($"Image removed to Property {idProperty}.");

            return Ok();
        }
    }
}
