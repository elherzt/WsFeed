﻿using DataLayer.Models;
using DataLayer.Repositories.TopicRepository;
using DataLayer.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.FeedRepository
{
    public interface IFeedRepository
    {
        Task<Response> GetUserFeedAsync(int userId, string feedName);
        Task<Response> GetUserFeedsAsync(int userId);
        Task<Response> CreateAsync(Feed feed);
        Task<Response> GetAsync(int feedId, int userId);
        Task<Response> GetPublicAsync(int userId);
        Task<Response> AddTopicAsync(TopicDTOCreate topic, int userID);
    }

    public class FeedRepository : IFeedRepository
    {
        private int topicLimit = 5;
        private readonly AppDbContext _context;

        public FeedRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response> GetAsync(int feedId, int userId)
        {
            Response response = new Response(TypeOfResponse.OK);
            try
            {
                var feed = await _context.Feeds.Include(x=> x.Topics).FirstOrDefaultAsync(x=> x.Id == feedId && x.UserId == userId);
                if (feed == null)
                {
                    response.TypeOfResponse = TypeOfResponse.NotFound;
                    response.Message = "Feed not found";
                }
                else
                {
                    response.Data = feed;
                }
            }
            catch(Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;

        }


        public async Task<Response> CreateAsync(Feed feed)
        {
            Response response = new Response(TypeOfResponse.OK, "Feed created");
            try
            {
               var existingFeed = await GetUserFeedAsync(feed.UserId, feed.Name);

               if (existingFeed.TypeOfResponse == TypeOfResponse.OK)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Feed already exists";
                    return response;
                }

                await _context.Feeds.AddAsync(feed);
                await _context.SaveChangesAsync();
                
                FeedDTOResponse feedDTOResponse = new FeedDTOResponse();
                feedDTOResponse.Id = feed.Id;
                feedDTOResponse.Name = feed.Name;
                feedDTOResponse.Description = feed.Description;
                feedDTOResponse.IsPrivate = feed.IsPrivate;
                feedDTOResponse.TopicsCount = 0;

                response.Data = feedDTOResponse;

            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response> GetUserFeedAsync(int userId, string feedName)
        {
            Response response = new Response(TypeOfResponse.OK, "Feed found");
            try
            {
                var feed = await _context.Feeds.Include(x=> x.Topics).Where(f => f.UserId == userId && f.Name.ToUpper() == feedName.ToUpper()).FirstOrDefaultAsync();
                if (feed == null)
                {
                    response.TypeOfResponse = TypeOfResponse.NotFound;
                    response.Message = "Feed not found";
                }
                else
                {
                    response.Data = feed;
                }
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response> GetTopicAsync(int feedId, string name)
        {
            Response response = new Response(TypeOfResponse.OK, "Topic found");
            try
            {
                var topic = await _context.Topics.FirstOrDefaultAsync(x => x.FeedId == feedId && x.Name == name);
                if (topic == null)
                {
                    response.TypeOfResponse = TypeOfResponse.NotFound;
                    response.Message = "Topic not found";
                }
                else
                {
                    response.Data = topic;
                }
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }

            public async Task<Response> GetUserFeedsAsync(int userId)
        {
            Response response = new Response(TypeOfResponse.OK, "Feeds found");
            try
            {
                var feeds = await _context.Feeds.Include(x => x.Topics).Where(f => f.UserId == userId).Select(x => new FeedDTOResponse(x)).ToListAsync();
                response.Data = feeds;
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<Response> GetPublicAsync(int userId)
        {
            Response response = new Response(TypeOfResponse.OK, "Feeds found");
            try
            {
                var feeds = await _context.Feeds.Include(x=> x.Topics).Where(f => f.IsPrivate == false && f.UserId != userId).Select(x=> new FeedDTOResponse(x)).ToListAsync();
                response.Data = feeds;
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<Response> AddTopicAsync(TopicDTOCreate topic, int userID)
        {
            Response response = new Response(TypeOfResponse.OK, "Topic created successfully");
            try
            {
                var existingFeed = await GetAsync(topic.FeedId, userID);
                if (existingFeed.TypeOfResponse != TypeOfResponse.OK)
                {
                    return existingFeed;
                }

                var checkLimit = await CheckTopicsLimit(topic.FeedId);
                if (checkLimit.TypeOfResponse != TypeOfResponse.OK)
                {
                    return checkLimit;
                }

                var existingTopic = await GetTopicAsync(topic.FeedId, topic.Name);
                if (existingTopic.TypeOfResponse == TypeOfResponse.OK)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Topic already exists";
                    return response;
                }


                Topic newTopic = new Topic();
                newTopic.Name = topic.Name;
                newTopic.FeedId = topic.FeedId;
                newTopic.Description = topic.Description;


                await _context.Topics.AddAsync(newTopic);
                await _context.SaveChangesAsync();

                response.Data = new TopicDTOResponse(newTopic);
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<Response> CheckTopicsLimit(int feedId)
        {
            Response response = new Response(TypeOfResponse.OK, "Success");
            try
            {
                var feedsCount = await _context.Topics.Where(f => f.FeedId == feedId).CountAsync();
                if (feedsCount >= topicLimit)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Limit reached";
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
