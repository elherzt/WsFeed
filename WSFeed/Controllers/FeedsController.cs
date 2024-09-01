using DataLayer.Models;
using DataLayer.Repositories.FeedRepository;
using DataLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WSFeed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FeedsController : CustomController
    {

        private readonly IFeedRepository _feedRepository;

        public FeedsController(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Response>> Create(FeedDTOcreate feedDTO)
        {
            Response response = new Response();

            try
            {
                var handleInvalidModelState = HandleInvalidModelState();
                if (handleInvalidModelState.TypeOfResponse != TypeOfResponse.OK)
                {
                    return Ok(handleInvalidModelState);
                }

                var getUser = GetUserIdFromToken();
                if (getUser.TypeOfResponse != TypeOfResponse.OK)
                {
                    return Ok(getUser);
                }
                int userId = (int)getUser.Data;

                Feed feed = new Feed();
                feed.Name = feedDTO.Name;
                feed.UserId = userId;
                feed.Description = feedDTO.Description;
                feed.IsPrivate = feedDTO.IsPrivate;

                response = await _feedRepository.CreateAsync(feed);


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
