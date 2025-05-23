using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAppApi.Data;
using ToDoAppApi.Models;

namespace ToDoAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenGenerator _jwtGenerator;
        private readonly AppDbContext _context;

        public AuthController(JwtTokenGenerator jwtGenerator, AppDbContext context)
        {
            _jwtGenerator = jwtGenerator;
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var data = await _context.Users.FirstOrDefaultAsync(x => x.Name == loginRequest.UserName && x.Password == loginRequest.Password);
            if (data == null) 
                return Unauthorized();

            var jwtToken = _jwtGenerator.generateToken(data);
            return Ok(jwtToken);
        }
    }
}
