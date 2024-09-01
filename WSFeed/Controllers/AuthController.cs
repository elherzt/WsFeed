using DataLayer.Models;
using DataLayer.Repositories.UserRepository;
using DataLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using WSFeed.Security;

namespace WSFeed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : CustomController
    {

        private readonly IUserRepository _userRepository;
        private readonly IJWTGenerator _jWTGenerator;

        public AuthController(IUserRepository userRepository, IJWTGenerator jWTGenerator)
        {
            _userRepository = userRepository;
            _jWTGenerator = jWTGenerator;
        }

        [HttpPost("GetToken")]
        public async Task<ActionResult<Response>> GetToken(UserDTOLogin userDTO)
        {
            Response response = new Response(TypeOfResponse.OK, "Token Generated");
            try
            {
                Response handleInvalidModelState = HandleInvalidModelState();
                if (handleInvalidModelState.TypeOfResponse != TypeOfResponse.OK)
                {
                    return Ok(handleInvalidModelState);
                }

                User user = new User
                {
                    Mail = userDTO.Mail,
                    Password = userDTO.Password
                };
                var existingUser = await _userRepository.LoginAsync(user);
                if (response.TypeOfResponse == TypeOfResponse.OK)
                {
                    var loggedUser = (UserDTOResponse)existingUser.Data;
                    var token = _jWTGenerator.GenerateToken(loggedUser.Id.ToString());
                    response.Data = token;
                }
                else
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Invalid credentials";

                }
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
