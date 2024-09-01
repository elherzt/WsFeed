using DataLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WSFeed.Controllers
{
    public class CustomController : ControllerBase
    {
        protected Response GetUserIdFromToken()
        {
            Response response = new Response(TypeOfResponse.OK);
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (int.TryParse(userIdClaim, out var userId))
                {
                    response.Data = userId;
                }
                else
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Invalid User";
                }
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
           return response;
        }

        protected Response HandleInvalidModelState()
        {
            Response response = new Response(TypeOfResponse.OK);
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
                }
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;

            }
            return response;
        }


    }
}
