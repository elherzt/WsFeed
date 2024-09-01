﻿using DataLayer.Models;
using DataLayer.Repositories.FeedRepository;
using DataLayer.Repositories.TopicRepository;
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



    }
}
