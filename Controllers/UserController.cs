using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;
using UserAPI.Data;
using UserAPI.Models;
using UserAPI.Utils;
using System.Text;
using System.Text.Json;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly Context _context;


        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _context = new Context();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(User model)
        {
            _logger.LogInformation("Create user posted: " + JsonSerializer.Serialize(model));

            // Try finding existing user
            var user = model.Id != Guid.Empty
                ? await _context.Users.SingleOrDefaultAsync(u => u.Id == model.Id)
                : await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email) ??
                    await _context.Users.FirstOrDefaultAsync(u => u.FirstName == model.FirstName && u.LastName== model.LastName);

            if(user != null)
            {
                _logger.LogInformation($"Create user conflict: user {model.Id} exists.");
                return BadRequest("Conflict: user already exists.");
            }

            if(model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }

            await _context.AddAsync(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), model);
        }
    }
}
