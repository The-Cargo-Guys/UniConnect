using Microsoft.AspNetCore.Mvc;
using MyAspNetVueApp.Data;
using UniHackPrototype.Models;

namespace MyAspNetVueApp.Controllers
{
    [ApiController]
    [Route("api/users")]  
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] string user)
        {
            _context.Users.Add(new User { Name = user});
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetUsers), new { id = new User { Name = user }.Id }, user);
        }
    }
}
