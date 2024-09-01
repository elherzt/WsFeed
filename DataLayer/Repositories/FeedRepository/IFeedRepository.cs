using DataLayer.Models;
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
        Task<Response> GetUserFeedsAsync(int userId, int pageNumber = 1, int pageSize = 10);
        Task<Response> CreateAsync(Feed feed);
        Task<Response> GetAsync(int feedId, int userId);
        Task<Response> GetPublicAsync(int userId, int pageNumber = 1, int pageSize = 10);
        Task<Response> AddTopicAsync(TopicDTOCreate topic, int userID);
        Task<Response> RemoveTopicAsync(int topicID, int userId);
        Task<Response> DeleteAsync(int feedId, int userId);
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

        public async Task<Response> GetUserFeedsAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            Response response = new Response(TypeOfResponse.OK, "Feeds found");
            try
            {
               
                if (pageNumber < 1 || pageSize < 1)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Invalid pagination settings";
                    return response;
                }

                int skip = (pageNumber - 1) * pageSize;
                int totalFeeds = await _context.Feeds.CountAsync(f => f.UserId == userId);
                int totalPages = (int)Math.Ceiling(totalFeeds / (double)pageSize);
                int itemPerPage = pageSize;

                if (skip >= totalFeeds)
                {
                    response.Message = "No records";
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Data = new
                    {
                        TotalRecords = totalFeeds,
                        CurrentPage = pageNumber,
                        TotalPages = totalPages,
                        ItemsPerPage = itemPerPage,
                        Feeds = new List<FeedDTOResponse>()
                    };
                    return response;
                }

                var feeds = await _context.Feeds.Include(x => x.Topics).Where(f => f.UserId == userId)
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(x => new FeedDTOResponse(x)).ToListAsync();

                response.Data = new
                {
                    TotalRecords = totalFeeds,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    ItemsPerPage = itemPerPage,
                    Feeds = feeds
                };
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<Response> GetPublicAsync(int userId, int pageNumber = 1, int pageSize = 10)
        {
            Response response = new Response(TypeOfResponse.OK, "Feeds found");
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Message = "Invalid pagination settings";
                    return response;
                }

                int skip = (pageNumber - 1) * pageSize;
                int totalFeeds = await _context.Feeds.CountAsync(f => f.IsPrivate == false && f.UserId != userId);
                int totalPages = (int)Math.Ceiling(totalFeeds / (double)pageSize);
                int itemPerPage = pageSize;

                if (skip >= totalFeeds)
                {
                    response.Message = "No records";
                    response.TypeOfResponse = TypeOfResponse.FailedResponse;
                    response.Data = new
                    {
                        TotalRecords = totalFeeds,
                        CurrentPage = pageNumber,
                        TotalPages = totalPages,
                        ItemsPerPage = itemPerPage,
                        Feeds = new List<FeedDTOResponse>()
                    };
                    return response;
                }


                var feeds = await _context.Feeds.Include(x=> x.Topics).Where(f => f.IsPrivate == false && f.UserId != userId)
                    .Skip(skip)  
                    .Take(pageSize)
                    .Select(x=> new FeedDTOResponse(x)).ToListAsync();

                response.Data = new 
                {
                    TotalRecords = totalFeeds,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    ItemsPerPage = itemPerPage,
                    Feeds = feeds
                };
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

        public async Task<Response> DeleteAsync(int feedId, int userId)
        {
            Response response = new Response(TypeOfResponse.OK, "Feed deleted");
            try
            {
                var existingFeed = await GetAsync(feedId, userId);
                if (existingFeed.TypeOfResponse != TypeOfResponse.OK)
                {
                    return existingFeed;
                }

                Feed feed = (Feed)existingFeed.Data;
                _context.Feeds.Remove(feed);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.TypeOfResponse = TypeOfResponse.Exception;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<Response> RemoveTopicAsync(int topicID, int userId)
        {
            Response response = new Response(TypeOfResponse.OK, "Topic removed");
            try
            {
                var topic = await _context.Topics.Include(x=> x.Feed).Where(x => x.Id == topicID && x.Feed.UserId == userId).FirstOrDefaultAsync();
                if (topic == null)
                {
                    response.TypeOfResponse = TypeOfResponse.NotFound;
                    response.Message = "Topic not found";
                    return response;
                }

                _context.Topics.Remove(topic);
                _context.SaveChanges();
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
