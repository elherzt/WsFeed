using DataLayer.Models;
using DataLayer.Repositories.FeedRepository;
using DataLayer.Repositories.TopicRepository;
using DataLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSFeed.Common;

namespace WSFeed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FeedsController : CustomController
    {

        private readonly IFeedRepository _feedRepository;
        private readonly INewsCalls _newsCalls;

        public FeedsController(IFeedRepository feedRepository, INewsCalls newsCalls)
        {
            _feedRepository = feedRepository;
            _newsCalls = newsCalls;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Response>> Create(FeedDTOcreate feedDTO)
        {
            Response response = new Response();

            try
            {
                feedDTO.Name = feedDTO.Name.Trim();
                feedDTO.Description = feedDTO.Description.Trim();
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

        [HttpGet("GetPublicFeeds")]
        public async Task<ActionResult<Response>> GetPublicFeeds(int PageNumber = 1, int PageSize = 10)
        {
            Response response = new Response();

            try
            {
               
                var getUser = GetUserIdFromToken();
                if (getUser.TypeOfResponse != TypeOfResponse.OK)
                {
                    return Ok(getUser);
                }
                int userId = (int)getUser.Data;

                response = await _feedRepository.GetPublicAsync(userId, PageNumber, PageSize);

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("AddTopic")]
        public async Task<ActionResult<Response>> AddTopic(TopicDTOCreate topicDTO)
        {
            Response response = new Response();

            try
            {
                topicDTO.Name = topicDTO.Name.Trim();
                topicDTO.Description = topicDTO.Description.Trim();
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

                response = await _feedRepository.AddTopicAsync(topicDTO, userId);

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }



        [HttpGet("GetUserFeeds")]
        public async Task<ActionResult<Response>> GetUserFeeds(int PageNumber = 1, int PageSize = 10)
        {
            Response response = new Response();

            try
            {

                var getUser = GetUserIdFromToken();
                if (getUser.TypeOfResponse != TypeOfResponse.OK)
                {
                    return Ok(getUser);
                }
                int userId = (int)getUser.Data;

                response = await _feedRepository.GetUserFeedsAsync(userId, PageNumber, PageSize);

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("EditFeed")]
        public async Task<ActionResult<Response>> EditFeed(FeedDTOEdit feedDTO)
        {
            Response response = new Response(TypeOfResponse.OK, "Feed edited successfully");
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

                response = await _feedRepository.EditAsync(feedDTO, userId);

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("DeleteFeed")]
        public async Task<ActionResult<Response>> DeleteFeed(FeedDTOId feedDTO)
        {
            Response response = new Response(TypeOfResponse.OK, "Feed deleted successfully");
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

                response = await _feedRepository.DeleteAsync(feedDTO.Id, userId);

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("DeleteTopic")]
        public async Task<ActionResult<Response>> DeleteTopic(TopicDTODelete topicDTO)
        {
            Response response = new Response(TypeOfResponse.OK, "Feed deleted successfully");
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

                response = await _feedRepository.RemoveTopicAsync(topicDTO.Id, userId);

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet("GetNews")]
        public async Task<ActionResult<Response>> GetNews(int feedId)
        {
            Response response = new Response();

            try
            {

                var getUser = GetUserIdFromToken();
                if (getUser.TypeOfResponse != TypeOfResponse.OK)
                {
                    return Ok(getUser);
                }
                int userId = (int)getUser.Data;

                Feed feed = new Feed();
                feed.Id = feedId;
                feed.UserId = userId;

                response = await _feedRepository.CanFetchNewsAsync(feed);

                if (response.TypeOfResponse != TypeOfResponse.OK)
                {
                    return response;
                }

                string topics = (string)response.Data;

                response = await _newsCalls.FetchNewsAsync(topics);

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
