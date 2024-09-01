using DataLayer;
using DataLayer.Models;
using DataLayer.Repositories.UserRepository;
using DataLayer.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WSFeed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public UsersController(AppDbContext context, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(UserDTORegister userDTO)
        {
            Response response = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage)
                                   .ToList();
                    var errorMessage = string.Join(", ", errors);
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = errorMessage;
                    return Ok(response);

                }
                User user = new User
                {
                    Mail = userDTO.Mail,
                    Name = userDTO.Name,
                    Password = userDTO.Password
                };
                response = await _userRepository.AddUserAsync(user);
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
                
            }
            
            return Ok(response);   
        }
    }
}
