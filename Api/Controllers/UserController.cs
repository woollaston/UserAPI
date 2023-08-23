using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text.Json;
using UserAPI.Interfaces;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IDbContext _context;

        public UserController(ILogger<UserController> logger, IDbContext context)
        {
            _logger = logger;
            _context = context; 
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(User model)
        {
            _logger.LogInformation("Create user posted: " + JsonSerializer.Serialize(model));

            if(model == null || model.RequiredFieldsMissing)
            {
                _logger.LogInformation($"Create user fields missing: user {model?.Id}.");
                return BadRequest("One or more required fields missing.");
            }

            // Try finding existing user
            var user = model.Id != Guid.Empty
                ? await _context.Users.SingleOrDefaultAsync(u => u.Id == model.Id)
                : await _context.Users.FirstOrDefaultAsync(u => u.FirstName == model.FirstName && u.LastName == model.LastName);

            if(user != null)
            {
                _logger.LogInformation($"Create user conflict: user {model.Id} exists.");
                return BadRequest("Conflict: user already exists.");
            }

            if(model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }

            await _context.Save(model);

            return CreatedAtAction(nameof(Create), model);
        }
    }
}
