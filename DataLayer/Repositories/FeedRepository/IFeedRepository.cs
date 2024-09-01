using DataLayer.Models;
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
        Task<Response> CreateAsync(Feed feed);
    }

    public class FeedRepository : IFeedRepository
    {
        private int feedLimit = 5;
        private readonly AppDbContext _context;

        public FeedRepository(AppDbContext context)
        {
            _context = context;
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
                response.Data = feed;

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
                var feed = await _context.Feeds.Where(f => f.UserId == userId && f.Name == feedName).FirstOrDefaultAsync();
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

        public async Task<Response> GetUserFeedAsync(Feed feed)
        {
            throw new NotImplementedException();
        }

        //public async Task<Response> CheckLimit(int userId)
        //{
        //    Response response = new Response(TypeOfResponse.OK, "Success");
        //    try
        //    {
        //        var feedsCount = await _context.Feeds.Where(f => f.UserId == userId).CountAsync();
        //        if (feedsCount >= feedLimit)
        //        {
        //            response.TypeOfResponse = TypeOfResponse.FailedResponse;
        //            response.Message = "";
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.TypeOfResponse = TypeOfResponse.ErrorException;
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
    }
}
