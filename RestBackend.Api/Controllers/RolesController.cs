using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestBackend.Api.Wrappers;
using RestBackend.Core.Models.Auth;

namespace RestBackend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;

        public RolesController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <response code="200">Role's list</response>
        /// <response code="400">An error occurred</response>
        /// <response code="500">An unhandled  error occurred</response>
        [HttpGet()]
        [ProducesResponseType(typeof(List<Role>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(500)]
        public IActionResult GetAllRoles()
        {
            var dataResult = _roleManager.Roles.ToList();
            return Ok(new Response<List<Role>>(dataResult));
        }

    }
}
