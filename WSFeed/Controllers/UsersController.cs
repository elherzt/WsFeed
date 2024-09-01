using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WSFeed.Common;

namespace WSFeed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ActionResult<Response>> Register(UserDto userDto)
        {

            Response response = new Response(TypeOfResponse.OK);

            try
            {

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return Ok(response);

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    ResponseType = TypeOfResponse.FailedResponse,
                    Message = "Invalid data provided.",
                    Data = null
                });
            }

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = _passwordHasher.HashPassword(null, userDto.Password), // Cifrar contraseña
                DateRegistered = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new Response
            {
                ResponseType = TypeOfResponse.OK,
                Message = "User registered successfully.",
                Data = user
            });
        }
    }
}
